using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Urasandesu.NAnonym;
using Urasandesu.NAnonym.Linq;

namespace Test.Urasandesu.NAnonym
{
    [TestFixture]
    public class RequiredTest
    {
        [Test]
        public void AllTest()
        {
            try
            {
                int a = 0;
                Required.NotDefault(a, () => a);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
                Assert.AreEqual("a", e.ParamName);
            }

            try
            {
                object o = new object();
                Required.Default(o, () => o);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
                Assert.AreEqual("o", e.ParamName);
            }

            try
            {
                object value = null;
                object destination = null;
                Required.JustOnce(value, ref destination, () => value);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
                Assert.AreEqual("value", e.ParamName);
            }

            try
            {
                object value = new object();
                object destination = new object();
                Required.JustOnce(value, ref destination, () => value);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
                Assert.AreEqual("value", e.ParamName);
            }

            {
                object value = new object();
                object destination = null;
                Assert.AreSame(value, Required.JustOnce(value, ref destination, () => value));
            }

            {
                int value = 10;
                int[] array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
                Assert.AreEqual(10, Required.Identical(value, array, _ => _.Count(), () => value));
            }

            try
            {
                int value = 10;
                int[] array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
                Assert.AreEqual(10, Required.Identical(
                    value, array, _ => _.Aggregate((accumulate, source) => accumulate + source), () => value));
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
                Assert.AreEqual("value", e.ParamName);
            }
        }
    }
}
