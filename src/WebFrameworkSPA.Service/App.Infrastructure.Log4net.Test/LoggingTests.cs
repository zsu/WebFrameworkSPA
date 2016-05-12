using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using App.Common.Logging;
using System.Threading;
using log4net.Config;
using App.Common;
using System;

namespace App.Infrastructure.Log4net.Test
{
    [TestClass]
    public class LoggingTests {
        private TestContext testContextInstance;
        private static string _baseFilePath = Path.GetFullPath(@"Logs\");
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
        public void MyTestInitialize()
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Util.LogConfigFilePath));
        }
        
        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            log4net.LogManager.Shutdown();
            if (Directory.Exists(_baseFilePath))
                Directory.Delete(_baseFilePath,true);

            //DeleteFiles(_baseFilePath);
        }
        //
        #endregion
        [TestMethod]
        public void Debug_Should_Log_Message()
        {
            string folder = Path.GetFullPath("Logs");
            string filePattern = string.Format("Test.log", Thread.CurrentThread.ManagedThreadId);
            VerifyLogFile(null, LogLevel.Debug, filePattern, true);
        }
        
        [TestMethod]
        public void Application_Debug_Should_Not_Log_Message() {
            string folder = Path.GetFullPath("Logs");
            string filePattern = "AppEvents.log";//string.Format("log-{0}.log",Thread.CurrentThread.ManagedThreadId);
            VerifyLogFile(LogType.Application.ToString(), LogLevel.Debug, filePattern, false);
        }
        [TestMethod]
        public void Application_Info_Should_Log_Message()
        {
            string folder = Path.GetFullPath("Logs");
            string filePattern = "AppEvents.log";//string.Format("log-{0}.log",Thread.CurrentThread.ManagedThreadId);
            VerifyLogFile(LogType.Application.ToString(), LogLevel.Info, filePattern, true);
        }
        //[TestMethod]
        //public void SystemEvents_Info_Should_Log_Message()
        //{
        //    string folder = Path.GetFullPath("Logs");
        //    string filePattern = @"SystemEvents.log";
        //    VerifyLogFile(LogType.SystemEvents.ToString(), LogLevel.Info, filePattern, true);
        //}

        //[TestMethod]
        //public void SystemEvents_Debug_Should_Not_Log_Message()
        //{
        //    string folder = Path.GetFullPath("Logs");
        //    string filePattern = @"SystemEvents.log";
        //    VerifyLogFile(LogType.SystemEvents.ToString(), LogLevel.Debug, filePattern, false);
        //}

        [TestMethod]
        public void LoginAudit_Debug_Should_Not_Log_Message()
        {
            string folder = Path.GetFullPath("Logs");
            string filePattern = "Audit.log";
            VerifyLogFile(LogType.LoginAudit.ToString(), LogLevel.Debug, filePattern, false);
        }
        
        [TestMethod]
        public void LoginAudit_Info_Should_Log_Message()
        {
            string folder = Path.GetFullPath("Logs");
            string filePattern = "Audit.log";
            VerifyLogFile(LogType.LoginAudit.ToString(), LogLevel.Info, filePattern, true);
        }

        //[TestMethod]
        //public void SecurityAudit_Info_Should_Log_Message()
        //{
        //    string folder = Path.GetFullPath("Logs");
        //    string filePattern = @"SecurityAudit.log";
        //    VerifyLogFile(LogType.SecurityAudit.ToString(), LogLevel.Info, filePattern, true);
        //}

        //[TestMethod]
        //public void SecurityAudit_Debug_Should_Not_Log_Message()
        //{
        //    string folder = Path.GetFullPath("Logs");
        //    string filePattern = @"SecurityAudit.log";
        //    VerifyLogFile(LogType.SecurityAudit.ToString(), LogLevel.Debug, filePattern, false);
        //}

        //[TestMethod]
        //public void ReportUsage_Info_Should_Log_Message()
        //{
        //    string folder = Path.GetFullPath("Logs");
        //    string filePattern = @"ReportUsage.log";
        //    VerifyLogFile(LogType.ReportUsage.ToString(), LogLevel.Info, filePattern, true);
        //}

        //[TestMethod]
        //public void ReportUsage_Debug_Should_Not_Log_Message()
        //{
        //    string folder = Path.GetFullPath("Logs");
        //    string filePattern = @"ReportUsage.log";
        //    VerifyLogFile(LogType.ReportUsage.ToString(), LogLevel.Debug, filePattern, false);
        //}

        #region Private Methods
        private void VerifyLogFile(string logType, LogLevel logLevel, string filePattern, bool shouldLog)
        {
            Log4netAdapter log4netAdapter = new Log4netAdapter(logType);

            string folder = Path.GetFullPath("Logs");
            string[] matches = Directory.GetFiles(folder, filePattern);
            long originalSize = 0, newSize = 0;
            if (matches != null && matches.Length > 0)
            {
                FileInfo originalInfo = new FileInfo(matches[0]);
                originalSize = originalInfo.Length;
            }
            log4netAdapter.Log(logLevel, string.Format("{0} {1}.", logType, logLevel));

            matches = Directory.GetFiles(folder, filePattern);
            if (matches != null && matches.Length > 0)
            {
                FileInfo newInfo = new FileInfo(matches[0]);
                newSize = newInfo.Length;
            }
            if (shouldLog)
                Assert.IsTrue(newSize > originalSize);
            else
                Assert.IsTrue(newSize == originalSize);
        }
        private static string GetWildcardPatternForFile(string baseFileName)
        {
            return baseFileName + '*';
        }
        private static void DeleteFiles(string baseFilePath)
        {
            string directory = null;

            string fullPath = Path.GetFullPath(baseFilePath);

            directory = Path.GetDirectoryName(fullPath);
            if (Directory.Exists(directory))
            {
                string baseFileName = Path.GetFileName(fullPath);

                string[] files = Directory.GetFiles(directory, GetWildcardPatternForFile(baseFileName));

                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        string curFileName = Path.GetFileName(files[i]);
                        if (curFileName.StartsWith(baseFileName))
                        {
                            try
                            {
                                File.Delete(files[i]);
                            }
                            catch (Exception e)
                            {
                                string err = e.Message;
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}