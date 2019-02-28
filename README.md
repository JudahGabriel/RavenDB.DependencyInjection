# RavenDB.DependencyInjection
# <img src="https://github.com/JudahGabriel/RavenDB.DependencyInjection/blob/master/RavenDB.DependencyInjection/nuget-icon.png?raw=true" width="100px" height="100px" /> Dependency Injection package for using RavenDB with ASP.NET Core.

This package lets you configure a RavenDB `DocumentStore` and create a singleton for it in the dependency injection container. Additionally, you can configure an `IAsyncDocumentSession` (or it's synchronous equivalent) to be created per scope.

## Getting Started:
Install the [RavenDB.DependencyInjection](https://www.nuget.org/packages/RavenDB.DependencyInjection) library through [NuGet](https://nuget.org).
```
    Install-Package RavenDB.DependencyInjection
```    

## Usage:   

Add a RavenSettings section to your appsettings.json:

```json
"RavenSettings": {
    "Urls": [
      "http://live-test.ravendb.net"
    ],
    "DatabaseName": "Northwind",
    "CertFilePath": "",
    "CertPassword": ""
  },
```

Note that CertFilePath is optional. If you use a certificate to connect to your database, this should be a path relative to the content root.

Then in Startup.cs, tell Raven to use this database and add it to the DI container:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // 1. Grab our RavenSettings object from appsettings.json.
    services.Configure<RavenSettings>(Configuration.GetSection("RavenSettings"));

    // 2. Add an IDocumentStore singleton.
    services.AddRavenDbDocStore();

    // 3. Add a scoped IAsyncDocumentSession. For the sync version, use .AddRavenSession().
    services.AddRavenDbAsyncSession(); 
}
```
Now you're cooking! Your controllers and services can now have IDocumentStore, IAsyncDocumentSession, or IDocumentSession injected into them. 😎

View the [Sample project](https://github.com/JudahGabriel/RavenDB.DependencyInjection/tree/master/Sample) to see it all in action.