using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using App.Common.TypeFinder;

namespace App.Common.Test.TypeFinder
{
    [TestClass]
    public class TypeFinderTest
    {
        [TestClass]
        public class TypeFinderTests
        {
            [TestMethod]
            public void TypeFinder_Benchmark_Findings()
            {
                var finder = new AppDomainTypeFinder();

                var type = finder.FindClassesOfType<ISomeInterface>();
                Assert.AreEqual(type.Count(),1);
                Assert.AreEqual(typeof(ISomeInterface).IsAssignableFrom(type.FirstOrDefault()), true);
            }

            public interface ISomeInterface
            {
            }

            public class SomeClass : ISomeInterface
            {
            }
        }
    }
}
