# <img src="https://github.com/JudahGabriel/RavenDB.DependencyInjection/blob/master/RavenDB.DependencyInjection/nuget-icon.png?raw=true" width="50px" height="50px" /> RavenDB.DependencyInjection
Dependency Injection package for using RavenDB with ASP.NET Core.

This package lets you configure a RavenDB `DocumentStore` and create a singleton for it in the dependency injection container. Additionally, you can configure an `IAsyncDocumentSession` (or its synchronous equivalent) to be created per scope.

## Getting Started
Install the [RavenDB.DependencyInjection](https://www.nuget.org/packages/RavenDB.DependencyInjection) library through [NuGet](https://nuget.org).
```
Install-Package RavenDB.DependencyInjection
```    

## Usage   

Add a RavenSettings section to your appsettings.json:

```json
"RavenSettings": {
    "Urls": [
       "http://live-test.ravendb.net"
    ],
    "DatabaseName": "Demo",
    "CertFilePath": "",
    "CertPassword": ""
},
```

Note that `CertFilePath` and `CertPassword` are optional. If you use a certificate to connect to your database, this should be a path relative to the content root. Is your certificate stored outside your code? See [manual configuration](https://github.com/JudahGabriel/RavenDB.DependencyInjection#manual-configuration).

Then in Startup.cs, tell Raven to use this database and add it to the DI container:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // 1. Add an IDocumentStore singleton. Make sure that RavenSettings section exist in appsettings.json
    services.AddRavenDbDocStore();

    // 2. Add a scoped IAsyncDocumentSession. For the sync version, use .AddRavenSession().
    services.AddRavenDbAsyncSession(); 
}
```

Now you're cooking! Your controllers and services can now have `IDocumentStore`, `IAsyncDocumentSession`, or `IDocumentSession` injected into them. 😎

### Configuring Raven conventions
Do you need to configure RavenDB conventions or perform other work before `docStore.Initialize()`? It's simple:
```csharp
services.AddRavenDbDocStore(options => 
{
    options.BeforeInitializeDocStore = docStore => docStore.Conventions.IdentityPartsSeparator = "-";
}
```

### Manual configuration
Is your Raven information stored outside of your code, such as environment variables or Azure Key Vault? If so, you can configure your doc store like this:

```csharp
services.AddRavenDbDocStore(options =>
{
    // Grab the DB name from appsettings.json
    var dbName = options.Settings.DbName;
    
    // But grab the cert and password from the cloud
    var certBytes = Convert.FromBase64String(...); // load the certificate from wherever
    var certPassword = ...; // grab the password from wherever
    options.Certificate = new X509Certificate2(certBytes, certPassword);
});
```

View the [Sample project](https://github.com/JudahGabriel/RavenDB.DependencyInjection/tree/master/Sample) to see it all in action.