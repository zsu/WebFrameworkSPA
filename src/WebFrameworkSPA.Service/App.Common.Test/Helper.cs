using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace App.Common.Test
{
    internal class Helper
    {
        public delegate R ExtensionDelegate<T, R>(T target);
        public static void TestInlineData<T, R>(T[] targets, R[] results, ExtensionDelegate<T,R> code)
        {
            if (targets.Length != results.Length)
                Assert.Fail("Test data error.");
            for (int i = 0; i < targets.Length; i++)
            {
                Assert.AreEqual(results[i], code(targets[i]));
            }
        }
    }

    public delegate void TestDelegate();
    public delegate R TestDelegateWithReturn<R>();

    public static class AssertExtension
    {
        [DebuggerStepThrough]
        public static void DoesNotThrow(TestDelegate code)
        {
            DoesNotThrow(code, null, null);
        }

        [DebuggerStepThrough]
        public static void DoesNotThrow(TestDelegate code, string message)
        {
            DoesNotThrow(code, message, null);
        }

        [DebuggerStepThrough]
        public static void DoesNotThrow(TestDelegate code, string message, params object[] args)
        {
            try
            {
                code();
            }
            catch (Exception exception)
            {
                Assert.Fail("{0}{1}Unexpected exception: {2}", message, Environment.NewLine, exception.GetType(), args);
            }
        }


        [DebuggerStepThrough]
        public static void Throws<T>(TestDelegate code)
        {
            Throws<T>(code, null, null);
        }

        [DebuggerStepThrough]
        public static void Throws<T>(TestDelegate code, string message, params object[] args)
        {
            try
            {
                code();
            }
            catch (Exception exception)
            {
                if (!(exception is T))
                    Assert.Fail("{0}{1}Unexpected exception: {2}", message, Environment.NewLine, exception.GetType(), args);
            }
        }

        [DebuggerStepThrough]
        public static void Throws<R, E>(TestDelegateWithReturn<R> code)
        {
            Throws<R, E>(code, null, null);
        }
        
        [DebuggerStepThrough]
        public static void Throws<R,E>(TestDelegateWithReturn<R> code, string message, params object[] args)
        {
            try
            {
                code();
            }
            catch (Exception exception)
            {
                if (!(exception is E))
                    Assert.Fail("{0}{1}Unexpected exception: {2}", message, Environment.NewLine, exception.GetType(), args);
            }
        }

        [DebuggerStepThrough]
        public static void IsType<T>(object @object)
        {
            IsType(typeof(T), @object);
        }

        [DebuggerStepThrough]
        public static void IsType(Type expectedType, object target)
        {
            if ((target == null) || !expectedType.Equals(target.GetType()))
            {
                Assert.Fail(string.Format("Target is not type of {0}.",expectedType.Name));
            }
        }
    }

}
