using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.IO;

namespace App.Common.Test
{
    /// <summary>
    /// Summary description for UtilTests
    /// </summary>
    [TestClass]
    public class UtilTests
    {
        public UtilTests()
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
        public void GetFullPath_Should_Return_Physical_FullPath()
        {
            string currentPath=Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string localPath = @"c:\Projects\App.Common.Test\Logs";
            string UNCPath = @"\\10.0.0.1\App.Common.Test\Logs";
            string relativePath = @"App.Common.Test\Logs";
            Assert.AreEqual(Util.GetFullPath(localPath).ToLower(), localPath.ToLower());
            Assert.AreEqual(Util.GetFullPath(UNCPath.ToLower()),UNCPath.ToLower());
            Assert.AreEqual(Util.GetFullPath(relativePath).ToLower(),Path.Combine(currentPath,relativePath).ToLower());
        }
    }
}
