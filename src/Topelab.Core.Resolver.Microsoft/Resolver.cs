using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Interfaces;

namespace Topelab.Core.Resolver.Microsoft
{
    /// <summary>
    /// Resolver for generic DI using Microsoft.Extensions.DependencyInjection
    /// </summary>
    public class Resolver : IResolver
    {

        private readonly ResolverKey resolverKey;
        private readonly IServiceProvider serviceProvider;
        private readonly Dictionary<Type, Dictionary<string, Type>> namedResolutions;
        private readonly Scope scope;
        private readonly Dictionary<ResolverKey, IResolver> globalResolvers = [];

        private static readonly List<Resolver> resolvers = [];

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider">Service provider</param>
        /// <param name="key">Key to have different resolvers</param>
        /// <exception cref="ArgumentNullException"></exception>
        internal Resolver(IServiceProvider serviceProvider, string key, Dictionary<Type, Dictionary<string, Type>> namedResolutions, Scope scope = null)
        {
            this.scope = scope ?? Scope.Default;
            this.scope.Add(this);

            resolverKey = new(this.scope, key);
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.namedResolutions = namedResolutions ?? throw new ArgumentNullException(nameof(namedResolutions));
            globalResolvers[resolverKey] = this;
            resolvers.Add(this);
        }

        public static IResolver GetResolver(Scope scope = null)
        {
            scope ??= Scope.Default;
            return resolvers.FirstOrDefault(r => r.scope == scope);
        }

        /// <summary>
        /// Resolve an instance of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        public T Get<T>() => (T)Get(typeof(T));

        public object Get(Type type)
        {
            var result = serviceProvider.GetService(type) ??
                resolvers
                    .Where(r => !r.Equals(this))
                    .Reverse()
                    .Select(r => r.serviceProvider.GetService(type))
                    .FirstOrDefault(r => r != null);

            return result;
        }

        /// <summary>
        /// Resolve an instance of type <typeparamref name="T"/> using constructor with param type <typeparamref name="T1"/>
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <typeparam name="T1">Type for constructor param 1</typeparam>
        /// <param name="arg1">Param 1 for constructor</param>
        public T Get<T, T1>(T1 arg1)
        {
            var key = ResolverKeyFactory.Create(typeof(T1));
            return Get<T, T1>(key, arg1);
        }

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
        {
            var key = ResolverKeyFactory.Create(typeof(T1), typeof(T2));
            return Get<T, T1, T2>(key, arg1, arg2);
        }

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
        {
            var key = ResolverKeyFactory.Create(typeof(T1), typeof(T2), typeof(T3));
            return Get<T, T1, T2, T3>(key, arg1, arg2, arg3);
        }

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
        {
            var key = ResolverKeyFactory.Create(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
            return Get<T, T1, T2, T3, T4>(key, arg1, arg2, arg3, arg4);
        }

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
        {
            var key = ResolverKeyFactory.Create(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
            return Get<T, T1, T2, T3, T4, T5>(key, arg1, arg2, arg3, arg4, arg5);
        }

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
        {
            var key = ResolverKeyFactory.Create(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
            return Get<T, T1, T2, T3, T4, T5, T6>(key, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// Resolve a named instance of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <param name="key">Key name to resolve</param>
        public T Get<T>(string key)
        {
            return (T)Get(typeof(T), key);
        }

        /// <summary>
        /// Resolve a named instance of type <paramref name="typeFrom"/>
        /// </summary>
        /// <param name="typeFrom">Type to resolve</param>
        /// <param name="key">Key name to resolve</param>
        public object Get(Type typeFrom, string key)
        {
            object result;
            Type type = FindTypeWithKey(typeFrom, key);
            if (type == null)
            {
                var resolver = FindResolverWithKey(key);
                result = resolver.serviceProvider.GetService(typeFrom);
            }
            else
            {
                result = serviceProvider.GetService(type);
            }
            return result;
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
            return GetImpl<T>(key, arg1);
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
            return GetImpl<T>(key, arg1, arg2);
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
            return GetImpl<T>(key, arg1, arg2, arg3);
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
            return GetImpl<T>(key, arg1, arg2, arg3, arg4);
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
            return GetImpl<T>(key, arg1, arg2, arg3, arg4, arg5);
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
            return GetImpl<T>(key, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        public T Get<T>(params object[] args)
        {
            var types = args.Select(a => a.GetType()).ToArray();
            var key = ResolverKeyFactory.Create(types);
            return GetImpl<T>(key, args);
        }

        public T Get<T>(string key, params object[] args) => GetImpl<T>(key, args);

        private T GetImpl<T>(string key, params object[] args)
        {
            var fullKey = key;
            var resolver = FindResolverWithKey(key);
            if (resolver == null)
            {
                (resolver, fullKey) = FindResolverWithPartialKey(key);
            }
            Type type = GetTypeFromNamedResolution(resolver, fullKey, typeof(T));
            return (T)ActivatorUtilities.CreateInstance(serviceProvider, type, args);
        }

        private Type GetTypeFromNamedResolution(Resolver resolver, string key, Type type)
        {
            Type result = type;
            if (resolver.namedResolutions.TryGetValue(type, out var typesByName))
            {
                if (typesByName.TryGetValue(key, out var foundType))
                {
                    result = foundType;
                }
            }

            return result;
        }

        private Type FindTypeWithKey(Type typeFrom, string key)
        {
            Type result = null;
            if (namedResolutions.TryGetValue(typeFrom, out var types))
            {
                if (types.TryGetValue(key, out var type))
                {
                    result = type;
                }
            }
            return result;
        }

        private Resolver FindResolverWithKey(string key)
        {
            ResolverKey otherKey = resolverKey with { Key = key };

            var resolver = globalResolvers.ContainsKey(otherKey)
                ? (Resolver)globalResolvers[otherKey]
                : resolvers.Reverse<Resolver>().FirstOrDefault(r => !r.Equals(this) && r.globalResolvers.ContainsKey(otherKey));

            return resolver;
        }

        private (Resolver resolver, string key) FindResolverWithPartialKey(string key)
        {
            return resolvers
                .Reverse<Resolver>()
                .SelectMany(r => r.globalResolvers)
                .Where(r => r.Key.Scope == resolverKey.Scope)
                .Where(gr => $"|{gr.Key.Key}|".Contains(key))
                .Select(gr => new { Index = gr.Key.Key.Split('|').Length, Value = (Resolver)gr.Value, gr.Key.Key })
                .OrderBy(k => k.Index)
                .Select(k => (k.Value, k.Key))
                .FirstOrDefault();
        }
    }
}