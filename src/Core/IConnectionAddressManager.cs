/*
 * Copyright © 2022 DotNotStandard. All rights reserved.
 * 
 * See the LICENSE file in the root of the repo for licensing details.
 * 
 */

namespace DotNotStandard.DataAccess.Core
{
    /// <summary>
    /// Contract to be fulfilled by a connection address manager
    /// </summary>
    public interface IConnectionAddressManager
    {
        /// <summary>
        /// Get the connection string for a named database
        /// </summary>
        /// <param name="databaseName">The name of the database for which a connection string is required</param>
        /// <returns>The connection string for the requested database</returns>
        string GetDBConnectionString(string databaseName);
    }
}