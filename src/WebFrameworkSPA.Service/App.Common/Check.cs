namespace App.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public static class Check
    {
        /// <summary>
        /// Throws an exception of type <typeparamref name="TException"/> with the specified message
        /// when the assertion statement is false.
        /// </summary>
        /// <typeparam name="TException">The type of exception to throw.</typeparam>
        /// <param name="assertion">The assertion to evaluate. If false then the <typeparamref name="TException"/> exception is thrown.</param>
        /// <param name="message">string. The exception message to throw.</param>
        [DebuggerStepThrough]
        public static void Assert<TException>(bool assertion, string message) where TException : Exception
        {
            if (!assertion)
                throw (TException)Activator.CreateInstance(typeof(TException), message);
        }

        /// <summary>
        /// Throws an exception of type <typeparamref name="TException"/> with the specified message
        /// when the assertion statement is false.
        /// </summary>
        /// <typeparam name="TException">The type of exception to throw.</typeparam>
        /// <param name="assertion">The assertion to evaluate. If false then the <typeparamref name="TException"/> exception is thrown.</param>
        /// <param name="message">string. The exception message to throw.</param>
        [DebuggerStepThrough]
        public static void Assert<TException>(Func<bool> assertion, string message) where TException : Exception
        {
            //Execute the lambda and if it evaluates to true then throw the exception.
            if (!assertion())
                throw (TException)Activator.CreateInstance(typeof(TException), message);
        }

        [DebuggerStepThrough]
        public static void IsNotEmpty(Guid argument, string argumentName)
        {
            if (argument == Guid.Empty)
            {
                throw new ArgumentException(AppCommon.Argument_Cannot_Be_Empty_Guid.FormatWith(argumentName), argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void IsNotEmpty(string argument, string argumentName)
        {
            if (string.IsNullOrEmpty((argument ?? string.Empty).Trim()))
            {
                throw new ArgumentException(AppCommon.Argument_Cannot_Be_Empty.FormatWith(argumentName), argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void IsNotOutOfLength(string argument, int length, string argumentName)
        {
            if (argument.Trim().Length > length)
            {
                throw new ArgumentException(AppCommon.Argument_Out_of_Length.FormatWith(argumentName, length), argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void IsNotNull(object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void IsNotNegative(int argument, string argumentName)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void IsNotNegativeOrZero(int argument, string argumentName)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void IsNotNegative(long argument, string argumentName)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void IsNotNegativeOrZero(long argument, string argumentName)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void IsNotNegative(float argument, string argumentName)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void IsNotNegativeOrZero(float argument, string argumentName)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void IsNotNegative(decimal argument, string argumentName)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void IsNotNegativeOrZero(decimal argument, string argumentName)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void IsNotInvalidDate(DateTime argument, string argumentName)
        {
            if (!argument.IsValid())
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void IsNotInPast(DateTime argument, string argumentName)
        {
            if (argument < SystemTime.Now())
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void IsNotInFuture(DateTime argument, string argumentName)
        {
            if (argument > SystemTime.Now())
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void IsNotNegative(TimeSpan argument, string argumentName)
        {
            if (argument < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void IsNotNegativeOrZero(TimeSpan argument, string argumentName)
        {
            if (argument <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void IsNotEmpty<T>(ICollection<T> argument, string argumentName)
        {
            IsNotNull(argument, argumentName);

            if (argument.Count == 0)
            {
                throw new ArgumentException(AppCommon.Collection_Cannot_Be_Empty, argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void IsNotOutOfRange(int argument, int min, int max, string argumentName)
        {
            if ((argument < min) || (argument > max))
            {
                throw new ArgumentOutOfRangeException(argumentName, AppCommon.Length_Must_Be_In_Range.FormatWith(argumentName, min, max));
            }
        }

        [DebuggerStepThrough]
        public static void IsNotInvalidEmail(string argument, string argumentName)
        {
            IsNotEmpty(argument, argumentName);

            if (!argument.IsEmail())
            {
                throw new ArgumentException(AppCommon.Invalid_Email_Address.FormatWith(argumentName), argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void IsNotInvalidWebUrl(string argument, string argumentName)
        {
            IsNotEmpty(argument, argumentName);

            if (!argument.IsWebUrl())
            {
                throw new ArgumentException(AppCommon.Invalid_Web_Url.FormatWith(argumentName), argumentName);
            }
        }
        [DebuggerStepThrough]
        public static void IsNotTypeOf<TType>(object instance, string message)
        {
            if (!(instance is TType))
                throw new InvalidOperationException(message);
        }
    }
}