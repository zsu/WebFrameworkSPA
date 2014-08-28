using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace App.Common.Test
{
    [TestClass]
    public class DateTimeExtensionTest
    {
        [TestMethod]
        public void IsValid_Should_Return_False_When_Specified_Date_Is_Smaller_Than_MinDate()
        {
            Assert.IsFalse(DateTime.MinValue.IsValid());
        }

        [TestMethod]
        public void IsValid_Should_Return_False_When_Specified_Date_Is_Greater_Than_MaxDate()
        {
            Assert.IsFalse(DateTime.MaxValue.IsValid());
        }

        [TestMethod]
        public void IsValid_Should_Return_True_When_Specified_Date_Is_Between_MinDate_And_MaxDate()
        {
            Assert.IsTrue(SystemTime.Now().IsValid());
        }
    }
}