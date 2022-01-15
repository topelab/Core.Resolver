using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using System;
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
                .Add<IGeremuDbContext, GeremuDbContext>(Enums.ResolveTypeEnum.Singleton)
                .Add<IClaseTest, SimpleClaseTest>();

            // var resolver = ResolverFactory.Create(resolveInfoCollection);

            // Act
            var hostBuilder = Host.CreateDefaultBuilder().ConfigureServices(
                collection => collection
                                    .AddScoped<IClaseTest2, SimpleClaseTest2>()
                                    .AddResolver(resolveInfoCollection)
                                    .AddScoped<IResolver>(s => new Microsoft.Resolver(s, s.GetService<IServiceFactory>(), null,null)) 
                                    );
            var provider = hostBuilder.Build().Services;

            // Assert
            //Assert.NotNull(resolver);
        }
    }
}
