using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topelab.Core.Resolver.Microsoft
{
    public interface IResolvable<T> where T : class
    {
        public static T GetInstance() => ResolverFactory.GetResolver().Get<T>();
        public static T GetInstance<T1>(T1 arg1) => ResolverFactory.GetResolver().Get<T, T1>(arg1);
    }
}
