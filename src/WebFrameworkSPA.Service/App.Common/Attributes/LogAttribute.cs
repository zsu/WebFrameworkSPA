using System;
using App.Common.Logging;

namespace App.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false,
        Inherited = false)]
    public class LogAttribute : Attribute
    {
        private LogLevel _entryLevel = LogLevel.Off, _successLevel = LogLevel.Off, _exceptionLevel = LogLevel.Error;
        public LogAttribute()
        {
        }

        /// <summary>
        /// Default: Off
        /// </summary>
        public LogLevel EntryLevel
        {
            get { return _entryLevel; }
            set { _entryLevel = value; }
        }

        /// <summary>
        /// Default: Off
        /// </summary>
        public LogLevel SuccessLevel
        {
            get { return _successLevel; }
            set { _successLevel = value; }
        }

        /// <summary>
        /// Default: Error
        /// </summary>
        public LogLevel ExceptionLevel
        {
            get { return _exceptionLevel; }
            set { _exceptionLevel = value; }
        }

        /// <summary>
        /// Default: Not specified
        /// </summary>
        public string LogType
        {
            get;
            set;
        }
        /// <summary>
        /// Comma seperated arguments' name to skip logging
        /// </summary>
        public string SkipArguments
        { get; set; }
    }
}