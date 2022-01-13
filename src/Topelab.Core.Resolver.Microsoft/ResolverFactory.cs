using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Topelab.Core.Resolver.Enums;
using Topelab.Core.Resolver.Interfaces;
using System.Linq;
using Topelab.Core.Resolver.Entities;

namespace Topelab.Core.Resolver.Microsoft
{
    /// <summary>
    /// Resolver factory
    /// </summary>
    public static class ResolverFactory
    {
        private const string DefaultKey = "__NULL__";

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
            var collection = PrepareCollection(resolveInfoCollection.Where(r => (r.Key ?? DefaultKey) == key));
            var serviceProvider = collection.BuildServiceProvider();
            var serviceFactory = serviceProvider.GetService<IServiceFactory>();
            Resolver resolver = new(serviceProvider, serviceFactory, key, globalResolvers);
            return resolver;
        }

        private static IServiceCollection PrepareCollection(IEnumerable<ResolveInfo> resolveInfoCollection)
        {
            IServiceCollection collection = new ServiceCollection();

            collection
                .AddSingleton<IServiceFactory>(provider => new ServiceFactory(provider.GetService, (T, P) => ActivatorUtilities.CreateInstance(provider, T, P)))
                .AddScoped<IService<IResolver>, Service<IResolver,Resolver>>();

            resolveInfoCollection.ToList().ForEach(resolveInfo =>
            {
                switch (resolveInfo.ResolveType)
                {
                    case ResolveTypeEnum.Singleton:
                        if (resolveInfo.ConstructorParamTypes.Length > 0)
                        {
                            collection.AddSingleton(typeof(IService<>).MakeGenericType(resolveInfo.TypeFrom), typeof(Service<,>).MakeGenericType(resolveInfo.TypeFrom, resolveInfo.TypeTo));
                        }
                        else
                        {
                            collection.AddSingleton(resolveInfo.TypeFrom, resolveInfo.TypeTo);
                        }
                        break;
                    case ResolveTypeEnum.Instance:
                        collection.AddSingleton(resolveInfo.TypeFrom, resolveInfo.Instance);
                        break;
                    default:
                        if (resolveInfo.ConstructorParamTypes.Length > 0)
                        {
                            collection.AddScoped(typeof(IService<>).MakeGenericType(resolveInfo.TypeFrom), typeof(Service<,>).MakeGenericType(resolveInfo.TypeFrom, resolveInfo.TypeTo));
                        }
                        else
                        {
                            collection.AddScoped(resolveInfo.TypeFrom, resolveInfo.TypeTo);
                        }
                        break;
                }
            });

            return collection;
        }
    }

}
