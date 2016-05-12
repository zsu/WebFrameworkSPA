using App.Common;
using App.Infrastructure.NHibernate;
using NHibernate.Cfg;
using NHibernate.Bytecode;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using System.Reflection;
using NHibernate.Cfg.MappingSchema;
using WebFramework.Data.Mappings.Security;
using WebFramework.Data.Mappings.Log;
using System.IO;
using WebFramework.Data.Mappings;


namespace Web
{
    public class NHibernateConfig
    {
        private static object _synRoot1 = new object(), _synRoot2 = new object(), _synRoot3 = new object();
        private static ISessionFactory _securityDomainFactory, _logDomainFactory,_appDomainFactory;
        public static ISessionFactory SecurityDomainFactory()
        {
            if (_securityDomainFactory == null)
            {
                lock (_synRoot1)
                {
                    if (_securityDomainFactory == null)
                    {
                        //var createSchema = false;
                        var configuration = new Configuration()
                            .DataBaseIntegration(d =>
                            {
                                d.ConnectionStringName = Constants.SECURITY_DB;
                                d.Dialect<MsSql2012Dialect>();
                                d.SchemaAction = SchemaAutoAction.Validate;
                            })
                            .Proxy(p => p.ProxyFactoryFactory<DefaultProxyFactoryFactory>())
                            .CurrentSessionContext<LazySessionContext>()
                            .SetProperty(NHibernate.Cfg.Environment.Hbm2ddlKeyWords, "none");
                            //.SetProperty(NHibernate.Cfg.Environment.Hbm2ddlAuto, (createSchema == true) ? SchemaAutoAction.Update.ToString() : SchemaAutoAction.Validate.ToString());
                        configuration.AddMapping(GetSecurityMappings());
                        configuration.BuildMapping();
                        if (File.Exists(Util.GetFullPath(System.Configuration.ConfigurationManager.AppSettings[Constants.HIBERNATE_CONFIG_KEY])))
                            configuration.Configure(Util.GetFullPath(System.Configuration.ConfigurationManager.AppSettings[Constants.HIBERNATE_CONFIG_KEY]));
                        if (File.Exists(Util.GetFullPath(System.Configuration.ConfigurationManager.AppSettings[Constants.HIBERNATE_CONFIG_KEY_Security])))
                            configuration.Configure(Util.GetFullPath(System.Configuration.ConfigurationManager.AppSettings[Constants.HIBERNATE_CONFIG_KEY_Security]));

                        _securityDomainFactory = configuration.BuildSessionFactory();
                    }
                }
            }
            return _securityDomainFactory;
        }
        public static ISessionFactory LogDomainFactory()
        {
            if (_logDomainFactory == null)
            {
                lock (_synRoot2)
                {
                    if (_logDomainFactory == null)
                    {
                        //var createSchema = false;
                        var configuration = new Configuration()
                            .DataBaseIntegration(d =>
                                            {
                                                d.ConnectionStringName = Constants.LOG_DB;
                                                d.Dialect<MsSql2012Dialect>();
                                                d.SchemaAction = SchemaAutoAction.Validate;
                                            })
                            .Proxy(p => p.ProxyFactoryFactory<DefaultProxyFactoryFactory>())
                            .CurrentSessionContext<LazySessionContext>()
                            .SetProperty(NHibernate.Cfg.Environment.Hbm2ddlKeyWords, "none");
                            //.SetProperty(NHibernate.Cfg.Environment.Hbm2ddlAuto, (createSchema == true) ? SchemaAutoAction.Update.ToString() : SchemaAutoAction.Validate.ToString());
                        configuration.AddMapping(GetLogMappings());
                        configuration.BuildMapping();
                        if (File.Exists(Util.GetFullPath(System.Configuration.ConfigurationManager.AppSettings[Constants.HIBERNATE_CONFIG_KEY])))
                            configuration.Configure(Util.GetFullPath(System.Configuration.ConfigurationManager.AppSettings[Constants.HIBERNATE_CONFIG_KEY]));
                        if (File.Exists(Util.GetFullPath(System.Configuration.ConfigurationManager.AppSettings[Constants.HIBERNATE_CONFIG_KEY_Log])))
                            configuration.Configure(Util.GetFullPath(System.Configuration.ConfigurationManager.AppSettings[Constants.HIBERNATE_CONFIG_KEY_Log]));

                        _logDomainFactory = configuration.BuildSessionFactory();
                    }
                }
            }
            return _logDomainFactory;
        }
        public static ISessionFactory AppDomainFactory()
        {
            if (_appDomainFactory == null)
            {
                lock (_synRoot3)
                {
                    if (_appDomainFactory == null)
                    {
                        //var createSchema = false;
                        var configuration = new Configuration()
                            .DataBaseIntegration(d =>
                            {
                                d.ConnectionStringName = Constants.APP_DB;
                                d.Dialect<MsSql2012Dialect>();
                                d.SchemaAction = SchemaAutoAction.Validate;
                            })
                            .Proxy(p => p.ProxyFactoryFactory<DefaultProxyFactoryFactory>())
                            .CurrentSessionContext<LazySessionContext>()
                            .SetProperty(NHibernate.Cfg.Environment.Hbm2ddlKeyWords, "none");
                            //.SetProperty(NHibernate.Cfg.Environment.Hbm2ddlAuto, (createSchema == true) ? SchemaAutoAction.Update.ToString() : SchemaAutoAction.Validate.ToString());
                        configuration.AddMapping(GetAppMappings());
                        configuration.BuildMapping();
                        if (File.Exists(Util.GetFullPath(System.Configuration.ConfigurationManager.AppSettings[Constants.HIBERNATE_CONFIG_KEY])))
                            configuration.Configure(Util.GetFullPath(System.Configuration.ConfigurationManager.AppSettings[Constants.HIBERNATE_CONFIG_KEY]));
                        if (File.Exists(Util.GetFullPath(System.Configuration.ConfigurationManager.AppSettings[Constants.HIBERNATE_CONFIG_KEY_App])))
                            configuration.Configure(Util.GetFullPath(System.Configuration.ConfigurationManager.AppSettings[Constants.HIBERNATE_CONFIG_KEY_App]));

                        _appDomainFactory = configuration.BuildSessionFactory();
                    }
                }
            }
            return _appDomainFactory;
        }
        private static HbmMapping GetSecurityMappings()
        {
            var mapper = new ModelMapper();

            mapper.AddMappings(Assembly.GetAssembly(typeof(RoleMap)).GetExportedTypes());
            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            return mapping;
        }
        private static HbmMapping GetLogMappings()
        {
            var mapper = new ModelMapper();

            mapper.AddMappings(Assembly.GetAssembly(typeof(AuthenticationAuditMap)).GetExportedTypes());
            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            return mapping;
        }
        private static HbmMapping GetAppMappings()
        {
            var mapper = new ModelMapper();

            mapper.AddMappings(Assembly.GetAssembly(typeof(SettingsMap)).GetExportedTypes());
            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            return mapping;
        }
    }
}