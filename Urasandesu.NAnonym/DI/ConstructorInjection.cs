using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    class ConstructorInjection : Injection
    {
        public HashSet<InjectionFieldInfo> FieldSet { get; private set; }
        public virtual Type DeclaringType { get { throw new NotImplementedException(); } }
        public virtual string GetFieldNameForDeclaringType(Type declaringType)
        {
            throw new NotImplementedException();
        }
        public ConstructorInjection(HashSet<InjectionFieldInfo> fieldSet)
        {
            FieldSet = fieldSet;
        }

        public override void Apply()
        {
            if (0 < FieldSet.Count)
            {
                ApplyContent();
            }
        }

        protected virtual void ApplyContent()
        {
            throw new NotImplementedException();
        }
    }
}
