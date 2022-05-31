/*
 * Copyright © 2022 DotNotStandard. All rights reserved.
 * 
 * See the LICENSE file in the root of the repo for licensing details.
 * 
 */
using DotNotStandard.DataAccess.Core;
using DotNotStandard.DataAccess.SqlServer;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for the IServiceCollection type
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Register the types required to implement SQL connection management
        /// </summary>
        /// <param name="services">The collection into which to register the required services</param>
        /// <returns>The extended type, to support method chaining</returns>
        public static IServiceCollection AddMSSQLConnectionManagement(this IServiceCollection services)
        {
            // Add all of the services required to manage SQL connections
            services.TryAddTransient<IConnectionAddressManager, ConnectionAddressManager>();
            services.TryAddScoped<IConnectionManager, ConnectionManager>();
            services.TryAddTransient<IConnectionFactory, SqlConnectionFactory>();

            return services;
        }
    }
}
