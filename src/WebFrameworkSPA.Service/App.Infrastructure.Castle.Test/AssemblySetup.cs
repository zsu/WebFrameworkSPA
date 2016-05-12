using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace App.Infrastructure.Castle.Test
{
    [TestClass()]
    public class AssemblySetup {
        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            //InitializeLog4Net();
            InitalizeServiceLocator();
        }

        private static void InitalizeServiceLocator() {
            ServiceLocatorInitializer.Init();
        }

        //private static void InitializeLog4Net() {
        //    XmlConfigurator.ConfigureAndWatch(new FileInfo(Util.LogConfigFilePath));
        //}
    }
}