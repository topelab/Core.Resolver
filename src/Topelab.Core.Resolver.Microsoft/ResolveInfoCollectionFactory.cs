using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Enums;

namespace Topelab.Core.Resolver.Microsoft
{
    /// <summary>
    /// Internal static class to merge ResolveInfoCollection with IServiceCollection
    /// </summary>
    public static class ResolveInfoCollectionFactory
    {
        /// <summary>
        /// Adds a factory to resolve info collection.
        /// </summary>
        /// <typeparam name="TOut">The out type.</typeparam>
        /// <param name="resolveInfoCollection">The resolve information collection.</param>
        /// <param name="factory">The factory.</param>
        public static ResolveInfoCollection AddFactory<TOut>(this ResolveInfoCollection resolveInfoCollection, Func<IServiceProvider, TOut> factory)
        {
            return resolveInfoCollection.AddFactory(null, factory);
        }

        /// <summary>
        /// Adds a named factory to resolve info collection.
        /// </summary>
        /// <typeparam name="TOut">The out type.</typeparam>
        /// <param name="resolveInfoCollection">The resolve information collection.</param>
        /// <param name="key">The name for factory resolution</param>
        /// <param name="factory">The factory.</param>
        /// <returns></returns>
        public static ResolveInfoCollection AddFactory<TOut>(this ResolveInfoCollection resolveInfoCollection, string key, Func<IServiceProvider, TOut> factory)
        {
            resolveInfoCollection.Add(new ResolveInfo(typeof(TOut), typeof(TOut), ResolveModeEnum.SpecificFactory, ResolveLifeCycleEnum.Singleton, key) { Instance = factory });
            return resolveInfoCollection;
        }

        /// <summary>
        /// Merges the specified services and resolve info collection.
        /// </summary>
        /// <param name="resolveInfoCollection">The resolve information collection.</param>
        /// <param name="services">The services.</param>
        internal static void Merge(this ResolveInfoCollection resolveInfoCollection, IServiceCollection services)
        {
            services.ToResolveInfoCollection().ForEach(source =>
            {
                if (!resolveInfoCollection.Any(dest => dest.Key == source.Key))
                {
                    resolveInfoCollection.Add(source);
                }
            });
        }

        private static ResolveInfoCollection ToResolveInfoCollection(this IServiceCollection services)
        {
            ResolveInfoCollection result = new();

            foreach (var service in services)
            {
                result.Add(ToResolveInfo(service));
            }

            return result;
        }

        private static ResolveInfo ToResolveInfo(ServiceDescriptor service)
        {
            ResolveLifeCycleEnum resolveType;
            var resolveMode = ResolveModeEnum.None;
            Type typeTo;
            object instance;

            if (service.ImplementationFactory != null)
            {
                resolveType = ResolveLifeCycleEnum.Singleton;
                resolveMode = ResolveModeEnum.Instance;
                typeTo = service.ImplementationFactory.GetType();
                instance = service.ImplementationFactory;
            }
            else
            {
                switch (service.Lifetime)
                {
                    case ServiceLifetime.Singleton:
                        resolveType = ResolveLifeCycleEnum.Singleton;
                        break;
                    case ServiceLifetime.Scoped:
                        resolveType = ResolveLifeCycleEnum.Scoped;
                        break;
                    default:
                        resolveType = ResolveLifeCycleEnum.Transient;
                        break;
                }

                typeTo = service.ImplementationType ?? service.ImplementationInstance.GetType();
                instance = service.ImplementationInstance;
            }

            return new ResolveInfo(service.ServiceType, typeTo, resolveMode, resolveType) { Instance = instance };
        }
    }
}
