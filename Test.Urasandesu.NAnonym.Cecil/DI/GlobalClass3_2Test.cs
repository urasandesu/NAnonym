using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Urasandesu.NAnonym.Cecil.DI;
using Test.Urasandesu.NAnonym.Etc;

namespace Test.Urasandesu.NAnonym.Cecil.DI
{
    [TestFixture]
    public class GlobalClass3_2Test
    {
        static GlobalClass3_2Test()
        {
            DependencyUtil.RollbackGlobal();

            DependencyUtil.RegisterGlobal<GlobalClass3_2>();
            DependencyUtil.LoadGlobal();
        }

        [Test]
        public void AddTest01()
        {
            var class3 = new Class3();

            Assert.AreEqual(2, class3.Add(1, 1));
            Assert.AreEqual(4, class3.Add(1, 1));
            Assert.AreEqual(6, class3.Add(1, 1));
        }
    }
}
