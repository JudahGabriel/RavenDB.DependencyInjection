namespace Microsoft.Framework.DependencyInjection
{
    public class RavenServicesBuilder : IAccessor<IServiceCollection>
    {
        private readonly IServiceCollection _serviceCollection;
        public RavenServicesBuilder(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }
        IServiceCollection IAccessor<IServiceCollection>.Service => _serviceCollection;
    }
}
