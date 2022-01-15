using Topelab.Core.Resolver.Test.Interfaces;

namespace Topelab.Core.Resolver.Test.Entities
{
    public class SimpleClaseTest2 : IClaseTest, IClaseTest2
    {
        public string GiveMe()
        {
            return "Simple class 2";
        }
    }

}
