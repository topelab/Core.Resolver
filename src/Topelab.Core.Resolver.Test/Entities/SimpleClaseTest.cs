using Topelab.Core.Resolver.Test.Interfaces;

namespace Topelab.Core.Resolver.Test.Entities
{
    public class SimpleClaseTest : IClaseTest
    {
        public bool ItsTrue { get; set; } = false;

        public string GiveMe()
        {
            return "Simple class";
        }
    }

}
