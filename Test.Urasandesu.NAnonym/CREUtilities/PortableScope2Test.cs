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
                var tempAssemblyNameDef =
                    new AssemblyNameDefinition(Path.GetFileNameWithoutExtension(tempFileName), new Version("1.0.0.0"));
                var tempAssemblyDef =
                    AssemblyDefinition.CreateAssembly(tempAssemblyNameDef, tempAssemblyNameDef.Name, ModuleKind.Dll);

                var carryPortableScope2Test01Def =
                    new TypeDefinition(
                        tempAssemblyNameDef.Name,
                        "CarryPortableScope2Test01",
                        MC::TypeAttributes.AutoClass |
                        MC::TypeAttributes.AnsiClass |
                        MC::TypeAttributes.BeforeFieldInit |
                        MC::TypeAttributes.Public,
                        tempAssemblyDef.MainModule.Import(typeof(object)));
                tempAssemblyDef.MainModule.Types.Add(carryPortableScope2Test01Def);

                var carryPortableScope2Test01CtorDef =
                    new MethodDefinition(
                        ".ctor",
                        MC::MethodAttributes.Public |
                        MC::MethodAttributes.HideBySig |
                        MC::MethodAttributes.SpecialName |
                        MC::MethodAttributes.RTSpecialName,
                        tempAssemblyDef.MainModule.Import(typeof(void)));
                carryPortableScope2Test01Def.Methods.Add(carryPortableScope2Test01CtorDef);
                carryPortableScope2Test01CtorDef.ExpressBody(
                gen =>
                {
                    gen.Eval(_ => _.Base());
                });

                var portableScope2A = 
                    new FieldDefinition(
                        carryPortableScope2Test01CtorDef.Name + PortableScope2.NameDelimiter + PortableScope2.NameDelimiter + "a", 
                        MC::FieldAttributes.Private | 
                        MC::FieldAttributes.SpecialName | 
                        MC::FieldAttributes.RTSpecialName, 
                        tempAssemblyDef.MainModule.Import(typeof(int)));
                carryPortableScope2Test01Def.Fields.Add(portableScope2A);

                var portableScope2AAttributeDef =
                    new TypeDefinition(
                        tempAssemblyNameDef.Name,
                        "PortableScope2AAttribute",
                        MC::TypeAttributes.AutoClass |
                        MC::TypeAttributes.AnsiClass |
                        MC::TypeAttributes.BeforeFieldInit |
                        MC::TypeAttributes.Public,
                        tempAssemblyDef.MainModule.Import(typeof(Attribute)));
                tempAssemblyDef.MainModule.Types.Add(portableScope2AAttributeDef);

                var portableScope2AAttributeCtorDef =
                    new MethodDefinition(
                        ".ctor",
                        MC::MethodAttributes.Public |
                        MC::MethodAttributes.HideBySig |
                        MC::MethodAttributes.SpecialName |
                        MC::MethodAttributes.RTSpecialName,
                        tempAssemblyDef.MainModule.Import(typeof(void)));
                portableScope2AAttributeDef.Methods.Add(portableScope2AAttributeCtorDef);
                portableScope2AAttributeCtorDef.ExpressBody(
                gen =>
                {
                    gen.Eval(_ => _.Base());
                });
                
                var portableScope2AAttribute = new CustomAttribute(portableScope2AAttributeCtorDef);
                portableScope2A.CustomAttributes.Add(portableScope2AAttribute);

                tempAssemblyDef.Write(tempFileName);

                var scope = carryPortableScope2Test01CtorDef.CarryPortableScope2();
                int a = 10;
                scope.SetValue(() => a, a);
                Assert.IsTrue(scope.Contains(() => a));
                Assert.AreEqual(10, scope.GetValue(() => a));

                object carryPortableScope2Test01Instance = 
                    Activator.CreateInstanceFrom(
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName), carryPortableScope2Test01Def.FullName).Unwrap();
                scope.DockWith(carryPortableScope2Test01Instance);
                Assert.AreEqual(10, scope.FetchValue(() => a, carryPortableScope2Test01Instance));
            });
        }
    }
}
