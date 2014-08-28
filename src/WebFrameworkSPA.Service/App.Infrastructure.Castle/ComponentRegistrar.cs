using System;
using Castle.Windsor;
using App.Common;
using App.Common.Logging;
using App.Infrastructure.Castle.Logging;
using Castle.MicroKernel.Registration;
using App.Infrastructure.Log4net;

namespace App.Infrastructure.Castle {
    public static class ComponentRegistrar {
        public static void AddComponentsTo(IWindsorContainer container) {
            Check.IsNotNull(container, "container");
            if (!container.Kernel.HasComponent("ExceptionLogger"))
            {
                container.Register(Component.For<IExceptionLogger>().ImplementedBy<ExceptionLogger>().Named("ExceptionLogger"))
                .Register(Component.For<IMethodLogger>().ImplementedBy<MethodLogger>().Named("MethodLogger"))
                .Register(Component.For<ILogFactory>().ImplementedBy<Log4netLogFactory>().Named("Log4netFactory"));
            } 
            if (!container.Kernel.HasComponent("LogFacility"))
            {
                container.AddFacility("LogFacility", new LogFacility());
                container.AddFacility("ExceptionHandlerFacility", new ExceptionHandlerFacility());
            }
        }
    }
}