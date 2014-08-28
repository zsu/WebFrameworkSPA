using System;
using Castle.Windsor;
using App.Infrastructure.Castle.Logging.Test;
using App.Common.Logging;
using App.Infrastructure.Log4net;
using App.Common.InversionOfControl;
using Castle.MicroKernel.Registration;
using App.Infrastructure.Castle.Logging;
using Castle.DynamicProxy;

namespace App.Infrastructure.Castle.Test
{
    public static class ServiceLocatorInitializer {
        public static void Init() {
            IoC.InitializeWith(new DependencyResolverFactory());
            IWindsorContainer container = IoC.GetContainer<IWindsorContainer>();//new WindsorContainer();
            ComponentRegistrar.AddComponentsTo(container);
            RegisterTestServices(container);
            //ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }

        private static void RegisterTestServices(IWindsorContainer container) {
            container.Register(Component.For<IInterceptor>().ImplementedBy<LogInterceptor>().Named(typeof(LogInterceptor).FullName))
                .Register(Component.For<ILogTestClass>().ImplementedBy<LogTestClass>())
                .Register(Component.For<IExceptionHandlerTestClass>().ImplementedBy<ExceptionHandlerTestClass>())
                .Register(Component.For<ILogFactory>().ImplementedBy<Log4netLogFactory>())
                .Register(Component.For<LogTestClass2>().ImplementedBy<LogTestClass2>());
        }
    }
}