using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil.Cil;

namespace Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil.Cil
{
    public static class ExceptionHandlerMixin
    {
        public static ExceptionHandler Duplicate(this ExceptionHandler source)
        {
            var destination = new ExceptionHandler(source.HandlerType);
            destination.CatchType = source.CatchType;
            destination.FilterEnd = source.FilterEnd;
            destination.FilterStart = source.FilterStart;
            destination.HandlerEnd = source.HandlerEnd;
            destination.HandlerStart = source.HandlerStart;
            destination.HandlerType = source.HandlerType;
            destination.TryEnd = source.TryEnd;
            destination.TryStart = source.TryStart;
            return destination;
        }
    }
}
