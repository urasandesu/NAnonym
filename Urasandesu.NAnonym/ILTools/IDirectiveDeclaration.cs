
namespace Urasandesu.NAnonym.ILTools
{
    public interface IDirectiveDeclaration
    {
        OpCode OpCode { get; }
        object Operand { get; }
    }

}