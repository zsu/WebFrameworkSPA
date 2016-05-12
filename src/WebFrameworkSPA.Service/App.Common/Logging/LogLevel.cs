namespace App.Common.Logging
{
    /// <summary>
    /// Log levels recognized by the system.
    /// </summary>
    /// <value>
    /// Debug -   Designates fine-grained informational events that are most useful to debug an application.
    /// Info  -   Designates informational messages that highlight the progress of the application at coarse-grained level.
    /// Warn -   Designates potentially harmful situations.
    /// Error -   Designates error events that might still allow the application to continue running.
    /// Fatal   -   Designates very severe error events that will presumably lead the application to abort.
    /// Off   -   Designates no logging information.    
    /// </value>
    public enum LogLevel
    {
        Off,
        Fatal,
        Error,
        Warn,
        Info,
        Debug
    }
}
