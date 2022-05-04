using System;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Interfaces;

namespace Topelab.Core.Resolver.Test.Cases
{
    public class ResolverFactoryCase
    {
        public ResolverFactoryCase(Func<ResolveInfoCollection, IResolver> create, Func<IResolver> getResolver)
        {
            Create = create;
            GetResolver = getResolver;
        }

        public Func<ResolveInfoCollection, IResolver> Create { get; }
        public Func<IResolver> GetResolver { get; }
    }
}
