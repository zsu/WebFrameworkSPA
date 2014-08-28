using App.Common;
using App.Common.Data;
using App.Common.InversionOfControl;
using App.Data;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using BrockAllen.MembershipReboot.Nh;
using BrockAllen.MembershipReboot.Relational;
using BrockAllen.MembershipReboot.WebHost;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Claims;
using System.Linq;
using System.Web;
using WebFramework.Data.Domain;
using Service;
using System.Text;

namespace Web
{
    public class SecurityConfig
    {
        public static MembershipRebootConfiguration<NhUserAccount> Config()
        {
            var settings = SecuritySettings.Instance;
            settings.MultiTenant = false;

            var config = new MembershipRebootConfiguration<NhUserAccount>(settings);
            config.RegisterPasswordValidator(new PasswordValidator());
            config.ConfigurePasswordComplexity(5, 3);

            config.AddCommandHandler(new CustomClaimsMapper());

            var delivery = new SmtpMessageDelivery();

            var appinfo = new AspNetApplicationInformation(Util.ApplicationConfiguration.AppAcronym, Util.ApplicationConfiguration.SupportOrganization,
                "UserAccount/Login",
                "UserAccount/ChangeEmail/Confirm/",
                "UserAccount/Register/Cancel/",
                "UserAccount/PasswordReset/Confirm/");
            var messageTemplateService=IoC.GetService<IMessageTemplateService>();
            var formatter = new CustomEmailMessageFormatter(appinfo,messageTemplateService);

            config.AddEventHandler(new EmailAccountEventsHandler<NhUserAccount>(formatter, delivery));
            config.AddEventHandler(new AuthenticationAuditEventHandler());
            config.AddEventHandler(new NotifyAccountOwnerWhenTooManyFailedLoginAttempts());

            config.AddValidationHandler(new PasswordChanging());
            config.AddEventHandler(new PasswordChanged());

            return config;
        }
    }
    public class PasswordValidator : IValidator<NhUserAccount>
    {
        public ValidationResult Validate(UserAccountService<NhUserAccount> service, NhUserAccount account, string value)
        {
            if (value.Contains("R"))
            {
                return new ValidationResult("You can't use an 'R' in your password (for some reason)");
            }

            return null;
        }
    }


    public class AuthenticationAuditEventHandler :
        IEventHandler<SuccessfulLoginEvent<NhUserAccount>>,
        IEventHandler<FailedLoginEvent<NhUserAccount>>
    {
        public void Handle(SuccessfulLoginEvent<NhUserAccount> evt)
        {
            //var db = IoC.GetService<IRepository<AuthenticationAudit, Guid>>();
            //using (var scope = new UnitOfWorkScope())
            //{
            //    var audit = new AuthenticationAudit
            //    {
            //        UserName = evt.Account.Username,
            //        Date = DateTime.UtcNow,
            //        Activity = "Login Success",
            //        Detail = null,
            //        ClientIP = HttpContext.Current.Request.UserHostAddress,
            //    };
            //    db.Add(audit);
            //    scope.Commit();
            //}
            var audit = new AuthenticationAudit
            {
                Application = Util.ApplicationConfiguration.AppAcronym,
                UserName = evt.Account.Username,
                CreatedDate = DateTime.UtcNow,
                Activity = "LoginSuccess",
                Detail = string.Format("User {0} login successfully.",evt.Account.Username),
                ClientIP = HttpContext.Current.Request.UserHostAddress,
            };
            var authenticationAuditService = IoC.GetService<IAuthenticationAuditService>();
            authenticationAuditService.Add(audit);
        }

        public void Handle(FailedLoginEvent<NhUserAccount> evt)
        {
            //var db = IoC.GetService<IRepository<AuthenticationAudit, Guid>>();
            //using (var scope = new UnitOfWorkScope())
            //{
            //    var audit = new AuthenticationAudit
            //    {
            //        UserName = evt.Account.Username,
            //        Date = DateTime.UtcNow,
            //        Activity = "Login Failure",
            //        Detail = evt.GetType().Name + ", Failed Login Count: " + evt.Account.FailedLoginCount,
            //        ClientIP = HttpContext.Current.Request.UserHostAddress,
            //    };
            //    db.Add(audit);
            //    scope.Commit();
            //}
            var audit = new AuthenticationAudit
            {
                Application=Util.ApplicationConfiguration.AppAcronym,
                UserName = evt.Account.Username,
                CreatedDate = DateTime.UtcNow,
                Activity = "LoginFailed",
                Detail = evt.GetType().Name + ", Failed Login Count: " + evt.Account.FailedLoginCount,
                ClientIP = HttpContext.Current.Request.UserHostAddress,
            };
            var authenticationAuditService = IoC.GetService<IAuthenticationAuditService>();
            authenticationAuditService.Add(audit);

        }
    }

    public class NotifyAccountOwnerWhenTooManyFailedLoginAttempts
        : IEventHandler<TooManyRecentPasswordFailuresEvent<NhUserAccount>>
    {
        public void Handle(TooManyRecentPasswordFailuresEvent<NhUserAccount> evt)
        {
            var smtp = new SmtpMessageDelivery();
            var msg = new Message
            {
                To = evt.Account.Email,
                Subject = "Your Account",
                Body = "It seems someone has tried to login too many times to your account. It's currently locked."
            };
            smtp.Send(msg);
        }
    }

    public class PasswordChanging :
        IEventHandler<PasswordChangedEvent<NhUserAccount>>
    {
        public void Handle(PasswordChangedEvent<NhUserAccount> evt)
        {
            //BrockAllen.MembershipReboot.Nh.Repository.IRepository<BrockAllen.MembershipReboot.Nh.PasswordHistory> userRepository = IoC.GetService<BrockAllen.MembershipReboot.Nh.Repository.IRepository<BrockAllen.MembershipReboot.Nh.PasswordHistory>>();

            var db = IoC.GetService<IRepository<WebFramework.Data.Domain.PasswordHistory, Guid>>();
            var oldEntires =
                db.Query.Where(x => x.User.ID == evt.Account.ID).OrderByDescending(x => x.DateChanged).ToArray();
            for (var i = 0; i < 3 && oldEntires.Length > i; i++)
            {
                var oldHash = oldEntires[i].PasswordHash;
                if (new DefaultCrypto().VerifyHashedPassword(oldHash, evt.NewPassword))
                {
                    throw new ValidationException("New Password must not be same as the past three");
                }
            }
        }
    }

    public class PasswordChanged :
        IEventHandler<AccountCreatedEvent<NhUserAccount>>,
        IEventHandler<PasswordChangedEvent<NhUserAccount>>
    {
        public void Handle(AccountCreatedEvent<NhUserAccount> evt)
        {
            if (evt.InitialPassword != null)
            {
                AddPasswordHistoryEntry(evt.Account, evt.InitialPassword);
            }
            StringBuilder message = new StringBuilder();
            var activityLogService = IoC.GetService<IActivityLogService>();
            message.AppendFormat("User {0} is created.", evt.Account.Username);
            ActivityLog activityItem = new ActivityLog("AddUser", message.ToString());
            activityLogService.Add(activityItem);
        }

        public void Handle(PasswordChangedEvent<NhUserAccount> evt)
        {
            AddPasswordHistoryEntry(evt.Account, evt.NewPassword);
        }

        private static void AddPasswordHistoryEntry(NhUserAccount user, string password)
        {
            var db = IoC.GetService<IRepository<WebFramework.Data.Domain.PasswordHistory, Guid>>();
            using (var scope = new UnitOfWorkScope())
            {
            //App.Common.Data.IRepository<WebFramework.Data.Domain.PasswordHistory,Guid> userRepository = IoC.GetService<App.Common.Data.IRepository<WebFramework.Data.Domain.PasswordHistory,Guid>>();

            var pw = new WebFramework.Data.Domain.PasswordHistory
                {
                    User = user,
                    Username = user.Username,
                    DateChanged = DateTime.UtcNow,
                    PasswordHash = new DefaultCrypto().HashPassword(password, 1000)
                };
            //userRepository.Add(pw);
            db.Add(pw);
            scope.Commit();
            }

        }
    }

    // customize default email messages
    public class CustomEmailMessageFormatter : EmailMessageFormatter<NhUserAccount>
    {
        private IMessageTemplateService _messageTemplateServce;
        public CustomEmailMessageFormatter(ApplicationInformation info, IMessageTemplateService messageTemplateService)
            : base(info)
        {
            _messageTemplateServce = messageTemplateService;
        }

        protected override string GetBody(UserAccountEvent<NhUserAccount> evt, IDictionary<string, string> values)
        {
            //if (evt is EmailVerifiedEvent<NhUserAccount>)
            //{
            //    return "your account was verified with " + this.ApplicationInformation.ApplicationName + ". good for you.";
            //}

            //if (evt is AccountClosedEvent<NhUserAccount>)
            //{
            //    return FormatValue(evt, "your account was closed with {applicationName}. good riddance.", values);
            //}
            return base.GetBody(evt, values);

        }
        protected override string LoadSubjectTemplate(UserAccountEvent<NhUserAccount> evt)
        {
            string templateName = CleanGenericName(evt.GetType());
            MessageTemplate template = _messageTemplateServce.GetByName(templateName);
            if (template == null)
                return base.LoadSubjectTemplate(evt);
            return template.Subject;
        }
        protected override string LoadBodyTemplate(UserAccountEvent<NhUserAccount> evt)
        {
            string templateName = CleanGenericName(evt.GetType());
            MessageTemplate template = _messageTemplateServce.GetByName(templateName);
            if (template == null)
                return base.LoadBodyTemplate(evt);
            return template.Body;
        }
        private string CleanGenericName(Type type)
        {
            var name = type.Name;
            var idx = name.IndexOf('`');
            if (idx > 0)
            {
                name = name.Substring(0, idx);
            }
            return name;
        }
    }

    public class CustomClaimsMapper : ICommandHandler<MapClaimsFromAccount<NhUserAccount>>
    {
        public void Handle(MapClaimsFromAccount<NhUserAccount> cmd)
        {
            cmd.MappedClaims = new System.Security.Claims.Claim[]
            {
                new System.Security.Claims.Claim(ClaimTypes.GivenName, cmd.Account.FirstName??cmd.Account.Username),
                new System.Security.Claims.Claim(ClaimTypes.Surname, cmd.Account.LastName??string.Empty),
            };
        }
    }
}