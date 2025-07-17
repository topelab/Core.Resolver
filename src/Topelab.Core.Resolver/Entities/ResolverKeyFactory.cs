using System;
using System.Linq;

namespace Topelab.Core.Resolver.Entities
{
    /// <summary>
    /// Key factory for resolver
    /// </summary>
    public class ResolverKeyFactory
    {
        public const string KEY_SEPARATOR = ":";
        public const string TYPE_SEPARATOR = "|";

        /// <summary>
        /// Creates the specified resolve info.
        /// </summary>
        /// <param name="resolveInfo">The resolve info.</param>
        public static string Create(ResolveInfo resolveInfo)
            => Create(resolveInfo.Key, resolveInfo.ConstructorParamTypes);

        /// <summary>
        /// Creates the specified types.
        /// </summary>
        /// <param name="types">The types.</param>
        public static string Create(params Type[] types)
            => types?.Length > 0 ? string.Join(TYPE_SEPARATOR, types.Select(p => p.Name)) : null;

        /// <summary>
        /// Creates the specified types.
        /// </summary>
        /// <param name="key"> The key.</param>
        /// <param name="types">The types.</param>
        public static string Create(string key, params Type[] types)
            => types?.Length > 0 ? $"{key}{KEY_SEPARATOR}{Create(types)}" : key;
    }
}
