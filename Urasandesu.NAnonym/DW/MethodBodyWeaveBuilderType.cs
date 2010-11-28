using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DW
{
    [Flags]
    public enum MethodBodyWeaveBuilderType
    {
        None = 0,
        Anonymous = 1,
        Instance = 2,
        Static = 4,
        Base = 8,
        AnonymousInstance = Anonymous | Instance,
        AnonymousInstanceWithBase = AnonymousInstance | Base,
        AnonymousStatic = Anonymous | Static,
        AnonymousStaticWithBase = AnonymousStatic | Base,
    }
}
