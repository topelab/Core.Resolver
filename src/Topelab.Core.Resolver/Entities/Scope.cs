using System.Collections.Generic;
using Topelab.Core.Resolver.Interfaces;

namespace Topelab.Core.Resolver.Entities
{
    public class Scope(string tag)
    {
        public const string DEFAULT_TAG = ".";

        private string tag = tag;
        private static readonly Scope defaultInstance = new(DEFAULT_TAG);
        private IResolver rootResolver;
        private readonly List<IResolver> resolvers = [];

        public string Tag
        {
            get => tag ?? DEFAULT_TAG;
            set => tag = value ?? DEFAULT_TAG;
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
