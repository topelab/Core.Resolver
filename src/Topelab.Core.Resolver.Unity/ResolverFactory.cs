using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Enums;
using Topelab.Core.Resolver.Interfaces;
using Unity;

namespace Topelab.Core.Resolver.Unity
{
    /// <summary>
    /// Resolver factory
    /// </summary>
    public static class ResolverFactory
    {
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
            container.RegisterInstance<IResolver>(resolver);

            return resolver;
        }
    }
}
