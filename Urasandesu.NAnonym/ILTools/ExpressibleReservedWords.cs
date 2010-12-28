/* 
 * File: ExpressibleReservedWords.cs
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
using System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{
    [ExpressibleReservedWords]
    public interface IExpressibleReservedWords
    {
        [ExpressibleReservedWordBase]
        void Base();

        [ExpressibleReservedWordThis]
        object This();

        [ExpressibleReservedWordDupAddOne]
        T DupAddOne<T>(T variable);

        [ExpressibleReservedWordAddOneDup]
        T AddOneDup<T>(T variable);

        [ExpressibleReservedWordSubOneDup]
        T SubOneDup<T>(T variable);

        [ExpressibleReservedWordNew]
        object New(ConstructorInfo constructor, object parameter);

        [ExpressibleReservedWordNew]
        object New(ConstructorInfo constructor, params object[] parameters);

        [ExpressibleReservedWordInvoke]
        object Invoke(object variable, MethodInfo method, object[] parameters);

        [ExpressibleReservedWordFtn]
        object Ftn(object variable, IMethodDeclaration methodDecl);

        [ExpressibleReservedWordFtn]
        object Ftn(IMethodDeclaration methodDecl);

        [ExpressibleReservedWordIf]
        void If(bool condition);

        [ExpressibleReservedWordEndIf]
        void EndIf();

        [ExpressibleReservedWordEnd]
        void End();

        [ExpressibleReservedWordReturn]
        void Return<T>(T variable);

        [ExpressibleReservedWordLd]
        T Ld<T>(string variableName);

        [ExpressibleReservedWordLd]
        object Ld(string variableName);

        [ExpressibleReservedWordLd]
        object[] Ld(string[] variableNames);

        [ExpressibleReservedWordSt]
        IExpressibleAllocReservedWords<T> St<T>(string variableName);

        [ExpressibleReservedWordSt]
        IExpressibleAllocReservedWords St(string variableName);

        [ExpressibleReservedWordAlloc]
        IExpressibleAllocReservedWords<T> Alloc<T>(T variable);

        [ExpressibleReservedWordAlloc]
        IExpressibleAllocReservedWords Alloc(object variable);

        [ExpressibleReservedWordX]
        T X<T>(T constant);

        [ExpressibleReservedWordX]
        T X<T>(object constant);

        [ExpressibleReservedWordCm]
        TValue Cm<TValue>(TValue constMember, Type declaringType);
    }

    [AttributeUsage(AttributeTargets.Interface, Inherited = false)]
    public sealed class ExpressibleReservedWordsAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ExpressibleReservedWordBaseAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ExpressibleReservedWordThisAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ExpressibleReservedWordDupAddOneAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ExpressibleReservedWordAddOneDupAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ExpressibleReservedWordSubOneDupAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ExpressibleReservedWordNewAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ExpressibleReservedWordInvokeAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ExpressibleReservedWordFtnAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ExpressibleReservedWordIfAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ExpressibleReservedWordEndIfAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ExpressibleReservedWordEndAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ExpressibleReservedWordReturnAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ExpressibleReservedWordLdAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ExpressibleReservedWordStAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ExpressibleReservedWordAllocAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ExpressibleReservedWordXAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ExpressibleReservedWordCmAttribute : Attribute { }

    [ExpressibleAllocReservedWords]
    public interface IExpressibleAllocReservedWords
    {
        [ExpressibleAllocReservedWordAs]
        object As(object value);
    }

    [ExpressibleAllocReservedWords]
    public interface IExpressibleAllocReservedWords<T>
    {
        [ExpressibleAllocReservedWordAs]
        T As(T value);
    }

    [AttributeUsage(AttributeTargets.Interface, Inherited = false)]
    public sealed class ExpressibleAllocReservedWordsAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ExpressibleAllocReservedWordAsAttribute : Attribute { }
}
