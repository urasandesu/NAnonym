using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Assert = Urasandesu.NAnonym.Test.Assert;
using Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Linq;
//using Mono.Cecil;
using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym;
using Test.Urasandesu.NAnonym.Etc;
using SRE = System.Reflection.Emit;

namespace Test.Urasandesu.NAnonym.DI
{
    [TestFixture]
    public class DependencyUtilTest
    {
        [Test]
        public void Hoge()
        {
            foreach (var value in typeof(SRE::OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static).Select(fieldInfo => string.Format("{0}", fieldInfo.GetValue(null))).OrderBy(_value => _value))
            {
                Console.WriteLine(value);
            }
        }

        class A<T>
        {
            public void Method1<S>(T t, S s)
            {
            }

            public void Method2(T t)
            {
            }

            A<T>[] field1 = null;
        }
    }
}
