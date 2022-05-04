namespace Topelab.Core.Resolver.Test.Cases
{
    internal class ResolverCases
    {
        public static readonly ResolverFactoryCase[] ResolverFactoriesCases =
        {
            new (Microsoft.ResolverFactory.Create, Microsoft.ResolverFactory.GetResolver),
            new (Unity.ResolverFactory.Create, Unity.ResolverFactory.GetResolver),
            new (Autofac.ResolverFactory.Create, Autofac.ResolverFactory.GetResolver),
        };
    }
}
