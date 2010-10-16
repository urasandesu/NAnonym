
namespace Urasandesu.NAnonym.ILTools
{
    public interface IDirectiveDeclaration
    {
        OpCode OpCode { get; }
        object RawOperand { get; }
        object NAnonymOperand { get; }
        object ClrOperand { get; }
    }

}