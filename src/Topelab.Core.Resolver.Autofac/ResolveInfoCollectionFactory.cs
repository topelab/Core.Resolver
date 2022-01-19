using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Enums;
using Autofac;

namespace Topelab.Core.Resolver.Autofac
{
    public static class ResolveInfoCollectionFactory
    {
        /// <summary>
        /// Adds the factory to resolve info collection.
        /// </summary>
        /// <typeparam name="TOut">The out type.</typeparam>
        /// <param name="resolveInfoCollection">The resolve information collection.</param>
        /// <param name="factory">The factory.</param>
        public static ResolveInfoCollection AddFactory<TOut>(this ResolveInfoCollection resolveInfoCollection, Func<IComponentContext, TOut> factory)
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
        public static ResolveInfoCollection AddFactory<TOut>(this ResolveInfoCollection resolveInfoCollection, string key, Func<IComponentContext, TOut> factory)
        {
            resolveInfoCollection.Add(new ResolveInfo(typeof(TOut), typeof(TOut), ResolveModeEnum.Factory, ResolveLifeCycleEnum.Singleton, key) { Instance = factory });
            return resolveInfoCollection;
        }

        /// <summary>
        /// Registers within the specified container the specified resolve info collection.
        /// </summary>
        /// <param name="builder">The container builder.</param>
        /// <param name="resolveInfoCollection">The resolve info collection.</param>
        /// <param name="constructorsByKey">The constructors by key.</param>
        internal static void RegisterAutofac(this ContainerBuilder builder, ResolveInfoCollection resolveInfoCollection, Dictionary<string, ConstructorInfo> constructorsByKey)
        {
            resolveInfoCollection.ForEach(resolveInfo =>
            {
                var key = ResolverKeyFactory.Create(resolveInfo);
                var constructorInfo = GetConstructorInfo(resolveInfo);

                if (constructorInfo != null && key != null)
                {
                    constructorsByKey[key] = constructorInfo;
                }

                switch (resolveInfo.ResolveMode)
                {
                    case ResolveModeEnum.Instance:
                        builder.RegisterInstance(resolveInfo.Instance).As(resolveInfo.TypeFrom);
                        break;
                    case ResolveModeEnum.Factory:
                        throw new NotSupportedException();
                    default:
                        switch (resolveInfo.ResolveLifeCycle)
                        {
                            case ResolveLifeCycleEnum.Singleton:
                                if (resolveInfo.ConstructorParamTypes.Length > 0)
                                {
                                    builder.RegisterType(resolveInfo.TypeTo).As(resolveInfo.TypeFrom).UsingConstructor(resolveInfo.ConstructorParamTypes).SingleInstance();
                                }
                                else
                                {
                                    builder.RegisterType(resolveInfo.TypeTo).As(resolveInfo.TypeFrom).SingleInstance();
                                }
                                break;
                            case ResolveLifeCycleEnum.Scoped:
                                if (resolveInfo.ConstructorParamTypes.Length > 0)
                                {
                                    builder.RegisterType(resolveInfo.TypeTo).As(resolveInfo.TypeFrom).UsingConstructor(resolveInfo.ConstructorParamTypes).InstancePerLifetimeScope();
                                }
                                else
                                {
                                    builder.RegisterType(resolveInfo.TypeTo).As(resolveInfo.TypeFrom).InstancePerLifetimeScope();
                                }
                                break;
                            default:
                                if (resolveInfo.ConstructorParamTypes.Length > 0)
                                {
                                    builder.RegisterType(resolveInfo.TypeTo).As(resolveInfo.TypeFrom).UsingConstructor(resolveInfo.ConstructorParamTypes).InstancePerDependency();
                                }
                                else
                                {
                                    builder.RegisterType(resolveInfo.TypeTo).As(resolveInfo.TypeFrom).InstancePerDependency();
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
