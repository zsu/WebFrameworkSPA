using App.Common;
using App.Common.Data;
using App.Common.InversionOfControl;
using App.Data;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Nh;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Claims;
using System.Linq;
using System.Web;
using WebFramework.Data.Domain;
using Service;
using System.Text;
using BrockAllen.MembershipReboot.Nh.Service;
using Owin;

namespace Web
{
    public class SecurityConfig
    {
        public static MembershipRebootConfiguration<NhUserAccount> Config(IAppBuilder app)
        {
            var settings = SecuritySettings.Instance;
            settings.MultiTenant = false;

            var config = new MembershipRebootConfiguration<NhUserAccount>(settings);
            //config.RegisterPasswordValidator(new PasswordValidator());
            config.ConfigurePasswordComplexity(7, 4);

            config.AddCommandHandler(new CustomClaimsMapper<NhUserAccount>());

            var delivery = new SmtpMessageDelivery();
            var appinfo = IoC.GetService<ApplicationInformation>();
            var messageTemplateService = IoC.GetService<IMessageTemplateService>();
            var formatter = new CustomEmailMessageFormatter<NhUserAccount>(appinfo, messageTemplateService);

            config.AddEventHandler(new EmailAccountEventsHandler<NhUserAccount>(formatter, delivery));
            config.AddEventHandler(new AuthenticationAuditEventHandler<NhUserAccount>());
            config.AddEventHandler(new NotifyAccountOwnerWhenTooManyFailedLoginAttempts<NhUserAccount>());

            config.AddValidationHandler(new PasswordChanging<NhUserAccount>());
            config.AddEventHandler(new PasswordChanged<NhUserAccount>());
            config.AddEventHandler(new PasswordGeneratedEventHandler<NhUserAccount>());

            return config;
        }
    }
    public class PasswordGeneratedEventHandler<T> : IEventHandler<PasswordGeneratedEvent<T>> where T : UserAccount
    {
        private IMessageTemplateService messageTemplateServce = IoC.GetService<IMessageTemplateService>();
        public void Handle(PasswordGeneratedEvent<T> evt)
        {
            StringBuilder body = new StringBuilder();
            body.AppendLine("Your temporary password for {applicationName} is: {InitialPassword}");
            body.AppendLine();
            body.AppendLine("Please click here to confirm your email address:");
            body.AppendLine();
            body.AppendLine("{confirmChangeEmailUrl}");
            body.AppendLine();
            body.AppendLine("If this was in error or not requested then click to cancel the request:");
            body.AppendLine();
            body.AppendLine("{cancelVerificationUrl}");
            body.AppendLine();
            body.AppendLine("Thanks!");
            body.AppendLine();
            body.AppendLine("{emailSignature}");
            var appinfo = IoC.GetService<ApplicationInformation>();
            string msgBody = body.ToString(), msgSubject = "[{applicationName}] Temporary Password";

            string templateName = CleanGenericName(evt.GetType());
            MessageTemplate template = messageTemplateServce.GetByName(templateName);
            if (template != null)
            {
                msgBody = template.Body;
                msgSubject = template.Subject;
            }
            var tokenizer = new EmailMessageFormatter<T>.Tokenizer();
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("InitialPassword", evt.InitialPassword);
            parameters.Add("VerificationKey", evt.VerificationKey);
            msgBody = tokenizer.Tokenize(evt, appinfo, msgBody, parameters);
            msgSubject = tokenizer.Tokenize(evt, appinfo, msgSubject, parameters);
            var smtp = new SmtpMessageDelivery();
            var msg = new Message
            {
                To = evt.Account.Email,
                Subject = msgSubject,
                Body = msgBody
            };
            smtp.Send(msg);
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
    //public class PasswordValidator : IValidator<NhUserAccount>
    //{
    //    public ValidationResult Validate(UserAccountService<NhUserAccount> service, NhUserAccount account, string value)
    //    {
    //        if (value.Contains("R"))
    //        {
    //            return new ValidationResult("You can't use an 'R' in your password (for some reason)");
    //        }

    //        return null;
    //    }
    //}

    public class AuthenticationAuditEventHandler<T> :
        IEventHandler<SuccessfulLoginEvent<T>>,
        IEventHandler<FailedLoginEvent<T>> where T : UserAccount
    {
        public void Handle(SuccessfulLoginEvent<T> evt)
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
                Detail = string.Format("User {0} login successfully.", evt.Account.Username),
                ClientIP = HttpContext.Current.Request.UserHostAddress,
            };
            var authenticationAuditService = IoC.GetService<IAuthenticationAuditService>();
            authenticationAuditService.Add(audit);
        }

        public void Handle(FailedLoginEvent<T> evt)
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
                Application = Util.ApplicationConfiguration.AppAcronym,
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

    public class NotifyAccountOwnerWhenTooManyFailedLoginAttempts<T>
        : IEventHandler<TooManyRecentPasswordFailuresEvent<T>> where T : UserAccount
    {
        public void Handle(TooManyRecentPasswordFailuresEvent<T> evt)
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

    public class PasswordChanging<T> :
        IEventHandler<PasswordChangedEvent<T>> where T : UserAccount
    {
        public void Handle(PasswordChangedEvent<T> evt)
        {
            //BrockAllen.MembershipReboot.Nh.Repository.IRepository<BrockAllen.MembershipReboot.Nh.PasswordHistory> userRepository = IoC.GetService<BrockAllen.MembershipReboot.Nh.Repository.IRepository<BrockAllen.MembershipReboot.Nh.PasswordHistory>>();

            var db = IoC.GetService<IRepository<PasswordHistory, Guid>>();
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

    public class PasswordChanged<T> :
        IEventHandler<AccountCreatedEvent<T>>,
        IEventHandler<PasswordChangedEvent<T>> where T : NhUserAccount
    {
        public void Handle(AccountCreatedEvent<T> evt)
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

        public void Handle(PasswordChangedEvent<T> evt)
        {
            AddPasswordHistoryEntry(evt.Account, evt.NewPassword);
        }

        private static void AddPasswordHistoryEntry(T user, string password)
        {
            var db = IoC.GetService<IRepository<PasswordHistory, Guid>>();
            using (var scope = new UnitOfWorkScope())
            {
                //App.Common.Data.IRepository<WebFramework.Data.Domain.PasswordHistory,Guid> userRepository = IoC.GetService<App.Common.Data.IRepository<WebFramework.Data.Domain.PasswordHistory,Guid>>();

                var pw = new PasswordHistory
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
    public class CustomEmailMessageFormatter<T> : EmailMessageFormatter<T> where T : UserAccount
    {
        private IMessageTemplateService _messageTemplateServce;
        public CustomEmailMessageFormatter(ApplicationInformation info, IMessageTemplateService messageTemplateService)
            : base(info)
        {
            _messageTemplateServce = messageTemplateService;
        }

        protected override string GetBody(UserAccountEvent<T> evt, IDictionary<string, string> values)
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
        protected override string LoadSubjectTemplate(UserAccountEvent<T> evt)
        {
            string templateName = CleanGenericName(evt.GetType());
            MessageTemplate template = _messageTemplateServce.GetByName(templateName);
            if (template == null)
                return base.LoadSubjectTemplate(evt);
            return template.Subject;
        }
        protected override string LoadBodyTemplate(UserAccountEvent<T> evt)
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

    public class CustomClaimsMapper<T> : ICommandHandler<MapClaimsFromAccount<T>> where T : NhUserAccount
    {
        public void Handle(MapClaimsFromAccount<T> cmd)
        {
            cmd.MappedClaims = new System.Security.Claims.Claim[]
            {
                new System.Security.Claims.Claim(ClaimTypes.GivenName, cmd.Account.FirstName??cmd.Account.Username),
                new System.Security.Claims.Claim(ClaimTypes.Surname, cmd.Account.LastName??string.Empty),
            };
        }
    }
}