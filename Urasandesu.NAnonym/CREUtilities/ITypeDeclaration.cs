using System;

namespace Urasandesu.NAnonym.CREUtilities
{
    public interface ITypeDeclaration : IMemberDeclaration
    {
        string FullName { get; }
        string AssemblyQualifiedName { get; }
        ITypeDeclaration BaseType { get; }
        IModuleDeclaration Module { get; }
        IConstructorDeclaration GetConstructor(Type[] types);

    }

}