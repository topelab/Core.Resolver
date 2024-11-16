using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topelab.Core.Resolver.Test.Entities
{
    internal class SimpleClaseTestFactoryTest : FactoryTest<SimpleClaseTest>
    {
        public override SimpleClaseTest Create()
        {
            SimpleClaseTest simpleClaseTest = base.Create();
            simpleClaseTest.ItsTrue = true;
            return simpleClaseTest;
        }
    }
}
