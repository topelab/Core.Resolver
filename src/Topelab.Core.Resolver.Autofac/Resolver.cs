using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Interfaces;

namespace Topelab.Core.Resolver.Autofac
{
    /// <summary>
    /// Resolver for generic DI using Unity
    /// </summary>
    public class Resolver : IResolver
    {
        private IContainer container;
        private Dictionary<string, ConstructorInfo> constructorsByKey;
        private static readonly List<Resolver> resolvers = new();

        public void Initialize(IContainer container, Dictionary<string, ConstructorInfo> constructorsByKey)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
            this.constructorsByKey = constructorsByKey ?? throw new ArgumentNullException(nameof(constructorsByKey));
            resolvers.Add(this);
        }

        /// <summary>
        /// Resolve an instance of type <paramref name="type"/>
        /// </summary>
        /// <param name="type">Type of instance to resolve</param>
        public object Get(Type type)
        {
            if (container.IsRegistered(type))
            {
                return Resolve(container, type);
            }
            var resolver = resolvers.Reverse<Resolver>().Where(r => !r.Equals(this) && r.container.IsRegistered(type)).FirstOrDefault();
            return resolver == null ? default : Resolve(resolver.container, type);
        }

        /// <summary>
        /// Resolve an instance of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        public T Get<T>()
        {
            if (container.IsRegistered<T>())
            {
                return Resolve<T>(container);
            }
            var resolver = resolvers.Reverse<Resolver>().Where(r => !r.Equals(this) && r.container.IsRegistered(typeof(T))).FirstOrDefault();
            return resolver == null ? default : Resolve<T>(resolver.container);
        }

        private T Resolve<T>(IContainer container)
        {
            using var scope = container.BeginLifetimeScope();
            return scope.Resolve<T>();
        }

        private object Resolve(IContainer container, Type type)
        {
            using var scope = container.BeginLifetimeScope();
            return scope.Resolve(type);
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
        public T Get<T>(string key) => (T)Get(typeof(T), key);

        /// <summary>
        /// Resolve a named instance of type <paramref name="typeFrom"/>
        /// </summary>
        /// <param name="typeFrom">Type to resolve</param>
        /// <param name="key">Key name to resolve</param>
        public object Get(Type typeFrom, string key)
        {
            using var scope = FindContainerWithKey(typeFrom, key).BeginLifetimeScope();
            return scope.ResolveNamed(key, typeFrom);
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
            (var resolver, _, key) = GetParametersWithThisSubKey(typeof(T), key);
            using var scope = resolver.container.BeginLifetimeScope();
            return scope.ResolveNamed<T>(key,
                                        new TypedParameter(typeof(T1), arg1)
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
            (var resolver, var parameters, key) = GetParametersWithThisSubKey(typeof(T), key);
            using var scope = resolver.container.BeginLifetimeScope();
            return scope.ResolveNamed<T>(key,
                                        new NamedParameter(parameters[0], arg1),
                                        new NamedParameter(parameters[1], arg2)
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
            (var resolver, var parameters, key) = GetParametersWithThisSubKey(typeof(T), key);
            using var scope = resolver.container.BeginLifetimeScope();
            return scope.ResolveNamed<T>(key,
                                        new NamedParameter(parameters[0], arg1),
                                        new NamedParameter(parameters[1], arg2),
                                        new NamedParameter(parameters[2], arg3)
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
            (var resolver, var parameters, key) = GetParametersWithThisSubKey(typeof(T), key);
            using var scope = resolver.container.BeginLifetimeScope();
            return scope.ResolveNamed<T>(key,
                                        new NamedParameter(parameters[0], arg1),
                                        new NamedParameter(parameters[1], arg2),
                                        new NamedParameter(parameters[2], arg3),
                                        new NamedParameter(parameters[3], arg4)
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
            (var resolver, var parameters, key) = GetParametersWithThisSubKey(typeof(T), key);
            using var scope = resolver.container.BeginLifetimeScope();
            return scope.ResolveNamed<T>(key,
                                        new NamedParameter(parameters[0], arg1),
                                        new NamedParameter(parameters[1], arg2),
                                        new NamedParameter(parameters[2], arg3),
                                        new NamedParameter(parameters[3], arg4),
                                        new NamedParameter(parameters[4], arg5)
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
            (var resolver, var parameters, key) = GetParametersWithThisSubKey(typeof(T), key);
            using var scope = resolver.container.BeginLifetimeScope();
            return scope.ResolveNamed<T>(key,
                                        new NamedParameter(parameters[0], arg1),
                                        new NamedParameter(parameters[1], arg2),
                                        new NamedParameter(parameters[2], arg3),
                                        new NamedParameter(parameters[3], arg4),
                                        new NamedParameter(parameters[4], arg5),
                                        new NamedParameter(parameters[5], arg6)
                                        );
        }

        /// <summary>
        /// Resolve an instance of type <typeparamref name="T"/> using constructor params array <paramref name="args"/>
        /// </summary>
        /// <typeparam name="T">Type of instance to get</typeparam>
        /// <param name="args">Params used to construct instance</param>
        public T Get<T>(params object[] args)
        {
            var types = args.Select(a => a.GetType()).ToArray();
            var key = ResolverKeyFactory.Create(types);
            return Get<T>(key, args);
        }

        /// <summary>
        /// Resolve a named instance of type <typeparamref name="T"/> using constructor params array <paramref name="args"/>
        /// </summary>
        /// <param name="key">Key name to resolve</param>
        /// <typeparam name="T">Type of instance to get</typeparam>
        /// <param name="args">Params used to construct instance</param>
        public T Get<T>(string key, params object[] args)
        {
            (var resolver, var parameters, key) = GetParametersWithThisSubKey(typeof(T), key);
            using var scope = resolver.container.BeginLifetimeScope();
            List<NamedParameter> namedParameters = new List<NamedParameter>();
            for (int i = 0; i < args.Length; i++)
            {
                namedParameters.Add(new NamedParameter(parameters[i], args[i]));
            }
            return scope.ResolveNamed<T>(key, namedParameters);
        }

        private (Resolver Resolver, string[] Parameters, string Key) GetParametersWithThisSubKey(Type type, string key)
        {
            var resultKey = key;
            var resultResolver = this;
            var resultParameters = Array.Empty<string>();

            var container = FindContainerWithKey(type, key);

            if (container == null)
            {
                foreach (var resolver in resolvers.Reverse<Resolver>())
                {
                    var keys = resolver.constructorsByKey.Keys.Where(k => $"|{k}|".Contains(key));
                    if (keys.Any())
                    {
                        resultKey = keys.OrderBy(k => k.Split('|').Length).First();
                        resultResolver = resolver;
                        break;
                    }
                }
                var originalKeys = $"|{key}|";
                resultParameters = resultResolver.constructorsByKey[resultKey].GetParameters().Where(p => originalKeys.Contains(p.ParameterType.Name)).Select(p => p.Name).ToArray();
            }
            else
            {
                resultParameters = constructorsByKey[key].GetParameters().Select(p => p.Name).ToArray();
            }

            return (resultResolver, resultParameters, resultKey);
        }


        private IContainer FindContainerWithKey(Type type, string key)
        {
            var result = container.IsRegisteredWithName(key, type)
                ? container
                : resolvers.Reverse<Resolver>().Where(r => !r.Equals(this) && r.container.IsRegisteredWithName(key, type)).Select(r => r.container).FirstOrDefault();

            return result;
        }
    }
}
