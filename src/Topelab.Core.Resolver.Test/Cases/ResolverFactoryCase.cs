using System;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Interfaces;

namespace Topelab.Core.Resolver.Test.Cases
{
    public class ResolverFactoryCase
    {
        private readonly Func<ResolveInfoCollection, Scope, IResolver> createFunc;
        private readonly Func<Scope, IResolver> getResolverFunc;
        public ResolverFactoryCase(Func<ResolveInfoCollection, Scope, IResolver> create, Func<Scope, IResolver> getResolver)
        {
            createFunc = create;
            getResolverFunc = getResolver;
        }


        public IResolver Create(ResolveInfoCollection resolveInfoColection, Scope scope = null) => createFunc(resolveInfoColection, scope);
        public IResolver GetResolver(Scope scope = null) => getResolverFunc(scope);
    }
}
