using NUnit.Framework;
using System;
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
                .Add<IClaseTest, ClaseTest>()
                .Add<IClaseTest, ClaseTest>("1")
                .Add<IClaseTest, ClaseTest>("2");

            var resolveInfoCollection2 = new ResolveInfoCollection()
                .Add<IClaseTest, ClaseTest>("3")
                .Add<IClaseTest, ClaseTest>("4")
                .Add<IClaseTest, ClaseTest>("5");

            // Act
            var result = resolveInfoCollection.AddCollection(resolveInfoCollection2);

            // Assert
            Assert.IsTrue(result.Count == 6);
        }
    }
}
