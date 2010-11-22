using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Urasandesu.NAnonym.ILTools;
using System.Linq.Expressions;

namespace Urasandesu.NAnonym.DI
{
    public class InjectionFieldInfo
    {
        public LambdaExpression FieldReference { get; set; }
        public LambdaExpression Initializer { get; set; }
        public Type FieldType { get; set; }

        public InjectionFieldInfo()
        {
        }

        public InjectionFieldInfo(LambdaExpression fieldReference, LambdaExpression initializer, Type fieldType)
        {
            FieldReference = fieldReference;
            Initializer = initializer;
            FieldType = fieldType;
        }
    }
}
