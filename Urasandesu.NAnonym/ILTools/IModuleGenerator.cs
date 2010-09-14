namespace Urasandesu.NAnonym.ILTools
{
    public interface IModuleGenerator : IModuleDeclaration
    {
        new IAssemblyGenerator Assembly { get; }
    }

}