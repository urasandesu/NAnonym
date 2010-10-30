using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    public abstract class DependencyField
    {
        DependencyClass @class;
        LambdaExpression reference;
        Type type;
        public DependencyField(DependencyClass @class, LambdaExpression reference, Type type)
        {
            this.@class = @class;
            this.reference = reference;
            this.type = type;
        }

        protected void As(LambdaExpression exp)
        {
            @class.TargetFieldInfoSet.Add(new TargetFieldInfo(reference, exp, type));
        }
    }
}
