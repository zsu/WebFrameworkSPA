using System.Transactions;
using App.Common.Logging;

namespace App.Data
{
    /// <summary>
    /// Helper class to create <see cref="TransactionScope"/> instances.
    /// </summary>
    public static class TransactionScopeHelper
    {
        ///<summary>
        ///</summary>
        ///<param name="isolationLevel"></param>
        ///<param name="txMode"></param>
        ///<returns></returns>
        ///<exception cref="NotImplementedException"></exception>
        public static TransactionScope CreateScope(IsolationLevel isolationLevel, TransactionMode txMode)
        {
            if (txMode == TransactionMode.New)
            {
                Logger.Log(LogLevel.Debug,"Creating a new TransactionScope with TransactionScopeOption.RequiresNew");
                return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = isolationLevel });
            }
            if (txMode == TransactionMode.Supress)
            {
                Logger.Log(LogLevel.Debug,"Creating a new TransactionScope with TransactionScopeOption.Supress");
                return new TransactionScope(TransactionScopeOption.Suppress);
            }
            Logger.Log(LogLevel.Debug,"Creating a new TransactionScope with TransactionScopeOption.Required");
            return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = isolationLevel });
        }
    }
}