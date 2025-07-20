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
            var collection = ServiceCollectionFactory.Create(resolveInfoCollection, services);
            collection.AddSingleton(s => GetResolverImpl(s, resolveInfoCollection, namedResolutions, scope));
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
            var collection = ServiceCollectionFactory.Create(resolveInfoCollection);
            collection.AddSingleton(s => GetResolverImpl(s, resolveInfoCollection, namedResolutions, scope));

            var serviceProvider = collection.BuildServiceProvider();
            Resolver resolver = (Resolver)serviceProvider.GetService<IResolver>();
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

        private static IResolver GetResolverImpl(IServiceProvider serviceProvider, ResolveInfoCollection resolveInfoCollection, Dictionary<Type, Dictionary<string, Type>> namedResolutions, Scope scope)
        {
            Resolver resolver = new(serviceProvider, namedResolutions, scope);
            resolveInfoCollection.InitializeIntializers(resolver);
            return resolver;
        }


        private static void FillNamedResolutions(ResolveInfoCollection resolveInfoCollection, Dictionary<Type, Dictionary<string, Type>> namedResolutions)
        {
            foreach (var resolveInfo in resolveInfoCollection)
            {
                if (!namedResolutions.TryGetValue(resolveInfo.TypeFrom, out var value))
                {
                    value = [];
                    namedResolutions[resolveInfo.TypeFrom] = value;
                }

                var key = resolveInfo.Key ?? string.Empty;
                value[key] = resolveInfo.TypeTo;
            }
        }
    }

}
