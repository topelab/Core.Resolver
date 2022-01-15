using System;
using System.Collections.Generic;

namespace Topelab.Core.Resolver.Interfaces
{
    public interface IResolverStorage<T> : IDictionary<T, IResolver>
    {
        event Action<IResolverStorage<T>, T> ValueAdded;
        event Action<IResolverStorage<T>, T> ValueRemoved;
        event Action<IResolverStorage<T>, T> ValueChanged;
        event Action<IResolverStorage<T>> AllValuesRemoved;
    }
}