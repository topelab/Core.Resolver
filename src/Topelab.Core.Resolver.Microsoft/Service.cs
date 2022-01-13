using System;

namespace Topelab.Core.Resolver.Microsoft
{
    /// <summary>
    /// Service to translate from interface <typeparamref name="I"/> to type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="I">Interface</typeparam>
    /// <typeparam name="T">type</typeparam>
    public class Service<I, T> : IService<I>
    {
        /// <summary>
        /// Types this instance.
        /// </summary>
        public Type Type()
        {
            return typeof(T);
        }
    }

}
