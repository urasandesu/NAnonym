using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Assert = Urasandesu.NAnonym.Test.Assert;
using Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Linq;
using Mono.Cecil;
using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym;
using Test.Urasandesu.NAnonym.Etc;

namespace Test.Urasandesu.NAnonym.DI
{
    [TestFixture]
    public class DependencyUtilTest
    {
        [Test]
        public void Hoge()
        {
#if HOGE
            string path = @"C:\Documents and Settings\Administrator\My Documents\自己学習\100627\GloabalClassDesign\ConsoleApplication1\bin\Debug\ConsoleApplication1.exe";
            var consoleApplication1 = ModuleDefinition.ReadModule(path);
            var piyo = consoleApplication1.GetType("ConsoleApplication1.Piyo");
            var print = piyo.Methods.First(method => method.Name == "Print");
            print.Body.Instructions.ForEach(instruction => Console.WriteLine(instruction));
#endif
            //// MEMO: GenericParameters は定義（typeof(A<>) みたいな状態）。GenericArguments は宣言（typeof(A<string>) まで指定）。
            //var a = typeof(A<string>).ToTypeRef();
            //Console.WriteLine(a);
            //var b = typeof(Type[]).ToTypeRef();
            //Console.WriteLine(b);

            //var fieldTestClass1Def = typeof(FieldTestClass1).ToTypeDef();
            //var valueFieldDef2 = fieldTestClass1Def.GetField("valueField", BindingFlags.Instance | BindingFlags.NonPublic).Duplicate();
            //valueFieldDef2.Name = "valueField2";
            //fieldTestClass1Def.Fields.Add(valueFieldDef2);
            //var objectFieldDef2 = fieldTestClass1Def.GetField("objectField", BindingFlags.Instance | BindingFlags.NonPublic).Duplicate();
            //objectFieldDef2.Name = "objectField2";
            //fieldTestClass1Def.Fields.Add(objectFieldDef2);
            //var staticValueFieldDef2 = fieldTestClass1Def.GetField("staticValueField", BindingFlags.Static | BindingFlags.NonPublic).Duplicate();
            //staticValueFieldDef2.Name = "staticValueField2";
            //fieldTestClass1Def.Fields.Add(staticValueFieldDef2);
            //var staticObjectFieldDef2 = fieldTestClass1Def.GetField("staticObjectField", BindingFlags.Static | BindingFlags.NonPublic).Duplicate();
            //staticObjectFieldDef2.Name = "staticObjectField2";
            //fieldTestClass1Def.Fields.Add(staticObjectFieldDef2);
            //var genericFieldDef2 = fieldTestClass1Def.GetField("genericField", BindingFlags.Instance | BindingFlags.NonPublic).Duplicate();
            //genericFieldDef2.Name = "genericField2";
            //fieldTestClass1Def.Fields.Add(genericFieldDef2);
            //var staticGenericFieldDef2 = fieldTestClass1Def.GetField("staticGenericField", BindingFlags.Static | BindingFlags.NonPublic).Duplicate();
            //staticGenericFieldDef2.Name = "staticGenericField2";
            //fieldTestClass1Def.Fields.Add(staticGenericFieldDef2);
            //fieldTestClass1Def.Module.Assembly.Write(fieldTestClass1Def.Module.Assembly.Name.Name + "1Field.dll");


            //// Property のコピーは外側 + 中身のコピーが必要！！
            //var propertyTestClassDef = typeof(PropertyTestClass1).ToTypeDef();
            //var valuePropertyDef2 = propertyTestClassDef.GetProperty("ValueProperty", BindingFlags.Instance | BindingFlags.Public).Duplicate();
            //valuePropertyDef2.Name = "ValueProperty2";
            //propertyTestClassDef.Properties.Add(valuePropertyDef2);
            //propertyTestClassDef.Module.Assembly.Write(propertyTestClassDef.Module.Assembly.Name.Name + "2Property.dll");


            //var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();
            //var action1Def2 = methodTestClassDef.GetMethod("Action1", BindingFlags.Instance | BindingFlags.Public, new Type[] { }).Duplicate();
            //action1Def2.Name = "Action12";
            //methodTestClassDef.Methods.Add(action1Def2);
            //var action2LocalVariableDef2 = methodTestClassDef.GetMethod("Action2LocalVariable", BindingFlags.Instance | BindingFlags.Public, new Type[] { }).Duplicate();
            //action2LocalVariableDef2.Name = "Action2LocalVariable2";
            //methodTestClassDef.Methods.Add(action2LocalVariableDef2);
            //var action3ExceptionDef2 = methodTestClassDef.GetMethod("Action3Exception", BindingFlags.Instance | BindingFlags.Public, new Type[] { }).Duplicate();
            //action3ExceptionDef2.Name = "Action3Exception2";
            //methodTestClassDef.Methods.Add(action3ExceptionDef2);
            //var action4GenericDef2 = methodTestClassDef.GetMethod("Action4Generic", BindingFlags.Instance | BindingFlags.Public, new Type[] { }).Duplicate();
            //action4GenericDef2.Name = "Action4Generic2";
            //methodTestClassDef.Methods.Add(action4GenericDef2);
            //var action5NoBodyDef2 = methodTestClassDef.GetMethod("Action5NoBody", BindingFlags.Instance | BindingFlags.Public, new Type[] { }).DuplicateWithoutBody();
            //action5NoBodyDef2.Name = "Action5NoBody2";
            //methodTestClassDef.Methods.Add(action5NoBodyDef2);
            //var func1ParametersDef2 = methodTestClassDef.GetMethod("Func1Parameters", BindingFlags.Instance | BindingFlags.Public, new Type[] { typeof(int) }).Duplicate();
            //func1ParametersDef2.Name = "Func1Parameters2";
            //methodTestClassDef.Methods.Add(func1ParametersDef2);
            //methodTestClassDef.Module.Assembly.Write(methodTestClassDef.Module.Assembly.Name.Name + "3Method.dll");

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
