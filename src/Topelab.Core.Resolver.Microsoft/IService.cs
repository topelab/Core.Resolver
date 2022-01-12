using System;

namespace Topelab.Core.Resolver.Microsoft
{
    public interface IService<I>
    {
        // Returns mapped type for this I
        Type Type();
    }

}
