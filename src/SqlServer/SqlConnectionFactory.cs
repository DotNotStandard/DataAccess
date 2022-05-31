/*
 * Copyright © 2022 DotNotStandard. All rights reserved.
 * 
 * See the LICENSE file in the root of the repo for licensing details.
 * 
 */

using DotNotStandard.DataAccess.Core;
using Microsoft.Data.SqlClient;

namespace DotNotStandard.DataAccess.SqlServer
{
    internal class SqlConnectionFactory : IConnectionFactory
    {
        private readonly IConnectionAddressManager _addressManager;

        public SqlConnectionFactory(IConnectionAddressManager addressManager)
        {
            _addressManager = addressManager;
        }

        #region IConnectionManager interface

        public async Task<object> CreateConnectionAsync(string connectionName)
        {
            string connectionString;

            if (connectionName is null) throw new ArgumentNullException(nameof(connectionName));
            connectionString = _addressManager.GetDBConnectionString(connectionName);

            SqlConnection connection = new SqlConnection(connectionString);            
            await connection.OpenAsync().ConfigureAwait(false);
            
            return connection;
        }

        public Task CleanupConnectionAsync(object? connection)
        {
            SqlConnection sqlConnection;

            if (connection is null) return Task.CompletedTask;

            sqlConnection = (SqlConnection)connection;
            if (sqlConnection is not null)
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return Task.CompletedTask;
        }

        #endregion

    }
}
