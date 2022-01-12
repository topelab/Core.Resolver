using NUnit.Framework;
using NUnit.Framework.Internal;
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
    }

}
