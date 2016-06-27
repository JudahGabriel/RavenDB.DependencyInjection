using System.Net;
using Raven.Client.Document;

namespace Microsoft.Extensions.DependencyInjection
{
    public class RavenOptions
    {
        public string Url { get; set; }
        public string Database { get; set; }
        public DocumentConvention Conventions { get; set; }
        public string ApiKey { get; set; }
        public ICredentials Credentials { get; set; }
    }
}
