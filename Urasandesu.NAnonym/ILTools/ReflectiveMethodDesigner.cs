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
using Urasandesu.NAnonym.Mixins.Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.ILTools
{
    public class ReflectiveMethodDesigner : IMethodBodyGenerator
    {
        public static readonly MethodInfo IFieldDeclarationSetValue_object_object = TypeSavable.GetInstanceMethod<IFieldDeclaration, object, object>(_ => _.SetValue);
        public static readonly MethodInfo IFieldDeclarationGetValue_object = TypeSavable.GetInstanceMethod<IFieldDeclaration, object, object>(_ => _.GetValue);
        public static readonly MethodInfo FieldInfoSetValue_object_object = TypeSavable.GetInstanceMethod<FieldInfo, object, object>(_ => _.SetValue);
        public static readonly MethodInfo FieldInfoGetValue_object = TypeSavable.GetInstanceMethod<FieldInfo, object, object>(_ => _.GetValue);
        public static readonly MethodInfo MethodInfoInvoke_object_objects = TypeSavable.GetInstanceMethod<MethodInfo, object, object[], object>(_ => _.Invoke);
        public static readonly MethodInfo ConstructorInfoInvoke_objects = TypeSavable.GetInstanceMethod<ConstructorInfo, object[], object>(_ => _.Invoke);
        public static readonly MethodInfo PropertyInfoSetValue_object_object_objects = TypeSavable.GetInstanceMethod<PropertyInfo, object, object, object[]>(_ => _.SetValue);
        public static readonly MethodInfo PropertyInfoGetValue_object_objects = TypeSavable.GetInstanceMethod<PropertyInfo, object, object[], object>(_ => _.GetValue);

        public static readonly MethodInfo DslExtract_T = TypeSavable.GetStaticMethod<object, object>(() => Dsl.Extract).GetGenericMethodDefinition();
        public static readonly MethodInfo DslExtract_object = TypeSavable.GetStaticMethod<object, object>(() => Dsl.Extract<object>).GetGenericMethodDefinition();

        readonly IMethodBaseGenerator method;
        protected readonly EvalState state;

        protected internal ReflectiveMethodDesigner(IMethodBaseGenerator method)
        {
            this.method = method;
            state = new EvalState();
        }

        public void Eval(Expression<Action> exp)
        {
            Eval(method, exp.Body, state);
        }

        public void EvalTo(Expression<Action> exp, IMethodBaseGenerator method)
        {
            Eval(method, exp.Body, new EvalState());
        }

        protected virtual void Eval(IMethodBaseGenerator method, Expression exp, EvalState state)
        {
            EvalExpression(method, exp, state);
            EvalExit(method, exp, state);
        }

        protected virtual void EvalExit(IMethodBaseGenerator method, Expression exp, EvalState state)
        {
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
                EvalExpression(method, exp, state);
            }
        }

        protected virtual void EvalArgumentsWithoutConversion(IMethodBaseGenerator method, ReadOnlyCollection<Expression> exps, EvalState state)
        {
            foreach (var exp in exps)
            {
                if (exp.NodeType == ExpressionType.Convert)
                {
                    EvalExpression(method, ((UnaryExpression)exp).Operand, state);
                }
                else
                {
                    EvalExpression(method, exp, state);
                }
            }
        }

        protected virtual void EvalExpression(IMethodBaseGenerator method, Expression exp, EvalState state)
        {
            state.CallStack.Push(exp);
            state.ProhibitsLastAutoPop = false;
            if (exp == null) return;

            switch (exp.NodeType)
            {
                case ExpressionType.Add:
                case ExpressionType.ArrayIndex:
                case ExpressionType.Multiply:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.AndAlso:
                case ExpressionType.ExclusiveOr:
                    EvalBinary(method, (BinaryExpression)exp, state);
                    break;
                case ExpressionType.AddChecked:
                    throw new NotImplementedException();
                case ExpressionType.And:
                    throw new NotImplementedException();
                case ExpressionType.Call:
                    EvalMethodCall(method, (MethodCallExpression)exp, state);
                    break;
                case ExpressionType.Coalesce:
                    throw new NotImplementedException();
                case ExpressionType.Conditional:
                    EvalConditional(method, (ConditionalExpression)exp, state);
                    break;
                case ExpressionType.Constant:
                    EvalConstant(method, (ConstantExpression)exp, state);
                    break;
                case ExpressionType.ArrayLength:
                case ExpressionType.Convert:
                case ExpressionType.TypeAs:
                    EvalUnary(method, (UnaryExpression)exp, state);
                    break;
                case ExpressionType.ConvertChecked:
                    throw new NotImplementedException();
                case ExpressionType.Divide:
                    throw new NotImplementedException();
                case ExpressionType.GreaterThan:
                    throw new NotImplementedException();
                case ExpressionType.GreaterThanOrEqual:
                    throw new NotImplementedException();
                case ExpressionType.Invoke:
                    EvalInvoke(method, (InvocationExpression)exp, state);
                    break;
                case ExpressionType.Lambda:
                    EvalLambda(method, (LambdaExpression)exp, state);
                    break;
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
                    break;
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
                    break;
                case ExpressionType.NewArrayBounds:
                    throw new NotImplementedException();
                case ExpressionType.NewArrayInit:
                    EvalNewArray(method, (NewArrayExpression)exp, state);
                    break;
                case ExpressionType.Not:
                    throw new NotImplementedException();
                case ExpressionType.Or:
                    throw new NotImplementedException();
                case ExpressionType.OrElse:
                    throw new NotImplementedException();
                case ExpressionType.Parameter:
                    EvalParameter(method, (ParameterExpression)exp, state);
                    break;
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

            EvalAdjustState(method, exp, state);
        }

        protected virtual void EvalBinary(IMethodBaseGenerator method, BinaryExpression exp, EvalState state)
        {
            if (exp.NodeType == ExpressionType.Add || 
                exp.NodeType == ExpressionType.Multiply ||
                exp.NodeType == ExpressionType.ExclusiveOr)
            {
                EvalArithmeticBinary(method, exp, state);
            }
            else if (exp.NodeType == ExpressionType.ArrayIndex)
            {
                EvalArrayIndexBinary(method, exp, state);
            }
            else if (exp.NodeType == ExpressionType.Equal ||
                     exp.NodeType == ExpressionType.NotEqual)
            {
                EvalLogicalBinaryWithoutAssociation(method, exp, state);
            }
            else if (exp.NodeType == ExpressionType.AndAlso)
            {
                EvalLogicalBinary(method, exp, state);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected virtual void EvalArithmeticBinary(IMethodBaseGenerator method, BinaryExpression exp, EvalState state)
        {
            EvalExpression(method, exp.Left, state);
            EvalExpression(method, exp.Right, state);

            // TODO: ?? 演算子とか演算子のオーバーロードとか。
            if (exp.Conversion != null) throw new NotImplementedException();
            if (exp.Method != null) throw new NotImplementedException();

            if (exp.NodeType == ExpressionType.Add)
            {
                if (exp.Left.Type == typeof(int) && exp.Right.Type == typeof(int))
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
                if (exp.Left.Type == typeof(int) && exp.Right.Type == typeof(int))
                {
                    method.Body.ILOperator.Emit(OpCodes.Mul);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else if (exp.NodeType == ExpressionType.ExclusiveOr)
            {
                if (exp.Left.Type == typeof(int) && exp.Right.Type == typeof(int))
                {
                    method.Body.ILOperator.Emit(OpCodes.Xor);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected virtual void EvalArrayIndexBinary(IMethodBaseGenerator method, BinaryExpression exp, EvalState state)
        {
            EvalExpression(method, exp.Left, state);
            EvalExpression(method, exp.Right, state);

            // TODO: ?? 演算子とか演算子のオーバーロードとか。
            if (exp.Conversion != null) throw new NotImplementedException();
            if (exp.Method != null) throw new NotImplementedException();

            if (typeof(double).IsAssignableFrom(exp.Left.Type.GetElementType()))
            {
                throw new NotImplementedException();
            }
            else
            {
                method.Body.ILOperator.Emit(OpCodes.Ldelem_Ref);
            }
        }

        protected virtual void EvalLogicalBinaryWithoutAssociation(IMethodBaseGenerator method, BinaryExpression exp, EvalState state)
        {
            EvalExpression(method, exp.Left, state);
            EvalExpression(method, exp.Right, state);

            // TODO: ?? 演算子とか演算子のオーバーロードとか。
            if (exp.Conversion != null) throw new NotImplementedException();
            if (exp.Method != null) throw new NotImplementedException();

            if (exp.NodeType == ExpressionType.Equal)
            {
                method.Body.ILOperator.Emit(OpCodes.Ceq);
            }
            else if (exp.NodeType == ExpressionType.NotEqual)
            {
                method.Body.ILOperator.Emit(OpCodes.Ceq);
                method.Body.ILOperator.Emit(OpCodes.Ldc_I4_0);
                method.Body.ILOperator.Emit(OpCodes.Ceq);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected virtual void EvalLogicalBinary(IMethodBaseGenerator method, BinaryExpression exp, EvalState state)
        {
            EvalExpression(method, exp.Left, state);

            var labelLeft = default(ILabelDeclaration);
            var localLeft = default(ILocalDeclaration);
            if (exp.NodeType == ExpressionType.AndAlso)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldc_I4_0);
                method.Body.ILOperator.Emit(OpCodes.Ceq);
                localLeft = method.Body.ILOperator.AddLocal(typeof(bool));
                method.Body.ILOperator.Emit(OpCodes.Stloc, localLeft);
                method.Body.ILOperator.Emit(OpCodes.Ldloc, localLeft);
                labelLeft = method.Body.ILOperator.AddLabel();
                method.Body.ILOperator.Emit(OpCodes.Brtrue, labelLeft);
            }
            else
            {
                throw new NotImplementedException();
            }

            EvalExpression(method, exp.Right, state);

            var labelRight = default(ILabelDeclaration);
            var localRight = default(ILocalDeclaration);
            if (exp.NodeType == ExpressionType.AndAlso)
            {
                localRight = method.Body.ILOperator.AddLocal(typeof(bool));
                method.Body.ILOperator.Emit(OpCodes.Stloc, localRight);
                method.Body.ILOperator.Emit(OpCodes.Ldloc, localRight);
                labelRight = method.Body.ILOperator.AddLabel();
                method.Body.ILOperator.Emit(OpCodes.Br, labelRight);
                method.Body.ILOperator.SetLabel(labelLeft);
                method.Body.ILOperator.Emit(OpCodes.Ldc_I4_0);
                method.Body.ILOperator.SetLabel(labelRight);
            }
            else
            {
                throw new NotImplementedException();
            }

            // TODO: ?? 演算子とか演算子のオーバーロードとか。
            if (exp.Conversion != null) throw new NotImplementedException();
            if (exp.Method != null) throw new NotImplementedException();
        }

        protected virtual void EvalMethodCall(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            if (exp.Object == null)
            {
                if (exp.Method.DeclaringType.IsDefined(typeof(MethodReservedWordsAttribute), false))
                {
                    if (exp.Method.IsDefined(typeof(MethodReservedWordBaseAttribute), false)) EvalBase(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordThisAttribute), false)) EvalThis(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordIncrementAttribute), false)) EvalDupAddOne(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordAddOneDupAttribute), false)) EvalAddOneDup(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordDecrementAttribute), false)) EvalSubOneDup(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordNewAttribute), false)) EvalNew(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordInvokeAttribute), false)) EvalInvoke(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordLoadPtrAttribute), false)) EvalFtn(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordIfAttribute), false)) EvalIf(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordElseIfAttribute), false)) EvalElseIf(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordElseAttribute), false)) EvalElse(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordEndIfAttribute), false)) EvalEndIf(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordReturnAttribute), false)) EvalReturn(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordEndAttribute), false)) EvalEnd(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordLoadAttribute), false)) EvalLd(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordAllocateAttribute), false)) EvalAlloc(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordStoreAttribute), false)) EvalSt(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordExtractAttribute), false)) EvalExtract(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordConstMemberAttribute), false)) EvalCm(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(MethodReservedWordLoadArgumentAttribute), false)) EvalLdArg(method, exp, state);
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    EvalArguments(method, exp.Arguments, state);
                    method.Body.ILOperator.Emit(OpCodes.Call, exp.Method);
                }
            }
            else
            {
                if (exp.Object.Type.IsDefined(typeof(MethodAllocReservedWordsAttribute), false))
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
                    else if (exp.Method == IFieldDeclarationGetValue_object) EvalIFieldDeclarationGetValue_object(method, exp, state);
                    else if (exp.Method == IFieldDeclarationSetValue_object_object) EvalIFieldDeclarationSetValue_object_object(method, exp, state);
                    else if (exp.Method == FieldInfoGetValue_object) EvalFieldInfoGetValue_object(method, exp, state);
                    else if (exp.Method == FieldInfoSetValue_object_object) EvalFieldInfoSetValue_object_object(method, exp, state);
                    else if (exp.Method == PropertyInfoSetValue_object_object_objects) EvalPropertyInfoSetValue_object_object_objects(method, exp, state);
                    else if (exp.Method == PropertyInfoGetValue_object_objects) EvalPropertyInfoGetValue_object_objects(method, exp, state);
                    else
                    {
                        // instance method call
                        EvalExpression(method, exp.Object, state);
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

        [Obsolete]
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
            if (0 < state.ExtractInfos.Count)
            {
                var extractInfo = state.ExtractInfos.Pop();
                var constructor = (ConstructorInfo)extractInfo.Value;
                method.Body.ILOperator.Emit(OpCodes.Newobj, constructor);
            }
        }

        [Obsolete]
        protected virtual void EvalInvoke(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            EvalExpression(method, exp.Arguments[0], state);
            if (0 < state.ExtractInfos.Count)
            {
                throw new NotImplementedException();
            }
            EvalExpression(method, exp.Arguments[2], state);
            if (0 < state.ExtractInfos.Count)
            {
                throw new NotImplementedException();
            }
            EvalExpression(method, exp.Arguments[1], state);
            if (0 < state.ExtractInfos.Count)
            {
                var extractInfo = state.ExtractInfos.Pop();
                var methodInfo = (MethodInfo)extractInfo.Value;
                method.Body.ILOperator.Emit(OpCodes.Callvirt, methodInfo);
            }
        }

        protected virtual void EvalMethodInfoInvoke_object_objects(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var mi = default(MethodInfo);
            {
                var extractMi = DslExtract_T.MakeGenericMethod(typeof(MethodInfo));
                var paramExps = new Expression[] { exp.Object };
                var extractExp = Expression.Call(extractMi, paramExps);
                EvalExtract(method, extractExp, state);
                if (state.ExtractInfos.Count == 0)
                {
                    throw new NotSupportedException();
                }
                mi = (MethodInfo)state.ExtractInfos.Pop().Value;
            }

            if (!mi.IsStatic)
            {
                EvalExpression(method, exp.Arguments[0], state);
            }

            if (exp.Arguments[1].NodeType == ExpressionType.NewArrayInit)
            {
                EvalArguments(method, ((NewArrayExpression)exp.Arguments[1]).Expressions, state);
            }
            else
            {
                throw new NotImplementedException();
            }

            if (mi.IsStatic)
            {
                method.Body.ILOperator.Emit(OpCodes.Call, mi);
            }
            else
            {
                method.Body.ILOperator.Emit(OpCodes.Callvirt, mi);
            }

            state.ProhibitsLastAutoPop = mi.ReturnType == typeof(void);
            state.MethodInfoDesignedInfos.Push(mi);
        }

        protected virtual void EvalConstructorInfoInvoke_objects(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var ci = default(ConstructorInfo);
            {
                var extractMi = DslExtract_T.MakeGenericMethod(typeof(ConstructorInfo));
                var paramExps = new Expression[] { exp.Object };
                var extractExp = Expression.Call(extractMi, paramExps);
                EvalExtract(method, extractExp, state);
                if (state.ExtractInfos.Count == 0)
                {
                    throw new NotSupportedException();
                }
                ci = (ConstructorInfo)state.ExtractInfos.Pop().Value;
            }

            state.ParametricConstructorInfos.Add(exp, ci);

            if (exp.Arguments[0].NodeType == ExpressionType.NewArrayInit)
            {
                EvalArgumentsWithoutConversion(method, ((NewArrayExpression)exp.Arguments[0]).Expressions, state);
            }
            else if (exp.Arguments[0].NodeType == ExpressionType.Constant && ((ConstantExpression)exp.Arguments[0]).Value == null)
            {
                // discard...
            }
            else
            {
                throw new NotImplementedException();
            }

            state.ParametricConstructorInfos.Remove(exp);

            method.Body.ILOperator.Emit(OpCodes.Newobj, ci);
            state.ReturneeConstructorInfos.Push(ci);
        }

        protected virtual void EvalIFieldDeclarationGetValue_object(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var fd = default(IFieldDeclaration);
            {
                var extractMi = DslExtract_T.MakeGenericMethod(typeof(IFieldDeclaration));
                var paramExps = new Expression[] { exp.Object };
                var extractExp = Expression.Call(extractMi, paramExps);
                EvalExtract(method, extractExp, state);
                if (state.ExtractInfos.Count == 0)
                {
                    throw new NotSupportedException();
                }
                fd = (IFieldDeclaration)state.ExtractInfos.Pop().Value;
            }

            if (!fd.IsStatic)
            {
                EvalExpression(method, exp.Arguments[0], state);
            }

            if (fd.IsStatic)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldsfld, fd);
            }
            else
            {
                method.Body.ILOperator.Emit(OpCodes.Ldfld, fd);
            }

            state.FieldDeclDesignedInfos.Push(fd);
        }

        protected virtual void EvalIFieldDeclarationSetValue_object_object(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var fd = default(IFieldDeclaration);
            {
                var extractMi = DslExtract_T.MakeGenericMethod(typeof(IFieldDeclaration));
                var paramExps = new Expression[] { exp.Object };
                var extractExp = Expression.Call(extractMi, paramExps);
                EvalExtract(method, extractExp, state);
                if (state.ExtractInfos.Count == 0)
                {
                    throw new NotSupportedException();
                }
                fd = (IFieldDeclaration)state.ExtractInfos.Pop().Value;
            }

            if (!fd.IsStatic)
            {
                EvalExpression(method, exp.Arguments[0], state);
            }

            if (fd.FieldType.IsValueType &&
                exp.Arguments[1].NodeType == ExpressionType.Convert &&
                ((UnaryExpression)exp.Arguments[1]).Operand.Type.IsSubclassOf(typeof(ValueType)))
            {
                EvalExpression(method, ((UnaryExpression)exp.Arguments[1]).Operand, state);
            }
            else
            {
                EvalExpression(method, exp.Arguments[1], state);
            }

            if (fd.IsStatic)
            {
                method.Body.ILOperator.Emit(OpCodes.Stsfld, fd);
            }
            else
            {
                method.Body.ILOperator.Emit(OpCodes.Stfld, fd);
            }
        }

        protected virtual void EvalFieldInfoGetValue_object(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var fi = default(FieldInfo);
            {
                var extractMi = DslExtract_T.MakeGenericMethod(typeof(FieldInfo));
                var paramExps = new Expression[] { exp.Object };
                var extractExp = Expression.Call(extractMi, paramExps);
                EvalExtract(method, extractExp, state);
                if (state.ExtractInfos.Count == 0)
                {
                    throw new NotSupportedException();
                }
                fi = (FieldInfo)state.ExtractInfos.Pop().Value;
            }

            if (!fi.IsStatic)
            {
                EvalExpression(method, exp.Arguments[0], state);
            }

            if (fi.IsStatic)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldsfld, fi);
            }
            else
            {
                method.Body.ILOperator.Emit(OpCodes.Ldfld, fi);
            }

            state.ReturneeFieldInfos.Push(fi);
        }

        protected virtual void EvalFieldInfoSetValue_object_object(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var fi = default(FieldInfo);
            {
                var extractMi = DslExtract_T.MakeGenericMethod(typeof(FieldInfo));
                var paramExps = new Expression[] { exp.Object };
                var extractExp = Expression.Call(extractMi, paramExps);
                EvalExtract(method, extractExp, state);
                if (state.ExtractInfos.Count == 0)
                {
                    throw new NotSupportedException();
                }
                fi = (FieldInfo)state.ExtractInfos.Pop().Value;
            }

            state.ParametricFieldInfos.Add(exp, fi);

            if (!fi.IsStatic)
            {
                EvalExpression(method, exp.Arguments[0], state);
            }

            if (exp.Arguments[1].NodeType == ExpressionType.Convert)
            {
                EvalExpression(method, ((UnaryExpression)exp.Arguments[1]).Operand, state);
            }
            else
            {
                EvalExpression(method, exp.Arguments[1], state);
            }

            if (fi.IsStatic)
            {
                method.Body.ILOperator.Emit(OpCodes.Stsfld, fi);
            }
            else
            {
                method.Body.ILOperator.Emit(OpCodes.Stfld, fi);
            }

            state.ParametricFieldInfos.Remove(exp);
        }

        protected virtual void EvalPropertyInfoSetValue_object_object_objects(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var pi = default(PropertyInfo);
            {
                var extractMi = DslExtract_T.MakeGenericMethod(typeof(PropertyInfo));
                var paramExps = new Expression[] { exp.Object };
                var extractExp = Expression.Call(extractMi, paramExps);
                EvalExtract(method, extractExp, state);
                if (state.ExtractInfos.Count == 0)
                {
                    throw new NotSupportedException();
                }
                pi = (PropertyInfo)state.ExtractInfos.Pop().Value;
            }

            var setter = pi.GetSetMethod(true);
            if (!setter.IsStatic)
            {
                EvalExpression(method, exp.Arguments[0], state);
            }

            if (pi.PropertyType.IsValueType && 
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
            var pi = default(PropertyInfo);
            {
                var extractMi = DslExtract_T.MakeGenericMethod(typeof(PropertyInfo));
                var paramExps = new Expression[] { exp.Object };
                var extractExp = Expression.Call(extractMi, paramExps);
                EvalExtract(method, extractExp, state);
                if (state.ExtractInfos.Count == 0)
                {
                    throw new NotSupportedException();
                }
                pi = (PropertyInfo)state.ExtractInfos.Pop().Value;
            }

            var getter = pi.GetGetMethod(true);
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

            state.PropertyInfoDesignedInfos.Push(pi);
        }

        // TODO: ここも EvalExtract 暗黙的に呼ぶようにする。
        protected virtual void EvalFtn(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            if (1 < exp.Arguments.Count)
            {
                EvalExpression(method, exp.Arguments[0], state);
                if (0 < state.ExtractInfos.Count)
                {
                    throw new NotImplementedException();
                }
            }

            if (exp.Arguments[exp.Arguments.Count - 1].Type == typeof(IMethodDeclaration))
            {
                var extractExp = CreateExtractExp_T(exp.Arguments[exp.Arguments.Count - 1], typeof(IMethodDeclaration));
                EvalExtract(method, extractExp, state);
                if (0 < state.ExtractInfos.Count)
                {
                    var extractInfo = state.ExtractInfos.Pop();
                    var methodDecl = (IMethodDeclaration)extractInfo.Value;
                    method.Body.ILOperator.Emit(OpCodes.Ldftn, methodDecl);
                }
            }
            else if (exp.Arguments[exp.Arguments.Count - 1].Type == typeof(MethodInfo))
            {
                var extractExp = CreateExtractExp_T(exp.Arguments[exp.Arguments.Count - 1], typeof(MethodInfo));
                EvalExtract(method, extractExp, state);
                if (0 < state.ExtractInfos.Count)
                {
                    var extractInfo = state.ExtractInfos.Pop();
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
            state.IfInfos.Push(new IfInfo(labelDecl));
            method.Body.ILOperator.Emit(OpCodes.Brtrue, labelDecl);
        }

        protected virtual void EvalElseIf(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var ifInfo = state.IfInfos.Pop();
            method.Body.ILOperator.SetLabel(ifInfo.Label);
            EvalExpression(method, exp.Arguments[0], state);
            method.Body.ILOperator.Emit(OpCodes.Ldc_I4_0);
            method.Body.ILOperator.Emit(OpCodes.Ceq);
            var localDecl = method.Body.ILOperator.AddLocal(typeof(bool));
            method.Body.ILOperator.Emit(OpCodes.Stloc, localDecl);
            method.Body.ILOperator.Emit(OpCodes.Ldloc, localDecl);
            var labelDecl = method.Body.ILOperator.AddLabel();
            state.IfInfos.Push(new IfInfo(labelDecl));
            method.Body.ILOperator.Emit(OpCodes.Brtrue, labelDecl);
        }

        protected virtual void EvalElse(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var ifInfo = state.IfInfos.Pop();
            method.Body.ILOperator.SetLabel(ifInfo.Label);
            var labelDecl = method.Body.ILOperator.AddLabel();
            state.IfInfos.Push(new IfInfo(labelDecl));
        }

        protected virtual void EvalEndIf(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var ifInfo = state.IfInfos.Pop();
            method.Body.ILOperator.SetLabel(ifInfo.Label);
        }

        protected virtual void EvalReturn(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            EvalExpression(method, exp.Arguments[0], state);
            var labelDecl = default(ILabelDeclaration);
            if (0 < state.ReturnInfos.Count)
            {
                var returnInfo = state.ReturnInfos.Pop();
                labelDecl = returnInfo.Label;
                state.ReturnInfos.Push(returnInfo);
            }
            else
            {
                labelDecl = method.Body.ILOperator.AddLabel();
                state.ReturnInfos.Push(new ReturnInfo(labelDecl));
            }
            method.Body.ILOperator.Emit(OpCodes.Br, labelDecl);
        }

        protected virtual void EvalEnd(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            if (1 < state.ReturnInfos.Count)
            {
                throw new NotSupportedException();
            }

            if (0 < state.ReturnInfos.Count)
            {
                var returnInfo = state.ReturnInfos.Pop();
                method.Body.ILOperator.SetLabel(returnInfo.Label);
            }
            method.Body.ILOperator.Emit(OpCodes.Ret);
        }

        protected virtual void EvalLd(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            var extractExp = default(MethodCallExpression);

            if (exp.Arguments.Count == 2)
            {
                extractExp = CreateExtractExp_T(exp.Arguments[1], typeof(int));
                EvalExtract(method, extractExp, state);
            }

            extractExp = CreateExtractExp_T(exp.Arguments[0], exp.Arguments[0].Type);
            EvalExtract(method, extractExp, state);

            if (0 < state.ExtractInfos.Count)
            {
                var extractInfo = state.ExtractInfos.Pop();
                var shiftCount = -1;
                if (0 < state.ExtractInfos.Count)
                {
                    var _extractInfo = state.ExtractInfos.Pop();
                    shiftCount = (int)_extractInfo.Value;
                }

                if (extractInfo.Type == typeof(string[]))
                {
                    ((string[])extractInfo.Value).
                    ForEach(name =>
                    {
                        var fieldInfo = new NameResolvableInfo(name, typeof(object));
                        if (-1 < shiftCount) state.ShiftInfos.Push(new ShiftInfo(shiftCount));
                        EvalMember(method, Expression.Field(null, fieldInfo), state);
                    });
                }
                else
                {
                    var fieldInfo = new NameResolvableInfo((string)extractInfo.Value, extractInfo.Type);
                    if (-1 < shiftCount) state.ShiftInfos.Push(new ShiftInfo(shiftCount));
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
            extractExp = CreateExtractExp_T(exp.Arguments[0], exp.Arguments[0].Type);
            EvalExtract(method, extractExp, state);

            if (0 < state.ExtractInfos.Count)
            {
                var extractInfo = state.ExtractInfos.Pop();
                if (extractInfo.Type == typeof(int[]))
                {
                    ((int[])extractInfo.Value).
                    ForEach(index =>
                    {
                        var fieldInfo = new ParameterIndexResolvableInfo(index, typeof(object));
                        EvalMember(method, Expression.Field(null, fieldInfo), state);
                    });
                }
                else
                {
                    var fieldInfo = new ParameterIndexResolvableInfo((int)extractInfo.Value, extractInfo.Type);
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
                state.AllocInfos.Push(new AllocInfo(fieldInfo.Name, fieldInfo.FieldType));
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        protected virtual void EvalAllocAs(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            EvalExpression(method, exp.Object, state);
            if (0 < state.AllocInfos.Count)
            {
                var allocInfo = state.AllocInfos.Pop();

                var stExp = exp.Arguments[0];
                var localGen = default(ILocalGenerator);
                var parameterGen = default(IParameterGenerator);
                var fieldGen = default(IFieldGenerator);
                if ((localGen = method.Body.Locals.FirstOrDefault(_localGen => _localGen.Name == allocInfo.Name)) != null)
                {
                    EvalExpression(method, stExp, state);
                    method.Body.ILOperator.Emit(OpCodes.Stloc, localGen);
                    method.Body.ILOperator.Emit(OpCodes.Ldloc, localGen);
                }
                else if ((parameterGen = method.Parameters.FirstOrDefault(_parameterGen => _parameterGen.Name == allocInfo.Name)) != null)
                {
                    EvalExpression(method, stExp, state);
                    method.Body.ILOperator.Emit(OpCodes.Starg, parameterGen);
                    method.Body.ILOperator.Emit(OpCodes.Ldarg, parameterGen);
                }
                else if (method.DeclaringType != null &&
                    (fieldGen = method.DeclaringType.Fields.FirstOrDefault(_fieldGen => _fieldGen.Name == allocInfo.Name)) != null)
                {
                    method.Body.ILOperator.Emit(OpCodes.Ldarg_0);
                    EvalExpression(method, stExp, state);
                    method.Body.ILOperator.Emit(OpCodes.Stfld, fieldGen);
                    method.Body.ILOperator.Emit(OpCodes.Ldarg_0);
                    method.Body.ILOperator.Emit(OpCodes.Ldfld, fieldGen);
                }
                else
                {
                    EvalExpression(method, stExp, state);
                    var local = method.Body.ILOperator.AddLocal(allocInfo.Name, allocInfo.Type);
                    method.Body.ILOperator.Emit(OpCodes.Stloc, local);
                    method.Body.ILOperator.Emit(OpCodes.Ldloc, local);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
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

            var extractMethod = DslExtract_object.MakeGenericMethod(xInfoType);
            var parameterExps = new Expression[] { exp.Arguments[0] };
            var extractExp = Expression.Call(extractMethod, parameterExps);
            EvalExtract(method, extractExp, state);
            if (0 < state.ExtractInfos.Count)
            {
                var extractInfo = state.ExtractInfos.Pop();
                var name = (string)extractInfo.Value;
                state.AllocInfos.Push(new AllocInfo(name, extractInfo.Type));
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
                state.ExtractInfos.Push(new ExtractInfo(exp.Type, o));
            }
            else
            {
                var extracted1 = Expression.Lambda(exp.Arguments[1]).Compile();
                Type type = (Type)extracted1.DynamicInvoke();
                state.ExtractInfos.Push(new ExtractInfo(type, o));
            }
        }

        protected virtual void EvalAdjustState(IMethodBaseGenerator method, Expression exp, EvalState state)
        {
            // Adjust ExtractInfoStack. 
            if (0 < state.ExtractInfos.Count)
            {
                var extractInfo = state.ExtractInfos.Pop();
                if (extractInfo.Type.IsArray)
                {
                    EvalNewArray(method, Expression.NewArrayInit(extractInfo.Type.GetElementType(),
                        ((Array)extractInfo.Value).Cast<object>().Select(value => (Expression)Expression.Constant(value))), state);
                }
                else if (typeof(LambdaExpression).IsAssignableFrom(extractInfo.Type))
                {
                    var lambdaExp = (LambdaExpression)extractInfo.Value;
                    EvalExpression(method, lambdaExp.Body, state);
                    state.ProhibitsLastAutoPop = true;
                }
                else
                {
                    EvalConstant(method, Expression.Constant(extractInfo.Value), state);
                }
            }

            // Adjust the return type.
            var designingExp = default(Expression);
            if (0 < state.CallStack.Count)
            {
                designingExp = state.CallStack.Pop();
            }

            var adjustingConvertExp = default(UnaryExpression);
            if (0 < state.MethodInfoDesignedInfos.Count)
            {
                var mi = state.MethodInfoDesignedInfos.Pop();
                if (designingExp != null && mi.ReturnType != typeof(void) && designingExp.Type != mi.ReturnType)
                {
                    var mockObjectExp = Expression.Convert(((MethodCallExpression)exp).Arguments[0], mi.DeclaringType);
                    var mockParamExps = ToMockParameterExpressions(mi.GetParameters());
                    var mockMethodCallExp = Expression.Call(mockObjectExp, mi, mockParamExps);
                    adjustingConvertExp = Expression.Convert(mockMethodCallExp, designingExp.Type);
                }
            }
            else if (0 < state.ReturneeConstructorInfos.Count)
            {
                var ci = state.ReturneeConstructorInfos.Pop();
                if (designingExp != null && designingExp.Type != ci.DeclaringType)
                {
                    var mockParamExps = ToMockParameterExpressions(ci.GetParameters());
                    var mockNewExp = Expression.New(ci, mockParamExps);
                    adjustingConvertExp = Expression.Convert(mockNewExp, designingExp.Type);
                }
            }
            else if (0 < state.FieldDeclDesignedInfos.Count)
            {
                var fd = state.FieldDeclDesignedInfos.Pop();
                if (designingExp != null && fd.FieldType.Equivalent(designingExp.Type))
                {
                    var mockObjectExp = Expression.Convert(((MethodCallExpression)exp).Arguments[0], fd.DeclaringType.Source);
                    var mockFieldExp = Expression.Field(mockObjectExp, fd.Name);
                    adjustingConvertExp = Expression.Convert(mockFieldExp, designingExp.Type);
                }
            }
            else if (0 < state.ReturneeFieldInfos.Count)
            {
                var fi = state.ReturneeFieldInfos.Pop();
                if (designingExp != null && designingExp.Type != fi.FieldType)
                {
                    var mockObjectExp = Expression.Convert(((MethodCallExpression)exp).Arguments[0], fi.DeclaringType);
                    var mockFieldExp = Expression.Field(mockObjectExp, fi);
                    adjustingConvertExp = Expression.Convert(mockFieldExp, designingExp.Type);
                }
            }
            else if (0 < state.PropertyInfoDesignedInfos.Count)
            {
                var pi = state.PropertyInfoDesignedInfos.Pop();
                if (designingExp != null && designingExp.Type != pi.PropertyType)
                {
                    var mockObjectExp = Expression.Convert(((MethodCallExpression)exp).Arguments[0], pi.DeclaringType);
                    var mockPropertyExp = Expression.Property(mockObjectExp, pi);
                    adjustingConvertExp = Expression.Convert(mockPropertyExp, designingExp.Type);
                }
            }

            if (adjustingConvertExp != null)
            {
                //EvalUnaryWithoutOperandEvaluation(method, adjustingConvertExp, state);
            }
        }

        private IEnumerable<Expression> ToMockParameterExpressions(ParameterInfo[] @params)
        {
            return @params.Select((param, i) => Expression.Parameter(param.ParameterType, i.ToString())).Cast<Expression>();
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

        protected virtual void EvalConditional(IMethodBaseGenerator method, ConditionalExpression exp, EvalState state)
        {
            EvalExpression(method, exp.Test, state);
            method.Body.ILOperator.Emit(OpCodes.Ldc_I4_0);
            method.Body.ILOperator.Emit(OpCodes.Ceq);
            var localDecl = method.Body.ILOperator.AddLocal(typeof(bool));
            method.Body.ILOperator.Emit(OpCodes.Stloc, localDecl);
            method.Body.ILOperator.Emit(OpCodes.Ldloc, localDecl);
            var labelFalse = method.Body.ILOperator.AddLabel();
            method.Body.ILOperator.Emit(OpCodes.Brtrue, labelFalse);

            EvalExpression(method, exp.IfTrue, state);
            var labelEnd = method.Body.ILOperator.AddLabel();
            method.Body.ILOperator.Emit(OpCodes.Br, labelEnd);

            method.Body.ILOperator.SetLabel(labelFalse);

            EvalExpression(method, exp.IfFalse, state);

            method.Body.ILOperator.SetLabel(labelEnd);
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
            EvalUnaryWithoutOperandEvaluation(method, exp, state);
        }

        protected virtual void EvalUnaryWithoutOperandEvaluation(IMethodBaseGenerator method, UnaryExpression exp, EvalState state)
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
                    if (0 < state.ShiftInfos.Count)
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
                    if (0 < state.ShiftInfos.Count)
                    {
                        var shiftInfo = state.ShiftInfos.Pop();
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
                        if (0 < state.ShiftInfos.Count)
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
                        if (0 < state.ShiftInfos.Count)
                        {
                            var shiftInfo = state.ShiftInfos.Pop();
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
                    EvalExpression(method, _exp, state);
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

        protected MethodCallExpression CreateExtractExp_T(Expression constant, Type type)
        {
            return Expression.Call(DslExtract_T.MakeGenericMethod(type), new Expression[] { constant });
        }

        protected class EvalState
        {
            public EvalState()
            {
                IfInfos = new Stack<IfInfo>();
                ReturnInfos = new Stack<ReturnInfo>();
                ExtractInfos = new Stack<ExtractInfo>();
                AllocInfos = new Stack<AllocInfo>();
                ShiftInfos = new Stack<ShiftInfo>();
                CallStack = new Stack<Expression>();
                MethodInfoDesignedInfos = new Stack<MethodInfo>();
                ParametricConstructorInfos = new Dictionary<Expression, ConstructorInfo>();
                ReturneeConstructorInfos = new Stack<ConstructorInfo>();
                FieldDeclDesignedInfos = new Stack<IFieldDeclaration>();
                ParametricFieldInfos = new Dictionary<Expression, FieldInfo>();
                ReturneeFieldInfos = new Stack<FieldInfo>();
                PropertyInfoDesignedInfos = new Stack<PropertyInfo>();
            }

            public bool ProhibitsLastAutoPop { get; set; }
            public Stack<IfInfo> IfInfos { get; private set; }
            public Stack<ReturnInfo> ReturnInfos { get; private set; }
            public Stack<ExtractInfo> ExtractInfos { get; private set; }
            public Stack<AllocInfo> AllocInfos { get; private set; }
            public Stack<ShiftInfo> ShiftInfos { get; private set; }

            public Stack<Expression> CallStack { get; private set; }
            
            public Stack<MethodInfo> MethodInfoDesignedInfos { get; private set; }
            
            public Dictionary<Expression, ConstructorInfo> ParametricConstructorInfos { get; private set; }
            public Stack<ConstructorInfo> ReturneeConstructorInfos { get; private set; }
            
            public Stack<IFieldDeclaration> FieldDeclDesignedInfos { get; private set; }
            
            public Dictionary<Expression, FieldInfo> ParametricFieldInfos { get; private set; }
            public Stack<FieldInfo> ReturneeFieldInfos { get; private set; }
            
            public Stack<PropertyInfo> PropertyInfoDesignedInfos { get; private set; }
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

            EvalTo(() => Dsl.Extract(expression), dummyMethod);

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
