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
        public SRTypeDeclarationImpl(Type type)
            : base(type)
        {
            this.type = type;
            baseTypeDecl = type == typeof(object) ? null : new SRTypeDeclarationImpl(type.BaseType);
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

        public IModuleDeclaration Module { get { throw new NotImplementedException(); } }

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
}
