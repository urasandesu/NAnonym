using System.Collections.ObjectModel;
namespace Urasandesu.NAnonym.ILTools
{
    public interface IMethodBodyGenerator : IMethodBodyDeclaration
    {
        IILOperator ILOperator { get; }
        new ReadOnlyCollection<ILocalGenerator> Locals { get; }
        new ReadOnlyCollection<IDirectiveGenerator> Directives { get; }
    }

}