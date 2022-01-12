using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Topelab.Core.Resolver.DTO;
using Topelab.Core.Resolver.Enums;
using Topelab.Core.Resolver.Interfaces;
using System.Linq;

namespace Topelab.Core.Resolver.Microsoft
{
    public static class ResolverFactory
    {
        private const string DefaultKey = "__NULL__";

        public static IResolver Create(ResolveDTOList resolveDTOs)
        {
            if (resolveDTOs is null)
            {
                throw new ArgumentNullException(nameof(resolveDTOs));
            }
            Dictionary<string, IResolver> globalResolvers = new();
            var keys = resolveDTOs.Select(r => r.Key ?? DefaultKey).Distinct().ToList();
            keys.ForEach(key => Create(key, resolveDTOs, globalResolvers));
            return globalResolvers.Where(r => r.Key == DefaultKey).Select(r => r.Value).FirstOrDefault() ?? globalResolvers.First().Value;
        }

        private static IResolver Create(string key, ResolveDTOList resolveDTOs, Dictionary<string, IResolver> globalResolvers)
        {
            IServiceCollection collection = PrepareCollection(resolveDTOs.Where(r => (r.Key ?? DefaultKey) == key));
            var serviceProvider = collection.BuildServiceProvider();
            var serviceFactory = serviceProvider.GetService<IServiceFactory>();
            var resolver = new Resolver(serviceProvider, serviceFactory, key, globalResolvers);
            return resolver;
        }

        private static IServiceCollection PrepareCollection(IEnumerable<ResolveDTO> resolveDTOs)
        {
            IServiceCollection collection = new ServiceCollection();

            collection
                .AddSingleton<IServiceFactory>(provider => new ServiceFactory(provider.GetService, (T, P) => ActivatorUtilities.CreateInstance(provider, T, P)))
                .AddScoped<IService<IResolver>, Service<IResolver,Resolver>>();

            resolveDTOs.ToList().ForEach(resolveDTO =>
            {
                switch (resolveDTO.ResolveType)
                {
                    case ResolveTypeEnum.Singleton:
                        if (resolveDTO.ConstructorParamTypes.Length > 0)
                        {
                            collection.AddSingleton(typeof(IService<>).MakeGenericType(resolveDTO.TypeFrom), typeof(Service<,>).MakeGenericType(resolveDTO.TypeFrom, resolveDTO.TypeTo));
                        }
                        else
                        {
                            collection.AddSingleton(resolveDTO.TypeFrom, resolveDTO.TypeTo);
                        }
                        break;
                    case ResolveTypeEnum.Instance:
                        collection.AddSingleton(resolveDTO.TypeFrom, resolveDTO.Instance);
                        break;
                    default:
                        if (resolveDTO.ConstructorParamTypes.Length > 0)
                        {
                            collection.AddScoped(typeof(IService<>).MakeGenericType(resolveDTO.TypeFrom), typeof(Service<,>).MakeGenericType(resolveDTO.TypeFrom, resolveDTO.TypeTo));
                        }
                        else
                        {
                            collection.AddScoped(resolveDTO.TypeFrom, resolveDTO.TypeTo);
                        }
                        break;
                }
            });

            return collection;
        }
    }

}
