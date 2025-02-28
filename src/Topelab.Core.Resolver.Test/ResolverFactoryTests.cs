using NUnit.Framework;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Test.Cases;
using Topelab.Core.Resolver.Test.Entities;
using Topelab.Core.Resolver.Test.Interfaces;

namespace Topelab.Core.Resolver.Test
{
    [TestFixture]
    public class ResolverFactoryTests
    {
        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void Get_UsingGetInFactory(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddSelf<SimpleClaseTest>()
                .AddSelf<SimpleClaseTest>("named")
                .AddFactory<IClaseTest>(r => r.Get<SimpleClaseTest>("named"))
                );

            // Act
            var result = resolver.Get<IClaseTest>();

            // Assert
            Assert.That(new SimpleClaseTest().GiveMe(), Is.EqualTo(result.GiveMe()));
        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void Get_UsingCurrentResolver(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddSelf<SimpleClaseTest>()
                .AddSelf<SimpleClaseTest>("named")
                .AddFactory<IClaseTest>(r => r.Get<SimpleClaseTest>("named"))
                );

            var otherResolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddSelf<ClaseTest>()
                .AddSelf<ClaseTest>("named")
                .AddFactory<IClaseTest>(r => r.Get<ClaseTest>("named"))
                );

            // Act
            var resolver2 = ResolverFactory.GetResolver();
            var result = resolver2.Get<IClaseTest>();

            // Assert
            Assert.That(new SimpleClaseTest().GiveMe(), Is.EqualTo(result.GiveMe()));
        }

    }
}
