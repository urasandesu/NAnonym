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
        //IFieldDeclaration GetField(string name, BindingFlags bindingAttr);
        //IFieldDeclaration[] GetFields(BindingFlags attr);
        IConstructorDeclaration GetConstructor(Type[] types);

    }

}