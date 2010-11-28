
namespace Urasandesu.NAnonym.ILTools
{
    public interface IMemberDeclaration : IManuallyDeserializable
    {
        string Name { get; }
        object Source { get; }
    }
}