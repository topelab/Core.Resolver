using System;
using System.Collections.Generic;
using Topelab.Core.Resolver.Interfaces;

namespace Topelab.Core.Resolver.Microsoft
{
    internal interface IResolverStorage<T> : IDictionary<T, IResolver>
    {
        event Action<IResolverStorage<T>, T> ValueAdded;
        event Action<IResolverStorage<T>, T> ValueRemoved;
        event Action<IResolverStorage<T>, T> ValueChanged;
        event Action<IResolverStorage<T>> AllValuesRemoved;
    }
}