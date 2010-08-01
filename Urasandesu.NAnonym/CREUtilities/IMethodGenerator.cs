namespace Urasandesu.NAnonym.CREUtilities
{
    public interface IMethodGenerator : IMethodDeclaration, IMethodBaseGenerator
    {
        new ITypeGenerator ReturnType { get; }

    }

}