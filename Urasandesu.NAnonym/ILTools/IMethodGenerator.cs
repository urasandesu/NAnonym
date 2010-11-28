namespace Urasandesu.NAnonym.ILTools
{
    public interface IMethodGenerator : IMethodDeclaration, IMethodBaseGenerator
    {
        new ITypeGenerator ReturnType { get; }
    }

}