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
    public static class ResolveInfoCollectionFactory
    {
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

                switch (resolveInfo.ResolveMode)
                {
                    case ResolveModeEnum.Instance:
                        container.RegisterInstance(resolveInfo.TypeFrom, resolveInfo.Instance);
                        break;
                    case ResolveModeEnum.Factory:
                        switch (resolveInfo.ResolveLifeCycle)
                        {
                            case ResolveLifeCycleEnum.Transient:
                                container.RegisterFactory(resolveInfo.TypeFrom, key, c => resolveInfo.Factory.Invoke(c.Resolve<IResolver>()), FactoryLifetime.PerResolve);
                                break;
                            case ResolveLifeCycleEnum.Scoped:
                                container.RegisterFactory(resolveInfo.TypeFrom, key, c => resolveInfo.Factory.Invoke(c.Resolve<IResolver>()), FactoryLifetime.PerContainer);
                                break;
                            case ResolveLifeCycleEnum.Singleton:
                                container.RegisterFactory(resolveInfo.TypeFrom, key, c => resolveInfo.Factory.Invoke(c.Resolve<IResolver>()), FactoryLifetime.Singleton);
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        switch (resolveInfo.ResolveLifeCycle)
                        {
                            case ResolveLifeCycleEnum.Singleton:
                                if (resolveInfo.ConstructorParamTypes.Length > 0)
                                {
                                    container.RegisterSingleton(resolveInfo.TypeFrom, resolveInfo.TypeTo, key, injectionMember);
                                }
                                else
                                {
                                    container.RegisterSingleton(resolveInfo.TypeFrom, resolveInfo.TypeTo, key);
                                }
                                break;
                            case ResolveLifeCycleEnum.Scoped:
                                if (resolveInfo.ConstructorParamTypes.Length > 0)
                                {
                                    container.RegisterType(resolveInfo.TypeFrom, resolveInfo.TypeTo, key, TypeLifetime.PerContainer, injectionMember);
                                }
                                else
                                {
                                    container.RegisterType(resolveInfo.TypeFrom, resolveInfo.TypeTo, key, TypeLifetime.PerContainer);
                                }
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
