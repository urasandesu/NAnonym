using System;

namespace Urasandesu.NAnonym.CREUtilities
{
    public interface ITypeDeclaration : IMemberDeclaration
    {
        string FullName { get; }
        ITypeDeclaration BaseType { get; }
        IConstructorDeclaration GetConstructor(Type[] types);

    }

}