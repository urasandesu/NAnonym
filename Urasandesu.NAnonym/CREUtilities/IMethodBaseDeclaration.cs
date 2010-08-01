using System.Collections.ObjectModel;

namespace Urasandesu.NAnonym.CREUtilities
{
    public interface IMethodBaseDeclaration : IMemberDeclaration
    {
        //string Name { get; }
        IMethodBodyDeclaration Body { get; }
        ITypeDeclaration DeclaringType { get; }
        ReadOnlyCollection<IParameterDeclaration> Parameters { get; }
    }

}