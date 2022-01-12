using System;
using System.Linq;
using Topelab.Core.Resolver.Enums;

namespace Topelab.Core.Resolver.DTO
{
    /// <summary>
    /// Resolve DTO
    /// </summary>
    public class ResolveDTO
    {
        /// <summary>
        /// Type from
        /// </summary>
        public Type TypeFrom { get; set; }

        /// <summary>
        /// Typo to
        /// </summary>
        public Type TypeTo { get; set; }

        /// <summary>
        /// Resolve type
        /// </summary>
        public ResolveTypeEnum ResolveType { get; set; }

        /// <summary>
        /// Instance
        /// </summary>
        public object Instance { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Constructors param types
        /// </summary>
        public Type[] ConstructorParamTypes { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="typeFrom">Type from</param>
        /// <param name="typeTo">Type to</param>
        /// <param name="resolveType">Resolve type</param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveDTO(Type typeFrom, Type typeTo, ResolveTypeEnum resolveType = ResolveTypeEnum.PerResolve, string key = null, Type[] constructorParamTypes = null)
        {
            TypeFrom = typeFrom;
            TypeTo = typeTo;
            ResolveType = resolveType;
            Key = key ?? ResolverKeyFactory.Create(constructorParamTypes);
            ConstructorParamTypes = constructorParamTypes;
        }
    }
}
