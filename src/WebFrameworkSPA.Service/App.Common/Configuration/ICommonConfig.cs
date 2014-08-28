using System;

namespace App.Common.Configuration
{
    /// <summary>
    /// Configuration interface exposed by CommonConfig to configure different services exposed by application.
    /// </summary>
    public interface ICommonConfig
    {
        /// <summary>
        /// Configure data providers used by applicaton.
        /// </summary>
        /// <typeparam name="T">A <see cref="IDataConfiguration"/> type that can be used to configure
        /// data providers for application.</typeparam>
        /// <returns><see cref="ICommonConfig"/></returns>
        ICommonConfig ConfigureData<T>() where T : IDataConfiguration, new();

        /// <summary>
        /// Configure data providers used by applicaton.
        /// </summary>
        /// <typeparam name="T">A <see cref="IDataConfiguration"/> type that can be used to configure
        /// data providers for application.</typeparam>
        /// <param name="actions">An <see cref="Action{T}"/> delegate that can be used to perform
        /// custom actions on the <see cref="IDataConfiguration"/> instance.</param>
        /// <returns><see cref="ICommonConfig"/></returns>
        ICommonConfig ConfigureData<T>(Action<T> actions) where T : IDataConfiguration, new();

        /// <summary>
        /// Configures unit of work settings.
        /// </summary>
        /// <typeparam name="T">A <see cref="IUnitOfWorkConfiguration"/> type that can be used to configure
        /// unit of work settings.</typeparam>
        /// <returns><see cref="ICommonConfig"/></returns>
        ICommonConfig ConfigureUnitOfWork<T> () where T : IUnitOfWorkConfiguration, new();

        ///<summary>
        /// Configures unit of work settings.
        ///</summary>
        /// <typeparam name="T">A <see cref="ICommonConfig"/> type that can be used to configure
        /// unit of work settings.</typeparam>
        ///<param name="actions">An <see cref="Action{T}"/> delegate that can be used to perform
        /// custom actions on the <see cref="IUnitOfWorkConfiguration"/> instance.</param>
        ///<returns><see cref="ICommonConfig"/></returns>
        ICommonConfig ConfigureUnitOfWork<T>(Action<T> actions) where T : IUnitOfWorkConfiguration, new();
    }
}