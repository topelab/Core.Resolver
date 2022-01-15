using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Threading;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Test.Entities;
using Topelab.Core.Resolver.Test.Interfaces;
using Topelab.Core.Resolver.Unity;

namespace Topelab.Core.Resolver.Test
{
    [TestFixture]
    public class ResolverUnityTests
    {
        private ResolverTests resolverTests;

        [OneTimeSetUp]
        public void Setup()
        {
            resolverTests = new(ResolverFactory.Create);
        }

        [Test]
        public void Get_Simple()
        {
            resolverTests.GetSimple();
        }

        [Test]
        public void Get_SimpleWithNames()
        {
            resolverTests.GetSimpleWithNames();
        }

        [Test]
        public void Get_MultipleConstructors_AreOK()
        {
            resolverTests.GetWithMultipleConstructors();
        }

        [Test]
        public void Get_MultipleImplementations_AreOK()
        {
            resolverTests.GetWithMultipleImplementations();
        }

        [Test]
        public void Get_WithMultipleResolvers()
        {
            resolverTests.GetWithMultipleResolvers();
        }

        [Test]
        public void GetWithDifferentParamsSameType()
        {
            resolverTests.GetWithDifferentParamsSameType();
        }

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
