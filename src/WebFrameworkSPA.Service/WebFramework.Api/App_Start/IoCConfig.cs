using App.Common;
using App.Common.Caching;
using App.Common.Configuration;
using App.Common.InversionOfControl;
using App.Common.SessionMessage;
using App.Common.TypeFinder;
using App.Infrastructure.Castle;
using App.Infrastructure.NHibernate;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Nh;
using BrockAllen.MembershipReboot.Nh.Repository;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Service;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using App.Common.Security;
using App.Common.Data;
using App.Common.Events;
using App.Common.Tasks;
namespace Web
{
    public static class IoCConfig
    {
        /// <summary>
        /// Creates the container that will manage your application.
        /// </summary>
        /// <returns>The created container.</returns>
        public static IWindsorContainer ConfigureContainer()
        {
            var container = IoC.GetContainer<IWindsorContainer>();// new WindsorContainer();
            RegisterServices(container);
            return container;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="container">The container.</param>
        private static void RegisterServices(IWindsorContainer container)
        {
            ComponentRegistrar.AddComponentsTo(IoC.GetContainer<IWindsorContainer>());
            IoC.RegisterType<ITypeFinder, WebAppTypeFinder>(LifetimeType.Singleton)
               .RegisterType(Util.ApplicationCacheKey, typeof(App.Common.Caching.ICacheManager), typeof(MemoryCacheManager), LifetimeType.Singleton)
               .RegisterType(Util.PerRequestCacheKey, typeof(App.Common.Caching.ICacheManager), typeof(PerRequestCacheManager), LifetimeType.Singleton)
               .RegisterType(Util.ContextCacheKey, typeof(App.Common.Caching.ICacheManager), typeof(ContextCacheManager), LifetimeType.Singleton)
               .RegisterInstance<ICustomDependencyResolver>(IoC.GetResolver<ICustomDependencyResolver>())
               .RegisterType<ICommonConfig, CommonConfig>(LifetimeType.Singleton)
               //.RegisterType<ISessionMessageProvider, SessionStateSessionMessageProvider>(LifetimeType.Singleton)
               .RegisterType<IEventPublisher, EventPublisher>(LifetimeType.Singleton)
               .RegisterType<ISubscriptionService, SubscriptionService>(LifetimeType.Singleton)
               .RegisterType<IScheduleTaskService, ScheduleTaskService>(LifetimeType.Transient)
               .RegisterType<ISessionMessageFactory, SessionMessageFactory>(LifetimeType.Singleton)
               .RegisterInstance<ISessionMessageProvider>(IoC.GetService<ISessionMessageFactory>().CreateInstance(), LifetimeType.Transient);
            //var eventConsumers=IoC.GetService<ITypeFinder>().FindClassesOfType(typeof(IConsumer<>));
            //foreach(var eventConsumer in eventConsumers)
            //{
            //    IoC.RegisterType(null, typeof(IConsumer<>), eventConsumer.GetType(), LifetimeType.PerRequest);
            //}
            container.Register(Classes.FromThisAssembly().BasedOn(typeof(IConsumer<>)).WithService.AllInterfaces().LifestyleTransient());

            IoC.RegisterInstance(null, typeof(IAppConfig), Util.ApplicationConfiguration, LifetimeType.Singleton);
            //var securityDomainFactory=NHibernateConfig.SecurityDomainFactory();
            //var logDomainFactory=NHibernateConfig.LogDomainFactory();
            IoC.GetService<ICommonConfig>()
                .ConfigureData<NHConfiguration>(x => x.WithSessionFactory(NHibernateConfig.SecurityDomainFactory).WithSessionFactory(NHibernateConfig.LogDomainFactory)
                    .WithSessionFactory(NHibernateConfig.AppDomainFactory))
                .ConfigureUnitOfWork<DefaultUnitOfWorkConfiguration>();//x => x.AutoCompleteScope());
            //container.Register(Component.For<NhUserAccountService<NhUserAccount>>().LifestyleTransient());
            //container.Register(Component.For<UserAccountService<NhUserAccount>>().LifestyleTransient());
            //container.Register(Component.For<AuthenticationService<NhUserAccount>>().ImplementedBy<OwinAuthenticationService<NhUserAccount>>().LifestyleTransient());
            IoC.RegisterType(null, typeof(IRepository<,>), typeof(NHRepository<,>), LifetimeType.Transient);
            container.Register(Component.For<IUserAccountRepository<NhUserAccount>>().ImplementedBy<NhUserAccountRepository<NhUserAccount>>().LifestyleTransient());
            IoC.RegisterType<IUserAccountQuery, NhUserAccountRepository<NhUserAccount>>(LifetimeType.Transient);
            IoC.RegisterType<IPasswordGenerator, PasswordGenerator>(LifetimeType.Transient);

            container.Register(Classes.FromThisAssembly().BasedOn<IController>().LifestyleTransient());
            container.Register(Classes.FromThisAssembly().BasedOn<IHttpController>().LifestylePerWebRequest());//.LifestyleScoped());
            container.Register(Classes.FromAssemblyNamed("WebFramework.Service").BasedOn<IService>().WithService.AllInterfaces().LifestyleTransient());
            //var config = SecurityConfig.Config();//Depends on IMessageTemplateService
            //container.Register(Component.For<MembershipRebootConfiguration<NhUserAccount>>().Instance(config));
            container.Register(Classes.FromThisAssembly().BasedOn<ITask>().LifestyleTransient());
        }
    }
}
