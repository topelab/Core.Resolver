using System;
using Topelab.Core.Resolver.Test.Interfaces;

namespace Topelab.Core.Resolver.Test.Entities
{
    public class GeremuDbContext : IGeremuDbContext, IClaseTest
    {
        private static int id;
        private readonly IClaseTest claseTest;
        public int Id { get; }

        public GeremuDbContext()
        {

        }

        public GeremuDbContext(IClaseTest claseTest)
        {
            Id = ++id;
            this.claseTest = claseTest ?? throw new ArgumentNullException(nameof(claseTest));
        }

        public string GiveMe()
        {
            return $"Hello, I'm instance {Id} with claseTest: {claseTest?.GiveMe()}";
        }
    }
}
