using System;
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
using NHibernate.Mapping.ByCode;
using NHibernate.Cfg;
using NHibernate.Bytecode;
using System.Reflection;
using NHEnv = NHibernate.Cfg.Environment;
using System.IO;

namespace App.Infrastructure.NHibernate.Test
{
    public class NHTestBase : IDisposable
    {
        //private static SQLiteConnection _connection;
        protected static ISessionFactory OrdersDomainFactory { get; private set; }
        protected static ISessionFactory HRDomainFactory { get; private set; }
        protected const string ConnectionString = @"Data Source=Test.sdf";
        //protected ISession OrdersDomainSession { get; set; }
        //protected ISession HRDomainSession { get; set; }
        private static System.Data.SqlServerCe.SqlCeEngine _engine;
        public NHTestBase()
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


            UnitOfWorkFactory = new NHUnitOfWorkFactory();
            UnitOfWorkFactory.RegisterSessionFactoryProvider(() => OrdersDomainFactory);
            UnitOfWorkFactory.RegisterSessionFactoryProvider(() => HRDomainFactory);
            UnitOfWorkSettings.DefaultIsolation = System.Transactions.IsolationLevel.ReadCommitted;
            //IUnitOfWork uow = UnitOfWorkFactory.Create();
            var dependencyResolverFactory = new Mock<IDependencyResolverFactory>();
            var dependencyResolver = new Mock<ICustomDependencyResolver>();
            var logFacotry = new Mock<ILogFactory>();
            var cacheManager = new Mock<ICacheManager>();
            var transactionManager = new Mock<ITransactionManager>();
            dependencyResolverFactory.Setup(x => x.CreateInstance()).Returns(dependencyResolver.Object);
            dependencyResolver.Setup(x => x.GetService(typeof(ILogFactory))).Returns(logFacotry.Object);
            dependencyResolver.Setup(x => x.GetService(typeof(ICacheManager), It.IsAny<string>())).Returns(cacheManager.Object);
            dependencyResolver.Setup(x => x.GetService(typeof(IUnitOfWorkFactory))).Returns(UnitOfWorkFactory);
            //cacheManager.Setup(x => x.Get<IUnitOfWork>(It.IsAny<string>())).Returns(uow);
            IoC.InitializeWith(dependencyResolverFactory.Object);
        }
        // To detect redundant calls
        private bool disposedValue = false;

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    //if (OrdersDomainSession != null)
                    //    OrdersDomainSession.Close();
                    //if (HRDomainSession != null)
                    //    HRDomainSession.Close();
                    // TODO: free other state (managed objects).
                    //if (_connection != null)
                    //    _connection.Close();
                    //_connection = null;
                    _engine.Dispose();
                }

                // TODO: free your own state (unmanaged objects).
                // TODO: set large fields to null.
            }
            this.disposedValue = true;
        }

        #region " IDisposable Support "
        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //    OrdersDomainFactory = Fluently.Configure()
        //           .Database(SQLiteConfiguration.Standard
        //           .ConnectionString(((ConnectionStringBuilder cs) => cs.Is(ConnectionString))))
        //           .Mappings(mappings => mappings.FluentMappings.AddFromAssembly(typeof(Order).Assembly).ExportTo("."))
        //           .ExposeConfiguration(x => new SchemaExport(x).Execute(false, true, false, GetConnection(), null))
        //           .BuildSessionFactory();

        //    HRDomainFactory = Fluently.Configure()
        //          .Database(SQLiteConfiguration.Standard
        //          .ConnectionString(((ConnectionStringBuilder cs) => cs.Is(ConnectionString))))
        //          .Mappings(mappings => mappings.FluentMappings.AddFromAssembly(typeof(SalesPerson).Assembly).ExportTo("."))
        //          .ExposeConfiguration(x => new SchemaExport(x).Execute(false, true, false, GetConnection(), null))
        //          .BuildSessionFactory();
        //}
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //    if (_connection != null)
        //        _connection.Dispose();
        //}
        //protected IDbConnection GetConnection()
        //{
        //    if (_connection == null)
        //    {
        //        _connection = new SQLiteConnection(ConnectionString);
        //        _connection.Open();
        //    }
        //    return _connection;
        //}

        public NHUnitOfWorkFactory UnitOfWorkFactory { get; set; }
    }
}
