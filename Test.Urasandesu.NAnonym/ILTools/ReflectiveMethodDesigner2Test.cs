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
            var gen = new ReflectiveMethodDesigner2();
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
            #region Assertion
            Assert.AreEqual("{\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [{\"NodeType\": \"Local" +
"\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}], \"Formulas\"" +
": [{\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Vari" +
"able\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"" +
"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\"" +
", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\"" +
": \"System.Int32\", \"ConstantValue\": 10}}, {\"NodeType\": \"Conditional\", \"TypeDeclaration\": \"" +
"System.Int32\", \"Test\": {\"NodeType\": \"AndAlso\", \"TypeDeclaration\": \"System.Int32\", \"Left\"" +
": {\"NodeType\": \"AndAlso\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"And" +
"Also\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"NotEqual\", \"TypeDeclarat" +
"ion\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\"" +
", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"Type" +
"Declaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Method\": \"!=\", \"" +
"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 20}}" +
", \"Method\": \"&&\", \"Right\": {\"NodeType\": \"NotEqual\", \"TypeDeclaration\": \"System.Int32\"," +
" \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"v" +
"alue\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System." +
"Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Method\": \"!=\", \"Right\": {\"NodeType\": " +
"\"Constant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 30}}}, \"Method\": \"&&\", \"" +
"Right\": {\"NodeType\": \"NotEqual\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\"" +
": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex" +
"\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\":" +
" \"value\", \"Local\": null}}, \"Method\": \"!=\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDec" +
"laration\": \"System.Int32\", \"ConstantValue\": 40}}}, \"Method\": \"&&\", \"Right\": {\"NodeType\"" +
": \"NotEqual\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable\", \"Type" +
"Declaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"" +
"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": " +
"null}}, \"Method\": \"!=\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.In" +
"t32\", \"ConstantValue\": 50}}}, \"IfTrue\": {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System" +
".Int32\", \"Locals\": [{\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Object\", \"LocalName" +
"\": \"objValue\", \"Local\": null}, {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Nullable" +
"`1[System.Int32]\", \"LocalName\": \"value2\", \"Local\": null}], \"Formulas\": [{\"NodeType\": \"As" +
"sign\", \"TypeDeclaration\": \"System.Object\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclara" +
"tion\": \"System.Object\", \"VariableName\": \"objValue\", \"VariableIndex\": 0, \"Resolved\": {\"No" +
"deType\": \"Local\", \"TypeDeclaration\": \"System.Object\", \"LocalName\": \"objValue\", \"Local\":" +
" null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Convert\", \"TypeDeclaration\": \"System.Obj" +
"ect\", \"Method\": null, \"Operand\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int" +
"32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"T" +
"ypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}}}, {\"NodeType\": \"" +
"Conditional\", \"TypeDeclaration\": \"System.Int32\", \"Test\": {\"NodeType\": \"NotEqual\", \"TypeD" +
"eclaration\": \"System.Nullable`1[System.Int32]\", \"Left\": {\"NodeType\": \"Assign\", \"TypeDeclar" +
"ation\": \"System.Nullable`1[System.Int32]\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclarati" +
"on\": \"System.Nullable`1[System.Int32]\", \"VariableName\": \"value2\", \"VariableIndex\": 0, \"Res" +
"olved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Nullable`1[System.Int32]\", \"Local" +
"Name\": \"value2\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"TypeAs\", \"Ty" +
"peDeclaration\": \"System.Nullable`1[System.Int32]\", \"Method\": null, \"Operand\": {\"NodeType\": " +
"\"Variable\", \"TypeDeclaration\": \"System.Object\", \"VariableName\": \"objValue\", \"VariableInde" +
"x\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Object\", \"LocalName\"" +
": \"objValue\", \"Local\": null}}}}, \"Method\": \"!=\", \"Right\": {\"NodeType\": \"Constant\", \"" +
"TypeDeclaration\": \"System.Nullable`1[System.Int32]\", \"ConstantValue\": null}}, \"IfTrue\": {\"No" +
"deType\": \"Block\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [], \"Formulas\": [{\"NodeTy" +
"pe\": \"Return\", \"TypeDeclaration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Add\", \"TypeDec" +
"laration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Add\", \"TypeDeclaration\": \"System.Int32\"" +
", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"" +
"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Syste" +
"m.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Method\": \"+\", \"Right\": {\"NodeType\":" +
" \"Multiply\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable\", \"TypeD" +
"eclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"" +
"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": n" +
"ull}}, \"Method\": \"*\", \"Right\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int3" +
"2\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"Ty" +
"peDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}}}, \"Method\": \"+\"," +
" \"Right\": {\"NodeType\": \"Convert\", \"TypeDeclaration\": \"System.Int32\", \"Method\": null, \"O" +
"perand\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Nullable`1[System.Int32]\", \"V" +
"ariableName\": \"value2\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDecla" +
"ration\": \"System.Nullable`1[System.Int32]\", \"LocalName\": \"value2\", \"Local\": null}}}}}], \"R" +
"esult\": {\"NodeType\": \"Return\", \"TypeDeclaration\": \"System.Int32\", \"Body\": {\"NodeType\": " +
"\"Add\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Add\", \"TypeDeclaration\"" +
": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"" +
"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDecl" +
"aration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Method\": \"+\", \"Right" +
"\": {\"NodeType\": \"Multiply\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"V" +
"ariable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0," +
" \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"val" +
"ue\", \"Local\": null}}, \"Method\": \"*\", \"Right\": {\"NodeType\": \"Variable\", \"TypeDeclaratio" +
"n\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\"" +
": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}}}, " +
"\"Method\": \"+\", \"Right\": {\"NodeType\": \"Convert\", \"TypeDeclaration\": \"System.Int32\", \"M" +
"ethod\": null, \"Operand\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Nullable`1[Sy" +
"stem.Int32]\", \"VariableName\": \"value2\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Lo" +
"cal\", \"TypeDeclaration\": \"System.Nullable`1[System.Int32]\", \"LocalName\": \"value2\", \"Local\"" +
": null}}}}}}, \"IfFalse\": {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.Int32\", \"Local" +
"s\": [], \"Formulas\": [{\"NodeType\": \"Return\", \"TypeDeclaration\": \"System.Int32\", \"Body\": " +
"{\"NodeType\": \"Add\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable\"" +
", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"L" +
"ocal\": null}}, \"Method\": \"+\", \"Right\": {\"NodeType\": \"Multiply\", \"TypeDeclaration\": \"Sy" +
"stem.Int32\", \"Left\": {\"NodeType\": \"Multiply\", \"TypeDeclaration\": \"System.Int32\", \"Left\"" +
": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"" +
"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", " +
"\"LocalName\": \"value\", \"Local\": null}}, \"Method\": \"*\", \"Right\": {\"NodeType\": \"Variable" +
"\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Reso" +
"lved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"" +
"Local\": null}}}, \"Method\": \"*\", \"Right\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"" +
"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"L" +
"ocal\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}}}}], \"Re" +
"sult\": {\"NodeType\": \"Return\", \"TypeDeclaration\": \"System.Int32\", \"Body\": {\"NodeType\": \"" +
"Add\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclarat" +
"ion\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeTyp" +
"e\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, " +
"\"Method\": \"+\", \"Right\": {\"NodeType\": \"Multiply\", \"TypeDeclaration\": \"System.Int32\", \"" +
"Left\": {\"NodeType\": \"Multiply\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\":" +
" \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\"" +
": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"" +
"value\", \"Local\": null}}, \"Method\": \"*\", \"Right\": {\"NodeType\": \"Variable\", \"TypeDeclar" +
"ation\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeT" +
"ype\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}" +
"}, \"Method\": \"*\", \"Right\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\"," +
" \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDe" +
"claration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}}}}}}], \"Result\": {\"Nod" +
"eType\": \"Conditional\", \"TypeDeclaration\": \"System.Int32\", \"Test\": {\"NodeType\": \"NotEqual" +
"\", \"TypeDeclaration\": \"System.Nullable`1[System.Int32]\", \"Left\": {\"NodeType\": \"Assign\", \"" +
"TypeDeclaration\": \"System.Nullable`1[System.Int32]\", \"Left\": {\"NodeType\": \"Variable\", \"Ty" +
"peDeclaration\": \"System.Nullable`1[System.Int32]\", \"VariableName\": \"value2\", \"VariableIndex\"" +
": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Nullable`1[System.Int32]" +
"\", \"LocalName\": \"value2\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Typ" +
"eAs\", \"TypeDeclaration\": \"System.Nullable`1[System.Int32]\", \"Method\": null, \"Operand\": {\"N" +
"odeType\": \"Variable\", \"TypeDeclaration\": \"System.Object\", \"VariableName\": \"objValue\", \"V" +
"ariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Object\", \"" +
"LocalName\": \"objValue\", \"Local\": null}}}}, \"Method\": \"!=\", \"Right\": {\"NodeType\": \"Con" +
"stant\", \"TypeDeclaration\": \"System.Nullable`1[System.Int32]\", \"ConstantValue\": null}}, \"IfTr" +
"ue\": {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [], \"Formulas\":" +
" [{\"NodeType\": \"Return\", \"TypeDeclaration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Add\"" +
", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Add\", \"TypeDeclaration\": \"Sy" +
"stem.Int32\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"Variab" +
"leName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration" +
"\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Method\": \"+\", \"Right\": {\"" +
"NodeType\": \"Multiply\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable" +
"\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Reso" +
"lved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"" +
"Local\": null}}, \"Method\": \"*\", \"Right\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"" +
"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Lo" +
"cal\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}}}, \"Metho" +
"d\": \"+\", \"Right\": {\"NodeType\": \"Convert\", \"TypeDeclaration\": \"System.Int32\", \"Method\"" +
": null, \"Operand\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Nullable`1[System.In" +
"t32]\", \"VariableName\": \"value2\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", " +
"\"TypeDeclaration\": \"System.Nullable`1[System.Int32]\", \"LocalName\": \"value2\", \"Local\": null" +
"}}}}}], \"Result\": {\"NodeType\": \"Return\", \"TypeDeclaration\": \"System.Int32\", \"Body\": {\"N" +
"odeType\": \"Add\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Add\", \"TypeD" +
"eclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System" +
".Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\"," +
" \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Method\": \"+" +
"\", \"Right\": {\"NodeType\": \"Multiply\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"Node" +
"Type\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"Variable" +
"Index\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalNa" +
"me\": \"value\", \"Local\": null}}, \"Method\": \"*\", \"Right\": {\"NodeType\": \"Variable\", \"Typ" +
"eDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {" +
"\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\":" +
" null}}}}, \"Method\": \"+\", \"Right\": {\"NodeType\": \"Convert\", \"TypeDeclaration\": \"System.I" +
"nt32\", \"Method\": null, \"Operand\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Nu" +
"llable`1[System.Int32]\", \"VariableName\": \"value2\", \"VariableIndex\": 0, \"Resolved\": {\"NodeT" +
"ype\": \"Local\", \"TypeDeclaration\": \"System.Nullable`1[System.Int32]\", \"LocalName\": \"value2\"" +
", \"Local\": null}}}}}}, \"IfFalse\": {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.Int32" +
"\", \"Locals\": [], \"Formulas\": [{\"NodeType\": \"Return\", \"TypeDeclaration\": \"System.Int32\"," +
" \"Body\": {\"NodeType\": \"Add\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"" +
"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": " +
"0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"v" +
"alue\", \"Local\": null}}, \"Method\": \"+\", \"Right\": {\"NodeType\": \"Multiply\", \"TypeDeclarat" +
"ion\": \"System.Int32\", \"Left\": {\"NodeType\": \"Multiply\", \"TypeDeclaration\": \"System.Int32\"" +
", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"" +
"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Syste" +
"m.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Method\": \"*\", \"Right\": {\"NodeType\":" +
" \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\"" +
": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"" +
"value\", \"Local\": null}}}, \"Method\": \"*\", \"Right\": {\"NodeType\": \"Variable\", \"TypeDecla" +
"ration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"Node" +
"Type\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}" +
"}}}}], \"Result\": {\"NodeType\": \"Return\", \"TypeDeclaration\": \"System.Int32\", \"Body\": {\"No" +
"deType\": \"Add\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable\", \"T" +
"ypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\":" +
" {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\"" +
": null}}, \"Method\": \"+\", \"Right\": {\"NodeType\": \"Multiply\", \"TypeDeclaration\": \"System." +
"Int32\", \"Left\": {\"NodeType\": \"Multiply\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"" +
"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"Vari" +
"ableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"Loc" +
"alName\": \"value\", \"Local\": null}}, \"Method\": \"*\", \"Right\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\"" +
": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Loca" +
"l\": null}}}, \"Method\": \"*\", \"Right\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Syst" +
"em.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\"" +
", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}}}}}}}, \"IfFal" +
"se\": {\"NodeType\": \"Conditional\", \"TypeDeclaration\": \"System.Int32\", \"Test\": {\"NodeType\"" +
": \"Equal\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDec" +
"laration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"No" +
"deType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": nul" +
"l}}, \"Method\": \"==\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int32" +
"\", \"ConstantValue\": 20}}, \"IfTrue\": {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.Int" +
"32\", \"Locals\": [], \"Formulas\": [{\"NodeType\": \"Return\", \"TypeDeclaration\": \"System.Int32\"" +
", \"Body\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"" +
"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Syste" +
"m.Int32\", \"LocalName\": \"value\", \"Local\": null}}}], \"Result\": {\"NodeType\": \"Return\", \"T" +
"ypeDeclaration\": \"System.Int32\", \"Body\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Sy" +
"stem.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Loca" +
"l\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}}}, \"IfFalse" +
"\": {\"NodeType\": \"Conditional\", \"TypeDeclaration\": \"System.Int32\", \"Test\": {\"NodeType\": " +
"\"Equal\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDecla" +
"ration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"Node" +
"Type\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}" +
"}, \"Method\": \"==\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int32\"" +
", \"ConstantValue\": 40}}, \"IfTrue\": {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.Int32" +
"\", \"Locals\": [], \"Formulas\": [{\"NodeType\": \"Return\", \"TypeDeclaration\": \"System.Int32\"," +
" \"Body\": {\"NodeType\": \"ExclusiveOr\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeT" +
"ype\": \"ExclusiveOr\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable\"" +
", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"L" +
"ocal\": null}}, \"Method\": \"^\", \"Right\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Sy" +
"stem.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Loca" +
"l\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}}, \"Method\"" +
": \"^\", \"Right\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableNa" +
"me\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": " +
"\"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}}}], \"Result\": {\"NodeType\": \"Retur" +
"n\", \"TypeDeclaration\": \"System.Int32\", \"Body\": {\"NodeType\": \"ExclusiveOr\", \"TypeDeclarat" +
"ion\": \"System.Int32\", \"Left\": {\"NodeType\": \"ExclusiveOr\", \"TypeDeclaration\": \"System.Int" +
"32\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\"" +
": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Sy" +
"stem.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Method\": \"^\", \"Right\": {\"NodeType" +
"\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableInde" +
"x\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\"" +
": \"value\", \"Local\": null}}}, \"Method\": \"^\", \"Right\": {\"NodeType\": \"Variable\", \"TypeDe" +
"claration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"N" +
"odeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": nu" +
"ll}}}}}, \"IfFalse\": {\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [" +
"], \"Formulas\": [{\"NodeType\": \"Return\", \"TypeDeclaration\": \"System.Int32\", \"Body\": {\"Nod" +
"eType\": \"Conditional\", \"TypeDeclaration\": \"System.Int32\", \"Test\": {\"NodeType\": \"Equal\"," +
" \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"" +
"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Metho" +
"d\": \"==\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.Int32\", \"Consta" +
"ntValue\": 30}}, \"IfTrue\": {\"NodeType\": \"Add\", \"TypeDeclaration\": \"System.Int32\", \"Left\"" +
": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"" +
"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", " +
"\"LocalName\": \"value\", \"Local\": null}}, \"Method\": \"+\", \"Right\": {\"NodeType\": \"Variable" +
"\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Reso" +
"lved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"" +
"Local\": null}}}, \"IfFalse\": {\"NodeType\": \"Multiply\", \"TypeDeclaration\": \"System.Int32\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"va" +
"lue\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.I" +
"nt32\", \"LocalName\": \"value\", \"Local\": null}}, \"Method\": \"*\", \"Right\": {\"NodeType\": \"" +
"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0" +
", \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"va" +
"lue\", \"Local\": null}}}}}], \"Result\": {\"NodeType\": \"Return\", \"TypeDeclaration\": \"System.I" +
"nt32\", \"Body\": {\"NodeType\": \"Conditional\", \"TypeDeclaration\": \"System.Int32\", \"Test\": {" +
"\"NodeType\": \"Equal\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable\"" +
", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resol" +
"ved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"" +
"Local\": null}}, \"Method\": \"==\", \"Right\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"" +
"System.Int32\", \"ConstantValue\": 30}}, \"IfTrue\": {\"NodeType\": \"Add\", \"TypeDeclaration\": \"" +
"System.Int32\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"Vari" +
"ableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclarati" +
"on\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Method\": \"+\", \"Right\": {" +
"\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value\", \"Va" +
"riableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"L" +
"ocalName\": \"value\", \"Local\": null}}}, \"IfFalse\": {\"NodeType\": \"Multiply\", \"TypeDeclarati" +
"on\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\"" +
", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeD" +
"eclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Method\": \"*\", \"Ri" +
"ght\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System.Int32\", \"VariableName\": \"value" +
"\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"System.Int3" +
"2\", \"LocalName\": \"value\", \"Local\": null}}}}}}}}}, {\"NodeType\": \"End\", \"TypeDeclaration\"" +
": \"System.Int32\"}], \"Result\": {\"NodeType\": \"End\", \"TypeDeclaration\": \"System.Int32\"}}", gen.Dump());
            #endregion
            //Console.WriteLine(gen.Dump());
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        [Test]
        public void EvalTest02()
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
            #region Assertion
            Assert.AreEqual("{\"NodeType\": \"Block\", \"TypeDeclaration\": \"System.Int32\", \"Locals\": [{\"NodeType\": \"Local" +
"\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", " +
"\"Local\": null}, {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.Field" +
"TestClass2\", \"LocalName\": \"f2\", \"Local\": null}, {\"NodeType\": \"Local\", \"TypeDeclaration\"" +
": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass2\", \"LocalName\": \"p2\", \"Local\": null}, {\"No" +
"deType\": \"Local\", \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": nul" +
"l}], \"Formulas\": [{\"NodeType\": \"ReflectiveCall\", \"TypeDeclaration\": \"System.Void\", \"Insta" +
"nce\": null, \"Method\": \"Void WriteLog(System.String, System.Object[])\", \"Arguments\": [{\"NodeT" +
"ype\": \"NewArrayInit\", \"TypeDeclaration\": \"System.Object[]\", \"Formulas\": [{\"NodeType\": \"C" +
"onstant\", \"TypeDeclaration\": \"System.String\", \"ConstantValue\": \"testtest\"}]}]}, {\"NodeType" +
"\": \"Assign\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"Left\": {" +
"\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\"," +
" \"VariableName\": \"p1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDecla" +
"ration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", \"Local\": null" +
"}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"ReflectiveNew\", \"TypeDeclaration\": \"Test.Uras" +
"andesu.NAnonym.Etc.PropertyTestClass1\", \"Constructor\": \"Void .ctor()\", \"Arguments\": []}}, {\"" +
"NodeType\": \"ReflectiveCall\", \"TypeDeclaration\": \"System.Void\", \"Instance\": null, \"Method\"" +
": \"Void WriteLog(System.String, System.Object[])\", \"Arguments\": [{\"NodeType\": \"NewArrayInit\"" +
", \"TypeDeclaration\": \"System.Object[]\", \"Formulas\": [{\"NodeType\": \"Constant\", \"TypeDeclar" +
"ation\": \"System.String\", \"ConstantValue\": \"{0}\"}, {\"NodeType\": \"Variable\", \"TypeDeclarat" +
"ion\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"VariableName\": \"p1\", \"VariableIndex" +
"\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.Pr" +
"opertyTestClass1\", \"LocalName\": \"p1\", \"Local\": null}}]}]}, {\"NodeType\": \"Assign\", \"TypeD" +
"eclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"ReflectiveProperty\", \"TypeDeclaration\"" +
": \"System.Int32\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandes" +
"u.NAnonym.Etc.PropertyTestClass1\", \"VariableName\": \"p1\", \"VariableIndex\": 0, \"Resolved\": {\"" +
"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"L" +
"ocalName\": \"p1\", \"Local\": null}}, \"Member\": \"Int32 ValueProperty\", \"Member\": \"Int32 Valu" +
"eProperty\"}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Convert\", \"TypeDeclaration\": \"Syste" +
"m.Object\", \"Method\": null, \"Operand\": {\"NodeType\": \"Constant\", \"TypeDeclaration\": \"Syste" +
"m.Int32\", \"ConstantValue\": 10}}}, {\"NodeType\": \"ReflectiveCall\", \"TypeDeclaration\": \"Syste" +
"m.Void\", \"Instance\": null, \"Method\": \"Void WriteLog(System.String, System.Object[])\", \"Argum" +
"ents\": [{\"NodeType\": \"NewArrayInit\", \"TypeDeclaration\": \"System.Object[]\", \"Formulas\": [{" +
"\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.String\", \"ConstantValue\": \"ValueProper" +
"ty: {0}\"}, {\"NodeType\": \"Convert\", \"TypeDeclaration\": \"System.Object\", \"Method\": null, \"" +
"Operand\": {\"NodeType\": \"ReflectiveProperty\", \"TypeDeclaration\": \"System.Int32\", \"Instance\"" +
": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass" +
"1\", \"VariableName\": \"p1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeD" +
"eclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", \"Local\": " +
"null}}, \"Member\": \"Int32 ValueProperty\", \"Member\": \"Int32 ValueProperty\"}}]}]}, {\"NodeType\"" +
": \"Assign\", \"TypeDeclaration\": \"System.String\", \"Left\": {\"NodeType\": \"ReflectiveProperty" +
"\", \"TypeDeclaration\": \"System.String\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclara" +
"tion\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"VariableName\": \"p1\", \"VariableInde" +
"x\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.P" +
"ropertyTestClass1\", \"LocalName\": \"p1\", \"Local\": null}}, \"Member\": \"System.String ObjectPro" +
"perty\", \"Member\": \"System.String ObjectProperty\"}, \"Method\": \"=\", \"Right\": {\"NodeType\":" +
" \"Constant\", \"TypeDeclaration\": \"System.String\", \"ConstantValue\": \"a\"}}, {\"NodeType\": \"" +
"Assign\", \"TypeDeclaration\": \"System.String\", \"Left\": {\"NodeType\": \"ReflectiveProperty\", \"" +
"TypeDeclaration\": \"System.String\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\"" +
": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"VariableName\": \"p1\", \"VariableIndex\": " +
"0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.Proper" +
"tyTestClass1\", \"LocalName\": \"p1\", \"Local\": null}}, \"Member\": \"System.String ObjectProperty" +
"\", \"Member\": \"System.String ObjectProperty\"}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Re" +
"flectiveProperty\", \"TypeDeclaration\": \"System.String\", \"Instance\": {\"NodeType\": \"Variable\"" +
", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"VariableName\": \"p1\"" +
", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasande" +
"su.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", \"Local\": null}}, \"Member\": \"System." +
"String ObjectProperty\", \"Member\": \"System.String ObjectProperty\"}}, {\"NodeType\": \"Reflective" +
"Call\", \"TypeDeclaration\": \"System.Void\", \"Instance\": null, \"Method\": \"Void WriteLog(System" +
".String, System.Object[])\", \"Arguments\": [{\"NodeType\": \"NewArrayInit\", \"TypeDeclaration\": \"" +
"System.Object[]\", \"Formulas\": [{\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.String" +
"\", \"ConstantValue\": \"ObjectProperty: {0}\"}, {\"NodeType\": \"ReflectiveProperty\", \"TypeDeclar" +
"ation\": \"System.String\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.U" +
"rasandesu.NAnonym.Etc.PropertyTestClass1\", \"VariableName\": \"p1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass" +
"1\", \"LocalName\": \"p1\", \"Local\": null}}, \"Member\": \"System.String ObjectProperty\", \"Membe" +
"r\": \"System.String ObjectProperty\"}]}]}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Test.U" +
"rasandesu.NAnonym.Etc.FieldTestClass2\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\":" +
" \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"VariableName\": \"f2\", \"VariableIndex\": 0, \"" +
"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestCl" +
"ass2\", \"LocalName\": \"f2\", \"Local\": null}}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"New" +
"\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"Constructor\": \"Void .c" +
"tor()\", \"Arguments\": []}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"System.Int32\", \"Le" +
"ft\": {\"NodeType\": \"ReflectiveField\", \"TypeDeclaration\": \"System.Int32\", \"Instance\": {\"No" +
"deType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"Vari" +
"ableName\": \"f2\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\"" +
": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"LocalName\": \"f2\", \"Local\": null}}, \"Memb" +
"er\": \"Int32 ValueField\", \"Member\": \"Int32 ValueField\"}, \"Method\": \"=\", \"Right\": {\"Node" +
"Type\": \"Convert\", \"TypeDeclaration\": \"System.Object\", \"Method\": null, \"Operand\": {\"NodeT" +
"ype\": \"Constant\", \"TypeDeclaration\": \"System.Int32\", \"ConstantValue\": 30}}}, {\"NodeType\":" +
" \"Assign\", \"TypeDeclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"ReflectiveField\", \"" +
"TypeDeclaration\": \"System.Int32\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\":" +
" \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"VariableName\": \"f2\", \"VariableIndex\": 0, \"" +
"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestCl" +
"ass2\", \"LocalName\": \"f2\", \"Local\": null}}, \"Member\": \"Int32 ValueField\", \"Member\": \"In" +
"t32 ValueField\"}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"ReflectiveField\", \"TypeDeclarati" +
"on\": \"System.Int32\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasa" +
"ndesu.NAnonym.Etc.FieldTestClass2\", \"VariableName\": \"f2\", \"VariableIndex\": 0, \"Resolved\": {" +
"\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"Loc" +
"alName\": \"f2\", \"Local\": null}}, \"Member\": \"Int32 ValueField\", \"Member\": \"Int32 ValueFiel" +
"d\"}}, {\"NodeType\": \"Call\", \"TypeDeclaration\": \"System.Void\", \"Instance\": null, \"Method\"" +
": \"Void WriteLog(System.String, System.Object[])\", \"Arguments\": [{\"NodeType\": \"NewArrayInit\"" +
", \"TypeDeclaration\": \"System.Object[]\", \"Formulas\": [{\"NodeType\": \"Constant\", \"TypeDeclar" +
"ation\": \"System.String\", \"ConstantValue\": \"ValueField: {0}\"}, {\"NodeType\": \"Convert\", \"T" +
"ypeDeclaration\": \"System.Object\", \"Method\": null, \"Operand\": {\"NodeType\": \"ReflectiveField" +
"\", \"TypeDeclaration\": \"System.Int32\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclarat" +
"ion\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"VariableName\": \"f2\", \"VariableIndex\":" +
" 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.Field" +
"TestClass2\", \"LocalName\": \"f2\", \"Local\": null}}, \"Member\": \"Int32 ValueField\", \"Member\"" +
": \"Int32 ValueField\"}}]}]}, {\"NodeType\": \"ReflectiveCall\", \"TypeDeclaration\": \"System.Void\"" +
", \"Instance\": null, \"Method\": \"Void WriteLog(System.String, System.Object[])\", \"Arguments\":" +
" [{\"NodeType\": \"NewArrayInit\", \"TypeDeclaration\": \"System.Object[]\", \"Formulas\": [{\"NodeT" +
"ype\": \"Constant\", \"TypeDeclaration\": \"System.String\", \"ConstantValue\": \"ValueField: {0}\"}" +
", {\"NodeType\": \"Convert\", \"TypeDeclaration\": \"System.Object\", \"Method\": null, \"Operand\":" +
" {\"NodeType\": \"ReflectiveField\", \"TypeDeclaration\": \"System.Int32\", \"Instance\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"VariableN" +
"ame\": \"f2\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"" +
"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"LocalName\": \"f2\", \"Local\": null}}, \"Member\":" +
" \"Int32 ValueField\", \"Member\": \"Int32 ValueField\"}}]}]}, {\"NodeType\": \"Assign\", \"TypeDecl" +
"aration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass2\", \"Left\": {\"NodeType\": \"Variable\"" +
", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass2\", \"VariableName\": \"p2\"," +
" \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandes" +
"u.NAnonym.Etc.PropertyTestClass2\", \"LocalName\": \"p2\", \"Local\": null}}, \"Method\": \"=\", \"R" +
"ight\": {\"NodeType\": \"ReflectiveNew\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.Propert" +
"yTestClass2\", \"Constructor\": \"Void .ctor(Int32, System.String)\", \"Arguments\": [{\"NodeType\":" +
" \"ReflectiveProperty\", \"TypeDeclaration\": \"System.Int32\", \"Instance\": {\"NodeType\": \"Varia" +
"ble\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"VariableName\": \"" +
"p1\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Uras" +
"andesu.NAnonym.Etc.PropertyTestClass1\", \"LocalName\": \"p1\", \"Local\": null}}, \"Member\": \"Int" +
"32 ValueProperty\", \"Member\": \"Int32 ValueProperty\"}, {\"NodeType\": \"ReflectiveProperty\", \"T" +
"ypeDeclaration\": \"System.String\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\":" +
" \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass1\", \"VariableName\": \"p1\", \"VariableIndex\": 0," +
" \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.Property" +
"TestClass1\", \"LocalName\": \"p1\", \"Local\": null}}, \"Member\": \"System.String ObjectProperty\"" +
", \"Member\": \"System.String ObjectProperty\"}]}}, {\"NodeType\": \"ReflectiveCall\", \"TypeDeclara" +
"tion\": \"System.Void\", \"Instance\": null, \"Method\": \"Void WriteLog(System.String, System.Objec" +
"t[])\", \"Arguments\": [{\"NodeType\": \"NewArrayInit\", \"TypeDeclaration\": \"System.Object[]\", \"" +
"Formulas\": [{\"NodeType\": \"Constant\", \"TypeDeclaration\": \"System.String\", \"ConstantValue\"" +
": \"({0}, {1})\"}, {\"NodeType\": \"Convert\", \"TypeDeclaration\": \"System.Object\", \"Method\": n" +
"ull, \"Operand\": {\"NodeType\": \"Property\", \"TypeDeclaration\": \"System.Int32\", \"Instance\": " +
"{\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass2\"" +
", \"VariableName\": \"p2\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDecl" +
"aration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass2\", \"LocalName\": \"p2\", \"Local\": nul" +
"l}}, \"Member\": \"Int32 ValueProperty\", \"Member\": \"Int32 ValueProperty\"}}, {\"NodeType\": \"Pr" +
"operty\", \"TypeDeclaration\": \"System.String\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeD" +
"eclaration\": \"Test.Urasandesu.NAnonym.Etc.PropertyTestClass2\", \"VariableName\": \"p2\", \"Variab" +
"leIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym" +
".Etc.PropertyTestClass2\", \"LocalName\": \"p2\", \"Local\": null}}, \"Member\": \"System.String Obj" +
"ectProperty\", \"Member\": \"System.String ObjectProperty\"}]}]}, {\"NodeType\": \"Assign\", \"TypeD" +
"eclaration\": \"System.Int32\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"System" +
".Int32\", \"VariableName\": \"value\", \"VariableIndex\": 0, \"Resolved\": {\"NodeType\": \"Local\"," +
" \"TypeDeclaration\": \"System.Int32\", \"LocalName\": \"value\", \"Local\": null}}, \"Method\": \"=" +
"\", \"Right\": {\"NodeType\": \"ReflectiveCall\", \"TypeDeclaration\": \"System.Int32\", \"Instance\"" +
": null, \"Method\": \"Int32 GetValue(Int32)\", \"Arguments\": [{\"NodeType\": \"ReflectiveField\", " +
"\"TypeDeclaration\": \"System.Int32\", \"Instance\": {\"NodeType\": \"Variable\", \"TypeDeclaration\"" +
": \"Test.Urasandesu.NAnonym.Etc.FieldTestClass2\", \"VariableName\": \"f2\", \"VariableIndex\": 0, " +
"\"Resolved\": {\"NodeType\": \"Local\", \"TypeDeclaration\": \"Test.Urasandesu.NAnonym.Etc.FieldTest" +
"Class2\", \"LocalName\": \"f2\", \"Local\": null}}, \"Member\": \"Int32 ValueField\", \"Member\": \"" +
"Int32 ValueField\"}]}}, {\"NodeType\": \"End\", \"TypeDeclaration\": \"System.Int32\"}], \"Result\":" +
" {\"NodeType\": \"End\", \"TypeDeclaration\": \"System.Int32\"}}", gen.Dump());
            #endregion
            //Console.WriteLine(gen.Dump());
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        [Test]
        public void EvalTest03()
        {
            var gen = new ReflectiveMethodDesigner2();
            var opcode1 = default(OpCode);
            var opcode2 = OpCodes.Add_Ovf;
            gen.Eval(() => Dsl.Allocate(opcode1).As(Dsl.ConstMember(OpCodes.Add, typeof(OpCodes))));
            for (int i = 0; i < 100; i++)
            {
                gen.Eval(() => Dsl.Allocate(opcode1).As(Dsl.ConstMember(opcode2, typeof(OpCodes))));
            }
            #region Assertion
            Assert.AreEqual("{\"NodeType\": \"Block\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Locals\": []" +
", \"Formulas\": [{\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode" +
"\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"" +
", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Ri" +
"ght\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Insta" +
"nce\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add\", \"Member\": \"Urasandesu.NAnonym" +
".ILTools.OpCode Add\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools" +
".OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=" +
"\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"," +
" \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasan" +
"desu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandes" +
"u.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu" +
".NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}," +
" \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.I" +
"LTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Me" +
"mber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaratio" +
"n\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration" +
"\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Re" +
"solved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Uras" +
"andesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode" +
" Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", " +
"\"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDecla" +
"ration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnony" +
"m.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType" +
"\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\":" +
" \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcod" +
"e1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Fiel" +
"d\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"U" +
"rasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"" +
"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\"" +
": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Variabl" +
"eName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"No" +
"deType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null," +
" \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools" +
".OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.Op" +
"Code\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpC" +
"ode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", " +
"\"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"I" +
"nstance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu" +
".NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NA" +
"nonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAn" +
"onym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"M" +
"ethod\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILToo" +
"ls.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member" +
"\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\":" +
" \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasande" +
"su.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add" +
"_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"Ty" +
"peDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableInde" +
"x\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclarati" +
"on\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.IL" +
"Tools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": " +
"\"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"V" +
"ariable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\"" +
", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\"," +
" \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}," +
" {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"" +
"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableNam" +
"e\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeTy" +
"pe\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"M" +
"ember\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpC" +
"ode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode" +
"\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"" +
", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Ri" +
"ght\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Insta" +
"nce\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAn" +
"onym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnony" +
"m.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Metho" +
"d\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.O" +
"pCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"U" +
"rasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\"" +
": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf" +
"\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDec" +
"laration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\":" +
" 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"As" +
"sign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Varia" +
"ble\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"" +
"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"T" +
"ypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandes" +
"u.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"" +
"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"Nod" +
"eType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\":" +
" \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\"" +
": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Membe" +
"r\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode " +
"Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\"" +
": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\"" +
": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym" +
".ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.IL" +
"Tools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\":" +
" \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasan" +
"desu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": nu" +
"ll}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnon" +
"ym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", " +
"\"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclar" +
"ation\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclara" +
"tion\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, " +
"\"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.Op" +
"Code Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign" +
"\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\"" +
", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Vari" +
"ableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeD" +
"eclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NA" +
"nonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"Node" +
"Type\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"" +
"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\":" +
" \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_" +
"Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Le" +
"ft\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Var" +
"iableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {" +
"\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": n" +
"ull, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools" +
".OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=" +
"\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"," +
" \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasan" +
"desu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandes" +
"u.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu" +
".NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}," +
" \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.I" +
"LTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Me" +
"mber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaratio" +
"n\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration" +
"\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Re" +
"solved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Uras" +
"andesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode" +
" Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", " +
"\"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDecla" +
"ration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnony" +
"m.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType" +
"\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\":" +
" \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcod" +
"e1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Fiel" +
"d\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"U" +
"rasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"" +
"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\"" +
": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Variabl" +
"eName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"No" +
"deType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null," +
" \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools" +
".OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.Op" +
"Code\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpC" +
"ode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", " +
"\"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"I" +
"nstance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu" +
".NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NA" +
"nonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAn" +
"onym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"M" +
"ethod\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILToo" +
"ls.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member" +
"\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\":" +
" \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasande" +
"su.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add" +
"_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"Ty" +
"peDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableInde" +
"x\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclarati" +
"on\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.IL" +
"Tools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": " +
"\"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"V" +
"ariable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\"" +
", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\"," +
" \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}," +
" {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"" +
"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableNam" +
"e\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeTy" +
"pe\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"M" +
"ember\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpC" +
"ode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode" +
"\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"" +
", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Ri" +
"ght\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Insta" +
"nce\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAn" +
"onym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnony" +
"m.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Metho" +
"d\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.O" +
"pCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"U" +
"rasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\"" +
": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf" +
"\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDec" +
"laration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\":" +
" 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"As" +
"sign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Varia" +
"ble\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"" +
"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"T" +
"ypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandes" +
"u.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"" +
"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"Nod" +
"eType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\":" +
" \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\"" +
": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Membe" +
"r\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode " +
"Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\"" +
": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\"" +
": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym" +
".ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.IL" +
"Tools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\":" +
" \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasan" +
"desu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": nu" +
"ll}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnon" +
"ym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", " +
"\"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclar" +
"ation\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclara" +
"tion\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, " +
"\"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.Op" +
"Code Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign" +
"\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\"" +
", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Vari" +
"ableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeD" +
"eclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NA" +
"nonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"Node" +
"Type\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"" +
"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\":" +
" \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_" +
"Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Le" +
"ft\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Var" +
"iableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {" +
"\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": n" +
"ull, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools" +
".OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=" +
"\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"," +
" \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasan" +
"desu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandes" +
"u.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu" +
".NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}," +
" \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.I" +
"LTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Me" +
"mber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaratio" +
"n\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration" +
"\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Re" +
"solved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Uras" +
"andesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode" +
" Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", " +
"\"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDecla" +
"ration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnony" +
"m.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType" +
"\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\":" +
" \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcod" +
"e1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Fiel" +
"d\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"U" +
"rasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"" +
"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\"" +
": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Variabl" +
"eName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"No" +
"deType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null," +
" \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools" +
".OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.Op" +
"Code\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpC" +
"ode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", " +
"\"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"I" +
"nstance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu" +
".NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NA" +
"nonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAn" +
"onym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"M" +
"ethod\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILToo" +
"ls.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member" +
"\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\":" +
" \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasande" +
"su.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add" +
"_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"Ty" +
"peDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableInde" +
"x\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclarati" +
"on\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.IL" +
"Tools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": " +
"\"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"V" +
"ariable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\"" +
", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\"," +
" \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}," +
" {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"" +
"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableNam" +
"e\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeTy" +
"pe\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"M" +
"ember\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpC" +
"ode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode" +
"\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"" +
", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Ri" +
"ght\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Insta" +
"nce\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAn" +
"onym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnony" +
"m.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Metho" +
"d\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.O" +
"pCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"U" +
"rasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\"" +
": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf" +
"\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDec" +
"laration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\":" +
" 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"As" +
"sign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Varia" +
"ble\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"" +
"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"T" +
"ypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandes" +
"u.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"" +
"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"Nod" +
"eType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\":" +
" \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\"" +
": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Membe" +
"r\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode " +
"Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\"" +
": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\"" +
": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym" +
".ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.IL" +
"Tools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\":" +
" \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasan" +
"desu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": nu" +
"ll}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnon" +
"ym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", " +
"\"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclar" +
"ation\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclara" +
"tion\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, " +
"\"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.Op" +
"Code Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign" +
"\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\"" +
", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Vari" +
"ableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeD" +
"eclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NA" +
"nonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"Node" +
"Type\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"" +
"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\":" +
" \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_" +
"Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Le" +
"ft\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Var" +
"iableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {" +
"\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": n" +
"ull, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools" +
".OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=" +
"\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"," +
" \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasan" +
"desu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandes" +
"u.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu" +
".NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}," +
" \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.I" +
"LTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Me" +
"mber\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaratio" +
"n\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration" +
"\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Re" +
"solved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Uras" +
"andesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode" +
" Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", " +
"\"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"" +
"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Variable" +
"Index\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDecla" +
"ration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnony" +
"m.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType" +
"\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\":" +
" \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcod" +
"e1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Fiel" +
"d\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"U" +
"rasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"" +
"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\"" +
": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Variabl" +
"eName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"No" +
"deType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null," +
" \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools" +
".OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.Op" +
"Code\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpC" +
"ode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", " +
"\"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"I" +
"nstance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu" +
".NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NA" +
"nonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAn" +
"onym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"M" +
"ethod\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILToo" +
"ls.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member" +
"\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\":" +
" \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolv" +
"ed\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasande" +
"su.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add" +
"_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"Ty" +
"peDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"Typ" +
"eDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableInde" +
"x\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclarati" +
"on\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.IL" +
"Tools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": " +
"\"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"V" +
"ariable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\"" +
", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\"," +
" \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}," +
" {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"" +
"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableNam" +
"e\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeTy" +
"pe\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"M" +
"ember\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpC" +
"ode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode" +
"\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\"" +
", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Ri" +
"ght\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Insta" +
"nce\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAn" +
"onym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnony" +
"m.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym" +
".ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Metho" +
"d\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.O" +
"pCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": " +
"\"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"U" +
"rasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\"" +
": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf" +
"\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDe" +
"claration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDec" +
"laration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\":" +
" 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\"" +
": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTool" +
"s.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"As" +
"sign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Varia" +
"ble\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"" +
"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"T" +
"ypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandes" +
"u.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"" +
"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"Nod" +
"eType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\":" +
" \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\"" +
": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Membe" +
"r\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode " +
"Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", " +
"\"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"" +
"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\"" +
": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\"" +
": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym" +
".ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.IL" +
"Tools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\":" +
" \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCod" +
"e\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Ur" +
"asandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasa" +
"ndesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasan" +
"desu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": nu" +
"ll}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnon" +
"ym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", " +
"\"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclar" +
"ation\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclara" +
"tion\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, " +
"\"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"" +
"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.Op" +
"Code Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"NodeType\": \"Assign" +
"\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\"" +
", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"Vari" +
"ableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeD" +
"eclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NA" +
"nonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}, {\"Node" +
"Type\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Left\": {\"NodeTyp" +
"e\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"VariableName\": \"o" +
"pcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {\"NodeType\": \"" +
"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": null, \"Member\":" +
" \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_" +
"Ovf\"}}, {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Le" +
"ft\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Var" +
"iableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"Method\": \"=\", \"Right\": {" +
"\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTools.OpCode\", \"Instance\": n" +
"ull, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Member\": \"Urasandesu.NAnonym.ILT" +
"ools.OpCode Add_Ovf\"}}], \"Result\": {\"NodeType\": \"Assign\", \"TypeDeclaration\": \"Urasandesu.N" +
"Anonym.ILTools.OpCode\", \"Left\": {\"NodeType\": \"Variable\", \"TypeDeclaration\": \"Urasandesu.NA" +
"nonym.ILTools.OpCode\", \"VariableName\": \"opcode1\", \"VariableIndex\": 0, \"Resolved\": null}, \"" +
"Method\": \"=\", \"Right\": {\"NodeType\": \"Field\", \"TypeDeclaration\": \"Urasandesu.NAnonym.ILTo" +
"ols.OpCode\", \"Instance\": null, \"Member\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\", \"Membe" +
"r\": \"Urasandesu.NAnonym.ILTools.OpCode Add_Ovf\"}}}", gen.Dump());
            #endregion
            //Console.WriteLine(gen.Dump());
        }
    }
}
