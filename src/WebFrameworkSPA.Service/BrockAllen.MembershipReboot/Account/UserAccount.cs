/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrockAllen.MembershipReboot
{
    //Changed by: Zhicheng Su
    public abstract class UserAccount
    {
        public virtual Guid ID { get; set; }

        [StringLength(50)]
        [Required]
        public virtual string Tenant { get; set; }
        [StringLength(100)]
        [Required]
        public virtual string Username { get; set; }

        public virtual DateTime Created { get; set; }
        public virtual DateTime LastUpdated { get; set; }
        public virtual bool IsAccountClosed { get; set; }
        public virtual DateTime? AccountClosed { get; set; }

        public virtual bool IsLoginAllowed { get; set; }
        public virtual DateTime? LastLogin { get; set; }
        public virtual DateTime? LastFailedLogin { get; set; }
        public virtual int FailedLoginCount { get; set; }

        public virtual DateTime? PasswordChanged { get; set; }
        public virtual bool RequiresPasswordReset { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public virtual string Email { get; set; }
        public virtual bool IsAccountVerified { get; set; }

        public virtual DateTime? LastFailedPasswordReset { get; set; }
        public virtual int FailedPasswordResetCount { get; set; }

        [StringLength(100)]
        public virtual string MobileCode { get; set; }
        public virtual DateTime? MobileCodeSent { get; set; }
        [StringLength(20)]
        public virtual string MobilePhoneNumber { get; set; }
        public virtual DateTime? MobilePhoneNumberChanged { get; set; }

        public virtual TwoFactorAuthMode AccountTwoFactorAuthMode { get; set; }
        public virtual TwoFactorAuthMode CurrentTwoFactorAuthStatus { get; set; }

        [StringLength(100)]
        public virtual string VerificationKey { get; set; }
        public virtual VerificationKeyPurpose? VerificationPurpose { get; set; }
        public virtual DateTime? VerificationKeySent { get; set; }
        [StringLength(100)]
        public virtual string VerificationStorage { get; set; }

        [StringLength(200)]
        public virtual string HashedPassword { get; set; }
        //Added by: Zhicheng Su
        [StringLength(100)]
        [Required]
        public virtual string FirstName { get; set; }
        //Added by: Zhicheng Su
        [StringLength(100)]
        public virtual string LastName { get; set; }

        public abstract IEnumerable<UserClaim> Claims { get; }
        protected internal abstract void AddClaim(UserClaim item);
        protected internal abstract void RemoveClaim(UserClaim item);

        public abstract IEnumerable<LinkedAccount> LinkedAccounts { get; }
        protected internal abstract void AddLinkedAccount(LinkedAccount item);
        protected internal abstract void RemoveLinkedAccount(LinkedAccount item);
        
        public abstract IEnumerable<LinkedAccountClaim> LinkedAccountClaims { get; }
        protected internal abstract void AddLinkedAccountClaim(LinkedAccountClaim item);
        protected internal abstract void RemoveLinkedAccountClaim(LinkedAccountClaim item);

        public abstract IEnumerable<UserCertificate> Certificates { get; }
        protected internal abstract void AddCertificate(UserCertificate item);
        protected internal abstract void RemoveCertificate(UserCertificate item);

        public abstract IEnumerable<TwoFactorAuthToken> TwoFactorAuthTokens { get; }
        protected internal abstract void AddTwoFactorAuthToken(TwoFactorAuthToken item);
        protected internal abstract void RemoveTwoFactorAuthToken(TwoFactorAuthToken item);

        public abstract IEnumerable<PasswordResetSecret> PasswordResetSecrets { get; }
        protected internal abstract void AddPasswordResetSecret(PasswordResetSecret item);
        protected internal abstract void RemovePasswordResetSecret(PasswordResetSecret item);
    }
}
