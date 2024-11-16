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
        private static IResolver rootResolver;
        private static IResolver currentResolver;

        /// <summary>
        /// Adds the resolver to service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="resolveInfoCollection">The resolve information collection.</param>
        public static IServiceCollection AddResolver(this IServiceCollection services, ResolveInfoCollection resolveInfoCollection, Scope scope = null)
        {
            ArgumentNullException.ThrowIfNull(resolveInfoCollection);

            scope ??= Scope.Default;
            Dictionary<Type, Dictionary<string, Type>> namedResolutions = [];
            FillNamedResolutions(resolveInfoCollection, namedResolutions);
            var standardResolveInfoCollection = resolveInfoCollection.Where(IsStandard);

            var collection = ServiceCollectionFactory.Create(standardResolveInfoCollection, services);
            collection.AddSingleton(s => GetResolverImpl(s, resolveInfoCollection.Except(standardResolveInfoCollection), namedResolutions, scope));
            return services;
        }

        /// <summary>
        /// Creates an IResolver based on the specified resolve info collection.
        /// </summary>
        /// <param name="resolveInfoCollection">The resolve info collection.</param>
        public static IResolver Create(ResolveInfoCollection resolveInfoCollection, Scope scope = null)
        {
            ArgumentNullException.ThrowIfNull(resolveInfoCollection);

            scope ??= Scope.Default;
            Dictionary<Type, Dictionary<string, Type>> namedResolutions = [];
            FillNamedResolutions(resolveInfoCollection, namedResolutions);
            var standardResolveInfoCollection = resolveInfoCollection.Where(IsStandard);

            var collection = ServiceCollectionFactory.Create(standardResolveInfoCollection);
            collection.AddSingleton(s => GetResolverImpl(s, resolveInfoCollection.Except(standardResolveInfoCollection), namedResolutions, scope));

            var serviceProvider = collection.BuildServiceProvider();
            Resolver resolver = (Resolver)serviceProvider.GetService<IResolver>();
            resolveInfoCollection.InitializeIntializers(resolver);
            rootResolver ??= resolver;
            currentResolver = resolver;
            return resolver;
        }

        /// <summary>
        /// Get current resolver
        /// </summary>
        public static IResolver GetResolver(Scope scope = null) => (scope ?? Scope.Default).Resolver;

        /// <summary>
        /// Select new scope and change currentResolver
        /// </summary>
        /// <param name="scope">New scope to select</param>
        public static void SelectScope(Scope scope = null)
        {
            scope ??= Scope.Default;
            if (scope.Resolver == null)
            {
                throw new System.Exception($"Scope {scope.Tag} doesn't have a resolver. Create a resolver with this scope");
            }
            currentResolver = (scope ?? Scope.Default).Resolver;
        }

        /// <summary>
        /// Resolve type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        public static T Resolve<T>() where T : class => currentResolver.Get<T>();

        /// <summary>
        /// Resolve type <typeparamref name="T"/> using key and params
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <param name="args">Params to ctor</param>
        public static T Resolve<T>(params object[] args) where T : class => currentResolver.Get<T>(args);

        /// <summary>
        /// Resolve type <typeparamref name="T"/> using key
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <param name="key">Key to resolve</param>
        public static T Resolve<T>(string key) where T : class => currentResolver.Get<T>(key);

        /// <summary>
        /// Resolve type <typeparamref name="T"/> using key and params
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <param name="key">Key to resolve</param>
        /// <param name="args">Params to ctor</param>
        public static T Resolve<T>(string key, params object[] args) where T : class => currentResolver.Get<T>(key, args);

        private static bool IsStandard(ResolveInfo r)
        {
            return (r.ResolveMode == Enums.ResolveModeEnum.None
                || (r.ResolveMode != Enums.ResolveModeEnum.None && (r.Key ?? DefaultKey) == DefaultKey))
                && (r.ConstructorParamTypes == null || r.ConstructorParamTypes.Length == 0);
        }

        private static IResolver GetResolverImpl(IServiceProvider serviceProvider, IEnumerable<ResolveInfo> resolveInfoCollection, Dictionary<Type, Dictionary<string, Type>> namedResolutions, Scope scope)
        {
            Resolver resolver = new(serviceProvider, DefaultKey, namedResolutions, scope);
            List<string> otherKeys = resolveInfoCollection.Select(r => r.Key ?? DefaultKey).Where(k => k != DefaultKey).Distinct().ToList();
            otherKeys.ForEach(key => Create(key, resolveInfoCollection, namedResolutions, scope));
            return resolver;
        }

        private static Resolver Create(string key, IEnumerable<ResolveInfo> resolveInfoCollection, Dictionary<Type, Dictionary<string, Type>> namedResolutions, Scope scope)
        {
            var resolveInfoCollectionWithKey = resolveInfoCollection.Where(r => (r.Key ?? DefaultKey) == key);
            var collection = ServiceCollectionFactory.Create(resolveInfoCollectionWithKey);

            var serviceProvider = collection.BuildServiceProvider();
            Resolver resolver = new(serviceProvider, key, namedResolutions, scope);
            return resolver;
        }

        private static void FillNamedResolutions(ResolveInfoCollection resolveInfoCollection, Dictionary<Type, Dictionary<string, Type>> namedResolutions)
        {
            var resolveInfoCollecionWithKey = resolveInfoCollection.Where(r => r.ResolveMode == Enums.ResolveModeEnum.None).Where(r => (r.Key ?? DefaultKey) != DefaultKey);
            foreach (var resolveInfo in resolveInfoCollecionWithKey)
            {
                if (!namedResolutions.TryGetValue(resolveInfo.TypeFrom, out var value))
                {
                    value = [];
                    namedResolutions[resolveInfo.TypeFrom] = value;
                }

                value[resolveInfo.Key] = resolveInfo.TypeTo;
            }
        }
    }

}
