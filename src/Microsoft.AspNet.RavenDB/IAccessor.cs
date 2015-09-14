namespace Microsoft.Framework.DependencyInjection
{
    public interface IAccessor<out TService>
    {
        TService Service { get; }
    }
}
