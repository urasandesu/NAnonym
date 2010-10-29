namespace Urasandesu.NAnonym.ILTools
{
    public interface IModuleDeclaration : IManuallyDeserializable
    {
        IAssemblyDeclaration Assembly { get; }
    }

}