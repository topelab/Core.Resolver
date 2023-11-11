using NUnit.Framework;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Test.Entities;
using Topelab.Core.Resolver.Test.Interfaces;

namespace Topelab.Core.Resolver.Test
{
    [TestFixture]
    public class ResolveInfoCollectionTests
    {
        [Test]
        public void Add_AddResolveInfoCollection_ReturnResolveInfoCollectionWithAdded()
        {
            // Arrange
            var resolveInfoCollection = new ResolveInfoCollection()
                .AddTransient<IClaseTest, ClaseTest>()
                .AddTransient<IClaseTest, ClaseTest>("1")
                .AddTransient<IClaseTest, ClaseTest>("2");

            var resolveInfoCollection2 = new ResolveInfoCollection()
                .AddTransient<IClaseTest, ClaseTest>("3")
                .AddTransient<IClaseTest, ClaseTest>("4")
                .AddTransient<IClaseTest, ClaseTest>("5");

            // Act
            var result = resolveInfoCollection.AddCollection(resolveInfoCollection2);

            // Assert
            Assert.That(result.Count == 6);
        }
    }
}
