/*
 * Copyright © 2022 DotNotStandard. All rights reserved.
 * 
 * See the LICENSE file in the root of the repo for licensing details.
 * 
 */

using Microsoft.Extensions.Configuration;

namespace DotNotStandard.DataAccess.Core
{
    /// <summary>
    /// Default implementation of the IConnectionAddressManager interface, which 
    /// uses configuration to gain access to connection strings by name
    /// </summary>
    public class ConnectionAddressManager : IConnectionAddressManager
    {

        private readonly IConfiguration _configuration;

        public ConnectionAddressManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Get the connection string for a database connection by name
        /// </summary>
        /// <param name="databaseName">The name of the database for which a connection string is required</param>
        /// <returns>The connection string for the database requested</returns>
        public string GetDBConnectionString(string databaseName)
        {
            return _configuration.GetConnectionString(databaseName);
        }

    }
}