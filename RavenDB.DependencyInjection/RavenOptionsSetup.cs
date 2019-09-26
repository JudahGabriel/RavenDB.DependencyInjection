﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Raven.Client.Documents;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

#if NETCOREAPP3_0
using Microsoft.Extensions.Hosting;
#elif NETSTANDARD2_0
using Microsoft.AspNetCore.Hosting;
#endif

namespace Raven.DependencyInjection
{
    /// <summary>
    /// The configurations for <see cref="RavenOptions"/>.
    /// </summary>
    public class RavenOptionsSetup : IConfigureOptions<RavenOptions>, IPostConfigureOptions<RavenOptions>
    {

#if NETCOREAPP3_0
        private readonly IHostEnvironment _hosting;
#elif NETSTANDARD2_0
        private readonly IHostingEnvironment _hosting;
#endif

        private readonly IConfiguration _configuration;
        private RavenOptions _options;

        /// <summary>
        /// The constructor for <see cref="RavenOptionsSetup"/>.
        /// </summary>
        /// <param name="hosting"></param>
        /// <param name="configuration"></param>
#if NETCOREAPP3_0
        public RavenOptionsSetup(IHostEnvironment hosting, IConfiguration configuration)
#elif NETSTANDARD2_0
        public RavenOptionsSetup(IHostingEnvironment hosting, IConfiguration configuration)
#endif
        
        {
            _hosting = hosting;
            _configuration = configuration;
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
                _configuration.Bind(options.SectionName, settings);

                options.Settings = settings;
            }

            if (options.GetHostingEnvironment == null)
            {
                options.GetHostingEnvironment = _hosting;
            }

            if (options.GetConfiguration == null)
            {
                options.GetConfiguration = _configuration;
            }
        }

        /// <summary>
        /// Post configuration for <see cref="RavenOptions"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        public void PostConfigure(string name, RavenOptions options)
        {
            _options = options;

            if (options.Certificate == null)
            {
                options.Certificate = GetCertificateFromFileSystem();
                _options.Certificate = options.Certificate;
            }

            if (options.GetDocumentStore == null)
            {
                options.GetDocumentStore = GetDocumentStore;
            }
        }

        private IDocumentStore GetDocumentStore(Action<IDocumentStore> configureDbStore)
        {
            if (string.IsNullOrEmpty(_options.Settings.DatabaseName))
            {
                throw new InvalidOperationException("You haven't configured a DatabaseName. Ensure your appsettings.json contains a RavenSettings section.");
            }
            if (_options.Settings.Urls == null || _options.Settings.Urls.Length == 0)
            {
                throw new InvalidOperationException("You haven't configured your Raven database URLs. Ensure your appsettings.json contains a RavenSettings section.");
            }

            var documentStore = new DocumentStore
            {
                Urls = _options.Settings.Urls,
                Database = _options.Settings.DatabaseName
            };

            if (_options.Certificate != null)
            {
                documentStore.Certificate = _options.Certificate;
            }

            configureDbStore?.Invoke(documentStore);

            documentStore.Initialize();

            return documentStore;
        }

        private X509Certificate2 GetCertificateFromFileSystem()
        {
            var certRelativePath = _options.Settings.CertFilePath;

            if (!string.IsNullOrEmpty(certRelativePath))
            {
                var certFilePath = Path.Combine(_options.GetHostingEnvironment.ContentRootPath, certRelativePath);
                if (!File.Exists(certFilePath))
                {
                    throw new InvalidOperationException($"The Raven certificate file, {certRelativePath} is missing. Expected it at {certFilePath}.");
                }

                return new X509Certificate2(certFilePath, _options.Settings.CertPassword);
            }

            return null;
        }
    }
}
