using NUnit.Framework;
using System;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Interfaces;
using Topelab.Core.Resolver.Test.Cases;
using Topelab.Core.Resolver.Test.Entities;
using Topelab.Core.Resolver.Test.Interfaces;

namespace Topelab.Core.Resolver.Test
{
    [TestFixture]
    public class ResolverFactoryTests
    {
        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactories))]
        public void Get_UsingGetInFactory(Func<ResolveInfoCollection, IResolver> ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory(new ResolveInfoCollection()
                .AddSelf<SimpleClaseTest>()
                .AddSelf<SimpleClaseTest>("named")
                .AddFactory<IClaseTest>(r => r.Get<SimpleClaseTest>("named"))
                );

            // Act
            var result = resolver.Get<IClaseTest>();

            // Assert
            Assert.AreEqual(new SimpleClaseTest().GiveMe(), result.GiveMe());
        }

    }
}
