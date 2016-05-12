using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using App.Common.InversionOfControl;
using App.Data;

namespace BrockAllen.MembershipReboot.Nh.Service
{
    public class NhUserAccountService<T> : UserAccountService<T> where T : NhUserAccount
    {
        private IUserAccountRepository<T> _userAccountRepository;
        private EventBusUserAccountRepository<T> _eventBusUserRepository;
        public NhUserAccountService(IUserAccountRepository<T> userRepository)
            : this(new MembershipRebootConfiguration<T>(), userRepository)
        {
        }

        public NhUserAccountService(MembershipRebootConfiguration<T> configuration, IUserAccountRepository<T> userRepository)
            : base(configuration, userRepository)
        {
            _userAccountRepository = userRepository;
            var validationEventBus = new EventBus();
            validationEventBus.Add(new UserAccountValidator<T>(this));
            _eventBusUserRepository = new EventBusUserAccountRepository<T>(this, userRepository,
               new AggregateEventBus { validationEventBus, configuration.ValidationBus },
               configuration.EventBus);
        }
        //Changed by: Zhicheng Su
        public virtual T CreateAccount(string username, string password, string email, string firstname, string lastname)
        { return CreateAccount(null, username, password, email, firstname, lastname); }
        //Changed by: Zhicheng Su
        public virtual T CreateAccount(string tenant, string username, string password, string email, string firstname, string lastname)
        {
            if (Configuration.EmailIsUsername)
            {
                Tracing.Verbose("[UserAccountService.CreateAccount] applying email is username");
                username = email;
            }

            if (!Configuration.MultiTenant)
            {
                Tracing.Verbose("[UserAccountService.CreateAccount] applying default tenant");
                tenant = Configuration.DefaultTenant;
            }
            T account = _userAccountRepository.Create(); //base.CreateAccount(tenant, username, password, email);
            Init(account, tenant, username, password, email);
            account.FirstName = firstname;
            account.LastName = lastname;
            ValidateEmail(account, email);
            ValidateUsername(account, username);
            ValidatePassword(account, password);
            _eventBusUserRepository.Add(account);
            return account;
        }
        //Changed by: Zhicheng Su
        public virtual T CreateAccountWithTempPassword(string tenant, string username, string password, string email, string firstname, string lastname, bool isLoginAllowed, bool isAccountVerified)
        {
            if (Configuration.EmailIsUsername)
            {
                Tracing.Verbose("[UserAccountService.CreateAccountWithTempPassword] applying email is username");
                username = email;
            }

            if (!Configuration.MultiTenant)
            {
                Tracing.Verbose("[UserAccountService.CreateAccountWithTempPassword] applying default tenant");
                tenant = Configuration.DefaultTenant;
            }

            Tracing.Information("[UserAccountService.CreateAccountWithTempPassword] called: {0}, {1}, {2}", tenant, username, email);

            var account = _userAccountRepository.Create();
            string key = Init(account, tenant, username, password, email);
            account.IsLoginAllowed = isLoginAllowed;
            account.IsAccountVerified = isAccountVerified;
            account.PasswordChanged = new DateTime(1900, 1, 1);
            account.FirstName = firstname;
            account.LastName = lastname;
            ValidateEmail(account, email);
            ValidateUsername(account, username);
            ValidatePassword(account, password);
            Tracing.Verbose("[UserAccountService.CreateAccountWithTempPassword] success");

            _eventBusUserRepository.Add(account);
            this.AddEvent(new PasswordGeneratedEvent<T> { Account = account, InitialPassword = password, VerificationKey = key });
            return account;
        }

        //Changed by: Zhicheng Su
        protected string Init(T account, string tenant, string username, string password, string email)
        {
            Tracing.Information("[UserAccountService.Init] called");

            if (account == null)
            {
                Tracing.Error("[UserAccountService.Init] failed -- null account");
                throw new ArgumentNullException("account");
            }

            if (String.IsNullOrWhiteSpace(tenant))
            {
                Tracing.Error("[UserAccountService.Init] failed -- no tenant");
                throw new ArgumentNullException("tenant");
            }

            if (String.IsNullOrWhiteSpace(username))
            {
                Tracing.Error("[UserAccountService.Init] failed -- no username");
                throw new ValidationException(GetValidationMessage("UsernameRequired"));
            }

            if (password != null && String.IsNullOrWhiteSpace(password.Trim()))
            {
                Tracing.Error("[UserAccountService.Init] failed -- no password");
                throw new ValidationException(GetValidationMessage("PasswordRequired"));
            }

            if (account.ID != Guid.Empty)
            {
                Tracing.Error("[UserAccountService.Init] failed -- ID already assigned");
                throw new Exception("Can't call Init if UserAccount is already assigned an ID");
            }
            account.ID = Guid.NewGuid();
            account.Tenant = tenant;
            account.Username = username;
            account.Email = email;
            account.Created = UtcNow;
            account.LastUpdated = account.Created;
            account.HashedPassword = password != null ?
                Configuration.Crypto.HashPassword(password, this.Configuration.PasswordHashingIterationCount) : null;
            account.PasswordChanged = password != null ? account.Created : (DateTime?)null;
            account.IsAccountVerified = false;
            account.AccountTwoFactorAuthMode = TwoFactorAuthMode.None;
            account.CurrentTwoFactorAuthStatus = TwoFactorAuthMode.None;

            account.IsLoginAllowed = Configuration.AllowLoginAfterAccountCreation;
            Tracing.Verbose("[UserAccountService.CreateAccount] SecuritySettings.AllowLoginAfterAccountCreation is set to: {0}", account.IsLoginAllowed);

            string key = null;
            if (!String.IsNullOrWhiteSpace(account.Email))
            {
                Tracing.Verbose("[UserAccountService.CreateAccount] Email was provided, so creating email verification request");
                key = SetVerificationKey(account, VerificationKeyPurpose.ChangeEmail, state: account.Email);
            }

            this.AddEvent(new AccountCreatedEvent<T> { Account = account, InitialPassword = password, VerificationKey = key });
            //Changed by: Zhicheng Su
            return key;
        }
        public override void CancelVerification(string key, out bool accountClosed)
        {
            Tracing.Information("[UserAccountService.CancelVerification] called: {0}", key);

            accountClosed = false;

            if (String.IsNullOrWhiteSpace(key))
            {
                Tracing.Error("[UserAccountService.CancelVerification] failed -- key null");
                throw new ValidationException(GetValidationMessage(MembershipRebootConstants.ValidationMessages.InvalidKey));
            }

            var account = this.GetByVerificationKey(key);
            if (account == null)
            {
                Tracing.Error("[UserAccountService.CancelVerification] failed -- account not found from key");
                throw new ValidationException(GetValidationMessage(MembershipRebootConstants.ValidationMessages.InvalidKey));
            }

            if (account.VerificationPurpose == null)
            {
                Tracing.Error("[UserAccountService.CancelVerification] failed -- no purpose");
                throw new ValidationException(GetValidationMessage(MembershipRebootConstants.ValidationMessages.InvalidKey));
            }

            var result = Configuration.Crypto.VerifyHash(key, account.VerificationKey);
            if (!result)
            {
                Tracing.Error("[UserAccountService.CancelVerification] failed -- key verification failed");
                throw new ValidationException(GetValidationMessage(MembershipRebootConstants.ValidationMessages.InvalidKey));
            }

            if (account.VerificationPurpose == VerificationKeyPurpose.ChangeEmail &&
                account.IsNew())
            {
                Tracing.Verbose("[UserAccountService.CancelVerification] account is new (deleting account)");
                App.Common.Data.IRepository<PasswordHistory, Guid> passwordRepository = IoC.GetService<App.Common.Data.IRepository<PasswordHistory, Guid>>();
                List<PasswordHistory> passwordHistories = passwordRepository.Query.Where(x => x.User.ID == account.ID).ToList();
                using (var scope = new UnitOfWorkScope())
                {
                    foreach (PasswordHistory item in passwordHistories)
                    {
                        passwordRepository.Delete(item);
                    }
                    scope.Commit();
                }
                // if last login is null then they've never logged in so we can delete the account
                DeleteAccount(account);
                accountClosed = true;
            }
            else
            {
                Tracing.Verbose("[UserAccountService.CancelVerification] account is not new (canceling clearing verification key)");
                ClearVerificationKey(account);
                UpdateInternal(account);
            }

            Tracing.Verbose("[UserAccountService.CancelVerification] succeeded");
        }
    }
    //Changed by:Zhicheng Su
    public class PasswordGeneratedEvent<T> : UserAccountEvent<T>
    {
        public string InitialPassword { get; set; }
        public string VerificationKey { get; set; }
    }
    class AggregateEventBus : List<IEventBus>, IEventBus
    {
        public void RaiseEvent(IEvent evt)
        {
            foreach (var eb in this)
            {
                eb.RaiseEvent(evt);
            }
        }
    }
    class UserAccountValidator<TAccount> :
    IEventHandler<CertificateAddedEvent<TAccount>>
    where TAccount : UserAccount
    {
        UserAccountService<TAccount> userAccountService;
        public UserAccountValidator(UserAccountService<TAccount> userAccountService)
        {
            if (userAccountService == null) throw new ArgumentNullException("userAccountService");
            this.userAccountService = userAccountService;
        }

        public void Handle(CertificateAddedEvent<TAccount> evt)
        {
            if (evt == null) throw new ArgumentNullException("event");
            if (evt.Account == null) throw new ArgumentNullException("account");
            if (evt.Certificate == null) throw new ArgumentNullException("certificate");

            var account = evt.Account;
            var otherAccount = userAccountService.GetByCertificate(account.Tenant, evt.Certificate.Thumbprint);
            if (otherAccount != null && otherAccount.ID != account.ID)
            {
                Tracing.Verbose("[UserAccountValidation.CertificateThumbprintMustBeUnique] validation failed: {0}, {1}", account.Tenant, account.Username);
                throw new ValidationException(userAccountService.GetValidationMessage("CertificateAlreadyInUse"));
            }
        }
    }
    class EventBus : List<IEventHandler>, IEventBus
    {
        Dictionary<Type, IEnumerable<IEventHandler>> handlerCache = new Dictionary<Type, IEnumerable<IEventHandler>>();
        GenericMethodActionBuilder<IEventHandler, IEvent> actions = new GenericMethodActionBuilder<IEventHandler, IEvent>(typeof(IEventHandler<>), "Handle");

        public void RaiseEvent(IEvent evt)
        {
            var action = GetAction(evt);
            var matchingHandlers = GetHandlers(evt);
            foreach (var handler in matchingHandlers)
            {
                action(handler, evt);
            }
        }

        Action<IEventHandler, IEvent> GetAction(IEvent evt)
        {
            return actions.GetAction(evt);
        }

        private IEnumerable<IEventHandler> GetHandlers(IEvent evt)
        {
            var eventType = evt.GetType();
            if (!handlerCache.ContainsKey(eventType))
            {
                var eventHandlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
                var query =
                    from handler in this
                    where eventHandlerType.IsAssignableFrom(handler.GetType())
                    select handler;
                var handlers = query.ToArray().Cast<IEventHandler>();
                handlerCache.Add(eventType, handlers);
            }
            return handlerCache[eventType];
        }
    }
    class GenericMethodActionBuilder<TargetBase, ParamBase>
    {
        Dictionary<Type, Action<TargetBase, ParamBase>> actionCache = new Dictionary<Type, Action<TargetBase, ParamBase>>();

        Type targetType;
        string method;
        public GenericMethodActionBuilder(Type targetType, string method)
        {
            this.targetType = targetType;
            this.method = method;
        }

        public Action<TargetBase, ParamBase> GetAction(ParamBase paramInstance)
        {
            var paramType = paramInstance.GetType();

            if (!actionCache.ContainsKey(paramType))
            {
                actionCache.Add(paramType, BuildActionForMethod(paramType));
            }

            return actionCache[paramType];
        }

        private Action<TargetBase, ParamBase> BuildActionForMethod(Type paramType)
        {
            var handlerType = targetType.MakeGenericType(paramType);

            var ehParam = Expression.Parameter(typeof(TargetBase));
            var evtParam = Expression.Parameter(typeof(ParamBase));
            var invocationExpression =
                Expression.Lambda(
                    Expression.Block(
                        Expression.Call(
                            Expression.Convert(ehParam, handlerType),
                            handlerType.GetMethod(method),
                            Expression.Convert(evtParam, paramType))),
                    ehParam, evtParam);

            return (Action<TargetBase, ParamBase>)invocationExpression.Compile();
        }
    }
}
