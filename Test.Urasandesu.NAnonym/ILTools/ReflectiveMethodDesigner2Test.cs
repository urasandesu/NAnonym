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
using SRE = System.Reflection.Emit;

namespace Test.Urasandesu.NAnonym.ILTools
{
    [TestFixture]
    public class ReflectiveMethodDesigner2Test
    {
        /*
         * MEMO: 
         *   The way of making each assertion string: 
         *     1. Output the string of intermediate formula with DumpEntryPoint().
         *     2. Replace /"/ -> /\\"/
         *     3. Replace /(.{1,100})/ -> /"\1" + \r\n/
         *     4. Replace /\\" \+ $/ -> /\\"" + /
         *     5. Replace /^""/ -> /"/
         */

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
                gen.Eval(() => Dsl.If(Dsl.Allocate(value2).As(objValue as int?) != null && value2 < 10));
                {
                    gen.Eval(() => Dsl.Return(value - value * value + (int)value2));
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
"ssign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclara" +
"tion\": \"System.Int32\", \"ConstantValue\": 10}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDecla" +
"ration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"Nod" +
"eType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null" +
"}}, \"Method\": \"=\"}, {\"NodeType\": \"Conditional\", \"TypeDeclaration\": \"System.Int32\", \"Tes" +
"t\": {\"NodeType\": \"AndAlso\", \"TypeDeclaration\": \"System.Boolean\", \"Left\": {\"NodeType\": \"" +
"AndAlso\", \"TypeDeclaration\": \"System.Boolean\", \"Left\": {\"NodeType\": \"AndAlso\", \"TypeDec" +
"laration\": \"System.Boolean\", \"Left\": {\"NodeType\": \"NotEqual\", \"TypeDeclaration\": \"System" +
".Boolean\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"Variable" +
"Name\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\"" +
": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Right\": {\"NodeType\": \"Consta" +
"nt\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 20}, \"Method\": \"!=\", \"Label\": " +
"null}, \"Right\": {\"NodeType\": \"NotEqual\", \"TypeDeclaration\": \"System.Boolean\", \"Left\": {\"" +
"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"Var" +
"iableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"L" +
"ocalName\": \"value\", \"Local\": null}}, \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\"" +
": \"System.Int32\", \"ConstantValue\": 30}, \"Method\": \"!=\", \"Label\": null}, \"Method\": \"&&\"" +
", \"Label\": null}, \"Right\": {\"NodeType\": \"NotEqual\", \"TypeDeclaration\": \"System.Boolean\"" +
", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"" +
"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Syste" +
"m.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Right\": {\"NodeType\": \"Constant\", \"Ty" +
"peDeclaration\": \"System.Int32\", \"ConstantValue\": 40}, \"Method\": \"!=\", \"Label\": null}, \"M" +
"ethod\": \"&&\", \"Label\": null}, \"Right\": {\"NodeType\": \"NotEqual\", \"TypeDeclaration\": \"Sy" +
"stem.Boolean\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"Vari" +
"ableName\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclarat" +
"ion\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Right\": {\"NodeType\": \"Co" +
"nstant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 50}, \"Method\": \"!=\", \"Label" +
"\": null}, \"Method\": \"&&\", \"Label\": null}, \"IfTrue\": {\"NodeType\": \"Block\", \"TypeDeclara" +
"tion\": \"System.Int32\", \"Locals\": [{\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Objec" +
"t\", \"LocalName\": \"objValue\", \"Local\": null}, {\"NodeType\": \"Local\", \"TypeDeclaration\": \"" +
"System.Nullable`1[System.Int32]\", \"LocalName\": \"value2\", \"Local\": null}], \"Formulas\": [{\"" +
"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Convert\"," +
" \"TypeDeclaration\": \"System.Object\", \"Method\": null, \"Operand\": {\"NodeType\": \"Variable\"," +
" \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"L" +
"ocal\": null}}}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Object\", \"Va" +
"riableName\": \"objValue\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDec" +
"laration\": \"System.Object\", \"LocalName\": \"objValue\", \"Local\": null}}, \"Method\": \"=\"}, {" +
"\"NodeType\": \"Conditional\", \"TypeDeclaration\": \"System.Int32\", \"Test\": {\"NodeType\": \"And" +
"Also\", \"TypeDeclaration\": \"System.Boolean\", \"Left\": {\"NodeType\": \"NotEqual\", \"TypeDeclar" +
"ation\": \"System.Boolean\", \"Left\": {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Null" +
"able`1[System.Int32]\", \"Right\": {\"NodeType\": \"TypeAs\", \"TypeDeclaration\": \"System.Nullable" +
"`1[System.Int32]\", \"Method\": null, \"Operand\": {\"NodeType\": \"Variable\", \"TypeDeclaration\":" +
" \"System.Object\", \"VariableName\": \"objValue\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType" +
"\": \"Local\", \"TypeDeclaration\": \"System.Object\", \"LocalName\": \"objValue\", \"Local\": null}" +
"}}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Nullable`1[System.Int32]\"," +
" \"VariableName\": \"value2\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"Type" +
"Declaration\": \"System.Nullable`1[System.Int32]\", \"LocalName\": \"value2\", \"Local\": null}}, \"" +
"Method\": \"=\"}, \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Nullable`1[S" +
"ystem.Int32]\", \"ConstantValue\": null}, \"Method\": \"!=\", \"Label\": null}, \"Right\": {\"NodeTy" +
"pe\": \"LessThan\", \"TypeDeclaration\": \"System.Boolean\", \"Left\": {\"NodeType\": \"Variable\", " +
"\"TypeDeclaration\": \"System.Nullable`1[System.Int32]\", \"VariableName\": \"value2\", \"VariableIn" +
"dex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Nullable`1[System.I" +
"nt32]\", \"LocalName\": \"value2\", \"Local\": null}}, \"Right\": {\"NodeType\": \"Convert\", \"Type" +
"Declaration\": \"System.Nullable`1[System.Int32]\", \"Method\": null, \"Operand\": {\"NodeType\": \"" +
"Constant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 10}}, \"Method\": \"<\", \"Lab" +
"el\": null}, \"Method\": \"&&\", \"Label\": null}, \"IfTrue\": {\"NodeType\": \"Block\", \"TypeDecla" +
"ration\": \"System.Int32\", \"Locals\": [], \"Formulas\": [{\"NodeType\": \"Return\", \"TypeDeclarat" +
"ion\": \"System.Int32\", \"Body\": {\"NodeType\": \"Add\", \"TypeDeclaration\": \"System.Int32\", \"" +
"Left\": {\"NodeType\": \"Subtract\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\":" +
" \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\"" +
": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": " +
"\"value\", \"Local\": null}}, \"Right\": {\"NodeType\": \"Multiply\", \"TypeDeclaration\": \"System." +
"Int32\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableNam" +
"e\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": " +
"\"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Right\": {\"NodeType\": \"Variable\"" +
", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Reso" +
"lved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"" +
"Local\": null}}, \"Method\": \"*\"}, \"Method\": \"-\"}, \"Right\": {\"NodeType\": \"Convert\", \"T" +
"ypeDeclaration\": \"System.Int32\", \"Method\": null, \"Operand\": {\"NodeType\": \"Variable\", \"Ty" +
"peDeclaration\": \"System.Nullable`1[System.Int32]\", \"VariableName\": \"value2\", \"VariableIndex\"" +
": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Nullable`1[System.Int32" +
"]\", \"LocalName\": \"value2\", \"Local\": null}}}, \"Method\": \"+\"}, \"Label\": null}]}, \"IfFals" +
"e\": {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [], \"Formulas\": " +
"[{\"NodeType\": \"Return\", \"TypeDeclaration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Add\"," +
" \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"" +
"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Righ" +
"t\": {\"NodeType\": \"Multiply\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"" +
"Multiply\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDecl" +
"aration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"No" +
"deType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": nul" +
"l}}, \"Right\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\"" +
": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"S" +
"ystem.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Method\": \"*\"}, \"Right\": {\"NodeTy" +
"pe\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIn" +
"dex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalNam" +
"e\": \"value\", \"Local\": null}}, \"Method\": \"*\"}, \"Method\": \"+\"}, \"Label\": null}]}, \"Lab" +
"el1\": null, \"Label2\": null}]}, \"IfFalse\": {\"NodeType\": \"Conditional\", \"TypeDeclaration\": " +
"\"System.Int32\", \"Test\": {\"NodeType\": \"Equal\", \"TypeDeclaration\": \"System.Boolean\", \"Lef" +
"t\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\"" +
", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32" +
"\", \"LocalName\": \"value\", \"Local\": null}}, \"Right\": {\"NodeType\": \"Constant\", \"TypeDecla" +
"ration\": \"System.Int32\", \"ConstantValue\": 20}, \"Method\": \"==\", \"Label\": null}, \"IfTrue\"" +
": {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [], \"Formulas\": [{\"" +
"NodeType\": \"Return\", \"TypeDeclaration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Variable\"" +
", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Reso" +
"lved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"" +
"Local\": null}}, \"Label\": null}]}, \"IfFalse\": {\"NodeType\": \"Conditional\", \"TypeDeclaration" +
"\": \"System.Int32\", \"Test\": {\"NodeType\": \"Equal\", \"TypeDeclaration\": \"System.Boolean\", \"" +
"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"val" +
"ue\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.I" +
"nt32\", \"LocalName\": \"value\", \"Local\": null}}, \"Right\": {\"NodeType\": \"Constant\", \"TypeD" +
"eclaration\": \"System.Int32\", \"ConstantValue\": 40}, \"Method\": \"==\", \"Label\": null}, \"IfTr" +
"ue\": {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [], \"Formulas\":" +
" [{\"NodeType\": \"Return\", \"TypeDeclaration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Exclu" +
"siveOr\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"ExclusiveOr\", \"TypeDec" +
"laration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.I" +
"nt32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", " +
"\"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Right\": {\"No" +
"deType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"Variab" +
"leIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"Loca" +
"lName\": \"value\", \"Local\": null}}, \"Method\": \"^\"}, \"Right\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Resolved" +
"\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Loc" +
"al\": null}}, \"Method\": \"^\"}, \"Label\": null}]}, \"IfFalse\": {\"NodeType\": \"Block\", \"TypeD" +
"eclaration\": \"System.Int32\", \"Locals\": [], \"Formulas\": [{\"NodeType\": \"Return\", \"TypeDecl" +
"aration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Conditional\", \"TypeDeclaration\": \"System" +
".Int32\", \"Test\": {\"NodeType\": \"Equal\", \"TypeDeclaration\": \"System.Boolean\", \"Left\": {\"" +
"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"Vari" +
"ableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"Lo" +
"calName\": \"value\", \"Local\": null}}, \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\"" +
": \"System.Int32\", \"ConstantValue\": 30}, \"Method\": \"==\", \"Label\": null}, \"IfTrue\": {\"Nod" +
"eType\": \"Add\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable\", \"Ty" +
"peDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Resolved\":" +
" {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\"" +
": null}}, \"Right\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"Variable" +
"Name\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\"" +
": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Method\": \"+\"}, \"IfFalse\": {" +
"\"NodeType\": \"Multiply\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variab" +
"le\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"R" +
"esolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\"" +
", \"Local\": null}}, \"Right\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", " +
"\"VariableName\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Method\": \"*\"}, \"La" +
"bel1\": null, \"Label2\": null}, \"Label\": null}]}, \"Label1\": null, \"Label2\": null}, \"Label1\"" +
": null, \"Label2\": null}, \"Label1\": null, \"Label2\": null}]}}", gen.DumpEntryPoint());
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
": \"Void WriteLog(System.String, System.Object[])\", \"Arguments\": [{\"NodeType\": \"Constant\", \"" +
"TypeDeclaration\": \"System.String\", \"ConstantValue\": \"testtest\"}, {\"NodeType\": \"NewArrayIni" +
"t\", \"TypeDeclaration\": \"System.Object[]\", \"Formulas\": []}]}, {\"NodeType\": \"Assign\", \"Typ" +
"eDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"ReflectiveNew\", \"TypeDeclaration\": \"" +
"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"Constructor\": \"Void .ctor()\", \"Arguments\":" +
" []}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.Prop" +
"ertyTestClass1\", \"VariableName\": \"p1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Lo" +
"cal\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\"" +
", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"ReflectiveCall\", \"TypeDeclaration\": \"" +
"System.Void\", \"Instance\": null, \"Method\": \"Void WriteLog(System.String, System.Object[])\", \"" +
"Arguments\": [{\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.String\", \"ConstantValue\"" +
": \"{0}\"}, {\"NodeType\": \"NewArrayInit\", \"TypeDeclaration\": \"System.Object[]\", \"Formulas\":" +
" [{\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1" +
"\", \"VariableName\": \"p1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeD" +
"eclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", \"Local\": " +
"null}}]}]}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\"" +
": \"Constant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 10}, \"Left\": {\"NodeTyp" +
"e\": \"ReflectiveProperty\", \"TypeDeclaration\": \"System.Int32\", \"Instance\": {\"NodeType\": \"V" +
"ariable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"VariableName\"" +
": \"p1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test" +
".Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", \"Local\": null}}, \"Member\": " +
"\"Int32 ValueProperty\", \"Member\": \"Int32 ValueProperty\"}, \"Method\": \"=\"}, {\"NodeType\": \"" +
"ReflectiveCall\", \"TypeDeclaration\": \"System.Void\", \"Instance\": null, \"Method\": \"Void Write" +
"Log(System.String, System.Object[])\", \"Arguments\": [{\"NodeType\": \"Constant\", \"TypeDeclaratio" +
"n\": \"System.String\", \"ConstantValue\": \"ValueProperty: {0}\"}, {\"NodeType\": \"NewArrayInit\"," +
" \"TypeDeclaration\": \"System.Object[]\", \"Formulas\": [{\"NodeType\": \"Convert\", \"TypeDeclarat" +
"ion\": \"System.Object\", \"Method\": null, \"Operand\": {\"NodeType\": \"ReflectiveProperty\", \"Ty" +
"peDeclaration\": \"System.Int32\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"" +
"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"VariableName\": \"p1\", \"VariableIndex\": -1, " +
"\"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyT" +
"estClass1\", \"LocalName\": \"p1\", \"Local\": null}}, \"Member\": \"Int32 ValueProperty\", \"Member" +
"\": \"Int32 ValueProperty\"}}]}]}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.String\", \"ConstantValue\": \"" +
"a\"}, \"Left\": {\"NodeType\": \"ReflectiveProperty\", \"TypeDeclaration\": \"System.String\", \"In" +
"stance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTe" +
"stClass1\", \"VariableName\": \"p1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\"," +
" \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", \"L" +
"ocal\": null}}, \"Member\": \"System.String ObjectProperty\", \"Member\": \"System.String ObjectProp" +
"erty\"}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Righ" +
"t\": {\"NodeType\": \"ReflectiveProperty\", \"TypeDeclaration\": \"System.String\", \"Instance\": {\"" +
"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", " +
"\"VariableName\": \"p1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDecla" +
"ration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", \"Local\": null" +
"}}, \"Member\": \"System.String ObjectProperty\", \"Member\": \"System.String ObjectProperty\"}, \"L" +
"eft\": {\"NodeType\": \"ReflectiveProperty\", \"TypeDeclaration\": \"System.String\", \"Instance\": " +
"{\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\"" +
", \"VariableName\": \"p1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDec" +
"laration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", \"Local\": nu" +
"ll}}, \"Member\": \"System.String ObjectProperty\", \"Member\": \"System.String ObjectProperty\"}, \"" +
"Method\": \"=\"}, {\"NodeType\": \"ReflectiveCall\", \"TypeDeclaration\": \"System.Void\", \"Instan" +
"ce\": null, \"Method\": \"Void WriteLog(System.String, System.Object[])\", \"Arguments\": [{\"NodeTy" +
"pe\": \"Constant\", \"TypeDeclaration\": \"System.String\", \"ConstantValue\": \"ObjectProperty: {0}" +
"\"}, {\"NodeType\": \"NewArrayInit\", \"TypeDeclaration\": \"System.Object[]\", \"Formulas\": [{\"No" +
"deType\": \"ReflectiveProperty\", \"TypeDeclaration\": \"System.String\", \"Instance\": {\"NodeType\"" +
": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"Variable" +
"Name\": \"p1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": " +
"\"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", \"Local\": null}}, \"Memb" +
"er\": \"System.String ObjectProperty\", \"Member\": \"System.String ObjectProperty\"}]}]}, {\"NodeTy" +
"pe\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"New\", \"TypeDec" +
"laration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"Constructor\": \"Void .ctor()\", \"Ar" +
"guments\": []}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnony" +
"m.Etc.FieldTestClass2\", \"VariableName\": \"f2\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\"" +
": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"LocalName\": \"" +
"f2\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Syst" +
"em.Void\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int32\", \"Constant" +
"Value\": 30}, \"Left\": {\"NodeType\": \"ReflectiveField\", \"TypeDeclaration\": \"System.Int32\", \"" +
"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTe" +
"stClass2\", \"VariableName\": \"f2\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\"," +
" \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"LocalName\": \"f2\", \"Loca" +
"l\": null}}, \"Member\": \"Int32 ValueField\", \"Member\": \"Int32 ValueField\"}, \"Method\": \"=\"}" +
", {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Refle" +
"ctiveField\", \"TypeDeclaration\": \"System.Int32\", \"Instance\": {\"NodeType\": \"Variable\", \"Ty" +
"peDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"VariableName\": \"f2\", \"Variab" +
"leIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnony" +
"m.Etc.FieldTestClass2\", \"LocalName\": \"f2\", \"Local\": null}}, \"Member\": \"Int32 ValueField\"," +
" \"Member\": \"Int32 ValueField\"}, \"Left\": {\"NodeType\": \"ReflectiveField\", \"TypeDeclaration\"" +
": \"System.Int32\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasande" +
"su.NAnonym.Etc.FieldTestClass2\", \"VariableName\": \"f2\", \"VariableIndex\": -1, \"Resolved\": {\"" +
"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"Local" +
"Name\": \"f2\", \"Local\": null}}, \"Member\": \"Int32 ValueField\", \"Member\": \"Int32 ValueField\"" +
"}, \"Method\": \"=\"}, {\"NodeType\": \"Call\", \"TypeDeclaration\": \"System.Void\", \"Instance\":" +
" null, \"Method\": \"Void WriteLog(System.String, System.Object[])\", \"Arguments\": [{\"NodeType\":" +
" \"Constant\", \"TypeDeclaration\": \"System.String\", \"ConstantValue\": \"ValueField: {0}\"}, {\"N" +
"odeType\": \"NewArrayInit\", \"TypeDeclaration\": \"System.Object[]\", \"Formulas\": [{\"NodeType\":" +
" \"Convert\", \"TypeDeclaration\": \"System.Object\", \"Method\": null, \"Operand\": {\"NodeType\": " +
"\"ReflectiveField\", \"TypeDeclaration\": \"System.Int32\", \"Instance\": {\"NodeType\": \"Variable\"" +
", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"VariableName\": \"f2\", \"" +
"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu" +
".NAnonym.Etc.FieldTestClass2\", \"LocalName\": \"f2\", \"Local\": null}}, \"Member\": \"Int32 ValueF" +
"ield\", \"Member\": \"Int32 ValueField\"}}]}]}, {\"NodeType\": \"ReflectiveCall\", \"TypeDeclaration" +
"\": \"System.Void\", \"Instance\": null, \"Method\": \"Void WriteLog(System.String, System.Object[])" +
"\", \"Arguments\": [{\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.String\", \"ConstantV" +
"alue\": \"ValueField: {0}\"}, {\"NodeType\": \"NewArrayInit\", \"TypeDeclaration\": \"System.Object[" +
"]\", \"Formulas\": [{\"NodeType\": \"Convert\", \"TypeDeclaration\": \"System.Object\", \"Method\": " +
"null, \"Operand\": {\"NodeType\": \"ReflectiveField\", \"TypeDeclaration\": \"System.Int32\", \"Inst" +
"ance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestCla" +
"ss2\", \"VariableName\": \"f2\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"Ty" +
"peDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"LocalName\": \"f2\", \"Local\": " +
"null}}, \"Member\": \"Int32 ValueField\", \"Member\": \"Int32 ValueField\"}}]}]}, {\"NodeType\": \"A" +
"ssign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"ReflectiveNew\", \"TypeDe" +
"claration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass2\", \"Constructor\": \"Void .ctor(Int32" +
", System.String)\", \"Arguments\": [{\"NodeType\": \"ReflectiveProperty\", \"TypeDeclaration\": \"Sy" +
"stem.Int32\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAno" +
"nym.Etc.PropertyTestClass1\", \"VariableName\": \"p1\", \"VariableIndex\": -1, \"Resolved\": {\"Node" +
"Type\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalN" +
"ame\": \"p1\", \"Local\": null}}, \"Member\": \"Int32 ValueProperty\", \"Member\": \"Int32 ValueProp" +
"erty\"}, {\"NodeType\": \"ReflectiveProperty\", \"TypeDeclaration\": \"System.String\", \"Instance\"" +
": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1" +
"\", \"VariableName\": \"p1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeD" +
"eclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", \"Local\": " +
"null}}, \"Member\": \"System.String ObjectProperty\", \"Member\": \"System.String ObjectProperty\"}]" +
"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.Propert" +
"yTestClass2\", \"VariableName\": \"p2\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local" +
"\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass2\", \"LocalName\": \"p2\", " +
"\"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"ReflectiveCall\", \"TypeDeclaration\": \"Sys" +
"tem.Void\", \"Instance\": null, \"Method\": \"Void WriteLog(System.String, System.Object[])\", \"Arg" +
"uments\": [{\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.String\", \"ConstantValue\": \"" +
"({0}, {1})\"}, {\"NodeType\": \"NewArrayInit\", \"TypeDeclaration\": \"System.Object[]\", \"Formula" +
"s\": [{\"NodeType\": \"Convert\", \"TypeDeclaration\": \"System.Object\", \"Method\": null, \"Operan" +
"d\": {\"NodeType\": \"Property\", \"TypeDeclaration\": \"System.Int32\", \"Instance\": {\"NodeType\"" +
": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass2\", \"VariableN" +
"ame\": \"p2\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"" +
"Test.Urasandesu.NAnonym.Etc.PropertyTestClass2\", \"LocalName\": \"p2\", \"Local\": null}}, \"Membe" +
"r\": \"Int32 ValueProperty\", \"Member\": \"Int32 ValueProperty\"}}, {\"NodeType\": \"Property\", \"" +
"TypeDeclaration\": \"System.String\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\"" +
": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass2\", \"VariableName\": \"p2\", \"VariableIndex\": -" +
"1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.Proper" +
"tyTestClass2\", \"LocalName\": \"p2\", \"Local\": null}}, \"Member\": \"System.String ObjectProperty" +
"\", \"Member\": \"System.String ObjectProperty\"}]}]}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Right\": {\"NodeType\": \"ReflectiveCall\", \"TypeDeclaration\": \"System.Int3" +
"2\", \"Instance\": null, \"Method\": \"Int32 GetValue(Int32)\", \"Arguments\": [{\"NodeType\": \"Ref" +
"lectiveField\", \"TypeDeclaration\": \"System.Int32\", \"Instance\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"VariableName\": \"f2\", \"Vari" +
"ableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAno" +
"nym.Etc.FieldTestClass2\", \"LocalName\": \"f2\", \"Local\": null}}, \"Member\": \"Int32 ValueField\"" +
", \"Member\": \"Int32 ValueField\"}]}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"" +
"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Meth" +
"od\": \"=\"}]}}", gen.DumpEntryPoint());
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
": [{\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Fiel" +
"d\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"U" +
"rasandesu.NAnonym.ILTools.OpCode Add\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add\"}, \"Le" +
"ft\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Var" +
"iableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDecla" +
"ration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Me" +
"thod\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeT" +
"ype\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"" +
"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.Op" +
"Code Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.IL" +
"Tools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"" +
"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"" +
"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\"" +
", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasande" +
"su.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolve" +
"d\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalNa" +
"me\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclarati" +
"on\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnony" +
"m.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"" +
"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableI" +
"ndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assi" +
"gn\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Uras" +
"andesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, " +
"{\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\"" +
", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Uras" +
"andesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}," +
" \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", " +
"\"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"Type" +
"Declaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}," +
" \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"" +
"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": nul" +
"l, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILToo" +
"ls.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnon" +
"ym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType" +
"\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\"" +
", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.V" +
"oid\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode" +
"\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Ura" +
"sandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Re" +
"solved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Lo" +
"calName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDecl" +
"aration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf" +
"\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\"" +
", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Vari" +
"ableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.I" +
"LTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"" +
"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclarat" +
"ion\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.I" +
"LTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"No" +
"deType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\"" +
": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=" +
"\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Fi" +
"eld\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ov" +
"f\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": nu" +
"ll}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\"" +
": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\"" +
": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym." +
"ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu." +
"NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"Nod" +
"eType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opc" +
"ode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Sys" +
"tem.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.O" +
"pCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclara" +
"tion\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1," +
" \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"," +
" \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"Typ" +
"eDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasand" +
"esu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Ad" +
"d_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Vari" +
"able\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"" +
"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAno" +
"nym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType" +
"\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDec" +
"laration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAno" +
"nym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": " +
"{\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableN" +
"ame\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration" +
"\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\"" +
": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\":" +
" \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member" +
"\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode A" +
"dd_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools." +
"OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local" +
"\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\"" +
": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Ri" +
"ght\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Insta" +
"nce\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAn" +
"onym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasan" +
"desu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {" +
"\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": " +
"\"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": " +
"\"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTo" +
"ols.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Membe" +
"r\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\"" +
": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", " +
"\"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"" +
"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1" +
"\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu" +
".NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"Nod" +
"eType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"Ty" +
"peDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu" +
".NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Lef" +
"t\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Vari" +
"ableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclar" +
"ation\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Met" +
"hod\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeTy" +
"pe\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"M" +
"ember\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpC" +
"ode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"" +
"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"L" +
"ocal\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\"," +
" \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandes" +
"u.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"U" +
"rasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved" +
"\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalNam" +
"e\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaratio" +
"n\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"" +
"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"T" +
"ypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIn" +
"dex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools" +
".OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assig" +
"n\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\":" +
" \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools" +
".OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType" +
"\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"op" +
"code1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {" +
"\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\"," +
" \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeD" +
"eclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, " +
"\"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"N" +
"odeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null" +
", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnony" +
"m.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\"" +
": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\"" +
", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Vo" +
"id\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"" +
", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Uras" +
"andesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Res" +
"olved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Loc" +
"alName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDecla" +
"ration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NA" +
"nonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"" +
", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\"" +
", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Varia" +
"bleIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.IL" +
"Tools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"" +
"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclarati" +
"on\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.IL" +
"Tools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"Nod" +
"eType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\":" +
" \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"" +
"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Fie" +
"ld\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf" +
"\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode" +
"\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": nul" +
"l}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\":" +
" {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\":" +
" null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.I" +
"LTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"Node" +
"Type\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opco" +
"de1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Syst" +
"em.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.Op" +
"Code\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclarat" +
"ion\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, " +
"\"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", " +
"\"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"Type" +
"Declaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasande" +
"su.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add" +
"_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Varia" +
"ble\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"" +
"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnon" +
"ym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\"" +
": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDecl" +
"aration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnon" +
"ym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {" +
"\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableNa" +
"me\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\":" +
" \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": " +
"\"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Ad" +
"d_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.O" +
"pCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\"" +
", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\"" +
": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Rig" +
"ht\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instan" +
"ce\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAno" +
"nym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasand" +
"esu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"" +
"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"" +
"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"" +
"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILToo" +
"ls.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member" +
"\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDec" +
"laration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\":" +
" -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"" +
"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Ura" +
"sandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"" +
"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\"" +
", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu." +
"NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"Node" +
"Type\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left" +
"\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Varia" +
"bleName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclara" +
"tion\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Meth" +
"od\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeTyp" +
"e\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Me" +
"mber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTo" +
"ols.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"L" +
"ocal\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Lo" +
"cal\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", " +
"\"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"I" +
"nstance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu" +
".NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\"" +
": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName" +
"\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration" +
"\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym." +
"ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"M" +
"ember\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"Ty" +
"peDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableInd" +
"ex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools." +
"OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign" +
"\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools." +
"OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\"" +
": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opc" +
"ode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasan" +
"desu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"" +
"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", " +
"\"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasan" +
"desu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"" +
"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"No" +
"deType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null," +
" \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools" +
".OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\"" +
": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\"," +
" \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Voi" +
"d\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"" +
", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\":" +
" \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Reso" +
"lved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Loca" +
"lName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclar" +
"ation\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAn" +
"onym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"" +
", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\"," +
" \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variab" +
"leIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"A" +
"ssign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaratio" +
"n\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"Node" +
"Type\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": " +
"\"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"U" +
"rasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"" +
"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Fiel" +
"d\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"U" +
"rasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"" +
"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"" +
", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"T" +
"ypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null" +
"}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": " +
"{\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": " +
"null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.IL" +
"Tools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NA" +
"nonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeT" +
"ype\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcod" +
"e1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Syste" +
"m.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpC" +
"ode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclarati" +
"on\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"" +
"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeD" +
"eclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandes" +
"u.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_" +
"Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variab" +
"le\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"V" +
"ariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnony" +
"m.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\"" +
": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDecla" +
"ration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnony" +
"m.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"" +
"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableNam" +
"e\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": " +
"\"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"" +
"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add" +
"_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.Op" +
"Code\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\"" +
", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\":" +
" null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Righ" +
"t\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instanc" +
"e\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnon" +
"ym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasande" +
"su.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"" +
"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"" +
"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"" +
"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDecl" +
"aration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": " +
"-1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode" +
"\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"" +
"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Uras" +
"andesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode" +
" Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"V" +
"ariable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\"" +
", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeT" +
"ype\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"Type" +
"Declaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\"" +
": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Variab" +
"leName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclarat" +
"ion\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Metho" +
"d\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType" +
"\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILToo" +
"ls.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Lo" +
"cal\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Loc" +
"al\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"In" +
"stance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu." +
"NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Ura" +
"sandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\"" +
": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\"" +
": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.I" +
"LTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Me" +
"mber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableInde" +
"x\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.O" +
"pCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.O" +
"pCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\"" +
": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opco" +
"de1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasand" +
"esu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"" +
"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasand" +
"esu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"" +
"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"V" +
"ariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDec" +
"laration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"" +
"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"Nod" +
"eType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, " +
"\"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools." +
"OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym." +
"ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", " +
"\"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void" +
"\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"," +
" \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasan" +
"desu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resol" +
"ved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Local" +
"Name\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclara" +
"tion\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAno" +
"nym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"," +
" \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", " +
"\"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variabl" +
"eIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTo" +
"ols.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"As" +
"sign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration" +
"\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTo" +
"ols.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeT" +
"ype\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"" +
"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}" +
", {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field" +
"\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"" +
"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"" +
", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"Ty" +
"peDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}" +
"}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {" +
"\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": n" +
"ull, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAn" +
"onym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeTy" +
"pe\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode" +
"1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System" +
".Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"U" +
"rasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaratio" +
"n\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"" +
"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDe" +
"claration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu" +
".NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_O" +
"vf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variabl" +
"e\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Va" +
"riableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\":" +
" \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclar" +
"ation\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym" +
".ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"" +
"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName" +
"\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\":" +
" \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"" +
"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"" +
"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\":" +
" \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_" +
"Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpC" +
"ode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\"," +
" \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": " +
"null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right" +
"\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance" +
"\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnony" +
"m.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandes" +
"u.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"N" +
"odeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"o" +
"pcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"S" +
"ystem.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools" +
".OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDecla" +
"ration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -" +
"1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"" +
", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"T" +
"ypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode " +
"Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Va" +
"riable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\"," +
" \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NA" +
"nonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeTy" +
"pe\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeD" +
"eclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NA" +
"nonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\"" +
": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Variabl" +
"eName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclarati" +
"on\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method" +
"\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\"" +
": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode" +
" Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Loc" +
"al\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Loca" +
"l\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"" +
"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Ins" +
"tance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Uras" +
"andesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\":" +
" {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\"" +
": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\"" +
": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.IL" +
"Tools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Mem" +
"ber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"Type" +
"Declaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex" +
"\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.Op" +
"Code\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\"" +
", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.Op" +
"Code Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\":" +
" \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcod" +
"e1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasande" +
"su.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"N" +
"odeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasande" +
"su.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"L" +
"eft\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Va" +
"riableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDecl" +
"aration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"M" +
"ethod\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"Node" +
"Type\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"" +
"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.O" +
"pCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.I" +
"LTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": " +
"\"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"" +
"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\"" +
", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", " +
"\"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasand" +
"esu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalN" +
"ame\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclarat" +
"ion\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnon" +
"ym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", " +
"\"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILToo" +
"ls.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Ass" +
"ign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILToo" +
"ls.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeTy" +
"pe\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"" +
"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Ura" +
"sandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}," +
" {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\"" +
", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Ura" +
"sandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}" +
", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"," +
" \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}" +
", \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"" +
"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": nu" +
"ll, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTo" +
"ols.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAno" +
"nym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeTyp" +
"e\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1" +
"\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System." +
"Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration" +
"\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"R" +
"esolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"L" +
"ocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDec" +
"laration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu." +
"NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ov" +
"f\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable" +
"\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Var" +
"iableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym." +
"ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": " +
"\"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclara" +
"tion\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym." +
"ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"N" +
"odeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\"" +
": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"" +
"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"F" +
"ield\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_O" +
"vf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCo" +
"de\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", " +
"\"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": n" +
"ull}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\"" +
": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\"" +
": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym" +
".ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu" +
".NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"No" +
"deType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"op" +
"code1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Sy" +
"stem.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools." +
"OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\":" +
" \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclar" +
"ation\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1" +
", \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"" +
", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"Ty" +
"peDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasan" +
"desu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode A" +
"dd_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Var" +
"iable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", " +
"\"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAn" +
"onym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeTyp" +
"e\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAn" +
"onym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\":" +
" {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Variable" +
"Name\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaratio" +
"n\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\"" +
": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\"" +
": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Membe" +
"r\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode " +
"Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools" +
".OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Loca" +
"l\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local" +
"\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"R" +
"ight\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Inst" +
"ance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NA" +
"nonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": " +
"{\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\":" +
" \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\":" +
" \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Memb" +
"er\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": \"Variable\", \"TypeD" +
"eclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\"" +
": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpC" +
"ode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\"," +
" \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"U" +
"rasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpC" +
"ode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Left\": {\"NodeType\": " +
"\"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode" +
"1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Urasandes" +
"u.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Method\": \"=\"}, {\"No" +
"deType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Field\", \"T" +
"ypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandes" +
"u.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}, \"Le" +
"ft\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Var" +
"iableName\": \"opcode1\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDecla" +
"ration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"LocalName\": \"opcode1\", \"Local\": null}}, \"Me" +
"thod\": \"=\"}]}}", gen.DumpEntryPoint());
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
"n\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration" +
"\": \"System.Int32\", \"ConstantValue\": 10}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclarati" +
"on\": \"System.Int32\", \"VariableName\": \"i\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\":" +
" \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"i\", \"Local\": null}}, \"Method" +
"\": \"=\"}, {\"NodeType\": \"Conditional\", \"TypeDeclaration\": \"System.Int32\", \"Test\": {\"Node" +
"Type\": \"LessThan\", \"TypeDeclaration\": \"System.Boolean\", \"Left\": {\"NodeType\": \"Variable\"" +
", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"i\", \"VariableIndex\": -1, \"Resolved\"" +
": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"i\", \"Local\":" +
" null}}, \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantVa" +
"lue\": 0}, \"Method\": \"<\", \"Label\": null}, \"IfTrue\": {\"NodeType\": \"Block\", \"TypeDeclarat" +
"ion\": \"System.Int32\", \"Locals\": [], \"Formulas\": [{\"NodeType\": \"Return\", \"TypeDeclaration" +
"\": \"System.Int32\", \"Body\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int32\", " +
"\"ConstantValue\": -1}, \"Label\": null}]}, \"IfFalse\": {\"NodeType\": \"Conditional\", \"TypeDecla" +
"ration\": \"System.Int32\", \"Test\": {\"NodeType\": \"Equal\", \"TypeDeclaration\": \"System.Boolea" +
"n\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\":" +
" \"i\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System" +
".Int32\", \"LocalName\": \"i\", \"Local\": null}}, \"Right\": {\"NodeType\": \"Constant\", \"TypeDec" +
"laration\": \"System.Int32\", \"ConstantValue\": 0}, \"Method\": \"==\", \"Label\": null}, \"IfTrue\"" +
": {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [], \"Formulas\": [{" +
"\"NodeType\": \"Return\", \"TypeDeclaration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Constant" +
"\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 0}, \"Label\": null}]}, \"IfFalse\": {" +
"\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [], \"Formulas\": [{\"No" +
"deType\": \"Return\", \"TypeDeclaration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Constant\", " +
"\"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 1}, \"Label\": null}]}, \"Label1\": null, " +
"\"Label2\": null}, \"Label1\": null, \"Label2\": null}]}}", gen.DumpEntryPoint());
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
"\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Constant\", \"Type" +
"Declaration\": \"System.Int32\", \"ConstantValue\": 10}, \"Left\": {\"NodeType\": \"Variable\", \"Ty" +
"peDeclaration\": \"System.Int32\", \"VariableName\": \"i\", \"VariableIndex\": -1, \"Resolved\": {\"" +
"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"i\", \"Local\": null}" +
"}, \"Method\": \"=\"}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {" +
"\"NodeType\": \"Call\", \"TypeDeclaration\": \"System.String\", \"Instance\": {\"NodeType\": \"Varia" +
"ble\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"i\", \"VariableIndex\": -1, \"Reso" +
"lved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"i\", \"Loc" +
"al\": null}}, \"Method\": \"System.String ToString()\", \"Arguments\": []}, \"Left\": {\"NodeType\":" +
" \"Variable\", \"TypeDeclaration\": \"System.String\", \"VariableName\": \"result\", \"VariableIndex" +
"\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.String\", \"LocalName\"" +
": \"result\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Conditional\", \"TypeDeclarat" +
"ion\": \"System.Void\", \"Test\": {\"NodeType\": \"LessThan\", \"TypeDeclaration\": \"System.Boolean" +
"\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": " +
"\"i\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System." +
"Int32\", \"LocalName\": \"i\", \"Local\": null}}, \"Right\": {\"NodeType\": \"Constant\", \"TypeDecl" +
"aration\": \"System.Int32\", \"ConstantValue\": 0}, \"Method\": \"<\", \"Label\": null}, \"IfTrue\":" +
" {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [], \"Formulas\": [{\"" +
"NodeType\": \"Return\", \"TypeDeclaration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Constant\"" +
", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": -1}, \"Label\": null}]}, \"IfFalse\": {\"" +
"NodeType\": \"Conditional\", \"TypeDeclaration\": \"System.Void\", \"Test\": {\"NodeType\": \"Equal" +
"\", \"TypeDeclaration\": \"System.Boolean\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaratio" +
"n\": \"System.Int32\", \"VariableName\": \"i\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": " +
"\"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"i\", \"Local\": null}}, \"Right\"" +
": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 0}, \"Metho" +
"d\": \"==\", \"Label\": null}, \"IfTrue\": {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.V" +
"oid\", \"Locals\": [], \"Formulas\": [{\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\"" +
", \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.String\", \"ConstantValue\"" +
": 00000000000}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.String\", \"Var" +
"iableName\": \"result\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclar" +
"ation\": \"System.String\", \"LocalName\": \"result\", \"Local\": null}}, \"Method\": \"=\"}]}, \"If" +
"False\": {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [], \"Formulas" +
"\": [{\"NodeType\": \"Return\", \"TypeDeclaration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Co" +
"nstant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 1}, \"Label\": null}]}, \"Label1" +
"\": null, \"Label2\": null}, \"Label1\": null, \"Label2\": null}, {\"NodeType\": \"Return\", \"TypeD" +
"eclaration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Property\", \"TypeDeclaration\": \"System" +
".Int32\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.String\", \"Varia" +
"bleName\": \"result\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclarat" +
"ion\": \"System.String\", \"LocalName\": \"result\", \"Local\": null}}, \"Member\": \"Int32 Length\"" +
", \"Member\": \"Int32 Length\"}, \"Label\": null}]}}", gen.DumpEntryPoint());
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
            //Console.WriteLine(gen.DumpEntryPoint());
            #region Assertion
            Assert.AreEqual("{\"NodeType\": \"End\", \"TypeDeclaration\": \"System.Int32\", \"EntryBlock\": {\"NodeType\": \"Bloc" +
"k\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [{\"NodeType\": \"Local\", \"TypeDeclaration" +
"\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}], \"Formulas\": [{\"NodeType\": \"A" +
"ssign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Variable\", \"TypeDeclara" +
"tion\": \"System.Int32\", \"VariableName\": null, \"VariableIndex\": 0, \"Resolved\": {\"NodeType\":" +
" \"Argument\", \"TypeDeclaration\": \"System.Int32\", \"ArgumentName\": null, \"ArgumentPosition\": " +
"0, \"Argument\": null}}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\"" +
", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"Typ" +
"eDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Method\": \"=\"}, {" +
"\"NodeType\": \"Return\", \"TypeDeclaration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Add\", \"" +
"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"" +
"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"L" +
"ocal\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Right\"" +
": {\"NodeType\": \"Multiply\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Mu" +
"ltiply\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclar" +
"ation\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": -1, \"Resolved\": {\"Node" +
"Type\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}" +
"}, \"Right\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": " +
"\"value\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Sys" +
"tem.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Method\": \"*\"}, \"Right\": {\"NodeType" +
"\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableInde" +
"x\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\"" +
": \"value\", \"Local\": null}}, \"Method\": \"*\"}, \"Method\": \"+\"}, \"Label\": null}]}}", gen.DumpEntryPoint());
            #endregion
        }

        [Test]
        public void EvalTest07_InternalStatememts()
        {
            var gen = new ReflectiveMethodDesigner2();
            var methodGen = new MockMethodBaseGenerator();
            var returnType = new MockTypeGenerator(typeof(string).ToTypeDecl());
            gen.ILBuilder = new EmptyILBuilder(methodGen, returnType);
            var dm = default(SRE::DynamicMethod);
            gen.Eval(() => Dsl.Allocate(dm).As(new SRE::DynamicMethod("DynamicMethod", typeof(string), null, true)));

            var il = default(SRE::ILGenerator);
            gen.Eval(() => Dsl.Allocate(il).As(dm.GetILGenerator()));

            gen.ExpressInternally(() => il, typeof(string).ToTypeDecl(), null,
            _gen =>
            {
                var f1StaticObjectField = typeof(FieldTestClass1).GetFieldStaticNonPublic("staticObjectField");
                _gen.Eval(() => f1StaticObjectField.SetValue(null, "testtest"));
                _gen.Eval(() => Dsl.Return(f1StaticObjectField.GetValue(null)));
            });

            var func = default(Func<string>);
            gen.Eval(() => Dsl.Allocate(func).As((Func<string>)dm.CreateDelegate(typeof(Func<string>))));
            gen.Eval(() => Dsl.Return(func()));
            gen.Eval(() => Dsl.End());
            //Console.WriteLine(gen.DumpEntryPoint());
            #region Assertion
            Assert.AreEqual("{\"NodeType\": \"End\", \"TypeDeclaration\": \"System.String\", \"EntryBlock\": {\"NodeType\": \"Blo" +
"ck\", \"TypeDeclaration\": \"System.String\", \"Locals\": [{\"NodeType\": \"Local\", \"TypeDeclarati" +
"on\": \"System.Reflection.Emit.DynamicMethod\", \"LocalName\": \"dm\", \"Local\": null}, {\"NodeType" +
"\": \"Local\", \"TypeDeclaration\": \"System.Reflection.Emit.ILGenerator\", \"LocalName\": \"il\", \"" +
"Local\": null}, {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Reflection.FieldInfo\", \"L" +
"ocalName\": \"local$0\", \"Local\": null}, {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.R" +
"eflection.Emit.Label\", \"LocalName\": \"label$0\", \"Local\": null}, {\"NodeType\": \"Local\", \"Ty" +
"peDeclaration\": \"System.Func`1[System.String]\", \"LocalName\": \"func\", \"Local\": null}], \"For" +
"mulas\": [{\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\":" +
" \"New\", \"TypeDeclaration\": \"System.Reflection.Emit.DynamicMethod\", \"Constructor\": \"Void .ct" +
"or(System.String, System.Type, System.Type[], Boolean)\", \"Arguments\": [{\"NodeType\": \"Constant\"" +
", \"TypeDeclaration\": \"System.String\", \"ConstantValue\": \"DynamicMethod\"}, {\"NodeType\": \"C" +
"onstant\", \"TypeDeclaration\": \"System.Type\", \"ConstantValue\": \"System.String\"}, {\"NodeType\"" +
": \"Constant\", \"TypeDeclaration\": \"System.Type[]\", \"ConstantValue\": null}, {\"NodeType\": \"" +
"Constant\", \"TypeDeclaration\": \"System.Boolean\", \"ConstantValue\": \"True\"}]}, \"Left\": {\"No" +
"deType\": \"Variable\", \"TypeDeclaration\": \"System.Reflection.Emit.DynamicMethod\", \"VariableNam" +
"e\": \"dm\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"S" +
"ystem.Reflection.Emit.DynamicMethod\", \"LocalName\": \"dm\", \"Local\": null}}, \"Method\": \"=\"}," +
" {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Call\"" +
", \"TypeDeclaration\": \"System.Reflection.Emit.ILGenerator\", \"Instance\": {\"NodeType\": \"Variab" +
"le\", \"TypeDeclaration\": \"System.Reflection.Emit.DynamicMethod\", \"VariableName\": \"dm\", \"Var" +
"iableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Reflection.E" +
"mit.DynamicMethod\", \"LocalName\": \"dm\", \"Local\": null}}, \"Method\": \"System.Reflection.Emit." +
"ILGenerator GetILGenerator()\", \"Arguments\": []}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDec" +
"laration\": \"System.Reflection.Emit.ILGenerator\", \"VariableName\": \"il\", \"VariableIndex\": -1," +
" \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Reflection.Emit.ILGenerator\"" +
", \"LocalName\": \"il\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType\": \"Call\", \"TypeDecla" +
"ration\": \"System.Void\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System." +
"Reflection.Emit.ILGenerator\", \"VariableName\": \"il\", \"VariableIndex\": -1, \"Resolved\": {\"Nod" +
"eType\": \"Local\", \"TypeDeclaration\": \"System.Reflection.Emit.ILGenerator\", \"LocalName\": \"il" +
"\", \"Local\": null}}, \"Method\": \"Void Emit(System.Reflection.Emit.OpCode, System.String)\", \"Ar" +
"guments\": [{\"NodeType\": \"Field\", \"TypeDeclaration\": \"System.Reflection.Emit.OpCode\", \"Inst" +
"ance\": null, \"Member\": \"System.Reflection.Emit.OpCode Ldstr\", \"Member\": \"System.Reflection.E" +
"mit.OpCode Ldstr\"}, {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.String\", \"Constant" +
"Value\": \"testtest\"}]}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\"" +
": {\"NodeType\": \"Call\", \"TypeDeclaration\": \"System.Reflection.FieldInfo\", \"Instance\": {\"No" +
"deType\": \"Constant\", \"TypeDeclaration\": \"System.Type\", \"ConstantValue\": \"Test.Urasandesu.N" +
"Anonym.Etc.FieldTestClass1\"}, \"Method\": \"System.Reflection.FieldInfo GetField(System.String, Sys" +
"tem.Reflection.BindingFlags)\", \"Arguments\": [{\"NodeType\": \"Constant\", \"TypeDeclaration\": \"" +
"System.String\", \"ConstantValue\": \"staticObjectField\"}, {\"NodeType\": \"Constant\", \"TypeDecla" +
"ration\": \"System.Reflection.BindingFlags\", \"ConstantValue\": \"Static, NonPublic\"}]}, \"Left\":" +
" {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Reflection.FieldInfo\", \"VariableName\"" +
": \"local$0\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"" +
"System.Reflection.FieldInfo\", \"LocalName\": \"local$0\", \"Local\": null}}, \"Method\": \"=\"}, {" +
"\"NodeType\": \"Call\", \"TypeDeclaration\": \"System.Void\", \"Instance\": {\"NodeType\": \"Variabl" +
"e\", \"TypeDeclaration\": \"System.Reflection.Emit.ILGenerator\", \"VariableName\": \"il\", \"Variab" +
"leIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Reflection.Emit" +
".ILGenerator\", \"LocalName\": \"il\", \"Local\": null}}, \"Method\": \"Void Emit(System.Reflection." +
"Emit.OpCode, System.Reflection.FieldInfo)\", \"Arguments\": [{\"NodeType\": \"Field\", \"TypeDeclara" +
"tion\": \"System.Reflection.Emit.OpCode\", \"Instance\": null, \"Member\": \"System.Reflection.Emit." +
"OpCode Stsfld\", \"Member\": \"System.Reflection.Emit.OpCode Stsfld\"}, {\"NodeType\": \"Variable\"," +
" \"TypeDeclaration\": \"System.Reflection.FieldInfo\", \"VariableName\": \"local$0\", \"VariableInde" +
"x\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Reflection.FieldInfo\"" +
", \"LocalName\": \"local$0\", \"Local\": null}}]}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": " +
"\"System.Void\", \"Right\": {\"NodeType\": \"Call\", \"TypeDeclaration\": \"System.Reflection.FieldI" +
"nfo\", \"Instance\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Type\", \"ConstantVa" +
"lue\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass1\"}, \"Method\": \"System.Reflection.FieldInfo G" +
"etField(System.String, System.Reflection.BindingFlags)\", \"Arguments\": [{\"NodeType\": \"Constant\"" +
", \"TypeDeclaration\": \"System.String\", \"ConstantValue\": \"staticObjectField\"}, {\"NodeType\":" +
" \"Constant\", \"TypeDeclaration\": \"System.Reflection.BindingFlags\", \"ConstantValue\": \"Static," +
" NonPublic\"}]}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Reflection.Fie" +
"ldInfo\", \"VariableName\": \"local$0\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local" +
"\", \"TypeDeclaration\": \"System.Reflection.FieldInfo\", \"LocalName\": \"local$0\", \"Local\": nul" +
"l}}, \"Method\": \"=\"}, {\"NodeType\": \"Call\", \"TypeDeclaration\": \"System.Void\", \"Instance\"" +
": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Reflection.Emit.ILGenerator\", \"Variab" +
"leName\": \"il\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\"" +
": \"System.Reflection.Emit.ILGenerator\", \"LocalName\": \"il\", \"Local\": null}}, \"Method\": \"Vo" +
"id Emit(System.Reflection.Emit.OpCode, System.Reflection.FieldInfo)\", \"Arguments\": [{\"NodeType\"" +
": \"Field\", \"TypeDeclaration\": \"System.Reflection.Emit.OpCode\", \"Instance\": null, \"Member\":" +
" \"System.Reflection.Emit.OpCode Ldsfld\", \"Member\": \"System.Reflection.Emit.OpCode Ldsfld\"}, {\"" +
"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Reflection.FieldInfo\", \"VariableName\": \"" +
"local$0\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Sy" +
"stem.Reflection.FieldInfo\", \"LocalName\": \"local$0\", \"Local\": null}}]}, {\"NodeType\": \"Assig" +
"n\", \"TypeDeclaration\": \"System.Void\", \"Right\": {\"NodeType\": \"Call\", \"TypeDeclaration\": " +
"\"System.Reflection.Emit.Label\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"" +
"System.Reflection.Emit.ILGenerator\", \"VariableName\": \"il\", \"VariableIndex\": -1, \"Resolved\":" +
" {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Reflection.Emit.ILGenerator\", \"LocalName\"" +
": \"il\", \"Local\": null}}, \"Method\": \"System.Reflection.Emit.Label DefineLabel()\", \"Argument" +
"s\": []}, \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Reflection.Emit.Label" +
"\", \"VariableName\": \"label$0\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"" +
"TypeDeclaration\": \"System.Reflection.Emit.Label\", \"LocalName\": \"label$0\", \"Local\": null}}, " +
"\"Method\": \"=\"}, {\"NodeType\": \"Call\", \"TypeDeclaration\": \"System.Void\", \"Instance\": {\"" +
"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Reflection.Emit.ILGenerator\", \"VariableNam" +
"e\": \"il\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"S" +
"ystem.Reflection.Emit.ILGenerator\", \"LocalName\": \"il\", \"Local\": null}}, \"Method\": \"Void Em" +
"it(System.Reflection.Emit.OpCode, System.Reflection.Emit.Label)\", \"Arguments\": [{\"NodeType\": \"" +
"Field\", \"TypeDeclaration\": \"System.Reflection.Emit.OpCode\", \"Instance\": null, \"Member\": \"S" +
"ystem.Reflection.Emit.OpCode Br\", \"Member\": \"System.Reflection.Emit.OpCode Br\"}, {\"NodeType\":" +
" \"Variable\", \"TypeDeclaration\": \"System.Reflection.Emit.Label\", \"VariableName\": \"label$0\"," +
" \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Reflec" +
"tion.Emit.Label\", \"LocalName\": \"label$0\", \"Local\": null}}]}, {\"NodeType\": \"Call\", \"TypeD" +
"eclaration\": \"System.Void\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Sys" +
"tem.Reflection.Emit.ILGenerator\", \"VariableName\": \"il\", \"VariableIndex\": -1, \"Resolved\": {\"" +
"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Reflection.Emit.ILGenerator\", \"LocalName\": " +
"\"il\", \"Local\": null}}, \"Method\": \"Void MarkLabel(System.Reflection.Emit.Label)\", \"Arguments" +
"\": [{\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Reflection.Emit.Label\", \"VariableN" +
"ame\": \"label$0\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration" +
"\": \"System.Reflection.Emit.Label\", \"LocalName\": \"label$0\", \"Local\": null}}]}, {\"NodeType\"" +
": \"Call\", \"TypeDeclaration\": \"System.Void\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeD" +
"eclaration\": \"System.Reflection.Emit.ILGenerator\", \"VariableName\": \"il\", \"VariableIndex\": -" +
"1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Reflection.Emit.ILGenerator" +
"\", \"LocalName\": \"il\", \"Local\": null}}, \"Method\": \"Void Emit(System.Reflection.Emit.OpCode)" +
"\", \"Arguments\": [{\"NodeType\": \"Field\", \"TypeDeclaration\": \"System.Reflection.Emit.OpCode\"" +
", \"Instance\": null, \"Member\": \"System.Reflection.Emit.OpCode Ret\", \"Member\": \"System.Reflec" +
"tion.Emit.OpCode Ret\"}]}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Void\", \"Right\"" +
": {\"NodeType\": \"Convert\", \"TypeDeclaration\": \"System.Func`1[System.String]\", \"Method\": nu" +
"ll, \"Operand\": {\"NodeType\": \"Call\", \"TypeDeclaration\": \"System.Delegate\", \"Instance\": {\"" +
"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Reflection.Emit.DynamicMethod\", \"Variable" +
"Name\": \"dm\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": " +
"\"System.Reflection.Emit.DynamicMethod\", \"LocalName\": \"dm\", \"Local\": null}}, \"Method\": \"Sy" +
"stem.Delegate CreateDelegate(System.Type)\", \"Arguments\": [{\"NodeType\": \"Constant\", \"TypeDecl" +
"aration\": \"System.Type\", \"ConstantValue\": \"System.Func`1[System.String]\"}]}}, \"Left\": {\"No" +
"deType\": \"Variable\", \"TypeDeclaration\": \"System.Func`1[System.String]\", \"VariableName\": \"f" +
"unc\", \"VariableIndex\": -1, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System." +
"Func`1[System.String]\", \"LocalName\": \"func\", \"Local\": null}}, \"Method\": \"=\"}, {\"NodeType" +
"\": \"Return\", \"TypeDeclaration\": \"System.String\", \"Body\": {\"NodeType\": \"Invoke\", \"TypeD" +
"eclaration\": \"System.String\", \"DelegateOrLambda\": {\"NodeType\": \"Variable\", \"TypeDeclaratio" +
"n\": \"System.Func`1[System.String]\", \"VariableName\": \"func\", \"VariableIndex\": -1, \"Resolved" +
"\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Func`1[System.String]\", \"LocalName\": " +
"\"func\", \"Local\": null}}, \"Method\": \"System.String Invoke()\", \"Arguments\": []}, \"Label\": " +
"null}]}}", gen.DumpEntryPoint());
            #endregion
        }
    }
}
