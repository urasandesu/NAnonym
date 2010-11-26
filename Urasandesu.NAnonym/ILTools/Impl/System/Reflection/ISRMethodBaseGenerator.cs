using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Reflection.Emit;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    interface ISRMethodBaseGenerator
    {
        ILGenerator GetILGenerator();
        ParameterBuilder DefineParameter(int position, ParameterAttributes attributes, string parameterName);
    }
}
