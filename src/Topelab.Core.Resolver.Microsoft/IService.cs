using System;

namespace Topelab.Core.Resolver.Microsoft
{
    /// <summary>
    /// Interface that represent any interface service of type <typeparamref name="I"/>
    /// </summary>
    /// <typeparam name="I">Type for an interface for a service</typeparam>
    public interface IService<I>
    {
        /// <summary>
        /// Returns mapped type for this I
        /// </summary>
        Type Type();
    }
}
