using System;
using Topelab.Core.Resolver.Test.Interfaces;

namespace Topelab.Core.Resolver.Test.Entities
{
    public class ClaseTest1 : IClaseTest
    {
        private readonly IGeremuDbContext context;

        public ClaseTest1(IGeremuDbContext context)
        {
            this.context = context;
        }

        public string GiveMe()
        {
            return $"ClaseTest1 with context ({context?.Id})";
        }
    }

}
