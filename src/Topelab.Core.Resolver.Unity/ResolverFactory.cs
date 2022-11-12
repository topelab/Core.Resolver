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
        private static Resolver rootResolver;

        /// <summary>
        /// Creates an IResolver based on the specified resolve info collection.
        /// </summary>
        /// <param name="resolveInfoCollection">The resolve info collection.</param>
        public static IResolver Create(ResolveInfoCollection resolveInfoCollection)
        {
            Dictionary<string, ConstructorInfo> constructorsByKey = new();

            UnityContainer container = new();
            if (resolveInfoCollection != null)
            {
                container.Register(resolveInfoCollection, constructorsByKey);
            }

            Resolver resolver = new(container, constructorsByKey);
            rootResolver ??= resolver;
            container.RegisterInstance<IResolver>(resolver);

            return resolver;
        }

        /// <summary>
        /// Get current resolver
        /// </summary>
        public static IResolver GetResolver() => rootResolver;
    }
}
