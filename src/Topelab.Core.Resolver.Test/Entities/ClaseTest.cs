using System;
using Topelab.Core.Resolver.Test.Interfaces;

namespace Topelab.Core.Resolver.Test.Entities
{
    public class ClaseTest : IClaseTest
    {
        private readonly int number;
        private readonly string test;
        private readonly IGeremuDbContext context;

        public ClaseTest(IGeremuDbContext context, string test) : this(context, test, 0)
        {
        }

        public ClaseTest(IGeremuDbContext context, string test, int number)
        {
            this.number = number;
            this.test = test;
            this.context = context;
        }

        public ClaseTest(int number, DateTime dateTime)
        {
            this.number = number;
            this.test = dateTime.ToString("G");
        }

        public string GiveMe()
        {
            return $"{test} ({context?.Id}) with number {number}";
        }
    }

}
