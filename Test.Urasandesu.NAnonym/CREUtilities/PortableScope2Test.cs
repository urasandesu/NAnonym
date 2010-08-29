using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Assert = Urasandesu.NAnonym.Test.Assert;
using Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.CREUtilities;
using Urasandesu.NAnonym.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MC = Mono.Cecil;
using System.Reflection;
using System.Reflection.Emit;
using SR = System.Reflection;
using Urasandesu.NAnonym;
using Test.Urasandesu.NAnonym.Etc;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using Urasandesu.NAnonym.Test;
using System.Threading;
using Test.Urasandesu.NAnonym.DI;
using OpCodes = Urasandesu.NAnonym.CREUtilities.OpCodes;


namespace Test.Urasandesu.NAnonym.CREUtilities
{
    [TestFixture]
    public class PortableScope2Test
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            TestHelper.TryDeleteFiles(".", "*.tmp");
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            TestHelper.TryDeleteFiles(".", "*.tmp");
        }

        [Test]
        public void CarryPortableScope2Test01()
        {
            TestHelper.UsingTempFile(tempFileName =>
            {
                var tempAssemblyNameDef = CreateTempAssemblyNameDef(tempFileName);
                var tempAssemblyDef = CreateTempAssemblyDef(tempAssemblyNameDef);

                var carryPortableScope2Test01Def = 
                    CreateTempType(tempAssemblyNameDef.Name, "CarryPortableScope2Test01", typeof(object), tempAssemblyDef);

                var carryPortableScope2Test01CtorDef = CreateTempCtor(carryPortableScope2Test01Def);
                carryPortableScope2Test01CtorDef.ExpressBody(
                gen =>
                {
                    gen.Eval(_ => _.Base());
                });

                var portableScope2A = 
                    CreateTempField(
                        carryPortableScope2Test01CtorDef.Name + PortableScope2.NameDelimiter + PortableScope2.NameDelimiter + "a", 
                        typeof(int), carryPortableScope2Test01Def);

                var portableScope2AAttributeDef =
                    CreateTempType(tempAssemblyNameDef.Name, "PortableScope2AAttribute", typeof(Attribute), tempAssemblyDef);

                var portableScope2AAttributeCtorDef = CreateTempCtor(portableScope2AAttributeDef);
                portableScope2AAttributeCtorDef.ExpressBody(
                gen =>
                {
                    gen.Eval(_ => _.Base());
                });

                var portableScope2AAttribute = CreateTempAttribute(portableScope2AAttributeCtorDef, portableScope2A);

                tempAssemblyDef.Write(tempFileName);

                var scope = carryPortableScope2Test01CtorDef.CarryPortableScope2();
                int a = 10;
                scope.SetValue(() => a, a);
                Assert.IsTrue(scope.Contains(() => a));
                Assert.AreEqual(10, scope.GetValue(() => a));

                object carryPortableScope2Test01Instance = CreateTempInstance(Path.GetFullPath(tempFileName), carryPortableScope2Test01Def); 
                scope.DockWith(carryPortableScope2Test01Instance);
                Assert.AreEqual(10, scope.FetchValue(() => a, carryPortableScope2Test01Instance));
                Assert.NotNull(scope.methodDecl);
            });
        }

        [Test]
        public void CarryPortableScope2Test02()
        {
            TestHelper.UsingTempFile(tempFileName =>
            NewDomainTest.Transfer(() =>
            {
                var tempAssemblyNameDef = CreateTempAssemblyNameDef(tempFileName);
                var tempAssemblyDef = CreateTempAssemblyDef(tempAssemblyNameDef);

                var carryPortableScope2Test02Def =
                    CreateTempType(tempAssemblyNameDef.Name, "CarryPortableScope2Test02", typeof(object), tempAssemblyDef);

                var carryPortableScope2Test02CtorDef = CreateTempCtor(carryPortableScope2Test02Def);
                carryPortableScope2Test02CtorDef.ExpressBody(
                gen =>
                {
                    gen.Eval(_ => _.Base());
                });

                var portableScope2A =
                    CreateTempField(
                        carryPortableScope2Test02CtorDef.Name + PortableScope2.NameDelimiter + PortableScope2.NameDelimiter + "a",
                        typeof(int), carryPortableScope2Test02Def);

                var portableScope2AAttributeDef =
                    CreateTempType(tempAssemblyNameDef.Name, "PortableScope2AAttribute", typeof(Attribute), tempAssemblyDef);

                var portableScope2AAttributeCtorDef = CreateTempCtor(portableScope2AAttributeDef);
                portableScope2AAttributeCtorDef.ExpressBody(
                gen =>
                {
                    gen.Eval(_ => _.Base());
                });

                var portableScope2AAttribute = CreateTempAttribute(portableScope2AAttributeCtorDef, portableScope2A);

                tempAssemblyDef.Write(tempFileName);

                var scope = carryPortableScope2Test02CtorDef.CarryPortableScope2();
                {
                    int a = 10;
                    scope.SetValue(() => a, a);
                    Assert.IsTrue(scope.Contains(() => a));
                    Assert.AreEqual(10, scope.GetValue(() => a));
                }

                var testInfo = new NewDomainTestInfo();
                testInfo.AssemblyFileName = Path.GetFullPath(tempFileName);
                testInfo.TypeFullName = carryPortableScope2Test02Def.FullName;
                testInfo.Scope2 = scope;
                testInfo.TestVerifier =
                    target =>
                    {
                        target.TestInfo.Scope2.DockWith(target.Instance);
                        int a = 0;
                        Assert.AreEqual(10, target.TestInfo.Scope2.FetchValue(() => a, target.Instance));
                        Assert.NotNull(target.TestInfo.Scope2.methodDecl);
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
