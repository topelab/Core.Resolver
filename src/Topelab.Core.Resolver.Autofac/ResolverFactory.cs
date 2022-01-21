using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Enums;
using Topelab.Core.Resolver.Interfaces;
using Autofac;

namespace Topelab.Core.Resolver.Autofac
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

            ContainerBuilder builder = new();
            if (resolveInfoCollection != null)
            {
                builder.RegisterAutofac(resolveInfoCollection, constructorsByKey);
            }

            IResolver resolver = new Resolver();
            builder.RegisterInstance(resolver);
            var container = builder.Build();
            ((Resolver)resolver).Initialize(container, constructorsByKey);
            return resolver;
        }
    }
}