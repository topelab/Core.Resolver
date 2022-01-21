using System;
using Topelab.Core.Resolver.Enums;
using Topelab.Core.Resolver.Interfaces;

namespace Topelab.Core.Resolver.Entities
{
    /// <summary>
    /// Resolve info
    /// </summary>
    public class ResolveInfo
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
        public ResolveLifeCycleEnum ResolveLifeCycle { get; set; }

        /// <summary>
        /// Resolve mode.
        /// </summary>
        public ResolveModeEnum ResolveMode { get; set; }

        /// <summary>
        /// Instance
        /// </summary>
        public object Instance { get; set; }

        /// <summary>
        /// Factory representation for instance
        /// </summary>
        public Func<IResolver, object> Factory => (Func<IResolver, object>)Instance; 

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
        /// <param name="resolveMode">Resolve mode</param>
        /// <param name="resolveLifeCycle">Resolve life cycle enumerator</param>
        /// <param name="key"></param>
        /// <param name="constructorParamTypes">Constructor param types</param>
        public ResolveInfo(Type typeFrom, Type typeTo, ResolveModeEnum resolveMode = ResolveModeEnum.None, ResolveLifeCycleEnum resolveLifeCycle = ResolveLifeCycleEnum.Transient, string key = null, Type[] constructorParamTypes = null)
        {
            TypeFrom = typeFrom;
            TypeTo = typeTo;
            ResolveLifeCycle = resolveLifeCycle;
            ResolveMode = resolveMode;
            Key = key ?? ResolverKeyFactory.Create(constructorParamTypes);
            ConstructorParamTypes = constructorParamTypes;
        }
    }
}
