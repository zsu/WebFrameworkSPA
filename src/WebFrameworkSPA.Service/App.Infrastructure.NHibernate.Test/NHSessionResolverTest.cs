using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NHibernate.Cfg;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Bytecode;
using NHibernate.Mapping.ByCode;
using App.Infrastructure.NHibernate.Test.OrdersDomain;
using App.Infrastructure.NHibernate.Test.HRDomain.Domain;
using log4net.Config;
using System.IO;
using App.Common;
using System.Reflection;
using SysEnv = System.Environment;
using NHEnv = NHibernate.Cfg.Environment;

namespace App.Infrastructure.NHibernate.Test
{


    /// <summary>
    ///This is a test class for NHSessionResolverTest and is intended
    ///to contain all NHSessionResolverTest Unit Tests
    ///</summary>
    [TestClass()]
    public class NHSessionResolverTest
    {


        private TestContext testContextInstance;
        private static ISessionFactory _ordersFactory;
        private static ISessionFactory _hrFactory;
        private const string ConnectionString = @"Data Source=Test.sdf";
        private static System.Data.SqlServerCe.SqlCeEngine _engine;
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes

        //You can use the following additional attributes as you write your tests:

        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Util.LogConfigFilePath));
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
            _ordersFactory = cnf.BuildSessionFactory();

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
            _hrFactory = cnf.BuildSessionFactory();
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            log4net.LogManager.Shutdown();
            _ordersFactory.Dispose();
            _hrFactory.Dispose();
            _engine.Dispose();
        }
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterSessionFactoryProvider_throws_ArgumentNullException_when_provider_is_null()
        {
            new NHSessionResolver().RegisterSessionFactoryProvider(null);
        }

        [TestMethod]
        public void SessionFactoriesRegistered_returns_correct_count()
        {
            var resolver = new NHSessionResolver();
            resolver.RegisterSessionFactoryProvider(() => _ordersFactory);
            resolver.RegisterSessionFactoryProvider(() => _hrFactory);
            Assert.AreEqual(resolver.SessionFactoriesRegistered, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetSessionKeyFor_throws_ArgumentException_when_no_factory_is_registered_to_handle_specified_type()
        {
            new NHSessionResolver().GetSessionKeyFor<string>();
        }

        [TestMethod]
        public void GetSessionKeyFor_returns_same_key_for_types_handled_by_the_same_factory()
        {
            var resolver = new NHSessionResolver();
            resolver.RegisterSessionFactoryProvider(() => _ordersFactory);
            resolver.RegisterSessionFactoryProvider(() => _hrFactory);

            var key = resolver.GetSessionKeyFor<Customer>();
            var key2 = resolver.GetSessionKeyFor<Order>();
            Assert.AreEqual(key, key2);
        }

        [TestMethod]
        public void GetSessionFactoryFor_returns_orders_factory_when_requested_for_customer()
        {
            var resolver = new NHSessionResolver();
            resolver.RegisterSessionFactoryProvider(() => _ordersFactory);
            resolver.RegisterSessionFactoryProvider(() => _hrFactory);

            var resolved = resolver.GetFactoryFor<Order>();
            Assert.IsNotNull(resolved);
            Assert.ReferenceEquals(resolved, _ordersFactory);
        }

        [TestMethod]
        public void GetSessionFactoryFor_returns_hr_factory_when_requested_for_Employee()
        {
            var resolver = new NHSessionResolver();
            resolver.RegisterSessionFactoryProvider(() => _ordersFactory);
            resolver.RegisterSessionFactoryProvider(() => _hrFactory);

            var resolved = resolver.GetFactoryFor<SalesPerson>();
            Assert.IsNotNull(resolved);
            Assert.ReferenceEquals(resolved, _hrFactory);
        }
    }
}
