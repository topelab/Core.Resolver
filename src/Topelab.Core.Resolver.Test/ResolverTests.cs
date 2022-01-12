using NUnit.Framework;
using System;
using Topelab.Core.Resolver.DTO;
using Topelab.Core.Resolver.Interfaces;
using Topelab.Core.Resolver.Test.Entities;
using Topelab.Core.Resolver.Test.Interfaces;

namespace Topelab.Core.Resolver.Test
{
    internal class ResolverTests
    {
        private readonly Func<ResolveDTOList, IResolver> ResolverFactory;

        public ResolverTests(Func<ResolveDTOList, IResolver> resolverFactory)
        {
            ResolverFactory = resolverFactory;
        }

        public void GetSimple()
        {
            // Arrange
            var resolver = ResolverFactory(new ResolveDTOList()
                .Add<IClaseTest, SimpleClaseTest>()
                );

            // Act
            var result = resolver.Get<IClaseTest>();

            // Assert
            Assert.AreEqual(new SimpleClaseTest().GiveMe(), result.GiveMe());
        }

        public void GetSimpleWithNames()
        {
            // Arrange
            var resolver = ResolverFactory(new ResolveDTOList()
                .Add<IClaseTest, SimpleClaseTest>()
                .Add<IClaseTest, SimpleClaseTest2>(nameof(SimpleClaseTest2))
                );

            // Act
            var result = resolver.Get<IClaseTest>();
            var result2 = resolver.Get<IClaseTest>(nameof(SimpleClaseTest2));

            // Assert
            Assert.AreNotEqual(result, result2);
        }

        public void GetWithMultipleConstructors()
        {
            // Arrange
            var resolver = ResolverFactory(new ResolveDTOList()
                .Add<IGeremuDbContext, GeremuDbContext>()
                .Add<IClaseTest, SimpleClaseTest>()
                .Add<IClaseTest, ClaseTest>(typeof(IGeremuDbContext), typeof(string))
                .Add<IClaseTest, ClaseTest>(typeof(IGeremuDbContext), typeof(string), typeof(int)));

            string param = "Direc hello";

            // Act
            var result2 = resolver.Get<IClaseTest, IGeremuDbContext, string, int>(resolver.Get<IGeremuDbContext>(), param, 2);
            var result0 = resolver.Get<IClaseTest, IGeremuDbContext, string>(resolver.Get<IGeremuDbContext>(), param);
            var simple = resolver.Get<IClaseTest>();

            // Assert
            Assert.IsTrue(result2.GiveMe().StartsWith($"{param} (") && result2.GiveMe().EndsWith("2"));
            Assert.IsTrue(result0.GiveMe().StartsWith($"{param} (") && result0.GiveMe().EndsWith("0"));
            Assert.IsTrue(simple.GiveMe().Equals("Simple class"));
        }

        public void GetWithMultipleImplementations()
        {
            // Arrange
            var resolver = ResolverFactory(new ResolveDTOList()
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

        public void GetWithMultipleResolvers()
        {
            // Arrange
            var resolver = ResolverFactory(new ResolveDTOList()
                .Add<IGeremuDbContext, GeremuDbContext>()
                .Add<IClaseTest, SimpleClaseTest>()
                .Add<IClaseTest, SimpleClaseTest2>(nameof(SimpleClaseTest2))
                );

            var resolver2 = ResolverFactory(new ResolveDTOList()
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

        public void GetWithDifferentParamsSameType()
        {
            // Arrange
            var resolver = ResolverFactory(new ResolveDTOList()
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

    }
}
