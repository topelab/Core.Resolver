using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Enums;
using Topelab.Core.Resolver.Interfaces;

namespace Topelab.Core.Resolver.Microsoft
{
    /// <summary>
    /// Internal factory class to create or populate an IServiceCollection
    /// </summary>
    internal static class ServiceCollectionFactory
    {
        /// <summary>
        /// Create or populate an IServiceCollection from a resolve info collection
        /// </summary>
        /// <param name="resolveInfoCollection">Resolve info collection</param>
        /// <param name="currentServices">Optional current service collection</param>
        public static IServiceCollection Create(IEnumerable<ResolveInfo> resolveInfoCollection, IServiceCollection currentServices = null)
        {
            var collection = currentServices ?? new ServiceCollection();
            collection
                .AddSingleton<IServiceFactory>(provider => new ServiceFactory(provider.GetService, (T, P) => ActivatorUtilities.CreateInstance(provider, T, P)))
                .AddScoped<IService<IResolver>, Service<IResolver, Resolver>>();

            foreach (var resolveInfo in resolveInfoCollection)
            {
                switch (resolveInfo.ResolveMode)
                {
                    case ResolveModeEnum.Instance:
                        collection.AddSingleton(resolveInfo.TypeFrom, resolveInfo.Instance);
                        break;
                    case ResolveModeEnum.Factory:
                        collection.AddSingleton(resolveInfo.TypeFrom, s => resolveInfo.Factory.Invoke(s.GetService<IResolver>()));
                        break;
                    default:
                        switch (resolveInfo.ResolveLifeCycle)
                        {
                            case ResolveLifeCycleEnum.Singleton:
                                if (resolveInfo.ConstructorParamTypes.Length > 0)
                                {
                                    collection.AddSingleton(typeof(IService<>).MakeGenericType(resolveInfo.TypeFrom), typeof(Service<,>).MakeGenericType(resolveInfo.TypeFrom, resolveInfo.TypeTo));
                                }
                                else
                                {
                                    collection.AddSingleton(resolveInfo.TypeFrom, resolveInfo.TypeTo);
                                }
                                break;
                            case ResolveLifeCycleEnum.Scoped:
                                if (resolveInfo.ConstructorParamTypes.Length > 0)
                                {
                                    collection.AddScoped(typeof(IService<>).MakeGenericType(resolveInfo.TypeFrom), typeof(Service<,>).MakeGenericType(resolveInfo.TypeFrom, resolveInfo.TypeTo));
                                }
                                else
                                {
                                    collection.AddScoped(resolveInfo.TypeFrom, resolveInfo.TypeTo);
                                }
                                break;
                            default:
                                if (resolveInfo.ConstructorParamTypes.Length > 0)
                                {
                                    collection.AddTransient(typeof(IService<>).MakeGenericType(resolveInfo.TypeFrom), typeof(Service<,>).MakeGenericType(resolveInfo.TypeFrom, resolveInfo.TypeTo));
                                }
                                else
                                {
                                    collection.AddTransient(resolveInfo.TypeFrom, resolveInfo.TypeTo);
                                }
                                break;
                        }
                        break;
                }

            }

            return collection;
        }

    }
}
