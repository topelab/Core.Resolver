using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Interfaces;
using Unity;
using Unity.Resolution;

namespace Topelab.Core.Resolver.Unity
{
    /// <summary>
    /// Resolver for generic DI using Unity
    /// </summary>
    public class Resolver : IResolver
    {
        private readonly IUnityContainer container;
        private readonly Dictionary<string, ConstructorInfo> constructorsByKey;
        private static readonly List<Resolver> resolvers = new();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Unity container</param>
        /// <param name="constructorsByKey">Constructors dictionary by key</param>
        public Resolver(IUnityContainer container, Dictionary<string, ConstructorInfo> constructorsByKey)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
            this.constructorsByKey = constructorsByKey ?? throw new ArgumentNullException(nameof(constructorsByKey));
            resolvers.Add(this);
        }

        /// <summary>
        /// Resolve an instance of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        public T Get<T>()
        {
            if (IsRegistered(typeof(T), container))
            {
                return container.Resolve<T>();
            }

            var resolver = resolvers.Reverse<Resolver>().Where(r => !r.Equals(this) && IsRegistered(typeof(T), r.container)).FirstOrDefault();
            return resolver == null ? default : resolver.container.Resolve<T>();
        }

        private bool IsRegistered(Type type, IUnityContainer container, string key = null)
        {
            bool result;

            result = container.IsRegistered(type, key);

            if (!result)
            {
                if (type.IsGenericType)
                {
                    var genericType = type.GetGenericTypeDefinition();
                    result = container.Registrations.Where(r => r.RegisteredType.IsGenericTypeDefinition && r.RegisteredType == genericType).Any();
                }
            }

            return result;
        }

        /// <summary>
        /// Resolve an instance of type <typeparamref name="T"/> using constructor with param type <typeparamref name="T1"/>
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <typeparam name="T1">Type for constructor param 1</typeparam>
        /// <param name="arg1">Param 1 for constructor</param>
        public T Get<T, T1>(T1 arg1)
            => Get<T, T1>(ResolverKeyFactory.Create(typeof(T1)), arg1);

        /// <summary>
        /// Resolve an instance of type <typeparamref name="T"/> using constructor with param types <typeparamref name="T1"/> and 
        /// <typeparamref name="T2"/>
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <typeparam name="T1">Type for constructor param 1</typeparam>
        /// <typeparam name="T2">Type for constructor param 2</typeparam>
        /// <param name="arg1">Param 1 for constructor</param>
        /// <param name="arg2">Param 2 for constructor</param>
        public T Get<T, T1, T2>(T1 arg1, T2 arg2)
            => Get<T, T1, T2>(ResolverKeyFactory.Create(typeof(T1), typeof(T2)), arg1, arg2);

        /// <summary>
        /// Resolve an instance of type <typeparamref name="T"/> using constructor with param types <typeparamref name="T1"/>, 
        /// <typeparamref name="T2"/> and <typeparamref name="T3"/>
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <typeparam name="T1">Type for constructor param 1</typeparam>
        /// <typeparam name="T2">Type for constructor param 2</typeparam>
        /// <typeparam name="T3">Type for constructor param 3</typeparam>
        /// <param name="arg1">Param 1 for constructor</param>
        /// <param name="arg2">Param 2 for constructor</param>
        /// <param name="arg3">Param 3 for constructor</param>
        public T Get<T, T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
            => Get<T, T1, T2, T3>(ResolverKeyFactory.Create(typeof(T1), typeof(T2), typeof(T3)), arg1, arg2, arg3);

        /// <summary>
        /// Resolve an instance of type <typeparamref name="T"/> using constructor with param types <typeparamref name="T1"/>, 
        /// <typeparamref name="T2"/>, <typeparamref name="T3"/> and <typeparamref name="T4"/>
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <typeparam name="T1">Type for constructor param 1</typeparam>
        /// <typeparam name="T2">Type for constructor param 2</typeparam>
        /// <typeparam name="T3">Type for constructor param 3</typeparam>
        /// <typeparam name="T4">Type for constructor param 4</typeparam>
        /// <param name="arg1">Param 1 for constructor</param>
        /// <param name="arg2">Param 2 for constructor</param>
        /// <param name="arg3">Param 3 for constructor</param>
        /// <param name="arg4">Param 4 for constructor</param>
        public T Get<T, T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => Get<T, T1, T2, T3, T4>(ResolverKeyFactory.Create(typeof(T1), typeof(T2), typeof(T3), typeof(T4)), arg1, arg2, arg3, arg4);

        /// <summary>
        /// Resolve an instance of type <typeparamref name="T"/> using constructor with param types <typeparamref name="T1"/>, 
        /// <typeparamref name="T2"/>, <typeparamref name="T3"/>, <typeparamref name="T4"/> and <typeparamref name="T5"/>
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <typeparam name="T1">Type for constructor param 1</typeparam>
        /// <typeparam name="T2">Type for constructor param 2</typeparam>
        /// <typeparam name="T3">Type for constructor param 3</typeparam>
        /// <typeparam name="T4">Type for constructor param 4</typeparam>
        /// <typeparam name="T5">Type for constructor param 5</typeparam>
        /// <param name="arg1">Param 1 for constructor</param>
        /// <param name="arg2">Param 2 for constructor</param>
        /// <param name="arg3">Param 3 for constructor</param>
        /// <param name="arg4">Param 4 for constructor</param>
        /// <param name="arg5">Param 5 for constructor</param>
        public T Get<T, T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => Get<T, T1, T2, T3, T4, T5>(ResolverKeyFactory.Create(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)), arg1, arg2, arg3, arg4, arg5);

        /// <summary>
        /// Resolve an instance of type <typeparamref name="T"/> using constructor with param types <typeparamref name="T1"/>, 
        /// <typeparamref name="T2"/>, <typeparamref name="T3"/>, <typeparamref name="T4"/>, <typeparamref name="T5"/> and <typeparamref name="T6"/>
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <typeparam name="T1">Type for constructor param 1</typeparam>
        /// <typeparam name="T2">Type for constructor param 2</typeparam>
        /// <typeparam name="T3">Type for constructor param 3</typeparam>
        /// <typeparam name="T4">Type for constructor param 4</typeparam>
        /// <typeparam name="T5">Type for constructor param 5</typeparam>
        /// <typeparam name="T6">Type for constructor param 6</typeparam>
        /// <param name="arg1">Param 1 for constructor</param>
        /// <param name="arg2">Param 2 for constructor</param>
        /// <param name="arg3">Param 3 for constructor</param>
        /// <param name="arg4">Param 4 for constructor</param>
        /// <param name="arg5">Param 5 for constructor</param>
        /// <param name="arg6">Param 6 for constructor</param>
        public T Get<T, T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => Get<T, T1, T2, T3, T4, T5, T6>(ResolverKeyFactory.Create(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)), arg1, arg2, arg3, arg4, arg5, arg6);

        /// <summary>
        /// Resolve a named instance of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <param name="key">Key name to resolve</param>
        public T Get<T>(string key)
        {
            var container = FindContainerWithKey(typeof(T), key);
            return container.Resolve<T>(key);
        }

        /// <summary>
        /// Resolve a named instance of type <typeparamref name="T"/> using constructor with param type <typeparamref name="T1"/>
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <typeparam name="T1">Type for constructor param 1</typeparam>
        /// <param name="key">Key name to resolve</param>
        /// <param name="arg1">Param 1 for constructor</param>
        public T Get<T, T1>(string key, T1 arg1)
        {
            Resolver resolver;
            string[] parameters = new string[0];

            var container = FindContainerWithKey(typeof(T), key);
            if (container == null)
            {
                var resolverDTO = GetParametersWithThisSubKey(key);
                resolver = resolverDTO.Resolver;
                container = resolver.container;
                key = resolverDTO.Key;
                parameters = resolverDTO.Parameters;
            }
            else
            {
                parameters = constructorsByKey[key].GetParameters().Select(p => p.Name).ToArray();
            }

            return container.Resolve<T>(key,
                                        new ParameterOverride(typeof(T1), arg1)
                                        );
        }

        /// <summary>
        /// Resolve a named instance of type <typeparamref name="T"/> using constructor with param types <typeparamref name="T1"/> and 
        /// <typeparamref name="T2"/>
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <typeparam name="T1">Type for constructor param 1</typeparam>
        /// <typeparam name="T2">Type for constructor param 2</typeparam>
        /// <param name="key">Key name to resolve</param>
        /// <param name="arg1">Param 1 for constructor</param>
        /// <param name="arg2">Param 2 for constructor</param>
        public T Get<T, T1, T2>(string key, T1 arg1, T2 arg2)
        {
            var container = FindContainerWithKey(typeof(T), key);
            var parameters = constructorsByKey[key].GetParameters().Select(p => p.Name).ToArray();
            return container.Resolve<T>(key,
                                        new ParameterOverride(typeof(T1), parameters[0], arg1),
                                        new ParameterOverride(typeof(T2), parameters[1], arg2)
                                        );
        }

        /// <summary>
        /// Resolve a named instance of type <typeparamref name="T"/> using constructor with param types <typeparamref name="T1"/>, 
        /// <typeparamref name="T2"/> and <typeparamref name="T3"/>
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <typeparam name="T1">Type for constructor param 1</typeparam>
        /// <typeparam name="T2">Type for constructor param 2</typeparam>
        /// <typeparam name="T3">Type for constructor param 3</typeparam>
        /// <param name="key">Key name to resolve</param>
        /// <param name="arg1">Param 1 for constructor</param>
        /// <param name="arg2">Param 2 for constructor</param>
        /// <param name="arg3">Param 3 for constructor</param>
        public T Get<T, T1, T2, T3>(string key, T1 arg1, T2 arg2, T3 arg3)
        {
            var container = FindContainerWithKey(typeof(T), key);
            var parameters = constructorsByKey[key].GetParameters().Select(p => p.Name).ToArray();
            return container.Resolve<T>(key,
                                        new ParameterOverride(typeof(T1), parameters[0], arg1),
                                        new ParameterOverride(typeof(T2), parameters[1], arg2),
                                        new ParameterOverride(typeof(T3), parameters[2], arg3)
                                        );
        }

        /// <summary>
        /// Resolve a named instance of type <typeparamref name="T"/> using constructor with param types <typeparamref name="T1"/>, 
        /// <typeparamref name="T2"/>, <typeparamref name="T3"/> and <typeparamref name="T4"/>
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <typeparam name="T1">Type for constructor param 1</typeparam>
        /// <typeparam name="T2">Type for constructor param 2</typeparam>
        /// <typeparam name="T3">Type for constructor param 3</typeparam>
        /// <typeparam name="T4">Type for constructor param 4</typeparam>
        /// <param name="key">Key name to resolve</param>
        /// <param name="arg1">Param 1 for constructor</param>
        /// <param name="arg2">Param 2 for constructor</param>
        /// <param name="arg3">Param 3 for constructor</param>
        /// <param name="arg4">Param 4 for constructor</param>
        public T Get<T, T1, T2, T3, T4>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var container = FindContainerWithKey(typeof(T), key);
            var parameters = constructorsByKey[key].GetParameters().Select(p => p.Name).ToArray();
            return container.Resolve<T>(key,
                                        new ParameterOverride(typeof(T1), parameters[0], arg1),
                                        new ParameterOverride(typeof(T2), parameters[1], arg2),
                                        new ParameterOverride(typeof(T3), parameters[2], arg3),
                                        new ParameterOverride(typeof(T4), parameters[3], arg4)
                                        );
        }

        /// <summary>
        /// Resolve a named instance of type <typeparamref name="T"/> using constructor with param types <typeparamref name="T1"/>, 
        /// <typeparamref name="T2"/>, <typeparamref name="T3"/>, <typeparamref name="T4"/> and <typeparamref name="T5"/>
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <typeparam name="T1">Type for constructor param 1</typeparam>
        /// <typeparam name="T2">Type for constructor param 2</typeparam>
        /// <typeparam name="T3">Type for constructor param 3</typeparam>
        /// <typeparam name="T4">Type for constructor param 4</typeparam>
        /// <typeparam name="T5">Type for constructor param 5</typeparam>
        /// <param name="key">Key name to resolve</param>
        /// <param name="arg1">Param 1 for constructor</param>
        /// <param name="arg2">Param 2 for constructor</param>
        /// <param name="arg3">Param 3 for constructor</param>
        /// <param name="arg4">Param 4 for constructor</param>
        /// <param name="arg5">Param 5 for constructor</param>
        public T Get<T, T1, T2, T3, T4, T5>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            var container = FindContainerWithKey(typeof(T), key);
            var parameters = constructorsByKey[key].GetParameters().Select(p => p.Name).ToArray();
            return container.Resolve<T>(key,
                                        new ParameterOverride(typeof(T1), parameters[0], arg1),
                                        new ParameterOverride(typeof(T2), parameters[1], arg2),
                                        new ParameterOverride(typeof(T3), parameters[2], arg3),
                                        new ParameterOverride(typeof(T4), parameters[3], arg4),
                                        new ParameterOverride(typeof(T5), parameters[4], arg5)
                                        );
        }

        /// <summary>
        /// Resolve a named instance of type <typeparamref name="T"/> using constructor with param types <typeparamref name="T1"/>, 
        /// <typeparamref name="T2"/>, <typeparamref name="T3"/>, <typeparamref name="T4"/>, <typeparamref name="T5"/> and <typeparamref name="T6"/>
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <typeparam name="T1">Type for constructor param 1</typeparam>
        /// <typeparam name="T2">Type for constructor param 2</typeparam>
        /// <typeparam name="T3">Type for constructor param 3</typeparam>
        /// <typeparam name="T4">Type for constructor param 4</typeparam>
        /// <typeparam name="T5">Type for constructor param 5</typeparam>
        /// <typeparam name="T6">Type for constructor param 6</typeparam>
        /// <param name="key">Key name to resolve</param>
        /// <param name="arg1">Param 1 for constructor</param>
        /// <param name="arg2">Param 2 for constructor</param>
        /// <param name="arg3">Param 3 for constructor</param>
        /// <param name="arg4">Param 4 for constructor</param>
        /// <param name="arg5">Param 5 for constructor</param>
        /// <param name="arg6">Param 6 for constructor</param>
        public T Get<T, T1, T2, T3, T4, T5, T6>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            Resolver resolver;
            string[] parameters = new string[0];

            var container = FindContainerWithKey(typeof(T), key);
            if (container == null)
            {
                var resolverDTO = GetParametersWithThisSubKey(key);
                resolver = resolverDTO.Resolver;
                container = resolver.container;
                key = resolverDTO.Key;
                parameters = resolverDTO.Parameters;
            }
            else
            {
                parameters = constructorsByKey[key].GetParameters().Select(p => p.Name).ToArray();
            }

            return container.Resolve<T>(key,
                                        new ParameterOverride(typeof(T1), parameters[0], arg1),
                                        new ParameterOverride(typeof(T2), parameters[1], arg2),
                                        new ParameterOverride(typeof(T3), parameters[2], arg3),
                                        new ParameterOverride(typeof(T4), parameters[3], arg4),
                                        new ParameterOverride(typeof(T5), parameters[4], arg5),
                                        new ParameterOverride(typeof(T6), parameters[5], arg6)
                                        );
        }

        private ResolverDTO GetParametersWithThisSubKey(string key)
        {
            string result = null;
            Resolver resolver = null;
            foreach (var r in resolvers)
            {
                var keys = r.constructorsByKey.Keys.Where(k => k.Split('|').Contains(key));
                if (keys.Any())
                {
                    result = keys.OrderBy(k => k.Split('|').Length).First();
                    resolver = r;
                    break;
                }
            }
            var originalKeys = key.Split('|');
            var parameters = resolver.constructorsByKey[result].GetParameters().Where(p => originalKeys.Contains(p.ParameterType.Name)).Select(p => p.Name).ToArray();
            return new ResolverDTO(resolver, parameters, result);
        }

        private IUnityContainer FindContainerWithKey(Type type, string key)
        {
            var result = container.IsRegistered(type, key)
                ? container
                : resolvers.Reverse<Resolver>().Where(r => !r.Equals(this) && IsRegistered(type, r.container, key)).Select(r => r.container).FirstOrDefault();

            return result;
        }
    }
}
