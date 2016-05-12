using App.Common;
using App.Common.InversionOfControl;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Nh;
using BrockAllen.MembershipReboot.Nh.Service;
using BrockAllen.MembershipReboot.Owin;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

[assembly: OwinStartupAttribute(typeof(Web.Startup))]
namespace Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use((context, next) =>
            {
                //IoC.RegisterInstance<IOwinContext>(context,LifetimeType.PerRequest);
                FirstRequestInitialization.Initialize();
                return next();
            });
            ConfigureMembershipReboot(app);
            var oauthServerConfig = new Microsoft.Owin.Security.OAuth.OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = false,
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(2),
                Provider = new AuthorizationServerProvider(),
                TokenEndpointPath = new PathString("/token")
            };
            app.UseOAuthAuthorizationServer(oauthServerConfig);

            var oauthConfig = new Microsoft.Owin.Security.OAuth.OAuthBearerAuthenticationOptions
            {
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active
                //AuthenticationType = "Bearer"
            };
            app.UseOAuthBearerAuthentication(oauthConfig);
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(1);
            GlobalHost.Configuration.LongPollDelay = TimeSpan.FromMilliseconds(5000);
            app.MapSignalR();
        }
        private static void ConfigureMembershipReboot(IAppBuilder app)
        {
            var cookieOptions = new CookieAuthenticationOptions
            {
                AuthenticationType = MembershipRebootOwinConstants.AuthenticationType,//DefaultAuthenticationTypes.ApplicationCookie,
                ExpireTimeSpan = TimeSpan.FromMinutes(string.IsNullOrEmpty(ConfigurationManager.AppSettings["AuthCookieExpireTimeSpanInMinutes"]) ? 30 : Int32.Parse(ConfigurationManager.AppSettings["AuthCookieExpireTimeSpanInMinutes"])),
                LoginPath = new PathString("/Login")
            };
            RegisterServices(app, cookieOptions.AuthenticationType);
            app.UseMembershipReboot(cookieOptions);
        }
        private static void RegisterServices(IAppBuilder app, string authType)
        {
            var container = IoC.GetContainer<IWindsorContainer>();
            container.Register(Component.For<IOwinContext>().UsingFactoryMethod(() => HttpContext.Current.GetOwinContext()).LifeStyle.Transient);
            var appinfo = new OwinApplicationInformation(app, Util.ApplicationConfiguration.AppAcronym, Util.ApplicationConfiguration.SupportOrganization,
            "UserAccount/Login",
            "UserAccount/ChangeEmail/Confirm/",
            "UserAccount/Register/Cancel/",
            "UserAccount/PasswordReset/Confirm/");
            container.Register(Component.For<ApplicationInformation>().Instance(appinfo).LifeStyle.Singleton);
            container.Register(Component.For<AuthenticationService<NhUserAccount>>().UsingFactoryMethod(ctx =>
            {
                return new OwinAuthenticationService<NhUserAccount>(authType, IoC.GetService<NhUserAccountService<NhUserAccount>>(), IoC.GetService<IOwinContext>().Environment, new Web.Infrastructure.RoleClaimsAuthenticationManager());
            }).LifestylePerWebRequest());


            container.Register(Component.For<UserAccountService<NhUserAccount>>().OnCreate(ctx =>
            {
                var debugging = false;
#if DEBUG
                debugging = true;
#endif
                ctx.ConfigureTwoFactorAuthenticationCookies(IoC.GetService<IOwinContext>().Environment, debugging);
            }).LifestylePerWebRequest());
            container.Register(Component.For<NhUserAccountService<NhUserAccount>>().OnCreate(ctx =>
            {
                var debugging = false;
#if DEBUG
                debugging = true;
#endif
                ctx.ConfigureTwoFactorAuthenticationCookies(IoC.GetService<IOwinContext>().Environment, debugging);
            }).LifestylePerWebRequest());

            var config = SecurityConfig.Config(app);//Depends on IMessageTemplateService
            container.Register(Component.For<MembershipRebootConfiguration<NhUserAccount>>().Instance(config));
        }

    }


    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override  async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //string cid, csecret;
            //if (context.TryGetFormCredentials(out cid, out csecret))
            //{
            //    var svc = IoC.GetService<UserAccountService<NhUserAccount>>();
            //    if (svc.Authenticate(cid, csecret))
            //    {
            //        context.Validated();
            //    }
            //}
            context.Validated();
            await Task.FromResult(0);
        }

        //public override async Task ValidateTokenRequest(OAuthValidateTokenRequestContext context)
        //{
        //    if (context.TokenRequest.IsResourceOwnerPasswordCredentialsGrantType)
        //    {
        //        var svc = IoC.GetService<UserAccountService<NhUserAccount>>();
        //        var client = svc.GetByUsername(context.ClientContext.ClientId);
        //        var scopes = context.TokenRequest.ResourceOwnerPasswordCredentialsGrant.Scope;
        //        if (scopes.All(scope=>client.HasClaim("scope", scope)))
        //        {
        //            var uid = context.TokenRequest.ResourceOwnerPasswordCredentialsGrant.UserName;
        //            var pwd = context.TokenRequest.ResourceOwnerPasswordCredentialsGrant.Password;
        //            if (svc.Authenticate(uid, pwd))
        //            {
        //                context.Validated();
        //            }
        //        }
        //    }
        //    await Task.FromResult(0);
        //}

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var svc = IoC.GetService<UserAccountService<NhUserAccount>>();
            if (!svc.Authenticate(context.UserName, context.Password))
            {
                context.Rejected();
                await Task.FromResult(0);
                return;
            }
            var user = svc.GetByUsername(context.UserName);
            if (user.RequiresPasswordReset || svc.IsPasswordExpired(user))
            {
                context.SetError("password_expired", "Password expired.");
                return;
            } 
            var claims = user.GetAllClaims();
            var id = new System.Security.Claims.ClaimsIdentity(claims, context.Options.AuthenticationType);
            IRoleService roleService = IoC.GetService<IRoleService>();
            List<Role> roles = roleService.GetRolesForUser(user.ID);
            foreach (var role in roles)
            {
                id.AddClaim(new Claim(ClaimTypes.Role, role.Name));
                if (role.Permissions != null && role.Permissions.Count > 0)
                    foreach (var permission in role.Permissions)
                        id.AddClaim(new Claim(ClaimTypes.Role, permission.Name));
            }
            context.Validated(id);
            //context.Request.Context.Authentication.SignIn(id);
            await Task.FromResult(0);
            //return base.GrantResourceOwnerCredentials(context);
        }
    }
    class FirstRequestInitialization
    {
        private static bool _initialized = false;
        private static Object _syncRoot = new Object();
        // Initialize only on the first request

        public static void Initialize()
        {
            if (_initialized)
            {
                return;
            }
            lock (_syncRoot)
            {
                if (_initialized)
                {
                    return;
                }
                // Perform first-request initialization here ...
                SeedDatabase();
                _initialized = true;
            }
        }
        private static void SeedDatabase()
        {
            string userEmail = ConfigurationManager.AppSettings[Constants.ADMIN_EMAIL];
            if (!string.IsNullOrWhiteSpace(userEmail))
            {
                IUserService userService = IoC.GetService<IUserService>();
                string username = userEmail.Substring(0, userEmail.IndexOf('@') > 0 ? userEmail.IndexOf('@') : userEmail.Length);
                //string username = userEmail;
                var account = userService.FindBy(x => x.Username == username || x.Email == userEmail);
                if (account == null)
                {
                    NhUserAccount user = new NhUserAccount() { Username = username, Email = userEmail, HashedPassword = "Abc123$", FirstName = "Admin", IsLoginAllowed = true, IsAccountVerified = true };
                    account = userService.CreateAccountWithTempPassword(user);
                }

                IRoleService roleServie = IoC.GetService<IRoleService>();
                Role adminRole = new Role { Name = Constants.ROLE_ADMIN, Description = "System Administrator" };
                if (!roleServie.RoleExists(adminRole.Name))
                {
                    roleServie.CreateRole(adminRole);
                }
                if (!roleServie.IsUserInRole(account.ID, Constants.ROLE_ADMIN))
                    roleServie.AddUsersToRoles(new List<Guid>() { account.ID }, new List<string>() { adminRole.Name });
            }
        }
    }
}