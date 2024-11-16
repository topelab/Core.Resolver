using Topelab.Core.Resolver.Test.Interfaces;

namespace Topelab.Core.Resolver.Test.Entities
{
    internal class FactoryTest<T> : IFactoryTest<T> where T : class, new()
    {
        public virtual T Create()
        {
            return new T();
        }
    }
}
