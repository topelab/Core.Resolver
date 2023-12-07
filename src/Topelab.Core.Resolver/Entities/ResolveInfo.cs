using System;
using Topelab.Core.Resolver.Enums;
using Topelab.Core.Resolver.Interfaces;

namespace Topelab.Core.Resolver.Entities
{
    /// <summary>
    /// Resolve info
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="typeFrom">Type from</param>
    /// <param name="typeTo">Type to</param>
    /// <param name="resolveMode">Resolve mode</param>
    /// <param name="resolveLifeCycle">Resolve life cycle enumerator</param>
    /// <param name="key"></param>
    /// <param name="constructorParamTypes">Constructor param types</param>
    public class ResolveInfo(Type typeFrom, Type typeTo, ResolveModeEnum resolveMode = ResolveModeEnum.None, ResolveLifeCycleEnum resolveLifeCycle = ResolveLifeCycleEnum.Transient, string key = null, Type[] constructorParamTypes = null)
    {
        /// <summary>
        /// Type from
        /// </summary>
        public Type TypeFrom { get; set; } = typeFrom;

        /// <summary>
        /// Typo to
        /// </summary>
        public Type TypeTo { get; set; } = typeTo;

        /// <summary>
        /// Resolve type
        /// </summary>
        public ResolveLifeCycleEnum ResolveLifeCycle { get; set; } = resolveLifeCycle;

        /// <summary>
        /// Resolve mode.
        /// </summary>
        public ResolveModeEnum ResolveMode { get; set; } = resolveMode;

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
        public string Key { get; set; } = key ?? ResolverKeyFactory.Create(constructorParamTypes);

        /// <summary>
        /// Constructors param types
        /// </summary>
        public Type[] ConstructorParamTypes { get; set; } = constructorParamTypes ?? [];
    }
}
