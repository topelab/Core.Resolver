namespace Topelab.Core.Resolver.Microsoft
{
    public interface IServiceFactory
    {
        T Get<T>();

        T Create<T>(params object[] p);
    }

}
