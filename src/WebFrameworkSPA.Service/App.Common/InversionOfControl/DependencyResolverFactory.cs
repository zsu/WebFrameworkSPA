using System;
using System.Configuration;
namespace App.Common.InversionOfControl
{


    public class DependencyResolverFactory : IDependencyResolverFactory
    {
        private readonly Type _resolverType;

        public DependencyResolverFactory(string resolverTypeName)
        {
            Check.IsNotEmpty(resolverTypeName, "resolverTypeName");

            _resolverType = Type.GetType(resolverTypeName, true, true);
        }

        public DependencyResolverFactory() : this(ConfigurationManager.AppSettings["dependencyResolverTypeName"])
        {
        }

        public ICustomDependencyResolver CreateInstance()
        {
            return Activator.CreateInstance(_resolverType) as ICustomDependencyResolver;
        }
    }
}