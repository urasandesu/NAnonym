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
