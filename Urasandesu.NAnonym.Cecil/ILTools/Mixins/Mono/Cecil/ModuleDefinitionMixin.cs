using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil;

namespace Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil
{
    public static class ModuleDefinitionMixin
    {
        public static ModuleDefinition Duplicate(this ModuleDefinition source)
        {
            throw new NotImplementedException();
        }

        public static ITypeGenerator AddType(this ModuleDefinition moduleDef, string fullName)
        {
            Required.NotDefault(fullName, () => fullName);

            var periodLastIndex = fullName.LastIndexOf('.');
            var @namespace = periodLastIndex < 0 ? string.Empty : fullName.Substring(0, periodLastIndex);
            var name = periodLastIndex < 0 ? fullName : fullName.Substring(periodLastIndex + 1);
            var defaultTypeAttribute = TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | TypeAttributes.Public;
            var typeDef = new TypeDefinition(@namespace, name, defaultTypeAttribute, moduleDef.Import(typeof(object)));
            moduleDef.Types.Add(typeDef);

            return new MCTypeGeneratorImpl(typeDef);
        }

        public static ITypeGenerator ReadType(this ModuleDefinition moduleDef, string fullName)
        {
            return new MCTypeGeneratorImpl(moduleDef.GetType(fullName));
        }
    }
}
