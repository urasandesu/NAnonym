using System;
//using Mono.Cecil;
using System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{
    public interface IMemberDeclaration : IDeserializableManually
    {
        string Name { get; }
    }
}