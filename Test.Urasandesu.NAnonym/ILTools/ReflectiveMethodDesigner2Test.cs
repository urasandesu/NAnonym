/* 
 * File: ReflectiveMethodDesigner2Test.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2010 Akira Sugiura
 *  
 *  This software is MIT License.
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Assert = Urasandesu.NAnonym.Test.Assert;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Test;
using Test.Urasandesu.NAnonym.Etc;
using Urasandesu.NAnonym.Formulas;
using Urasandesu.NAnonym;
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.NAnonym.Mixins.System.Reflection;
using Urasandesu.NAnonym.ILTools.Impl.System.Reflection;

namespace Test.Urasandesu.NAnonym.ILTools
{
    [TestFixture]
    public class ReflectiveMethodDesigner2Test
    {
        [Test]
        public void EvalTest01_ComplexStatements()
        {
            var gen = new ReflectiveMethodDesigner2();
            var methodGen = new MockMethodGenerator();
            var returnType = new MockTypeGenerator(typeof(int).ToTypeDecl());
            methodGen.ReturnTypeProvider = () => returnType;
            gen.ILBuilder = new EmptyILBuilder(methodGen);
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
        public void EvalTest02_ReflectiveDesign()
        {
            var gen = new ReflectiveMethodDesigner2();

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
        public void EvalTest03_Performance()
        {
            var gen = new ReflectiveMethodDesigner2();
            var opcode1 = default(OpCode);
            var opcode2 = OpCodes.Add_Ovf;
            gen.Eval(() => Dsl.Allocate(opcode1).As(Dsl.ConstMember(OpCodes.Add, typeof(OpCodes))));
            for (int i = 0; i < 100; i++)
            {
                gen.Eval(() => Dsl.Allocate(opcode1).As(Dsl.ConstMember(opcode2, typeof(OpCodes))));
            }
            gen.Eval(() => Dsl.End());
            Console.WriteLine(gen.Dump());
        }

        [Test]
        public void EvalTest04_ValidReturn1_SimpleStatements()
        {
            var gen = new ReflectiveMethodDesigner2();
            var methodGen = new MockMethodGenerator();
            var returnType = new MockTypeGenerator(typeof(int).ToTypeDecl());
            methodGen.ReturnTypeProvider = () => returnType;
            gen.ILBuilder = new EmptyILBuilder(methodGen);
            gen.Eval(() => Dsl.Return(10));
            gen.Eval(() => Dsl.End());
            Console.WriteLine(gen.Dump());
        }

        [Test]
        public void EvalTest04_ValidReturn2_ComplexStatements()
        {
            var gen = new ReflectiveMethodDesigner2();
            var methodGen = new MockMethodGenerator();
            var returnType = new MockTypeGenerator(typeof(int).ToTypeDecl());
            methodGen.ReturnTypeProvider = () => returnType;
            gen.ILBuilder = new EmptyILBuilder(methodGen);
            var i = default(int);
            gen.Eval(() => Dsl.Allocate(i).As(10));
            gen.Eval(() => Dsl.If(i < 0));
            {
                gen.Eval(() => Dsl.Return(-1));
            }
            gen.Eval(() => Dsl.ElseIf(i == 0));
            {
                gen.Eval(() => Dsl.Return(0));
            }
            gen.Eval(() => Dsl.Else());
            {
                gen.Eval(() => Dsl.Return(1));
            }
            gen.Eval(() => Dsl.EndIf());
            gen.Eval(() => Dsl.End());
            Console.WriteLine(gen.Dump());
        }

        [Test]
        [ExpectedException(typeof(ReturnCheckException))]
        public void EvalTest05_InvalidReturn1_IncompatibleType_SimpleStatements()
        {
            var gen = new ReflectiveMethodDesigner2();
            var methodGen = new MockMethodGenerator();
            var returnType = new MockTypeGenerator(typeof(int).ToTypeDecl());
            methodGen.ReturnTypeProvider = () => returnType;
            gen.ILBuilder = new EmptyILBuilder(methodGen);
            gen.Eval(() => Console.WriteLine("aiueo"));
            gen.Eval(() => Dsl.End());
            Console.WriteLine(gen.Dump());
        }

        [Test]
        [ExpectedException(typeof(ReturnCheckException))]
        public void EvalTest05_InvalidReturn2_IncompatibleType_ComplexStatements()
        {
            var gen = new ReflectiveMethodDesigner2();
            var methodGen = new MockMethodGenerator();
            var returnType = new MockTypeGenerator(typeof(int).ToTypeDecl());
            methodGen.ReturnTypeProvider = () => returnType;
            gen.ILBuilder = new EmptyILBuilder(methodGen);
            var i = default(int);
            gen.Eval(() => Dsl.Allocate(i).As(10));
            gen.Eval(() => Dsl.If(i < 0));
            {
                gen.Eval(() => Dsl.Return("hoge"));
            }
            gen.Eval(() => Dsl.ElseIf(i == 0));
            {
                gen.Eval(() => Dsl.Return(0));
            }
            gen.Eval(() => Dsl.Else());
            {
                gen.Eval(() => Dsl.Return(1));
            }
            gen.Eval(() => Dsl.EndIf());
            Console.WriteLine(gen.Dump());
            gen.Eval(() => Dsl.End());
        }
    }
}
