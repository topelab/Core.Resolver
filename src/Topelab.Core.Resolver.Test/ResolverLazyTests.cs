using NUnit.Framework;
using System;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Test.Cases;
using Topelab.Core.Resolver.Test.Entities;
using Topelab.Core.Resolver.Test.Interfaces;

namespace Topelab.Core.Resolver.Test
{
    [TestFixture]
    internal class ResolverLazyTests
    {
        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void ResolveInfoCollection_OnAddLazyTransient_ThenLazyInstantCreated(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddTransient<IClaseTest, SimpleClaseTest>()
                );

            // Act
            var result = resolver.Get<Lazy<IClaseTest>>();

            // Assert
            Assert.That(new SimpleClaseTest().GiveMe(), Is.EqualTo(result.Value.GiveMe()));

        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void ResolveInfoCollection_OnAddLazyScoped_ThenLazyInstantCreated(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddScoped<IClaseTest, SimpleClaseTest>()
                );

            // Act
            var result = resolver.Get<Lazy<IClaseTest>>();

            // Assert
            Assert.That(new SimpleClaseTest().GiveMe(), Is.EqualTo(result.Value.GiveMe()));

        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void ResolveInfoCollection_OnAddLazySingleton_ThenLazyInstantCreated(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddSingleton<IClaseTest, SimpleClaseTest>()
                );

            // Act
            var result = resolver.Get<Lazy<IClaseTest>>();

            // Assert
            Assert.That(new SimpleClaseTest().GiveMe(), Is.EqualTo(result.Value.GiveMe()));

        }
    }
}
