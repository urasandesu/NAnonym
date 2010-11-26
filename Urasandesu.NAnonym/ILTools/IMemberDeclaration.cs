using System;
//using Mono.Cecil;
using System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{
    public interface IMemberDeclaration : IManuallyDeserializable
    {
        string Name { get; }
        object Source { get; }
    }
}