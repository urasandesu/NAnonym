using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{
    public sealed class Expressible
    {
        readonly MethodInfo BaseInfo;
        readonly MethodInfo ThisInfo;
        readonly MethodInfo AddlocInfo;
        readonly MethodInfo LdlocInfo;
        readonly MethodInfo StlocInfo;
        readonly MethodInfo LdsfldInfo;
        readonly MethodInfo LdfldInfo;
        readonly MethodInfo StsfldInfo;
        readonly MethodInfo StfldInfo1;
        readonly MethodInfo StfldInfo2;
        readonly MethodInfo DupAddOneInfo;
        readonly MethodInfo AddOneDupInfo;
        readonly MethodInfo SubOneDupInfo;
        readonly MethodInfo InvokeInfo;
        readonly MethodInfo IfInfo;
        readonly MethodInfo EndIfInfo;
        readonly MethodInfo LdargInfo;
        readonly MethodInfo ExpandInfo1;
        readonly MethodInfo ExpandInfo2;
        readonly MethodInfo ExtractInfo1;
        readonly MethodInfo ExtractInfo2;
        readonly MethodInfo ExtractInfo3;
        readonly MethodInfo EndInfo;
        readonly MethodInfo ReturnInfo;

        readonly MethodInfo LdInfo1;
        readonly MethodInfo LdInfo2;
        readonly MethodInfo LdInfo3;
        readonly MethodInfo StInfo1;
        readonly MethodInfo StInfo2;
        readonly MethodInfo XInfo;
        readonly MethodInfo CmInfo;

        public Expressible()
        {
            BaseInfo = TypeSavable.GetMethodInfo(() => Base);
            ThisInfo = TypeSavable.GetMethodInfo<object>(() => This);
            AddlocInfo = TypeSavable.GetMethodInfo<object, object>(() => Addloc).GetGenericMethodDefinition();
            LdlocInfo = TypeSavable.GetMethodInfo<object, object>(() => Ldloc).GetGenericMethodDefinition();
            StlocInfo = TypeSavable.GetMethodInfo<object, object, object>(() => Stloc).GetGenericMethodDefinition();
            LdsfldInfo = TypeSavable.GetMethodInfo<object, object>(() => Ldsfld).GetGenericMethodDefinition();
            LdfldInfo = TypeSavable.GetMethodInfo<object, object>(() => Ldfld).GetGenericMethodDefinition();
            StsfldInfo = TypeSavable.GetMethodInfo<object, object, object>(() => Stsfld).GetGenericMethodDefinition();
            StfldInfo1 = TypeSavable.GetMethodInfo<object, object, object>(() => Stfld).GetGenericMethodDefinition();
            StfldInfo2 = TypeSavable.GetMethodInfo<object, Type, object, object>(() => Stfld);
            DupAddOneInfo = TypeSavable.GetMethodInfo<object, object>(() => DupAddOne).GetGenericMethodDefinition();
            AddOneDupInfo = TypeSavable.GetMethodInfo<object, object>(() => AddOneDup).GetGenericMethodDefinition();
            SubOneDupInfo = TypeSavable.GetMethodInfo<object, object>(() => SubOneDup).GetGenericMethodDefinition();
            InvokeInfo = TypeSavable.GetMethodInfo<object, MethodInfo, object[], object>(() => Invoke);    
            IfInfo = TypeSavable.GetMethodInfo<bool>(() => If);
            EndIfInfo = TypeSavable.GetMethodInfo(() => EndIf);
            LdargInfo = TypeSavable.GetMethodInfo<object, object>(() => Ldarg).GetGenericMethodDefinition();
            ExpandInfo1 = TypeSavable.GetMethodInfo<object, object>(() => Expand).GetGenericMethodDefinition();
            ExpandInfo2 = TypeSavable.GetMethodInfo<object, Type, object>(() => Expand).GetGenericMethodDefinition();
            ExtractInfo1 = TypeSavable.GetMethodInfo<object, object>(() => Extract).GetGenericMethodDefinition();
            ExtractInfo2 = TypeSavable.GetMethodInfo<object, object>(() => Extract<object>).GetGenericMethodDefinition();
            ExtractInfo3 = TypeSavable.GetMethodInfo<object, Type, object>(() => Extract);
            EndInfo = TypeSavable.GetMethodInfo(() => End);
            ReturnInfo = TypeSavable.GetMethodInfo<object>(() => Return).GetGenericMethodDefinition();

            LdInfo1 = TypeSavable.GetMethodInfo<string, object>(() => Ld);
            LdInfo2 = TypeSavable.GetMethodInfo<string[], object[]>(() => Ld);
            LdInfo3 = TypeSavable.GetMethodInfo<string, object>(() => Ld<object>).GetGenericMethodDefinition();
            StInfo1 = TypeSavable.GetMethodInfo<object, StoreExpressible<object>>(() => St<object>).GetGenericMethodDefinition();
            StInfo2 = TypeSavable.GetMethodInfo<string, StoreExpressible>(() => St);
            XInfo = TypeSavable.GetMethodInfo<object, object>(() => X).GetGenericMethodDefinition();
            CmInfo = TypeSavable.GetMethodInfo<object, Type, object>(() => Cm).GetGenericMethodDefinition();
        }

        public bool IsBase(MethodInfo target)
        {
            return target == BaseInfo;
        }

        public bool IsThis(MethodInfo target)
        {
            return target == ThisInfo;
        }

        public bool IsAddloc(MethodInfo target)
        {
            if (AddlocInfo.Equivalent(target))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsLdloc(MethodInfo target)
        {
            return LdlocInfo.Equivalent(target);
        }

        public bool IsStloc(MethodInfo target)
        {
            if (StlocInfo.Equivalent(target))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsLdsfld(MethodInfo target)
        {
            if (LdsfldInfo.Equivalent(target))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsLdfld(MethodInfo target)
        {
            if (LdfldInfo.Equivalent(target))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsStsfld(MethodInfo target)
        {
            if (StsfldInfo.Equivalent(target))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsStfld(MethodInfo target)
        {
            if (StfldInfo2.Equivalent(target))
            {
                return true;
            }
            else if (StfldInfo1.Equivalent(target))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsDupAddOne(MethodInfo target)
        {
            if (DupAddOneInfo.Equivalent(target))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsAddOneDup(MethodInfo target)
        {
            if (AddOneDupInfo.Equivalent(target))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsSubOneDup(MethodInfo target)
        {
            if (SubOneDupInfo.Equivalent(target))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsInvoke(MethodInfo target)
        {
            return target == InvokeInfo;
        }

        public bool IsIf(MethodInfo target)
        {
            return target == IfInfo;
        }

        public bool IsEndIf(MethodInfo target)
        {
            return target == EndIfInfo;
        }

        public bool IsLdarg(MethodInfo target)
        {
            if (LdargInfo.Equivalent(target))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsExpand(MethodInfo target)
        {
            if (ExpandInfo1.Equivalent(target))
            {
                return true;
            }
            else if (ExpandInfo2.Equivalent(target))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsExtract(MethodInfo target)
        {
            if (ExtractInfo3.Equivalent(target))
            {
                return true;
            }
            else if (ExtractInfo1.Equivalent(target) || ExtractInfo2.Equivalent(target))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsEnd(MethodInfo target)
        {
            return target == EndInfo;
        }

        public bool IsReturn(MethodInfo target)
        {
            if (ReturnInfo.Equivalent(target))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsLd(MethodInfo target)
        {
            return LdInfo2 == target || LdInfo1 == target || LdInfo3.Equivalent(target);
        }

        public bool IsSt(MethodInfo target)
        {
            return StInfo2 == target || StInfo1.Equivalent(target);
        }

        public bool IsX(MethodInfo target)
        {
            return XInfo.Equivalent(target);
        }

        public bool IsCm(MethodInfo target)
        {
            return CmInfo.Equivalent(target);
        }



        public void Base()
        {
        }

        public object This()
        {
            return null;
        }

        [Obsolete]
        public void Addloc<T>(T variable, T value)
        {
        }

        [Obsolete]
        public T Ldloc<T>(T variable)
        {
            return default(T);
        }

        [Obsolete]
        public T Stloc<T>(T variable, T value)
        {
            return default(T);
        }

        [Obsolete]
        public T Ldsfld<T>(T variable)
        {
            return default(T);
        }

        [Obsolete]
        public T Ldfld<T>(T variable)
        {
            return default(T);
        }

        [Obsolete]
        public T Stsfld<T>(T variable, T value)
        {
            return default(T);
        }

        [Obsolete]
        public T Stfld<T>(T variable, T value)
        {
            return default(T);
        }

        [Obsolete]
        public object Stfld(object variable, Type castType, object value)
        {
            return null;
        }

        public T DupAddOne<T>(T variable)
        {
            return default(T);
        }

        public T AddOneDup<T>(T variable)
        {
            return default(T);
        }

        public T SubOneDup<T>(T variable)
        {
            return default(T);
        }

        public object Invoke(object variable, MethodInfo method, object[] parameters)
        {
            return null;
        }

        public void If(bool condition)
        {
        }

        public void EndIf()
        {
        }

        [Obsolete]
        public T Ldarg<T>(T variable)
        {
            return default(T);
        }

        // その場で展開するってことで結構制限厳しくしてもいいのかも？
        // Expand するってことで、副作用がある処理は書けないってことで問題ないと思われ。
        // ただ、これをそのまま LambdaExpression に
        [Obsolete]
        public T Expand<T>(T constant)
        {
            return default(T);
        }

        [Obsolete]
        public T Expand<T>(T staticMember, Type declaringType)
        {
            return default(T);
        }

        [Obsolete]
        public T Extract<T>(T constant)
        {
            return default(T);
        }

        [Obsolete]
        public TForCast Extract<TForCast>(object constant)
        {
            return default(TForCast);
        }

        [Obsolete]
        public object Extract(object constant, Type castType)
        {
            return null;
        }

        public void End()
        {
        }

        public void Return<T>(T variable)
        {
        }

        
        
        // 新 API テスト

        public T Ld<T>(string variableName)
        {
            return default(T);
        }

        public object Ld(string variableName)
        {
            return null;
        }

        public object[] Ld(string[] variableNames)
        {
            return null;
        }

        public StoreExpressible<T> St<T>(T variable)
        {
            return default(StoreExpressible<T>);
        }

        public StoreExpressible St(string variableName)
        {
            return null;
        }

        public T X<T>(T constant) 
        {
            return default(T);
        }


        public TValue Cm<TValue>(TValue constMember, Type declaringType)
        {
            return default(TValue);
        }
    }
}
