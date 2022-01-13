using System;
using System.Linq;

namespace Topelab.Core.Resolver.Entities
{
    /// <summary>
    /// Key factory for resolver
    /// </summary>
    public class ResolverKeyFactory
    {
        /// <summary>
        /// Creates the specified resolve info.
        /// </summary>
        /// <param name="resolveInfo">The resolve info.</param>
        public static string Create(ResolveInfo resolveInfo) => resolveInfo.Key ?? Create(resolveInfo.ConstructorParamTypes);

        /// <summary>
        /// Creates the specified types.
        /// </summary>
        /// <param name="types">The types.</param>
        public static string Create(params Type[] types) => types?.Length > 0 ? string.Join("|", types.Select(p => p.Name)) : null;
    }
}
