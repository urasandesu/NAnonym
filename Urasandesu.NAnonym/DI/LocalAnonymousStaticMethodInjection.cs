using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools;
using SR = System.Reflection;
using SRE = System.Reflection.Emit;
using Urasandesu.NAnonym.Mixins.System.Reflection;
using Urasandesu.NAnonym.ILTools.Mixins.System;
using Urasandesu.NAnonym.ILTools.Mixins.System.Reflection.Emit;

namespace Urasandesu.NAnonym.DI
{
    abstract class LocalAnonymousStaticMethodInjection : LocalMethodInjection
    {
        public new static LocalAnonymousStaticMethodInjection CreateInstance<TBase>(TypeBuilder tbaseTypeBuilder, TargetMethodInfo targetMethodInfo)
        {
            if (targetMethodInfo.Mode == SetupModes.Override)
            {
                throw new NotImplementedException();
            }
            else if (targetMethodInfo.Mode == SetupModes.Implement)
            {
                return new LocalImplementAnonymousStaticMethodInjection(tbaseTypeBuilder, targetMethodInfo);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public LocalAnonymousStaticMethodInjection(TypeBuilder tbaseTypeBuilder, TargetMethodInfo targetMethodInfo)
            : base(tbaseTypeBuilder, targetMethodInfo)
        {
        }

        public abstract override void Apply(FieldBuilder cachedSettingBuilder, FieldBuilder cachedMethodBuilder);
    }
}
