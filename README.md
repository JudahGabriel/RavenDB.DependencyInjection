# <img src="https://github.com/JudahGabriel/RavenDB.DependencyInjection/blob/master/RavenDB.DependencyInjection/nuget-icon.png?raw=true" width="50px" height="50px" /> RavenDB.DependencyInjection
Dependency Injection package for using RavenDB with ASP.NET Core.

This package lets you configure a RavenDB `DocumentStore` and create a singleton for it in the dependency injection container. Additionally, you can configure an `IAsyncDocumentSession` (or its synchronous equivalent) to be created per scope.

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
	"DatabaseName": "Demo",
	"CertFilePath": "",
	"CertPassword": ""
},
```

Note that CertFilePath is optional. If you use a certificate to connect to your database, this should be a path relative to the content root.

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

### If SSL Certificate stored in Azure Vault the following code can be used

```

    services.AddRavenDbDocStore(options: options =>
    {
        var settings = new RavenSettings();
        configuration.Bind(nameof(RavenSettings), settings);
        options.Settings = settings;

        // password is stored in azure vault.
        var certString = configuration.GetValue<string>(settings.CertFilePath);
        if (certString != null)
        {
            var certificate = Convert.FromBase64String(certString);
            options.Certificate = new X509Certificate2(certificate);
        }
    });
```

Now you're cooking! Your controllers and services can now have `IDocumentStore`, `IAsyncDocumentSession`, or `IDocumentSession` injected into them. 😎

View the [Sample project](https://github.com/JudahGabriel/RavenDB.DependencyInjection/tree/master/Sample) to see it all in action.