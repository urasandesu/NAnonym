
namespace Urasandesu.NAnonym.CREUtilities
{
    public interface IDirectiveDeclaration
    {
        OpCode OpCode { get; }
        object Operand { get; }
    }

}