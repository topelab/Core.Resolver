using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Interfaces;

namespace Topelab.Core.Resolver.Microsoft
{
    /// <summary>
    /// Resolver factory
    /// </summary>
    public static class ResolverFactory
    {
        private const string DefaultKey = "__NULL__";
        private static readonly IResolverStorage<IServiceProvider> resolversByService = new ResolverStorage<IServiceProvider>();

        internal static IResolverStorage<IServiceProvider> ResolversByService => resolversByService;

        /// <summary>
        /// Adds the resolver to service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="resolveInfoCollection">The resolve information collection.</param>
        public static IServiceCollection AddResolver(this IServiceCollection services, ResolveInfoCollection resolveInfoCollection)
        {
            if (resolveInfoCollection is null)
            {
                throw new ArgumentNullException(nameof(resolveInfoCollection));
            }
            IResolverStorage<string> globalResolvers = new ResolverStorage<string>();
            var resolveInfoCollectionWithDefaultKey = resolveInfoCollection.Where(r => (r.Key ?? DefaultKey) == DefaultKey);

            var collection = ServiceCollectionFactory.Create(resolveInfoCollectionWithDefaultKey, services);
            collection.AddScoped(s =>
            {
                var serviceFactory = s.GetService<IServiceFactory>();
                var resolver = serviceFactory.Create<IResolver>(s, serviceFactory, DefaultKey, globalResolvers);
                List<string> otherKeys = resolveInfoCollection.Select(r => r.Key ?? DefaultKey).Where(k => k != DefaultKey).Distinct().ToList();
                otherKeys.ForEach(key => Create(key, resolveInfoCollection, globalResolvers));
                return resolver;
            });
            return services;
        }

        /// <summary>
        /// Creates an IResolver based on the specified resolve info collection.
        /// </summary>
        /// <param name="resolveInfoCollection">The resolve info collection.</param>
        public static IResolver Create(ResolveInfoCollection resolveInfoCollection)
        {
            if (resolveInfoCollection is null)
            {
                throw new ArgumentNullException(nameof(resolveInfoCollection));
            }
            IResolverStorage<string> globalResolvers = new ResolverStorage<string>();
            List<string> keys = resolveInfoCollection.Select(r => r.Key ?? DefaultKey).Distinct().ToList();
            keys.ForEach(key => Create(key, resolveInfoCollection, globalResolvers));
            return globalResolvers.Where(r => r.Key == DefaultKey).Select(r => r.Value).FirstOrDefault() ?? globalResolvers.First().Value;
        }

        private static IResolver Create(string key, ResolveInfoCollection resolveInfoCollection, IResolverStorage<string> globalResolvers)
        {
            var resolveInfoCollectionWithKey = resolveInfoCollection.Where(r => (r.Key ?? DefaultKey) == key);
            var collection = ServiceCollectionFactory.Create(resolveInfoCollectionWithKey);

            var serviceProvider = collection.BuildServiceProvider();
            var serviceFactory = serviceProvider.GetService<IServiceFactory>();
            var resolver = serviceFactory.Create<IResolver>(serviceProvider, serviceFactory, key, globalResolvers);
            resolversByService[serviceProvider] = resolver;
            return resolver;
        }
    }

}
