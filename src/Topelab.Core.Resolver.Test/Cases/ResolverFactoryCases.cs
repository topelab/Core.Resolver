using System;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Interfaces;

namespace Topelab.Core.Resolver.Test.Cases
{
    internal class ResolverCases
    {
        public static readonly Func<ResolveInfoCollection, IResolver>[] ResolverFactories =
        {
            Microsoft.ResolverFactory.Create,
            Unity.ResolverFactory.Create,
            Autofac.ResolverFactory.Create,
        };
    }
}
