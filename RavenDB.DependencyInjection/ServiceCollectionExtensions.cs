﻿using Microsoft.AspNetCore.Hosting;
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
        /// <param name="svc">The dependency injection services.</param>
        /// <returns>The dependency injection services.</returns>
        public static IServiceCollection AddRavenDbDocStore(this IServiceCollection svc)
        {
            return svc.AddSingleton(provider => CreateDocStore(provider));
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
        /// <param name="serviceCollection"> The <see cref="IServiceCollection" /> to add services to. </param>
        /// <remarks>Based on code from https://github.com/maqduni/AspNetCore.Identity.RavenDb/blob/master/src/Maqduni.AspNetCore.Identity.RavenDb/RavenDbServiceCollectionExtensions.cs</remarks>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddRavenDbAsyncSession(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddScoped(provider => 
                provider.GetRequiredService<IDocumentStore>().OpenAsyncSession());
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
        /// <param name="serviceCollection"> The <see cref="IServiceCollection" /> to add services to. </param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddRavenDbSession(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddScoped(provider => 
                provider.GetRequiredService<IDocumentStore>().OpenSession());
        }

        private static IDocumentStore CreateDocStore(IServiceProvider svcProvider)
        {
            var settings = svcProvider.GetRequiredService<IOptions<RavenSettings>>().Value;

            if (string.IsNullOrEmpty(settings.DatabaseName))
            {
                throw new InvalidOperationException("You haven't configured a DatabaseName. Ensure your appsettings.json contains a RavenSettings section.");
            }
            if (settings.Urls == null || settings.Urls.Length == 0)
            {
                throw new InvalidOperationException("You haven't configured your Raven database URLs. Ensure your appsettings.json contains a RavenSettings section.");
            }

            var dbUrl = settings.Urls;
            var dbName = settings.DatabaseName;
            var docStore = new DocumentStore
            {
                Urls = settings.Urls,
                Database = dbName
            };

            // Configure the certificate if we have one in app settings.
            var certRelativePath = settings.CertFilePath;
            if (!string.IsNullOrEmpty(certRelativePath))
            {
                var host = svcProvider.GetRequiredService<IHostingEnvironment>();
                var certFilePath = System.IO.Path.Combine(host.ContentRootPath, certRelativePath);
                if (!System.IO.File.Exists(certFilePath))
                {
                    throw new InvalidOperationException($"The Raven certificate file, {certRelativePath} is missing. Expected it at {certFilePath}.");
                }

                docStore.Certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(certFilePath, settings.CertPassword);
            }

            docStore.Initialize();
            return docStore;
        }
    }
}