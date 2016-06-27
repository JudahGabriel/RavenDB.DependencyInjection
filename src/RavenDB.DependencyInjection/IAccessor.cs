namespace Microsoft.Extensions.DependencyInjection
{
    public interface IAccessor<out TService>
    {
        TService Service { get; }
    }
}
