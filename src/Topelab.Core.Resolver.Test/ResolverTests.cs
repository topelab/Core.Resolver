using NUnit.Framework;
using System.Globalization;
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
                .AddTransient<IClaseTest, SimpleClaseTest>()
                );

            // Act
            var result = resolver.Get<IClaseTest>();

            // Assert
            Assert.That(new SimpleClaseTest().GiveMe(), Is.EqualTo(result.GiveMe()));
        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void GetSimple_NotGeneric(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddTransient<IClaseTest, SimpleClaseTest>()
                );

            // Act
            var result = (IClaseTest)resolver.Get(typeof(IClaseTest));

            // Assert
            Assert.That(new SimpleClaseTest().GiveMe(), Is.EqualTo(result.GiveMe()));
        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void GetSimpleWithNames(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddTransient<IClaseTest, SimpleClaseTest>()
                .AddTransient<IClaseTest, SimpleClaseTest2>(nameof(SimpleClaseTest2))
                );

            // Act
            var result = resolver.Get<IClaseTest>();
            var result2 = resolver.Get<IClaseTest>(nameof(SimpleClaseTest2));

            // Assert
            Assert.That(result, Is.Not.EqualTo(result2));
        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void GetSimpleWithNames_NotGeneric(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddTransient<IClaseTest, SimpleClaseTest>()
                .AddTransient<IClaseTest, SimpleClaseTest2>(nameof(SimpleClaseTest2))
                );

            // Act
            var result = (IClaseTest)resolver.Get(typeof(IClaseTest));
            var result2 = resolver.Get<IClaseTest>(nameof(SimpleClaseTest2));

            // Assert
            Assert.That(result, Is.Not.EqualTo(result2));
        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void GetWithMultipleConstructors(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddTransient<IGeremuDbContext, GeremuDbContext>()
                .AddTransient<IClaseTest, SimpleClaseTest>()
                .AddTransient<IClaseTest, ClaseTest>(typeof(IGeremuDbContext), typeof(string))
                .AddTransient<IClaseTest, ClaseTest>(typeof(IGeremuDbContext), typeof(string), typeof(int)));

            var param = "Direct hello";

            // Act
            var result2 = resolver.Get<IClaseTest, IGeremuDbContext, string, int>(resolver.Get<IGeremuDbContext>(), param, 2);
            var result0 = resolver.Get<IClaseTest, IGeremuDbContext, string>(resolver.Get<IGeremuDbContext>(), param);
            var simple = resolver.Get<IClaseTest>();

            // Assert
            Assert.That(result2.GiveMe().StartsWith($"{param} (") && result2.GiveMe().EndsWith("2"));
            Assert.That(result0.GiveMe().StartsWith($"{param} (") && result0.GiveMe().EndsWith("0"));
            Assert.That(simple.GiveMe().Equals("Simple class"));
        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void GetWithMultipleImplementations(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddTransient<IGeremuDbContext, GeremuDbContext>()
                .AddTransient<IClaseTest, SimpleClaseTest>()
                .AddTransient<IClaseTest, SimpleClaseTest2>(nameof(SimpleClaseTest2))
                );

            // Act
            var result = resolver.Get<IClaseTest>();
            var result2 = resolver.Get<IClaseTest>(nameof(SimpleClaseTest2));

            // Assert
            Assert.That(result, Is.Not.EqualTo(result2));
        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void GetWithMultipleResolvers(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddTransient<IGeremuDbContext, GeremuDbContext>()
                .AddTransient<IClaseTest, SimpleClaseTest>()
                .AddTransient<IClaseTest, SimpleClaseTest2>(nameof(SimpleClaseTest2))
                );

            var resolver2 = ResolverFactory.Create(new ResolveInfoCollection()
                .AddTransient<IGeremuDbContext, GeremuDbContext>()
                .AddTransient<IClaseTest, SimpleClaseTest>()
                .AddTransient<IClaseTest, SimpleClaseTest2>(nameof(SimpleClaseTest2))
                .AddTransient<IClaseTest2, SimpleClaseTest2>()
                );

            // Act
            var result = resolver.Get<IClaseTest>();
            var result2 = resolver.Get<IClaseTest>(nameof(SimpleClaseTest2));
            var result20 = resolver2.Get<IClaseTest>();
            var result202 = resolver2.Get<IClaseTest>(nameof(SimpleClaseTest2));
            var result3 = resolver.Get<IClaseTest2>();

            // Assert
            Assert.That(result, Is.Not.EqualTo(result2));
            Assert.That(result20, Is.Not.EqualTo(result202));
            Assert.That(result, Is.Not.EqualTo(result20));
            Assert.That(result2, Is.Not.EqualTo(result202));
            Assert.That(result3, Is.Not.Null);
        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void GetWithDifferentParamsSameType(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddTransient<IClaseTest, SimpleClaseTest>(nameof(SimpleClaseTest))
                .AddTransient<IClaseTest, SimpleClaseTest2>(nameof(SimpleClaseTest2))
                .AddTransient<IClaseTest, ClassUsingSimple>(typeof(IClaseTest), typeof(IClaseTest))
                );

            // Act
            var simpleClaseTest = resolver.Get<IClaseTest>(nameof(SimpleClaseTest));
            var simpleClaseTest2 = resolver.Get<IClaseTest>(nameof(SimpleClaseTest2));
            var claseTest = resolver.Get<IClaseTest, IClaseTest, IClaseTest>(simpleClaseTest, simpleClaseTest2);

            // Assert
            Assert.That(simpleClaseTest, Is.Not.EqualTo(simpleClaseTest2));
            Assert.That($"{simpleClaseTest.GiveMe()} - {simpleClaseTest2.GiveMe()}", Is.EqualTo(claseTest.GiveMe()));
        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void GetWithConstructorResolvedByParam(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddTransient<IClaseTest, SimpleClaseTest>()
                .AddTransient<IGeremuDbContext, GeremuDbContext>()
                );

            // Act
            var simpleClaseTest = resolver.Get<IClaseTest>();
            var geremuDbContext = resolver.Get<IGeremuDbContext>();

            // Assert
            Assert.That($"Hello, I'm instance {geremuDbContext.Id} with claseTest: {simpleClaseTest.GiveMe()}", Is.EqualTo(((IClaseTest)geremuDbContext).GiveMe()));
        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void GetGenerics(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddTransient(typeof(IFactoryTest<>), typeof(FactoryTest<>))
                );

            // Act
            var result = resolver.Get<IFactoryTest<SimpleClaseTest>>();

            // Assert
            Assert.That(new SimpleClaseTest().GiveMe(), Is.EqualTo(result.Create().GiveMe()));
        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void GetGenericsWithSpecificCase(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddTransient(typeof(IFactoryTest<>), typeof(FactoryTest<>))
                .AddTransient<IFactoryTest<SimpleClaseTest>, SimpleClaseTestFactoryTest>()
                );

            // Act
            var result = resolver.Get<IFactoryTest<SimpleClaseTest>>();

            // Assert
            Assert.That(new SimpleClaseTest().ItsTrue, Is.Not.EqualTo(result.Create().ItsTrue));
        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void GetWithKey(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddTransient<IClaseTest, SimpleClaseTest>()
                .AddScoped<IGeremuDbContext, GeremuDbContext>()
                .AddTransient<IClaseTest, ClaseTest1>("1")
                .AddTransient<IClaseTest, ClaseTest2>("2")
                );

            // Act
            var result = resolver.Get<IClaseTest>("1");

            // Assert
            Assert.That(result.GiveMe().StartsWith("ClaseTest1 with"));
        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void InitializeExtensionClasses(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddTransient<IClaseTest, SimpleClaseTest>()
                .AddInitializer(ExtensionsTest.Initialize)
                );
            var expectedResult = new SimpleClaseTest().GiveMe();

            // Act
            var result = "tets_".GetInfo();

            // Assert
            Assert.That(expectedResult, Is.EqualTo(result));
        }

        [TestCaseSource(typeof(ResolverCases), nameof(ResolverCases.ResolverFactoriesCases))]
        public void ResolveNotPassedParameters(ResolverFactoryCase ResolverFactory)
        {
            // Arrange
            var resolver = ResolverFactory.Create(new ResolveInfoCollection()
                .AddTransient<IClaseTest, ClaseTest>(typeof(IGeremuDbContext), typeof(string), typeof(int))
                .AddTransient<IGeremuDbContext, GeremuDbContext>()
                );
            var text = "hello";
            int num = 1;
            var context = resolver.Get<IGeremuDbContext>();
            var expected = new ClaseTest(context, text, num).GiveMe();

            // Act
            var result = resolver.Get<IClaseTest, string, int>(text, num).GiveMe();

            // Assert 
            Assert.That(expected, Is.EqualTo(result));
        }
    }
}
