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
            var methodGen = new MockMethodBaseGenerator();
            var returnType = new MockTypeGenerator(typeof(int).ToTypeDecl());
            gen.ILBuilder = new EmptyILBuilder(methodGen, returnType);
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
            Console.WriteLine(gen.DumpEntryPoint());
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
            //Console.WriteLine(gen.Dump());
            #region Assertion
            Assert.AreEqual("{\"NodeType\": \"End\", \"TypeDeclaration\": \"System.Void\", \"EntryBlock\": {\"NodeType\": \"Block" +
"\", \"TypeDeclaration\": \"System.Void\", \"Locals\": [{\"NodeType\": \"Local\", \"TypeDeclaration\"" +
": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", \"Local\": null}, {\"No" +
"deType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"LocalNa" +
"me\": \"f2\", \"Local\": null}, {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAn" +
"onym.Etc.PropertyTestClass2\", \"LocalName\": \"p2\", \"Local\": null}, {\"NodeType\": \"Local\", \"" +
"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}], \"Formulas\": [{\"" +
"NodeType\": \"ReflectiveCall\", \"TypeDeclaration\": \"System.Void\", \"Instance\": null, \"Method\"" +
": \"Void WriteLog(System.String, System.Object[])\", \"Arguments\": [{\"NodeType\": \"NewArrayInit\"" +
", \"TypeDeclaration\": \"System.Object[]\", \"Formulas\": [{\"NodeType\": \"Constant\", \"TypeDeclar" +
"ation\": \"System.String\", \"ConstantValue\": \"testtest\"}]}]}, {\"NodeType\": \"Assign\", \"TypeD" +
"eclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Ur" +
"asandesu.NAnonym.Etc.PropertyTestClass1\", \"VariableName\": \"p1\", \"VariableIndex\": 0, \"Resolve" +
"d\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1" +
"\", \"LocalName\": \"p1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Reflect" +
"iveNew\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"Constructor\": " +
"\"Void .ctor()\", \"Arguments\": []}}, {\"NodeType\": \"ReflectiveCall\", \"TypeDeclaration\": \"Sys" +
"tem.Void\", \"Instance\": null, \"Method\": \"Void WriteLog(System.String, System.Object[])\", \"Arg" +
"uments\": [{\"NodeType\": \"NewArrayInit\", \"TypeDeclaration\": \"System.Object[]\", \"Formulas\": " +
"[{\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.String\", \"ConstantValue\": \"{0}\"}, {" +
"\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\"," +
" \"VariableName\": \"p1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDecla" +
"ration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", \"Local\": null" +
"}}]}]}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"" +
"ReflectiveProperty\", \"TypeDeclaration\": \"System.Int32\", \"Instance\": {\"NodeType\": \"Variable" +
"\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"VariableName\": \"p1\"" +
", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasand" +
"esu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", \"Local\": null}}, \"Member\": \"Int32 " +
"ValueProperty\", \"Member\": \"Int32 ValueProperty\"}, \"Method\": \"=\", \"Right\": {\"NodeType\": " +
"\"Convert\", \"TypeDeclaration\": \"System.Object\", \"Method\": null, \"Operand\": {\"NodeType\": \"" +
"Constant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 10}}}, {\"NodeType\": \"Refle" +
"ctiveCall\", \"TypeDeclaration\": \"System.Void\", \"Instance\": null, \"Method\": \"Void WriteLog(S" +
"ystem.String, System.Object[])\", \"Arguments\": [{\"NodeType\": \"NewArrayInit\", \"TypeDeclaration" +
"\": \"System.Object[]\", \"Formulas\": [{\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.S" +
"tring\", \"ConstantValue\": \"ValueProperty: {0}\"}, {\"NodeType\": \"Convert\", \"TypeDeclaration\"" +
": \"System.Object\", \"Method\": null, \"Operand\": {\"NodeType\": \"ReflectiveProperty\", \"TypeDec" +
"laration\": \"System.Int32\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test" +
".Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"VariableName\": \"p1\", \"VariableIndex\": 0, \"Reso" +
"lved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestCla" +
"ss1\", \"LocalName\": \"p1\", \"Local\": null}}, \"Member\": \"Int32 ValueProperty\", \"Member\": \"" +
"Int32 ValueProperty\"}}]}]}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left" +
"\": {\"NodeType\": \"ReflectiveProperty\", \"TypeDeclaration\": \"System.String\", \"Instance\": {\"" +
"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"" +
"VariableName\": \"p1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclara" +
"tion\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", \"Local\": null}}" +
", \"Member\": \"System.String ObjectProperty\", \"Member\": \"System.String ObjectProperty\"}, \"Met" +
"hod\": \"=\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.String\", \"Cons" +
"tantValue\": \"a\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"" +
"NodeType\": \"ReflectiveProperty\", \"TypeDeclaration\": \"System.String\", \"Instance\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"Variab" +
"leName\": \"p1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\":" +
" \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", \"Local\": null}}, \"Mem" +
"ber\": \"System.String ObjectProperty\", \"Member\": \"System.String ObjectProperty\"}, \"Method\": " +
"\"=\", \"Right\": {\"NodeType\": \"ReflectiveProperty\", \"TypeDeclaration\": \"System.String\", \"I" +
"nstance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyT" +
"estClass1\", \"VariableName\": \"p1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\"," +
" \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", \"L" +
"ocal\": null}}, \"Member\": \"System.String ObjectProperty\", \"Member\": \"System.String ObjectProp" +
"erty\"}}, {\"NodeType\": \"ReflectiveCall\", \"TypeDeclaration\": \"System.Void\", \"Instance\": nul" +
"l, \"Method\": \"Void WriteLog(System.String, System.Object[])\", \"Arguments\": [{\"NodeType\": \"N" +
"ewArrayInit\", \"TypeDeclaration\": \"System.Object[]\", \"Formulas\": [{\"NodeType\": \"Constant\"," +
" \"TypeDeclaration\": \"System.String\", \"ConstantValue\": \"ObjectProperty: {0}\"}, {\"NodeType\":" +
" \"ReflectiveProperty\", \"TypeDeclaration\": \"System.String\", \"Instance\": {\"NodeType\": \"Vari" +
"able\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"VariableName\": \"" +
"p1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Ura" +
"sandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", \"Local\": null}}, \"Member\": \"Sy" +
"stem.String ObjectProperty\", \"Member\": \"System.String ObjectProperty\"}]}]}, {\"NodeType\": \"As" +
"sign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclarati" +
"on\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"VariableName\": \"f2\", \"VariableIndex\": " +
"0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldT" +
"estClass2\", \"LocalName\": \"f2\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": " +
"\"New\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"Constructor\": \"Vo" +
"id .ctor()\", \"Arguments\": []}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Left\": {\"NodeType\": \"ReflectiveField\", \"TypeDeclaration\": \"System.Int32\", \"Instance\": {" +
"\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"" +
"VariableName\": \"f2\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclarat" +
"ion\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"LocalName\": \"f2\", \"Local\": null}}, \"" +
"Member\": \"Int32 ValueField\", \"Member\": \"Int32 ValueField\"}, \"Method\": \"=\", \"Right\": {\"" +
"NodeType\": \"Convert\", \"TypeDeclaration\": \"System.Object\", \"Method\": null, \"Operand\": {\"N" +
"odeType\": \"Constant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 30}}}, {\"NodeTyp" +
"e\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"ReflectiveField\"," +
" \"TypeDeclaration\": \"System.Int32\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration" +
"\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"VariableName\": \"f2\", \"VariableIndex\": 0," +
" \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTes" +
"tClass2\", \"LocalName\": \"f2\", \"Local\": null}}, \"Member\": \"Int32 ValueField\", \"Member\": \"" +
"Int32 ValueField\"}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"ReflectiveField\", \"TypeDeclar" +
"ation\": \"System.Int32\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Ur" +
"asandesu.NAnonym.Etc.FieldTestClass2\", \"VariableName\": \"f2\", \"VariableIndex\": 0, \"Resolved\"" +
": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"" +
"LocalName\": \"f2\", \"Local\": null}}, \"Member\": \"Int32 ValueField\", \"Member\": \"Int32 ValueF" +
"ield\"}}, {\"NodeType\": \"Call\", \"TypeDeclaration\": \"System.Void\", \"Instance\": null, \"Metho" +
"d\": \"Void WriteLog(System.String, System.Object[])\", \"Arguments\": [{\"NodeType\": \"NewArrayIni" +
"t\", \"TypeDeclaration\": \"System.Object[]\", \"Formulas\": [{\"NodeType\": \"Constant\", \"TypeDec" +
"laration\": \"System.String\", \"ConstantValue\": \"ValueField: {0}\"}, {\"NodeType\": \"Convert\", " +
"\"TypeDeclaration\": \"System.Object\", \"Method\": null, \"Operand\": {\"NodeType\": \"ReflectiveFi" +
"eld\", \"TypeDeclaration\": \"System.Int32\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDecla" +
"ration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"VariableName\": \"f2\", \"VariableIndex" +
"\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.Fi" +
"eldTestClass2\", \"LocalName\": \"f2\", \"Local\": null}}, \"Member\": \"Int32 ValueField\", \"Membe" +
"r\": \"Int32 ValueField\"}}]}]}, {\"NodeType\": \"ReflectiveCall\", \"TypeDeclaration\": \"System.Vo" +
"id\", \"Instance\": null, \"Method\": \"Void WriteLog(System.String, System.Object[])\", \"Arguments" +
"\": [{\"NodeType\": \"NewArrayInit\", \"TypeDeclaration\": \"System.Object[]\", \"Formulas\": [{\"No" +
"deType\": \"Constant\", \"TypeDeclaration\": \"System.String\", \"ConstantValue\": \"ValueField: {0}" +
"\"}, {\"NodeType\": \"Convert\", \"TypeDeclaration\": \"System.Object\", \"Method\": null, \"Operand" +
"\": {\"NodeType\": \"ReflectiveField\", \"TypeDeclaration\": \"System.Int32\", \"Instance\": {\"Node" +
"Type\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"Variab" +
"leName\": \"f2\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\":" +
" \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"LocalName\": \"f2\", \"Local\": null}}, \"Member" +
"\": \"Int32 ValueField\", \"Member\": \"Int32 ValueField\"}}]}]}, {\"NodeType\": \"Assign\", \"TypeD" +
"eclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Ur" +
"asandesu.NAnonym.Etc.PropertyTestClass2\", \"VariableName\": \"p2\", \"VariableIndex\": 0, \"Resolve" +
"d\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass2" +
"\", \"LocalName\": \"p2\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Reflect" +
"iveNew\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass2\", \"Constructor\": " +
"\"Void .ctor(Int32, System.String)\", \"Arguments\": [{\"NodeType\": \"ReflectiveProperty\", \"TypeD" +
"eclaration\": \"System.Int32\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Te" +
"st.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"VariableName\": \"p1\", \"VariableIndex\": 0, \"Re" +
"solved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestC" +
"lass1\", \"LocalName\": \"p1\", \"Local\": null}}, \"Member\": \"Int32 ValueProperty\", \"Member\": " +
"\"Int32 ValueProperty\"}, {\"NodeType\": \"ReflectiveProperty\", \"TypeDeclaration\": \"System.Strin" +
"g\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.P" +
"ropertyTestClass1\", \"VariableName\": \"p1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"" +
"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p" +
"1\", \"Local\": null}}, \"Member\": \"System.String ObjectProperty\", \"Member\": \"System.String Ob" +
"jectProperty\"}]}}, {\"NodeType\": \"ReflectiveCall\", \"TypeDeclaration\": \"System.Void\", \"Insta" +
"nce\": null, \"Method\": \"Void WriteLog(System.String, System.Object[])\", \"Arguments\": [{\"NodeT" +
"ype\": \"NewArrayInit\", \"TypeDeclaration\": \"System.Object[]\", \"Formulas\": [{\"NodeType\": \"C" +
"onstant\", \"TypeDeclaration\": \"System.String\", \"ConstantValue\": \"({0}, {1})\"}, {\"NodeType\"" +
": \"Convert\", \"TypeDeclaration\": \"System.Object\", \"Method\": null, \"Operand\": {\"NodeType\":" +
" \"Property\", \"TypeDeclaration\": \"System.Int32\", \"Instance\": {\"NodeType\": \"Variable\", \"T" +
"ypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass2\", \"VariableName\": \"p2\", \"Va" +
"riableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAn" +
"onym.Etc.PropertyTestClass2\", \"LocalName\": \"p2\", \"Local\": null}}, \"Member\": \"Int32 ValuePr" +
"operty\", \"Member\": \"Int32 ValueProperty\"}}, {\"NodeType\": \"Property\", \"TypeDeclaration\": \"" +
"System.String\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu." +
"NAnonym.Etc.PropertyTestClass2\", \"VariableName\": \"p2\", \"VariableIndex\": 0, \"Resolved\": {\"N" +
"odeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass2\", \"Loc" +
"alName\": \"p2\", \"Local\": null}}, \"Member\": \"System.String ObjectProperty\", \"Member\": \"Sys" +
"tem.String ObjectProperty\"}]}]}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"" +
"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"val" +
"ue\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.In" +
"t32\", \"LocalName\": \"value\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"R" +
"eflectiveCall\", \"TypeDeclaration\": \"System.Int32\", \"Instance\": null, \"Method\": \"Int32 GetV" +
"alue(Int32)\", \"Arguments\": [{\"NodeType\": \"ReflectiveField\", \"TypeDeclaration\": \"System.Int" +
"32\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc." +
"FieldTestClass2\", \"VariableName\": \"f2\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Lo" +
"cal\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"LocalName\": \"f2\", " +
"\"Local\": null}}, \"Member\": \"Int32 ValueField\", \"Member\": \"Int32 ValueField\"}]}}]}}", gen.DumpEntryPoint());
            #endregion
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
            //Console.WriteLine(gen.Dump());
            #region Assertion
            Assert.AreEqual("{\"NodeType\": \"End\", \"TypeDeclaration\": \"System.Void\", \"EntryBlock\": {\"NodeType\": \"Block" +
"\", \"TypeDeclaration\": \"System.Void\", \"Locals\": [{\"NodeType\": \"Local\", \"TypeDeclaration\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}], \"Formulas\"" +
": [{\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Varia" +
"ble\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"" +
"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnony" +
"m.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"" +
"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": nul" +
"l, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add\", \"Member\": \"Urasandesu.NAnonym.ILTools.O" +
"pCode Add\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}}]}}", gen.DumpEntryPoint());
            #endregion
        }

        [Test]
        public void EvalTest04_ValidReturn1_SimpleStatements()
        {
            var gen = new ReflectiveMethodDesigner2();
            var methodGen = new MockMethodBaseGenerator();
            var returnType = new MockTypeGenerator(typeof(int).ToTypeDecl());
            gen.ILBuilder = new EmptyILBuilder(methodGen, returnType);
            gen.Eval(() => Dsl.Return(10));
            gen.Eval(() => Dsl.End());
            //Console.WriteLine(gen.Dump());
            #region Assertion
            Assert.AreEqual("{\"NodeType\": \"End\", \"TypeDeclaration\": \"System.Int32\", \"EntryBlock\": {\"NodeType\": \"Bloc" +
"k\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [], \"Formulas\": [{\"NodeType\": \"Return\"" +
", \"TypeDeclaration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Constant\", \"TypeDeclaration\":" +
" \"System.Int32\", \"ConstantValue\": 10}, \"Label\": null}]}}", gen.DumpEntryPoint());
            #endregion
        }

        [Test]
        public void EvalTest04_ValidReturn2_ComplexStatements1()
        {
            var gen = new ReflectiveMethodDesigner2();
            var methodGen = new MockMethodBaseGenerator();
            var returnType = new MockTypeGenerator(typeof(int).ToTypeDecl());
            gen.ILBuilder = new EmptyILBuilder(methodGen, returnType);
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
            //Console.WriteLine(gen.Dump());
            #region Assertion
            Assert.AreEqual("{\"NodeType\": \"End\", \"TypeDeclaration\": \"System.Int32\", \"EntryBlock\": {\"NodeType\": \"Bloc" +
"k\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [{\"NodeType\": \"Local\", \"TypeDeclaration" +
"\": \"System.Int32\", \"LocalName\": \"i\", \"Local\": null}], \"Formulas\": [{\"NodeType\": \"Assig" +
"n\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\"" +
": \"System.Int32\", \"VariableName\": \"i\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"L" +
"ocal\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"i\", \"Local\": null}}, \"Method\": " +
"\"=\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValu" +
"e\": 10}}, {\"NodeType\": \"Conditional\", \"TypeDeclaration\": \"System.Int32\", \"Test\": {\"NodeT" +
"ype\": \"LessThan\", \"TypeDeclaration\": \"System.Boolean\", \"Left\": {\"NodeType\": \"Variable\"," +
" \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"i\", \"VariableIndex\": 0, \"Resolved\":" +
" {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"i\", \"Local\": n" +
"ull}}, \"Method\": \"<\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int3" +
"2\", \"ConstantValue\": 0}}, \"IfTrue\": {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.Int" +
"32\", \"Locals\": [], \"Formulas\": [{\"NodeType\": \"Return\", \"TypeDeclaration\": \"System.Int32\"" +
", \"Body\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": " +
"-1}, \"Label\": null}]}, \"IfFalse\": {\"NodeType\": \"Conditional\", \"TypeDeclaration\": \"System." +
"Int32\", \"Test\": {\"NodeType\": \"Equal\", \"TypeDeclaration\": \"System.Boolean\", \"Left\": {\"N" +
"odeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"i\", \"VariableI" +
"ndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalNam" +
"e\": \"i\", \"Local\": null}}, \"Method\": \"==\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDec" +
"laration\": \"System.Int32\", \"ConstantValue\": 0}}, \"IfTrue\": {\"NodeType\": \"Block\", \"TypeDe" +
"claration\": \"System.Int32\", \"Locals\": [], \"Formulas\": [{\"NodeType\": \"Return\", \"TypeDecla" +
"ration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int" +
"32\", \"ConstantValue\": 0}, \"Label\": null}]}, \"IfFalse\": {\"NodeType\": \"Block\", \"TypeDeclar" +
"ation\": \"System.Int32\", \"Locals\": [], \"Formulas\": [{\"NodeType\": \"Return\", \"TypeDeclarati" +
"on\": \"System.Int32\", \"Body\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int32\"" +
", \"ConstantValue\": 1}, \"Label\": null}]}}}]}}", gen.DumpEntryPoint());
            #endregion
        }

        [Test]
        public void EvalTest04_ValidReturn2_ComplexStatements2()
        {
            var gen = new ReflectiveMethodDesigner2();
            var methodGen = new MockMethodBaseGenerator();
            var returnType = new MockTypeGenerator(typeof(int).ToTypeDecl());
            gen.ILBuilder = new EmptyILBuilder(methodGen, returnType);
            var i = default(int);
            var result = default(string);
            gen.Eval(() => Dsl.Allocate(i).As(10));
            gen.Eval(() => Dsl.Allocate(result).As(i.ToString()));
            gen.Eval(() => Dsl.If(i < 0));
            {
                gen.Eval(() => Dsl.Return(-1));
            }
            gen.Eval(() => Dsl.ElseIf(i == 0));
            {
                gen.Eval(() => Dsl.Allocate(result).As("00000000000"));
            }
            gen.Eval(() => Dsl.Else());
            {
                gen.Eval(() => Dsl.Return(1));
            }
            gen.Eval(() => Dsl.EndIf());
            gen.Eval(() => Dsl.Return(result.Length));
            gen.Eval(() => Dsl.End());
            //Console.WriteLine(gen.Dump());
            #region Assertion
            Assert.AreEqual("{\"NodeType\": \"End\", \"TypeDeclaration\": \"System.Int32\", \"EntryBlock\": {\"NodeType\": \"Bloc" +
"k\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [{\"NodeType\": \"Local\", \"TypeDeclaration" +
"\": \"System.Int32\", \"LocalName\": \"i\", \"Local\": null}, {\"NodeType\": \"Local\", \"TypeDeclar" +
"ation\": \"System.String\", \"LocalName\": \"result\", \"Local\": null}], \"Formulas\": [{\"NodeType" +
"\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\", \"TypeD" +
"eclaration\": \"System.Int32\", \"VariableName\": \"i\", \"VariableIndex\": 0, \"Resolved\": {\"Node" +
"Type\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"i\", \"Local\": null}}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int32\", \"C" +
"onstantValue\": 10}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"" +
"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.String\", \"VariableName\": \"result\", \"Va" +
"riableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.String\", \"" +
"LocalName\": \"result\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Call\", \"" +
"TypeDeclaration\": \"System.String\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\"" +
": \"System.Int32\", \"VariableName\": \"i\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"L" +
"ocal\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"i\", \"Local\": null}}, \"Method\": " +
"\"System.String ToString()\", \"Arguments\": []}}, {\"NodeType\": \"Conditional\", \"TypeDeclaration" +
"\": \"System.Void\", \"Test\": {\"NodeType\": \"LessThan\", \"TypeDeclaration\": \"System.Boolean\"," +
" \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"i" +
"\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int3" +
"2\", \"LocalName\": \"i\", \"Local\": null}}, \"Method\": \"<\", \"Right\": {\"NodeType\": \"Constan" +
"t\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 0}}, \"IfTrue\": {\"NodeType\": \"Blo" +
"ck\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [], \"Formulas\": [{\"NodeType\": \"Return\"" +
", \"TypeDeclaration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Constant\", \"TypeDeclaration\"" +
": \"System.Int32\", \"ConstantValue\": -1}, \"Label\": null}]}, \"IfFalse\": {\"NodeType\": \"Condit" +
"ional\", \"TypeDeclaration\": \"System.Void\", \"Test\": {\"NodeType\": \"Equal\", \"TypeDeclaration" +
"\": \"System.Boolean\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\"" +
", \"VariableName\": \"i\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDecla" +
"ration\": \"System.Int32\", \"LocalName\": \"i\", \"Local\": null}}, \"Method\": \"==\", \"Right\": " +
"{\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 0}}, \"IfTrue" +
"\": {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.Void\", \"Locals\": [], \"Formulas\": [{" +
"\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Left\": {\"NodeType\": \"Variable\"" +
", \"TypeDeclaration\": \"System.String\", \"VariableName\": \"result\", \"VariableIndex\": 0, \"Res" +
"olved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.String\", \"LocalName\": \"result\"" +
", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\":" +
" \"System.String\", \"ConstantValue\": 00000000000}}]}, \"IfFalse\": {\"NodeType\": \"Block\", \"Typ" +
"eDeclaration\": \"System.Int32\", \"Locals\": [], \"Formulas\": [{\"NodeType\": \"Return\", \"TypeDe" +
"claration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System." +
"Int32\", \"ConstantValue\": 1}, \"Label\": null}]}}}, {\"NodeType\": \"Return\", \"TypeDeclaration\"" +
": \"System.Int32\", \"Body\": {\"NodeType\": \"Property\", \"TypeDeclaration\": \"System.Int32\", \"" +
"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.String\", \"VariableName\": \"" +
"result\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Syst" +
"em.String\", \"LocalName\": \"result\", \"Local\": null}}, \"Member\": \"Int32 Length\", \"Member\":" +
" \"Int32 Length\"}, \"Label\": null}]}}", gen.DumpEntryPoint());
            #endregion
        }

        [Test]
        [ExpectedException(typeof(TypeCheckException), ExpectedMessage = "The return type, \"System.String\" can't convert to \"System.Int32\".")]
        public void EvalTest05_InvalidReturn1_IncompatibleType_SimpleStatements1()
        {
            var gen = new ReflectiveMethodDesigner2();
            var methodGen = new MockMethodBaseGenerator();
            var returnType = new MockTypeGenerator(typeof(int).ToTypeDecl());
            gen.ILBuilder = new EmptyILBuilder(methodGen, returnType);
            gen.Eval(() => Dsl.Return("aiueo"));
            gen.Eval(() => Dsl.End());
        }

        [Test]
        [ExpectedException(typeof(TypeCheckException), ExpectedMessage = "This method doesn't have return type but it contains return type, \"System.Int32\".")]
        public void EvalTest05_InvalidReturn1_IncompatibleType_SimpleStatements2()
        {
            var gen = new ReflectiveMethodDesigner2();
            var methodGen = new MockMethodBaseGenerator();
            var returnType = new MockTypeGenerator(typeof(void).ToTypeDecl());
            gen.ILBuilder = new EmptyILBuilder(methodGen, returnType);
            gen.Eval(() => Dsl.Return(10));
            gen.Eval(() => Dsl.End());
        }

        [Test]
        [ExpectedException(typeof(TypeCheckException), ExpectedMessage = "The IfTrue return type, \"System.Int32\" is incompatible to IfFalse return type, \"System.String\".")]
        public void EvalTest05_InvalidReturn2_IncompatibleType_ComplexStatements1()
        {
            var gen = new ReflectiveMethodDesigner2();
            var methodGen = new MockMethodBaseGenerator();
            var returnType = new MockTypeGenerator(typeof(int).ToTypeDecl());
            gen.ILBuilder = new EmptyILBuilder(methodGen, returnType);
            var i = default(int);
            var p1 = default(PropertyTestClass1);
            var p1ValueProperty = typeof(PropertyTestClass1).GetProperty("ValueProperty");
            var p1ObjectProperty = typeof(PropertyTestClass1).GetProperty("ObjectProperty");
            gen.Eval(() => Dsl.Allocate(i).As(10));
            gen.Eval(() => Dsl.Allocate(p1).As(new PropertyTestClass1()));
            gen.Eval(() => Dsl.Return(i < 10 ? p1ValueProperty.GetValue(p1, null) : p1ObjectProperty.GetValue(p1, null)));
            gen.Eval(() => Dsl.End());
        }

        [Test]
        [ExpectedException(typeof(TypeCheckException), ExpectedMessage = "The method has path that doesn't return value.")]
        public void EvalTest05_InvalidReturn2_IncompatibleType_ComplexStatements2()
        {
            var gen = new ReflectiveMethodDesigner2();
            var methodGen = new MockMethodBaseGenerator();
            var returnType = new MockTypeGenerator(typeof(int).ToTypeDecl());
            gen.ILBuilder = new EmptyILBuilder(methodGen, returnType);
            var i = default(int);
            gen.Eval(() => Dsl.Allocate(i).As(10));
            gen.Eval(() => Dsl.If(i < 0));
            {
                gen.Eval(() => Console.WriteLine("a"));
            }
            gen.Eval(() => Dsl.ElseIf(i == 0));
            {
                gen.Eval(() => Console.WriteLine("b"));
            }
            gen.Eval(() => Dsl.Else());
            {
                gen.Eval(() => Dsl.Return(-10));
            }
            gen.Eval(() => Dsl.EndIf());
            gen.Eval(() => Dsl.End());
        }

        [Test]
        public void EvalTest06_WithArgument()
        {
            var gen = new ReflectiveMethodDesigner2();
            var methodGen = new MockMethodBaseGenerator();

            var parameter = default(IParameterGenerator);
            parameter = new MockParameterGenerator(typeof(int));
            methodGen.Parameters.Add(parameter);

            var returnType = new MockTypeGenerator(typeof(int));

            gen.ILBuilder = new EmptyILBuilder(methodGen, returnType);
            var value = default(int);
            gen.Eval(() => Dsl.Allocate(value).As(Dsl.LoadArgument<int>(0)));
            gen.Eval(() => Dsl.Return(value + value * value * value));
            gen.Eval(() => Dsl.End());
            Console.WriteLine(gen.DumpEntryPoint());
        }
    }
}
