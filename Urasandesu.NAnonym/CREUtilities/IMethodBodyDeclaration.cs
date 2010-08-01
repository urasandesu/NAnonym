
using System.Collections.ObjectModel;
namespace Urasandesu.NAnonym.CREUtilities
{
    public interface IMethodBodyDeclaration
    {
        ReadOnlyCollection<ILocalDeclaration> Locals { get; }
        ReadOnlyCollection<IDirectiveDeclaration> Directives { get; }
    }
}