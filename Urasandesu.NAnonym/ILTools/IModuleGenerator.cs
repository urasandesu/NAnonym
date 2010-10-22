using System;
using SR = System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{
    public interface IModuleGenerator : IModuleDeclaration
    {
        new IAssemblyGenerator Assembly { get; }
        ITypeGenerator AddType(string fullName, SR::TypeAttributes attr, Type parent);
    }

}