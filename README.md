# DotNotStandard.DataAccess
Data access utilities, including connection management and custom exceptions

This package includes a connection manager for managing connections in .NET applications. It requires use of more than one package, as data access functionality that is specific to a database vendor is split into a separate package per vendor.

Available packages include:
DotNotStandard.DataAccess - the core package
DotNotStandard.DataAccess.SqlServer - the implementations for SQL Server

Use of the functionality in this package requires that the user register the required services into the DI container at application startup.

For SQL Server, you would include the following code in Program.cs/Startup.cs:
``` csharp
services.AddMSSQLConnectionManagement();
```

As .NET Core does not yet support distributed transactions, the connection manager included in this package manages only a single connection per transaction. Calling the basic `StartTransaction()` method on the type implementing the `IConnectionManager` interface results in the creation of a new, root transaction, with no open connection. A call to `GetConnectionAsync<T>(string connectionName)` then opens a connection using the connection string represented by the connection name provided. This connection is returned for all subsequent calls to the method.

The subsystem is implemented using DI, so you may customise the behaviour by creating and registering your own types that implement the exposed interfaces.

Only a single connection factory is supported at any one time - it is not currently possible to use databases from multiple vendors in the same application via the connection manager. Future releases may enhance the package to support multiple connection factories - to enable management of connections to multiple database providers at once. However, this is not on the backlog at this time, so users should not expect this to be included.