﻿using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace App.Infrastructure.NHibernate.Test
{
    [DebuggerStepThrough]
    [DebuggerNonUserCode]
    public static class ExceptionAssert
    {
        public static void Throws<T>(Action task, string expectedMessage, ExceptionMessageCompareOptions options) where T : Exception
        {
            try
            {
                task();
            }
            catch (Exception ex)
            {
                AssertExceptionType<T>(ex);
                AssertExceptionMessage(ex, expectedMessage, options);
                return;
            }

            if (typeof(T).Equals(new Exception().GetType()))
            {
                Assert.Fail("Expected exception but no exception was thrown.");
            }
            else
            {
                Assert.Fail(string.Format("Expected exception of type {0} but no exception was thrown.", typeof(T)));
            }
        }

        #region Overloaded methods

        public static void Throws<T>(this IAssertion assertion, Action task) where T : Exception
        {
            Throws<T>(task, null, ExceptionMessageCompareOptions.None);
        }

        public static void Throws<T>(Action task) where T : Exception
        {
            Throws<T>(task, null, ExceptionMessageCompareOptions.None);
        }

        public static void Throws<T>(this IAssertion assertion, Action task, string expectedMessage) where T : Exception
        {
            Throws<T>(task, expectedMessage, ExceptionMessageCompareOptions.Exact);
        }

        public static void Throws<T>(Action task, string expectedMessage) where T : Exception
        {
            Throws<T>(task, expectedMessage, ExceptionMessageCompareOptions.Exact);
        }

        public static void Throws<T>(this IAssertion assertion, Action task, string expectedMessage, ExceptionMessageCompareOptions options) where T : Exception
        {
            Throws<T>(task, expectedMessage, options);
        }

        public static void Throws(this IAssertion assertion, Action task, string expectedMessage, ExceptionMessageCompareOptions options)
        {
            Throws<Exception>(task, expectedMessage, options);
        }

        public static void Throws(Action task, string expectedMessage, ExceptionMessageCompareOptions options)
        {
            Throws<Exception>(task, expectedMessage, options);
        }

        public static void Throws(this IAssertion assertion, Action task, string expectedMessage)
        {
            Throws<Exception>(task, expectedMessage, ExceptionMessageCompareOptions.Exact);
        }

        public static void Throws(Action task, string expectedMessage)
        {
            Throws<Exception>(task, expectedMessage, ExceptionMessageCompareOptions.Exact);
        }

        public static void Throws(this IAssertion assertion, Action task)
        {
            Throws<Exception>(task, null, ExceptionMessageCompareOptions.None);
        }

        public static void Throws(Action task)
        {
            Throws<Exception>(task, null, ExceptionMessageCompareOptions.None);
        }

        #endregion

        private static void AssertExceptionType<T>(Exception ex)
        {
            Assert.IsInstanceOfType(ex, typeof(T), "Expected exception type failed.");
        }

        private static void AssertExceptionMessage(Exception ex, string expectedMessage, ExceptionMessageCompareOptions options)
        {
            if (!string.IsNullOrEmpty(expectedMessage))
            {
                switch (options)
                {
                    case ExceptionMessageCompareOptions.Exact:
                        Assert.AreEqual(ex.Message.ToUpper(), expectedMessage.ToUpper(), "Expected exception message failed.");
                        break;
                    case ExceptionMessageCompareOptions.Contains:
                        Assert.IsTrue(ex.Message.Contains(expectedMessage), string.Format("Expected exception message does not contain <{0}>.", expectedMessage));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("options");
                }

            }
        }
    }
    public enum ExceptionMessageCompareOptions
    {
        None,
        Exact,
        Contains
    }
}
