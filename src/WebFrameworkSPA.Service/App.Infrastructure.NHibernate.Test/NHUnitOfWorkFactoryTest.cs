using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NHibernate;
using Moq;
using NHibernate.Metadata;
using System.Collections.Generic;

namespace App.Infrastructure.NHibernate.Test
{


    /// <summary>
    ///This is a test class for NHUnitOfWorkFactoryTest and is intended
    ///to contain all NHUnitOfWorkFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class NHUnitOfWorkFactoryTest
    {


        private TestContext testContextInstance;

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
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
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
        [ExpectedException(typeof(InvalidOperationException))]
        public void Create_Throws_InvalidOperationException_When_No_SessionFactoryProvider_Has_Been_Set()
        {
            var factory = new NHUnitOfWorkFactory();
            factory.Create();
        }

        [TestMethod]
        public void Create_Returns_NHUnitOfWork_Instance_When_SessionFactoryProvider_Has_Been_Set()
        {
            var factory = new NHUnitOfWorkFactory();
            var sessionFactory = new Mock<ISessionFactory>();
            var dict = new Mock<IDictionary<string, IClassMetadata>>();
            //dict.Setup(x => x.ContainsKey(It.IsAny<string>())).Returns(false);
            sessionFactory.Setup(x => x.GetAllClassMetadata()).Returns(dict.Object);
            factory.RegisterSessionFactoryProvider(() => sessionFactory.Object);
            var uowInstance = factory.Create();

            Assert.IsNotNull(uowInstance);
            Assert.IsInstanceOfType(uowInstance, typeof(NHUnitOfWork));
        }
    }
}
