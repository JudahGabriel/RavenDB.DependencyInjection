using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;

namespace Raven.DependencyInjection
{
    /// <summary>
    /// Extends the <see cref="IServiceCollection"/> so that RavenDB services can be registered through it.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a Raven <see cref="IDocumentStore"/> singleton to the dependency injection services.
        /// The document store is configured using a <see cref="RavenSettings"/> section in your appsettings.json file.
        /// </summary>
        /// <param name="services">The dependency injection services.</param>
        /// <returns>The dependency injection services.</returns>
        public static IServiceCollection AddRavenDbDocStore(this IServiceCollection services)
        {
            return services.AddRavenDbDocStore(options: options => { });
        }

        /// <summary>
        /// Adds a Raven <see cref="IDocumentStore"/> singleton to the dependency injection services.
        /// The document store is configured using a <see cref="RavenSettings"/> section in your appsettings.json file.
        /// </summary>
        /// <param name="services">The dependency injection services.</param>
        /// <param name="configure">A method that will be used to configure the document store.</param>
        /// <returns>The dependency injection services.</returns>
        public static IServiceCollection AddRavenDbDocStore(
            this IServiceCollection services,
            Action<IDocumentStore> configure)
        {
            return services.AddRavenDbDocStore(options: options =>
            {
                options.ConfigureDocumentStore = configure;
            });
        }

        /// <summary>
        /// Adds a Raven <see cref="IDocumentStore"/> singleton to the dependency injection services.
        /// The document store is configured based on the <see cref="RavenOptions"/> action configuration.
        /// </summary>
        /// <param name="services">The dependency injection services.</param>
        /// <param name="options">The configuration for the <see cref="RavenOptions"/></param>
        /// <returns>The dependency injection services.</returns>
        public static IServiceCollection AddRavenDbDocStore(
            this IServiceCollection services,
            Action<RavenOptions> options)
        {
            services.ConfigureOptions<RavenOptionsSetup>();

            services.Configure(options);

            services.AddSingleton(sp =>
            {
                var setup = sp.GetRequiredService<IOptions<RavenOptions>>().Value;

                return setup.GetDocumentStore(setup.ConfigureDocumentStore);
            });

            return services;
        }

        /// <summary>
        /// Registers a RavenDB <see cref="IAsyncDocumentSession"/> to be created and disposed on each request.
        /// This requires for an <see cref="IDocumentStore"/> to be added to dependency injection services.
        /// </summary>
        /// <example>
        ///     <code>
        ///         public void ConfigureServices(IServiceCollection services)
        ///         {
        ///             services.AddRavenDbAsyncSession();
        ///         }
        ///     </code>
        /// </example>
        /// <param name="services"> The <see cref="IServiceCollection" /> to add services to. </param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddRavenDbAsyncSession(this IServiceCollection services)
        {
            return services.AddScoped(sp => sp.GetRequiredService<IDocumentStore>().OpenAsyncSession());
        }

        /// <summary>
        /// Registers a RavenDB <see cref="IAsyncDocumentSession"/> to be created and disposed on each request.
        /// </summary>
        /// <example>
        ///     <code>
        ///         public void ConfigureServices(IServiceCollection services)
        ///         {
        ///             services.AddRavenDbAsyncSession(docStore);
        ///         }
        ///     </code>
        /// </example>
        /// <param name="services"> The <see cref="IServiceCollection" /> to add services to. </param>
        /// <param name="docStore">The <see cref="IDocumentStore"/> to use in creating the session.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddRavenDbAsyncSession(
            this IServiceCollection services,
            IDocumentStore docStore)
        {
            return services.AddScoped(_ => docStore.OpenAsyncSession());
        }

        /// <summary>
        /// Registers a RavenDB <see cref="IDocumentSession"/> to be created and disposed on each request.
        /// This requires for an <see cref="IDocumentStore"/> to be added to dependency injection services.
        /// </summary>
        /// <example>
        ///     <code>
        ///         public void ConfigureServices(IServiceCollection services)
        ///         {
        ///             services.AddRavenDbAsyncSession();
        ///         }
        ///     </code>
        /// </example>
        /// <param name="services"> The <see cref="IServiceCollection" /> to add services to. </param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddRavenDbSession(this IServiceCollection services)
        {
            return services.AddScoped(sp => sp.GetRequiredService<IDocumentStore>().OpenSession());
        }

        /// <summary>
        /// Registers a RavenDB <see cref="IDocumentSession"/> to be created and disposed on each request.
        /// </summary>
        /// <example>
        ///     <code>
        ///         public void ConfigureServices(IServiceCollection services)
        ///         {
        ///             services.AddRavenDbAsyncSession();
        ///         }
        ///     </code>
        /// </example>
        /// <param name="services"> The <see cref="IServiceCollection" /> to add services to. </param>
        /// <param name="docStore">The <see cref="IDocumentStore"/> to use to create the <see cref="IDocumentSession"/></param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddRavenDbSession(
            this IServiceCollection services,
            IDocumentStore docStore)
        {
            return services.AddScoped(_ => docStore.OpenSession());
        }

    }
}
