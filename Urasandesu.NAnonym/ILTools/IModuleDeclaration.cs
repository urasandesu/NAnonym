namespace Urasandesu.NAnonym.ILTools
{
    public interface IModuleDeclaration : IDeserializableManually
    {
        IAssemblyDeclaration Assembly { get; }
    }

}