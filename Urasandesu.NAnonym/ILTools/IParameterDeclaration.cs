namespace Urasandesu.NAnonym.ILTools
{
    public interface IParameterDeclaration
    {
        string Name { get; }
        ITypeDeclaration ParameterType { get; }
    }

}