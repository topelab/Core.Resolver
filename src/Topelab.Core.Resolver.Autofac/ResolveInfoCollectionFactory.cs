using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Enums;
using Autofac;
using Autofac.Builder;
using Topelab.Core.Resolver.Interfaces;

namespace Topelab.Core.Resolver.Autofac
{
    public static class ResolveInfoCollectionFactory
    {
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
                        builder.Register(c => resolveInfo.Factory.Invoke(c.Resolve<IResolver>())).WithName(resolveInfo.Key, resolveInfo.TypeFrom);
                        break;
                    default:
                        switch (resolveInfo.ResolveLifeCycle)
                        {
                            case ResolveLifeCycleEnum.Singleton:
                                if (resolveInfo.ConstructorParamTypes.Length > 0)
                                {
                                    builder.RegisterType(resolveInfo.TypeTo).WithName(resolveInfo.Key, resolveInfo.TypeFrom).UsingConstructor(resolveInfo.ConstructorParamTypes).SingleInstance();
                                }
                                else
                                {
                                    builder.RegisterType(resolveInfo.TypeTo).WithName(resolveInfo.Key, resolveInfo.TypeFrom).SingleInstance();
                                }
                                break;
                            case ResolveLifeCycleEnum.Scoped:
                                if (resolveInfo.ConstructorParamTypes.Length > 0)
                                {
                                    builder.RegisterType(resolveInfo.TypeTo).WithName(resolveInfo.Key, resolveInfo.TypeFrom).UsingConstructor(resolveInfo.ConstructorParamTypes).InstancePerLifetimeScope();
                                }
                                else
                                {
                                    builder.RegisterType(resolveInfo.TypeTo).WithName(resolveInfo.Key, resolveInfo.TypeFrom).InstancePerLifetimeScope();
                                }
                                break;
                            default:
                                if (resolveInfo.ConstructorParamTypes.Length > 0)
                                {
                                    builder.RegisterType(resolveInfo.TypeTo).WithName(resolveInfo.Key, resolveInfo.TypeFrom).UsingConstructor(resolveInfo.ConstructorParamTypes).InstancePerDependency();
                                }
                                else
                                {
                                    builder.RegisterType(resolveInfo.TypeTo).WithName(resolveInfo.Key, resolveInfo.TypeFrom).InstancePerDependency();
                                }
                                break;
                        }
                        break;
                }

            });
        }

        private static IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> WithName(this IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> builder, string key, Type type)
        {
            return key == null ? builder.As(type) : builder.Named(key, type);
        }

        private static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> WithName(this IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> builder, string key, Type type)
        {
            return key == null ? builder.As(type) : builder.Named(key, type);
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
