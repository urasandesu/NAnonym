namespace Urasandesu.NAnonym.CREUtilities
{
    public interface IModuleDeclaration : IDeserializableManually
    {
        IAssemblyDeclaration Assembly { get; }
    }

}