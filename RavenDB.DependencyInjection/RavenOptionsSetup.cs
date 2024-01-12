using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Raven.Client.Documents;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Raven.DependencyInjection
{
    /// <summary>
    /// The configurations for <see cref="RavenOptions"/>.
    /// </summary>
    public class RavenOptionsSetup : IConfigureOptions<RavenOptions>, IPostConfigureOptions<RavenOptions>
    {
        private readonly IHostEnvironment host;
        private readonly IConfiguration config;
        private RavenOptions? options;

        /// <summary>
        /// The constructor for <see cref="RavenOptionsSetup"/>.
        /// </summary>
        /// <param name="hosting"></param>
        /// <param name="configuration"></param>
        public RavenOptionsSetup(
            IHostEnvironment hosting,
            IConfiguration configuration)
        {
            host = hosting;
            config = configuration;
        }

        /// <summary>
        /// The default configuration if needed.
        /// </summary>
        /// <param name="options"></param>
        public void Configure(RavenOptions options)
        {
            if (options.Settings == null)
            {
                var settings = new RavenSettings();
                config.Bind(options.SectionName, settings);

                options.Settings = settings;
            }

            if (options.HostEnvironment == null)
            {
                options.HostEnvironment = host;
            }

            if (options.GetConfiguration == null)
            {
                options.GetConfiguration = config;
            }
        }

        /// <summary>
        /// Post configuration for <see cref="RavenOptions"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        public void PostConfigure(string? name, RavenOptions options)
        {
            this.options = options;

            if (options.Certificate == null)
            {
                options.Certificate = GetCertificateFromFileSystem();
                this.options.Certificate = options.Certificate;
            }

            if (options.GetDocumentStore == null)
            {
                options.GetDocumentStore = GetDocumentStore;
            }
        }

        private IDocumentStore GetDocumentStore(Action<IDocumentStore>? configureDbStore)
        {
            if (string.IsNullOrEmpty(options?.Settings?.DatabaseName))
            {
                throw new InvalidOperationException("You haven't configured a DatabaseName. Ensure your appsettings.json contains a RavenSettings section.");
            }
            if (options.Settings.Urls == null || options.Settings.Urls.Length == 0)
            {
                throw new InvalidOperationException("You haven't configured your Raven database URLs. Ensure your appsettings.json contains a RavenSettings section.");
            }

            var documentStore = new DocumentStore
            {
                Urls = options.Settings.Urls,
                Database = options.Settings.DatabaseName
            };

            if (options.Certificate != null)
            {
                documentStore.Certificate = options.Certificate;
            }

            configureDbStore?.Invoke(documentStore);

            documentStore.Initialize();

            return documentStore;
        }

        private X509Certificate2? GetCertificateFromFileSystem()
        {
            var certRelativePath = options?.Settings?.CertFilePath;

            if (!string.IsNullOrEmpty(certRelativePath))
            {
                var certFilePath = Path.Combine(options?.HostEnvironment?.ContentRootPath ?? string.Empty, certRelativePath);
                if (!File.Exists(certFilePath))
                {
                    throw new InvalidOperationException($"The Raven certificate file, {certRelativePath} is missing. Expected it at {certFilePath}.");
                }

                return new X509Certificate2(certFilePath, options?.Settings?.CertPassword);
            }

            return null;
        }
    }
}
