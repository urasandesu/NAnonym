using System;
using System.Reflection;
using System.Collections.ObjectModel;

namespace Urasandesu.NAnonym.ILTools
{
    public interface ITypeDeclaration : IMemberDeclaration
    {
        string FullName { get; }
        string AssemblyQualifiedName { get; }
        ITypeDeclaration BaseType { get; }
        IModuleDeclaration Module { get; }
        ReadOnlyCollection<IFieldDeclaration> Fields { get; }
        ReadOnlyCollection<IConstructorDeclaration> Constructors { get; }
        ReadOnlyCollection<IMethodDeclaration> Methods { get; }
        IConstructorDeclaration GetConstructor(Type[] types);
        new Type Source { get; }
    }
}