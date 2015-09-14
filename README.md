# Microsoft.AspNet.RavenDB
Dependency Injection extensions for using RavenDB with ASPNET 5.

The easy way to automatically inject your DocumentSession into your ASPNET 5 (vNext) project.

To use:

1. Add reference to Microsoft.AspNet.RavenDB in your project.json.

2. Add this to your Startup.cs:

```
using Microsoft.AspNet.RavenDB;

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
