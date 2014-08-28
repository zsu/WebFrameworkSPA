using System;
using App.Common.Logging;
using App.Common.InversionOfControl;
using App.Common.Caching;
using App.Common;

namespace App.Data
{
    ///<summary>
    /// Gets an instances of <see cref="ITransactionManager"/>.
    ///</summary>
    public static class UnitOfWorkManager
    {
        static Func<ITransactionManager> _provider;
        private const string LocalTransactionManagerKey = "UnitOfWorkManager.LocalTransactionManager",UnitOfWorkCacheKey="UnitOfWork";
        static readonly Func<ITransactionManager> DefaultTransactionManager = () =>
        {
            Logger.Log(LogLevel.Debug,"Using default UnitOfWorkManager provider to resolve current transaction manager.");
            var cache = IoC.GetService<ICacheManager>(Util.ContextCacheKey);
            var transactionManager = cache.Get<ITransactionManager>(LocalTransactionManagerKey);
            if (transactionManager == null)
            {
                Logger.Log(LogLevel.Debug,"No valid ITransactionManager found in Local state. Creating a new TransactionManager.");
                transactionManager = new TransactionManager();
                cache.Set(LocalTransactionManagerKey, transactionManager,0);
            }
            return transactionManager;
        };

        /// <summary>
        /// Default Constructor.
        /// Creates a new instance of the <see cref="UnitOfWorkManager"/>.
        /// </summary>
        static UnitOfWorkManager()
        {
            _provider = DefaultTransactionManager;
        }

        ///<summary>
        /// Sets a <see cref="Func{T}"/> of <see cref="ITransactionManager"/> that the 
        /// <see cref="UnitOfWorkManager"/> uses to get an instance of <see cref="ITransactionManager"/>
        ///</summary>
        ///<param name="provider"></param>
        public static void SetTransactionManagerProvider(Func<ITransactionManager> provider)
        {
            if (provider == null)
            {
                Logger.Log(LogLevel.Debug,"The transaction manager provide is being set to null. Using " +
                                    " the transaction manager to the default transaction manager provider.");
                _provider = DefaultTransactionManager;
                return;
            }
            Logger.Log(LogLevel.Debug,"The transaction manager provider is being overriden. Using supplied" +
                                " trasaction manager provider.");
            _provider = provider;
        }

        /// <summary>
        /// Gets the current <see cref="ITransactionManager"/>.
        /// </summary>
        public static ITransactionManager CurrentTransactionManager
        {
            get
            {
                return _provider();
            }
        }

        /// <summary>
        /// Gets the current <see cref="IUnitOfWork"/> instance.
        /// </summary>
        public static IUnitOfWork CurrentUnitOfWork
        {
            get
            {
                var unitOfWork= _provider().CurrentUnitOfWork;
                if (unitOfWork != null)
                    return unitOfWork;
                var cache = IoC.GetService<ICacheManager>(Util.ContextCacheKey);
                unitOfWork = cache.Get<IUnitOfWork>(UnitOfWorkCacheKey);
                if (unitOfWork != null)
                    return unitOfWork;
                var uowFactory = IoC.GetService<IUnitOfWorkFactory>();
                unitOfWork = uowFactory.Create();
                cache.Set(UnitOfWorkCacheKey, unitOfWork, 0);
                return unitOfWork;
            }
        }
    }
}