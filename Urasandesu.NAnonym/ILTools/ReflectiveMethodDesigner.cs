/* 
 * File: ReflectiveMethodDesigner.cs
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Urasandesu.NAnonym.Linq;
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{
    public class ReflectiveMethodDesigner : IMethodBodyGenerator
    {
        readonly IMethodReservedWords reservedWords = new MethodReservedWords();
        readonly IMethodAllocReservedWords allocReservedWords = new MethodAllocReservedWords();

        public static readonly MethodInfo MethodInfoInvoke_object_objects = TypeSavable.GetInstanceMethod<MethodInfo, object, object[], object>(_ => _.Invoke);
        public static readonly MethodInfo ConstructorInfoInvoke_objects = TypeSavable.GetInstanceMethod<ConstructorInfo, object[], object>(_ => _.Invoke);
        public static readonly MethodInfo PropertyInfoSetValue_object_object_objects = TypeSavable.GetInstanceMethod<PropertyInfo, object, object, object[]>(_ => _.SetValue);
        public static readonly MethodInfo PropertyInfoGetValue_object_objects = TypeSavable.GetInstanceMethod<PropertyInfo, object, object[], object>(_ => _.GetValue);

        public static readonly MethodInfo ReservedWordXInfo_T = TypeSavable.GetInstanceMethod<IMethodReservedWords, object, object>(_ => _.X).GetGenericMethodDefinition();
        public static readonly MethodInfo ReservedWordXInfo_object = TypeSavable.GetInstanceMethod<IMethodReservedWords, object, object>(_ => _.X<object>).GetGenericMethodDefinition();

        readonly IMethodBaseGenerator method;
        protected readonly EvalState state;

        protected internal ReflectiveMethodDesigner(IMethodBaseGenerator method)
        {
            this.method = method;
            state = new EvalState();
        }

        public void Eval(Expression<Action<IMethodReservedWords>> exp)
        {
            Eval(method, exp.Body, state);
        }

        public void EvalTo(Expression<Action<IMethodReservedWords>> exp, IMethodBaseGenerator method)
        {
            Eval(method, exp.Body, new EvalState());
        }

        protected virtual IMethodReservedWords ReservedWords
        {
            get
            {
                return reservedWords;
            }
        }

        protected virtual IMethodAllocReservedWords AllocReservedWords
        {
            get
            {
                return allocReservedWords;
            }
        }

        protected virtual void Eval(IMethodBaseGenerator method, Expression exp, EvalState state)
        {
            EvalExpression(method, exp, state);
            EvalExit(method, exp, state);
        }

        protected virtual void EvalExit(IMethodBaseGenerator method, Expression exp, EvalState state)
        {
            EvalAdjustState(method, exp, state);

            if (exp.Type != typeof(void) && !state.ProhibitsLastAutoPop)
            {
                // NOTE: void ではないということは評価スタックに情報が残っているということ。
                //       pop するのは、基本的に 1 回の Emit(Expression<Action<ExpressiveILProcessor>>) で完結するようにしたいため。
                method.Body.ILOperator.Emit(OpCodes.Pop);
            }
        }

        protected virtual void EvalArguments(IMethodBaseGenerator method, ReadOnlyCollection<Expression> exps, EvalState state)
        {
            foreach (var exp in exps)
            {
                state.CandidateDesigningExpressionStack.Push(exp);
                EvalExpression(method, exp, state);
                EvalAdjustState(method, exp, state);
            }
        }

        protected virtual void EvalExpression(IMethodBaseGenerator method, Expression exp, EvalState state)
        {
            state.ProhibitsLastAutoPop = false;
            if (exp == null) return;

            switch (exp.NodeType)
            {
                case ExpressionType.Add:
                case ExpressionType.ArrayIndex:
                case ExpressionType.Multiply:
                case ExpressionType.Equal:
                    EvalBinary(method, (BinaryExpression)exp, state);
                    return;
                case ExpressionType.AddChecked:
                    throw new NotImplementedException();
                case ExpressionType.And:
                    throw new NotImplementedException();
                case ExpressionType.AndAlso:
                    throw new NotImplementedException();
                case ExpressionType.Call:
                    EvalMethodCall(method, (MethodCallExpression)exp, state);
                    return;
                case ExpressionType.Coalesce:
                    throw new NotImplementedException();
                case ExpressionType.Conditional:
                    throw new NotImplementedException();
                case ExpressionType.Constant:
                    EvalConstant(method, (ConstantExpression)exp, state);
                    return;
                case ExpressionType.ArrayLength:
                case ExpressionType.Convert:
                case ExpressionType.TypeAs:
                    EvalUnary(method, (UnaryExpression)exp, state);
                    return;
                case ExpressionType.ConvertChecked:
                    throw new NotImplementedException();
                case ExpressionType.Divide:
                    throw new NotImplementedException();
                case ExpressionType.ExclusiveOr:
                    throw new NotImplementedException();
                case ExpressionType.GreaterThan:
                    throw new NotImplementedException();
                case ExpressionType.GreaterThanOrEqual:
                    throw new NotImplementedException();
                case ExpressionType.Invoke:
                    EvalInvoke(method, (InvocationExpression)exp, state);
                    return;
                case ExpressionType.Lambda:
                    EvalLambda(method, (LambdaExpression)exp, state);
                    return;
                case ExpressionType.LeftShift:
                    throw new NotImplementedException();
                case ExpressionType.LessThan:
                    throw new NotImplementedException();
                case ExpressionType.LessThanOrEqual:
                    throw new NotImplementedException();
                case ExpressionType.ListInit:
                    throw new NotImplementedException();
                case ExpressionType.MemberAccess:
                    EvalMember(method, (MemberExpression)exp, state);
                    return;
                case ExpressionType.MemberInit:
                    throw new NotImplementedException();
                case ExpressionType.Modulo:
                    throw new NotImplementedException();
                case ExpressionType.MultiplyChecked:
                    throw new NotImplementedException();
                case ExpressionType.Negate:
                    throw new NotImplementedException();
                case ExpressionType.NegateChecked:
                    throw new NotImplementedException();
                case ExpressionType.New:
                    EvalNew(method, (NewExpression)exp, state);
                    return;
                case ExpressionType.NewArrayBounds:
                    throw new NotImplementedException();
                case ExpressionType.NewArrayInit:
                    EvalNewArray(method, (NewArrayExpression)exp, state);
                    return;
                case ExpressionType.Not:
                    throw new NotImplementedException();
                case ExpressionType.NotEqual:
                    throw new NotImplementedException();
                case ExpressionType.Or:
                    throw new NotImplementedException();
                case ExpressionType.OrElse:
                    throw new NotImplementedException();
                case ExpressionType.Parameter:
                    EvalParameter(method, (ParameterExpression)exp, state);
                    return;
                case ExpressionType.Power:
                    throw new NotImplementedException();
                case ExpressionType.Quote:
                    throw new NotImplementedException();
                case ExpressionType.RightShift:
                    throw new NotImplementedException();
                case ExpressionType.Subtract:
                    throw new NotImplementedException();
                case ExpressionType.SubtractChecked:
                    throw new NotImplementedException();
                case ExpressionType.TypeIs:
                    throw new NotImplementedException();
                case ExpressionType.UnaryPlus:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        protected virtual void EvalBinary(IMethodBaseGenerator method, BinaryExpression exp, EvalState state)
        {
            EvalExpression(method, exp.Left, state);
            EvalExpression(method, exp.Right, state);

            // TODO: ?? 演算子とか演算子のオーバーロードとか。
            if (exp.Conversion != null) throw new NotImplementedException();
            if (exp.Method != null) throw new NotImplementedException();

            if (exp.NodeType == ExpressionType.Coalesce)
            {
                throw new NotImplementedException();
            }
            else if (exp.NodeType == ExpressionType.Add)
            {
                if (exp.Left.Type == typeof(int) && exp.Left.Type == typeof(int))
                {
                    method.Body.ILOperator.Emit(OpCodes.Add);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else if (exp.NodeType == ExpressionType.Multiply)
            {
                if (exp.Left.Type == typeof(int) && exp.Left.Type == typeof(int))
                {
                    method.Body.ILOperator.Emit(OpCodes.Mul);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else if (exp.NodeType == ExpressionType.ArrayIndex)
            {
                if (typeof(double).IsAssignableFrom(exp.Left.Type.GetElementType()))
                {
                    throw new NotImplementedException();
                }
                else
                {
                    method.Body.ILOperator.Emit(OpCodes.Ldelem_Ref);
                }
            }
            else if (exp.NodeType == ExpressionType.Equal)
            {
                method.Body.ILOperator.Emit(OpCodes.Ceq);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected virtual void EvalMethodCall(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            // 評価の順番は、Object -> Arguments -> Method。
            if (exp.Object == null)
            {
                // static method call
                EvalArguments(method, exp.Arguments, state);
                method.Body.ILOperator.Emit(OpCodes.Call, exp.Method);
            }
            else
            {
                if (exp.Object.Type.IsDefined(typeof(MethodReservedWordsAttribute), false))
                {
                    if (exp.Method.IsDefined(typeof(MethodReservedWordBaseAttribute), false)) EvalBase(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordThisAttribute), false)) EvalThis(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordDupAddOneAttribute), false)) EvalDupAddOne(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordAddOneDupAttribute), false)) EvalAddOneDup(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordSubOneDupAttribute), false)) EvalSubOneDup(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordNewAttribute), false)) EvalNew(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordInvokeAttribute), false)) EvalInvoke(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordFtnAttribute), false)) EvalFtn(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordIfAttribute), false)) EvalIf(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordElseIfAttribute), false)) EvalElseIf(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordElseAttribute), false)) EvalElse(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordEndIfAttribute), false)) EvalEndIf(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordReturnAttribute), false)) EvalReturn(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordEndAttribute), false)) EvalEnd(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordLdAttribute), false)) EvalLd(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordAllocAttribute), false)) EvalAlloc(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordStAttribute), false)) EvalSt(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordXAttribute), false)) EvalExtract(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordCmAttribute), false)) EvalCm(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordLdArgAttribute), false)) EvalLdArg(method, exp, state);
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else if (exp.Object.Type.IsDefined(typeof(MethodAllocReservedWordsAttribute), false))
                {
                    if (exp.Method.IsDefined(typeof(MethodAllocReservedWordAsAttribute), false)) EvalAllocAs(method, exp, state);
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    if (exp.Method == MethodInfoInvoke_object_objects) EvalMethodInfoInvoke_object_objects(method, exp, state);
                    else if (exp.Method == ConstructorInfoInvoke_objects) EvalConstructorInfoInvoke_objects(method, exp, state);
                    else if (exp.Method == PropertyInfoSetValue_object_object_objects) EvalPropertyInfoSetValue_object_object_objects(method, exp, state);
                    else if (exp.Method == PropertyInfoGetValue_object_objects) EvalPropertyInfoGetValue_object_objects(method, exp, state);
                    else
                    {
                        // instance method call
                        state.CandidateDesigningExpressionStack.Push(exp.Object);
                        EvalExpression(method, exp.Object, state);
                        EvalAdjustState(method, exp.Object, state);
                        if (exp.Object.Type.IsValueType)
                        {
                            // NOTE: 値型のメソッドを呼び出すには、アドレスへの変換が必要。
                            var local = method.Body.ILOperator.AddLocal(exp.Object.Type);
                            method.Body.ILOperator.Emit(OpCodes.Stloc, local);
                            method.Body.ILOperator.Emit(OpCodes.Ldloca, local);
                        }

                        EvalArguments(method, exp.Arguments, state);

                        method.Body.ILOperator.Emit(OpCodes.Callvirt, exp.Method);
                    }
                }
            }
        }

        protected virtual void EvalBase(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            method.Body.ILOperator.Emit(OpCodes.Ldarg_0);
            var constructorDecl = default(IConstructorDeclaration);
            if (method.DeclaringType.BaseType != null)
            {
                constructorDecl = method.DeclaringType.BaseType.GetConstructor(new Type[] { });
            }
            else
            {
                throw new NotImplementedException();
            }
            method.Body.ILOperator.Emit(OpCodes.Call, constructorDecl);
        }

        protected virtual void EvalThis(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            method.Body.ILOperator.Emit(OpCodes.Ldarg_0);
        }

        protected virtual void EvalDupAddOne(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
            var localDecl = method.Body.Locals.First(_localDecl => _localDecl.Name == fieldInfo.Name);
            method.Body.ILOperator.Emit(OpCodes.Ldloc, localDecl);
            method.Body.ILOperator.Emit(OpCodes.Dup);
            method.Body.ILOperator.Emit(OpCodes.Ldc_I4_1);
            method.Body.ILOperator.Emit(OpCodes.Add);
            method.Body.ILOperator.Emit(OpCodes.Stloc, localDecl);
        }

        protected virtual void EvalAddOneDup(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
            var localDecl = method.Body.Locals.First(_localDecl => _localDecl.Name == fieldInfo.Name);
            method.Body.ILOperator.Emit(OpCodes.Ldloc, localDecl);
            method.Body.ILOperator.Emit(OpCodes.Ldc_I4_1);
            method.Body.ILOperator.Emit(OpCodes.Add);
            method.Body.ILOperator.Emit(OpCodes.Dup);
            method.Body.ILOperator.Emit(OpCodes.Stloc, localDecl);
        }

        protected virtual void EvalSubOneDup(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
            var localDecl = method.Body.Locals.First(_localDecl => _localDecl.Name == fieldInfo.Name);
            method.Body.ILOperator.Emit(OpCodes.Ldloc, localDecl);
            method.Body.ILOperator.Emit(OpCodes.Ldc_I4_1);
            method.Body.ILOperator.Emit(OpCodes.Sub);
            method.Body.ILOperator.Emit(OpCodes.Dup);
            method.Body.ILOperator.Emit(OpCodes.Stloc, localDecl);
        }

        protected virtual void EvalNew(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            if (exp.Arguments[1].NodeType == ExpressionType.NewArrayInit)
            {
                ((NewArrayExpression)exp.Arguments[1]).Expressions.ForEach(_exp => EvalExpression(method, _exp, state));
            }
            else
            {
                EvalExpression(method, exp.Arguments[1], state);
            }

            EvalExpression(method, exp.Arguments[0], state);
            if (0 < state.ExtractInfoStack.Count)
            {
                var extractInfo = state.ExtractInfoStack.Pop();
                var constructor = (ConstructorInfo)extractInfo.Value;
                method.Body.ILOperator.Emit(OpCodes.Newobj, constructor);
            }
        }

        protected virtual void EvalInvoke(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            EvalExpression(method, exp.Arguments[0], state);
            if (0 < state.ExtractInfoStack.Count)
            {
                throw new NotImplementedException();
            }
            EvalExpression(method, exp.Arguments[2], state);
            if (0 < state.ExtractInfoStack.Count)
            {
                throw new NotImplementedException();
            }
            EvalExpression(method, exp.Arguments[1], state);
            if (0 < state.ExtractInfoStack.Count)
            {
                var extractInfo = state.ExtractInfoStack.Pop();
                var methodInfo = (MethodInfo)extractInfo.Value;
                method.Body.ILOperator.Emit(OpCodes.Callvirt, methodInfo);
            }
        }

        protected virtual void EvalMethodInfoInvoke_object_objects(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var methodInfo = default(MethodInfo);
            {
                var constantExp = Expression.Constant(ReservedWords);
                var extractMethod = ReservedWordXInfo_T.MakeGenericMethod(typeof(MethodInfo));
                var parameterExps = new Expression[] { exp.Object };
                var extractExp = Expression.Call(constantExp, extractMethod, parameterExps);
                EvalExtract(method, extractExp, state);
                if (state.ExtractInfoStack.Count == 0)
                {
                    throw new NotSupportedException();
                }
                var extractInfo = state.ExtractInfoStack.Pop();
                methodInfo = (MethodInfo)extractInfo.Value;
            }

            if (!methodInfo.IsStatic)
            {
                EvalExpression(method, exp.Arguments[0], state);
            }

            var arguments = new List<Expression>();
            if (exp.Arguments[1].NodeType == ExpressionType.NewArrayInit)
            {
                EvalArguments(method, ((NewArrayExpression)exp.Arguments[1]).Expressions, state);
            }
            else
            {
                throw new NotImplementedException();
            }

            if (methodInfo.IsStatic)
            {
                method.Body.ILOperator.Emit(OpCodes.Call, methodInfo);
            }
            else
            {
                method.Body.ILOperator.Emit(OpCodes.Callvirt, methodInfo);
            }

            state.ProhibitsLastAutoPop = methodInfo.ReturnType == typeof(void);
            state.MethodDesignedInfoStack.Push(methodInfo);
        }

        protected virtual void EvalConstructorInfoInvoke_objects(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var constructorInfo = default(ConstructorInfo);
            {
                var constantExp = Expression.Constant(ReservedWords);
                var extractMethod = ReservedWordXInfo_T.MakeGenericMethod(typeof(ConstructorInfo));
                var parameterExps = new Expression[] { exp.Object };
                var extractExp = Expression.Call(constantExp, extractMethod, parameterExps);
                EvalExtract(method, extractExp, state);
                if (state.ExtractInfoStack.Count == 0)
                {
                    throw new NotSupportedException();
                }
                var extractInfo = state.ExtractInfoStack.Pop();
                constructorInfo = (ConstructorInfo)extractInfo.Value;
            }

            var arguments = new List<Expression>();
            if (exp.Arguments[0].NodeType == ExpressionType.NewArrayInit)
            {
                EvalArguments(method, ((NewArrayExpression)exp.Arguments[0]).Expressions, state);
            }
            else if (exp.Arguments[0].NodeType == ExpressionType.Constant && ((ConstantExpression)exp.Arguments[0]).Value == null)
            {
                // discard...
            }
            else
            {
                throw new NotImplementedException();
            }

            method.Body.ILOperator.Emit(OpCodes.Newobj, constructorInfo);
            state.ConstructorDesignedInfoStack.Push(constructorInfo);
        }

        protected virtual void EvalPropertyInfoSetValue_object_object_objects(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var propertyInfo = default(PropertyInfo);
            {
                var constantExp = Expression.Constant(ReservedWords);
                var extractMethod = ReservedWordXInfo_T.MakeGenericMethod(typeof(PropertyInfo));
                var parameterExps = new Expression[] { exp.Object };
                var extractExp = Expression.Call(constantExp, extractMethod, parameterExps);
                EvalExtract(method, extractExp, state);
                if (state.ExtractInfoStack.Count == 0)
                {
                    throw new NotSupportedException();
                }
                var extractInfo = state.ExtractInfoStack.Pop();
                propertyInfo = (PropertyInfo)extractInfo.Value;
            }

            var setter = propertyInfo.GetSetMethod(true);
            if (!setter.IsStatic)
            {
                EvalExpression(method, exp.Arguments[0], state);
            }

            if (propertyInfo.PropertyType.IsValueType && 
                exp.Arguments[1].NodeType == ExpressionType.Convert && 
                ((UnaryExpression)exp.Arguments[1]).Operand.Type.IsSubclassOf(typeof(ValueType)))
            {
                EvalExpression(method, ((UnaryExpression)exp.Arguments[1]).Operand, state);
            }
            else
            {
                EvalExpression(method, exp.Arguments[1], state);
            }

            if (exp.Arguments[2].NodeType == ExpressionType.NewArrayInit)
            {
                throw new NotImplementedException();
            }
            else if (exp.Arguments[2].NodeType == ExpressionType.Constant && ((ConstantExpression)exp.Arguments[2]).Value == null)
            {
                // discard...
            }
            else
            {
                throw new NotImplementedException();
            }

            if (setter.IsStatic)
            {
                method.Body.ILOperator.Emit(OpCodes.Call, setter);
            }
            else
            {
                method.Body.ILOperator.Emit(OpCodes.Callvirt, setter);
            }
        }

        protected virtual void EvalPropertyInfoGetValue_object_objects(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var propertyInfo = default(PropertyInfo);
            {
                var constantExp = Expression.Constant(ReservedWords);
                var extractMethod = ReservedWordXInfo_T.MakeGenericMethod(typeof(PropertyInfo));
                var parameterExps = new Expression[] { exp.Object };
                var extractExp = Expression.Call(constantExp, extractMethod, parameterExps);
                EvalExtract(method, extractExp, state);
                if (state.ExtractInfoStack.Count == 0)
                {
                    throw new NotSupportedException();
                }
                var extractInfo = state.ExtractInfoStack.Pop();
                propertyInfo = (PropertyInfo)extractInfo.Value;
            }

            var getter = propertyInfo.GetGetMethod(true);
            if (!getter.IsStatic)
            {
                EvalExpression(method, exp.Arguments[0], state);
            }

            if (exp.Arguments[1].NodeType == ExpressionType.NewArrayInit)
            {
                throw new NotImplementedException();
            }
            else if (exp.Arguments[1].NodeType == ExpressionType.Constant && ((ConstantExpression)exp.Arguments[1]).Value == null)
            {
                // discard...
            }
            else
            {
                throw new NotImplementedException();
            }

            if (getter.IsStatic)
            {
                method.Body.ILOperator.Emit(OpCodes.Call, getter);
            }
            else
            {
                method.Body.ILOperator.Emit(OpCodes.Callvirt, getter);
            }

            state.PropertyDesignedInfoStack.Push(propertyInfo);
        }

        protected virtual void EvalFtn(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            if (1 < exp.Arguments.Count)
            {
                EvalExpression(method, exp.Arguments[0], state);
                if (0 < state.ExtractInfoStack.Count)
                {
                    throw new NotImplementedException();
                }
            }

            if (exp.Arguments[exp.Arguments.Count - 1].Type == typeof(IMethodDeclaration))
            {
                EvalExpression(method, exp.Arguments[exp.Arguments.Count - 1], state);
                if (0 < state.ExtractInfoStack.Count)
                {
                    var extractInfo = state.ExtractInfoStack.Pop();
                    var methodDecl = (IMethodDeclaration)extractInfo.Value;
                    method.Body.ILOperator.Emit(OpCodes.Ldftn, methodDecl);
                }
            }
            else if (exp.Arguments[exp.Arguments.Count - 1].Type == typeof(MethodInfo))
            {
                EvalExpression(method, exp.Arguments[exp.Arguments.Count - 1], state);
                if (0 < state.ExtractInfoStack.Count)
                {
                    var extractInfo = state.ExtractInfoStack.Pop();
                    var methodInfo = (MethodInfo)extractInfo.Value;
                    method.Body.ILOperator.Emit(OpCodes.Ldftn, methodInfo);
                }
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        protected virtual void EvalIf(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            EvalExpression(method, exp.Arguments[0], state);
            method.Body.ILOperator.Emit(OpCodes.Ldc_I4_0);
            method.Body.ILOperator.Emit(OpCodes.Ceq);
            var localDecl = method.Body.ILOperator.AddLocal(typeof(bool));
            method.Body.ILOperator.Emit(OpCodes.Stloc, localDecl);
            method.Body.ILOperator.Emit(OpCodes.Ldloc, localDecl);
            var labelDecl = method.Body.ILOperator.AddLabel();
            state.IfInfoStack.Push(new IfInfo(labelDecl));
            method.Body.ILOperator.Emit(OpCodes.Brtrue, labelDecl);
        }

        protected virtual void EvalElseIf(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var ifInfo = state.IfInfoStack.Pop();
            method.Body.ILOperator.SetLabel(ifInfo.Label);
            EvalExpression(method, exp.Arguments[0], state);
            method.Body.ILOperator.Emit(OpCodes.Ldc_I4_0);
            method.Body.ILOperator.Emit(OpCodes.Ceq);
            var localDecl = method.Body.ILOperator.AddLocal(typeof(bool));
            method.Body.ILOperator.Emit(OpCodes.Stloc, localDecl);
            method.Body.ILOperator.Emit(OpCodes.Ldloc, localDecl);
            var labelDecl = method.Body.ILOperator.AddLabel();
            state.IfInfoStack.Push(new IfInfo(labelDecl));
            method.Body.ILOperator.Emit(OpCodes.Brtrue, labelDecl);
        }

        protected virtual void EvalElse(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var ifInfo = state.IfInfoStack.Pop();
            method.Body.ILOperator.SetLabel(ifInfo.Label);
            var labelDecl = method.Body.ILOperator.AddLabel();
            state.IfInfoStack.Push(new IfInfo(labelDecl));
        }

        protected virtual void EvalEndIf(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var ifInfo = state.IfInfoStack.Pop();
            method.Body.ILOperator.SetLabel(ifInfo.Label);
        }

        protected virtual void EvalReturn(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            EvalExpression(method, exp.Arguments[0], state);
            var labelDecl = default(ILabelDeclaration);
            if (0 < state.ReturnInfoStack.Count)
            {
                var returnInfo = state.ReturnInfoStack.Pop();
                labelDecl = returnInfo.Label;
                state.ReturnInfoStack.Push(returnInfo);
            }
            else
            {
                labelDecl = method.Body.ILOperator.AddLabel();
                state.ReturnInfoStack.Push(new ReturnInfo(labelDecl));
            }
            method.Body.ILOperator.Emit(OpCodes.Br, labelDecl);
        }

        protected virtual void EvalEnd(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            if (1 < state.ReturnInfoStack.Count)
            {
                throw new NotSupportedException();
            }

            if (0 < state.ReturnInfoStack.Count)
            {
                var returnInfo = state.ReturnInfoStack.Pop();
                method.Body.ILOperator.SetLabel(returnInfo.Label);
            }
            method.Body.ILOperator.Emit(OpCodes.Ret);
        }

        protected virtual void EvalLd(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var extractExp = default(MethodCallExpression);

            if (exp.Arguments.Count == 2)
            {
                extractExp = CreateExtractExp1(exp.Arguments[1], typeof(int));
                EvalExtract(method, extractExp, state);
            }

            extractExp = CreateExtractExp1(exp.Arguments[0], exp.Arguments[0].Type);
            EvalExtract(method, extractExp, state);

            if (0 < state.ExtractInfoStack.Count)
            {
                var extractInfo = state.ExtractInfoStack.Pop();
                var shiftCount = -1;
                if (0 < state.ExtractInfoStack.Count)
                {
                    var _extractInfo = state.ExtractInfoStack.Pop();
                    shiftCount = (int)_extractInfo.Value;
                }

                if (extractInfo.Type == typeof(string[]))
                {
                    ((string[])extractInfo.Value).
                    ForEach(name =>
                    {
                        var fieldInfo = new NameResolvableInfo(name, typeof(object));
                        if (-1 < shiftCount) state.ShiftInfoStack.Push(new ShiftInfo(shiftCount));
                        EvalMember(method, Expression.Field(null, fieldInfo), state);
                    });
                }
                else
                {
                    var fieldInfo = new NameResolvableInfo((string)extractInfo.Value, extractInfo.Type);
                    if (-1 < shiftCount) state.ShiftInfoStack.Push(new ShiftInfo(shiftCount));
                    EvalMember(method, Expression.Field(null, fieldInfo), state);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected virtual void EvalLdArg(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var extractExp = default(MethodCallExpression);

            if (exp.Arguments.Count == 2)
            {
                extractExp = CreateExtractExp1(exp.Arguments[1], typeof(int));
                EvalExtract(method, extractExp, state);
            }

            extractExp = CreateExtractExp1(exp.Arguments[0], exp.Arguments[0].Type);
            EvalExtract(method, extractExp, state);

            if (0 < state.ExtractInfoStack.Count)
            {
                var extractInfo = state.ExtractInfoStack.Pop();
                var shiftCount = -1;
                if (0 < state.ExtractInfoStack.Count)
                {
                    var _extractInfo = state.ExtractInfoStack.Pop();
                    shiftCount = (int)_extractInfo.Value;
                }

                if (extractInfo.Type == typeof(int[]))
                {
                    ((int[])extractInfo.Value).
                    ForEach(index =>
                    {
                        var fieldInfo = new ParameterIndexResolvableInfo(index, typeof(object));
                        if (-1 < shiftCount) state.ShiftInfoStack.Push(new ShiftInfo(shiftCount));
                        EvalMember(method, Expression.Field(null, fieldInfo), state);
                    });
                }
                else
                {
                    var fieldInfo = new ParameterIndexResolvableInfo((int)extractInfo.Value, extractInfo.Type);
                    if (-1 < shiftCount) state.ShiftInfoStack.Push(new ShiftInfo(shiftCount));
                    EvalMember(method, Expression.Field(null, fieldInfo), state);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected virtual void EvalAlloc(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            if (exp.Arguments[0].NodeType == ExpressionType.MemberAccess)
            {
                var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
                state.AllocInfoStack.Push(new AllocInfo(fieldInfo.Name, fieldInfo.FieldType));
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        protected virtual void EvalAllocAs(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            EvalExpression(method, exp.Object, state);
            if (0 < state.AllocInfoStack.Count)
            {
                var allocInfo = state.AllocInfoStack.Pop();

                var stExp = exp.Arguments[0];
                var localGen = default(ILocalGenerator);
                var parameterGen = default(IParameterGenerator);
                var fieldGen = default(IFieldGenerator);
                if ((localGen = method.Body.Locals.FirstOrDefault(_localGen => _localGen.Name == allocInfo.Name)) != null)
                {
                    state.CandidateDesigningExpressionStack.Push(stExp);
                    EvalExpression(method, stExp, state);
                    EvalAdjustState(method, stExp, state);
                    method.Body.ILOperator.Emit(OpCodes.Stloc, localGen);
                }
                else if ((parameterGen = method.Parameters.FirstOrDefault(_parameterGen => _parameterGen.Name == allocInfo.Name)) != null)
                {
                    state.CandidateDesigningExpressionStack.Push(stExp);
                    EvalExpression(method, stExp, state);
                    EvalAdjustState(method, stExp, state);
                    method.Body.ILOperator.Emit(OpCodes.Starg, parameterGen);
                }
                else if ((fieldGen = method.DeclaringType.Fields.FirstOrDefault(_fieldGen => _fieldGen.Name == allocInfo.Name)) != null)
                {
                    method.Body.ILOperator.Emit(OpCodes.Ldarg_0);
                    state.CandidateDesigningExpressionStack.Push(stExp);
                    EvalExpression(method, stExp, state);
                    EvalAdjustState(method, stExp, state);
                    method.Body.ILOperator.Emit(OpCodes.Stfld, fieldGen);
                }
                else
                {
                    state.CandidateDesigningExpressionStack.Push(stExp);
                    EvalExpression(method, stExp, state);
                    EvalAdjustState(method, stExp, state);
                    var local = method.Body.ILOperator.AddLocal(allocInfo.Name, allocInfo.Type);
                    method.Body.ILOperator.Emit(OpCodes.Stloc, local);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            state.ProhibitsLastAutoPop = true;
        }

        protected virtual void EvalSt(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var xInfoType = default(Type);
            if (typeof(IMethodAllocReservedWords).IsAssignableFrom(exp.Type))
            {
                xInfoType = typeof(object);
            }
            else if (typeof(IMethodAllocReservedWords<>).IsAssignableWithoutGenericArgumentsFrom(exp.Type))
            {
                xInfoType = exp.Type.GetGenericArguments()[0];
            }
            else
            {
                throw new NotSupportedException();
            }

            var constantExp = Expression.Constant(ReservedWords);
            var extractMethod = ReservedWordXInfo_object.MakeGenericMethod(xInfoType);
            var parameterExps = new Expression[] { exp.Arguments[0] };
            var extractExp = Expression.Call(constantExp, extractMethod, parameterExps);
            EvalExtract(method, extractExp, state);
            if (0 < state.ExtractInfoStack.Count)
            {
                var extractInfo = state.ExtractInfoStack.Pop();
                var name = (string)extractInfo.Value;
                state.AllocInfoStack.Push(new AllocInfo(name, extractInfo.Type));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected virtual void EvalExtract(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var extracted = Expression.Lambda(exp.Arguments[0]).Compile();
            object o = extracted.DynamicInvoke();
            if (exp.Arguments.Count == 1)
            {
                state.ExtractInfoStack.Push(new ExtractInfo(exp.Type, o));
            }
            else
            {
                var extracted1 = Expression.Lambda(exp.Arguments[1]).Compile();
                Type type = (Type)extracted1.DynamicInvoke();
                state.ExtractInfoStack.Push(new ExtractInfo(type, o));
            }
        }

        protected virtual void EvalAdjustState(IMethodBaseGenerator method, Expression exp, EvalState state)
        {
            // Adjust ExtractInfoStack. 
            if (0 < state.ExtractInfoStack.Count)
            {
                var extractInfo = state.ExtractInfoStack.Pop();
                if (extractInfo.Type.IsArray)
                {
                    EvalNewArray(method, Expression.NewArrayInit(extractInfo.Type.GetElementType(),
                        ((Array)extractInfo.Value).Cast<object>().Select(value => (Expression)Expression.Constant(value))), state);
                }
                else if (typeof(LambdaExpression).IsAssignableFrom(extractInfo.Type))
                {
                    var lambdaExp = (LambdaExpression)extractInfo.Value;
                    state.CandidateDesigningExpressionStack.Push(lambdaExp.Body);
                    EvalExpression(method, lambdaExp.Body, state);
                    EvalAdjustState(method, lambdaExp.Body, state);
                    state.ProhibitsLastAutoPop = true;
                }
                else
                {
                    EvalConstant(method, Expression.Constant(extractInfo.Value), state);
                }
            }

            // Adjust the return type.
            var designingExp = default(Expression);
            if (0 < state.CandidateDesigningExpressionStack.Count)
            {
                designingExp = state.CandidateDesigningExpressionStack.Pop();
            }

            var convertExp = default(UnaryExpression);
            if (0 < state.MethodDesignedInfoStack.Count)
            {
                var methodInfo = state.MethodDesignedInfoStack.Pop();
                if (designingExp != null && designingExp.Type != methodInfo.ReturnType)
                {
                    var actualParameters = methodInfo.GetParameters();
                    var methodCallExp = (MethodCallExpression)exp;
                    var objectExp = methodCallExp.Arguments[0];
                    var parameterExps = default(ReadOnlyCollection<Expression>);
                    if (methodCallExp.Arguments[1].NodeType == ExpressionType.NewArrayInit)
                    {
                        var _parameterExps = new List<Expression>();
                        ((NewArrayExpression)methodCallExp.Arguments[1]).Expressions.
                        ForEach((_exp, parameterIndex) =>
                        {
                            if (_exp.NodeType == ExpressionType.Convert && 
                                ((UnaryExpression)_exp).Operand.Type == actualParameters[parameterIndex].ParameterType)
                            {
                                _parameterExps.Add(((UnaryExpression)_exp).Operand);
                            }
                            else
                            {
                                _parameterExps.Add(_exp);
                            }
                        });
                        parameterExps = new ReadOnlyCollection<Expression>(_parameterExps);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                    var actualMethodCallExp = Expression.Call(objectExp, methodInfo, parameterExps);
                    convertExp = Expression.Convert(actualMethodCallExp, designingExp.Type);
                }
            }
            else if (0 < state.ConstructorDesignedInfoStack.Count)
            {
                var constructorInfo = state.ConstructorDesignedInfoStack.Pop();
                if (designingExp != null && designingExp.Type != constructorInfo.DeclaringType)
                {
                    var methodCallExp = (MethodCallExpression)exp;
                    var parameterExps = methodCallExp.Arguments;
                    var actualNewExp = Expression.New(constructorInfo, parameterExps);
                    convertExp = Expression.Convert(actualNewExp, designingExp.Type);
                }
            }
            else if (0 < state.PropertyDesignedInfoStack.Count)
            {
                var propertyInfo = state.PropertyDesignedInfoStack.Pop();
                if (designingExp != null && designingExp.Type != propertyInfo.PropertyType)
                {
                    var methodCallExp = (MethodCallExpression)exp;
                    var objectExp = methodCallExp.Arguments[0];
                    var actualPropertyExp = Expression.Property(objectExp, propertyInfo);
                    convertExp = Expression.Convert(actualPropertyExp, designingExp.Type);
                }
            }

            if (convertExp != null)
            {
                EvalUnaryWithoutOperandEval(method, convertExp, state);
            }
        }

        protected virtual void EvalCm(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var expanded0 = Expression.Lambda(exp.Arguments[0]).Compile();
            object staticMember = expanded0.DynamicInvoke();
            var expanded1 = Expression.Lambda(exp.Arguments[1]).Compile();
            Type declaringType = (Type)expanded1.DynamicInvoke();

            var targetFieldInfo = default(FieldInfo);
            foreach (var member in declaringType.GetMembers(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var fieldInfo = default(FieldInfo);
                if ((fieldInfo = member as FieldInfo) != null && staticMember.Equals(fieldInfo.GetValue(null)))
                {
                    targetFieldInfo = fieldInfo;
                    break;
                }
                else
                {
                    continue;
                }
            }

            EvalMember(method, Expression.Field(null, targetFieldInfo), state);
        }

        protected virtual void EvalConstant(IMethodBaseGenerator method, ConstantExpression exp, EvalState state)
        {
            string s = default(string);
            short? ns = default(short?);
            int? ni = default(int?);
            double? nd = default(double?);
            char? nc = default(char?);
            sbyte? nsb = default(sbyte?);
            bool? nb = default(bool?);
            var e = default(Enum);
            var t = default(Type);
            var mi = default(MethodInfo);
            var ci = default(ConstructorInfo);
            var fi = default(FieldInfo);
            if (exp.Value == null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldnull);
            }
            else if ((s = exp.Value as string) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldstr, s);
            }
            else if ((ns = exp.Value as short?) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldc_I4, (int)ns);
                method.Body.ILOperator.Emit(OpCodes.Conv_I2);
            }
            else if ((ni = exp.Value as int?) != null)
            {
                // HACK: _S が付く命令は短い形式。-127 ～ 128 以外は最適化が必要。
                method.Body.ILOperator.Emit(OpCodes.Ldc_I4_S, (sbyte)(int)ni);
            }
            else if ((nd = exp.Value as double?) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldc_R8, (double)nd);
            }
            else if ((nc = exp.Value as char?) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldc_I4_S, (sbyte)(char)nc);
            }
            else if ((nsb = exp.Value as sbyte?) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldc_I4_S, (sbyte)nsb);
            }
            else if ((nb = exp.Value as bool?) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldc_I4, (bool)nb ? 1 : 0);
            }
            else if ((e = exp.Value as Enum) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldc_I4, e.GetHashCode());
            }
            else if ((t = exp.Value as Type) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldtoken, t);
                method.Body.ILOperator.Emit(OpCodes.Call, MethodInfoMixin.GetTypeFromHandleInfo);
            }
            else if ((mi = exp.Value as MethodInfo) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldtoken, mi);
                method.Body.ILOperator.Emit(OpCodes.Call, MethodInfoMixin.GetMethodFromHandleInfo);
                method.Body.ILOperator.Emit(OpCodes.Castclass, typeof(MethodInfo));
            }
            else if ((ci = exp.Value as ConstructorInfo) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldtoken, ci);
                method.Body.ILOperator.Emit(OpCodes.Call, MethodInfoMixin.GetMethodFromHandleInfo);
                method.Body.ILOperator.Emit(OpCodes.Castclass, typeof(ConstructorInfo));
            }
            else if ((fi = exp.Value as FieldInfo) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldtoken, fi);
                method.Body.ILOperator.Emit(OpCodes.Call, MethodInfoMixin.GetFieldFromHandleInfo);
            }
            else
            {
                // TODO: exp.Value がオブジェクト型の場合はどうするのだ？
                throw new NotImplementedException();
            }
        }

        protected virtual void EvalUnary(IMethodBaseGenerator method, UnaryExpression exp, EvalState state)
        {
            EvalExpression(method, exp.Operand, state);
            if (0 < state.CandidateDesigningExpressionStack.Count &&
                (0 < state.MethodDesignedInfoStack.Count ||
                 0 < state.ConstructorDesignedInfoStack.Count ||
                 0 < state.PropertyDesignedInfoStack.Count))
            {
                EvalAdjustState(method, exp.Operand, state);
            }
            else
            {
                EvalUnaryWithoutOperandEval(method, exp, state);
            }
        }

        protected virtual void EvalUnaryWithoutOperandEval(IMethodBaseGenerator method, UnaryExpression exp, EvalState state)
        {
            switch (exp.NodeType)
            {
                case ExpressionType.Convert:
                    if (exp.Type.IsSubclassOf(typeof(ValueType)))
                    {
                        if (exp.Operand.Type.IsAssignableWithoutGenericArgumentsFrom(typeof(Nullable<>)))
                        {
                            var nullableValue = exp.Operand.Type.GetProperty("Value");
                            var nullableget_Value = nullableValue.GetGetMethod();
                            var local = method.Body.ILOperator.AddLocal(exp.Operand.Type);
                            method.Body.ILOperator.Emit(OpCodes.Stloc, local);
                            method.Body.ILOperator.Emit(OpCodes.Ldloca, local);
                            method.Body.ILOperator.Emit(OpCodes.Callvirt, nullableget_Value);
                        }
                        else
                        {
                            if (exp.Type == typeof(int))
                            {
                                method.Body.ILOperator.Emit(OpCodes.Conv_I4);
                            }
                            else if (exp.Type == typeof(sbyte))
                            {
                                method.Body.ILOperator.Emit(OpCodes.Conv_I2);
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                    }
                    else if (exp.Operand.Type.IsSubclassOf(typeof(ValueType)) && !exp.Type.IsSubclassOf(typeof(ValueType)))
                    {
                        // TODO: implicit operator や explicit operator の実装。
                        method.Body.ILOperator.Emit(OpCodes.Box, exp.Operand.Type);
                    }
                    else
                    {
                        // TODO: implicit operator や explicit operator の実装。
                        method.Body.ILOperator.Emit(OpCodes.Castclass, exp.Type);
                    }
                    break;
                case ExpressionType.ArrayLength:
                    method.Body.ILOperator.Emit(OpCodes.Ldlen);
                    method.Body.ILOperator.Emit(OpCodes.Conv_I4);
                    break;
                case ExpressionType.TypeAs:
                    method.Body.ILOperator.Emit(OpCodes.Isinst, exp.Type);
                    if (exp.Type.IsValueType)
                    {
                        method.Body.ILOperator.Emit(OpCodes.Unbox_Any, exp.Type);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        protected virtual void EvalInvoke(IMethodBaseGenerator method, InvocationExpression exp, EvalState state)
        {
            EvalExpression(method, exp.Expression, state);
            EvalArguments(method, exp.Arguments, state);
            if (exp.Expression.Type.IsSubclassOf(typeof(Delegate)))
            {
                var invokeMethod = exp.Expression.Type.GetMethod("Invoke", BindingFlags.Public | BindingFlags.Instance);
                method.Body.ILOperator.Emit(OpCodes.Callvirt, invokeMethod);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected virtual void EvalLambda(IMethodBaseGenerator method, LambdaExpression exp, EvalState state)
        {
            // TODO: DynamicMethod 再帰して作ることになる。手間なので後回し。
            EvalExpression(method, exp.Body, state);
        }

        protected virtual void EvalMember(IMethodBaseGenerator method, MemberExpression exp, EvalState state)
        {
            var fieldInfo = default(FieldInfo);
            var propertyInfo = default(PropertyInfo);
            if ((fieldInfo = exp.Member as FieldInfo) != null)
            {
                var localIndex = default(LocalIndexResolvableInfo);
                var parameterIndex = default(ParameterIndexResolvableInfo);
                if ((localIndex = fieldInfo as LocalIndexResolvableInfo) != null)
                {
                    if (0 < state.ShiftInfoStack.Count)
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        method.Body.ILOperator.Emit(OpCodes.Ldloc, (short)localIndex.Index);
                    }
                }
                else if ((parameterIndex = fieldInfo as ParameterIndexResolvableInfo) != null)
                {
                    if (0 < state.ShiftInfoStack.Count)
                    {
                        var shiftInfo = state.ShiftInfoStack.Pop();
                        method.Body.ILOperator.Emit(OpCodes.Ldarg, (short)(parameterIndex.Index + shiftInfo.Count));
                    }
                    else
                    {
                        method.Body.ILOperator.Emit(OpCodes.Ldarg, (short)parameterIndex.Index);
                    }
                }
                else
                {
                    var localGen = default(ILocalGenerator);
                    var parameterGen = default(IParameterGenerator);
                    var fieldGen = default(IFieldGenerator);
                    var portable = default(ConstantExpression);
                    if ((localGen = method.Body.Locals.FirstOrDefault(_localGen => _localGen.Name == fieldInfo.Name)) != null)
                    {
                        if (0 < state.ShiftInfoStack.Count)
                        {
                            throw new NotImplementedException();
                        }
                        else
                        {
                            method.Body.ILOperator.Emit(OpCodes.Ldloc, localGen);
                        }
                    }
                    else if ((parameterGen = method.Parameters.FirstOrDefault(_parameterGen => _parameterGen.Name == fieldInfo.Name)) != null)
                    {
                        if (0 < state.ShiftInfoStack.Count)
                        {
                            var shiftInfo = state.ShiftInfoStack.Pop();
                            method.Body.ILOperator.Emit(OpCodes.Ldarg, (short)(parameterGen.Position + shiftInfo.Count));
                        }
                        else
                        {
                            method.Body.ILOperator.Emit(OpCodes.Ldarg, parameterGen);
                        }
                    }
                    else if ((fieldGen = method.DeclaringType.Fields.FirstOrDefault(_fieldGen => _fieldGen.Name == fieldInfo.Name)) != null)
                    {
                        method.Body.ILOperator.Emit(OpCodes.Ldarg_0);
                        method.Body.ILOperator.Emit(OpCodes.Ldfld, fieldGen);
                    }
                    else if ((portable = exp.Expression as ConstantExpression) != null)
                    {
                        // NOTE: 同じ名前の変数を Addloc されるとやっかい。擬似的にローカル変数としても定義することを検討中。
                        // NOTE: 他の簡略表記向けモードを競合するようになってきた。一時的に NotSupportedException 化。
                        // NOTE: 例えば、CarryPortableScope で作成した scope に Add してあったらそれが利用される、とか。
                        throw new NotSupportedException();
                        //var item = method.AddPortableScopeItem(fieldInfo);
                        //method.Body.ILOperator.Emit(OpCodes.Ldarg_0);
                        //method.Body.ILOperator.Emit(OpCodes.Ldfld, item);
                    }
                    else
                    {
                        EvalExpression(method, exp.Expression, state);

                        if (fieldInfo.IsStatic)
                        {
                            method.Body.ILOperator.Emit(OpCodes.Ldsfld, fieldInfo);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                }
            }
            else if ((propertyInfo = exp.Member as PropertyInfo) != null)
            {
                EvalExpression(method, exp.Expression, state);

                if (propertyInfo.IsStatic())
                {
                    throw new NotImplementedException();
                }
                else
                {
                    method.Body.ILOperator.Emit(OpCodes.Callvirt, propertyInfo.GetGetMethod());
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected virtual void EvalNew(IMethodBaseGenerator method, NewExpression exp, EvalState state)
        {
            EvalArguments(method, exp.Arguments, state);
            method.Body.ILOperator.Emit(OpCodes.Newobj, exp.Constructor);
            if (exp.Members != null)
            {
                //// 使わなくても構築は問題なくできそう？
                //// → 匿名型の初期化では見た目と異なり、コンストラクタの引数で設定するよう変更されている。
                //var variable = new VariableDefinition(methodDef.Module.Import(exp.Constructor.DeclaringType));
                //Direct.Emit(MC::Cil.OpCodes.Stloc, variable);
                //foreach (var member in exp.Members)
                //{
                //    Direct.Emit(MC::Cil.OpCodes.Ldloc, variable);
                //}
            }
        }

        protected virtual void EvalNewArray(IMethodBaseGenerator method, NewArrayExpression exp, EvalState state)
        {
            if (exp.NodeType == ExpressionType.NewArrayInit)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldc_I4_S, (sbyte)exp.Expressions.Count);
                method.Body.ILOperator.Emit(OpCodes.Newarr, exp.Type.GetElementType());
                var localDecl = method.Body.ILOperator.AddLocal(exp.Type);
                method.Body.ILOperator.Emit(OpCodes.Stloc, localDecl);
                exp.Expressions.
                ForEach((_exp, index) =>
                {
                    method.Body.ILOperator.Emit(OpCodes.Ldloc, localDecl);
                    method.Body.ILOperator.Emit(OpCodes.Ldc_I4, index);
                    state.CandidateDesigningExpressionStack.Push(_exp);
                    EvalExpression(method, _exp, state);
                    EvalAdjustState(method, _exp, state);
                    if (typeof(double).IsAssignableFrom(_exp.Type))
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        method.Body.ILOperator.Emit(OpCodes.Stelem_Ref);
                    }
                });
                method.Body.ILOperator.Emit(OpCodes.Ldloc, localDecl);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected virtual void EvalParameter(IMethodBaseGenerator method, ParameterExpression exp, EvalState state)
        {
            if (exp.Type == typeof(ReflectiveMethodDesigner))
            {
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IILOperator ILOperator { get { return method.Body.ILOperator; } }

        public ReadOnlyCollection<ILocalGenerator> Locals
        {
            get { return method.Body.Locals; }
        }

        public ReadOnlyCollection<IDirectiveGenerator> Directives
        {
            get { return method.Body.Directives; }
        }

        ReadOnlyCollection<ILocalDeclaration> IMethodBodyDeclaration.Locals
        {
            get { throw new NotImplementedException(); }
        }

        ReadOnlyCollection<IDirectiveDeclaration> IMethodBodyDeclaration.Directives
        {
            get { throw new NotImplementedException(); }
        }

        public void Dump()
        {
            for (int directivesIndex = 0; directivesIndex < Directives.Count; directivesIndex++)
            {
                Console.WriteLine(string.Format("L_{0:X4}: {1}", directivesIndex, Directives[directivesIndex]));
            }
        }

        protected MethodCallExpression CreateExtractExp1(Expression constant, Type type)
        {
            return Expression.Call(Expression.Constant(ReservedWords), ReservedWordXInfo_T.MakeGenericMethod(type), new Expression[] { constant });
        }

        protected class EvalState
        {
            public EvalState()
            {
                IfInfoStack = new Stack<IfInfo>();
                ReturnInfoStack = new Stack<ReturnInfo>();
                ExtractInfoStack = new Stack<ExtractInfo>();
                AllocInfoStack = new Stack<AllocInfo>();
                ShiftInfoStack = new Stack<ShiftInfo>();
                CandidateDesigningExpressionStack = new Stack<Expression>();
                MethodDesignedInfoStack = new Stack<MethodInfo>();
                ConstructorDesignedInfoStack = new Stack<ConstructorInfo>();
                PropertyDesignedInfoStack = new Stack<PropertyInfo>();
            }

            public bool ProhibitsLastAutoPop { get; set; }
            public Stack<IfInfo> IfInfoStack { get; private set; }
            public Stack<ReturnInfo> ReturnInfoStack { get; private set; }
            public Stack<ExtractInfo> ExtractInfoStack { get; private set; }
            public Stack<AllocInfo> AllocInfoStack { get; private set; }
            public Stack<ShiftInfo> ShiftInfoStack { get; private set; }
            public Stack<Expression> CandidateDesigningExpressionStack { get; private set; }
            public Stack<MethodInfo> MethodDesignedInfoStack { get; private set; }
            public Stack<ConstructorInfo> ConstructorDesignedInfoStack { get; private set; }
            public Stack<PropertyInfo> PropertyDesignedInfoStack { get; private set; }
        }

        protected class IfInfo
        {
            public IfInfo(ILabelDeclaration label)
            {
                Label = label;
            }

            public ILabelDeclaration Label { get; private set; }
        }

        protected class ReturnInfo
        {
            public ReturnInfo(ILabelDeclaration label)
            {
                Label = label;
            }

            public ILabelDeclaration Label { get; private set; }
        }

        protected class ExtractInfo
        {
            public ExtractInfo(Type type, object value)
            {
                Type = type;
                Value = value;
            }

            public Type Type { get; private set; }
            public object Value { get; private set; }
        }

        protected class AllocInfo
        {
            public AllocInfo(string name, Type type)
            {
                Name = name;
                Type = type;
            }

            public string Name { get; private set; }
            public Type Type { get; private set; }
        }

        protected class ShiftInfo
        {
            public ShiftInfo(int count)
            {
                Count = count;
            }

            public int Count { get; private set; }
        }

        protected class NameResolvableInfo : FieldInfo
        {
            string name;
            Type fieldType;
            public NameResolvableInfo(string name, Type fieldType)
            {
                this.name = name;
                this.fieldType = fieldType;
            }

            public override FieldAttributes Attributes
            {
                get { return FieldAttributes.Public | FieldAttributes.Static; }
            }

            public override RuntimeFieldHandle FieldHandle
            {
                get { throw new NotImplementedException(); }
            }

            public override Type FieldType
            {
                get { return fieldType; }
            }

            public override object GetValue(object obj)
            {
                throw new NotImplementedException();
            }

            public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            public override Type DeclaringType
            {
                get { throw new NotImplementedException(); }
            }

            public override object[] GetCustomAttributes(Type attributeType, bool inherit)
            {
                throw new NotImplementedException();
            }

            public override object[] GetCustomAttributes(bool inherit)
            {
                throw new NotImplementedException();
            }

            public override bool IsDefined(Type attributeType, bool inherit)
            {
                throw new NotImplementedException();
            }

            public override string Name
            {
                get { return name; }
            }

            public override Type ReflectedType
            {
                get { throw new NotImplementedException(); }
            }
        }

        protected abstract class IndexResolvableInfo : FieldInfo
        {
            int index;
            Type fieldType;
            public IndexResolvableInfo(int index, Type fieldType)
            {
                this.index = index;
                this.fieldType = fieldType;
            }

            public override FieldAttributes Attributes
            {
                get { return FieldAttributes.Public | FieldAttributes.Static; }
            }

            public override RuntimeFieldHandle FieldHandle
            {
                get { throw new NotImplementedException(); }
            }

            public override Type FieldType
            {
                get { return fieldType; }
            }

            public override object GetValue(object obj)
            {
                throw new NotImplementedException();
            }

            public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            public override Type DeclaringType
            {
                get { throw new NotImplementedException(); }
            }

            public override object[] GetCustomAttributes(Type attributeType, bool inherit)
            {
                throw new NotImplementedException();
            }

            public override object[] GetCustomAttributes(bool inherit)
            {
                throw new NotImplementedException();
            }

            public override bool IsDefined(Type attributeType, bool inherit)
            {
                throw new NotImplementedException();
            }

            public override string Name
            {
                get { throw new NotImplementedException(); }
            }

            public override Type ReflectedType
            {
                get { throw new NotImplementedException(); }
            }

            public int Index
            {
                get { return index; }
            }
        }

        protected class LocalIndexResolvableInfo : IndexResolvableInfo
        {
            public LocalIndexResolvableInfo(int index, Type fieldType)
                : base(index, fieldType)
            {
            }
        }

        protected class ParameterIndexResolvableInfo : IndexResolvableInfo
        {
            public ParameterIndexResolvableInfo(int index, Type fieldType)
                : base(index, fieldType)
            {
            }
        }

        public ReadOnlyCollection<IDirectiveGenerator> ToDirectives(LambdaExpression expression)
        {
            var dummyAssemblyName = new AssemblyName("Dummy");
            var dummyAssembly = method.DeclaringType.Module.Assembly.CreateInstance(dummyAssemblyName);
            var dummyModule = dummyAssembly.AddModule("Dummy");
            var dummyType = dummyModule.AddType("Dummy.Dummy", TypeAttributes.Public, typeof(object));
            var dummyMethod = dummyType.AddMethod("Dummy", MethodAttributes.Public | MethodAttributes.Static, typeof(void), Type.EmptyTypes);

            EvalTo(_ => _.X(expression), dummyMethod);

            return dummyMethod.Body.Directives;
        }

        public ILocalGenerator AddLocal(string name, Type localType)
        {
            throw new NotImplementedException();
        }

        public ILocalGenerator AddLocal(Type localType)
        {
            throw new NotImplementedException();
        }

        public ILocalGenerator AddLocal(ILocalGenerator localGen)
        {
            throw new NotImplementedException();
        }

        public IMethodBaseGenerator Method
        {
            get { return method; }
        }

        IMethodBaseDeclaration IMethodBodyDeclaration.Method
        {
            get { return Method; }
        }

        class MethodReservedWords : IMethodReservedWords
        {
            public void Base()
            {
                throw new NotSupportedException();
            }

            public object This()
            {
                throw new NotSupportedException();
            }

            public T DupAddOne<T>(T variable)
            {
                throw new NotSupportedException();
            }

            public T AddOneDup<T>(T variable)
            {
                throw new NotSupportedException();
            }

            public T SubOneDup<T>(T variable)
            {
                throw new NotSupportedException();
            }

            public object New(ConstructorInfo constructor, object parameter)
            {
                throw new NotSupportedException();
            }

            public T New<T>(ConstructorInfo constructor, params object[] parameters)
            {
                throw new NotSupportedException();
            }

            public object Invoke(MethodInfo method, params object[] parameters)
            {
                throw new NotSupportedException();
            }

            public object Invoke(object variable, MethodInfo method, params object[] parameters)
            {
                throw new NotSupportedException();
            }

            public object Ftn(object variable, IMethodDeclaration methodDecl)
            {
                throw new NotSupportedException();
            }

            public object Ftn(IMethodDeclaration methodDecl)
            {
                throw new NotSupportedException();
            }

            public object Ftn(MethodInfo methodInfo)
            {
                throw new NotSupportedException();
            }

            public void If(bool condition)
            {
                throw new NotSupportedException();
            }

            public void ElseIf(bool condition)
            {
                throw new NotSupportedException();
            }

            public void Else()
            {
                throw new NotSupportedException();
            }

            public void EndIf()
            {
                throw new NotSupportedException();
            }

            public void End()
            {
                throw new NotSupportedException();
            }

            public void Return<T>(T variable)
            {
                throw new NotSupportedException();
            }

            public T Ld<T>(string variableName)
            {
                throw new NotSupportedException();
            }

            public object Ld(string variableName)
            {
                throw new NotSupportedException();
            }

            public object[] Ld(string[] variableNames)
            {
                throw new NotSupportedException();
            }

            public object[] Ld(string[] variableNames, int shift)
            {
                throw new NotSupportedException();
            }

            public object LdArg(int variableIndex)
            {
                throw new NotSupportedException();
            }

            public object[] LdArg(int[] variableIndexes)
            {
                throw new NotSupportedException();
            }

            public IMethodAllocReservedWords<T> St<T>(string variableName)
            {
                throw new NotSupportedException();
            }

            public IMethodAllocReservedWords St(string variableName)
            {
                throw new NotSupportedException();
            }

            public IMethodAllocReservedWords<T> Alloc<T>(T variable)
            {
                throw new NotSupportedException();
            }

            public IMethodAllocReservedWords Alloc(object variable)
            {
                throw new NotSupportedException();
            }

            public T X<T>(T constant)
            {
                throw new NotSupportedException();
            }

            public T X<T>(object constant)
            {
                throw new NotSupportedException();
            }

            public TValue Cm<TValue>(TValue constMember, Type declaringType)
            {
                throw new NotSupportedException();
            }

            public bool AreEqual(object left, object right)
            {
                throw new NotSupportedException();
            }
        }

        class MethodAllocReservedWords : IMethodAllocReservedWords
        {
            public object As(object value)
            {
                throw new NotSupportedException();
            }
        }

        class MethodAllocReservedWords<T> : IMethodAllocReservedWords<T>
        {
            public T As(T value)
            {
                throw new NotSupportedException();
            }
        }
    }
}
