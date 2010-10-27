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
            get { return base.Assembly as IAssemblyGenerator; }
        }

        public ITypeGenerator AddType(string fullName, TypeAttributes attr, Type parent)
        {
            var typeBuilder = moduleBuilder.DefineType(fullName, attr, parent);
            return new SRTypeGeneratorImpl(typeBuilder);
        }

        #endregion
    }
}
