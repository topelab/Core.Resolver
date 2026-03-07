using System;
using System.Runtime.CompilerServices;
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


        public IResolver Create(ResolveInfoCollection resolveInfoColection, [CallerMemberName] string scope = null) => createFunc(resolveInfoColection, new Scope(scope));
        public IResolver GetResolver([CallerMemberName] string scope = null) => getResolverFunc(new Scope(scope));
    }
}
