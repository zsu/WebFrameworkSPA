using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using App.Data;
using Moq;
using App.Common.InversionOfControl;
using App.Common.Logging;
using App.Common.Caching;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using App.Infrastructure.EntityFramework.Test.OrdersDomain;
using App.Infrastructure.EntityFramework;
using System.Data.Entity.Infrastructure;

namespace App.Infrastructure.EntityFramework.Test
{
    public class EFTestUtil
    {
        //internal static ObjectContext OrdersDomainFactory { get; private set; }
        //internal static ObjectContext HRDomainFactory { get; private set; }
        private const string ConnectionString = @"Data Source=Test.sdf";
        public static void Setup()
        {
            Database.SetInitializer<OrdersDomainContext>(new CreateDatabaseIfNotExists<OrdersDomainContext>());

            EFUnitOfWorkFactory unitOfWorkFactory = new EFUnitOfWorkFactory();
            //http://fknet.wordpress.com/2011/02/25/entity-framework-problemsolution-of-default-connection-closing/
            unitOfWorkFactory.RegisterObjectContextProvider(() => { 

                            var ctx = new OrdersDomainContext();

                            if (Transaction.Current != null)

                            {

                                ctx.Context.Connection.Open();

                            }

                            return ctx.Context;

                        });
            //unitOfWorkFactory.RegisterObjectContextProvider(() => HRDomainFactory);
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
            //logFacotry.Setup(x => x.Create(It.IsAny<string>())).Returns(new Mock<ILog>().Object);
            //cacheManager.Setup(x => x.Get<IUnitOfWork>(It.IsAny<string>())).Returns(uow);
            IoC.InitializeWith(dependencyResolverFactory.Object);
            App.Data.TransactionManager transactionManager = new Data.TransactionManager();
            cacheManager.Setup(x => x.Get<ITransactionManager>(It.IsAny<string>())).Returns(transactionManager);
        }

    }
}
