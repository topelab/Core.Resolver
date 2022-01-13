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
            container.RegisterType<IResolver, Resolver>();
            if (resolveInfoCollection != null)
            {
                Register(container, resolveInfoCollection, constructorsByKey);
            }

            return new Resolver(container, constructorsByKey);
        }

        /// <summary>
        /// Registers within the specified container the specified resolve info collection.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="resolveInfoCollection">The resolve info collection.</param>
        /// <param name="constructorsByKey">The constructors by key.</param>
        public static void Register(IUnityContainer container, ResolveInfoCollection resolveInfoCollection, Dictionary<string, ConstructorInfo> constructorsByKey)
        {
            resolveInfoCollection.ForEach(resolveInfo =>
            {
                var key = ResolverKeyFactory.Create(resolveInfo);
                var injectionMember = resolveInfo.ConstructorParamTypes.Length > 0 ? Invoke.Constructor(resolveInfo.ConstructorParamTypes) : null;
                var constructorInfo = GetConstructorInfo(resolveInfo);

                if (constructorInfo != null && key != null)
                {
                    constructorsByKey[key] = constructorInfo;
                }

                switch (resolveInfo.ResolveType)
                {
                    case ResolveTypeEnum.Singleton:
                        if (resolveInfo.ConstructorParamTypes.Length > 0)
                        {
                            container.RegisterSingleton(resolveInfo.TypeFrom, resolveInfo.TypeTo, key, injectionMember);
                        }
                        else
                        {
                            container.RegisterSingleton(resolveInfo.TypeFrom, resolveInfo.TypeTo, key);
                        }
                        break;
                    case ResolveTypeEnum.Instance:
                        container.RegisterInstance(resolveInfo.TypeFrom, resolveInfo.Instance);
                        break;
                    default:
                        if (resolveInfo.ConstructorParamTypes.Length > 0)
                        {
                            container.RegisterType(resolveInfo.TypeFrom, resolveInfo.TypeTo, key, injectionMember);
                        }
                        else
                        {
                            container.RegisterType(resolveInfo.TypeFrom, resolveInfo.TypeTo, key);
                        }
                        break;
                }
            });
        }

        private static ConstructorInfo GetConstructorInfo(ResolveInfo resolveInfo)
        {
            return resolveInfo.TypeTo.GetConstructors()
                .Where(c => c.GetParameters().Length == resolveInfo.ConstructorParamTypes.Length)
                .Where(c => c.GetParameters().Select(p => p.ParameterType).SequenceEqual(resolveInfo.ConstructorParamTypes))
                .FirstOrDefault();
        }

    }
}
