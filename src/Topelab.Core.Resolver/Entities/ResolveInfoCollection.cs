using System;
using System.Collections.Generic;
using Topelab.Core.Resolver.Enums;

namespace Topelab.Core.Resolver.Entities
{
    /// <summary>
    /// Resolve info collection
    /// </summary>
    public class ResolveInfoCollection : List<ResolveInfo>
    {
        /// <summary>
        /// Add types from, to, and optionally, constructor param types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TFrom">Type from</typeparam>
        /// <typeparam name="TTo">Type to</typeparam>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection Add<TFrom, TTo>(params Type[] constructorParamTypes)
        {
            Add(new ResolveInfo(typeof(TFrom), typeof(TTo), ResolveTypeEnum.PerResolve, null, constructorParamTypes));
            return this;
        }

        /// <summary>
        /// Add types from, to, and optionally, constructor param types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TFrom">Type from</typeparam>
        /// <typeparam name="TTo">Type to</typeparam>
        /// <param name="key">Key to resolve</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection Add<TFrom, TTo>(string key, params Type[] constructorParamTypes)
        {
            Add(new ResolveInfo(typeof(TFrom), typeof(TTo), ResolveTypeEnum.PerResolve, key, constructorParamTypes));
            return this;
        }

        /// <summary>
        /// Add types from, to with resolve type, and optionally, constructor param types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TFrom">Type from</typeparam>
        /// <typeparam name="TTo">Type to</typeparam>
        /// <param name="resolveType">Resolve type</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection Add<TFrom, TTo>(ResolveTypeEnum resolveType, params Type[] constructorParamTypes)
        {
            Add(new ResolveInfo(typeof(TFrom), typeof(TTo), resolveType, null, constructorParamTypes));
            return this;
        }

        /// <summary>
        /// Add types from, to with resolve type, and optionally, constructor param types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TFrom">Type from</typeparam>
        /// <typeparam name="TTo">Type to</typeparam>
        /// <param name="resolveType">Resolve type</param>
        /// <param name="key">Key to resolve</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection Add<TFrom, TTo>(ResolveTypeEnum resolveType, string key, params Type[] constructorParamTypes)
        {
            Add(new ResolveInfo(typeof(TFrom), typeof(TTo), resolveType, key, constructorParamTypes));
            return this;
        }

        /// <summary>
        /// Add types from, to with instance
        /// </summary>
        /// <typeparam name="TFrom">Type from</typeparam>
        /// <typeparam name="TTo">Type to</typeparam>
        /// <param name="instance">Instance</param>
        public ResolveInfoCollection Add<TFrom, TTo>(TTo instance)
        {
            ResolveInfo resolveInfo = new(typeof(TFrom), typeof(TTo), ResolveTypeEnum.Instance) { Instance = instance };
            Add(resolveInfo);
            return this;
        }

        /// <summary>
        /// Add types from, to with instance with key
        /// </summary>
        /// <typeparam name="TFrom">Type from</typeparam>
        /// <typeparam name="TTo">Type to</typeparam>
        /// <param name="key">Key to resolve</param>
        /// <param name="instance">Instance</param>
        public ResolveInfoCollection Add<TFrom, TTo>(string key, TTo instance)
        {
            ResolveInfo resolveInfo = new(typeof(TFrom), typeof(TTo), ResolveTypeEnum.Instance, key) { Instance = instance };
            Add(resolveInfo);
            return this;
        }
    }
}
