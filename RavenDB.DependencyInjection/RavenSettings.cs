﻿#if NETCOREAPP3_0
using Microsoft.Extensions.Hosting;
#elif NETSTANDARD2_0
using Microsoft.AspNetCore.Hosting;
#endif

namespace Raven.DependencyInjection
{
    /// <summary>
    /// Contains settings for RavenDB, such as the URL to the database.
    /// </summary>
    public class RavenSettings
    {
        /// <summary>
        /// The URLs where the database resides.
        /// </summary>
        public string[] Urls { get; set; }

        /// <summary>
        /// The name of the database.
        /// </summary>
        public string DatabaseName { get; set; }

#if NETCOREAPP3_0
        /// <summary>
        /// The file path to the PFX certificate to use to connect to Raven. This should be a path relative to <see cref="IHostEnvironment.ContentRootPath"/>. For example, if the cert is named foo.pfx and in the same directory as your deployed assemblies, this should be set to "foo.pfx". If null or empty, no certificate will be used.
        /// </summary>
#elif NETSTANDARD2_0
        /// <summary>
        /// The file path to the PFX certificate to use to connect to Raven. This should be a path relative to <see cref="IHostingEnvironment.ContentRootPath"/>. For example, if the cert is named foo.pfx and in the same directory as your deployed assemblies, this should be set to "foo.pfx". If null or empty, no certificate will be used.
        /// </summary>
#endif
        public string CertFilePath { get; set; }



        /// <summary>
        /// The password to use for the certificate.
        /// </summary>
        public string CertPassword { get; set; }
    }
}