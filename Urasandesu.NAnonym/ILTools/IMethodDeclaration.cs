
namespace Urasandesu.NAnonym.ILTools
{
    public interface IMethodDeclaration : IMethodBaseDeclaration
    {
        ITypeDeclaration ReturnType { get; }

    }

}