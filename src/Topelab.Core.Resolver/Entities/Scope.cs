using System.Collections.Generic;
using Topelab.Core.Resolver.Interfaces;

namespace Topelab.Core.Resolver.Entities
{
    public class Scope(string tag)
    {
        public const string DEFAULT_TAG = ".";

        private string tag = tag;
        private static readonly Scope defaultInstance = new(DEFAULT_TAG);
        private IResolver resolver;
        private readonly HashSet<IResolver> resolvers = [];

        public string Tag
        {
            get => tag ?? DEFAULT_TAG;
            set => tag = value ?? DEFAULT_TAG;
        }

        public void Add(IResolver resolver)
        {
            this.resolver ??= resolver;
            resolvers.Add(resolver);
        }

        public IResolver Resolver => resolver;

        public static Scope Default => defaultInstance;

    }
}
