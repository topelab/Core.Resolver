using System.Collections.Generic;
using System.Reflection;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Interfaces;
using Unity;

namespace Topelab.Core.Resolver.Unity
{
    /// <summary>
    /// Resolver factory
    /// </summary>
    public static class ResolverFactory
    {
        private static IResolver rootResolver;
        private static IResolver currentResolver;

        /// <summary>
        /// Creates an IResolver based on the specified resolve info collection.
        /// </summary>
        /// <param name="resolveInfoCollection">The resolve info collection.</param>
        public static IResolver Create(ResolveInfoCollection resolveInfoCollection, Scope scope = null)
        {
            scope ??= Scope.Default;
            Dictionary<string, ConstructorInfo> constructorsByKey = new();

            UnityContainer container = new();
            if (resolveInfoCollection != null)
            {
                container.Register(resolveInfoCollection, constructorsByKey);
            }

            Resolver resolver = new(container, constructorsByKey, scope);
            resolveInfoCollection.InitializeIntializers(resolver);
            rootResolver ??= resolver;
            currentResolver = resolver;
            container.RegisterInstance<IResolver>(resolver);

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
    }
}
