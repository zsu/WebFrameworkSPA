using System;
using App.Common.InversionOfControl;
using App.Common.Contexts;

namespace App.Common.Configuration
{
    ///<summary>
    /// Default implementation of <see cref="ICommonConfig"/> class.
    ///</summary>
    public class CommonConfig : ICommonConfig
    {
        readonly ICustomDependencyResolver _containerAdapter;
        ///<summary>
        /// Default Constructor.
        /// Creates a new instance of the <see cref="CommonConfig"/>  class.
        ///</summary>
        ///<param name="containerAdapter">An instance of <see cref="ICustomDependencyResolver"/> that can be
        /// used to register components.</param>
        public CommonConfig(ICustomDependencyResolver containterAdapter)
        {
            _containerAdapter = containterAdapter;
            InitializeDefaults();
        }

        /// <summary>
        /// Registers default components for CommonConfig.
        /// </summary>
        void InitializeDefaults()
        {
            IoC.RegisterType<IContext, Contexts.Context>(LifetimeType.Transient);
        }

        /// <summary>
        /// Configure data providers used by CommonConfig.
        /// </summary>
        /// <typeparam name="T">A <see cref="IDataConfiguration"/> type that can be used to configure
        /// data providers for CommonConfig.</typeparam>
        /// <returns><see cref="ICommonConfig"/></returns>
        public ICommonConfig ConfigureData<T>() where T : IDataConfiguration, new()
        {
            var datConfiguration = (T) Activator.CreateInstance(typeof (T));
            datConfiguration.Configure(_containerAdapter);
            return this;
        }

        /// <summary>
        /// Configure data providers used by CommonConfig.
        /// </summary>
        /// <typeparam name="T">A <see cref="IDataConfiguration"/> type that can be used to configure
        /// data providers for CommonConfig.</typeparam>
        /// <param name="actions">An <see cref="Action{T}"/> delegate that can be used to perform
        /// custom actions on the <see cref="IDataConfiguration"/> instance.</param>
        /// <returns><see cref="ICommonConfig"/></returns>
        public ICommonConfig ConfigureData<T>(Action<T> actions) where T : IDataConfiguration, new()
        {
            var dataConfiguration = (T) Activator.CreateInstance(typeof (T));
            actions(dataConfiguration);
            dataConfiguration.Configure(_containerAdapter);
            return this;
        }

        /// <summary>
        /// Configures unit of work settings.
        /// </summary>
        /// <typeparam name="T">A <see cref="IUnitOfWorkConfiguration"/> type that can be used to configure
        /// unit of work settings.</typeparam>
        /// <returns><see cref="ICommonConfig"/></returns>
        public ICommonConfig ConfigureUnitOfWork<T> () where T : IUnitOfWorkConfiguration, new()
        {
            var uowConfiguration = (T) Activator.CreateInstance(typeof (T));
            uowConfiguration.Configure(_containerAdapter);
            return this;
        }

        ///<summary>
        /// Configures unit of work settings.
        ///</summary>
        /// <typeparam name="T">A <see cref="ICommonConfig"/> type that can be used to configure
        /// unit of work settings.</typeparam>
        ///<param name="actions">An <see cref="Action{T}"/> delegate that can be used to perform
        /// custom actions on the <see cref="IUnitOfWorkConfiguration"/> instance.</param>
        ///<returns><see cref="ICommonConfig"/></returns>
        public ICommonConfig ConfigureUnitOfWork<T>(Action<T> actions) where T : IUnitOfWorkConfiguration, new()
        {
            var uowConfiguration = (T) Activator.CreateInstance(typeof (T));
            actions(uowConfiguration);
            uowConfiguration.Configure(_containerAdapter);
            return this;
        }
    }
}