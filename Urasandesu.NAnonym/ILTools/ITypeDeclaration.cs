using System;
using System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{
    public interface ITypeDeclaration : IMemberDeclaration
    {
        string FullName { get; }
        string AssemblyQualifiedName { get; }
        ITypeDeclaration BaseType { get; }
        IModuleDeclaration Module { get; }
        IFieldDeclaration GetField(string name, BindingFlags bindingAttr);
        IFieldDeclaration[] GetFields(BindingFlags attr);
        IConstructorDeclaration GetConstructor(Type[] types);

    }

}