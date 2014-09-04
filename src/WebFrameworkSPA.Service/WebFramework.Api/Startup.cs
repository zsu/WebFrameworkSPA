using App.Common.InversionOfControl;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Nh;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Web;

[assembly: OwinStartupAttribute(typeof(Web.Startup))]
namespace Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            SecurityConfig.Config();
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
}