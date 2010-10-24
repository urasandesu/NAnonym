using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    [Flags]
    public enum NewMethodType
    {
        None = 0,
        Anonymous = 1,
        Instance = 2,
        Static = 4,
        AnonymousInstance = Anonymous | Instance,
        AnonymousStatic = Anonymous | Static,
    }
}
