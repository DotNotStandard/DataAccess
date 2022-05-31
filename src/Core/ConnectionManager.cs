/*
 * Copyright © 2022 DotNotStandard. All rights reserved.
 * 
 * See the LICENSE file in the root of the repo for licensing details.
 * 
 */

using System.Collections.Concurrent;
using System.Transactions;

namespace DotNotStandard.DataAccess.Core
{

    /// <summary>
    /// Default connection manager implementing the IConnectionManager interface
    /// </summary>
    public class ConnectionManager : IConnectionManager, IAsyncDisposable
    {
        private readonly ConcurrentStack<TransactionFrame> _transactions = new ConcurrentStack<TransactionFrame>();
        private readonly IConnectionFactory _connectionFactory;

        public ConnectionManager(IConnectionFactory connectionManager)
        {
            _connectionFactory = connectionManager;
        }

        #region ITransactionManager interface

        /// <summary>
        /// Start a new, root transaction
        /// </summary>
        /// <returns>An IAsyncDisposable that can be used to clean up the transaction</returns>
        public IAsyncDisposable StartTransaction()
        {
            return StartTransaction(TransactionScopeOption.RequiresNew);
        }

        /// <summary>
        /// Start a new transaction with the appropriate scope
        /// </summary>
        /// <returns>An IAsyncDisposable that can be used to clean up the transaction</returns>
        public IAsyncDisposable StartTransaction(TransactionScopeOption scopeOption)
        {
            TransactionScope transactionScope = TransactionScopeFactory.CreateReadCommittedTransaction(scopeOption);
            TransactionFrame transFrame = new TransactionFrame(transactionScope);
            _transactions.Push(transFrame);

            return new TransactionDisposer(this);
        }

        /// <summary>
        /// Get a connection
        /// </summary>
        /// <typeparam name="T">The type that is used to interact with the connection</typeparam>
        /// <param name="connectionName">The name of the required connection</param>
        /// <returns>The requested connection</returns>
        /// <exception cref="InvalidOperationException">The connection is not of the type requested</exception>
        public async Task<T> GetConnectionAsync<T>(string connectionName) where T: class
        {
            TransactionFrame? transFrame;
            object connection;
            T? typedData = null;

            if (_transactions.TryPeek(out transFrame))
            {
                if (transFrame.ConnectionDetails is null)
                {
                    connection = await _connectionFactory.CreateConnectionAsync(connectionName).ConfigureAwait(false);
                    transFrame.ConnectionDetails = new ConnectionDetails(connection, connectionName);
                }
                typedData = transFrame.ConnectionDetails.Connection as T;
                if (typedData is null) throw new InvalidOperationException("Connection is not of the expected type!");
            }

            if (typedData is null) throw new InvalidOperationException("No connection available!");
            return typedData;
        }

        /// <summary>
        /// Commit the transaction
        /// </summary>
        public async Task CommitTransactionAsync()
        {
            TransactionFrame? transFrame;
            _transactions.TryPeek(out transFrame);
            if (!(transFrame is null))
            {
                await CloseAndDisposeConnectionAsync(transFrame).ConfigureAwait(false);
                transFrame.TransactionScope.Complete();
            }
        }

        /// <summary>
        /// Clean up the transaction
        /// </summary>
        public async Task FinaliseTransactionAsync()
        {
            TransactionFrame? transFrame;
            if (_transactions.TryPop(out transFrame))
            {
                await CloseAndDisposeConnectionAsync(transFrame).ConfigureAwait(false);
                await DisposeTransactionAsync(transFrame).ConfigureAwait(false);
            }
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Close and dispose of the connection, with appropriate handling
        /// </summary>
        /// <param name="transFrame">The transaction frame whose connection we are to influence</param>
        private async Task CloseAndDisposeConnectionAsync(TransactionFrame transFrame)
        {
            ConnectionDetails connectionData;

            if (transFrame.ConnectionDetails is null) return;
            connectionData = transFrame.ConnectionDetails;
            if (connectionData.Connection is null) return;
            await _connectionFactory.CleanupConnectionAsync(connectionData.Connection).ConfigureAwait(false);
            transFrame.ConnectionDetails = null;
            return;
        }

        /// <summary>
        /// Dispose of the transaction managed in the transaction frame
        /// </summary>
        /// <param name="transFrame">The transaction frame</param>
        /// <returns></returns>
        private Task DisposeTransactionAsync(TransactionFrame transFrame)
        {
            transFrame.TransactionScope.Dispose();
            return Task.CompletedTask;
        }

        #endregion

        #region IAsyncDisposable interface

        /// <summary>
        /// Dispose of the resources in use by this connection manager
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Core implementation of async disposal for the resources used
        /// </summary>
        protected virtual async ValueTask DisposeAsyncCore()
        {
            TransactionFrame? transFrame;

            while (_transactions.TryPop(out transFrame))
            {
                await CloseAndDisposeConnectionAsync(transFrame).ConfigureAwait(false);
                await DisposeTransactionAsync(transFrame).ConfigureAwait(false);
            }
        }

        #endregion

        #region Private Helper Types

        /// <summary>
        /// Type used to retain and manage information on a transaction
        /// </summary>
        private class TransactionFrame
        {
            public TransactionFrame(TransactionScope transactionScope)
            {
                TransactionScope = transactionScope;
            }

            public TransactionScope TransactionScope { get; private set; }

            public ConnectionDetails? ConnectionDetails { get; set; }

        }

        /// <summary>
        /// DTO used to hold information about a connection
        /// </summary>
        private class ConnectionDetails
        {
            public ConnectionDetails(object connection, string connectionName)
            {
                Connection = connection;
                ConnectionName = connectionName;
            }

            public object Connection { get; set; }

            public string ConnectionName { get; set; }

        }

        /// <summary>
        /// Class exposing disposal of a transaction to a client
        /// </summary>
        private class TransactionDisposer : IAsyncDisposable
        {
            private readonly IConnectionManager _parent;

            public TransactionDisposer(IConnectionManager transactionManager)
            {
                _parent = transactionManager;
            }

            #region IAsyncDisposable interface

            public async ValueTask DisposeAsync()
            {
                await DisposeAsyncCore().ConfigureAwait(false);

                // Suppress finalization.
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// Core implementation of the async disposal of a transaction
            /// </summary>
            protected virtual async ValueTask DisposeAsyncCore()
            {
                await _parent.FinaliseTransactionAsync().ConfigureAwait(false);
            }

            #endregion
        }

        #endregion
    }
}
