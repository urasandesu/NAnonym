using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRTypeDeclarationImpl : SRMemberDeclarationImpl, ITypeDeclaration
    {
        readonly Type type;

        readonly ITypeDeclaration baseTypeDecl;
        IModuleDeclaration moduleDecl;
        public SRTypeDeclarationImpl(Type type)
            : base(type)
        {
            this.type = type;
            baseTypeDecl = type == typeof(object) ? null : new SRTypeDeclarationImpl(type.BaseType);
            var moduleBuilder = ExportModule(type);
            if (moduleBuilder != null)
            {
                moduleDecl = new SRModuleGeneratorImple(moduleBuilder);
            }
        }

        static ModuleBuilder ExportModule(Type type)
        {
            if (!(type is TypeBuilder)) return null;

            var typeBuilderType = typeof(TypeBuilder);
            var m_moduleField = typeBuilderType.GetField("m_module", BindingFlags.NonPublic | BindingFlags.Instance);
            return m_moduleField == null ? default(ModuleBuilder) : (ModuleBuilder)m_moduleField.GetValue(type);
        }

        public object Source { get { throw new NotImplementedException(); } }

        public string FullName
        {
            get { throw new NotImplementedException(); }
        }

        public string AssemblyQualifiedName { get { throw new NotImplementedException(); } }

        public ITypeDeclaration BaseType
        {
            get { return baseTypeDecl; }
        }

        public IModuleDeclaration Module { get { return moduleDecl; } }

        public IConstructorDeclaration GetConstructor(Type[] types)
        {
            return new SRConstructorDeclarationImpl(type.GetConstructor(types));
        }

        #region ITypeDeclaration メンバ


        public IFieldDeclaration[] GetFields(global::System.Reflection.BindingFlags attr)
        {
            throw new NotImplementedException();
        }

        public virtual IFieldDeclaration GetField(string name, BindingFlags bindingAttr)
        {
            return new SRFieldDeclarationImpl(type.GetField(name, bindingAttr));
        }

        #endregion
    }

    class SRModuleDeclarationImpl : IModuleDeclaration
    {
        Module module;
        IAssemblyDeclaration assemblyDecl;
        public SRModuleDeclarationImpl(Module module)
        {
            this.module = module;
            var assemblyBuilder = ExportAssembly(module);
            if (assemblyBuilder != null)
            {
                assemblyDecl = new SRAssemblyGeneratorImpl(assemblyBuilder);
            }
        }

        static AssemblyBuilder ExportAssembly(Module module)
        {
            if (!(module is ModuleBuilder)) return null;

            var moduleBuilderType = typeof(ModuleBuilder);
            var m_assemblyBuilder = moduleBuilderType.GetField("m_assemblyBuilder", BindingFlags.NonPublic | BindingFlags.Instance);
            return m_assemblyBuilder == null ? default(AssemblyBuilder) : (AssemblyBuilder)m_assemblyBuilder.GetValue(module);
        }

        #region IModuleDeclaration メンバ

        public IAssemblyDeclaration Assembly
        {
            get { return assemblyDecl; }
        }

        #endregion

        #region IDeserializableManually メンバ

        public void OnDeserialized(StreamingContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    class SRModuleGeneratorImple : SRModuleDeclarationImpl, IModuleGenerator
    {
        ModuleBuilder moduleBuilder;
        public SRModuleGeneratorImple(ModuleBuilder moduleBuilder)
            : base(moduleBuilder)
        {
            this.moduleBuilder = moduleBuilder;
        }

        #region IModuleGenerator メンバ

        public new IAssemblyGenerator Assembly
        {
            get { throw new NotImplementedException(); }
        }

        public ITypeGenerator AddType(string fullName, TypeAttributes attr, Type parent)
        {
            var typeBuilder = moduleBuilder.DefineType(fullName, attr, parent);
            return new SRTypeGeneratorImpl(typeBuilder);
        }

        #endregion
    }

    class SRAssemblyDeclarationImpl : IAssemblyDeclaration
    {
        Assembly assembly;
        public SRAssemblyDeclarationImpl(Assembly assembly)
        {
            this.assembly = assembly;
        }

        #region IDeserializableManually メンバ

        public void OnDeserialized(StreamingContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    class SRAssemblyGeneratorImpl : SRAssemblyDeclarationImpl, IAssemblyGenerator
    {
        AssemblyBuilder assemblyBuilder;
        public SRAssemblyGeneratorImpl(AssemblyBuilder assemblyBuilder)
            : base(assemblyBuilder)
        {
            this.assemblyBuilder = assemblyBuilder;
        }

        #region IAssemblyGenerator メンバ

        public IAssemblyGenerator CreateInstance(AssemblyName name)
        {
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
            return new SRAssemblyGeneratorImpl(assemblyBuilder);
        }

        public IModuleGenerator AddModule(string name)
        {
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(name);
            return new SRModuleGeneratorImple(moduleBuilder);
        }

        #endregion
    }
}
