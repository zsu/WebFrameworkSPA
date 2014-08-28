using App.Common.InversionOfControl;
namespace App.Common.Configuration
{
    /// <summary>
    /// Base interface implemented by specific data configurators that configure application data providers.
    /// </summary>
    public interface IDataConfiguration
    {
        /// <summary>
        /// Called by application <see cref="Configure"/> to configure data providers.
        /// </summary>
        /// <param name="containerAdapter">The <see cref="ICustomDependencyResolver"/> instance that allows
        /// registering components.</param>
        void Configure(ICustomDependencyResolver containerAdapter);
    }
}