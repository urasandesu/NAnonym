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
    abstract class LocalMethodInjection
    {
        public static LocalMethodInjection CreateInstance<TBase>(TypeBuilder localClassTypeBuilder, TargetMethodInfo targetMethodInfo)
        {
            // MEMO: 先に NewMethod の定義先情報で振り分けたほうが共通化できる処理が多そう。
            if ((targetMethodInfo.NewMethodType & NewMethodType.AnonymousInstance) == NewMethodType.AnonymousInstance)
            {
                return LocalAnonymousInstanceMethodInjection.CreateInstance<TBase>(localClassTypeBuilder, targetMethodInfo);
            }
            else if ((targetMethodInfo.NewMethodType & NewMethodType.AnonymousStatic) == NewMethodType.AnonymousStatic)
            {
                return LocalAnonymousStaticMethodInjection.CreateInstance<TBase>(localClassTypeBuilder, targetMethodInfo);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected readonly TypeBuilder localClassTypeBuilder;
        protected readonly TargetMethodInfo targetMethodInfo;
        public LocalMethodInjection(TypeBuilder localClassTypeBuilder, TargetMethodInfo targetMethodInfo)
        {
            this.localClassTypeBuilder = localClassTypeBuilder;
            this.targetMethodInfo = targetMethodInfo;
        }

        public abstract void Apply(FieldBuilder cachedSettingBuilder, FieldBuilder cachedMethodBuilder);
    }
}
