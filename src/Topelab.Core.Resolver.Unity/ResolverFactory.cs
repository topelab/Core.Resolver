using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Topelab.Core.Resolver.DTO;
using Topelab.Core.Resolver.Enums;
using Topelab.Core.Resolver.Interfaces;
using Unity;
using Unity.Injection;

namespace Topelab.Core.Resolver.Unity
{
    public static class ResolverFactory
    {
        public static IResolver Create(ResolveDTOList moduleDependecies)
        {
            Dictionary<string, ConstructorInfo> constructorsByKey = new();

            var container = new UnityContainer();
            container.RegisterType<IResolver, Resolver>();
            if (moduleDependecies != null)
            {
                Register(container, moduleDependecies, constructorsByKey);
            }

            return new Resolver(container, constructorsByKey);
        }

        public static void Register(IUnityContainer container, ResolveDTOList moduleDependecies, Dictionary<string, ConstructorInfo> constructorsByKey)
        {
            moduleDependecies.ForEach(resolveDTO =>
            {
                string key = ResolverKeyFactory.Create(resolveDTO);
                InjectionMember injectionMember = resolveDTO.ConstructorParamTypes.Length > 0 ? Invoke.Constructor(resolveDTO.ConstructorParamTypes) : null;
                ConstructorInfo constructorInfo = GetConstructorInfo(resolveDTO);

                if (constructorInfo != null && key != null)
                {
                    constructorsByKey[key] = constructorInfo;
                }

                switch (resolveDTO.ResolveType)
                {
                    case ResolveTypeEnum.Singleton:
                        if (resolveDTO.ConstructorParamTypes.Length > 0)
                        {
                            container.RegisterSingleton(resolveDTO.TypeFrom, resolveDTO.TypeTo, key, injectionMember);
                        }
                        else
                        {
                            container.RegisterSingleton(resolveDTO.TypeFrom, resolveDTO.TypeTo, key);
                        }
                        break;
                    case ResolveTypeEnum.Instance:
                        container.RegisterInstance(resolveDTO.TypeFrom, resolveDTO.Instance);
                        break;
                    default:
                        if (resolveDTO.ConstructorParamTypes.Length > 0)
                        {
                            container.RegisterType(resolveDTO.TypeFrom, resolveDTO.TypeTo, key, injectionMember);
                        }
                        else
                        {
                            container.RegisterType(resolveDTO.TypeFrom, resolveDTO.TypeTo, key);
                        }
                        break;
                }
            });
        }

        private static ConstructorInfo GetConstructorInfo(ResolveDTO resolveDTO)
        {
            return resolveDTO.TypeTo.GetConstructors()
                .Where(c => c.GetParameters().Length == resolveDTO.ConstructorParamTypes.Length)
                .Where(c => c.GetParameters().Select(p => p.ParameterType).SequenceEqual(resolveDTO.ConstructorParamTypes))
                .FirstOrDefault();
        }

    }
}
