﻿/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace BrockAllen.MembershipReboot
{
    internal class UserAccountValidation<TAccount>
        where TAccount : UserAccount
    {
        public static readonly IValidator<TAccount> UsernameDoesNotContainAtSign =
            new DelegateValidator<TAccount>((service, account, value) =>
            {
                if (value.Contains("@"))
                {
                    Tracing.Verbose("[UserAccountValidation.UsernameDoesNotContainAtSign] validation failed: {0}, {1}, {2}", account.Tenant, account.Username, value);

                    return new ValidationResult(service.GetValidationMessage("UsernameCannotContainAtSign"));
                }
                return null;
            });

        public static readonly IValidator<TAccount> UsernameOnlyContainsLettersAndDigits =
            new DelegateValidator<TAccount>((service, account, value) =>
            {
                if (!value.All(x => Char.IsLetterOrDigit(x)))
                {
                    Tracing.Verbose("[UserAccountValidation.UsernameOnlyContainsLettersAndDigits] validation failed: {0}, {1}, {2}", account.Tenant, account.Username, value);

                    return new ValidationResult(service.GetValidationMessage("UsernameOnlyContainLettersAndDigits"));
                }
                return null;
            });

        //Added: Zhicheng Su
        public static readonly IValidator<TAccount> UsernameOnlyContainsLettersAndDigitsOrIsEmail =
        new DelegateValidator<TAccount>((service, account, value) =>
        {
            if (!value.All(x => Char.IsLetterOrDigit(x)) && !IsEmail(value))
            {
                Tracing.Verbose("[UserAccountValidation.UsernameOnlyContainsLettersAndDigitsOrIsEmail] validation failed: {0}, {1}, {2}", account.Tenant, account.Username, value);

                return new ValidationResult(service.GetValidationMessage("UsernameOnlyContainLettersAndDigitsOrIsEmail"));
            }
            return null;
        });
        public static readonly IValidator<TAccount> UsernameMustNotAlreadyExist =
            new DelegateValidator<TAccount>((service, account, value) =>
            {
                if (service.UsernameExists(account.Tenant, value))
                {
                    Tracing.Verbose("[UserAccountValidation.EmailMustNotAlreadyExist] validation failed: {0}, {1}, {2}", account.Tenant, account.Username, value);

                    return new ValidationResult(service.GetValidationMessage("UsernameAlreadyInUse"));
                }
                return null;
            });

        public static readonly IValidator<TAccount> EmailRequired =
            new DelegateValidator<TAccount>((service, account, value) =>
            {
                if (service.Configuration.RequireAccountVerification &&
                    String.IsNullOrWhiteSpace(value))
                {
                    Tracing.Verbose("[UserAccountValidation.EmailRequired] validation failed: {0}, {1}", account.Tenant, account.Username);

                    return new ValidationResult(service.GetValidationMessage("EmailRequired"));
                }
                return null;
            });

        public static readonly IValidator<TAccount> EmailIsValidFormat =
            new DelegateValidator<TAccount>((service, account, value) =>
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    EmailAddressAttribute validator = new EmailAddressAttribute();
                    if (!validator.IsValid(value))
                    {
                        Tracing.Verbose("[UserAccountValidation.EmailIsValidFormat] validation failed: {0}, {1}, {2}", account.Tenant, account.Username, value);

                        return new ValidationResult(service.GetValidationMessage("InvalidEmail"));
                    }
                }
                return null;
            });

        public static readonly IValidator<TAccount> EmailIsRequiredIfRequireAccountVerificationEnabled =
            new DelegateValidator<TAccount>((service, account, value) =>
            {
                if (service.Configuration.RequireAccountVerification && String.IsNullOrWhiteSpace(value))
                {
                    return new ValidationResult(service.GetValidationMessage("EmailRequired"));
                }
                return null;
            });

        public static readonly IValidator<TAccount> EmailMustNotAlreadyExist =
            new DelegateValidator<TAccount>((service, account, value) =>
            {
                if (!String.IsNullOrWhiteSpace(value) && service.EmailExistsOtherThan(account, value))
                {
                    Tracing.Verbose("[UserAccountValidation.EmailMustNotAlreadyExist] validation failed: {0}, {1}, {2}", account.Tenant, account.Username, value);

                    return new ValidationResult(service.GetValidationMessage("EmailAlreadyInUse"));
                }
                return null;
            });

        public static readonly IValidator<TAccount> PasswordMustBeDifferentThanCurrent =
            new DelegateValidator<TAccount>((service, account, value) =>
        {
            // Use LastLogin null-check to see if it's a new account
            // we don't want to run this logic if it's a new account
            if (!account.IsNew() && service.VerifyHashedPassword(account, value))
            {
                Tracing.Verbose("[UserAccountValidation.PasswordMustBeDifferentThanCurrent] validation failed: {0}, {1}", account.Tenant, account.Username);

                return new ValidationResult(service.GetValidationMessage("NewPasswordMustBeDifferent"));
            }
            return null;
        });
        private static readonly Regex EmailExpression = new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", RegexOptions.Singleline | RegexOptions.Compiled);
        private static bool IsEmail(string target)
        {
            return !string.IsNullOrEmpty(target) && EmailExpression.IsMatch(target);
        }
    }
}
