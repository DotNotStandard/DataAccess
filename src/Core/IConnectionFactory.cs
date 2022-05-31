/*
 * Copyright © 2022 DotNotStandard. All rights reserved.
 * 
 * See the LICENSE file in the root of the repo for licensing details.
 * 
 */

namespace DotNotStandard.DataAccess.Core
{
    /// <summary>
    /// Contract to be fulfilled by a connection factory
    /// </summary>
    public interface IConnectionFactory
    {
        /// <summary>
        /// Create a connection and return it to the client
        /// </summary>
        /// <param name="connectionName">The name of the connection to be created</param>
        /// <returns>An object representing the connection</returns>
        Task<object> CreateConnectionAsync(string connectionName);

        /// <summary>
        /// Clean up the connection
        /// </summary>
        /// <param name="connection">The connection to be cleaned up</param>
        Task CleanupConnectionAsync(object? connection);
    }
}
