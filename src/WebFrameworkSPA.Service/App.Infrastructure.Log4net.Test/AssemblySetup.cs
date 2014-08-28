using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net.Config;
using System;
using System.Reflection;
using System.Text;
using App.Common;

namespace App.Infrastructure.Log4net.Test {
    [TestClass()]
    public class AssemblySetup {
        //[AssemblyInitialize()]
        //public static void AssemblyInit(TestContext context)
        //{
        //    InitializeLog4Net();
        //}

        //private static void InitializeLog4Net() {
        //    XmlConfigurator.ConfigureAndWatch(new FileInfo(Util.GetFullPath(Util.LogConfigFilePath)));
        //}
    }
}