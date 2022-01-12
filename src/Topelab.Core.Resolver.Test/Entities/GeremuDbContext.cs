using Topelab.Core.Resolver.Test.Interfaces;

namespace Topelab.Core.Resolver.Test.Entities
{
    public class GeremuDbContext : IGeremuDbContext
    {
        private static int id;
        public int Id { get; }

        public GeremuDbContext()
        {
            Id = ++id;
        }
    }

}
