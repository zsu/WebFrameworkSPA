//using System;
//using System.Text;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Threading;
//using log4net.Repository;
//using log4net.Config;
//using System.Collections;
//using System.IO;
//using log4net.Util;
//using log4net;
//using App.Common;

//namespace App.Infrastructure.Log4net.Test
//{
//    /// <summary>
//    /// Summary description for PatternRollingFileAppenderTest
//    /// </summary>
//    [TestClass]
//    public class PatternRollingFileAppenderTest
//    {
//        private const int _threadCount = 50;

//        private readonly ManualResetEvent _startEvent = new ManualResetEvent(false);
//        private readonly ManualResetEvent _stopEvent = new ManualResetEvent(false);

//        private static int _preLogCounter = 0;
//        private static int _postLogCounter = 0;

//        private static string _baseFilePath = Path.GetFullPath(@"Logs\Log-");

//        private static ILog _log = LogManager.GetLogger(typeof(PatternRollingFileAppenderTest));
//        public PatternRollingFileAppenderTest()
//        {
//            //
//            // TODO: Add constructor logic here
//            //
//        }

//        private TestContext testContextInstance;

//        /// <summary>
//        ///Gets or sets the test context which provides
//        ///information about and functionality for the current test run.
//        ///</summary>
//        public TestContext TestContext
//        {
//            get
//            {
//                return testContextInstance;
//            }
//            set
//            {
//                testContextInstance = value;
//            }
//        }

//        #region Additional test attributes
//        //
//        // You can use the following additional attributes as you write your tests:
//        //
//        // Use ClassInitialize to run code before running the first test in the class
//        [ClassInitialize()]
//        public static void MyClassInitialize(TestContext testContext)
//        {
//            XmlConfigurator.ConfigureAndWatch(new FileInfo(Util.LogConfigFilePath));
//        }
//        //
//        // Use ClassCleanup to run code after all tests in a class have run
//        [ClassCleanup()]
//        public static void MyClassCleanup() { log4net.LogManager.Shutdown(); }
//        //
//        // Use TestInitialize to run code before running each test 
//        [TestInitialize()]
//        public void MyTestInitialize() {
//            DeleteFiles(_baseFilePath);
//        }
//        //
//        // Use TestCleanup to run code after each test has run
//        [TestCleanup()]
//        public void MyTestCleanup() {
//            DeleteFiles(_baseFilePath);
//        }
//        //
//        #endregion

//        [TestMethod]
//        public void ShouldGenerateOneLogFilePerThread()
//        {
//            Thread[] threads = new Thread[_threadCount];

//            // idea borrowed from Castle.Facilities.IBatisNetIntegration.Tests.DaoTestCase
//            for (int i = 0; i < _threadCount; i++)
//            {
//                threads[i] = new Thread(executionMethodUntilSignal);
//                threads[i].Name = i.ToString();
//                threads[i].Start();
//            }

//            // start the threads!
//            _startEvent.Set();

//            // let them run for 30 seconds
//            Thread.CurrentThread.Join(30000);

//            // trigger the stop event
//            _stopEvent.Set();

//            // these may be different ???
//            // int unjoinedPreLogCounter = preLogCounter;
//            // int unjoinedPostLogCounter = postLogCounter;

//            // give each thread 1 second to finish up
//            for (int i = 0; i < _threadCount; i++)
//            {
//                if (threads[i].Join(1000) == false)
//                {
//                    Assert.Fail("Thread {0} did not have enough time to finish", threads[i].Name);
//                }
//            }

//            // ensure that our log statement was called
//            Assert.AreEqual(_preLogCounter, _postLogCounter);

//            ArrayList logFiles = GetExistingFiles(_baseFilePath);
//            Assert.AreEqual(logFiles.Count, _threadCount);
//        }

//        private void executionMethodUntilSignal()
//        {
//            log4net.ThreadContext.Properties["SessionId"] = Thread.CurrentThread.Name; 
//            Console.WriteLine("Starting thread {0}", Thread.CurrentThread.Name);

//            // do not start until startEvent.Set() has been called
//            _startEvent.WaitOne(int.MaxValue, false);
//            int count = 0;
//            while (!_stopEvent.WaitOne(1, false))
//            {
//                Interlocked.Increment(ref _preLogCounter);
//                _log.Debug(count++);
//                Interlocked.Increment(ref _postLogCounter);
//            }

//            Console.WriteLine("Ending thread {0}", Thread.CurrentThread.Name);
//        }

//        /// <summary>
//        /// Builds a list of filenames for all files matching the base filename plus a file
//        /// pattern.
//        /// </summary>
//        /// <param name="baseFilePath"></param>
//        /// <returns></returns>
//        private ArrayList GetExistingFiles(string baseFilePath)
//        {
//            ArrayList alFiles = new ArrayList();

//            string directory = null;

//            string fullPath = Path.GetFullPath(baseFilePath);

//            directory = Path.GetDirectoryName(fullPath);
//            if (Directory.Exists(directory))
//            {
//                string baseFileName = Path.GetFileName(fullPath);

//                string[] files = Directory.GetFiles(directory, GetWildcardPatternForFile(baseFileName));

//                if (files != null)
//                {
//                    for (int i = 0; i < files.Length; i++)
//                    {
//                        string curFileName = Path.GetFileName(files[i]);
//                        if (curFileName.StartsWith(baseFileName))
//                        {
//                            alFiles.Add(curFileName);
//                        }
//                    }
//                }
//            }
//            return alFiles;
//        }
//        /// <summary>
//        /// Generates a wildcard pattern that can be used to find all files
//        /// that are similar to the base file name.
//        /// </summary>
//        /// <param name="baseFileName"></param>
//        /// <returns></returns>
//        private static string GetWildcardPatternForFile(string baseFileName)
//        {
//            return baseFileName + '*';
//        }

//        private static void DeleteFiles(string baseFilePath)
//        {
//            string directory = null;

//            string fullPath = Path.GetFullPath(baseFilePath);

//            directory = Path.GetDirectoryName(fullPath);
//            if (Directory.Exists(directory))
//            {
//                string baseFileName = Path.GetFileName(fullPath);

//                string[] files = Directory.GetFiles(directory, GetWildcardPatternForFile(baseFileName));

//                if (files != null)
//                {
//                    for (int i = 0; i < files.Length; i++)
//                    {
//                        string curFileName = Path.GetFileName(files[i]);
//                        if (curFileName.StartsWith(baseFileName))
//                        {
//                            try
//                            {
//                                File.Delete(files[i]);
//                            }
//                            catch (Exception e)
//                            {
//                                string err = e.Message;
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }
//}
