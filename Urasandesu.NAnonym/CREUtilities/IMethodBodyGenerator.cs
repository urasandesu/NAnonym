using System.Collections.ObjectModel;
namespace Urasandesu.NAnonym.CREUtilities
{
    public interface IMethodBodyGenerator : IMethodBodyDeclaration
    {
        IILOperator GetILOperator();
        new ReadOnlyCollection<ILocalGenerator> Locals { get; }
        new ReadOnlyCollection<IDirectiveGenerator> Directives { get; }
    }

}