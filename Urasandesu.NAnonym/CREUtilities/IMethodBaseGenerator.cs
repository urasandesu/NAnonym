using System.Collections.ObjectModel;
using System.Reflection;

namespace Urasandesu.NAnonym.CREUtilities
{
    public interface IMethodBaseGenerator : IMethodBaseDeclaration, IMemberGenerator
    {
        new IMethodBodyGenerator Body { get; }
        new ITypeGenerator DeclaringType { get; }
        new ReadOnlyCollection<IParameterGenerator> Parameters { get; }
        IPortableScopeItem AddPortableScopeItem(FieldInfo fieldInfo);
    }

}