using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Runtime.CompilerServices;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Interfaces;
using Topelab.Core.Resolver.Microsoft;
using Topelab.Core.Resolver.Test.Entities;
using Topelab.Core.Resolver.Test.Interfaces;

namespace Topelab.Core.Resolver.Test
{
    [TestFixture]
    public class SpecificMicrosoftResolverTests
    {
        [Test]
        public void ResolverFactory_CreateResolver_ReturnsResolverThatResolvesInstances()
        {
            // Arrange
            var number = 99;
            var dateTime = DateTime.Now;
            var resolver = CreateNewResolver(new ResolveInfoCollection()
                .AddFactory(s => (IClaseTest)Activator.CreateInstance(typeof(ClaseTest), number, dateTime))
                .AddFactory("dos", s => (IClaseTest)Activator.CreateInstance(typeof(ClaseTest), number * 2, dateTime))
                );
            ClaseTest goodResult = new(number, dateTime);
            ClaseTest goodResultDos = new(number * 2, dateTime);

            // Act
            var result = resolver.Get<IClaseTest>();
            var resultDos = resolver.Get<IClaseTest>("dos");

            // Assert
            Assert.That(goodResult.GiveMe(), Is.EqualTo(result.GiveMe()));
            Assert.That(goodResultDos.GiveMe(), Is.EqualTo(resultDos.GiveMe()));
        }

        [Test]
        public void AddResolver_InitializingServiceCollection_ResolverCollectionOK()
        {
            // Arrange
            var resolveInfoCollection = new ResolveInfoCollection()
                .AddScoped<IGeremuDbContext, GeremuDbContext>()
                .AddScoped<IClaseTest, SimpleClaseTest>()
                .AddScoped<IClaseTest, SimpleClaseTest2>("NOT");

            // Act
            var hostBuilder = Host.CreateDefaultBuilder().ConfigureServices(
                collection => collection
                                    .AddScoped<IClaseTest2, SimpleClaseTest2>()
                                    .AddResolver(resolveInfoCollection)
                                    );
            var provider = hostBuilder.Build().Services;
            var resolver = provider.GetService<IResolver>();

            // Assert
            Assert.That(resolver, Is.Not.Null);
            Assert.That(resolver.Get<IClaseTest>(), Is.EqualTo(provider.GetService<IClaseTest>()));
            Assert.That(resolver.Get<IGeremuDbContext>().Id, Is.EqualTo(provider.GetService<IGeremuDbContext>().Id));
            Assert.That(resolver.Get<IClaseTest2>(), Is.Not.Null);
        }

        [Test]
        public void Resolve_WithCurrentResolver_ResolvesSameInstance()
        {
            // Arrange
            var resolver = CreateNewResolver(new ResolveInfoCollection()
                .AddTransient<IClaseTest, SimpleClaseTest>()
                .AddTransient<IClaseTest2, SimpleClaseTest2>()
                .AddTransient<IClaseTest, SimpleClaseTest2>(nameof(SimpleClaseTest2))
                );

            var resolver2 = CreateNewResolver(new ResolveInfoCollection()
                .AddTransient<IClaseTest, SimpleClaseTest>()
                .AddTransient<IClaseTest2, SimpleClaseTest2>()
                .AddTransient<IClaseTest, SimpleClaseTest2>(nameof(SimpleClaseTest2))
                );

            // Act
            var result1_1 = resolver.Get<IClaseTest>();
            var result1_2 = resolver2.Get<IClaseTest>();
            var result2_1 = resolver.Get<IClaseTest2>();
            var result2_2 = resolver2.Get<IClaseTest2>();
            var result3_1 = resolver.Get<IClaseTest>(nameof(SimpleClaseTest2));
            var result3_2 = resolver2.Get<IClaseTest>(nameof(SimpleClaseTest2));

            // Assert
            Assert.That(result1_1, Is.Not.EqualTo(result1_2));
            Assert.That(result2_1, Is.Not.EqualTo(result2_2));
            Assert.That(result3_1, Is.Not.EqualTo(result3_2));

        }

        [Test]
        public void ResolveFactoryCreate_Create2Resolvers_ResolversAreDifferent()
        {
            // Arrange
            var collection = new ResolveInfoCollection()
                .AddTransient<IClaseTest, SimpleClaseTest>()
                .AddTransient<IClaseTest2, SimpleClaseTest2>()
                .AddTransient<IClaseTest, SimpleClaseTest2>(nameof(SimpleClaseTest2));

            // Act
            var resolver = CreateNewResolver(collection);
            var resolver2 = CreateNewResolver(collection);

            // Assert
            Assert.That(resolver, Is.Not.EqualTo(resolver2));

        }

        private IResolver CreateNewResolver(ResolveInfoCollection collection, [CallerMemberName] string scope = null) => ResolverFactory.Create(collection, new Scope(scope));

    }

}
