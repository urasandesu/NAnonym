using System;
using System.Reflection;

namespace Urasandesu.NAnonym.CREUtilities
{
    public interface ITypeDeclaration : IMemberDeclaration
    {
        string FullName { get; }
        string AssemblyQualifiedName { get; }
        ITypeDeclaration BaseType { get; }
        IModuleDeclaration Module { get; }
        IFieldDeclaration[] GetFields(BindingFlags attr);
        IConstructorDeclaration GetConstructor(Type[] types);

    }

}