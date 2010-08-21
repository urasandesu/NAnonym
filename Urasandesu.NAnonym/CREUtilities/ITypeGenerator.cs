
using System;
using SR = System.Reflection;

namespace Urasandesu.NAnonym.CREUtilities
{
    public interface ITypeGenerator : ITypeDeclaration, IMemberGenerator
    {
        IFieldGenerator AddField(string fieldName, Type type, SR::FieldAttributes attributes);
    }

}