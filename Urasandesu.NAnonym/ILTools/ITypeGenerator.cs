
using System;
using SR = System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{
    public interface ITypeGenerator : ITypeDeclaration, IMemberGenerator
    {
        IFieldGenerator AddField(string fieldName, Type type, SR::FieldAttributes attributes);
        IMethodBaseGenerator AddMethod(string name, SR::MethodAttributes attributes, Type returnType, Type[] parameterTypes);
    }

}