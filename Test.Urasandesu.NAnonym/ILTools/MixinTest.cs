using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Urasandesu.NAnonym.ILTools;
using Test.Urasandesu.NAnonym.Etc;
using Urasandesu.NAnonym;
using Urasandesu.NAnonym.Mixins.System;

namespace Test.Urasandesu.NAnonym.ILTools
{
    [TestFixture]
    public class MixinTest
    {
        [Test]
        public void GetMethodTest1()
        {
            var dummy = default(Dummy);
            var dummyPrintMethodInfo = TypeSavable.GetMethodInfo<string, string>(() => dummy.Print);
            Assert.IsNotNull(typeof(ISample2).GetMethod(dummyPrintMethodInfo));
        }

        class Dummy
        {
            public string Print(string value)
            {
                throw new NotSupportedException();
            }
        }
    }
}
