/* 
 * File: Expressible.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2010 Akira Sugiura
 *  
 *  This software is MIT License.
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */
 

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{
    public sealed class Expressible
    {
        public static readonly MethodInfo GetTypeFromHandle =
            typeof(Type).GetMethod(
                                "GetTypeFromHandle",
                                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                                null,
                                new Type[] 
                                {
                                    typeof(RuntimeTypeHandle) 
                                },
                                null);
        public static readonly MethodInfo GetMethodFromHandle =
            typeof(MethodBase).GetMethod(
                                "GetMethodFromHandle",
                                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                                null,
                                new Type[]
                                {
                                    typeof(RuntimeMethodHandle)
                                },
                                null);

        readonly MethodInfo BaseInfo;
        readonly MethodInfo ThisInfo;
        readonly MethodInfo DupAddOneInfo;
        readonly MethodInfo AddOneDupInfo;
        readonly MethodInfo SubOneDupInfo;
        readonly MethodInfo NewInfo;
        readonly MethodInfo InvokeInfo;
        readonly MethodInfo FtnInfo1;
        readonly MethodInfo FtnInfo2;
        readonly MethodInfo IfInfo;
        readonly MethodInfo EndIfInfo;
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
            DupAddOneInfo = TypeSavable.GetMethodInfo<object, object>(() => DupAddOne).GetGenericMethodDefinition();
            AddOneDupInfo = TypeSavable.GetMethodInfo<object, object>(() => AddOneDup).GetGenericMethodDefinition();
            SubOneDupInfo = TypeSavable.GetMethodInfo<object, object>(() => SubOneDup).GetGenericMethodDefinition();
            NewInfo = TypeSavable.GetMethodInfo<ConstructorInfo, object, object>(() => New);
            InvokeInfo = TypeSavable.GetMethodInfo<object, MethodInfo, object[], object>(() => Invoke);
            FtnInfo1 = TypeSavable.GetMethodInfo<IMethodDeclaration, object>(() => Ftn);
            FtnInfo2 = TypeSavable.GetMethodInfo<object, IMethodDeclaration, object>(() => Ftn);
            IfInfo = TypeSavable.GetMethodInfo<bool>(() => If);
            EndIfInfo = TypeSavable.GetMethodInfo(() => EndIf);
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

        public bool IsNew(MethodInfo target)
        {
            return NewInfo.Equivalent(target);
        }

        public bool IsInvoke(MethodInfo target)
        {
            return target == InvokeInfo;
        }

        public bool IsFtn(MethodInfo target)
        {
            return FtnInfo1.Equivalent(target) || FtnInfo2.Equivalent(target);
        }

        public bool IsIf(MethodInfo target)
        {
            return target == IfInfo;
        }

        public bool IsEndIf(MethodInfo target)
        {
            return target == EndIfInfo;
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

        public object New(ConstructorInfo constructor, object parameter)
        {
            return null;
        }

        public object Invoke(object variable, MethodInfo method, object[] parameters)
        {
            return null;
        }

        public object Ftn(object variable, IMethodDeclaration methodDecl)
        {
            return null;
        }

        public object Ftn(IMethodDeclaration methodDecl)
        {
            return null;
        }

        public void If(bool condition)
        {
        }

        public void EndIf()
        {
        }

        public void End()
        {
        }

        public void Return<T>(T variable)
        {
        }

        
        
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

