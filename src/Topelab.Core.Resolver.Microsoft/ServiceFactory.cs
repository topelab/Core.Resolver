using System;

namespace Topelab.Core.Resolver.Microsoft
{
    /// <summary>
    /// Service Factory
    /// </summary>
    public class ServiceFactory : IServiceFactory
    {
        private readonly Func<Type, object> factory;
        private readonly Func<Type, object[], object> creator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceFactory"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="creator">The creator.</param>
        public ServiceFactory(Func<Type, object> factory, Func<Type, object[], object> creator)
        {
            this.factory = factory;
            this.creator = creator;
        }

        /// <summary>
        /// Get an object of type T where T is usually an interface
        /// </summary>
        public T Get<T>()
        {
            return (T)factory(typeof(T));
        }

        /// <summary>
        /// Create (an obviously transient) object of type T, with runtime parameters 'p'.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="p">Parameters.</param>
        public T Create<T>(params object[] p)
        {
            var lookup = Get<IService<T>>();
            return (T)creator(lookup.Type(), p);
        }
    }

}
