using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{
    public sealed class Expressible
    {
        readonly MethodInfo BaseInfo;
        readonly MethodInfo AddlocInfo;
        readonly MethodInfo StlocInfo;
        readonly MethodInfo StfldInfo;
        readonly MethodInfo ExpandStfldInfo;
        readonly MethodInfo DupAddOneInfo;
        readonly MethodInfo AddOneDupInfo;
        readonly MethodInfo SubOneDupInfo;
        readonly MethodInfo ExpandInvokeInfo;
        readonly MethodInfo IfInfo;
        readonly MethodInfo EndIfInfo;
        readonly MethodInfo ExpandLdargInfo;
        readonly MethodInfo ExpandLdargsInfo;
        readonly MethodInfo ExpandInfo;
        readonly MethodInfo EndInfo;
        readonly MethodInfo ReturnInfo;

        public Expressible()
        {
            BaseInfo = TypeSavable.GetMethodInfo(() => Base);
            AddlocInfo = TypeSavable.GetMethodInfo<object, object>(() => Addloc).GetGenericMethodDefinition();
            StlocInfo = TypeSavable.GetMethodInfo<object, object, object>(() => Stloc).GetGenericMethodDefinition();
            StfldInfo = TypeSavable.GetMethodInfo<object, object, object>(() => Stfld).GetGenericMethodDefinition();
            ExpandStfldInfo = TypeSavable.GetMethodInfo<object, Type, object, object>(() => ExpandStfld);
            DupAddOneInfo = TypeSavable.GetMethodInfo<object, object>(() => DupAddOne).GetGenericMethodDefinition();
            AddOneDupInfo = TypeSavable.GetMethodInfo<object, object>(() => AddOneDup).GetGenericMethodDefinition();
            SubOneDupInfo = TypeSavable.GetMethodInfo<object, object>(() => SubOneDup).GetGenericMethodDefinition();
            ExpandInvokeInfo = TypeSavable.GetMethodInfo<object, MethodInfo, object[], object>(() => ExpandInvoke);
            IfInfo = TypeSavable.GetMethodInfo<bool>(() => If);
            EndIfInfo = TypeSavable.GetMethodInfo(() => EndIf);
            ExpandLdargInfo = TypeSavable.GetMethodInfo<string, object>(() => ExpandLdarg<object>).GetGenericMethodDefinition();
            ExpandLdargsInfo = TypeSavable.GetMethodInfo<string[], object[]>(() => ExpandLdargs);
            ExpandInfo = TypeSavable.GetMethodInfo<object, object>(() => Expand).GetGenericMethodDefinition();
            EndInfo = TypeSavable.GetMethodInfo(() => End);
            ReturnInfo = TypeSavable.GetMethodInfo<object>(() => Return).GetGenericMethodDefinition();
        }

        public bool IsBase(MethodInfo target)
        {
            return target == BaseInfo;
        }

        public bool IsAddloc(MethodInfo target)
        {
            if (target.Name == AddlocInfo.Name && target == AddlocInfo.MakeGenericMethod(target.GetGenericArguments()))
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
            if (target.Name == StlocInfo.Name && target == StlocInfo.MakeGenericMethod(target.GetGenericArguments()))
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
            if (target.Name == StfldInfo.Name && target == StfldInfo.MakeGenericMethod(target.GetGenericArguments()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsExpandStfld(MethodInfo target)
        {
            return target == ExpandStfldInfo;
        }

        public bool IsDupAddOne(MethodInfo target)
        {
            if (target.Name == DupAddOneInfo.Name && target == DupAddOneInfo.MakeGenericMethod(target.GetGenericArguments()))
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
            if (target.Name == AddOneDupInfo.Name && target == AddOneDupInfo.MakeGenericMethod(target.GetGenericArguments()))
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
            if (target.Name == SubOneDupInfo.Name && target == SubOneDupInfo.MakeGenericMethod(target.GetGenericArguments()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsExpandInvoke(MethodInfo target)
        {
            return target == ExpandInvokeInfo;
        }

        public bool IsIf(MethodInfo target)
        {
            return target == IfInfo;
        }

        public bool IsEndIf(MethodInfo target)
        {
            return target == EndIfInfo;
        }

        public bool IsExpandLdarg(MethodInfo target)
        {
            if (target.Name == ExpandLdargInfo.Name && target == ExpandLdargInfo.MakeGenericMethod(target.GetGenericArguments()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsExpandLdargs(MethodInfo target)
        {
            return target == ExpandLdargsInfo;
        }

        public bool IsExpand(MethodInfo target)
        {
            if (target.Name == ExpandInfo.Name && target == ExpandInfo.MakeGenericMethod(target.GetGenericArguments()))
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
            if (target.Name == ReturnInfo.Name && target == ReturnInfo.MakeGenericMethod(target.GetGenericArguments()))
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
        
        public void Addloc<T>(T variable, T value)
        {
        }

        public T Stloc<T>(T variable, T value)
        {
            return default(T);
        }

        public T Stfld<T>(T variable, T value)
        {
            return default(T);
        }

        public object ExpandStfld(object variable, Type variableType, object value)
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

        public object ExpandInvoke(object variable, MethodInfo method, object[] parameters)
        {
            return null;
        }

        public void If(bool condition)
        {
        }

        public void EndIf()
        {
        }

        public object[] ExpandLdargs(string[] names)
        {
            return new object[] { };
        }

        public T ExpandLdarg<T>(string name)
        {
            return default(T);
        }

        // その場で展開するってことで結構制限厳しくしてもいいのかも？
        // Expand するってことで、副作用がある処理は書けないってことで問題ないと思われ。
        // ただ、これをそのまま LambdaExpression に
        public T Expand<T>(T value)
        {
            return default(T);
        }

        public void End()
        {
        }

        public void Return<T>(T variable)
        {
        }
    }
}
