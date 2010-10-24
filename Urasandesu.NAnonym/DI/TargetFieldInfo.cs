using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Urasandesu.NAnonym.ILTools;
using System.Linq.Expressions;

namespace Urasandesu.NAnonym.DI
{
    public class TargetFieldInfo
    {
        public LambdaExpression Reference { get; set; }
        public LambdaExpression Expression { get; set; }
        public Type Type { get; set; }

        public TargetFieldInfo()
        {
        }

        public TargetFieldInfo(LambdaExpression reference, LambdaExpression expression, Type type)
        {
            Reference = reference;
            Expression = expression;
            Type = type;
        }
    }
}
