using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace App.Common.Test
{
    [TestClass]
    public class EnumerableExtensionTest
    {
        [TestMethod]
        public void ForEach_Should_Call_The_Specified_Method_Each_Item()
        {
            var array = new[] { "Item 1", "Item2" };
            var called = new[] { false, false };

            array.ForEach(i => called[Array.IndexOf(array, i)] = true);

            Assert.IsTrue(called.All(c => c));
        }
    }
}