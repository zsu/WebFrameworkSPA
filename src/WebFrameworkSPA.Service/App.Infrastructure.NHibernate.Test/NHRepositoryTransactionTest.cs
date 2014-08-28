using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using log4net.Config;
using App.Common;
using App.Infrastructure.NHibernate.Test.OrdersDomain;
using App.Data;

namespace App.Infrastructure.NHibernate.Test
{
    [TestClass()]
    public class NHRepositoryTransactionTests
    {
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Util.LogConfigFilePath));
            NHTestUtil.Setup();
        }
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            log4net.LogManager.Shutdown();
            NHTestUtil.Dispose();
        }
        [TestMethod]
        public void can_commit()
        {
            var customer = new Customer
            {
                FirstName = "John",
                LastName = "Doe"
            };
            using (var scope = new UnitOfWorkScope())
            {
                new NHRepository<Customer,int>()
                    .Add(customer);
                scope.Commit();
            }

            using (var testData = new NHTestData(NHTestUtil.OrdersDomainFactory.OpenSession()))
            {
                Customer savedCustomer = null;
                testData.Batch(action => savedCustomer = action.GetCustomerById(customer.CustomerID));

                Assert.IsNotNull(savedCustomer);
                Assert.AreEqual(savedCustomer.CustomerID, customer.CustomerID);
            }

        }

        [TestMethod]
        public void can_rollback()
        {
            using (var testData = new NHTestData(NHTestUtil.OrdersDomainFactory.OpenSession()))
            {
                Customer customer = null;
                testData.Batch(action => customer = action.CreateCustomer());

                using (new UnitOfWorkScope())
                {
                    var savedCustomer = new NHRepository<Customer,int>().Query
                        .Where(x => x.CustomerID == customer.CustomerID)
                        .First();
                    savedCustomer.LastName = "Changed";
                } //Dispose here as scope is not comitted.

                testData.Session.Refresh(customer);
                Assert.AreNotEqual(customer.LastName, "Changed");
            }
        }

        [TestMethod]
        public void nested_commit_works()
        {
            var customer = new Customer { FirstName = "Joe", LastName = "Data" };
            var order = new Order {OrderDate = DateTime.Now, ShipDate = DateTime.Now};
            using (var scope = new UnitOfWorkScope())
            {
                new NHRepository<Customer,int>().Add(customer);
                using (var scope2 = new UnitOfWorkScope())
                {
                    new NHRepository<Order,int>().Add(order);
                    scope2.Commit();
                }
                scope.Commit();
            }

            using (var testData = new NHTestData(NHTestUtil.OrdersDomainFactory.OpenSession()))
            {
                Customer savedCustomer = null;
                Order savedOrder = null;
                testData.Batch(actions =>
                {
                    savedCustomer = actions.GetCustomerById(customer.CustomerID);
                    savedOrder = actions.GetOrderById(order.OrderID);
                });

                Assert.IsNotNull(savedCustomer);
                Assert.AreEqual(savedCustomer.CustomerID, customer.CustomerID);
                Assert.IsNotNull(savedOrder);
                Assert.AreEqual(savedOrder.OrderID, order.OrderID);
            }
        }

        [TestMethod]
        public void nested_commit_with_seperate_transaction_commits_when_wrapping_scope_rollsback()
        {
            var customer = new Customer { FirstName = "Joe", LastName = "Data" };
            var order = new Order { OrderDate = DateTime.Now, ShipDate = DateTime.Now };
            using (var scope = new UnitOfWorkScope())
            {
                new NHRepository<Customer,int>().Add(customer);
                using (var scope2 = new UnitOfWorkScope(TransactionMode.New))
                {
                    new NHRepository<Order,int>().Add(order);
                    scope2.Commit();
                }
            } //Rollback

            using (var testData = new NHTestData(NHTestUtil.OrdersDomainFactory.OpenSession()))
            {
                Customer savedCustomer = null;
                Order savedOrder = null;
                testData.Batch(actions =>
                {
                    savedCustomer = actions.GetCustomerById(customer.CustomerID);
                    savedOrder = actions.GetOrderById(order.OrderID);
                });

                Assert.IsNull(savedCustomer);
                Assert.IsNotNull(savedOrder);
                Assert.AreEqual(savedOrder.OrderID, order.OrderID);
            }
        }

        [TestMethod]
        public void nested_rollback_works()
        {
            var customer = new Customer { FirstName = "Joe", LastName = "Data" };
            var order = new Order { OrderDate = DateTime.Now, ShipDate = DateTime.Now };
            using (var scope = new UnitOfWorkScope())
            {
                new NHRepository<Customer,int>().Add(customer);
                using (var scope2 = new UnitOfWorkScope())
                {
                    new NHRepository<Order,int>().Add(order);
                    scope2.Commit();
                } 
            } //Rollback.

            using (var testData = new NHTestData(NHTestUtil.OrdersDomainFactory.OpenSession()))
            {
                Customer savedCustomer = null;
                Order savedOrder = null;
                testData.Batch(actions =>
                {
                    savedCustomer = actions.GetCustomerById(customer.CustomerID);
                    savedOrder = actions.GetOrderById(order.OrderID);
                });

                Assert.IsNull(savedCustomer);
                Assert.IsNull(savedOrder);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void commit_throws_when_child_scope_rollsback()
        {
            var customer = new Customer { FirstName = "Joe", LastName = "Data" };
            var order = new Order { OrderDate = DateTime.Now, ShipDate = DateTime.Now };
            using (var scope = new UnitOfWorkScope())
            {
                new NHRepository<Customer,int>().Add(customer);
                using (var scope2 = new UnitOfWorkScope())
                {
                    new NHRepository<Order,int>().Add(order);
                } //child scope rollback.
                scope.Commit();
            } 
        }

        //[TestMethod]
        //public void can_commit_multiple_db_operations()
        //{
        //    var customer = new Customer { FirstName = "John", LastName = "Doe" };
        //    var salesPerson = new SalesPerson { FirstName = "Jane", LastName = "Doe", SalesQuota = 2000 };

        //    using (var scope = new UnitOfWorkScope())
        //    {
        //        new NHRepository<Customer,int>().Add(customer);
        //        new NHRepository<SalesPerson,int>().Add(salesPerson);
        //        scope.Commit();
        //    }

        //    using (var ordersTestData = new NHTestData(NHTestUtil.OrdersDomainFactory.OpenSession()))
        //    using (var hrTestData = new NHTestData(NHTestUtil.HRDomainFactory.OpenSession()))
        //    {
        //        Customer savedCustomer = null;
        //        SalesPerson savedSalesPerson = null;
        //        ordersTestData.Batch(action => savedCustomer = action.GetCustomerById(customer.CustomerID));
        //        hrTestData.Batch(action => savedSalesPerson = action.GetSalesPersonById(salesPerson.Id));

        //        Assert.IsNotNull(savedCustomer);
        //        Assert.IsNotNull(savedSalesPerson);
        //        Assert.AreEqual(savedCustomer.CustomerID, customer.CustomerID);
        //        Assert.AreEqual(savedSalesPerson.Id, salesPerson.Id);
        //    }
        //}

        //[TestMethod]
        //public void can_rollback_multipe_db_operations()
        //{
        //    var customer = new Customer { FirstName = "John", LastName = "Doe" };
        //    var salesPerson = new SalesPerson { FirstName = "Jane", LastName = "Doe", SalesQuota = 2000 };

        //    using (var scope = new UnitOfWorkScope())
        //    {
        //        new NHRepository<Customer>().Add(customer);
        //        new NHRepository<SalesPerson>().Add(salesPerson);
        //    }// Rolllback

        //    using (var ordersTestData = new NHTestData(OrdersDomainFactory.OpenSession()))
        //    using (var hrTestData = new NHTestData(HRDomainFactory.OpenSession()))
        //    {
        //        Customer savedCustomer = null;
        //        SalesPerson savedSalesPerson = null;
        //        ordersTestData.Batch(action => savedCustomer = action.GetCustomerById(customer.CustomerID));
        //        hrTestData.Batch(action => savedSalesPerson = action.GetSalesPersonById(salesPerson.Id));

        //        Assert.That(savedCustomer, Is.Null);
        //        Assert.That(savedSalesPerson, Is.Null);
        //    }
        //}

        [TestMethod]
        public void rollback_does_not_rollback_new_scope()
        {
            var customer = new Customer { FirstName = "Joe", LastName = "Data" };
            var product = new Product { Name = "apple", Description = "fruit" };
            using (var scope = new UnitOfWorkScope())
            {
                new NHRepository<Customer, int>().Add(customer);
                using (var scope2 = new UnitOfWorkScope(TransactionMode.New))//.Supress))//"null identifier" exception,cannot generate ProductId
                {
                    new NHRepository<Product,int>().Add(product);
                    scope2.Commit();
                }
            } //Rollback.

            using (var testData = new NHTestData(NHTestUtil.OrdersDomainFactory.OpenSession()))
            {

                var savedCustomer = new NHRepository<Customer, int>().Query.FirstOrDefault(x => x.CustomerID == customer.CustomerID);
                var savedProduct = new NHRepository<Product, int>().Query.First(x => x.ProductID == product.ProductID);
                Assert.IsNull(savedCustomer);
                Assert.IsNotNull(savedProduct);
            }
        }
    }
}