using System.Collections.Generic;
using Topelab.Core.Resolver.Interfaces;

namespace Topelab.Core.Resolver.Entities
{
    public class Scope(string defaultScope)
    {
        public const string DEFAULT_SCOPE = ".";

        private string value = defaultScope;
        private static readonly Scope defaultInstance = new(DEFAULT_SCOPE);
        private IResolver rootResolver;
        private readonly List<IResolver> resolvers = [];

        public string Value
        {
            get { return @value; }
            set { this.@value = value; }
        }

        public void Add(IResolver resolver)
        {
            rootResolver ??= resolver;
            if (!resolvers.Contains(resolver))
            {
                resolvers.Add(resolver);
            }
        }

        public IResolver Resolver => rootResolver;

        public static Scope Default => defaultInstance;

    }
}
