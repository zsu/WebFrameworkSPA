using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net.Config;
using System.IO;
using App.Common;
using App.Infrastructure.EntityFramework.Test.OrdersDomain.Domain;
namespace App.Infrastructure.EntityFramework.Test
{
    [TestClass]
    public class EFRepositoryExtensionsTest
    {
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
        public void Fetch_returns_a_fetching_repository_with_correct_path()
        {
            var repository = new EFRepository<Order,int>();
            var fetchingRepo = repository.Fetch(order => order.Customer);
            Assert.AreEqual("Customer", fetchingRepo.FetchingPath);
        }

        [TestMethod]
        public void FetchMany_returns_a_fetching_repository_with_corrent_path()
        {
            var repository = new EFRepository<Customer,int>();
            var fetchingRepo = repository.FetchMany(customer => customer.Orders);
            Assert.AreEqual("Orders", fetchingRepo.FetchingPath);
        }

        [TestMethod]
        public void Can_fectch_many_on_association()
        {
            var repository = new EFRepository<Order,int>();
            var fetchingRepo = repository
                .Fetch(order => order.Customer)
                .ThenFetchMany(customer => customer.Orders);

            Assert.AreEqual("Customer.Orders", fetchingRepo.FetchingPath);
        }

        [TestMethod]
        public void Can_fetch_after_a_many_fetch()
        {
            var repository = new EFRepository<Order,int>();
            var fetchingRepo = repository
                .FetchMany(x => x.OrderItems)
                .ThenFetch(x => x.Product);
            Assert.AreEqual("OrderItems.Product", fetchingRepo.FetchingPath);
        }
    }
}