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
        private static Resolver resolver;

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

            resolver = new Resolver();
            builder.RegisterInstance((IResolver)resolver);
            var container = builder.Build();
            resolver.Initialize(container, constructorsByKey);
            return resolver;
        }

        /// <summary>
        /// Get current resolver
        /// </summary>
        public static IResolver GetResolver() => resolver;
    }
}
