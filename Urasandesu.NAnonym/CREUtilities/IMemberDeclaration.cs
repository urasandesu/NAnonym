using System;
using Mono.Cecil;
using System.Reflection;

namespace Urasandesu.NAnonym.CREUtilities
{
    public interface IMemberDeclaration : IDeserializableManually
    {
        string Name { get; }
    }
}