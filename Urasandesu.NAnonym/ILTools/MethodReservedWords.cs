/* 
 * File: ReflectiveReservedWords.cs
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
    [MethodReservedWords]
    public interface IMethodReservedWords
    {
        [MethodReservedWordBase]
        void Base();

        [MethodReservedWordThis]
        object This();

        [MethodReservedWordDupAddOne]
        T DupAddOne<T>(T variable);

        [MethodReservedWordAddOneDup]
        T AddOneDup<T>(T variable);

        [MethodReservedWordSubOneDup]
        T SubOneDup<T>(T variable);

        [MethodReservedWordNew]
        object New(ConstructorInfo constructor, object parameter);

        [MethodReservedWordNew]
        T New<T>(ConstructorInfo constructor, params object[] parameters);

        [MethodReservedWordInvoke]
        object Invoke(MethodInfo method, params object[] parameters);

        [MethodReservedWordInvoke]
        object Invoke(object variable, MethodInfo method, params object[] parameters);

        [MethodReservedWordFtn]
        object Ftn(object variable, IMethodDeclaration methodDecl);

        [MethodReservedWordFtn]
        object Ftn(IMethodDeclaration methodDecl);

        [MethodReservedWordFtn]
        object Ftn(MethodInfo methodInfo);

        [MethodReservedWordIf]
        void If(bool condition);

        [MethodReservedWordEndIf]
        void EndIf();

        [MethodReservedWordEnd]
        void End();

        [MethodReservedWordReturn]
        void Return<T>(T variable);

        [MethodReservedWordLd]
        T Ld<T>(string variableName);

        [MethodReservedWordLd]
        object Ld(string variableName);

        [MethodReservedWordLd]
        object[] Ld(string[] variableNames);

        [MethodReservedWordLd]
        object[] Ld(string[] variableNames, int shift);

        [MethodReservedWordLdArg]
        object LdArg(int variableIndex);

        [MethodReservedWordLdArg]
        object[] LdArg(int[] variableIndexes);

        [MethodReservedWordSt]
        IMethodAllocReservedWords<T> St<T>(string variableName);

        [MethodReservedWordSt]
        IMethodAllocReservedWords St(string variableName);

        [MethodReservedWordAlloc]
        IMethodAllocReservedWords<T> Alloc<T>(T variable);

        [MethodReservedWordAlloc]
        IMethodAllocReservedWords Alloc(object variable);

        [MethodReservedWordX]
        T X<T>(T constant);

        [MethodReservedWordX]
        T X<T>(object constant);

        [MethodReservedWordCm]
        TValue Cm<TValue>(TValue constMember, Type declaringType);

        [MethodReservedWordAreEqual]
        bool AreEqual(object left, object right);
    }

    [AttributeUsage(AttributeTargets.Interface, Inherited = false)]
    public sealed class MethodReservedWordsAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordBaseAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordThisAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordDupAddOneAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordAddOneDupAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordSubOneDupAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordNewAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordInvokeAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordFtnAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordIfAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordEndIfAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordEndAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordReturnAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordLdAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordStAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordAllocAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordXAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordCmAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordLdArgAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordAreEqualAttribute : Attribute { }

    [MethodAllocReservedWords]
    public interface IMethodAllocReservedWords
    {
        [MethodAllocReservedWordAs]
        object As(object value);
    }

    [MethodAllocReservedWords]
    public interface IMethodAllocReservedWords<T>
    {
        [MethodAllocReservedWordAs]
        T As(T value);
    }

    [AttributeUsage(AttributeTargets.Interface, Inherited = false)]
    public sealed class MethodAllocReservedWordsAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodAllocReservedWordAsAttribute : Attribute { }
}
