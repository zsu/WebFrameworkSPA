using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SLib.Cryptography;

namespace SLib.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class TripleDesProtectedConfigurationProviderTests
    {
        private readonly string _plainText = "<connectionStrings>"+
    "<add name=\"AppDB\" connectionString=\"Database=SampleDB;Server=ServerName;User ID=UserName;Password=DBPassword;\" providerName=\"System.Data.SqlClient\" />"+
    "<add name=\"UserDB\" connectionString=\"Database=SampleDB;Server=ServerName;user id=UserName;password=DBPassword;\" providerName=\"System.Data.SqlClient\" />" +
    "</connectionStrings>";
        private TripleDesProtectedConfigurationProvider _provider = new TripleDesProtectedConfigurationProvider();

        public TripleDesProtectedConfigurationProviderTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void DecryptString_Should_Return_Plain_Text()
        {
            string encrypted = _provider.EncryptString(_plainText);
            Assert.AreEqual(_plainText,_provider.DecryptString(encrypted));
        }

        [TestMethod]
        public void Decrypt_Should_Return_Plain_Text()
        {
            string encrypted = _provider.EncryptString(_plainText);
            Assert.AreEqual(_plainText, _provider.DecryptString(encrypted));
        }
    }
}
