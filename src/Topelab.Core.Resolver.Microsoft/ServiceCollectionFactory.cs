using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Enums;
using Topelab.Core.Resolver.Interfaces;

namespace Topelab.Core.Resolver.Microsoft
{
    internal static class ServiceCollectionFactory
    {
        public static IServiceCollection CreateServiceCollection(this IEnumerable<ResolveInfo> resolveInfoCollection)
        {
            IServiceCollection collection = new ServiceCollection();

            collection
                .AddSingleton<IServiceFactory>(provider => new ServiceFactory(provider.GetService, (T, P) => ActivatorUtilities.CreateInstance(provider, T, P)))
                .AddScoped<IService<IResolver>, Service<IResolver, Resolver>>();

            resolveInfoCollection.ToList().ForEach(resolveInfo =>
            {
                switch (resolveInfo.ResolveType)
                {
                    case ResolveTypeEnum.Factory:
                        collection.AddSingleton(resolveInfo.TypeTo, (Func<IServiceProvider, object>)resolveInfo.Instance);
                        break;
                    case ResolveTypeEnum.Singleton:
                        if (resolveInfo.ConstructorParamTypes.Length > 0)
                        {
                            collection.AddSingleton(typeof(IService<>).MakeGenericType(resolveInfo.TypeFrom), typeof(Service<,>).MakeGenericType(resolveInfo.TypeFrom, resolveInfo.TypeTo));
                        }
                        else
                        {
                            collection.AddSingleton(resolveInfo.TypeFrom, resolveInfo.TypeTo);
                        }
                        break;
                    case ResolveTypeEnum.Instance:
                        collection.AddSingleton(resolveInfo.TypeFrom, resolveInfo.Instance);
                        break;
                    default:
                        if (resolveInfo.ConstructorParamTypes.Length > 0)
                        {
                            collection.AddScoped(typeof(IService<>).MakeGenericType(resolveInfo.TypeFrom), typeof(Service<,>).MakeGenericType(resolveInfo.TypeFrom, resolveInfo.TypeTo));
                        }
                        else
                        {
                            collection.AddScoped(resolveInfo.TypeFrom, resolveInfo.TypeTo);
                        }
                        break;
                }
            });

            return collection;
        }

    }
}
