
using System.Collections.ObjectModel;
namespace Urasandesu.NAnonym.ILTools
{
    public interface IMethodBodyDeclaration
    {
        ReadOnlyCollection<ILocalDeclaration> Locals { get; }
        ReadOnlyCollection<IDirectiveDeclaration> Directives { get; }
    }
}