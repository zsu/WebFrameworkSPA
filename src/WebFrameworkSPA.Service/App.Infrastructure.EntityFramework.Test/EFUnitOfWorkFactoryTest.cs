using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using App.Infrastructure.EntityFramework.Test.OrdersDomain;
using System.Data.Entity.Infrastructure;

namespace App.Infrastructure.EntityFramework.Test
{
    [TestClass]
    public class EFUnitOfWorkFactoryTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Create_Throws_InvalidOperationException_When_No_SessionFactoryProvider_Has_Been_Set()
        {
            var factory = new EFUnitOfWorkFactory();
            factory.Create();
        }

        [TestMethod]
        public void Create_Returns_NHUnitOfWork_Instance_When_SessionFactoryProvider_Has_Been_Set()
        {
            var factory = new EFUnitOfWorkFactory();
            factory.RegisterObjectContextProvider(() => ((IObjectContextAdapter)new OrdersDomainContext()).ObjectContext);
            var uowInstance = factory.Create();

            Assert.IsNotNull(uowInstance);
            Assert.IsInstanceOfType(uowInstance, typeof(EFUnitOfWork));
        }
    }
}
