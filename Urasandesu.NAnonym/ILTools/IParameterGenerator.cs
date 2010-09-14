
namespace Urasandesu.NAnonym.ILTools
{
    public interface IParameterGenerator : IParameterDeclaration
    {
        new ITypeGenerator ParameterType { get; }
    }

}