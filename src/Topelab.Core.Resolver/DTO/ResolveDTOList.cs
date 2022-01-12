using System;
using System.Collections.Generic;
using Topelab.Core.Resolver.Enums;

namespace Topelab.Core.Resolver.DTO
{
    /// <summary>
    /// Resolve DTO list
    /// </summary>
    public class ResolveDTOList : List<ResolveDTO>
    {
        /// <summary>
        /// Add types from, to, and optionally, contructor params types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TFrom">Type from</typeparam>
        /// <typeparam name="TTo">Type to</typeparam>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveDTOList Add<TFrom, TTo>(params Type[] constructorParamTypes)
        {
            Add(new ResolveDTO(typeof(TFrom), typeof(TTo), ResolveTypeEnum.PerResolve, null, constructorParamTypes));
            return this;
        }

        /// <summary>
        /// Add types from, to, and optionally, contructor params types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TFrom">Type from</typeparam>
        /// <typeparam name="TTo">Type to</typeparam>
        /// <param name="key">Key to resolve</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveDTOList Add<TFrom, TTo>(string key, params Type[] constructorParamTypes)
        {
            Add(new ResolveDTO(typeof(TFrom), typeof(TTo), ResolveTypeEnum.PerResolve, key, constructorParamTypes));
            return this;
        }

        /// <summary>
        /// Add types from, to with resolve type, and optionally, contructor params types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TFrom">Type from</typeparam>
        /// <typeparam name="TTo">Type to</typeparam>
        /// <param name="resolveType">Resolve type</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveDTOList Add<TFrom, TTo>(ResolveTypeEnum resolveType, params Type[] constructorParamTypes)
        {
            Add(new ResolveDTO(typeof(TFrom), typeof(TTo), resolveType, null, constructorParamTypes));
            return this;
        }

        /// <summary>
        /// Add types from, to with resolve type, and optionally, contructor params types for type <typeparamref name="TTo"/>
        /// </summary>
        /// <typeparam name="TFrom">Type from</typeparam>
        /// <typeparam name="TTo">Type to</typeparam>
        /// <param name="resolveType">Resolve type</param>
        /// <param name="key">Key to resolve</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveDTOList Add<TFrom, TTo>(ResolveTypeEnum resolveType, string key, params Type[] constructorParamTypes)
        {
            Add(new ResolveDTO(typeof(TFrom), typeof(TTo), resolveType, key, constructorParamTypes));
            return this;
        }

        /// <summary>
        /// Add types from, to with instance
        /// </summary>
        /// <typeparam name="TFrom">Type from</typeparam>
        /// <typeparam name="TTo">Type to</typeparam>
        /// <param name="instance">Instance</param>
        public ResolveDTOList Add<TFrom, TTo>(TTo instance)
        {
            var resolveDTO = new ResolveDTO(typeof(TFrom), typeof(TTo), ResolveTypeEnum.Instance) { Instance = instance };
            Add(resolveDTO);
            return this;
        }
    }
}
