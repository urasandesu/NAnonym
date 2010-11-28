using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Mono.Cecil;

namespace Urasandesu.NAnonym.Cecil.Mixins.System.Reflection
{
    public static class CallingConventionsMixin
    {
        public static MethodCallingConvention ToCecil(this CallingConventions source)
        {
            var destination = MethodCallingConvention.Default;
            if ((source & CallingConventions.Standard) == CallingConventions.Standard)
            {
                destination = MethodCallingConvention.StdCall;
            }
            else if ((source & CallingConventions.VarArgs) == CallingConventions.VarArgs)
            {
                destination = MethodCallingConvention.VarArg;
            }
            else if ((source & CallingConventions.Any) == CallingConventions.Any)
            {
                throw new NotSupportedException();
            }
            else if ((source & CallingConventions.HasThis) == CallingConventions.HasThis)
            {
                destination = MethodCallingConvention.ThisCall;
            }
            else if ((source & CallingConventions.ExplicitThis) == CallingConventions.ExplicitThis)
            {
                throw new NotSupportedException();
            }
            else
            {
                throw new NotSupportedException();
            }
            return destination;
        }
    }
}
