using System;
using Topelab.Core.Resolver.Test.Interfaces;

namespace Topelab.Core.Resolver.Test.Entities
{
    public class ClaseTest2 : IClaseTest
    {
        private readonly IGeremuDbContext context;

        public ClaseTest2(IGeremuDbContext context)
        {
            this.context = context;
        }

        public string GiveMe()
        {
            return $"ClaseTest2 with context ({context?.Id})";
        }
    }

}
