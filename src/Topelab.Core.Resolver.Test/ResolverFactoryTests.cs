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
                .AddTransient<IClaseTest, SimpleClaseTest>()
                .AddTransient<IClaseTest, SimpleClaseTest2>("named")
                .AddFactory(r => r.Get<IClaseTest>("named"))
                );

            // Act
            var result = resolver.Get<IClaseTest>();

            // Assert
            Assert.That(result, Is.TypeOf<SimpleClaseTest2>());
        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void Get_UsingCurrentResolver(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddTransient<IClaseTest, SimpleClaseTest>()
                .AddTransient<IClaseTest, SimpleClaseTest2>("named")
                .AddFactory(r => r.Get<IClaseTest>("named"))
                );

            var otherResolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddTransient<IClaseTest, ClaseTest>()
                .AddTransient<IClaseTest, ClaseTest>("named")
                .AddFactory(r => r.Get<IClaseTest>("named"))
                );


            // Act
            var currentResolver = ResolverFactory.GetResolver();

            // Assert
            Assert.That(currentResolver, Is.Not.EqualTo(otherResolver));
        }

    }
}
