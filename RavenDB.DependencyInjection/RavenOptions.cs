﻿using Microsoft.Extensions.Configuration;
using Raven.Client.Documents;
using System;
using System.Security.Cryptography.X509Certificates;

#if NETCOREAPP3_0
using Microsoft.Extensions.Hosting;
#elif NETSTANDARD2_0
using Microsoft.AspNetCore.Hosting;
#endif

namespace Raven.DependencyInjection
{
    /// <summary>
    /// The Options to configure <see cref="IDocumentStore"/>.
    /// </summary>
    public class RavenOptions
    {
        /// <summary>
        /// The Raven Db basic configuration information.
        /// The default configuration information is loaded via <see cref="SectionName"/> parameter.
        /// </summary>
        public RavenSettings Settings { get; set; }

        /// <summary>
        /// The name of the configuration section for <see cref="RavenSettings"/>.
        /// The default value is <see cref="RavenSettings"/>.
        /// </summary>
        public string SectionName { get; set; } = nameof(RavenSettings);

        /// <summary>
        /// Gets the <see cref="IConfiguration"/> object.
        /// The default value is set to context of the execution.
        /// </summary>
        public IConfiguration GetConfiguration { get; set; }

#if NETCOREAPP3_0
        /// <summary>
        /// The default value is set to <see cref="IHostEnvironment"/>.
        /// </summary>
        public IHostEnvironment GetHostingEnvironment { get; set; }
#elif NETSTANDARD2_0
        /// <summary>
        /// The default value is set to <see cref="IHostingEnvironment"/>.
        /// This will change with AspNetCore 3.0 version.
        /// </summary>
        public IHostingEnvironment GetHostingEnvironment { get; set; }
#endif

        /// <summary>
        /// The certificate file for the <see cref="IDocumentStore"/>.
        /// </summary>
        public X509Certificate2 Certificate { get; set; }

        /// <summary>
        /// Gets instance of the <see cref="IDocumentStore"/>.
        /// </summary>
        public Func<Action<IDocumentStore>, IDocumentStore> GetDocumentStore { get; set; }

        /// <summary>
        /// Action executed on the document store prior to calling docStore.Initialize(...). 
        /// This should be used to configure RavenDB conventions.
        /// </summary>
        /// <example>
        ///     <code>
        ///         services.AddRavenDbDocStore(options =>
        ///         {
        ///             options.BeforeInitializeDocStore = docStore => docStore.Conventions.IdentityPartsSeparator = "-";
        ///         }
        ///     </code>
        /// </example>
        public Action<IDocumentStore> BeforeInitializeDocStore { get; set; }
    }
}
