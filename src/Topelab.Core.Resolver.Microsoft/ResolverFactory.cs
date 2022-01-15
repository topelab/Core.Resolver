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

        /// <summary>
        /// Adds the resolver to service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="resolveInfoCollection">The resolve information collection.</param>
        public static IServiceCollection AddResolver(this IServiceCollection services, ResolveInfoCollection resolveInfoCollection)
        {
            if (services.Any(s => s.ServiceType == typeof(IResolver)))
            {
                resolveInfoCollection.Merge(services);
            }
            var resolver = Create(resolveInfoCollection);
            services.AddSingleton(typeof(IResolver), resolver);
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
            Dictionary<string, IResolver> globalResolvers = new();
            List<string> keys = resolveInfoCollection.Select(r => r.Key ?? DefaultKey).Distinct().ToList();
            keys.ForEach(key => Create(key, resolveInfoCollection, globalResolvers));
            return globalResolvers.Where(r => r.Key == DefaultKey).Select(r => r.Value).FirstOrDefault() ?? globalResolvers.First().Value;
        }

        private static IResolver Create(string key, ResolveInfoCollection resolveInfoCollection, Dictionary<string, IResolver> globalResolvers)
        {
            var collection = resolveInfoCollection
                .Where(r => (r.Key ?? DefaultKey) == key)
                .CreateServiceCollection();

            var serviceProvider = collection.BuildServiceProvider();
            var serviceFactory = serviceProvider.GetService<IServiceFactory>();
            Resolver resolver = new(serviceProvider, serviceFactory, key, globalResolvers);
            return resolver;
        }
    }

}
