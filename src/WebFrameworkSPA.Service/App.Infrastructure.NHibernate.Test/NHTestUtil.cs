using NHibernate;
using App.Infrastructure.NHibernate.Test.OrdersDomain;
using App.Infrastructure.NHibernate.Test.HRDomain.Domain;
using NHibernate.Tool.hbm2ddl;
using App.Data;
using Moq;
using App.Common.InversionOfControl;
using App.Common.Logging;
using App.Common.Caching;
using NHibernate.Dialect;
using System.IO;
using NHibernate.Cfg;
using System.Reflection;
using NHibernate.Mapping.ByCode;
using NHibernate.Bytecode;
using NHEnv = NHibernate.Cfg.Environment;

namespace App.Infrastructure.NHibernate.Test
{
    public class NHTestUtil
    {
        //private static SQLiteConnection _connection;
        internal static ISessionFactory OrdersDomainFactory { get; private set; }
        internal static ISessionFactory HRDomainFactory { get; private set; }
        private const string ConnectionString = @"Data Source=Test.sdf";
        //protected ISession OrdersDomainSession { get; set; }
        //protected ISession HRDomainSession { get; set; }
        private static System.Data.SqlServerCe.SqlCeEngine _engine;
        public static void Setup()
        {
            //if (File.Exists("Test.sdf")) File.Delete("Test.sdf"); 
            using (_engine = new System.Data.SqlServerCe.SqlCeEngine(ConnectionString))
            {
                if (!File.Exists("Test.sdf"))
                    _engine.CreateDatabase();
            }

            var cnf = new Configuration()
                .DataBaseIntegration(d =>
                {
                    d.ConnectionString = ConnectionString;
                    d.Dialect<MsSqlCe40Dialect>();
                    //d.Dialect<Oracle10gDialect>();
                    d.SchemaAction = SchemaAutoAction.Create;
                })
                .Proxy(p => p.ProxyFactoryFactory<DefaultProxyFactoryFactory>())
                .CurrentSessionContext<LazySessionContext>()
                //.SetProperty(NHEnv.Hbm2ddlKeyWords, "none")
                //.SetProperty(NHEnv.Hbm2ddlAuto, (createSchema == true) ? SchemaAutoAction.Update.ToString() : SchemaAutoAction.Validate.ToString())
                .SetProperty(NHEnv.ReleaseConnections, "on_close");
            var mapper = new ModelMapper();

            mapper.AddMappings(Assembly.GetAssembly(typeof(Order)).GetExportedTypes());
            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            cnf.AddMapping(mapping);
            cnf.BuildMapping();
            OrdersDomainFactory = cnf.BuildSessionFactory();

            cnf = new Configuration()
                            .DataBaseIntegration(d =>
                            {
                                d.ConnectionString = ConnectionString;
                                d.Dialect<MsSqlCe40Dialect>();
                                //d.Dialect<Oracle10gDialect>();
                                d.SchemaAction = SchemaAutoAction.Create;
                            })
                            .Proxy(p => p.ProxyFactoryFactory<DefaultProxyFactoryFactory>())
                            .CurrentSessionContext<LazySessionContext>()
                            //.SetProperty(NHEnv.Hbm2ddlKeyWords, "none")
                            //.SetProperty(NHEnv.Hbm2ddlAuto, (createSchema == true) ? SchemaAutoAction.Update.ToString() : SchemaAutoAction.Validate.ToString())
                            .SetProperty(NHEnv.ReleaseConnections, "on_close");
            mapper = new ModelMapper();

            mapper.AddMappings(Assembly.GetAssembly(typeof(SalesPerson)).GetExportedTypes());
            mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            cnf.AddMapping(mapping);
            cnf.BuildMapping();
            HRDomainFactory = cnf.BuildSessionFactory();
            NHUnitOfWorkFactory unitOfWorkFactory = new NHUnitOfWorkFactory();
            unitOfWorkFactory.RegisterSessionFactoryProvider(() => OrdersDomainFactory);
            unitOfWorkFactory.RegisterSessionFactoryProvider(() => HRDomainFactory);
            UnitOfWorkSettings.DefaultIsolation = System.Transactions.IsolationLevel.ReadCommitted;
            //IUnitOfWork uow = UnitOfWorkFactory.Create();
            var dependencyResolverFactory = new Mock<IDependencyResolverFactory>();
            var dependencyResolver = new Mock<ICustomDependencyResolver>();
            var logFacotry = new Mock<ILogFactory>();
            var cacheManager = new Mock<ICacheManager>();
            dependencyResolverFactory.Setup(x => x.CreateInstance()).Returns(dependencyResolver.Object);
            dependencyResolver.Setup(x => x.GetService(typeof(ILogFactory))).Returns(logFacotry.Object);
            dependencyResolver.Setup(x => x.GetService(typeof(ICacheManager), It.IsAny<string>())).Returns(cacheManager.Object);
            dependencyResolver.Setup(x => x.GetService(typeof(IUnitOfWorkFactory))).Returns(unitOfWorkFactory);
            //cacheManager.Setup(x => x.Get<IUnitOfWork>(It.IsAny<string>())).Returns(uow);
            IoC.InitializeWith(dependencyResolverFactory.Object);
            App.Data.TransactionManager transactionManager = new Data.TransactionManager();
            cacheManager.Setup(x => x.Get<ITransactionManager>(It.IsAny<string>())).Returns(transactionManager);
        }
        internal static void Dispose()
        {
            OrdersDomainFactory.Dispose();
            HRDomainFactory.Dispose();
            _engine.Dispose();
        }

    }
}
