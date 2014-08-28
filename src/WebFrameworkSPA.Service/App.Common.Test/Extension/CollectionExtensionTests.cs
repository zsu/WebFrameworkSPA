using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace App.Common.Test
{
    [TestClass]
    public class CollectionExtensionTest
    {
        [TestMethod]
        public void IsNullOrEmpty_Should_Return_True_When_Collection_Is_Null()
        {
            Assert.IsTrue(((string[]) null).IsNullOrEmpty());
        }

        [TestMethod]
        public void IsNullOrEmpty_Should_Return_True_When_Collection_Is_Empty()
        {
            Assert.IsTrue((new string[]{}).IsNullOrEmpty());
        }

        [TestMethod]
        public void IsNullOrEmpty_Should_Return_False_When_Collection_Containes_Items()
        {
            Assert.IsFalse((new[] { "An item" }).IsNullOrEmpty());
        }
    }
}