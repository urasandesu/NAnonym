namespace Urasandesu.NAnonym.ILTools
{
    public interface ILocalDeclaration
    {
        string Name { get; }
        ITypeDeclaration Type { get; }
        int Index { get; }
    }

}