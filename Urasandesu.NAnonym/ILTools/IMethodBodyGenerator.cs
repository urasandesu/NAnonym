using System.Collections.ObjectModel;
using System;
namespace Urasandesu.NAnonym.ILTools
{
    public interface IMethodBodyGenerator : IMethodBodyDeclaration
    {
        IILOperator ILOperator { get; }
        new ReadOnlyCollection<ILocalGenerator> Locals { get; }
        new ReadOnlyCollection<IDirectiveGenerator> Directives { get; }
        ILocalGenerator AddLocal(ILocalGenerator localGen);
    }

}