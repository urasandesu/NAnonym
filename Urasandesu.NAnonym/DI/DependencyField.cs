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
        LambdaExpression fieldReference;
        Type fieldType;
        public DependencyField(DependencyClass @class, LambdaExpression fieldReference, Type fieldType)
        {
            this.@class = @class;
            this.fieldReference = fieldReference;
            this.fieldType = fieldType;
        }

        protected void As(LambdaExpression initializer)
        {
            @class.FieldSet.Add(new InjectionFieldInfo(fieldReference, initializer, fieldType));
        }
    }
}
