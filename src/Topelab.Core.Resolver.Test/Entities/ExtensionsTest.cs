using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topelab.Core.Resolver.Interfaces;
using Topelab.Core.Resolver.Test.Interfaces;

namespace Topelab.Core.Resolver.Test.Entities
{
    internal static class ExtensionsTest
    {
        private static IClaseTest claseTest;
        public static void Initialize(IResolver resolver)
        {
            claseTest = resolver.Get<IClaseTest>();
        }

        public static string GetInfo(this string testString)
        {
            return claseTest.GiveMe();
        }
    }
}
