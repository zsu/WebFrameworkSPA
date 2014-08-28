using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace App.Common.Test
{
    [TestClass]
    public class GuidExtensionTest
    {
        [TestMethod]
        public void Shrink_Should_Always_Return_Twenty_Two_Character_String()
        {
            string[] targets = new string[] {
                "CD31A824-7686-4774-9143-3FC3ED79510F",
                "{909F4A0A-805A-4dea-BF20-C35E26A4089C}"
            };
            foreach(string target in targets)
                Assert.AreEqual(22, new Guid(target).Shrink().Length);
        }

        [TestMethod]
        public void IsEmpty_Should_Return_True_For_Empty_Guid()
        {
            Assert.IsTrue(Guid.Empty.IsEmpty());
        }
    }
}