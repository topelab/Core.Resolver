namespace Topelab.Core.Resolver.Microsoft
{
    /// <summary>
    /// Interface for service factory
    /// </summary>
    public interface IServiceFactory
    {
        /// <summary>
        /// Get an object of type T where T is usually an interface
        /// </summary>
        T Get<T>();

        /// <summary>
        /// Create (an obviously transient) object of type T, with runtime parameters 'p'.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="p">Parameters.</param>
        T Create<T>(params object[] p);
    }
}
