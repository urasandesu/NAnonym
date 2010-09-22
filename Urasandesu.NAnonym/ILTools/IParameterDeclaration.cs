namespace Urasandesu.NAnonym.ILTools
{
    public interface IParameterDeclaration
    {
        string Name { get; }
        int Position { get; }
        ITypeDeclaration ParameterType { get; }
    }

}