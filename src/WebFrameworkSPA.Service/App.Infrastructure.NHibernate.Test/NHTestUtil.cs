using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using App.Infrastructure.NHibernate.Test.OrdersDomain;
using App.Infrastructure.NHibernate.Test.HRDomain.Domain;
using NHibernate.Tool.hbm2ddl;
using System.Transactions;
using App.Data;
using Moq;
using App.Common.InversionOfControl;
using App.Common.Logging;
using App.Common.Caching;
using NHibernateCfg = NHibernate.Cfg;
using System.Data;
using NHibernate.Dialect;
using System.IO;

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
        public static void Setup()
        {
            if (File.Exists("Test.sdf")) File.Delete("Test.sdf"); 
            using (var engine = new System.Data.SqlServerCe.SqlCeEngine(ConnectionString))
            {
                engine.CreateDatabase();
            }
            var cnf = Fluently.Configure()
            .Database(MsSqlCeConfiguration.Standard
            .ConnectionString(((ConnectionStringBuilder cs) => cs.Is(ConnectionString)))
            .Dialect <MsSqlCe40Dialect>())
            .Mappings(mappings => mappings.FluentMappings.AddFromAssembly(typeof(Order).Assembly).ExportTo("."))
            .ExposeConfiguration(x => new SchemaExport(x).Execute(false, true, false));//, GetConnection(), null));
            //var config = cnf.BuildConfiguration()
            //    .SetProperty(NHibernateCfg.Environment.ReleaseConnections, "on_close");
            OrdersDomainFactory = cnf.BuildConfiguration().BuildSessionFactory();

            cnf  = Fluently.Configure()
            .Database(MsSqlCeConfiguration.Standard
            .ConnectionString(((ConnectionStringBuilder cs) => cs.Is(ConnectionString)))
            .Dialect <MsSqlCe40Dialect>())
            .Mappings(mappings => mappings.FluentMappings.AddFromAssembly(typeof(SalesPerson).Assembly).ExportTo("."))
            .ExposeConfiguration(x => new SchemaExport(x).Execute(false, true, false));//, GetConnection(), null));
            //config = cnf.BuildConfiguration()
            //    .SetProperty(NHibernateCfg.Environment.ReleaseConnections, "on_close"); 
            HRDomainFactory = cnf.BuildConfiguration().BuildSessionFactory();
            //OrdersDomainSession = OrdersDomainFactory.OpenSession(GetConnection());
            //HRDomainSession = HRDomainFactory.OpenSession(GetConnection());
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
        }

    }
}
