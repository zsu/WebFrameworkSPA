namespace App.Common.InversionOfControl
{
    /// <summary>
    /// Represents an enum which defines the life time of the registered service.
    /// </summary>
    public enum LifetimeType
    {
        /// <summary>
        /// The same object will be returned for the same request.
        /// </summary>
        PerRequest,

        /// <summary>
        /// This object will be created only once and the same object will be returned each time it is requested.
        /// </summary>
        Singleton,

        /// <summary>
        /// The object will be created every time it is requested.
        /// </summary>
        Transient
    }
}