using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.IO;

namespace App.Infrastructure.Castle.Test
{
    public delegate void TestDelegate();

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
    }
    internal class TestUtil
    {
        internal static bool VerifyLogContent(string filePath, string pattern, bool shouldLog)
        {
            bool match = false;
            if (filePath == null || filePath.Trim() == string.Empty || pattern == null || pattern.Trim() == string.Empty)
                return true;
            if (File.Exists(filePath))
            {
                string fileString = null;
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var textReader = new StreamReader(fileStream))
                {
                    fileString = textReader.ReadToEnd();
                }
                if (fileString != null && fileString.Trim() != string.Empty && Regex.Match(fileString, pattern, RegexOptions.IgnoreCase).Success)
                    match = true;
            }
            return (shouldLog && match) || (!shouldLog && !match);
        }
    }

}
