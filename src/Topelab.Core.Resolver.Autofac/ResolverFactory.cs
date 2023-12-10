using Autofac;
using System.Collections.Generic;
using System.Reflection;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Interfaces;

namespace Topelab.Core.Resolver.Autofac
{
    /// <summary>
    /// Resolver factory
    /// </summary>
    public static class ResolverFactory
    {
        private static Resolver rootResolver;

        /// <summary>
        /// Creates an IResolver based on the specified resolve info collection.
        /// </summary>
        /// <param name="resolveInfoCollection">The resolve info collection.</param>
        public static IResolver Create(ResolveInfoCollection resolveInfoCollection)
        {
            Dictionary<string, ConstructorInfo> constructorsByKey = new();

            ContainerBuilder builder = new();
            if (resolveInfoCollection != null)
            {
                builder.RegisterAutofac(resolveInfoCollection, constructorsByKey);
            }

            var resolver = new Resolver();
            builder.RegisterInstance((IResolver)resolver);
            var container = builder.Build();
            resolver.Initialize(container, constructorsByKey);
            resolveInfoCollection.InitializeIntializers(resolver);
            rootResolver ??= resolver;
            return resolver;
        }

        /// <summary>
        /// Get current resolver
        /// </summary>
        public static IResolver GetResolver() => rootResolver;

        /// <summary>
        /// Resolve type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        public static T Resolve<T>() where T : class => rootResolver.Get<T>();

        /// <summary>
        /// Resolve type <typeparamref name="T"/> using key and params
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <param name="args">Params to ctor</param>
        public static T Resolve<T>(params object[] args) where T : class => rootResolver.Get<T>(args);

        /// <summary>
        /// Resolve type <typeparamref name="T"/> using key
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <param name="key">Key to resolve</param>
        public static T Resolve<T>(string key) where T : class => rootResolver.Get<T>(key);

        /// <summary>
        /// Resolve type <typeparamref name="T"/> using key and params
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <param name="key">Key to resolve</param>
        /// <param name="args">Params to ctor</param>
        public static T Resolve<T>(string key, params object[] args) where T : class => rootResolver.Get<T>(key, args);
    }
}
