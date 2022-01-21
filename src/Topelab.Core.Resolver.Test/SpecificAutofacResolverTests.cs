using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using Topelab.Core.Resolver.Autofac;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Test.Entities;
using Topelab.Core.Resolver.Test.Interfaces;

namespace Topelab.Core.Resolver.Test
{
    [TestFixture]
    public class SpecificAutofacResolverTests
    {
        [Test]
        public void GetFactory()
        {
            // Arrange
            var number = 99;
            var dateTime = DateTime.Now;
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddFactory(s => (IClaseTest)Activator.CreateInstance(typeof(ClaseTest), number, dateTime))
                .AddFactory("dos", s => (IClaseTest)Activator.CreateInstance(typeof(ClaseTest), number * 2, dateTime))
                );
            ClaseTest goodResult = new(number, dateTime);
            ClaseTest goodResultDos = new(number * 2, dateTime);

            // Act
            var result = resolver.Get<IClaseTest>();
            var resultDos = resolver.Get<IClaseTest>("dos");

            // Assert
            Assert.AreEqual(goodResult.GiveMe(), result.GiveMe());
            Assert.AreEqual(goodResultDos.GiveMe(), resultDos.GiveMe());
        }
    }

}
