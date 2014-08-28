using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using App.Infrastructure.Castle.Test;
using log4net.Config;
using System.IO;
using App.Common;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;
using System.Collections;
using App.Common.InversionOfControl;

namespace App.Infrastructure.Castle.Logging.Test
{
    [TestClass]
    public class ExceptionHandlerAttributeTests {
        private string logFilePath = string.Format(@"Logs/Log-{0}.log", Thread.CurrentThread.ManagedThreadId);
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
        #region Setup/Teardown
        [TestInitialize()]
        public void TestInitialize()
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Util.LogConfigFilePath));
            exceptionHandlerTestClass = IoC.GetService<IExceptionHandlerTestClass>();
       }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void TestCleanup()
        {
            log4net.LogManager.Shutdown();
        }
        #endregion

        private IExceptionHandlerTestClass exceptionHandlerTestClass;

        [TestMethod]
        public void LoggedExceptionDoesNotRethrow() {
            AssertExtension.DoesNotThrow(() => exceptionHandlerTestClass.ThrowExceptionSilent());
        }

        [TestMethod]
        public void LoggedExceptionDoesNotRethrowWithReturn() {
            Assert.AreEqual(exceptionHandlerTestClass.ThrowExceptionSilentWithReturn(),6f);
        }

        [TestMethod]
        public void LoggedExceptionDoesNotRethrowWithReturnWithLogAttribute() {
           Assert.AreEqual(exceptionHandlerTestClass.ThrowExceptionSilentWithReturnWithLogAttribute(),6f);
        }

        [TestMethod]
        public void LoggedExceptionRethrows() {
            AssertExtension.Throws<NotImplementedException>(() => exceptionHandlerTestClass.ThrowException());
        }

        [TestMethod]
        public void ThrowBaseExceptionNoCatch()
        {
            AssertExtension.Throws<Exception>(() => exceptionHandlerTestClass.ThrowBaseExceptionNoCatch());
        }

        [TestMethod]
        public void ThrowNotImplementedExceptionCatch()
        {
            Assert.AreEqual(exceptionHandlerTestClass.ThrowNotImplementedExceptionCatch(),6f);
        }

        [TestMethod]
        public void SkipPropertyShouldNotLog()
        {
            Hashtable htRoles=new Hashtable();
            htRoles.Add(2,"Supervisor");
            htRoles.Add(1,"Scorer");
            try
            {
                exceptionHandlerTestClass.SkipPropertyShouldNotLog("user1", "password1", "senssitiveData1", new List<string> { "Admin", "Manager" },
                    htRoles, new List<Client> { new Client { Id = 1, Name = "client1" }, new Client { Id = 2, Name = "Client2" } });
            }
            catch
            {
            }
            Assert.IsTrue(VerifyLogContent(logFilePath, "password1", false));
        }

        private bool VerifyLogContent(string filePath, string pattern, bool shouldLog)
        {
            bool match=false;
            if (filePath == null || filePath.Trim() == string.Empty || pattern == null || pattern.Trim() == string.Empty)
                return true;
            if (File.Exists(filePath))
            {
                string fileString = File.ReadAllText(filePath);
                if (fileString != null && fileString.Trim() != string.Empty && Regex.Match(fileString, pattern, RegexOptions.IgnoreCase).Success)
                    match=true;
            }
            return (shouldLog && match) || (!shouldLog && !match);
        }
    }
}