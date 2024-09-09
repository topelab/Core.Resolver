using Topelab.Core.Resolver.Microsoft;

namespace Topelab.Core.Resolver.Test.Interfaces
{
    public interface IClaseTest
    {
        public static IClaseTest GetInstance() => ResolverFactory.GetResolver().Get<IClaseTest>();
        public static IClaseTest GetInstance(IGeremuDbContext context, string test) => ResolverFactory.GetResolver().Get<IClaseTest, IGeremuDbContext, string>(context, test);
        public static IClaseTest GetInstance(IGeremuDbContext context, string test, int number) => ResolverFactory.GetResolver().Get<IClaseTest, IGeremuDbContext, string, int>(context, test, number);



        string GiveMe();
    }

}
