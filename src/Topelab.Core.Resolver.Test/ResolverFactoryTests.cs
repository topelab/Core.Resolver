using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using Topelab.Core.Resolver.Entities;
using Topelab.Core.Resolver.Interfaces;
using Topelab.Core.Resolver.Microsoft;
using Topelab.Core.Resolver.Test.Entities;
using Topelab.Core.Resolver.Test.Interfaces;

namespace Topelab.Core.Resolver.Test
{
    [TestFixture]
    public class ResolverFactoryTests
    {
        [Test]
        public void AddResolver_IntialitzingServiceCollection_ResolverCollectionOK()
        {
            // Arrange
            var resolveInfoCollection = new ResolveInfoCollection()
                .Add<IGeremuDbContext, GeremuDbContext>()
                .Add<IClaseTest, SimpleClaseTest>();

            // Act
            var hostBuilder = Host.CreateDefaultBuilder().ConfigureServices(
                collection => collection
                                    .AddScoped<IClaseTest2, SimpleClaseTest2>()
                                    .AddResolver(resolveInfoCollection)
                                    );
            var provider = hostBuilder.Build().Services;
            var resolver = provider.GetService<IResolver>();

            // Assert
            Assert.NotNull(resolver);
            Assert.AreSame(resolver.Get<IClaseTest>(), provider.GetService<IClaseTest>());
            Assert.AreEqual(resolver.Get<IGeremuDbContext>().Id, provider.GetService<IGeremuDbContext>().Id);
            Assert.NotNull(resolver.Get<IClaseTest2>());
        }
    }
}
