using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Assert = Urasandesu.NAnonym.Test.Assert;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Test;
using Test.Urasandesu.NAnonym.Etc;

namespace Test.Urasandesu.NAnonym.ILTools
{
    [TestFixture]
    public class ReflectiveMethodDesigner2Test
    {
        [Test]
        public void EvalTest01()
        {
            var gen = new ReflectiveMethodDesigner2(null);
            var value = default(int);
            var objValue = default(object);
            var value2 = default(int?);
            gen.Eval(() => Dsl.Allocate(value).As(10));
            gen.Eval(() => Dsl.If(value != 20 && value != 30 && value != 40 && value != 50));
            {
                gen.Eval(() => Dsl.Allocate(objValue).As(value));
                gen.Eval(() => Dsl.If(Dsl.Allocate(value2).As(objValue as int?) != null));
                {
                    gen.Eval(() => Dsl.Return(value + value * value + (int)value2));
                }
                gen.Eval(() => Dsl.Else());
                {
                    gen.Eval(() => Dsl.Return(value + value * value * value));
                }
                gen.Eval(() => Dsl.EndIf());
            }
            gen.Eval(() => Dsl.ElseIf(value == 20));
            {
                gen.Eval(() => Dsl.Return(value));
            }
            gen.Eval(() => Dsl.ElseIf(value == 40));
            {
                gen.Eval(() => Dsl.Return(value ^ value ^ value));
            }
            gen.Eval(() => Dsl.Else());
            {
                gen.Eval(() => Dsl.Return(value == 30 ? value + value : value * value));
            }
            gen.Eval(() => Dsl.EndIf());
            gen.Eval(() => Dsl.End());
            Console.WriteLine(gen.Dump());
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        [Test]
        public void EvalTest02()
        {
            var gen = new ReflectiveMethodDesigner2(null);

            var writeLog = typeof(TestHelper).GetMethod("WriteLog", new Type[] { typeof(string), typeof(object[]) });
            gen.Eval(() => writeLog.Invoke(null, new object[] { "testtest", new object[] { } }));
            var p1 = default(PropertyTestClass1);
            var p1Ci = typeof(PropertyTestClass1).GetConstructor(Type.EmptyTypes);
            gen.Eval(() => Dsl.Allocate(p1).As((PropertyTestClass1)p1Ci.Invoke(null)));
            gen.Eval(() => writeLog.Invoke(null, new object[] { "{0}", new object[] { p1 } }));
            var p1ValueProperty = typeof(PropertyTestClass1).GetProperty("ValueProperty");
            gen.Eval(() => p1ValueProperty.SetValue(p1, 10, null));
            gen.Eval(() => writeLog.Invoke(null, new object[] { "ValueProperty: {0}", new object[] { (int)p1ValueProperty.GetValue(p1, null) } }));
            var p1ObjectProperty = typeof(PropertyTestClass1).GetProperty("ObjectProperty");
            gen.Eval(() => p1ObjectProperty.SetValue(p1, "a", null));
            gen.Eval(() => p1ObjectProperty.SetValue(p1, p1ObjectProperty.GetValue(p1, null), null));
            gen.Eval(() => writeLog.Invoke(null, new object[] { "ObjectProperty: {0}", new object[] { p1ObjectProperty.GetValue(p1, null) } }));
            var f2 = default(FieldTestClass2);
            gen.Eval(() => Dsl.Allocate(f2).As(new FieldTestClass2()));
            var f2ValueField = typeof(FieldTestClass2).GetField("ValueField");
            gen.Eval(() => f2ValueField.SetValue(f2, 30));
            gen.Eval(() => f2ValueField.SetValue(f2, f2ValueField.GetValue(f2)));
            gen.Eval(() => TestHelper.WriteLog("ValueField: {0}", f2ValueField.GetValue(f2)));
            gen.Eval(() => writeLog.Invoke(null, new object[] { "ValueField: {0}", new object[] { f2ValueField.GetValue(f2) } }));
            var p2 = default(PropertyTestClass2);
            var p2Ci = typeof(PropertyTestClass2).GetConstructor(new Type[] { typeof(int), typeof(string) });
            gen.Eval(() => Dsl.Allocate(p2).As((PropertyTestClass2)p2Ci.Invoke(new object[] { p1ValueProperty.GetValue(p1, null), p1ObjectProperty.GetValue(p1, null) })));
            gen.Eval(() => writeLog.Invoke(null, new object[] { "({0}, {1})", new object[] { p2.ValueProperty, p2.ObjectProperty } }));
            var getValue = typeof(TestHelper).GetMethod("GetValue", new Type[] { typeof(int) });
            var value = default(int);
            gen.Eval(() => Dsl.Allocate(value).As((int)getValue.Invoke(null, new object[] { f2ValueField.GetValue(f2) })));
            gen.Eval(() => Dsl.End());
            Console.WriteLine(gen.Dump());
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        [Test]
        public void EvalTest03()
        {
            var gen = new ReflectiveMethodDesigner2(null);
            var opcode1 = default(OpCode);
            var opcode2 = OpCodes.Add_Ovf;
            gen.Eval(() => Dsl.Allocate(opcode1).As(Dsl.ConstMember(OpCodes.Add, typeof(OpCodes))));
            for (int i = 0; i < 100; i++)
            {
                gen.Eval(() => Dsl.Allocate(opcode1).As(Dsl.ConstMember(opcode2, typeof(OpCodes))));
            }
            Console.WriteLine(gen.Dump());
        }
    }
}
