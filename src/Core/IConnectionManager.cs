/*
 * Copyright © 2022 DotNotStandard. All rights reserved.
 * 
 * See the LICENSE file in the root of the repo for licensing details.
 * 
 */

using System.Transactions;

namespace DotNotStandard.DataAccess.Core
{

    /// <summary>
    /// Contract that a connection manager must implement
    /// </summary>
    public interface IConnectionManager : IAsyncDisposable
    {
        /// <summary>
        /// Start a new root transaction
        /// </summary>
        /// <returns>An IAsyncDisposable which can clean up the transaction</returns>
        IAsyncDisposable StartTransaction();

        /// <summary>
        /// Start a transaction of the requested scope
        /// </summary>
        /// <param name="transactionScope">The scope with which the transaction should be created</param>
        /// <returns>An IAsyncDisposable that can clean up the transaction started</returns>
        IAsyncDisposable StartTransaction(TransactionScopeOption transactionScope);

        /// <summary>
        /// Commit the current transaction
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Finalize the transaction, used when committing it is inappropriate
        /// </summary>
        Task FinaliseTransactionAsync();

        /// <summary>
        /// Get a connection enrolled within the current transaction scope
        /// </summary>
        /// <typeparam name="T">The type used to manage the connection</typeparam>
        /// <param name="connectionName">The name of the connection required</param>
        /// <returns>The connection requested</returns>
        Task<T> GetConnectionAsync<T>(string connectionName) where T : class;
    }
}
