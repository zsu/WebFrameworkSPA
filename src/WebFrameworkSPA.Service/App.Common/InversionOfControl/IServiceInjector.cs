namespace App.Common.InversionOfControl
{
    /// <summary>
    /// Represents an interface which is used to inject dependencies for a given object.
    /// </summary>
    public interface IServiceInjector
    {
        /// <summary>
        /// Injects the matching dependences.
        /// </summary>
        /// <param name="instance">The instance.</param>
        void Inject(object instance);
    }
}