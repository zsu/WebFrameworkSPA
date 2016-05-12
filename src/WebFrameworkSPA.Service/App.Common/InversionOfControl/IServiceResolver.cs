using System;

namespace App.Common.InversionOfControl
{
    public interface IServiceResolver
    {        
        // Summary:
        //     Resolves singly registered services that support arbitrary object creation.
        //
        // Parameters:
        //   serviceType:
        //     The type of the requested service or object.
        //
        // Returns:
        //     The requested service or object.
        //object GetService(Type serviceType);
        // Summary:
        //     Resolves singly registered services that support arbitrary object creation.
        //
        // Parameters:
        //   serviceType:
        //     The type of the requested service or object.
        //   key:
        //      The key.
        //
        // Returns:
        //     The requested service or object.
        object GetService(Type serviceType, string key);
        //
        // Summary:
        //     Resolves multiply registered services.
        //
        // Parameters:
        //   serviceType:
        //     The type of the requested services.
        //
        // Returns:
        //     The requested services.
        //IEnumerable<object> GetServices(Type serviceType);
    }
}
