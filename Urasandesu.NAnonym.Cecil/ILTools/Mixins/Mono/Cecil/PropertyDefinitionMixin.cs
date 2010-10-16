using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil
{
    public static class PropertyDefinitionMixin
    {
        public static PropertyDefinition Duplicate(this PropertyDefinition source)
        {
            // Property のコピーは外側だけのコピーだけではなく、中身の get_ メソッド、 set_ メソッドのコピーが必要！
            var destination = new PropertyDefinition(source.Name, source.Attributes, source.PropertyType);
            return destination;
        }
    }
}
