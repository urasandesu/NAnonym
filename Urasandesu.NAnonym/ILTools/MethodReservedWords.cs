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
using System.ComponentModel;

namespace Urasandesu.NAnonym.ILTools
{
    [ILReservedWords]
    [MethodReservedWords]
    public class Dsl
    {
        protected Dsl() { }

        [ILReservedWordLd]
        public static T Load<T>(FieldInfo field){ throw new NotSupportedException(); }

        [ILReservedWordLd]
        public static T Load<T>(object instance, IFieldDeclaration field){ throw new NotSupportedException(); }

        [ILReservedWordSt]
        public static IILAllocReservedWords<T> Store<T>(FieldInfo field){ throw new NotSupportedException(); }

        [MethodReservedWordBase]
        public static void Base(){ throw new NotSupportedException(); }

        [MethodReservedWordThis]
        public static object This(){ throw new NotSupportedException(); }

        [MethodReservedWordIncrement]
        public static T Increment<T>(T variable){ throw new NotSupportedException(); }

        [Obsolete]
        [MethodReservedWordAddOneDup]
        public static T AddOneDup<T>(T variable){ throw new NotSupportedException(); }

        [MethodReservedWordDecrement]
        public static T Decrement<T>(T variable){ throw new NotSupportedException(); }

        [Obsolete]
        [MethodReservedWordNew]
        public static object New(ConstructorInfo constructor, object parameter){ throw new NotSupportedException(); }

        [Obsolete]
        [MethodReservedWordNew]
        public static T New<T>(ConstructorInfo constructor, params object[] parameters){ throw new NotSupportedException(); }

        [Obsolete]
        [MethodReservedWordInvoke]
        public static object Invoke(MethodInfo method, params object[] parameters){ throw new NotSupportedException(); }

        [Obsolete]
        [MethodReservedWordInvoke]
        public static object Invoke(object variable, object method, params object[] parameters){ throw new NotSupportedException(); }

        [MethodReservedWordLoadPtr]
        public static IntPtr LoadPtr(object variable, IMethodDeclaration methodDecl) { throw new NotSupportedException(); }

        [MethodReservedWordLoadPtr]
        public static IntPtr LoadPtr(IMethodDeclaration methodDecl) { throw new NotSupportedException(); }

        [MethodReservedWordLoadPtr]
        public static IntPtr LoadPtr(MethodInfo methodInfo){ throw new NotSupportedException(); }

        [MethodReservedWordIf]
        public static void If(bool condition){ throw new NotSupportedException(); }

        [MethodReservedWordElseIf]
        public static void ElseIf(bool condition){ throw new NotSupportedException(); }

        [MethodReservedWordElse]
        public static void Else(){ throw new NotSupportedException(); }

        [MethodReservedWordEndIf]
        public static void EndIf(){ throw new NotSupportedException(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodReservedWordEnd]
        public static void End(){ throw new NotSupportedException(); }

        [MethodReservedWordReturn]
        public static void Return<T>(T variable){ throw new NotSupportedException(); }

        [MethodReservedWordLoad]
        public static T Load<T>(string variableName){ throw new NotSupportedException(); }

        [MethodReservedWordLoad]
        public static object Load(string variableName){ throw new NotSupportedException(); }

        [MethodReservedWordLoad]
        public static object[] Load(string[] variableNames){ throw new NotSupportedException(); }

        [Obsolete]
        [MethodReservedWordLoad]
        public static object[] Load(string[] variableNames, int shift){ throw new NotSupportedException(); }

        [MethodReservedWordLoadArgument]
        public static object LoadArgument(int variableIndex){ throw new NotSupportedException(); }

        [MethodReservedWordLoadArgument]
        public static object[] LoadArguments(int[] variableIndexes){ throw new NotSupportedException(); }

        [MethodReservedWordLoadArgument]
        public static T LoadArgument<T>(int variableIndex){ throw new NotSupportedException(); }

        [MethodReservedWordStore]
        public static IMethodAllocReservedWords<T> Store<T>(string variableName){ throw new NotSupportedException(); }

        [MethodReservedWordStore]
        public static IMethodAllocReservedWords Store(string variableName){ throw new NotSupportedException(); }

        [MethodReservedWordAllocate]
        public static IMethodAllocReservedWords<T> Allocate<T>(T variable){ throw new NotSupportedException(); }

        [MethodReservedWordAllocate]
        public static IMethodAllocReservedWords Allocate(object variable){ throw new NotSupportedException(); }

        [MethodReservedWordExtract]
        public static T Extract<T>(T constant){ throw new NotSupportedException(); }

        [MethodReservedWordExtract]
        public static T Extract<T>(object constant){ throw new NotSupportedException(); }

        [MethodReservedWordConstMember]
        public static TValue ConstMember<TValue>(TValue constMember, Type declaringType){ throw new NotSupportedException(); }

        [MethodReservedWordAreEqual]
        public static bool AreEqual(object left, object right){ throw new NotSupportedException(); }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class MethodReservedWordsAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordBaseAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordThisAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordIncrementAttribute : Attribute { }

    [Obsolete]
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordAddOneDupAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordDecrementAttribute : Attribute { }

    [Obsolete]
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordNewAttribute : Attribute { }

    [Obsolete]
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordInvokeAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordLoadPtrAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordIfAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordElseIfAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordElseAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordEndIfAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordEndAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordReturnAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordLoadAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordStoreAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordAllocateAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordExtractAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordConstMemberAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class MethodReservedWordLoadArgumentAttribute : Attribute { }

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
