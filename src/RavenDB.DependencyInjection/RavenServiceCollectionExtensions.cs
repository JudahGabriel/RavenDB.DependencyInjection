using Microsoft.Framework.DependencyInjection.Extensions;
using Raven.Client;
using Raven.Client.Document;

namespace Microsoft.Framework.DependencyInjection
{
    public static class RavenServiceCollectionExtensions
    {
        //
        // Summary:
        //     Adds the default RavenDB services (IDocumentStore and IAsyncDocumentSession)
        //     to the services container.
        public static RavenServicesBuilder AddRaven(
            this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddRaven(new RavenOptions());
        }
        public static RavenServicesBuilder AddRaven(
            this IServiceCollection serviceCollection,
            RavenOptions options)
        {
            serviceCollection.TryAdd(new ServiceCollection()
                .AddSingleton<IDocumentStore>
                (p => new DocumentStore
                {
                    Url = options.Url != null ? options.Url : "http://localhost:8080",
                    DefaultDatabase = options.Database != null ? options.Database : "",
                    Conventions = options.Conventions != null ? options.Conventions : new DocumentConvention(),
                    ApiKey = options.ApiKey != null ? options.ApiKey : "",
                    Credentials = options.Credentials != null ? options.Credentials : null
                }.Initialize())
                .AddScoped<IAsyncDocumentSession>
                (p => p.GetRequiredService<IDocumentStore>().OpenAsyncSession()));

            return new RavenServicesBuilder(serviceCollection);
        }
    }
}
