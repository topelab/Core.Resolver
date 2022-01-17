namespace Topelab.Core.Resolver.Enums
{
    /// <summary>
    /// Resolve type enumeration
    /// </summary>
    public enum ResolveLifeCycleEnum
    {
        /// <summary>
        /// Per resolve
        /// </summary>
        Transient,

        /// <summary>
        /// Scoped
        /// </summary>
        Scoped,

        /// <summary>
        /// Singleton life time
        /// </summary>
        Singleton,
    }
}