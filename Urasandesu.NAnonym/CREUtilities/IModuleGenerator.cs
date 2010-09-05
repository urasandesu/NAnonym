namespace Urasandesu.NAnonym.CREUtilities
{
    public interface IModuleGenerator : IModuleDeclaration
    {
        new IAssemblyGenerator Assembly { get; }
    }

}