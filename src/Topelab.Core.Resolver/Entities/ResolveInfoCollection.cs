using System;
using System.Collections.Generic;
using Topelab.Core.Resolver.Enums;
using Topelab.Core.Resolver.Interfaces;

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
            Add(new ResolveInfo(typeof(TFrom), typeof(TTo), ResolveModeEnum.None, ResolveLifeCycleEnum.Transient, null, constructorParamTypes));
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
            Add(new ResolveInfo(typeof(TFrom), typeof(TTo), ResolveModeEnum.None, ResolveLifeCycleEnum.Transient, key, constructorParamTypes));
            return this;
        }

        /// <summary>
        /// Add types from, to with resolve type, and optionally, constructor param types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TFrom">Type from</typeparam>
        /// <typeparam name="TTo">Type to</typeparam>
        /// <param name="resolveType">Resolve type</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection Add<TFrom, TTo>(ResolveLifeCycleEnum resolveType, params Type[] constructorParamTypes)
        {
            Add(new ResolveInfo(typeof(TFrom), typeof(TTo), ResolveModeEnum.None, resolveType, null, constructorParamTypes));
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
        public ResolveInfoCollection Add<TFrom, TTo>(ResolveLifeCycleEnum resolveType, string key, params Type[] constructorParamTypes)
        {
            Add(new ResolveInfo(typeof(TFrom), typeof(TTo), ResolveModeEnum.None, resolveType, key, constructorParamTypes));
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
            ResolveInfo resolveInfo = new(typeof(TFrom), typeof(TTo), ResolveModeEnum.Instance, ResolveLifeCycleEnum.Singleton) { Instance = instance };
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
            ResolveInfo resolveInfo = new(typeof(TFrom), typeof(TTo), ResolveModeEnum.Instance, ResolveLifeCycleEnum.Singleton, key) { Instance = instance };
            Add(resolveInfo);
            return this;
        }

        /// <summary>
        /// Adds the specified resolve infos.
        /// </summary>
        /// <param name="resolveInfos">The resolve infos.</param>
        public ResolveInfoCollection Add(IEnumerable<ResolveInfo> resolveInfos)
        {
            foreach (var resolveInfo in resolveInfos)
            {
                Add(resolveInfo);
            }
            return this;
        }

        /// <summary>
        /// Add a factory
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <param name="factory"></param>
        /// <returns></returns>
        public ResolveInfoCollection Add<TFrom>(Func<IResolver, TFrom> factory)
        {
            ResolveInfo resolveInfo = new(typeof(TFrom), typeof(TFrom), ResolveModeEnum.Factory, ResolveLifeCycleEnum.Singleton) { Instance = factory };
            Add(resolveInfo);
            return this;
        }
    }
}
