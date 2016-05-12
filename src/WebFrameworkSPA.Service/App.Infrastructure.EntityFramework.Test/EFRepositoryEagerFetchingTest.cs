using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net.Config;
using System.IO;
using App.Common;
using System.Data.Entity.Infrastructure;
using App.Infrastructure.EntityFramework.Test.OrdersDomain;
using System.Data.Entity.Core.Objects;
using App.Infrastructure.EntityFramework.Test.OrdersDomain.Domain;
using App.Data;

namespace App.Infrastructure.EntityFramework.Test
{
    [TestClass]
    public class EFRepositoryEagerFetchingTest
    {
        private ObjectContext Context { get { return ((IObjectContextAdapter)new OrdersDomainContext()).ObjectContext; } }
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Util.LogConfigFilePath));
            EFTestUtil.Setup();
        }
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            log4net.LogManager.Shutdown();
        }
        [TestMethod]
        public void Can_eager_fetch()
        {
            var testData = new EFTestData(Context);

            Order order = null;
            Order savedOrder = null;

            testData.Batch(x => order = x.CreateOrderForCustomer(x.CreateCustomer()));

            using (var scope = new UnitOfWorkScope())
            {
                savedOrder = new EFRepository<Order,int>()
                    .Fetch(o => o.Customer).Query
                    .Where(x => x.OrderID == order.OrderID)
                    .SingleOrDefault();
                scope.Commit();
            }

            Assert.IsNotNull(savedOrder);
            Assert.IsNotNull(savedOrder.Customer);
            var firstName = savedOrder.Customer.FirstName;

        }

        [TestMethod]
        public void Can_eager_fetch_many()
        {
            var testData = new EFTestData(Context);

            Customer customer = null;
            Customer savedCustomer = null;
            testData.Batch(x =>
            {
                customer = x.CreateCustomer();
                var order = x.CreateOrderForCustomer(customer);
                order.OrderItems.Add(x.CreateOrderItem(item => item.Order = order));
                order.OrderItems.Add(x.CreateOrderItem(item => item.Order = order));
                order.OrderItems.Add(x.CreateOrderItem(item => item.Order = order));
            });

            using (var scope = new UnitOfWorkScope())
            {
                savedCustomer = new EFRepository<Customer,int>()
                    .FetchMany(x => x.Orders)
                    .ThenFetchMany(x => x.OrderItems)
                    .ThenFetch(x => x.Product).Query
                    .Where(x => x.CustomerID == customer.CustomerID)
                    .SingleOrDefault();
                scope.Commit();
            }

            Assert.IsNotNull(savedCustomer);
            Assert.IsNotNull(savedCustomer.Orders);
            savedCustomer.Orders.ForEach(order =>
            {
                Assert.IsNotNull(order.OrderItems);
                order.OrderItems.ForEach(orderItem => Assert.IsNotNull(orderItem.Product));
            });
        }

        //[TestMethod]
        //public void Can_eager_fetch_using_for()
        //{
        //    Locator.Stub(x => x.GetAllInstances<IFetchingStrategy<Customer, EFRepositoryEagerFetchingTests>>())
        //        .Return(new[] { new FetchingStrategy() });

        //    var testData = new EFTestData(Context);
        //    Customer customer = null;
        //    Customer savedCustomer = null;
        //    testData.Batch(x =>
        //    {
        //        customer = x.CreateCustomer();
        //        var order = x.CreateOrderForCustomer(customer);
        //        order.OrderItems.Add(x.CreateOrderItem(item => item.Order = order));
        //        order.OrderItems.Add(x.CreateOrderItem(item => item.Order = order));
        //        order.OrderItems.Add(x.CreateOrderItem(item => item.Order = order));
        //    });

        //    using (var scope = new UnitOfWorkScope())
        //    {
        //        savedCustomer = new EFRepository<Customer>()
        //            .For<EFRepositoryEagerFetchingTests>()
        //            .Where(x => x.CustomerID == customer.CustomerID)
        //            .SingleOrDefault();
        //        scope.Commit();
        //    }

        //    Assert.NotNull(savedCustomer);
        //    Assert.NotNull(savedCustomer.Orders);
        //    savedCustomer.Orders.ForEach(order =>
        //    {
        //        Assert.NotNull(order.OrderItems);
        //        order.OrderItems.ForEach(orderItem => Assert.NotNull(orderItem.Product));
        //    });
        //}

        //class FetchingStrategy : IFetchingStrategy<Customer, EFRepositoryEagerFetchingTest>
        //{
        //    public IQueryable<Customer> Define(IRepository<Customer> repository)
        //    {
        //        return repository.FetchMany(x => x.Orders)
        //            .ThenFetchMany(x => x.OrderItems)
        //            .ThenFetch(x => x.Product);
        //    }
        //}
    }
}