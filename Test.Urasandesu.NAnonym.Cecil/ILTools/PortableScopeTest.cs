/* 
 * File: PortableScopeTest.cs
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
using System.IO;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NUnit.Framework;
using Urasandesu.NAnonym.Cecil.ILTools;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Test;
using Assert = Urasandesu.NAnonym.Test.Assert;
using MC = Mono.Cecil;
using Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil;

namespace Test.Urasandesu.NAnonym.Cecil.ILTools
{
    [TestFixture]
    public class PortableScopeTest
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            TestHelper.TryDeleteFiles(".", "tmp*.dll");
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            TestHelper.TryDeleteFiles(".", "tmp*.dll");
        }

        [Test]
        public void CarryPortableScopeTest01()
        {
            TestHelper.UsingTempFile(tempFileName =>
            {
                var tempAssemblyNameDef = CreateTempAssemblyNameDef(tempFileName);
                var tempAssemblyDef = CreateTempAssemblyDef(tempAssemblyNameDef);

                var carryPortableScopeTest01Def = 
                    CreateTempType(tempAssemblyNameDef.Name, "CarryPortableScopeTest01", typeof(object), tempAssemblyDef);

                var carryPortableScopeTest01CtorDef = CreateTempCtor(carryPortableScopeTest01Def);
                carryPortableScopeTest01CtorDef.ExpressBody(
                gen =>
                {
                    gen.Eval(() => Dsl.Base());
                });

                var portableScopeALocal = CreateTempVariable("a", typeof(int), carryPortableScopeTest01CtorDef);

                var portableScopeA = 
                    CreateTempField(
                        carryPortableScopeTest01CtorDef.Name + PortableScope.NameDelimiter + PortableScope.NameDelimiter + "a" + PortableScope.NameDelimiter + "0", 
                        typeof(int), carryPortableScopeTest01Def);

                var portableScopeAAttributeDef =
                    CreateTempType(tempAssemblyNameDef.Name, "PortableScopeAAttribute", typeof(Attribute), tempAssemblyDef);

                var portableScopeAAttributeCtorDef = CreateTempCtor(portableScopeAAttributeDef);
                portableScopeAAttributeCtorDef.ExpressBody(
                gen =>
                {
                    gen.Eval(() => Dsl.Base());
                });

                var portableScopeAAttribute = CreateTempAttribute(portableScopeAAttributeCtorDef, portableScopeA);

                tempAssemblyDef.Write(tempFileName);

                var scope = carryPortableScopeTest01CtorDef.CarryPortableScope();
                int a = 10;
                scope.SetValue(() => a, a);
                Assert.IsTrue(scope.Contains(() => a));
                Assert.AreEqual(10, scope.GetValue(() => a));

                object carryPortableScopeTest01Instance = CreateTempInstance(Path.GetFullPath(tempFileName), carryPortableScopeTest01Def); 
                scope.DockWith(carryPortableScopeTest01Instance);
                Assert.AreEqual(10, scope.FetchValue(() => a, carryPortableScopeTest01Instance));
                Assert.NotNull(scope.methodDecl);
            });
        }

        [Test]
        public void CarryPortableScopeTest02()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
            {
                var tempAssemblyNameDef = CreateTempAssemblyNameDef(tempFileName);
                var tempAssemblyDef = CreateTempAssemblyDef(tempAssemblyNameDef);

                var carryPortableScopeTest02Def =
                    CreateTempType(tempAssemblyNameDef.Name, "CarryPortableScopeTest02", typeof(object), tempAssemblyDef);

                var carryPortableScopeTest02CtorDef = CreateTempCtor(carryPortableScopeTest02Def);
                carryPortableScopeTest02CtorDef.ExpressBody(
                gen =>
                {
                    gen.Eval(() => Dsl.Base());
                });

                var portableScopeALocal = CreateTempVariable("a", typeof(int), carryPortableScopeTest02CtorDef);

                var portableScopeA =
                    CreateTempField(
                        carryPortableScopeTest02CtorDef.Name + PortableScope.NameDelimiter + PortableScope.NameDelimiter + "a" + PortableScope.NameDelimiter + "0",
                        typeof(int), carryPortableScopeTest02Def);

                var portableScopeAAttributeDef =
                    CreateTempType(tempAssemblyNameDef.Name, "PortableScopeAAttribute", typeof(Attribute), tempAssemblyDef);

                var portableScopeAAttributeCtorDef = CreateTempCtor(portableScopeAAttributeDef);
                portableScopeAAttributeCtorDef.ExpressBody(
                gen =>
                {
                    gen.Eval(() => Dsl.Base());
                });

                var portableScopeAAttribute = CreateTempAttribute(portableScopeAAttributeCtorDef, portableScopeA);

                tempAssemblyDef.Write(tempFileName);

                var scope = carryPortableScopeTest02CtorDef.CarryPortableScope();
                {
                    int a = 10;
                    scope.SetValue(() => a, a);
                    Assert.IsTrue(scope.Contains(() => a));
                    Assert.AreEqual(10, scope.GetValue(() => a));
                }

                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
                testInfo.AssemblyFileName = Path.GetFullPath(tempFileName);
                testInfo.TypeFullName = carryPortableScopeTest02Def.FullName;
                testInfo.Scope = scope;
                testInfo.TestVerifier =
                    target =>
                    {
                        ((NewDomainTestInfoWithScope)target.TestInfo).Scope.DockWith(target.Instance);
                        int a = 0;
                        Assert.AreEqual(10, ((NewDomainTestInfoWithScope)target.TestInfo).Scope.FetchValue(() => a, target.Instance));
                        Assert.NotNull(((NewDomainTestInfoWithScope)target.TestInfo).Scope.methodDecl);
                    };

                return testInfo;
            }));
        }







        AssemblyNameDefinition CreateTempAssemblyNameDef(string tempFileName)
        {
            return new AssemblyNameDefinition(Path.GetFileNameWithoutExtension(tempFileName), new Version("1.0.0.0"));
        }

        AssemblyDefinition CreateTempAssemblyDef(AssemblyNameDefinition tempAssemblyNameDef)
        {
            return AssemblyDefinition.CreateAssembly(tempAssemblyNameDef, tempAssemblyNameDef.Name, ModuleKind.Dll);
        }

        TypeDefinition CreateTempType(string @namespace, string name, Type baseType, AssemblyDefinition declaringAssembly)
        {
            var typeDef = 
                new TypeDefinition(
                    @namespace, 
                    name, 
                    MC::TypeAttributes.AutoClass | MC::TypeAttributes.AnsiClass | 
                    MC::TypeAttributes.BeforeFieldInit | MC::TypeAttributes.Public,
                    declaringAssembly.MainModule.Import(baseType));
            declaringAssembly.MainModule.Types.Add(typeDef);
            return typeDef;
        }

        MethodDefinition CreateTempCtor(TypeDefinition declaringType)
        {
            var methodDef =
                new MethodDefinition(
                    ".ctor",
                    MC::MethodAttributes.Public |
                    MC::MethodAttributes.HideBySig |
                    MC::MethodAttributes.SpecialName |
                    MC::MethodAttributes.RTSpecialName,
                    declaringType.Module.Import(typeof(void)));
            declaringType.Methods.Add(methodDef);
            return methodDef;
        }

        FieldDefinition CreateTempField(string name, Type fieldType, TypeDefinition declaringType)
        {
            var fieldDef = 
                new FieldDefinition(
                    name,
                    MC::FieldAttributes.Private |
                    MC::FieldAttributes.SpecialName |
                    MC::FieldAttributes.RTSpecialName,
                    declaringType.Module.Import(fieldType));
            declaringType.Fields.Add(fieldDef);
            return fieldDef;
        }

        VariableDefinition CreateTempVariable(string name, Type variableType, MethodDefinition methodDef)
        {
            var variableDef = new VariableDefinition(name, methodDef.Module.Import(variableType));
            methodDef.Body.Variables.Add(variableDef);
            return variableDef;
        }

        CustomAttribute CreateTempAttribute(MethodReference ctorRef, FieldDefinition targetFieldDef)
        {
            var customAttribute = new CustomAttribute(ctorRef);
            targetFieldDef.CustomAttributes.Add(customAttribute);
            return customAttribute;
        }

        object CreateTempInstance(string assemblyFile, TypeDefinition targetTypeDef)
        {
            return Activator.CreateInstanceFrom(assemblyFile, targetTypeDef.FullName).Unwrap();
        }
    }
}

