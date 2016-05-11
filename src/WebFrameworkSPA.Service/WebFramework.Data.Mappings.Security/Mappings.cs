using BrockAllen.MembershipReboot.Nh;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
namespace WebFramework.Data.Mappings.Security
{
    public class GroupMap : ClassMapping<NhGroup>
    {
        public GroupMap()
        {
            this.Table("Groups");
            this.Id(x => x.ID, idm => idm.Generator(Generators.Assigned));
            this.Property(
                x => x.Tenant,
                pm =>
                {
                    pm.Length(50);
                    pm.NotNullable(true);
                });
            this.Property(
                x => x.Name,
                pm =>
                {
                    pm.Length(100);
                    pm.NotNullable(true);
                });
            this.Property(x => x.Created);
            this.Property(x => x.LastUpdated);
            this.Version(
                x => x.Version,
                vm =>
                {
                    vm.Generated(VersionGeneration.Never);
                    vm.Type(new Int64Type());
                });

            this.Set(
                x => x.ChildrenCollection,
                spm =>
                {
                    spm.Inverse(true);
                    spm.Cascade(Cascade.All);
                    spm.Key(
                        km =>
                        {
                            km.Column("GroupID");
                            km.ForeignKey("FK_Group_GroupChildren");
                        });
                },
                r => r.OneToMany());

            this.DynamicInsert(true);
            this.DynamicUpdate(true);
        }
    }

    public class GroupChildMap : ClassMapping<NhGroupChild>
    {
        public GroupChildMap()
        {
            this.Table("GroupChildren");
            this.ComposedId(
                pm =>
                {
                    pm.ManyToOne(
                            x => x.Group,
                            mm =>
                            {
                                mm.Column("GroupID");
                                mm.ForeignKey("FK_Group_GroupChildren");
                                mm.NotNullable(true);
                            });
                    pm.Property(x => x.ChildGroupID);
                });
            this.Version(
                x => x.Version,
                vm =>
                {
                    vm.Generated(VersionGeneration.Never);
                    vm.Type(new Int64Type());
                });

            this.DynamicInsert(true);
            this.DynamicUpdate(true);
        }
    }

    public class LinkedAccountMap : ClassMapping<NhLinkedAccount>
    {
        public LinkedAccountMap()
        {
            this.Table("LinkedAccounts");
            this.ComposedId(
                pm =>
                {
                    pm.ManyToOne(
                            x => x.Account,
                            mm =>
                            {
                                mm.Column("UserAccountID");
                                mm.ForeignKey("FK_UserAccount_UserClaims");
                                mm.NotNullable(true);
                            });
                    pm.Property(x => x.ProviderName, m => m.Length(30));
                    pm.Property(x => x.ProviderAccountID, m => m.Length(100));
                });
            this.Property(x => x.LastLogin);
            this.Version(
                x => x.Version,
                vm =>
                {
                    vm.Generated(VersionGeneration.Never);
                    vm.Type(new Int64Type());
                });

            this.DynamicInsert(true);
            this.DynamicUpdate(true);
        }
    }

    public class LinkedAccountClaimMap : ClassMapping<NhLinkedAccountClaim>
    {
        public LinkedAccountClaimMap()
        {
            this.Table("LinkedAccountClaims");
            this.ComposedId(
                pm =>
                {
                    pm.ManyToOne(
                            x => x.Account,
                            mm =>
                            {
                                mm.Column("UserAccountID");
                                mm.ForeignKey("FK_UserAccount_UserClaims");
                                mm.NotNullable(true);
                            });
                    pm.Property(x => x.ProviderName, m => m.Length(30));
                    pm.Property(x => x.ProviderAccountID, m => m.Length(100));
                    pm.Property(x => x.Type, m => m.Length(150));
                    pm.Property(x => x.Value, m => m.Length(150));
                });
            this.Version(
                x => x.Version,
                vm =>
                {
                    vm.Generated(VersionGeneration.Never);
                    vm.Type(new Int64Type());
                });

            this.DynamicInsert(true);
            this.DynamicUpdate(true);
        }
    }

    public class PasswordResetSecretMap : ClassMapping<NhPasswordResetSecret>
    {
        public PasswordResetSecretMap()
        {
            this.Table("PasswordResetSecrets");
            this.ComposedId(
                pm =>
                {
                    pm.ManyToOne(
                        x => x.Account,
                        mm =>
                        {
                            mm.Column("UserAccountID");
                            mm.ForeignKey("FK_UserAccount_UserClaims");
                            mm.NotNullable(true);
                        });
                    pm.Property(x => x.PasswordResetSecretID);
                });
            this.Property(
                x => x.Question,
                pm =>
                {
                    pm.NotNullable(true);
                    pm.Length(150);
                });
            this.Property(
                x => x.Answer,
                pm =>
                {
                    pm.NotNullable(true);
                    pm.Length(150);
                });
            this.Version(
                x => x.Version,
                vm =>
                {
                    vm.Generated(VersionGeneration.Never);
                    vm.Type(new Int64Type());
                });

            this.DynamicInsert(true);
            this.DynamicUpdate(true);
        }
    }

    public class TwoFactorAuthTokenMap : ClassMapping<NhTwoFactorAuthToken>
    {
        public TwoFactorAuthTokenMap()
        {
            this.Table("TwoFactorAuthTokens");
            this.ComposedId(
                pm =>
                {
                    pm.ManyToOne(
                        x => x.Account,
                        mm =>
                        {
                            mm.Column("UserAccountID");
                            mm.ForeignKey("FK_UserAccount_UserClaims");
                            mm.NotNullable(true);
                        });
                    pm.Property(x => x.Token, p => p.Length(100));
                });
            this.Property(x => x.Issued);
            this.Version(
                x => x.Version,
                vm =>
                {
                    vm.Generated(VersionGeneration.Never);
                    vm.Type(new Int64Type());
                });

            this.DynamicInsert(true);
            this.DynamicUpdate(true);
        }
    }

    public class UserAccountMap : ClassMapping<NhUserAccount>
    {
        public UserAccountMap()
        {
            this.Table("UserAccounts");
            this.Id(x => x.ID, idm => idm.Generator(Generators.Assigned));
            this.Property(
                x => x.Tenant,
                pm =>
                {
                    pm.NotNullable(true);
                    pm.Length(50);
                });
            this.Property(
                x => x.Username,
                pm =>
                {
                    pm.NotNullable(true);
                    pm.Length(100);
                });
            this.Property(x => x.Created);
            this.Property(x => x.LastUpdated);
            this.Property(x => x.IsAccountClosed);
            this.Property(x => x.AccountClosed);
            this.Property(x => x.IsLoginAllowed);
            this.Property(x => x.LastLogin);
            this.Property(x => x.LastFailedLogin);
            this.Property(x => x.FailedLoginCount);
            this.Property(x => x.PasswordChanged);
            this.Property(x => x.RequiresPasswordReset);
            this.Property(x => x.Email, pm => pm.Length(100));
            //Changed by: Zhicheng Su
            this.Property(x => x.FirstName, pm => { pm.NotNullable(true); pm.Length(100); });
            this.Property(x => x.LastName, pm => pm.Length(100));
            this.Property(x => x.IsAccountVerified);
            this.Property(x => x.LastFailedPasswordReset);
            this.Property(x => x.FailedPasswordResetCount);
            this.Property(x => x.MobileCode, pm => pm.Length(100));
            this.Property(x => x.MobileCodeSent);
            this.Property(x => x.MobilePhoneNumber, pm => pm.Length(20));
            this.Property(x => x.MobilePhoneNumberChanged);
            this.Property(x => x.AccountTwoFactorAuthMode);
            this.Property(x => x.CurrentTwoFactorAuthStatus);
            this.Property(x => x.VerificationKey, pm => pm.Length(100));
            this.Property(x => x.VerificationPurpose);
            this.Property(x => x.VerificationKeySent);
            this.Property(x => x.VerificationStorage, pm => pm.Length(100));
            this.Property(x => x.HashedPassword, pm => pm.Length(100));
            this.Version(
                x => x.Version,
                vm =>
                {
                    vm.Generated(VersionGeneration.Never);
                    vm.Type(new Int64Type());
                });

            this.Set(
                x => x.ClaimsCollection,
                spm =>
                {
                    spm.Inverse(true);
                    spm.Cascade(Cascade.All);
                    spm.Key(
                        km =>
                        {
                            km.Column("UserAccountID");
                            km.ForeignKey("FK_UserAccount_UserClaims");
                        });
                },
                r => r.OneToMany());
            this.Set(
                x => x.LinkedAccountsCollection,
                spm =>
                {
                    spm.Inverse(true);
                    spm.Cascade(Cascade.All);
                    spm.Key(
                        km =>
                        {
                            km.Column("UserAccountID");
                            km.ForeignKey("FK_UserAccount_LinkedAccounts");
                        });
                },
                r => r.OneToMany());
            this.Set(
                x => x.LinkedAccountClaimsCollection,
                spm =>
                {
                    spm.Inverse(true);
                    spm.Cascade(Cascade.All);
                    spm.Key(
                        km =>
                        {
                            km.Column("UserAccountID");
                            km.ForeignKey("FK_UserAccount_LinkedClaims");
                        });
                },
                r => r.OneToMany());
            this.Set(
                x => x.CertificatesCollection,
                spm =>
                {
                    spm.Inverse(true);
                    spm.Cascade(Cascade.All);
                    spm.Key(
                        km =>
                        {
                            km.Column("UserAccountID");
                            km.ForeignKey("FK_UserAccount_Certificates");
                        });
                },
                r => r.OneToMany());
            this.Set(
                x => x.TwoFactorAuthTokensCollection,
                spm =>
                {
                    spm.Inverse(true);
                    spm.Cascade(Cascade.All);
                    spm.Key(
                        km =>
                        {
                            km.Column("UserAccountID");
                            km.ForeignKey("FK_UserAccount_TFATokens");
                        });
                },
                r => r.OneToMany());
            this.Set(
                x => x.PasswordResetSecretsCollection,
                spm =>
                {
                    spm.Inverse(true);
                    spm.Cascade(Cascade.All);
                    spm.Key(
                        km =>
                        {
                            km.Column("UserAccountID");
                            km.ForeignKey("FK_UserAccount_PWDSecrets");
                        });
                },
                r => r.OneToMany());
            //Changed by: Zhicheng Su
            //this.Set(
            //x => x.Roles,
            //spm =>
            //{
            //    spm.Inverse(true);
            //    spm.Cascade(Cascade.All);
            //    spm.Key(
            //        km =>
            //        {
            //            km.Column("UserAccountID");
            //            km.ForeignKey("FK_UserAccount_Roles");
            //        });
            //},
            //r => r.OneToMany());
            this.Set(
              x => x.Roles,
              map =>
              {
                  map.Table("UserRoles");
                  map.Key(key =>
                  { key.Column("UserAccountId");
                  key.ForeignKey("FK_UserRoles_UserAccount");
                  });
                  map.Cascade(Cascade.None);
              },
              map => map.ManyToMany(c => {c.Column("RoleId");c.ForeignKey("FK_UserRoles_Roles");})
              );
            //Set(x => x.PasswordHistories, colmap => { colmap.Key(x => x.Column("UserId")); colmap.Inverse(true); }, map => { map.OneToMany(); });

            this.DynamicInsert(true);
            this.DynamicUpdate(true);
        }
    }

    public class UserCertificateMap : ClassMapping<NhUserCertificate>
    {
        public UserCertificateMap()
        {
            this.Table("UserCertificates");
            this.ComposedId(
                pm =>
                {
                    pm.ManyToOne(
                        x => x.Account,
                        mm =>
                        {
                            mm.Column("UserAccountID");
                            mm.ForeignKey("FK_UserAccount_Certificates");
                            mm.NotNullable(true);
                        });
                    pm.Property(x => x.Thumbprint, p => p.Length(150));
                });
            this.Property(x => x.Subject, pm => pm.Length(250));
            this.Version(
                x => x.Version,
                vm =>
                {
                    vm.Generated(VersionGeneration.Never);
                    vm.Type(new Int64Type());
                });

            this.DynamicInsert(true);
            this.DynamicUpdate(true);
        }
    }

    public class UserClaimMap : ClassMapping<NhUserClaim>
    {
        public UserClaimMap()
        {
            this.Table("UserClaims");
            this.ComposedId(
                pm =>
                {
                    pm.ManyToOne(
                        x => x.Account,
                        mm =>
                        {
                            mm.Column("UserAccountID");
                            mm.ForeignKey("FK_UserAccount_UserClaims");
                            mm.NotNullable(true);
                        });
                    pm.Property(x => x.Type, p => p.Length(150));
                    pm.Property(x => x.Value, p => p.Length(150));
                });
            this.Version(
                x => x.Version,
                vm =>
                {
                    vm.Generated(VersionGeneration.Never);
                    vm.Type(new Int64Type());
                });

            this.DynamicInsert(true);
            this.DynamicUpdate(true);
        }
    }
}