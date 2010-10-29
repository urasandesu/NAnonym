using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.ILTools
{
    public interface IPortableScopeItem : ILocalDeclaration, IFieldDeclaration, IManuallyDeserializable
    {
        object Value { get; set; }
        object Source { get; }
        new string Name { get; }
    }
}
