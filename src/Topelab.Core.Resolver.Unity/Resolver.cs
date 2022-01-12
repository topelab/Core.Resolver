using System.Collections.Generic;
using System;
using Topelab.Core.Resolver.DTO;
using Topelab.Core.Resolver.Interfaces;
using Unity;
using Unity.Resolution;
using System.Reflection;
using System.Linq;

namespace Topelab.Core.Resolver.Unity
{
    public class Resolver : IResolver
    {
        private readonly IUnityContainer container;
        private readonly Dictionary<string, ConstructorInfo> constructorsByKey;

        public Resolver(IUnityContainer container, Dictionary<string, ConstructorInfo> constructorsByKey)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
            this.constructorsByKey = constructorsByKey ?? throw new ArgumentNullException(nameof(constructorsByKey));
        }

        public T Get<T>()
        {
            return container.Resolve<T>();
        }

        public T Get<T, T1>(T1 arg1) => Get<T, T1>(ResolverKeyFactory.Create(typeof(T1)), arg1);

        public T Get<T, T1, T2>(T1 arg1, T2 arg2) => Get<T, T1, T2>(ResolverKeyFactory.Create(typeof(T1), typeof(T2)), arg1, arg2);

        public T Get<T, T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3) => Get<T, T1, T2, T3>(ResolverKeyFactory.Create(typeof(T1), typeof(T2), typeof(T3)), arg1, arg2, arg3);

        public T Get<T, T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4) => Get<T, T1, T2, T3, T4>(ResolverKeyFactory.Create(typeof(T1), typeof(T2), typeof(T3), typeof(T4)), arg1, arg2, arg3, arg4);

        public T Get<T, T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => Get<T, T1, T2, T3, T4, T5>(ResolverKeyFactory.Create(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)), arg1, arg2, arg3, arg4, arg5);

        public T Get<T, T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => Get<T, T1, T2, T3, T4, T5, T6>(ResolverKeyFactory.Create(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)), arg1, arg2, arg3, arg4, arg5, arg6);

        public T Get<T>(string key)
        {
            return container.Resolve<T>(key);
        }

        public T Get<T, T1>(string key, T1 arg1)
        {
            return container.Resolve<T>(key,
                                        new ParameterOverride(typeof(T1), arg1)
                                        );
        }

        public T Get<T, T1, T2>(string key, T1 arg1, T2 arg2)
        {
            var parameters = constructorsByKey[key].GetParameters().Select(p => p.Name).ToArray();
            return container.Resolve<T>(key,
                                        new ParameterOverride(typeof(T1), parameters[0], arg1),
                                        new ParameterOverride(typeof(T2), parameters[1], arg2)
                                        );
        }

        public T Get<T, T1, T2, T3>(string key, T1 arg1, T2 arg2, T3 arg3)
        {
            var parameters = constructorsByKey[key].GetParameters().Select(p => p.Name).ToArray();
            return container.Resolve<T>(key,
                                        new ParameterOverride(typeof(T1), parameters[0], arg1),
                                        new ParameterOverride(typeof(T2), parameters[1], arg2),
                                        new ParameterOverride(typeof(T3), parameters[2], arg3)
                                        );
        }

        public T Get<T, T1, T2, T3, T4>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var parameters = constructorsByKey[key].GetParameters().Select(p => p.Name).ToArray();
            return container.Resolve<T>(key,
                                        new ParameterOverride(typeof(T1), parameters[0], arg1),
                                        new ParameterOverride(typeof(T2), parameters[1], arg2),
                                        new ParameterOverride(typeof(T3), parameters[2], arg3),
                                        new ParameterOverride(typeof(T4), parameters[3], arg4)
                                        );
        }

        public T Get<T, T1, T2, T3, T4, T5>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            var parameters = constructorsByKey[key].GetParameters().Select(p => p.Name).ToArray();
            return container.Resolve<T>(key,
                                        new ParameterOverride(typeof(T1), parameters[0], arg1),
                                        new ParameterOverride(typeof(T2), parameters[1], arg2),
                                        new ParameterOverride(typeof(T3), parameters[2], arg3),
                                        new ParameterOverride(typeof(T4), parameters[3], arg4),
                                        new ParameterOverride(typeof(T5), parameters[4], arg5)
                                        );
        }

        public T Get<T, T1, T2, T3, T4, T5, T6>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            var parameters = constructorsByKey[key].GetParameters().Select(p => p.Name).ToArray();
            return container.Resolve<T>(key,
                                        new ParameterOverride(typeof(T1), parameters[0], arg1),
                                        new ParameterOverride(typeof(T2), parameters[1], arg2),
                                        new ParameterOverride(typeof(T3), parameters[2], arg3),
                                        new ParameterOverride(typeof(T4), parameters[3], arg4),
                                        new ParameterOverride(typeof(T5), parameters[4], arg5),
                                        new ParameterOverride(typeof(T6), parameters[5], arg6)
                                        );
        }

    }
}
