
namespace Urasandesu.NAnonym.CREUtilities
{
    public interface IParameterGenerator : IParameterDeclaration
    {
        new ITypeGenerator ParameterType { get; }
    }

}