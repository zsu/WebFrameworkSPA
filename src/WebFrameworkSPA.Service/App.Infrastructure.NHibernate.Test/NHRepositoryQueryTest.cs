using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using App.Data;
using App.Infrastructure.NHibernate.Test.OrdersDomain;
using App.Common;
using log4net.Config;
using System.IO;

namespace App.Infrastructure.NHibernate.Test
{
    /// <summary>
    /// Runs standard query tests on Repository
    /// </summary>
    [TestClass()]
    public class RepositoryQueryTests
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
        public void Can_perform_simple_query()
        {
            using (var testData = new NHTestData(NHTestUtil.OrdersDomainFactory.OpenSession()))
            {
                var customerId = 0;
                testData.Batch(x => customerId = x.CreateCustomer().CustomerID);

                using (var scope = new UnitOfWorkScope())
                {
                    var customer = new NHRepository<Customer, int>().Query
                        .Where(x => x.CustomerID == customerId)
                        .First();

                    Assert.IsNotNull(customer);
                    Assert.AreEqual(customer.CustomerID, customerId);
                    scope.Commit();
                }
            }
        }

        [TestMethod]
        public void Can_save()
        {
            var customer = new Customer
            {
                FirstName = "Jane",
                LastName = "Doe",
                Address = new Address
                {
                    StreetAddress1 = "123 Main St",
                    City = "Sunset City",
                    State = "LA",
                    ZipCode = "12345"
                }
            };

            using (var scope = new UnitOfWorkScope())
            {
                var repository = new NHRepository<Customer, int>();
                repository.Add(customer);
                scope.Commit();
            }
            Assert.IsTrue(customer.CustomerID > 0);
            using (var testData = new NHTestData(NHTestUtil.OrdersDomainFactory.OpenSession()))
            {
                Customer savedCustomer = null;
                testData.Batch(action => savedCustomer = action.GetCustomerById(customer.CustomerID));
                Assert.IsNotNull(savedCustomer);
                Assert.AreEqual(savedCustomer.CustomerID, customer.CustomerID);
            }
        }

        [TestMethod]
        public void Can_modify()
        {
            using (var testData = new NHTestData(NHTestUtil.OrdersDomainFactory.OpenSession()))
            {
                Customer customer = null;
                testData.Batch(x => customer = x.CreateCustomer());

                using (var scope = new UnitOfWorkScope())
                {
                    var savedCustomer = new NHRepository<Customer, int>().Query
                        .Where(x => x.CustomerID == customer.CustomerID)
                        .First();

                    savedCustomer.LastName = "Changed";
                    scope.Commit();
                }

                testData.Session.Refresh(customer);
                Assert.AreEqual(customer.LastName, "Changed");
            }
        }

        [TestMethod]
        public void Can_delete()
        {
            var customer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
            };
            using (var scope = new UnitOfWorkScope())
            {
                new NHRepository<Customer, int>().Add(customer);
                scope.Commit();
            }
            Assert.IsTrue(customer.CustomerID > 0);
            using (var scope = new UnitOfWorkScope())
            {
                var repository = new NHRepository<Customer, int>();
                var savedCustomer = repository.Query.Where(x => x.CustomerID == customer.CustomerID).First();
                repository.Delete(savedCustomer);
                scope.Commit();
            }

            //Making sure customer is deleted
            using (var testData = new NHTestData(NHTestUtil.OrdersDomainFactory.OpenSession()))
            {
                Customer savedCustomer = null;
                testData.Batch(x => savedCustomer = x.GetCustomerById(customer.CustomerID));
                Assert.IsNull(savedCustomer);
            }
        }

        [TestMethod]
        public void Can_detach()
        {
            using (var testData = new NHTestData(NHTestUtil.OrdersDomainFactory.OpenSession()))
            {
                Customer customer = null;
                testData.Batch(action => customer = action.CreateCustomer());

                using (var scope = new UnitOfWorkScope())
                {
                    var repository = new NHRepository<Customer, int>();
                    var savedCustomer = repository.Query
                        .Where(x => x.CustomerID == customer.CustomerID)
                        .First();

                    repository.Detach(savedCustomer);
                    savedCustomer.LastName = "Changed"; //This shouldn't be saved since the savedCustomer instance is detached.
                    scope.Commit();
                }

                testData.Session.Refresh(customer);
                Assert.AreNotEqual(customer.LastName, "Changed");
            }
        }

        [TestMethod]
        public void Can_attach()
        {
            var customer = new Customer
            {
                FirstName = "Jane",
                LastName = "Doe"
            };
            var session = NHTestUtil.OrdersDomainFactory.OpenSession();
            ITransaction transaction = session.BeginTransaction();
            session.Save(customer);
            transaction.Commit();
            session.Evict(customer); //Detching from owning session
            session.Dispose(); //Auto flush

            using (var scope = new UnitOfWorkScope())
            {
                var repository = new NHRepository<Customer,int>();
                repository.Attach(customer);
                customer.LastName = "Changed";
                scope.Commit(); //Should change since the customer was attached to repository.
            }

            using (var testData = new NHTestData(NHTestUtil.OrdersDomainFactory.OpenSession()))
            {
                Customer savedCustomer = null;
                testData.Batch(x => savedCustomer = x.GetCustomerById(customer.CustomerID));
                Assert.IsNotNull(savedCustomer);
                Assert.AreEqual(savedCustomer.LastName, "Changed");
            }
        }

        [TestMethod]
        public void Can_attach_modified_entity()
        {
            var customer = new Customer
            {
                FirstName = "Jane",
                LastName = "Doe"
            };
            var session = NHTestUtil.OrdersDomainFactory.OpenSession();
            ITransaction transaction = session.BeginTransaction();
            session.Save(customer);
            transaction.Commit();
            session.Evict(customer); //Detaching from owning session
            session.Dispose(); //Auto flush

            using (var scope = new UnitOfWorkScope())
            {
                customer.LastName = "Changed";
                var repository = new NHRepository<Customer,int>();
                repository.Attach(customer);
                scope.Commit(); //Should change since the customer was attached to repository.
            }

            using (var testData = new NHTestData(NHTestUtil.OrdersDomainFactory.OpenSession()))
            {
                Customer savedCustomer = null;
                testData.Batch(x => savedCustomer = x.GetCustomerById(customer.CustomerID));
                Assert.IsNotNull(savedCustomer);
                Assert.AreEqual(savedCustomer.LastName, "Changed");
            }
        }

        //[TestMethod]
        //public void Can_query_using_specification()
        //{
        //    using (var testData = new NHTestData(OrdersDomainFactory.OpenSession()))
        //    {
        //        testData.Batch(actions =>
        //        {
        //            actions.CreateOrdersForCustomers(actions.CreateCustomersInState("PA", 2));
        //            actions.CreateOrdersForCustomers(actions.CreateCustomersInState("DE", 5));
        //            actions.CreateOrdersForCustomers(actions.CreateCustomersInState("LA", 3));
        //        });

        //        using (new UnitOfWorkScope())
        //        {


        //            var customersInPa = new Specification<Order>(x => x.Customer.Address.State == "DE");

        //            var ordersRepository = new Repository<Order>();
        //            var results = from order in ordersRepository.Query(customersInPa) select order;

        //            Assert.That(results.Count(), Is.GreaterThan(0));
        //            Assert.That(results.Count(), Is.EqualTo(5));
        //        }
        //    }
        //}

        [TestMethod]
        public void Can_lazyload()
        {
            using (var testData = new NHTestData(NHTestUtil.OrdersDomainFactory.OpenSession()))
            {
                Customer customer = null;
                testData.Batch(x =>
                {
                    customer = x.CreateCustomer();
                    x.CreateOrderForCustomer(customer);
                });

                using (var scope = new UnitOfWorkScope())
                {
                    var savedCustomer = new NHRepository<Customer, int>().Query
                        .Where(x => x.CustomerID == customer.CustomerID)
                        .First();

                    Assert.IsNotNull(savedCustomer);
                    Assert.IsNotNull(savedCustomer.Orders);
                    Assert.IsTrue(savedCustomer.Orders.Count > 0);
                    scope.Commit();
                }
            }
        }

        [TestMethod]
        public void Lazyloading_should_not_load()
        {
            using (var testData = new NHTestData(NHTestUtil.OrdersDomainFactory.OpenSession()))
            {
                Order order = null;
                testData.Batch(x => order = x.CreateOrderForCustomer(x.CreateCustomer()));
                Order savedOrder;
                using (var scope = new UnitOfWorkScope())
                {
                    savedOrder = new NHRepository<Order, int>().Query
                        .Where(x => x.OrderID == order.OrderID)
                        .First();
                    scope.Commit();
                }
                Assert.IsNotNull(savedOrder);
                Assert.IsFalse(NHibernateUtil.IsInitialized(savedOrder.Customer));
            }
        }
        [TestMethod]
        public void Can_eager_fetch_using_fetch()
        {
            using (var tesData = new NHTestData(NHTestUtil.OrdersDomainFactory.OpenSession()))
            {
                Customer customer = null;
                tesData.Batch(x =>
                {
                    var products = x.CreateProducts(10);
                    var order = x.CreateOrderForProducts(products);
                    customer = order.Customer = x.CreateCustomer();
                });

                Customer savedCustomer;
                using (var scope = new UnitOfWorkScope())
                {
                    savedCustomer = new NHRepository<Customer,int>().Query
                        .Where(x => x.CustomerID == customer.CustomerID)
                        .FetchMany(x => x.Orders)
                        .ThenFetchMany(x=>x.Items)
                        .ThenFetch(x=>x.Product)
                        .First();
                    scope.Commit();
                }

                Assert.IsNotNull(savedCustomer);
                Assert.IsTrue(NHibernateUtil.IsInitialized(savedCustomer.Orders));
                foreach (var order in savedCustomer.Orders)
                {
                    Assert.IsTrue(NHibernateUtil.IsInitialized(order.Items));
                    foreach (var item in order.Items)
                    {
                        Assert.IsTrue(NHibernateUtil.IsInitialized(item.Product));
                    }
                }
                //((IEnumerable<Order>)(savedCustomer.Orders)).ForEach(order =>
                //{
                //    Assert.IsTrue(NHibernateUtil.IsInitialized(order.Items));
                //    ((IEnumerable<OrderItem>)(order.Items)).ForEach(item => Assert.IsTrue(NHibernateUtil.IsInitialized(item.Product)));
                //});
            }
        }
        ///SQLCE limitation: http://stackoverflow.com/questions/5153573/error-on-using-transactionscope-in-ef4-sql-compact-4
        //[TestMethod]
        //public void Can_query_multiple_databases()
        //{
        //    using (var ordersTestData = new NHTestData(NHTestUtil.OrdersDomainFactory.OpenSession()))
        //    using (var hrTestData = new NHTestData(NHTestUtil.HRDomainFactory.OpenSession()))
        //    {
        //        Customer customer = null;
        //        SalesPerson salesPerson = null;
        //        ordersTestData.Batch(x => customer = x.CreateCustomer());
        //        hrTestData.Batch(x => salesPerson = x.CreateSalesPerson());

        //        //NOTE: This will enlist a Distributed DTC tx.
        //        using (var scope = new UnitOfWorkScope())
        //        {
        //            var savedCustomer = new NHRepository<Customer, int>().Query
        //                .Where(x => x.CustomerID == customer.CustomerID)
        //                .First();
        //            var savedPerson = new NHRepository<SalesPerson, int>().Query
        //                .Where(x => x.Id == salesPerson.Id)
        //                .First();
        //            Assert.IsNotNull(savedCustomer);
        //            Assert.IsNotNull(savedPerson);
        //            scope.Commit();
        //        }
        //    }
        //}

        //[TestMethod]
        //public void Query_using_orderby_in_specification_works()
        //{
        //    using (var testData = new NHTestData(OrdersDomainFactory.OpenSession()))
        //    {
        //        testData.Batch(actions =>
        //        {
        //            actions.CreateOrdersForCustomers(actions.CreateCustomersInState("PA", 2));
        //            actions.CreateOrdersForCustomers(actions.CreateCustomersInState("DE", 5));
        //            actions.CreateOrdersForCustomers(actions.CreateCustomersInState("LA", 3));
        //        });

        //        var customersInPa = new Specification<Order>(x => x.Customer.Address.State == "PA");
        //        using (var scope = new UnitOfWorkScope())
        //        {
        //            var ordersRepository = new Repository<Order>();
        //            var results = from order in ordersRepository.Query(customersInPa)
        //                          orderby order.OrderDate
        //                          select order;

        //            Assert.That(results, Is.Not.Empty);
        //            Assert.That(results.Count(), Is.GreaterThan(0));
        //            Assert.That(results.Count(), Is.EqualTo(2));
        //            scope.Commit();
        //        }
        //    }
        //}
    }
}