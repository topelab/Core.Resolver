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
        private static Resolver rootResolver;

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
            IDictionary<string, IResolver> globalResolvers = new Dictionary<string, IResolver>();
            Dictionary<Type, Dictionary<string, Type>> namedResolutions = new();
            FillNamedResolutions(resolveInfoCollection, namedResolutions);
            var standardResolveInfoCollection = resolveInfoCollection.Where(r => IsStandard(r));

            var collection = ServiceCollectionFactory.Create(standardResolveInfoCollection, services);
            collection.AddSingleton(s => GetResolverImpl(s, resolveInfoCollection.Except(standardResolveInfoCollection), globalResolvers, namedResolutions));
            return services;
        }

        private static bool IsStandard(ResolveInfo r)
        {
            return (r.ResolveMode == Enums.ResolveModeEnum.None
                || (r.ResolveMode != Enums.ResolveModeEnum.None && (r.Key ?? DefaultKey) == DefaultKey))
                && (r.ConstructorParamTypes == null || r.ConstructorParamTypes.Length == 0);
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
            IDictionary<string, IResolver> globalResolvers = new Dictionary<string, IResolver>();
            Dictionary<Type, Dictionary<string, Type>> namedResolutions = new();
            FillNamedResolutions(resolveInfoCollection, namedResolutions);
            var standardResolveInfoCollection = resolveInfoCollection.Where(r => IsStandard(r));

            var collection = ServiceCollectionFactory.Create(standardResolveInfoCollection);
            collection.AddSingleton(s => GetResolverImpl(s, resolveInfoCollection.Except(standardResolveInfoCollection), globalResolvers, namedResolutions));

            var serviceProvider = collection.BuildServiceProvider();
            Resolver resolver = (Resolver)serviceProvider.GetService<IResolver>();
            resolveInfoCollection.InitializeIntializers(resolver);
            rootResolver ??= resolver;
            return resolver;
        }

        /// <summary>
        /// Get current resolver
        /// </summary>
        public static IResolver GetResolver() => rootResolver;

        private static IResolver GetResolverImpl(IServiceProvider serviceProvider, IEnumerable<ResolveInfo> resolveInfoCollection, IDictionary<string, IResolver> globalResolvers, Dictionary<Type, Dictionary<string, Type>> namedResolutions)
        {
            var resolver = new Resolver(serviceProvider, DefaultKey, globalResolvers, namedResolutions);
            List<string> otherKeys = resolveInfoCollection.Select(r => r.Key ?? DefaultKey).Where(k => k != DefaultKey).Distinct().ToList();
            otherKeys.ForEach(key => Create(key, resolveInfoCollection, globalResolvers, namedResolutions));
            return resolver;
        }

        private static IResolver Create(string key, IEnumerable<ResolveInfo> resolveInfoCollection, IDictionary<string, IResolver> globalResolvers, Dictionary<Type, Dictionary<string, Type>> namedResolutions)
        {
            var resolveInfoCollectionWithKey = resolveInfoCollection.Where(r => (r.Key ?? DefaultKey) == key);
            var collection = ServiceCollectionFactory.Create(resolveInfoCollectionWithKey);

            var serviceProvider = collection.BuildServiceProvider();
            var resolver = new Resolver(serviceProvider, key, globalResolvers, namedResolutions);
            return resolver;
        }

        private static void FillNamedResolutions(ResolveInfoCollection resolveInfoCollection, Dictionary<Type, Dictionary<string, Type>> namedResolutions)
        {
            var resolveInfoCollecionWithKey = resolveInfoCollection.Where(r => r.ResolveMode == Enums.ResolveModeEnum.None).Where(r => (r.Key ?? DefaultKey) != DefaultKey);
            foreach (var resolveInfo in resolveInfoCollecionWithKey)
            {
                if (!namedResolutions.ContainsKey(resolveInfo.TypeFrom))
                {
                    namedResolutions[resolveInfo.TypeFrom] = new();
                }
                namedResolutions[resolveInfo.TypeFrom][resolveInfo.Key] = resolveInfo.TypeTo;
            }
        }
    }

}
