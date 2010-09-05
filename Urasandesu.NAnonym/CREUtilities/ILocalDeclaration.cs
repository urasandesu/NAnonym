namespace Urasandesu.NAnonym.CREUtilities
{
    public interface ILocalDeclaration
    {
        string Name { get; }
        ITypeDeclaration Type { get; }
        int Index { get; }
    }

}