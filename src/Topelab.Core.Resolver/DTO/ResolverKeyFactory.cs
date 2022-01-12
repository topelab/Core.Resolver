using System;
using System.Linq;

namespace Topelab.Core.Resolver.DTO
{
    public class ResolverKeyFactory
    {
        public static string Create(ResolveDTO resolveDTO) => resolveDTO.Key ?? Create(resolveDTO.ConstructorParamTypes);

        public static string Create(params Type[] types) => (types != null && types.Length > 0) ? string.Join("|", types.Select(p => p.Name)) : null;
    }
}
