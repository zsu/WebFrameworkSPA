using System;
using System.Linq;
using App.Infrastructure.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net.Config;
using System.IO;
using App.Common;
using App.Infrastructure.EntityFramework.Test.OrdersDomain.Domain;
using App.Data;
using App.Infrastructure.EntityFramework.Test.OrdersDomain;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;

namespace App.Infrastructure.EntityFramework.Test
{
    [TestClass]
    public class EFRepositoryQueryTests
    {
        private ObjectContext Context
        {
            get
            {
                    return new OrdersDomainContext().Context;
            }
        }
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
        public void Can_perform_simple_query()
        {
            var testData = new EFTestData(Context);
            Customer customer = null;
            testData.Batch(x => customer = x.CreateCustomer());
            using (var scope = new UnitOfWorkScope())
            {
                var savedCustomer = new EFRepository<Customer,int>().Query
                    .First(x => x.CustomerID == customer.CustomerID);
                Assert.IsNotNull(savedCustomer);
                scope.Commit();
            }
        }

        [TestMethod]
        public void Can_save()
        {
            int customerId;
            using (var scope = new UnitOfWorkScope())
            {
                var customer = new Customer
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    StreetAddress1 = "123 Main St.",
                };
                new EFRepository<Customer,int>().Add(customer);
                scope.Commit();
                customerId = customer.CustomerID;
            }
            var savedCustomer = new EFTestData(Context)
                .Get<Customer>(x => x.CustomerID == customerId);
            Assert.IsNotNull(savedCustomer);
        }

        [TestMethod]
        public void Can_modify()
        {
            Customer customer = null;
            var testData = new EFTestData(Context);
            testData.Batch(x => customer = x.CreateCustomer());
            using (var scope = new UnitOfWorkScope())
            {
                var savedCustomer = new EFRepository<Customer,int>().Query
                    .First(x => x.CustomerID == customer.CustomerID);
                savedCustomer.FirstName = "Changed";
                scope.Commit();
            }

            testData.Refresh(customer);
            Assert.AreEqual(customer.FirstName, "Changed");
        }

        [TestMethod]
        public void Can_delete()
        {
            Customer customer = null;
            var testData = new EFTestData(Context);
            testData.Batch(x => customer = x.CreateCustomer());
            using (var scope = new UnitOfWorkScope())
            {
                var repository = new EFRepository<Customer,int>();
                var savedCustomer = repository.Query.First(x => x.CustomerID == customer.CustomerID);
                repository.Delete(savedCustomer);
                scope.Commit();
            }

            using (var scope = new UnitOfWorkScope())
            {
                var repository = new EFRepository<Customer,int>();
                Assert.IsNull(repository.Query.FirstOrDefault(x => x.CustomerID == customer.CustomerID));
                scope.Commit();
            }
        }

        [TestMethod]
        public void Can_attach()
        {
            Customer customer = null;
            var testData = new EFTestData(Context);
            testData.Batch(x => customer = x.CreateCustomer());
            testData.Context.Detach(customer);
            Context.Dispose();

            using (var scope = new UnitOfWorkScope())
            {
                var repository = new EFRepository<Customer,int>();
                repository.Attach(customer);
                customer.FirstName = "Changed";
                scope.Commit();
            }

            testData = new EFTestData(Context);
            customer = testData.Get<Customer>(x => x.CustomerID == customer.CustomerID);
            Assert.AreEqual(customer.FirstName, "Changed");
        }

        //[TestMethod]
        //public void Can_query_using_specification()
        //{
        //    var testData = new EFTestData(Context);
        //    testData.Batch(x =>
        //    {
        //        x.CreateCustomer(customer => customer.State = "CA");
        //        x.CreateCustomer(customer => customer.State = "CA");
        //        x.CreateCustomer(customer => customer.State = "PA");
        //    });

        //    using (var scope = new UnitOfWorkScope())
        //    {
        //        var specification = new Specification<Customer>(x => x.State == "CA");
        //        var results = new EFRepository<Customer>()
        //            .Query(specification);
        //        Assert.That(results.Count(), Is.EqualTo(2));
        //        scope.Commit();
        //    }
        //}

        [TestMethod]
        public void Can_lazyload()
        {
            Customer customer = null;
            var testData = new EFTestData(Context);
            testData.Batch(x =>
            {
                customer = x.CreateCustomer();
                x.CreateOrderForCustomer(customer);
            });

            //using (var scope = new UnitOfWorkScope())
            //{
                var savedCustomer = new EFRepository<Customer,int>().Query
                    .First(x => x.CustomerID == customer.CustomerID);
                Assert.IsNotNull(savedCustomer);
                Assert.AreEqual(savedCustomer.Orders.Count, 1);
            //    scope.Commit();
            //}
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void throws_when_lazyloading_outside_of_scope()
        {
            Order order = null;
            var testData = new EFTestData(Context);
            testData.Batch(x =>
                order = x.CreateOrderForCustomer(x.CreateCustomer()));

            Order savedOrder = null;
            using (var scope = new UnitOfWorkScope())
            {
                savedOrder = new EFRepository<Order,int>().Query
                    .First(x => x.OrderID == order.OrderID);
                scope.Commit();
            }

            Assert.IsNotNull(savedOrder);
            var fname = savedOrder.Customer.FirstName;
        }
    }
}