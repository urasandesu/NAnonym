using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools;
using Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.Mixins.System;
using MC = Mono.Cecil;
using System.Reflection.Emit;
using SRE = System.Reflection.Emit;
using UND = Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil;
using Urasandesu.NAnonym.ILTools;
using TypeAnalyzer = Urasandesu.NAnonym.Cecil.ILTools.TypeAnalyzer;
using Urasandesu.NAnonym.ILTools.Mixins.Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil;

namespace Urasandesu.NAnonym.Cecil.DI
{

    public abstract class GlobalField : DependencyField
    {
        public GlobalField(DependencyClass @class, LambdaExpression reference, Type type)
            : base(@class, reference, type)
        {
        }
    }

    public class GlobalField<T> : GlobalField
    {
        public GlobalField(GlobalClass @class, Expression<Func<T>> reference)
            : base(@class, reference, typeof(T))
        {
        }

        public void As(Expression<Func<Expressible, T>> exp)
        {
            base.As(exp);
        }
    }

    public class GlobalFieldInt : GlobalField
    {
        public GlobalFieldInt(GlobalClass @class, Expression<Func<int>> reference)
            : base(@class, reference, typeof(int))
        {
        }

        public void As(int arg)
        {
            Expression<Func<Expressible, int>> exp = _ => _.X(arg);
            base.As(exp);
        }
    }
}
