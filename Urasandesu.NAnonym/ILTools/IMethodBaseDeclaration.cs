using System.Collections.ObjectModel;
using System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{
    public interface IMethodBaseDeclaration : IMemberDeclaration
    {
        IMethodBodyDeclaration Body { get; }
        ITypeDeclaration DeclaringType { get; }
        ReadOnlyCollection<IParameterDeclaration> Parameters { get; }
        IPortableScopeItem NewPortableScopeItem(PortableScopeItemRawData itemRawData, object value);
    }

}