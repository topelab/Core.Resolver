using NUnit.Framework;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Test.Cases;
using Topelab.Core.Resolver.Test.Entities;
using Topelab.Core.Resolver.Test.Interfaces;

namespace Topelab.Core.Resolver.Test
{
    [TestFixture]
    internal class ResolverTests
    {
        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void GetSimple(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .Add<IClaseTest, SimpleClaseTest>()
                );

            // Act
            var result = resolver.Get<IClaseTest>();

            // Assert
            Assert.AreEqual(new SimpleClaseTest().GiveMe(), result.GiveMe());
        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void GetSimpleWithNames(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .Add<IClaseTest, SimpleClaseTest>()
                .Add<IClaseTest, SimpleClaseTest2>(nameof(SimpleClaseTest2))
                );

            // Act
            var result = resolver.Get<IClaseTest>();
            var result2 = resolver.Get<IClaseTest>(nameof(SimpleClaseTest2));

            // Assert
            Assert.AreNotEqual(result, result2);
        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void GetWithMultipleConstructors(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .Add<IGeremuDbContext, GeremuDbContext>()
                .Add<IClaseTest, SimpleClaseTest>()
                .Add<IClaseTest, ClaseTest>(typeof(IGeremuDbContext), typeof(string))
                .Add<IClaseTest, ClaseTest>(typeof(IGeremuDbContext), typeof(string), typeof(int)));

            var param = "Direct hello";

            // Act
            var result2 = resolver.Get<IClaseTest, IGeremuDbContext, string, int>(resolver.Get<IGeremuDbContext>(), param, 2);
            var result0 = resolver.Get<IClaseTest, IGeremuDbContext, string>(resolver.Get<IGeremuDbContext>(), param);
            var simple = resolver.Get<IClaseTest>();

            // Assert
            Assert.IsTrue(result2.GiveMe().StartsWith($"{param} (") && result2.GiveMe().EndsWith("2"));
            Assert.IsTrue(result0.GiveMe().StartsWith($"{param} (") && result0.GiveMe().EndsWith("0"));
            Assert.IsTrue(simple.GiveMe().Equals("Simple class"));
        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void GetWithMultipleImplementations(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .Add<IGeremuDbContext, GeremuDbContext>()
                .Add<IClaseTest, SimpleClaseTest>()
                .Add<IClaseTest, SimpleClaseTest2>(nameof(SimpleClaseTest2))
                );

            // Act
            var result = resolver.Get<IClaseTest>();
            var result2 = resolver.Get<IClaseTest>(nameof(SimpleClaseTest2));

            // Assert
            Assert.AreNotSame(result, result2);
        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void GetWithMultipleResolvers(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .Add<IGeremuDbContext, GeremuDbContext>()
                .Add<IClaseTest, SimpleClaseTest>()
                .Add<IClaseTest, SimpleClaseTest2>(nameof(SimpleClaseTest2))
                );

            var resolver2 = ResolverFactory.Create(new ResolveInfoCollection()
                .Add<IGeremuDbContext, GeremuDbContext>()
                .Add<IClaseTest, SimpleClaseTest>()
                .Add<IClaseTest, SimpleClaseTest2>(nameof(SimpleClaseTest2))
                );

            // Act
            var result = resolver.Get<IClaseTest>();
            var result2 = resolver.Get<IClaseTest>(nameof(SimpleClaseTest2));
            var result20 = resolver2.Get<IClaseTest>();
            var result202 = resolver2.Get<IClaseTest>(nameof(SimpleClaseTest2));

            // Assert
            Assert.AreNotSame(result, result2);
            Assert.AreNotSame(result20, result202);
            Assert.AreNotSame(result, result20);
            Assert.AreNotSame(result2, result202);
        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void GetWithDifferentParamsSameType(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .Add<IClaseTest, SimpleClaseTest>(nameof(SimpleClaseTest))
                .Add<IClaseTest, SimpleClaseTest2>(nameof(SimpleClaseTest2))
                .Add<IClaseTest, ClassUsingSimple>(typeof(IClaseTest), typeof(IClaseTest))
                );

            // Act
            var simpleClaseTest = resolver.Get<IClaseTest>(nameof(SimpleClaseTest));
            var simpleClaseTest2 = resolver.Get<IClaseTest>(nameof(SimpleClaseTest2));
            var claseTest = resolver.Get<IClaseTest, IClaseTest, IClaseTest>(simpleClaseTest, simpleClaseTest2);

            // Assert
            Assert.AreNotSame(simpleClaseTest, simpleClaseTest2);
            Assert.AreEqual($"{simpleClaseTest.GiveMe()} - {simpleClaseTest2.GiveMe()}", claseTest.GiveMe());
        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void GetWithConstructorResolvedByParam(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .Add<IClaseTest, SimpleClaseTest>()
                .Add<IGeremuDbContext, GeremuDbContext>()
                );

            // Act
            var simpleClaseTest = resolver.Get<IClaseTest>();
            var geremuDbContext = resolver.Get<IGeremuDbContext>();

            // Assert
            Assert.AreEqual($"Hello, I'm instance {geremuDbContext.Id} with claseTest: {simpleClaseTest.GiveMe()}", ((IClaseTest)geremuDbContext).GiveMe());
        }

    }
}
