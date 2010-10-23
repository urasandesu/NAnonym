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
    abstract class LocalAnonymousInstanceMethodInjection : LocalMethodInjection
    {
        public new static LocalAnonymousInstanceMethodInjection CreateInstance<TBase>(TypeBuilder tbaseTypeBuilder, TargetMethodInfo targetMethodInfo)
        {
            if (targetMethodInfo.Mode == SetupModes.Override)
            {
                throw new NotImplementedException();
            }
            else if (targetMethodInfo.Mode == SetupModes.Implement)
            {
                return new LocalImplementAnonymousInstanceMethodInjection(tbaseTypeBuilder, targetMethodInfo);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public LocalAnonymousInstanceMethodInjection(TypeBuilder tbaseTypeBuilder, TargetMethodInfo targetMethodInfo)
            : base(tbaseTypeBuilder, targetMethodInfo)
        {
        }

        public abstract override void Apply(FieldBuilder cachedSettingBuilder, FieldBuilder cachedMethodBuilder);
    }
}
