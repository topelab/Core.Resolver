using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;
using System;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Interfaces;
using Topelab.Core.Resolver.Microsoft;
using Topelab.Core.Resolver.Test.Entities;
using Topelab.Core.Resolver.Test.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Topelab.Core.Resolver.Test
{
    [TestFixture]
    public class SpecificMicrosoftResolverTests
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
            Assert.That(goodResult.GiveMe(), Is.EqualTo(result.GiveMe()));
            Assert.That(goodResultDos.GiveMe(), Is.EqualTo(resultDos.GiveMe()));
        }

        [Test]
        public void AddResolver_IntialitzingServiceCollection_ResolverCollectionOK()
        {
            // Arrange
            var resolveInfoCollection = new ResolveInfoCollection()
                .Add<IGeremuDbContext, GeremuDbContext>(Enums.ResolveLifeCycleEnum.Scoped)
                .Add<IClaseTest, SimpleClaseTest>(Enums.ResolveLifeCycleEnum.Scoped);

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
        public void GetInstance_FromImplentationOfIResolvable_ReturnsInstance()
        {
            // Arrange
            var number = 99;
            var dateTime = DateTime.Now;
            var resolveInfoCollection = new ResolveInfoCollection()
                .AddTransient<IClaseTest, SimpleClaseTest>();
            ClaseTest goodResult = new(number, dateTime);

            // Act
            var result = IClaseTest.GetInstance();

            // Assert
            Assert.That(goodResult.GiveMe(), Is.EqualTo(result.GiveMe()));
        }

    }

}
