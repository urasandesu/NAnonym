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
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{
    public sealed class Expressible
    {
        public static readonly MethodInfo GetTypeFromHandle = TypeSavable.GetStaticMethod<RuntimeTypeHandle, Type>(() => Type.GetTypeFromHandle);
        public static readonly MethodInfo GetMethodFromHandle = TypeSavable.GetStaticMethod<RuntimeMethodHandle, MethodBase>(() => MethodBase.GetMethodFromHandle);

        public readonly MethodInfo BaseInfo;
        public readonly MethodInfo ThisInfo;
        public readonly MethodInfo DupAddOneInfo;
        public readonly MethodInfo AddOneDupInfo;
        public readonly MethodInfo SubOneDupInfo;
        public readonly MethodInfo NewInfo;
        public readonly MethodInfo InvokeInfo;
        public readonly MethodInfo FtnInfo1;
        public readonly MethodInfo FtnInfo2;
        public readonly MethodInfo IfInfo;
        public readonly MethodInfo EndIfInfo;
        public readonly MethodInfo EndInfo;
        public readonly MethodInfo ReturnInfo;

        public readonly MethodInfo LdInfo1;
        public readonly MethodInfo LdInfo2;
        public readonly MethodInfo LdInfo3;
        public readonly MethodInfo AllocInfo1;
        public readonly MethodInfo AllocInfo2;
        public readonly MethodInfo StInfo1;
        public readonly MethodInfo StInfo2;
        public readonly MethodInfo XInfo1;
        public readonly MethodInfo XInfo2;
        public readonly MethodInfo CmInfo;

        public Expressible()
        {
            BaseInfo = TypeSavable.GetInstanceMethod<Expressible>(_ => _.Base);
            ThisInfo = TypeSavable.GetInstanceMethod<Expressible, object>(_ => _.This);
            DupAddOneInfo = TypeSavable.GetInstanceMethod<Expressible, object, object>(_ => _.DupAddOne).GetGenericMethodDefinition();
            AddOneDupInfo = TypeSavable.GetInstanceMethod<Expressible, object, object>(_ => _.AddOneDup).GetGenericMethodDefinition();
            SubOneDupInfo = TypeSavable.GetInstanceMethod<Expressible, object, object>(_ => _.SubOneDup).GetGenericMethodDefinition();
            NewInfo = TypeSavable.GetInstanceMethod<Expressible, ConstructorInfo, object, object>(_ => _.New);
            InvokeInfo = TypeSavable.GetInstanceMethod<Expressible, object, MethodInfo, object[], object>(_ => _.Invoke);
            FtnInfo1 = TypeSavable.GetInstanceMethod<Expressible, IMethodDeclaration, object>(_ => _.Ftn);
            FtnInfo2 = TypeSavable.GetInstanceMethod<Expressible, object, IMethodDeclaration, object>(_ => _.Ftn);
            IfInfo = TypeSavable.GetInstanceMethod<Expressible, bool>(_ => _.If);
            EndIfInfo = TypeSavable.GetInstanceMethod<Expressible>(_ => _.EndIf);
            EndInfo = TypeSavable.GetInstanceMethod<Expressible>(_ => _.End);
            ReturnInfo = TypeSavable.GetInstanceMethod<Expressible, object>(_ => _.Return).GetGenericMethodDefinition();

            LdInfo1 = TypeSavable.GetInstanceMethod<Expressible, string, object>(_ => _.Ld);
            LdInfo2 = TypeSavable.GetInstanceMethod<Expressible, string[], object[]>(_ => _.Ld);
            LdInfo3 = TypeSavable.GetInstanceMethod<Expressible, string, object>(_ => _.Ld<object>).GetGenericMethodDefinition();
            AllocInfo1 = TypeSavable.GetInstanceMethod<Expressible, object, AllocExpressible<object>>(_ => _.Alloc<object>).GetGenericMethodDefinition();
            AllocInfo2 = TypeSavable.GetInstanceMethod<Expressible, object, AllocExpressible>(_ => _.Alloc);
            StInfo1 = TypeSavable.GetInstanceMethod<Expressible, string, AllocExpressible<object>>(_ => _.St<object>).GetGenericMethodDefinition();
            StInfo2 = TypeSavable.GetInstanceMethod<Expressible, string, AllocExpressible>(_ => _.St);
            XInfo1 = TypeSavable.GetInstanceMethod<Expressible, object, object>(_ => _.X).GetGenericMethodDefinition();
            XInfo2 = TypeSavable.GetInstanceMethod<Expressible, object, object>(_ => _.X<object>).GetGenericMethodDefinition();
            CmInfo = TypeSavable.GetInstanceMethod<Expressible, object, Type, object>(_ => _.Cm).GetGenericMethodDefinition();
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
            return DupAddOneInfo.Equivalent(target);
        }

        public bool IsAddOneDup(MethodInfo target)
        {
            return AddOneDupInfo.Equivalent(target);
        }

        public bool IsSubOneDup(MethodInfo target)
        {
            return SubOneDupInfo.Equivalent(target);
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
            return ReturnInfo.Equivalent(target);
        }

        public bool IsLd(MethodInfo target)
        {
            return LdInfo2 == target || LdInfo1 == target || LdInfo3.Equivalent(target);
        }

        public bool IsAlloc(MethodInfo target)
        {
            return AllocInfo2 == target || AllocInfo1.Equivalent(target);
        }

        public bool IsSt(MethodInfo target)
        {
            return StInfo2 == target || StInfo1.EquivalentWithoutGenericArguments(target);
        }

        public bool IsX(MethodInfo target)
        {
            return XInfo1.Equivalent(target) || XInfo2.EquivalentWithoutGenericArguments(target);
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

        public AllocExpressible<T> St<T>(string variableName)
        {
            return default(AllocExpressible<T>);
        }

        public AllocExpressible St(string variableName)
        {
            return null;
        }

        public AllocExpressible<T> Alloc<T>(T variable)
        {
            return default(AllocExpressible<T>);
        }

        public AllocExpressible Alloc(object variable)
        {
            return default(AllocExpressible);
        }

        public T X<T>(T constant) 
        {
            return default(T);
        }

        public T X<T>(object constant)
        {
            return default(T);
        }


        public TValue Cm<TValue>(TValue constMember, Type declaringType)
        {
            return default(TValue);
        }
    }
}

