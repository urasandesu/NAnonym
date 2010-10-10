using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    abstract class GlobalMethodInjection
    {
        public static GlobalMethodInjection CreateInstance<TBase>(TypeDefinition tbaseTypeDef, TargetMethodInfo targetMethodInfo)
        {
            // MEMO: 先に NewMethod の定義先情報で振り分けたほうが共通化できる処理が多そう。
            if ((targetMethodInfo.NewMethodType & NewMethodType.AnonymousInstance) == NewMethodType.AnonymousInstance)
            {
                return GlobalAnonymousInstanceMethodInjection.CreateInstance<TBase>(tbaseTypeDef, targetMethodInfo);
            }
            else if ((targetMethodInfo.NewMethodType & NewMethodType.AnonymousStatic) == NewMethodType.AnonymousStatic)
            {
                return GlobalAnonymousStaticMethodInjection.CreateInstance<TBase>(tbaseTypeDef, targetMethodInfo);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected readonly TypeDefinition tbaseTypeDef;
        protected readonly TargetMethodInfo targetMethodInfo;
        public GlobalMethodInjection(TypeDefinition tbaseTypeDef, TargetMethodInfo targetMethodInfo)
        {
            this.tbaseTypeDef = tbaseTypeDef;
            this.targetMethodInfo = targetMethodInfo;
        }

        public abstract void Apply(FieldDefinition cachedSettingDef, FieldDefinition cachedMethodDef);
    }
}
