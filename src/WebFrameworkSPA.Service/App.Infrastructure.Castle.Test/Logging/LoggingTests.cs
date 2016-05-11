using System.IO;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using log4net.Config;
using App.Common;
using App.Common.Logging;
using System.Threading;
using App.Common.Attributes;
using App.Common.InversionOfControl;
using System.Collections;
using System.Collections.Generic;
using App.Infrastructure.Castle.Test;

namespace App.Infrastructure.Castle.Logging.Test
{
    [TestClass]
    public class LoggingTests
    {
        private string logFilePath = @"Logs/Test.log"; //string.Format(@"Logs/Log-{0}.log", Thread.CurrentThread.ManagedThreadId);
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
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        // Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void TestInitialize()
        {
            if (File.Exists(Path.GetFullPath(logFilePath)))
                File.Delete(Path.GetFullPath(logFilePath));
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Util.LogConfigFilePath));
        }
        
        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void TestCleanup()
        {
            log4net.LogManager.Shutdown();
        }
        //
        #endregion
        
        public class TestLogger2
        {
            [Log]
            public string GetMessage(string message)
            {
                return message;
            }

            [Log(EntryLevel = LogLevel.Info)]
            public virtual string GetMessageVirtual(string message)
            {
                return message;
            }

            public virtual string GetMessageNotLogged(string message)
            {
                return message;
            }
        }

        [TestMethod]
        public void Attribute_With_Default_Values_Should_Not_Log()
        {
            string filePath = Path.GetFullPath(logFilePath);
            VerifyFileSize(LogAttributeWithDefaultValues, filePath, false);
        }
        [TestMethod]
        public void Attribute_With_Custom_Values_For_Virtual_Method_Should_Log()
        {
            string filePath = Path.GetFullPath(logFilePath);
            VerifyFileSize(LogAttributeWithCustomValuesForVirtualMethod, filePath, true);
        }
        [TestMethod]
        public void Attribute_With_Off_Values_Should_Not_Log()
        {
            string filePath = Path.GetFullPath(logFilePath);
            VerifyFileSize(LogAttributeWithOffValues, filePath, false);
        }
        [TestMethod]
        public void No_Log_Attribute_Should_Not_Log()
        {
            string filePath = Path.GetFullPath(logFilePath);
            VerifyFileSize(NoLogAttribute, filePath, false);
        }

        [TestMethod]
        public void Attribute_With_Default_Values_Via_Proxy_Should_Not_Log()
        {
            string filePath = Path.GetFullPath(logFilePath);
            VerifyFileSize(LogAttributeWithDefaultValuesViaProxy, filePath, false);
        }
        [TestMethod]
        public void Attribute_With_Custom_Values_For_Virtual_Method_Via_Proxy_Should_Log()
        {
            string filePath = Path.GetFullPath(logFilePath);
            VerifyFileSize(LogAttributeWithCustomValuesForVirtualMethodViaProxy, filePath, true);
        }
        [TestMethod]
        public void No_Log_Attribute_Via_Proxy_Should_Not_Log()
        {
            string filePath = Path.GetFullPath(logFilePath);
            VerifyFileSize(NoLogAttributeViaProxy, filePath, false);
        }
        [TestMethod]
        public void Attribute_With_Overide_Values_For_Concrete_Type_Is_Be_Intercepted()
        {
            string filePath = Path.GetFullPath(logFilePath);
            VerifyFileSize(LogAttributeWithOverideValuesForConcreteType, filePath, false);
        }
        [TestMethod]
        public void Attribute_With_Overide_Values_For_Virtual_Method_Should_Log()
        {
            string filePath = Path.GetFullPath(logFilePath);
            VerifyFileSize(LogAttributeWithOverideValuesForVirtualMethod, filePath, true);
        }
        [TestMethod]
        public void Attribute_With_Overide_Off_Values_For_Virtual_Method_Should_Not_Log()
        {
            string filePath = Path.GetFullPath(logFilePath);
            VerifyFileSize(LogAttributeWithOverideOffValuesForVirtualMethod, filePath, false);
        }
        [TestMethod]
        public void Attribute_At_Class_Level_For_Virtual_Method_Should_Log()
        {
            string filePath = Path.GetFullPath(logFilePath);
            VerifyFileSize(LogAttributeAtClassLevel, filePath, true);
        }
        [TestMethod]
        public void SkipArgumentsShouldNotLog()
        {
            string filePath = Path.GetFullPath(logFilePath);
            Hashtable htRoles = new Hashtable();
            htRoles.Add(2, "Supervisor");
            htRoles.Add(1, "Scorer");
            try
            {
                var testClass = IoC.GetService<ILogTestClass>();
                testClass.SkipPropertyShouldNotLog("user1", "password1", "senssitiveData1", new List<string> { "Admin", "Manager" },
                    htRoles, new List<Client> { new Client { Id = 1, Name = "client1" }, new Client { Id = 2, Name = "Client2" } });
            }
            catch
            {
            }
            Assert.IsTrue(TestUtil.VerifyLogContent(filePath, "password1", false));
        }
        #region Private Methods
        private void LogAttributeWithDefaultValues()
        {
            var testClass = IoC.GetService<ILogTestClass>();
            testClass.Method("Tom", 1);
        }

        private void LogAttributeWithCustomValuesForVirtualMethod()
        {
            var testClass = IoC.GetService<ILogTestClass>();
            testClass.VirtualMethod("Bill", 2);
        }

        private void NoLogAttribute()
        {
            var testClass = IoC.GetService<ILogTestClass>();
            testClass.NotLogged("Philly", 3);
        }
        private void LogAttributeWithDefaultValuesViaProxy()
        {
            var generator = new ProxyGenerator();
            var testLogger2 =
                generator.CreateClassProxy<TestLogger2>(
                    IoC.GetService<IInterceptor>(typeof(LogInterceptor), typeof(LogInterceptor).FullName));
            testLogger2.GetMessage("message1");
        }

        private void LogAttributeWithCustomValuesForVirtualMethodViaProxy()
        {
            var generator = new ProxyGenerator();
            var testLogger2 =
                generator.CreateClassProxy<TestLogger2>(
                    IoC.GetService<IInterceptor>(typeof(IInterceptor), typeof(LogInterceptor).FullName));
            testLogger2.GetMessageVirtual("message2");
        }

        private void NoLogAttributeViaProxy()
        {
            var generator = new ProxyGenerator();
            var testLogger2 =
                generator.CreateClassProxy<TestLogger2>(
                    IoC.GetService<IInterceptor>(typeof(IInterceptor), typeof(LogInterceptor).FullName));
            testLogger2.GetMessageNotLogged("message3");
        }

        private void LogAttributeWithOverideValuesForConcreteType()
        {
            var testLogger = IoC.GetService<LogTestClass2>(); 
            testLogger.GetMessage("message1");
        }

        private void LogAttributeWithOverideValuesForVirtualMethod()
        {
            var testLogger = IoC.GetService<LogTestClass2>();
            testLogger.VirtualMethod("message2");
        }
        private void LogAttributeWithOverideOffValuesForVirtualMethod()
        {
            var testLogger = IoC.GetService<LogTestClass2>();
            testLogger.LogAttributeWithOverideOffValues();
        }
        private void LogAttributeAtClassLevel()
        {
            var testLogger = IoC.GetService<LogTestClass2>(); 
            testLogger.GetMessageWithoutAttributes("message3");
        }
        private void LogAttributeWithOffValues()
        {
            var testLogger = IoC.GetService<ILogTestClass>();
            testLogger.LogAttributeWithOffValues();
        }
        private void VerifyFileSize(Action action, string filePath, bool shouldLog)
        {
            long originalLength = 0, newLength = 0;
            if (File.Exists(filePath))
            {
                FileInfo originalInfo = new FileInfo(filePath);
                originalLength = originalInfo.Length;
            }
            action();
            if (File.Exists(filePath))
            {
                FileInfo newInfo = new FileInfo(filePath);
                newLength = newInfo.Length;
            }
            if (shouldLog)
                Assert.IsTrue(newLength > originalLength);
            else
                Assert.IsTrue(newLength == originalLength);
        }
        #endregion
    }
}