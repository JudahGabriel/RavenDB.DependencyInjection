# RavenDB.DependencyInjection Sample
This sample shows a Razor Pages app that uses RavenDB.DependencyInjection.

Three areas of interest:

## appsettings.json
The [appsettings.json](https://github.com/JudahGabriel/RavenDB.DependencyInjection/blob/master/Sample/appsettings.json) file contains information about the Raven database we're connecting to.

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

## Startup.cs
[Startup.cs](https://github.com/JudahGabriel/RavenDB.DependencyInjection/blob/master/Sample/Startup.cs#L36) loads the appsettings and adds a Raven `IDocumentStore` singleton to the dependency injection services.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // 1. Add an IDocumentStore singleton.
    services.AddRavenDbDocStore();

    // 2. Optional: Add a scoped IAsyncDocumentSession. For the sync version, use .AddRavenSession().
    services.AddRavenDbAsyncSession(); 
}
```

## Using Raven inside your app services
Now that you've injected an `IDocumentStore` and `IAsyncDocumentSession` into the DI services, you can use them in your controllers, services, and Razor pages.

In this sample, we've created an [OrderService](https://github.com/JudahGabriel/RavenDB.DependencyInjection/blob/master/Sample/Services/OrderService.cs) that loads orders from Raven:

```csharp
public class OrderService
{
    private readonly IAsyncDocumentSession dbSession;

	// Have our Raven IAsyncDocumentSession injected.
    public OrderService(IAsyncDocumentSession dbSession)
    {
        this.dbSession = dbSession;
    }

    public Task<int> GetOrderCount()
    {
        return this.dbSession
            .Query<Order>()
            .CountAsync();
    }
}
```

We [add this OrderService to our Startup.cs](https://github.com/JudahGabriel/RavenDB.DependencyInjection/blob/master/Sample/Startup.cs#L47):
```csharp
services.AddScoped<OrderService>();
```

Finally, we use this service inside our controllers, services, or Razor Pages. In this sample, we use it inside our [Index.cshtml Razor Page](https://github.com/JudahGabriel/RavenDB.DependencyInjection/blob/master/Sample/Pages/Index.cshtml#L6)
```cshtml
@page
@model IndexModel
@{
    ViewData["Title"] = "Home page";
}
@inject Services.OrderService orderSvc

<h1>There are <strong>@await orderSvc.GetOrderCount()</strong> orders in the database 😎</h1>
```