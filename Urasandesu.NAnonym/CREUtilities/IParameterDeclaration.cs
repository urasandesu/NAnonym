namespace Urasandesu.NAnonym.CREUtilities
{
    public interface IParameterDeclaration
    {
        string Name { get; }
        ITypeDeclaration ParameterType { get; }
    }

}