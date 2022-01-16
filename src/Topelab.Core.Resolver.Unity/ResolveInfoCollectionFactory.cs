using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Enums;
using Unity;

namespace Topelab.Core.Resolver.Unity
{
    public static class ResolveInfoCollectionFactory
    {
        /// <summary>
        /// Adds the factory to resolve info collection.
        /// </summary>
        /// <typeparam name="TOut">The out type.</typeparam>
        /// <param name="resolveInfoCollection">The resolve information collection.</param>
        /// <param name="factory">The factory.</param>
        public static ResolveInfoCollection AddFactory<TOut>(this ResolveInfoCollection resolveInfoCollection, Func<IUnityContainer, TOut> factory)
        {
            return resolveInfoCollection.AddFactory(null, factory);
        }

        /// <summary>
        /// Adds the named factory to resolve info collection.
        /// </summary>
        /// <typeparam name="TOut">The out type.</typeparam>
        /// <param name="resolveInfoCollection">The resolve information collection.</param>
        /// <param name="key">The name for factory resolution</param>
        /// <param name="factory">The factory.</param>
        public static ResolveInfoCollection AddFactory<TOut>(this ResolveInfoCollection resolveInfoCollection, string key, Func<IUnityContainer, TOut> factory)
        {
            resolveInfoCollection.Add(new ResolveInfo(typeof(TOut), typeof(TOut), ResolveTypeEnum.Factory, key) { Instance = factory });
            return resolveInfoCollection;
        }

        /// <summary>
        /// Registers within the specified container the specified resolve info collection.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="resolveInfoCollection">The resolve info collection.</param>
        /// <param name="constructorsByKey">The constructors by key.</param>
        internal static void Register(this IUnityContainer container, ResolveInfoCollection resolveInfoCollection, Dictionary<string, ConstructorInfo> constructorsByKey)
        {
            resolveInfoCollection.ForEach(resolveInfo =>
            {
                var key = ResolverKeyFactory.Create(resolveInfo);
                var injectionMember = resolveInfo.ConstructorParamTypes?.Length > 0 ? Invoke.Constructor(resolveInfo.ConstructorParamTypes) : null;
                var constructorInfo = GetConstructorInfo(resolveInfo);

                if (constructorInfo != null && key != null)
                {
                    constructorsByKey[key] = constructorInfo;
                }

                switch (resolveInfo.ResolveType)
                {
                    case ResolveTypeEnum.Factory:
                        container.RegisterFactory(resolveInfo.TypeTo, key, (Func<IUnityContainer, object>)resolveInfo.Instance);
                        break;
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
