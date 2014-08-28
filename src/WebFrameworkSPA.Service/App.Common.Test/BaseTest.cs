using System;
using Moq;
using App.Common.InversionOfControl;
using App.Common.Logging;

namespace App.Common.Test
{
    public abstract class BaseTest : IDisposable
    {
        protected readonly Mock<IDependencyResolverFactory> resolverFactory;
        protected readonly Mock<ILog> log;
        protected readonly Mock<ICustomDependencyResolver> resolver;

        protected BaseTest()
        {
            log = new Mock<ILog>();
            resolver = new Mock<ICustomDependencyResolver>();

            resolverFactory = new Mock<IDependencyResolverFactory>();

            resolverFactory.Setup(f => f.CreateInstance()).Returns(resolver.Object);

            resolver.Setup(r => (ILog)r.GetService(typeof(ILog),null)).Returns(log.Object);

            IoC.InitializeWith(resolverFactory.Object);
        }
        
        protected Mock<T> SetupResolve<T>() where T : class
        {
            var repository = new Mock<T>();
            resolver.Setup(r => (T)r.GetService(typeof(T),null)).Returns(repository.Object);
            return repository;
        }
        public virtual void Dispose()
        {
        }
    }
}