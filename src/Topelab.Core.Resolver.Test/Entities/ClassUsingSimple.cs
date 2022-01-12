using Topelab.Core.Resolver.Test.Interfaces;

namespace Topelab.Core.Resolver.Test.Entities
{
    public class ClassUsingSimple : IClaseTest
    {
        private readonly string name;

        public ClassUsingSimple(IClaseTest clase1, IClaseTest clase2)
        {
            name = $"{clase1.GiveMe()} - {clase2.GiveMe()}";
        }

        public string GiveMe()
        {
            return name;
        }

    }
}
