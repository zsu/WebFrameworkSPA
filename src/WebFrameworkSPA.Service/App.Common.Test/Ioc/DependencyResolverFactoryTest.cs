using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using App.Common.InversionOfControl;

namespace App.Common.Test
{
    [TestClass]
    public class DependencyResolverFactoryTest
    {
        private readonly ICustomDependencyResolver _resolver;

        public DependencyResolverFactoryTest()
        {
            var type = typeof(DependencyResolverTestDouble);
            var typeName = string.Format("{0}, {1}", type.FullName, type.Assembly.FullName);

            var factory = new DependencyResolverFactory(typeName);

            _resolver = factory.CreateInstance();
        }

        [TestMethod]
        public void CreateInstance_Should_Return_New_Resolver()
        {
            Assert.IsNotNull(_resolver);
        }

        [TestMethod]
        public void CreateInstance_Should_Return_Correct_Resolver_Type()
        {
            AssertExtension.IsType<DependencyResolverTestDouble>(_resolver);
        }

        [TestMethod]
        public void Constructor_Should_Throw_Exception_When_Config_File_Is_Missing()
        {
            AssertExtension.Throws<ArgumentException>(() => new DependencyResolverFactory());
        }
    }
}