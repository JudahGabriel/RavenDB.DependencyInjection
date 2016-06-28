# RavenDB.DependencyInjection
Dependency Injection extensions for using RavenDB with ASP.NET Core.

The easy way to automatically inject your DocumentSession into your ASP.NET Core project.

To use:

1. Add reference to RavenDB.DependencyInjection in your project.json.

2. Add this to your Startup.cs:

```
public void ConfigureServices(IServiceCollection services)
        {
            // Add RavenDB services (IDocumentStore and IAsyncDocumentSession)
            // to the services container.
            services.AddRaven(new RavenOptions()
            {
                Url = "RavenDB server URL",
                Conventions = new DocumentConvention()
                {
                    // Add conventions here if required
                },
                Database = "Default" 
            });

```
NOTE: You can simply call services.AddRaven() to use default "http://localhost:8080"
