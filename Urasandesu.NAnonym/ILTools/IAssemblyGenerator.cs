using System.Reflection;
namespace Urasandesu.NAnonym.ILTools
{
    public interface IAssemblyGenerator : IAssemblyDeclaration
    {
        IAssemblyGenerator CreateInstance(AssemblyName name);
        IModuleGenerator AddModule(string name);
    }

}