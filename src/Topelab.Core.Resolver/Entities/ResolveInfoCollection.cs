using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Add type from, type to with resolve type, key, and optionally, constructor param types for type <paramref name="typeFrom"/>
        /// </summary>
        /// <param name="resolveType">Resolve type</param>
        /// <param name="typeFrom">Type from (Interface)</param>
        /// <param name="typeTo">Type to (Implementation)</param>
        /// <param name="key">Key to resolve</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection Add(Type typeFrom, Type typeTo, ResolveLifeCycleEnum resolveType, string key, params Type[] constructorParamTypes)
        {
            Add(new ResolveInfo(typeFrom, typeTo, ResolveModeEnum.None, resolveType, key, constructorParamTypes));
            return this;
        }

        /// <summary>
        /// Add type from, type to with resolve type, and optionally, constructor param types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TFrom">Type from (Interface)</typeparam>
        /// <typeparam name="TTo">Type to (Implementation)</typeparam>
        /// <param name="resolveType">Resolve type</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection Add<TFrom, TTo>(ResolveLifeCycleEnum resolveType, params Type[] constructorParamTypes)
            => Add(typeof(TFrom), typeof(TTo), resolveType, null, constructorParamTypes);

        /// <summary>
        /// Add type from, type to with resolve type, key, and optionally, constructor param types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TFrom">Type from (Interface)</typeparam>
        /// <typeparam name="TTo">Type to (Implementation)</typeparam>
        /// <param name="resolveType">Resolve type</param>
        /// <param name="key">Key to resolve</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection Add<TFrom, TTo>(ResolveLifeCycleEnum resolveType, string key, params Type[] constructorParamTypes)
            => Add(typeof(TFrom), typeof(TTo), resolveType, key, constructorParamTypes);

        /// <summary>
        /// Add type from, type to, and optionally, constructor param types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TFrom">Type from (Interface)</typeparam>
        /// <typeparam name="TTo">Type to (Implementation)</typeparam>
        /// <param name="constructorParamTypes">Constructor param types</param>
        [Obsolete("This method is deprecated, please use AddTransient instead")]
        public ResolveInfoCollection Add<TFrom, TTo>(params Type[] constructorParamTypes)
            => Add(typeof(TFrom), typeof(TTo), ResolveLifeCycleEnum.Transient, null, constructorParamTypes);

        /// <summary>
        /// Add type from, type to, and optionally, constructor param types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TFrom">Type from (Interface)</typeparam>
        /// <typeparam name="TTo">Type to (Implementation)</typeparam>
        /// <param name="key">Key to resolve</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        [Obsolete("This method is deprecated, please use AddTransient instead")]
        public ResolveInfoCollection Add<TFrom, TTo>(string key, params Type[] constructorParamTypes)
            => Add(typeof(TFrom), typeof(TTo), ResolveLifeCycleEnum.Transient, key, constructorParamTypes);

        /// <summary>
        /// Add transient type from, type to, and optionally, constructor param types for type <paramref name="typeFrom"/>
        /// </summary>
        /// <param name="typeFrom">Type from (Interface)</param>
        /// <param name="typeTo">Type to (Implementation)</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection AddTransient(Type typeFrom, Type typeTo, params Type[] constructorParamTypes)
            => Add(typeFrom, typeTo, ResolveLifeCycleEnum.Transient, null, constructorParamTypes);

        /// <summary>
        /// Add transient type from, type to, key, and optionally, constructor param types for type <paramref name="typeFrom"/>
        /// </summary>
        /// <param name="typeFrom">Type from (Interface)</param>
        /// <param name="typeTo">Type to (Implementation)</param>
        /// <param name="key">Key to resolve</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection AddTransient(Type typeFrom, Type typeTo, string key, params Type[] constructorParamTypes)
            => Add(typeFrom, typeTo, ResolveLifeCycleEnum.Transient, key, constructorParamTypes);

        /// <summary>
        /// Add type from, type to, and optionally, constructor param types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TFrom">Type from (Interface)</typeparam>
        /// <typeparam name="TTo">Type to (Implementation)</typeparam>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection AddTransient<TFrom, TTo>(params Type[] constructorParamTypes)
            => Add(typeof(TFrom), typeof(TTo), ResolveLifeCycleEnum.Transient, null, constructorParamTypes);

        /// <summary>
        /// Add type from, type to, and optionally, constructor param types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TFrom">Type from (Interface)</typeparam>
        /// <typeparam name="TTo">Type to (Implementation)</typeparam>
        /// <param name="key">Key to resolve</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection AddTransient<TFrom, TTo>(string key, params Type[] constructorParamTypes)
            => Add(typeof(TFrom), typeof(TTo), ResolveLifeCycleEnum.Transient, key, constructorParamTypes);

        /// <summary>
        /// Add types from with instance
        /// </summary>
        /// <typeparam name="TFrom">Type from (Interface)</typeparam>
        /// <param name="instance">Instance</param>
        public ResolveInfoCollection AddInstance<TFrom>(TFrom instance)
        {
            ResolveInfo resolveInfo = new(typeof(TFrom), typeof(TFrom), ResolveModeEnum.Instance, ResolveLifeCycleEnum.Singleton) { Instance = instance };
            Add(resolveInfo);
            return this;
        }

        /// <summary>
        /// Add types from with instance with key
        /// </summary>
        /// <typeparam name="TFrom">Type from (Interface)</typeparam>
        /// <typeparam name="TTo">Type to (Implementation)</typeparam>
        /// <param name="key">Key to resolve</param>
        /// <param name="instance">Instance</param>
        public ResolveInfoCollection AddInstance<TFrom, TTo>(string key, TTo instance)
        {
            ResolveInfo resolveInfo = new(typeof(TFrom), typeof(TTo), ResolveModeEnum.Instance, ResolveLifeCycleEnum.Singleton, key) { Instance = instance };
            Add(resolveInfo);
            return this;
        }

        /// <summary>
        /// Adds the specified resolve infos.
        /// </summary>
        /// <param name="resolveInfos">The resolve infos.</param>
        public ResolveInfoCollection AddCollection(IEnumerable<ResolveInfo> resolveInfos)
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
        /// <typeparam name="TFrom">Type from (Interface)</typeparam>
        /// <param name="factory">Factory</param>
        /// <returns></returns>
        public ResolveInfoCollection AddFactory<TFrom>(Func<IResolver, TFrom> factory, ResolveLifeCycleEnum resolveLifeCycle = ResolveLifeCycleEnum.Singleton)
        {
            ResolveInfo resolveInfo = new(typeof(TFrom), typeof(TFrom), ResolveModeEnum.Factory, resolveLifeCycle) { Instance = factory };
            Add(resolveInfo);
            return this;
        }

        /// <summary>
        /// Add a factory
        /// </summary>
        /// <typeparam name="TFrom">Type from (Interface)</typeparam>
        /// <param name="key">Key to resolve</param>
        /// <param name="factory">Factory</param>
        public ResolveInfoCollection AddFactory<TFrom>(string key, Func<IResolver, TFrom> factory, ResolveLifeCycleEnum resolveLifeCycle = ResolveLifeCycleEnum.Singleton)
        {
            ResolveInfo resolveInfo = new(typeof(TFrom), typeof(TFrom), ResolveModeEnum.Factory, resolveLifeCycle, key) { Instance = factory };
            Add(resolveInfo);
            return this;
        }

        /// <summary>
        /// Add scoped type from, type to, and optionally, constructor param types for type <paramref name="typeFrom"/>
        /// </summary>
        /// <param name="typeFrom">Type from (Interface)</param>
        /// <param name="typeTo">Type to (Implementation)</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection AddScoped(Type typeFrom, Type typeTo, params Type[] constructorParamTypes)
            => Add(typeFrom, typeTo, ResolveLifeCycleEnum.Scoped, null, constructorParamTypes);

        /// <summary>
        /// Add scoped type from, type to, key, and optionally, constructor param types for type <paramref name="typeFrom"/>
        /// </summary>
        /// <param name="typeFrom">Type from (Interface)</param>
        /// <param name="typeTo">Type to (Implementation)</param>
        /// <param name="key">Key to resolve</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection AddScoped(Type typeFrom, Type typeTo, string key, params Type[] constructorParamTypes)
            => Add(typeFrom, typeTo, ResolveLifeCycleEnum.Scoped, key, constructorParamTypes);

        /// <summary>
        /// Add scoped type from, type to, and optionally, constructor param types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TFrom">Type from (Interface)</typeparam>
        /// <typeparam name="TTo">Type to (Implementation)</typeparam>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection AddScoped<TFrom, TTo>(params Type[] constructorParamTypes)
            => Add(typeof(TFrom), typeof(TTo), ResolveLifeCycleEnum.Scoped, null, constructorParamTypes);

        /// <summary>
        /// Add scoped type from, type to, key, and optionally, constructor param types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TFrom">Type from (Interface)</typeparam>
        /// <typeparam name="TTo">Type to (Implementation)</typeparam>
        /// <param name="key">Key to resolve</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection AddScoped<TFrom, TTo>(string key, params Type[] constructorParamTypes)
            => Add(typeof(TFrom), typeof(TTo), ResolveLifeCycleEnum.Scoped, key, constructorParamTypes);

        /// <summary>
        /// Add Singleton type from, type to, and optionally, constructor param types for type <paramref name="typeFrom"/>
        /// </summary>
        /// <param name="typeFrom">Type from (Interface)</param>
        /// <param name="typeTo">Type to (Implementation)</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection AddSingleton(Type typeFrom, Type typeTo, params Type[] constructorParamTypes)
            => Add(typeFrom, typeTo, ResolveLifeCycleEnum.Singleton, null, constructorParamTypes);

        /// <summary>
        /// Add Singleton type from, type to, key, and optionally, constructor param types for type <paramref name="typeFrom"/>
        /// </summary>
        /// <param name="typeFrom">Type from (Interface)</param>
        /// <param name="typeTo">Type to (Implementation)</param>
        /// <param name="key">Key to resolve</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection AddSingleton(Type typeFrom, Type typeTo, string key, params Type[] constructorParamTypes)
            => Add(typeFrom, typeTo, ResolveLifeCycleEnum.Singleton, key, constructorParamTypes);

        /// <summary>
        /// Add Singleton type from, type to, and optionally, constructor param types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TFrom">Type from (Interface)</typeparam>
        /// <typeparam name="TTo">Type to (Implementation)</typeparam>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection AddSingleton<TFrom, TTo>(params Type[] constructorParamTypes)
            => Add(typeof(TFrom), typeof(TTo), ResolveLifeCycleEnum.Singleton, null, constructorParamTypes);

        /// <summary>
        /// Add Singleton type from, type to, and optionally, constructor param types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TFrom">Type from (Interface)</typeparam>
        /// <typeparam name="TTo">Type to (Implementation)</typeparam>
        /// <param name="key">Key to resolve</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection AddSingleton<TFrom, TTo>(string key, params Type[] constructorParamTypes)
            => Add(typeof(TFrom), typeof(TTo), ResolveLifeCycleEnum.Singleton, key, constructorParamTypes);

        /// <summary>
        /// Add Self types to, and optionally, constructor param types for type <paramref name="typeTo"/>
        /// </summary>
        /// <param name="typeTo">Type to (Implementation)</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection AddSelf(Type typeTo, params Type[] constructorParamTypes)
            => Add(typeTo, typeTo, ResolveLifeCycleEnum.Transient, null, constructorParamTypes);

        /// <summary>
        /// Add Self types to, key, and optionally, constructor param types for type <paramref name="typeTo"/>
        /// </summary>
        /// <param name="typeTo">Type to (Implementation)</param>
        /// <param name="key">Key to resolve</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection AddSelf(Type typeTo, string key, params Type[] constructorParamTypes)
            => Add(typeTo, typeTo, ResolveLifeCycleEnum.Transient, key, constructorParamTypes);

        /// <summary>
        /// Add Self types to, key, and optionally, constructor param types for type <paramref name="typeTo"/>
        /// </summary>
        /// <param name="typeTo">Type to (Implementation)</param>
        /// <param name="resolveType">Resolve type</param>
        /// <param name="key">Key to resolve</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection AddSelf(Type typeTo, ResolveLifeCycleEnum resolveType, string key, params Type[] constructorParamTypes)
            => Add(typeTo, typeTo, resolveType, key, constructorParamTypes);

        /// <summary>
        /// Add Self types to, and optionally, constructor param types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TTo">Type to (Implementation)</typeparam>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection AddSelf<TTo>(params Type[] constructorParamTypes)
            => Add(typeof(TTo), typeof(TTo), ResolveLifeCycleEnum.Transient, null, constructorParamTypes);

        /// <summary>
        /// Add Self types to, and optionally, constructor param types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TTo">Type to (Implementation)</typeparam>
        /// <param name="key">Key to resolve</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection AddSelf<TTo>(string key, params Type[] constructorParamTypes)
            => Add(typeof(TTo), typeof(TTo), ResolveLifeCycleEnum.Transient, key, constructorParamTypes);

        /// <summary>
        /// Add Self types to, and optionally, constructor param types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TTo">Type to (Implementation)</typeparam>
        /// <param name="resolveType">Resolve type</param>
        /// <param name="key">Key to resolve</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfoCollection AddSelf<TTo>(ResolveLifeCycleEnum resolveType, string key, params Type[] constructorParamTypes)
            => Add(typeof(TTo), typeof(TTo), resolveType, key, constructorParamTypes);

        /// <summary>
        /// Add initializer that will be called when a resolver instance is created
        /// </summary>
        /// <typeparam name="TFrom">Type implementation (like an extension)</typeparam>
        /// <param name="initializer">Initializer action with resolver as  param</param>
        public ResolveInfoCollection AddInitializer(Action<IResolver> initializer)
        {
            ResolveInfo resolveInfo = new(typeof(object), typeof(object), ResolveModeEnum.Initializer, ResolveLifeCycleEnum.Singleton) { Instance = initializer };
            Add(resolveInfo);
            return this;
        }

        /// <summary>
        /// Initialize all registered initializers
        /// </summary>
        /// <param name="resolver">Resolver</param>
        public void InitializeIntializers(IResolver resolver)
        {
            foreach (var resolveInfo in this.Where(r => r.ResolveMode == ResolveModeEnum.Initializer))
            {
                if (resolveInfo.Instance is Action<IResolver> initializer)
                {
                    initializer(resolver);
                }
            }
        }
    }
}
