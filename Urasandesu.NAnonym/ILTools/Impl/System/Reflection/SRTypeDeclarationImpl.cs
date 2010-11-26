using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRTypeDeclarationImpl : SRMemberDeclarationImpl, ITypeDeclaration
    {
        readonly Type type;

        readonly ITypeDeclaration baseTypeDecl;
        IModuleDeclaration moduleDecl;
        List<IFieldDeclaration> listFields;
        ReadOnlyCollection<IFieldDeclaration> fields;
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
            listFields = new List<IFieldDeclaration>();
            if (!(type is TypeBuilder))
            {
                listFields.AddRange(type.GetFields(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
                    .Select(field => (IFieldDeclaration)new SRFieldDeclarationImpl(field)));
            }
            fields = new ReadOnlyCollection<IFieldDeclaration>(listFields);
        }

        static ModuleBuilder ExportModule(Type type)
        {
            if (!(type is TypeBuilder)) return null;

            var typeBuilderType = typeof(TypeBuilder);
            var m_moduleField = typeBuilderType.GetField("m_module", BindingFlags.NonPublic | BindingFlags.Instance);
            return m_moduleField == null ? default(ModuleBuilder) : (ModuleBuilder)m_moduleField.GetValue(type);
        }

        internal new Type Source { get { return (Type)base.Source; } }
        

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

        #region ITypeDeclaration メンバ

        public ReadOnlyCollection<IFieldDeclaration> Fields
        {
            get { return fields; }
        }

        #endregion
    }
}
