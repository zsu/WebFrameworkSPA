using System;

namespace App.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false,
        Inherited = false)]
    public class ExceptionHandlerAttribute : Attribute
    {
        public ExceptionHandlerAttribute()
        {
        }

        public bool IsSilent
        {
            get;
            set;
        }

        public object ReturnValue
        {
            get;
            set;
        }

        public Type ExceptionType
        {
            get;
            set;
        }

        public string SkipArguments
        {
            get;
            set;
        }
    }
}