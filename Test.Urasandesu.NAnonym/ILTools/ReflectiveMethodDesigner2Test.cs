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
            //Console.WriteLine(gen.DumpEntryPoint());
            #region Assertion
            Assert.AreEqual("{\"NodeType\": \"End\", \"TypeDeclaration\": \"System.Int32\", \"EntryBlock\": {\"NodeType\": \"Bloc" +
"k\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [{\"NodeType\": \"Local\", \"TypeDeclaration" +
"\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}], \"Formulas\": [{\"NodeType\": \"A" +
"ssign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Consta" +
"nt\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 10}, \"Left\": {\"NodeType\": \"Vari" +
"able\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"" +
"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value" +
"\", \"Local\": null}}}, {\"NodeType\": \"Conditional\", \"TypeDeclaration\": \"System.Int32\", \"Tes" +
"t\": {\"NodeType\": \"AndAlso\", \"TypeDeclaration\": \"System.Boolean\", \"Method\": \"&&\", \"Righ" +
"t\": {\"NodeType\": \"NotEqual\", \"TypeDeclaration\": \"System.Boolean\", \"Method\": \"!=\", \"Rig" +
"ht\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 50}, \"" +
"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"valu" +
"e\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.In" +
"t32\", \"LocalName\": \"value\", \"Local\": null}}}, \"Left\": {\"NodeType\": \"AndAlso\", \"TypeDec" +
"laration\": \"System.Boolean\", \"Method\": \"&&\", \"Right\": {\"NodeType\": \"NotEqual\", \"TypeDe" +
"claration\": \"System.Boolean\", \"Method\": \"!=\", \"Right\": {\"NodeType\": \"Constant\", \"TypeD" +
"eclaration\": \"System.Int32\", \"ConstantValue\": 40}, \"Left\": {\"NodeType\": \"Variable\", \"Typ" +
"eDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Resolved\": " +
"{\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\"" +
": null}}}, \"Left\": {\"NodeType\": \"AndAlso\", \"TypeDeclaration\": \"System.Boolean\", \"Method\"" +
": \"&&\", \"Right\": {\"NodeType\": \"NotEqual\", \"TypeDeclaration\": \"System.Boolean\", \"Method\"" +
": \"!=\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int32\", \"Constant" +
"Value\": 30}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"Variab" +
"leName\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaratio" +
"n\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}}, \"Left\": {\"NodeType\": \"NotE" +
"qual\", \"TypeDeclaration\": \"System.Boolean\", \"Method\": \"!=\", \"Right\": {\"NodeType\": \"Con" +
"stant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 20}, \"Left\": {\"NodeType\": \"V" +
"ariable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1" +
", \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"va" +
"lue\", \"Local\": null}}}}}}, \"IfTrue\": {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.In" +
"t32\", \"Locals\": [{\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Object\", \"LocalName\":" +
" \"objValue\", \"Local\": null}, {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Nullable`1[" +
"System.Int32]\", \"LocalName\": \"value2\", \"Local\": null}], \"Formulas\": [{\"NodeType\": \"Assig" +
"n\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Convert\"," +
" \"TypeDeclaration\": \"System.Object\", \"Method\": null, \"Operand\": {\"NodeType\": \"Variable\"," +
" \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"L" +
"ocal\": null}}}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Object\", \"Va" +
"riableName\": \"objValue\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDec" +
"laration\": \"System.Object\", \"LocalName\": \"objValue\", \"Local\": null}}}, {\"NodeType\": \"Con" +
"ditional\", \"TypeDeclaration\": \"System.Int32\", \"Test\": {\"NodeType\": \"NotEqual\", \"TypeDecl" +
"aration\": \"System.Boolean\", \"Method\": \"!=\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDec" +
"laration\": \"System.Nullable`1[System.Int32]\", \"ConstantValue\": null}, \"Left\": {\"NodeType\": " +
"\"Assign\", \"TypeDeclaration\": \"System.Nullable`1[System.Int32]\", \"Method\": \"=\", \"Right\": " +
"{\"NodeType\": \"TypeAs\", \"TypeDeclaration\": \"System.Nullable`1[System.Int32]\", \"Method\": nul" +
"l, \"Operand\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Object\", \"VariableName\"" +
": \"objValue\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\":" +
" \"System.Object\", \"LocalName\": \"objValue\", \"Local\": null}}}, \"Left\": {\"NodeType\": \"Vari" +
"able\", \"TypeDeclaration\": \"System.Nullable`1[System.Int32]\", \"VariableName\": \"value2\", \"Va" +
"riableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Nullable`1[" +
"System.Int32]\", \"LocalName\": \"value2\", \"Local\": null}}}}, \"IfTrue\": {\"NodeType\": \"Block\"" +
", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [], \"Formulas\": [{\"NodeType\": \"Return\", " +
"\"TypeDeclaration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Add\", \"TypeDeclaration\": \"Syst" +
"em.Int32\", \"Method\": \"+\", \"Right\": {\"NodeType\": \"Convert\", \"TypeDeclaration\": \"System." +
"Int32\", \"Method\": null, \"Operand\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.N" +
"ullable`1[System.Int32]\", \"VariableName\": \"value2\", \"VariableIndex\": -1, \"Resolved\": {\"Nod" +
"eType\": \"Local\", \"TypeDeclaration\": \"System.Nullable`1[System.Int32]\", \"LocalName\": \"value" +
"2\", \"Local\": null}}}, \"Left\": {\"NodeType\": \"Add\", \"TypeDeclaration\": \"System.Int32\", \"" +
"Method\": \"+\", \"Right\": {\"NodeType\": \"Multiply\", \"TypeDeclaration\": \"System.Int32\", \"Me" +
"thod\": \"*\", \"Right\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"Vari" +
"ableName\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclarat" +
"ion\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Left\": {\"NodeType\": \"Var" +
"iable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, " +
"\"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"valu" +
"e\", \"Local\": null}}}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\"" +
", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"Typ" +
"eDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}}}, \"Label\": null}]}," +
" \"IfFalse\": {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [], \"For" +
"mulas\": [{\"NodeType\": \"Return\", \"TypeDeclaration\": \"System.Int32\", \"Body\": {\"NodeType\":" +
" \"Add\", \"TypeDeclaration\": \"System.Int32\", \"Method\": \"+\", \"Right\": {\"NodeType\": \"Mult" +
"iply\", \"TypeDeclaration\": \"System.Int32\", \"Method\": \"*\", \"Right\": {\"NodeType\": \"Variab" +
"le\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"R" +
"esolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\"" +
", \"Local\": null}}, \"Left\": {\"NodeType\": \"Multiply\", \"TypeDeclaration\": \"System.Int32\", \"" +
"Method\": \"*\", \"Right\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"V" +
"ariableName\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDecla" +
"ration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Left\": {\"NodeType\": \"" +
"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -" +
"1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"v" +
"alue\", \"Local\": null}}}}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.In" +
"t32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"" +
"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}}, \"Label\": null}" +
"]}}]}, \"IfFalse\": {\"NodeType\": \"Conditional\", \"TypeDeclaration\": \"System.Int32\", \"Test\":" +
" {\"NodeType\": \"Equal\", \"TypeDeclaration\": \"System.Boolean\", \"Method\": \"==\", \"Right\": {" +
"\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 20}, \"Left\":" +
" {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"" +
"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", " +
"\"LocalName\": \"value\", \"Local\": null}}}, \"IfTrue\": {\"NodeType\": \"Block\", \"TypeDeclaratio" +
"n\": \"System.Int32\", \"Locals\": [], \"Formulas\": [{\"NodeType\": \"Return\", \"TypeDeclaration\"" +
": \"System.Int32\", \"Body\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"" +
"VariableName\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDecl" +
"aration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Label\": null}]}, \"IfFa" +
"lse\": {\"NodeType\": \"Conditional\", \"TypeDeclaration\": \"System.Int32\", \"Test\": {\"NodeType\"" +
": \"Equal\", \"TypeDeclaration\": \"System.Boolean\", \"Method\": \"==\", \"Right\": {\"NodeType\":" +
" \"Constant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 40}, \"Left\": {\"NodeType\"" +
": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex" +
"\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\"" +
": \"value\", \"Local\": null}}}, \"IfTrue\": {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System" +
".Int32\", \"Locals\": [], \"Formulas\": [{\"NodeType\": \"Return\", \"TypeDeclaration\": \"System.In" +
"t32\", \"Body\": {\"NodeType\": \"ExclusiveOr\", \"TypeDeclaration\": \"System.Int32\", \"Method\": " +
"\"^\", \"Right\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName" +
"\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"" +
"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Left\": {\"NodeType\": \"ExclusiveOr" +
"\", \"TypeDeclaration\": \"System.Int32\", \"Method\": \"^\", \"Right\": {\"NodeType\": \"Variable\"" +
", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Resol" +
"ved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"" +
"Local\": null}}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"Var" +
"iableName\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclara" +
"tion\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}}}, \"Label\": null}]}, \"IfFal" +
"se\": {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [], \"Formulas\":" +
" [{\"NodeType\": \"Return\", \"TypeDeclaration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Condi" +
"tional\", \"TypeDeclaration\": \"System.Int32\", \"Test\": {\"NodeType\": \"Equal\", \"TypeDeclarati" +
"on\": \"System.Boolean\", \"Method\": \"==\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclarat" +
"ion\": \"System.Int32\", \"ConstantValue\": 30}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclar" +
"ation\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"Node" +
"Type\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}" +
"}}, \"IfTrue\": {\"NodeType\": \"Add\", \"TypeDeclaration\": \"System.Int32\", \"Method\": \"+\", \"" +
"Right\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"val" +
"ue\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.I" +
"nt32\", \"LocalName\": \"value\", \"Local\": null}}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDe" +
"claration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"" +
"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": n" +
"ull}}}, \"IfFalse\": {\"NodeType\": \"Multiply\", \"TypeDeclaration\": \"System.Int32\", \"Method\":" +
" \"*\", \"Right\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableNam" +
"e\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": " +
"\"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Left\": {\"NodeType\": \"Variable\"" +
", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Resol" +
"ved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"" +
"Local\": null}}}}, \"Label\": null}]}}}}]}}", gen.DumpEntryPoint());
            #endregion
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
            //Console.WriteLine(gen.DumpEntryPoint());
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
"eclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"ReflectiveNew\", \"Typ" +
"eDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"Constructor\": \"Void .ctor()\"" +
", \"Arguments\": []}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu" +
".NAnonym.Etc.PropertyTestClass1\", \"VariableName\": \"p1\", \"VariableIndex\": -1, \"Resolved\": {\"" +
"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"L" +
"ocalName\": \"p1\", \"Local\": null}}}, {\"NodeType\": \"ReflectiveCall\", \"TypeDeclaration\": \"Sy" +
"stem.Void\", \"Instance\": null, \"Method\": \"Void WriteLog(System.String, System.Object[])\", \"Ar" +
"guments\": [{\"NodeType\": \"NewArrayInit\", \"TypeDeclaration\": \"System.Object[]\", \"Formulas\":" +
" [{\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.String\", \"ConstantValue\": \"{0}\"}, " +
"{\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\"" +
", \"VariableName\": \"p1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDec" +
"laration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", \"Local\": nu" +
"ll}}]}]}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Righ" +
"t\": {\"NodeType\": \"Convert\", \"TypeDeclaration\": \"System.Object\", \"Method\": null, \"Operand" +
"\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 10}}, \"L" +
"eft\": {\"NodeType\": \"ReflectiveProperty\", \"TypeDeclaration\": \"System.Int32\", \"Instance\": {" +
"\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\"," +
" \"VariableName\": \"p1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDecl" +
"aration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", \"Local\": nul" +
"l}}, \"Member\": \"Int32 ValueProperty\", \"Member\": \"Int32 ValueProperty\"}}, {\"NodeType\": \"Re" +
"flectiveCall\", \"TypeDeclaration\": \"System.Void\", \"Instance\": null, \"Method\": \"Void WriteLo" +
"g(System.String, System.Object[])\", \"Arguments\": [{\"NodeType\": \"NewArrayInit\", \"TypeDeclarat" +
"ion\": \"System.Object[]\", \"Formulas\": [{\"NodeType\": \"Constant\", \"TypeDeclaration\": \"Syste" +
"m.String\", \"ConstantValue\": \"ValueProperty: {0}\"}, {\"NodeType\": \"Convert\", \"TypeDeclaratio" +
"n\": \"System.Object\", \"Method\": null, \"Operand\": {\"NodeType\": \"ReflectiveProperty\", \"Type" +
"Declaration\": \"System.Int32\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"T" +
"est.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"VariableName\": \"p1\", \"VariableIndex\": -1, \"" +
"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTes" +
"tClass1\", \"LocalName\": \"p1\", \"Local\": null}}, \"Member\": \"Int32 ValueProperty\", \"Member\"" +
": \"Int32 ValueProperty\"}}]}]}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.String\", \"C" +
"onstantValue\": \"a\"}, \"Left\": {\"NodeType\": \"ReflectiveProperty\", \"TypeDeclaration\": \"Syst" +
"em.String\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnon" +
"ym.Etc.PropertyTestClass1\", \"VariableName\": \"p1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeT" +
"ype\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalNa" +
"me\": \"p1\", \"Local\": null}}, \"Member\": \"System.String ObjectProperty\", \"Member\": \"System." +
"String ObjectProperty\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method" +
"\": \"=\", \"Right\": {\"NodeType\": \"ReflectiveProperty\", \"TypeDeclaration\": \"System.String\"," +
" \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.Prope" +
"rtyTestClass1\", \"VariableName\": \"p1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Loc" +
"al\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\"" +
", \"Local\": null}}, \"Member\": \"System.String ObjectProperty\", \"Member\": \"System.String Objec" +
"tProperty\"}, \"Left\": {\"NodeType\": \"ReflectiveProperty\", \"TypeDeclaration\": \"System.String\"" +
", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.Pro" +
"pertyTestClass1\", \"VariableName\": \"p1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"L" +
"ocal\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1" +
"\", \"Local\": null}}, \"Member\": \"System.String ObjectProperty\", \"Member\": \"System.String Obj" +
"ectProperty\"}}, {\"NodeType\": \"ReflectiveCall\", \"TypeDeclaration\": \"System.Void\", \"Instance" +
"\": null, \"Method\": \"Void WriteLog(System.String, System.Object[])\", \"Arguments\": [{\"NodeType" +
"\": \"NewArrayInit\", \"TypeDeclaration\": \"System.Object[]\", \"Formulas\": [{\"NodeType\": \"Cons" +
"tant\", \"TypeDeclaration\": \"System.String\", \"ConstantValue\": \"ObjectProperty: {0}\"}, {\"Node" +
"Type\": \"ReflectiveProperty\", \"TypeDeclaration\": \"System.String\", \"Instance\": {\"NodeType\":" +
" \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"VariableNa" +
"me\": \"p1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"" +
"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", \"Local\": null}}, \"Member" +
"\": \"System.String ObjectProperty\", \"Member\": \"System.String ObjectProperty\"}]}]}, {\"NodeType" +
"\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"" +
"New\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"Constructor\": \"Voi" +
"d .ctor()\", \"Arguments\": []}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test." +
"Urasandesu.NAnonym.Etc.FieldTestClass2\", \"VariableName\": \"f2\", \"VariableIndex\": -1, \"Resolve" +
"d\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\"," +
" \"LocalName\": \"f2\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System" +
".Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Convert\", \"TypeDeclaration\": \"System.Obj" +
"ect\", \"Method\": null, \"Operand\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int" +
"32\", \"ConstantValue\": 30}}, \"Left\": {\"NodeType\": \"ReflectiveField\", \"TypeDeclaration\": \"" +
"System.Int32\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NA" +
"nonym.Etc.FieldTestClass2\", \"VariableName\": \"f2\", \"VariableIndex\": -1, \"Resolved\": {\"NodeT" +
"ype\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"LocalName\"" +
": \"f2\", \"Local\": null}}, \"Member\": \"Int32 ValueField\", \"Member\": \"Int32 ValueField\"}}, " +
"{\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"No" +
"deType\": \"ReflectiveField\", \"TypeDeclaration\": \"System.Int32\", \"Instance\": {\"NodeType\": \"" +
"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"VariableName\":" +
" \"f2\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test." +
"Urasandesu.NAnonym.Etc.FieldTestClass2\", \"LocalName\": \"f2\", \"Local\": null}}, \"Member\": \"In" +
"t32 ValueField\", \"Member\": \"Int32 ValueField\"}, \"Left\": {\"NodeType\": \"ReflectiveField\", \"" +
"TypeDeclaration\": \"System.Int32\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\"" +
": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"VariableName\": \"f2\", \"VariableIndex\": -1, " +
"\"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTest" +
"Class2\", \"LocalName\": \"f2\", \"Local\": null}}, \"Member\": \"Int32 ValueField\", \"Member\": \"" +
"Int32 ValueField\"}}, {\"NodeType\": \"Call\", \"TypeDeclaration\": \"System.Void\", \"Instance\": n" +
"ull, \"Method\": \"Void WriteLog(System.String, System.Object[])\", \"Arguments\": [{\"NodeType\": \"" +
"NewArrayInit\", \"TypeDeclaration\": \"System.Object[]\", \"Formulas\": [{\"NodeType\": \"Constant\"" +
", \"TypeDeclaration\": \"System.String\", \"ConstantValue\": \"ValueField: {0}\"}, {\"NodeType\": \"" +
"Convert\", \"TypeDeclaration\": \"System.Object\", \"Method\": null, \"Operand\": {\"NodeType\": \"" +
"ReflectiveField\", \"TypeDeclaration\": \"System.Int32\", \"Instance\": {\"NodeType\": \"Variable\"," +
" \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"VariableName\": \"f2\", \"V" +
"ariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.N" +
"Anonym.Etc.FieldTestClass2\", \"LocalName\": \"f2\", \"Local\": null}}, \"Member\": \"Int32 ValueFie" +
"ld\", \"Member\": \"Int32 ValueField\"}}]}]}, {\"NodeType\": \"ReflectiveCall\", \"TypeDeclaration\"" +
": \"System.Void\", \"Instance\": null, \"Method\": \"Void WriteLog(System.String, System.Object[])\"" +
", \"Arguments\": [{\"NodeType\": \"NewArrayInit\", \"TypeDeclaration\": \"System.Object[]\", \"Formu" +
"las\": [{\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.String\", \"ConstantValue\": \"Va" +
"lueField: {0}\"}, {\"NodeType\": \"Convert\", \"TypeDeclaration\": \"System.Object\", \"Method\": nu" +
"ll, \"Operand\": {\"NodeType\": \"ReflectiveField\", \"TypeDeclaration\": \"System.Int32\", \"Instan" +
"ce\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass" +
"2\", \"VariableName\": \"f2\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"Type" +
"Declaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"LocalName\": \"f2\", \"Local\": nu" +
"ll}}, \"Member\": \"Int32 ValueField\", \"Member\": \"Int32 ValueField\"}}]}]}, {\"NodeType\": \"Ass" +
"ign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Reflecti" +
"veNew\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass2\", \"Constructor\": \"" +
"Void .ctor(Int32, System.String)\", \"Arguments\": [{\"NodeType\": \"ReflectiveProperty\", \"TypeDe" +
"claration\": \"System.Int32\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Tes" +
"t.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"VariableName\": \"p1\", \"VariableIndex\": -1, \"Re" +
"solved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestC" +
"lass1\", \"LocalName\": \"p1\", \"Local\": null}}, \"Member\": \"Int32 ValueProperty\", \"Member\": " +
"\"Int32 ValueProperty\"}, {\"NodeType\": \"ReflectiveProperty\", \"TypeDeclaration\": \"System.Strin" +
"g\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.P" +
"ropertyTestClass1\", \"VariableName\": \"p1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"" +
"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"" +
"p1\", \"Local\": null}}, \"Member\": \"System.String ObjectProperty\", \"Member\": \"System.String O" +
"bjectProperty\"}]}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NA" +
"nonym.Etc.PropertyTestClass2\", \"VariableName\": \"p2\", \"VariableIndex\": -1, \"Resolved\": {\"No" +
"deType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass2\", \"Loca" +
"lName\": \"p2\", \"Local\": null}}}, {\"NodeType\": \"ReflectiveCall\", \"TypeDeclaration\": \"Syste" +
"m.Void\", \"Instance\": null, \"Method\": \"Void WriteLog(System.String, System.Object[])\", \"Argum" +
"ents\": [{\"NodeType\": \"NewArrayInit\", \"TypeDeclaration\": \"System.Object[]\", \"Formulas\": [{" +
"\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.String\", \"ConstantValue\": \"({0}, {1})\"" +
"}, {\"NodeType\": \"Convert\", \"TypeDeclaration\": \"System.Object\", \"Method\": null, \"Operand\"" +
": {\"NodeType\": \"Property\", \"TypeDeclaration\": \"System.Int32\", \"Instance\": {\"NodeType\": " +
"\"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass2\", \"VariableNam" +
"e\": \"p2\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"T" +
"est.Urasandesu.NAnonym.Etc.PropertyTestClass2\", \"LocalName\": \"p2\", \"Local\": null}}, \"Member\"" +
": \"Int32 ValueProperty\", \"Member\": \"Int32 ValueProperty\"}}, {\"NodeType\": \"Property\", \"Ty" +
"peDeclaration\": \"System.String\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Test.Urasandesu.NAnonym.Etc.PropertyTestClass2\", \"VariableName\": \"p2\", \"VariableIndex\": -1," +
" \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.Property" +
"TestClass2\", \"LocalName\": \"p2\", \"Local\": null}}, \"Member\": \"System.String ObjectProperty\"" +
", \"Member\": \"System.String ObjectProperty\"}]}]}, {\"NodeType\": \"Assign\", \"TypeDeclaration\":" +
" \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"ReflectiveCall\", \"TypeDeclaration" +
"\": \"System.Int32\", \"Instance\": null, \"Method\": \"Int32 GetValue(Int32)\", \"Arguments\": [{\"" +
"NodeType\": \"ReflectiveField\", \"TypeDeclaration\": \"System.Int32\", \"Instance\": {\"NodeType\":" +
" \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"VariableName\"" +
": \"f2\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Tes" +
"t.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"LocalName\": \"f2\", \"Local\": null}}, \"Member\": \"" +
"Int32 ValueField\", \"Member\": \"Int32 ValueField\"}]}, \"Left\": {\"NodeType\": \"Variable\", \"Ty" +
"peDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Resolved\":" +
" {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\"" +
": null}}}]}}", gen.DumpEntryPoint());
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
            //Console.WriteLine(gen.DumpEntryPoint());
            #region Assertion
            Assert.AreEqual("{\"NodeType\": \"End\", \"TypeDeclaration\": \"System.Void\", \"EntryBlock\": {\"NodeType\": \"Block" +
"\", \"TypeDeclaration\": \"System.Void\", \"Locals\": [{\"NodeType\": \"Local\", \"TypeDeclaration\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}], \"Formulas\"" +
": [{\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"" +
"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": nu" +
"ll, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add\", \"Member\": \"Urasandesu.NAnonym.ILTools." +
"OpCode Add\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTo" +
"ols.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"L" +
"ocal\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Lo" +
"cal\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", " +
"\"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"I" +
"nstance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu" +
".NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\"" +
": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName" +
"\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\"" +
", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym." +
"ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"M" +
"ember\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"Ty" +
"peDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableInd" +
"ex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools." +
"OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclarati" +
"on\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools." +
"OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\"" +
": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opc" +
"ode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasan" +
"desu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assi" +
"gn\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", " +
"\"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasan" +
"desu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"" +
"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, " +
"{\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"No" +
"deType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null," +
" \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools" +
".OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\"" +
": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\"," +
" \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"" +
"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"" +
", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\":" +
" \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Reso" +
"lved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Loca" +
"lName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.V" +
"oid\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAn" +
"onym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"" +
", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\"," +
" \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variab" +
"leIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDecl" +
"aration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaratio" +
"n\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"Node" +
"Type\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": " +
"\"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"U" +
"rasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"" +
"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Fiel" +
"d\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"U" +
"rasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"" +
"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"" +
", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"T" +
"ypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null" +
"}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": " +
"{\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": " +
"null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.IL" +
"Tools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NA" +
"nonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeT" +
"ype\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcod" +
"e1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\"" +
": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpC" +
"ode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclarati" +
"on\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"" +
"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Sys" +
"tem.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandes" +
"u.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_" +
"Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variab" +
"le\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"V" +
"ariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnony" +
"m.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"Typ" +
"eDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDecla" +
"ration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnony" +
"m.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"" +
"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableNam" +
"e\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType" +
"\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"" +
"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add" +
"_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.Op" +
"Code\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\"" +
", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\":" +
" null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Righ" +
"t\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instanc" +
"e\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnon" +
"ym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasande" +
"su.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"" +
"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"" +
"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Me" +
"thod\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDecl" +
"aration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": " +
"-1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode" +
"\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": " +
"\"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Uras" +
"andesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode" +
" Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"V" +
"ariable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\"" +
", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", " +
"\"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Type" +
"Declaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\"" +
": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Variab" +
"leName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclarat" +
"ion\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"Nod" +
"eType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILToo" +
"ls.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Lo" +
"cal\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Loc" +
"al\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"In" +
"stance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Ura" +
"sandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\"" +
": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\"" +
": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\"," +
" \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.I" +
"LTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Me" +
"mber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableInde" +
"x\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.O" +
"pCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaratio" +
"n\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.O" +
"pCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\"" +
": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opco" +
"de1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasand" +
"esu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assig" +
"n\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasand" +
"esu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"" +
"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"V" +
"ariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDec" +
"laration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {" +
"\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"Nod" +
"eType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, " +
"\"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools." +
"OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym." +
"ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=" +
"\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"," +
" \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasan" +
"desu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resol" +
"ved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Local" +
"Name\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Vo" +
"id\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAno" +
"nym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"," +
" \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", " +
"\"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variabl" +
"eIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTo" +
"ols.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDecla" +
"ration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration" +
"\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTo" +
"ols.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeT" +
"ype\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"" +
"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"" +
"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field" +
"\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"" +
"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"" +
", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"Ty" +
"peDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}" +
"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {" +
"\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": n" +
"ull, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAn" +
"onym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeTy" +
"pe\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode" +
"1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\"" +
": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"U" +
"rasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaratio" +
"n\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"" +
"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Syst" +
"em.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu" +
".NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_O" +
"vf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variabl" +
"e\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Va" +
"riableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"Type" +
"Declaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclar" +
"ation\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym" +
".ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"" +
"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName" +
"\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\":" +
" \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\"" +
": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"" +
"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\":" +
" \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_" +
"Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpC" +
"ode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\"," +
" \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": " +
"null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right" +
"\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance" +
"\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnony" +
"m.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandes" +
"u.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"N" +
"odeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"o" +
"pcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Met" +
"hod\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools" +
".OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDecla" +
"ration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -" +
"1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"" +
", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"" +
"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode " +
"Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Va" +
"riable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\"," +
" \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NA" +
"nonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"" +
"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeD" +
"eclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NA" +
"nonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\"" +
": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Variabl" +
"eName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclarati" +
"on\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"Node" +
"Type\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\"" +
": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode" +
" Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Loc" +
"al\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Loca" +
"l\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Uras" +
"andesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\":" +
" {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\"" +
": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.IL" +
"Tools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"Type" +
"Declaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex" +
"\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.Op" +
"Code\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration" +
"\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.Op" +
"Code Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\":" +
" \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcod" +
"e1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasande" +
"su.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign" +
"\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasande" +
"su.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"L" +
"eft\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Va" +
"riableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDecl" +
"aration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"" +
"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"Node" +
"Type\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"" +
"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.O" +
"pCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.I" +
"LTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": " +
"\"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"" +
"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\"" +
", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", " +
"\"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasand" +
"esu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Voi" +
"d\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnon" +
"ym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", " +
"\"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILToo" +
"ls.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclar" +
"ation\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILToo" +
"ls.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeTy" +
"pe\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"" +
"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Ura" +
"sandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"A" +
"ssign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\"" +
", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Ura" +
"sandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}" +
", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"," +
" \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}" +
"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"" +
"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": nu" +
"ll, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTo" +
"ols.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAno" +
"nym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeTyp" +
"e\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1" +
"\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\":" +
" \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration" +
"\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"R" +
"esolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"L" +
"ocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Syste" +
"m.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu." +
"NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ov" +
"f\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable" +
"\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Var" +
"iableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym." +
"ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeD" +
"eclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclara" +
"tion\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym." +
"ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"N" +
"odeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\"" +
": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\"" +
": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"F" +
"ield\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_O" +
"vf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", " +
"\"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": n" +
"ull}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\"" +
": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\"" +
": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym" +
".ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu" +
".NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"No" +
"deType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"op" +
"code1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Meth" +
"od\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools." +
"OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\":" +
" \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclar" +
"ation\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1" +
", \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"" +
", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"" +
"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasan" +
"desu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode A" +
"dd_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Var" +
"iable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", " +
"\"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAn" +
"onym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"" +
"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAn" +
"onym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\":" +
" {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Variable" +
"Name\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaratio" +
"n\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeT" +
"ype\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\"" +
": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Membe" +
"r\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode " +
"Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools" +
".OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Loca" +
"l\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local" +
"\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"R" +
"ight\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Inst" +
"ance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NA" +
"nonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": " +
"{\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\":" +
" \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeD" +
"eclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\"" +
": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpC" +
"ode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"U" +
"rasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpC" +
"ode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": " +
"\"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode" +
"1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandes" +
"u.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"T" +
"ypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandes" +
"u.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Le" +
"ft\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Var" +
"iableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDecla" +
"ration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"" +
"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeT" +
"ype\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"" +
"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.Op" +
"Code Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.IL" +
"Tools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"" +
"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"" +
"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\"" +
", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasande" +
"su.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolve" +
"d\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalNa" +
"me\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void" +
"\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnony" +
"m.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"" +
"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableI" +
"ndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclara" +
"tion\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Uras" +
"andesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"As" +
"sign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\"" +
", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Uras" +
"andesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}," +
" \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", " +
"\"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"Type" +
"Declaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}" +
", {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"" +
"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": nul" +
"l, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILToo" +
"ls.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnon" +
"ym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType" +
"\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\"" +
", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": " +
"\"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode" +
"\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Ura" +
"sandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Re" +
"solved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Lo" +
"calName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System" +
".Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf" +
"\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\"" +
", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Vari" +
"ableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.I" +
"LTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDe" +
"claration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclarat" +
"ion\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.I" +
"LTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"No" +
"deType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\"" +
": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\":" +
" \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Fi" +
"eld\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ov" +
"f\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": nu" +
"ll}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\"" +
": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\"" +
": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym." +
"ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu." +
"NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"Nod" +
"eType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opc" +
"ode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Metho" +
"d\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.O" +
"pCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclara" +
"tion\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1," +
" \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"," +
" \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"S" +
"ystem.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasand" +
"esu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Ad" +
"d_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Vari" +
"able\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"" +
"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAno" +
"nym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"T" +
"ypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDec" +
"laration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAno" +
"nym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": " +
"{\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableN" +
"ame\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration" +
"\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeTy" +
"pe\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\":" +
" \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member" +
"\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode A" +
"dd_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools." +
"OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local" +
"\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\"" +
": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Ri" +
"ght\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Insta" +
"nce\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAn" +
"onym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasan" +
"desu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {" +
"\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": " +
"\"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTo" +
"ols.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Membe" +
"r\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\"" +
": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"" +
"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1" +
"\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu" +
".NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Ty" +
"peDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu" +
".NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Lef" +
"t\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Vari" +
"ableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclar" +
"ation\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"N" +
"odeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeTy" +
"pe\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"M" +
"ember\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpC" +
"ode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"" +
"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"L" +
"ocal\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\"," +
" \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandes" +
"u.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"U" +
"rasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved" +
"\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalNam" +
"e\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\"" +
", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"" +
"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"T" +
"ypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIn" +
"dex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools" +
".OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclarat" +
"ion\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\":" +
" \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools" +
".OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType" +
"\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"op" +
"code1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Ass" +
"ign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\"," +
" \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeD" +
"eclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}," +
" {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"N" +
"odeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null" +
", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnony" +
"m.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\"" +
": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\"" +
", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"" +
"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"" +
", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Uras" +
"andesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Res" +
"olved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Loc" +
"alName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System." +
"Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NA" +
"nonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"" +
", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\"" +
", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Varia" +
"bleIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.IL" +
"Tools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDec" +
"laration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclarati" +
"on\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.IL" +
"Tools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"Nod" +
"eType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\":" +
" \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": " +
"\"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Fie" +
"ld\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf" +
"\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode" +
"\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": nul" +
"l}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\":" +
" {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\":" +
" null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.I" +
"LTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"Node" +
"Type\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opco" +
"de1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method" +
"\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.Op" +
"Code\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclarat" +
"ion\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, " +
"\"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", " +
"\"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Sy" +
"stem.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasande" +
"su.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add" +
"_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Varia" +
"ble\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"" +
"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnon" +
"ym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"Ty" +
"peDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDecl" +
"aration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnon" +
"ym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {" +
"\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableNa" +
"me\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeTyp" +
"e\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": " +
"\"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Ad" +
"d_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.O" +
"pCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\"" +
", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\"" +
": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Rig" +
"ht\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instan" +
"ce\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAno" +
"nym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasand" +
"esu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"" +
"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"" +
"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"M" +
"ethod\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILToo" +
"ls.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member" +
"\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDec" +
"laration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\":" +
" -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\":" +
" \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ura" +
"sandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"" +
"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\"" +
", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu." +
"NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"NodeType\": \"Assign\"," +
" \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left" +
"\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Varia" +
"bleName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclara" +
"tion\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeTyp" +
"e\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Me" +
"mber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTo" +
"ols.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"L" +
"ocal\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Lo" +
"cal\": null}}}]}}", gen.DumpEntryPoint());
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
            //Console.WriteLine(gen.DumpEntryPoint());
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
            //Console.WriteLine(gen.DumpEntryPoint());
            #region Assertion
            Assert.AreEqual("{\"NodeType\": \"End\", \"TypeDeclaration\": \"System.Int32\", \"EntryBlock\": {\"NodeType\": \"Bloc" +
"k\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [{\"NodeType\": \"Local\", \"TypeDeclaration" +
"\": \"System.Int32\", \"LocalName\": \"i\", \"Local\": null}], \"Formulas\": [{\"NodeType\": \"Assig" +
"n\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"Constant\"" +
", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 10}, \"Left\": {\"NodeType\": \"Variable" +
"\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"i\", \"VariableIndex\": -1, \"Resolve" +
"d\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"i\", \"Local\"" +
": null}}}, {\"NodeType\": \"Conditional\", \"TypeDeclaration\": \"System.Int32\", \"Test\": {\"Node" +
"Type\": \"LessThan\", \"TypeDeclaration\": \"System.Boolean\", \"Method\": \"<\", \"Right\": {\"Node" +
"Type\": \"Constant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 0}, \"Left\": {\"Nod" +
"eType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"i\", \"VariableInd" +
"ex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName" +
"\": \"i\", \"Local\": null}}}, \"IfTrue\": {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.I" +
"nt32\", \"Locals\": [], \"Formulas\": [{\"NodeType\": \"Return\", \"TypeDeclaration\": \"System.Int3" +
"2\", \"Body\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\"" +
": -1}, \"Label\": null}]}, \"IfFalse\": {\"NodeType\": \"Conditional\", \"TypeDeclaration\": \"Syste" +
"m.Int32\", \"Test\": {\"NodeType\": \"Equal\", \"TypeDeclaration\": \"System.Boolean\", \"Method\": " +
"\"==\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantVal" +
"ue\": 0}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableNa" +
"me\": \"i\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"S" +
"ystem.Int32\", \"LocalName\": \"i\", \"Local\": null}}}, \"IfTrue\": {\"NodeType\": \"Block\", \"Typ" +
"eDeclaration\": \"System.Int32\", \"Locals\": [], \"Formulas\": [{\"NodeType\": \"Return\", \"TypeDe" +
"claration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System." +
"Int32\", \"ConstantValue\": 0}, \"Label\": null}]}, \"IfFalse\": {\"NodeType\": \"Block\", \"TypeDec" +
"laration\": \"System.Int32\", \"Locals\": [], \"Formulas\": [{\"NodeType\": \"Return\", \"TypeDeclar" +
"ation\": \"System.Int32\", \"Body\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int3" +
"2\", \"ConstantValue\": 1}, \"Label\": null}]}}}]}}", gen.DumpEntryPoint());
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
            //Console.WriteLine(gen.DumpEntryPoint());
            #region Assertion
            Assert.AreEqual("{\"NodeType\": \"End\", \"TypeDeclaration\": \"System.Int32\", \"EntryBlock\": {\"NodeType\": \"Bloc" +
"k\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [{\"NodeType\": \"Local\", \"TypeDeclaration" +
"\": \"System.Int32\", \"LocalName\": \"i\", \"Local\": null}, {\"NodeType\": \"Local\", \"TypeDeclar" +
"ation\": \"System.String\", \"LocalName\": \"result\", \"Local\": null}], \"Formulas\": [{\"NodeType" +
"\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {\"NodeType\": \"" +
"Constant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 10}, \"Left\": {\"NodeType\":" +
" \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"i\", \"VariableIndex\": -1" +
", \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"i\"" +
", \"Local\": null}}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": " +
"\"=\", \"Right\": {\"NodeType\": \"Call\", \"TypeDeclaration\": \"System.String\", \"Instance\": {\"" +
"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"i\", \"Variable" +
"Index\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalN" +
"ame\": \"i\", \"Local\": null}}, \"Method\": \"System.String ToString()\", \"Arguments\": []}, \"Lef" +
"t\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.String\", \"VariableName\": \"result" +
"\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Str" +
"ing\", \"LocalName\": \"result\", \"Local\": null}}}, {\"NodeType\": \"Conditional\", \"TypeDeclarat" +
"ion\": \"System.Void\", \"Test\": {\"NodeType\": \"LessThan\", \"TypeDeclaration\": \"System.Boolean" +
"\", \"Method\": \"<\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int32\"" +
", \"ConstantValue\": 0}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\"" +
", \"VariableName\": \"i\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDec" +
"laration\": \"System.Int32\", \"LocalName\": \"i\", \"Local\": null}}}, \"IfTrue\": {\"NodeType\": \"" +
"Block\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [], \"Formulas\": [{\"NodeType\": \"Ret" +
"urn\", \"TypeDeclaration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Constant\", \"TypeDeclarati" +
"on\": \"System.Int32\", \"ConstantValue\": -1}, \"Label\": null}]}, \"IfFalse\": {\"NodeType\": \"Co" +
"nditional\", \"TypeDeclaration\": \"System.Void\", \"Test\": {\"NodeType\": \"Equal\", \"TypeDeclara" +
"tion\": \"System.Boolean\", \"Method\": \"==\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclar" +
"ation\": \"System.Int32\", \"ConstantValue\": 0}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDecla" +
"ration\": \"System.Int32\", \"VariableName\": \"i\", \"VariableIndex\": -1, \"Resolved\": {\"NodeTyp" +
"e\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"i\", \"Local\": null}}}, \"I" +
"fTrue\": {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.Void\", \"Locals\": [], \"Formulas\"" +
": [{\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Method\": \"=\", \"Right\": {" +
"\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.String\", \"ConstantValue\": 00000000000}," +
" \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.String\", \"VariableName\": \"" +
"result\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Syst" +
"em.String\", \"LocalName\": \"result\", \"Local\": null}}}]}, \"IfFalse\": {\"NodeType\": \"Block\"," +
" \"TypeDeclaration\": \"System.Int32\", \"Locals\": [], \"Formulas\": [{\"NodeType\": \"Return\", \"" +
"TypeDeclaration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"S" +
"ystem.Int32\", \"ConstantValue\": 1}, \"Label\": null}]}}}, {\"NodeType\": \"Return\", \"TypeDeclara" +
"tion\": \"System.Int32\", \"Body\": {\"NodeType\": \"Property\", \"TypeDeclaration\": \"System.Int32" +
"\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.String\", \"VariableNam" +
"e\": \"result\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\":" +
" \"System.String\", \"LocalName\": \"result\", \"Local\": null}}, \"Member\": \"Int32 Length\", \"Me" +
"mber\": \"Int32 Length\"}, \"Label\": null}]}}", gen.DumpEntryPoint());
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
