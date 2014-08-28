using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net.Config;
using System.IO;
using App.Common;
using App.Infrastructure.EntityFramework.Test.OrdersDomain.Domain;
using App.Data;
using System.Data.Entity.Infrastructure;
using App.Infrastructure.EntityFramework.Test.OrdersDomain;

namespace App.Infrastructure.EntityFramework.Test
{
    [TestClass]
    public class EFRepositoryTransactionTest
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
        public void can_commit()
        {
            var customer = new Customer
            {
                FirstName = "John",
                LastName = "Doe"
            };
            using (var scope = new UnitOfWorkScope())
            {
                new EFRepository<Customer,int>()
                    .Add(customer);
                scope.Commit();
            }

            var savedCustomer = new EFRepository<Customer, int>().Query
                .First(x => x.CustomerID == customer.CustomerID);
            Assert.IsNotNull(savedCustomer);

        }

        [TestMethod]
        public void can_rollback()
        {
            using (var testData = new EFTestData(Context))
            {
                Customer customer = null;
                testData.Batch(action => customer = action.CreateCustomer());

                using (new UnitOfWorkScope())
                {
                    var savedCustomer = new EFRepository<Customer,int>().Query
                        .Where(x => x.CustomerID == customer.CustomerID)
                        .First();
                    savedCustomer.LastName = "Changed";
                } //Dispose here as scope is not comitted.
                var repository=new EFRepository<Customer, int>();
                testData.Context.Refresh(RefreshMode.StoreWins, customer);
                Assert.AreNotEqual(customer.LastName, "Changed");
            }
        }

        [TestMethod]
        public void nested_commit_works()
        {
            var customer = new Customer { FirstName = "Joe", LastName = "Data" };
            var order = new Order { OrderDate = DateTime.Now, ShipDate = DateTime.Now };
            using (var scope = new UnitOfWorkScope())
            {
                new EFRepository<Customer,int>().Add(customer);
                using (var scope2 = new UnitOfWorkScope())
                {
                    new EFRepository<Order,int>().Add(order);
                    scope2.Commit();
                }
                scope.Commit();
            }
            var savedCustomer = new EFRepository<Customer, int>().Query.First(x => x.CustomerID == customer.CustomerID);
            var savedOrder = new EFRepository<Order, int>().Query.First(x => x.OrderID == order.OrderID);

            Assert.IsNotNull(savedCustomer);
            Assert.AreEqual(savedCustomer.CustomerID, customer.CustomerID);
            Assert.IsNotNull(savedOrder);
            Assert.AreEqual(savedOrder.OrderID, order.OrderID);
        }

        [TestMethod]
        public void nested_commit_with_seperate_transaction_commits_when_wrapping_scope_rollsback()
        {
            var customer = new Customer { FirstName = "Joe", LastName = "Data" };
            var product = new Product { Name="apple",Description="fruit" };
            using (var scope = new UnitOfWorkScope())
            {
                new EFRepository<Customer,int>().Add(customer);
                using (var scope2 = new UnitOfWorkScope(TransactionMode.New))
                {
                    new EFRepository<Product, int>().Add(product);
                    scope2.Commit();
                }
            } //Rollback
            var savedCustomer = new EFRepository<Customer, int>().Query.FirstOrDefault(x => x.CustomerID == customer.CustomerID);
            var savedProduct = new EFRepository<Product, int>().Query.First(x => x.ProductID == product.ProductID);
            Assert.IsNull(savedCustomer);
            Assert.IsNotNull(savedProduct);
            Assert.AreEqual(savedProduct.ProductID, product.ProductID);
        }

        [TestMethod]
        public void nested_rollback_works()
        {
            var customer = new Customer { FirstName = "Joe", LastName = "Data" };
            var order = new Order { OrderDate = DateTime.Now, ShipDate = DateTime.Now };
            using (var scope = new UnitOfWorkScope())
            {
                new EFRepository<Customer,int>().Add(customer);
                using (var scope2 = new UnitOfWorkScope())
                {
                    new EFRepository<Order,int>().Add(order);
                    scope2.Commit();
                }
            } //Rollback.

            var savedCustomer = new EFRepository<Customer, int>().Query.FirstOrDefault(x => x.CustomerID == customer.CustomerID);
            var savedOrder = new EFRepository<Order, int>().Query.FirstOrDefault(x => x.OrderID == order.OrderID);

            Assert.IsNull(savedCustomer);
            Assert.IsNull(savedOrder);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void commit_throws_when_child_scope_rollsback()
        {
            var customer = new Customer { FirstName = "Joe", LastName = "Data" };
            var order = new Order { OrderDate = DateTime.Now, ShipDate = DateTime.Now };
            using (var scope = new UnitOfWorkScope())
            {
                new EFRepository<Customer,int>().Add(customer);
                using (var scope2 = new UnitOfWorkScope())
                {
                    new EFRepository<Order,int>().Add(order);
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
        //        new EFRepository<Customer>().Add(customer);
        //        new EFRepository<SalesPerson>().Add(salesPerson);
        //        scope.Commit();
        //    }

        //    using (var ordersTestData = new EFTestData(OrdersContextProvider()))
        //    using (var hrTestData = new EFTestData(HRContextProvider()))
        //    {
        //        Customer savedCustomer = null;
        //        SalesPerson savedSalesPerson = null;
        //        ordersTestData.Batch(action => savedCustomer = action.GetCustomerById(customer.CustomerID));
        //        hrTestData.Batch(action => savedSalesPerson = action.GetSalesPersonById(salesPerson.Id));

        //        Assert.That(savedCustomer, Is.Not.Null);
        //        Assert.That(savedSalesPerson, Is.Not.Null);
        //        Assert.That(savedCustomer.CustomerID, Is.EqualTo(customer.CustomerID));
        //        Assert.That(savedSalesPerson.Id, Is.EqualTo(salesPerson.Id));
        //    }
        //}

        //[TestMethod]
        //public void can_rollback_multipe_db_operations()
        //{
        //    var customer = new Customer { FirstName = "John", LastName = "Doe" };
        //    var salesPerson = new SalesPerson { FirstName = "Jane", LastName = "Doe", SalesQuota = 2000 };

        //    using (var scope = new UnitOfWorkScope())
        //    {
        //        new EFRepository<Customer>().Add(customer);
        //        new EFRepository<SalesPerson>().Add(salesPerson);
        //    }// Rolllback

        //    using (var ordersTestData = new EFTestData(OrdersContextProvider()))
        //    using (var hrTestData = new EFTestData(HRContextProvider()))
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
        public void rollback_does_not_rollback_supressed_scope()
        {
            var customer = new Customer { FirstName = "Joe", LastName = "Data" };
            var product = new Product { Name = "apple", Description = "fruit" };
            using (var scope = new UnitOfWorkScope())
            {
                new EFRepository<Customer,int>().Add(customer);
                using (var scope2 = new UnitOfWorkScope(TransactionMode.Supress))
                {
                    new EFRepository<Product, int>().Add(product);
                    scope2.Commit();
                }
            } //Rollback.

            using (var testData = new EFTestData(Context))
            {
                var savedCustomer = new EFRepository<Customer, int>().Query.FirstOrDefault(x => x.CustomerID == customer.CustomerID);
                var savedProduct = new EFRepository<Product, int>().Query.First(x => x.ProductID == product.ProductID);
                Assert.IsNull(savedCustomer);
                Assert.IsNotNull(savedProduct);
            }
        }
    }
}