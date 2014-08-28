namespace App.Common.InversionOfControl
{
    public interface IDependencyResolverFactory
    {
        ICustomDependencyResolver CreateInstance();
    }
}