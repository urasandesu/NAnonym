using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{
    public sealed class Expressible
    {
        readonly MethodInfo BaseInfo;
        readonly MethodInfo ThisInfo;
        readonly MethodInfo AddlocInfo;
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
        readonly MethodInfo ExpandInfo;
        readonly MethodInfo ExtractInfo1;
        readonly MethodInfo ExtractInfo2;
        readonly MethodInfo ExtractInfo3;
        readonly MethodInfo EndInfo;
        readonly MethodInfo ReturnInfo;

        public Expressible()
        {
            BaseInfo = TypeSavable.GetMethodInfo(() => Base);
            ThisInfo = TypeSavable.GetMethodInfo<object>(() => This);
            AddlocInfo = TypeSavable.GetMethodInfo<object, object>(() => Addloc).GetGenericMethodDefinition();
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
            ExpandInfo = TypeSavable.GetMethodInfo<object, object>(() => Expand).GetGenericMethodDefinition();
            ExtractInfo1 = TypeSavable.GetMethodInfo<object, object>(() => Extract).GetGenericMethodDefinition();
            ExtractInfo2 = TypeSavable.GetMethodInfo<object, object>(() => Extract<object>).GetGenericMethodDefinition();
            ExtractInfo3 = TypeSavable.GetMethodInfo<object, Type, object>(() => Extract);
            EndInfo = TypeSavable.GetMethodInfo(() => End);
            ReturnInfo = TypeSavable.GetMethodInfo<object>(() => Return).GetGenericMethodDefinition();
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
            if (target.Name == AddlocInfo.Name && 
                target.IsGenericMethod &&
                target.GetGenericArguments().Length == AddlocInfo.GetGenericArguments().Length &&
                target == AddlocInfo.MakeGenericMethod(target.GetGenericArguments()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsStloc(MethodInfo target)
        {
            if (target.Name == StlocInfo.Name && 
                target.IsGenericMethod &&
                target.GetGenericArguments().Length == StlocInfo.GetGenericArguments().Length &&
                target == StlocInfo.MakeGenericMethod(target.GetGenericArguments()))
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
            if (target.Name == LdsfldInfo.Name && 
                target.IsGenericMethod &&
                target.GetGenericArguments().Length == LdsfldInfo.GetGenericArguments().Length &&
                target == LdsfldInfo.MakeGenericMethod(target.GetGenericArguments()))
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
            if (target.Name == LdfldInfo.Name && 
                target.IsGenericMethod &&
                target.GetGenericArguments().Length == LdfldInfo.GetGenericArguments().Length &&
                target == LdfldInfo.MakeGenericMethod(target.GetGenericArguments()))
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
            if (target.Name == StsfldInfo.Name && 
                target.IsGenericMethod &&
                target.GetGenericArguments().Length == StsfldInfo.GetGenericArguments().Length &&
                target == StsfldInfo.MakeGenericMethod(target.GetGenericArguments()))
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
            if (target == StfldInfo2)
            {
                return true;
            }
            else if (target.Name == StfldInfo1.Name && 
                target.IsGenericMethod &&
                target.GetGenericArguments().Length == StfldInfo1.GetGenericArguments().Length &&
                target == StfldInfo1.MakeGenericMethod(target.GetGenericArguments()))
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
            if (target.Name == DupAddOneInfo.Name && 
                target.IsGenericMethod &&
                target.GetGenericArguments().Length == DupAddOneInfo.GetGenericArguments().Length &&
                target == DupAddOneInfo.MakeGenericMethod(target.GetGenericArguments()))
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
            if (target.Name == AddOneDupInfo.Name && 
                target.IsGenericMethod &&
                target.GetGenericArguments().Length == AddOneDupInfo.GetGenericArguments().Length &&
                target == AddOneDupInfo.MakeGenericMethod(target.GetGenericArguments()))
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
            if (target.Name == SubOneDupInfo.Name && 
                target.IsGenericMethod &&
                target.GetGenericArguments().Length == SubOneDupInfo.GetGenericArguments().Length &&
                target == SubOneDupInfo.MakeGenericMethod(target.GetGenericArguments()))
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
            if (target.Name == LdargInfo.Name && 
                target.IsGenericMethod &&
                target.GetGenericArguments().Length == LdargInfo.GetGenericArguments().Length &&
                target == LdargInfo.MakeGenericMethod(target.GetGenericArguments()))
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
            if (target.Name == ExpandInfo.Name && 
                target.IsGenericMethod &&
                target.GetGenericArguments().Length == ExpandInfo.GetGenericArguments().Length &&
                target == ExpandInfo.MakeGenericMethod(target.GetGenericArguments()))
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
            if (target == ExtractInfo3)
            {
                return true;
            }
            else if (
                target.Name == ExtractInfo1.Name &&
                target.IsGenericMethod &&
                target.GetGenericArguments().Length == ExtractInfo1.GetGenericArguments().Length &&
                target == ExtractInfo1.MakeGenericMethod(target.GetGenericArguments()) 
                ||
                target.Name == ExtractInfo2.Name &&
                target.IsGenericMethod &&
                target.GetGenericArguments().Length == ExtractInfo2.GetGenericArguments().Length &&
                target == ExtractInfo2.MakeGenericMethod(target.GetGenericArguments()))
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
            if (target.Name == ReturnInfo.Name && 
                target.IsGenericMethod &&
                target.GetGenericArguments().Length == ReturnInfo.GetGenericArguments().Length &&
                target == ReturnInfo.MakeGenericMethod(target.GetGenericArguments()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public void Base()
        {
        }

        public object This()
        {
            return null;
        }
        
        public void Addloc<T>(T variable, T value)
        {
        }

        public T Stloc<T>(T variable, T value)
        {
            return default(T);
        }

        public T Ldsfld<T>(T variable)
        {
            return default(T);
        }

        public T Ldfld<T>(T variable)
        {
            return default(T);
        }

        public T Stsfld<T>(T variable, T value)
        {
            return default(T);
        }

        public T Stfld<T>(T variable, T value)
        {
            return default(T);
        }

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

        public T Ldarg<T>(T variable)
        {
            return default(T);
        }

        // その場で展開するってことで結構制限厳しくしてもいいのかも？
        // Expand するってことで、副作用がある処理は書けないってことで問題ないと思われ。
        // ただ、これをそのまま LambdaExpression に
        public T Expand<T>(T constant)
        {
            return default(T);
        }

        public T Extract<T>(T constant)
        {
            return default(T);
        }

        public TForCast Extract<TForCast>(object constant)
        {
            return default(TForCast);
        }

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
    }
}
