
using System;
namespace Urasandesu.NAnonym.ILTools
{
    public interface IFieldDeclaration : IMemberDeclaration
    {
        Type FieldType { get; }
    }

}