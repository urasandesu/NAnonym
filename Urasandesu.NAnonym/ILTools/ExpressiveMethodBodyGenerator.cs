/* 
 * File: ExpressiveMethodBodyGenerator.cs
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
using System.Collections.ObjectModel;
using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools.Impl.System.Reflection;
using SR = System.Reflection;
using SRE = System.Reflection.Emit;
using Urasandesu.NAnonym.Linq;
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.NAnonym.Mixins.System.Reflection;
using System.Collections.Generic;
using Urasandesu.NAnonym.Mixins.Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.ILTools
{
    public class ExpressiveMethodBodyGenerator : IMethodBodyGenerator
    {
        static readonly Expressible expressible = new Expressible();
        static readonly StoreExpressible storeExpressible = new StoreExpressible();
        static readonly MethodInfo GetTypeFromHandle = 
            typeof(Type).GetMethod(
                                "GetTypeFromHandle",
                                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                                null,
                                new Type[] 
                                {
                                    typeof(RuntimeTypeHandle) 
                                },
                                null);
        static readonly MethodInfo GetMethodFromHandle =
            typeof(MethodBase).GetMethod(
                                "GetMethodFromHandle",
                                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                                null,
                                new Type[]
                                {
                                    typeof(RuntimeMethodHandle)
                                },
                                null);
        
        readonly IMethodBaseGenerator methodGen;
        readonly EvalState state;

        internal ExpressiveMethodBodyGenerator(IMethodBaseGenerator methodGen)
        {
            this.methodGen = methodGen;
            state = new EvalState();
        }

        public void Eval(Expression<Action<Expressible>> exp)
        {
            EvalExpression(methodGen, exp.Body, state);
            EvalExit(methodGen, exp.Body, state);
        }

        public static void EvalTo(Expression<Action<Expressible>> exp, IMethodBaseGenerator methodGen)
        {
            var state = new EvalState();
            EvalExpression(methodGen, exp.Body, state);
            EvalExit(methodGen, exp.Body, state);
        }

        static void EvalExit(IMethodBaseGenerator methodGen, Expression exp, EvalState state)
        {
            if (0 < state.ExtractInfoStack.Count)
            {
                var extractInfo = state.ExtractInfoStack.Pop();
                EvalExtracted(methodGen, extractInfo, state);
            }

            if (exp.Type != typeof(void) && !state.ProhibitsLastAutoPop)
            {
                // NOTE: void ではないということは評価スタックに情報が残っているということ。
                //       pop するのは、基本的に 1 回の Emit(Expression<Action<ExpressiveILProcessor>>) で完結するようにしたいため。
                methodGen.Body.ILOperator.Emit(OpCodes.Pop);
            }
        }

        static void EvalArguments(IMethodBaseGenerator methodGen, ReadOnlyCollection<Expression> exps, EvalState state)
        {
            foreach (var exp in exps)
            {
                EvalExpression(methodGen, exp, state);
                if (0 < state.ExtractInfoStack.Count)
                {
                    var extractInfo = state.ExtractInfoStack.Pop();
                    EvalExtracted(methodGen, extractInfo, state);
                }
            }
        }

        static void EvalExpression(IMethodBaseGenerator methodGen, Expression exp, EvalState state)
        {
            state.ProhibitsLastAutoPop = false;
            if (exp == null) return;

            switch (exp.NodeType)
            {
                case ExpressionType.Add:
                case ExpressionType.ArrayIndex:
                case ExpressionType.Multiply:
                case ExpressionType.Equal:
                    EvalBinary(methodGen, (BinaryExpression)exp, state);
                    return;
                case ExpressionType.AddChecked:
                    break;
                case ExpressionType.And:
                    break;
                case ExpressionType.AndAlso:
                    break;
                case ExpressionType.Call:
                    EvalMethodCall(methodGen, (MethodCallExpression)exp, state);
                    return;
                case ExpressionType.Coalesce:
                    break;
                case ExpressionType.Conditional:
                    break;
                case ExpressionType.Constant:
                    EvalConstant(methodGen, (ConstantExpression)exp, state);
                    return;
                case ExpressionType.ArrayLength:
                case ExpressionType.Convert:
                    EvalUnary(methodGen, (UnaryExpression)exp, state);
                    return;
                case ExpressionType.ConvertChecked:
                    break;
                case ExpressionType.Divide:
                    break;
                case ExpressionType.ExclusiveOr:
                    break;
                case ExpressionType.GreaterThan:
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    break;
                case ExpressionType.Invoke:
                    EvalInvoke(methodGen, (InvocationExpression)exp, state);
                    return;
                case ExpressionType.Lambda:
                    EvalLambda(methodGen, (LambdaExpression)exp, state);
                    return;
                case ExpressionType.LeftShift:
                    break;
                case ExpressionType.LessThan:
                    break;
                case ExpressionType.LessThanOrEqual:
                    break;
                case ExpressionType.ListInit:
                    break;
                case ExpressionType.MemberAccess:
                    EvalMember(methodGen, (MemberExpression)exp, state);
                    return;
                case ExpressionType.MemberInit:
                    break;
                case ExpressionType.Modulo:
                    break;
                case ExpressionType.MultiplyChecked:
                    break;
                case ExpressionType.Negate:
                    break;
                case ExpressionType.NegateChecked:
                    break;
                case ExpressionType.New:
                    EvalNew(methodGen, (NewExpression)exp, state);
                    return;
                case ExpressionType.NewArrayBounds:
                    break;
                case ExpressionType.NewArrayInit:
                    EvalNewArray(methodGen, (NewArrayExpression)exp, state);
                    return;
                case ExpressionType.Not:
                    break;
                case ExpressionType.NotEqual:
                    break;
                case ExpressionType.Or:
                    break;
                case ExpressionType.OrElse:
                    break;
                case ExpressionType.Parameter:
                    EvalParameter(methodGen, (ParameterExpression)exp, state);
                    return;
                case ExpressionType.Power:
                    break;
                case ExpressionType.Quote:
                    break;
                case ExpressionType.RightShift:
                    break;
                case ExpressionType.Subtract:
                    break;
                case ExpressionType.SubtractChecked:
                    break;
                case ExpressionType.TypeAs:
                    break;
                case ExpressionType.TypeIs:
                    break;
                case ExpressionType.UnaryPlus:
                    break;
                default:
                    break;
            }
            throw new NotImplementedException();
        }

        static void EvalBinary(IMethodBaseGenerator methodGen, BinaryExpression exp, EvalState state)
        {
            EvalExpression(methodGen, exp.Left, state);
            EvalExpression(methodGen, exp.Right, state);

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
                    methodGen.Body.ILOperator.Emit(OpCodes.Add);
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
                    methodGen.Body.ILOperator.Emit(OpCodes.Mul);
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
                    methodGen.Body.ILOperator.Emit(OpCodes.Ldelem_Ref);
                }
            }
            else if (exp.NodeType == ExpressionType.Equal)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ceq);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        static void EvalMethodCall(IMethodBaseGenerator methodGen, MethodCallExpression exp, EvalState state)
        {
            // 評価の順番は、Object -> Arguments -> Method。
            if (exp.Object == null)
            {
                // static method call
                EvalArguments(methodGen, exp.Arguments, state);
                methodGen.Body.ILOperator.Emit(OpCodes.Call, exp.Method);
            }
            else
            {
                if (exp.Object.Type == typeof(Expressible))
                {
                    if (expressible.IsBase(exp.Method)) EvalBase(methodGen, exp, state);
                    else if (expressible.IsThis(exp.Method)) EvalThis(methodGen, exp, state);
                    else if (expressible.IsDupAddOne(exp.Method)) EvalDupAddOne(methodGen, exp, state);
                    else if (expressible.IsAddOneDup(exp.Method)) EvalAddOneDup(methodGen, exp, state);
                    else if (expressible.IsSubOneDup(exp.Method)) EvalSubOneDup(methodGen, exp, state);
                    else if (expressible.IsNew(exp.Method)) EvalNew(methodGen, exp, state);
                    else if (expressible.IsInvoke(exp.Method)) EvalInvoke(methodGen, exp, state);
                    else if (expressible.IsFtn(exp.Method)) EvalFtn(methodGen, exp, state);
                    else if (expressible.IsIf(exp.Method)) EvalIf(methodGen, exp, state);
                    else if (expressible.IsEndIf(exp.Method)) EvalEndIf(methodGen, exp, state);
                    else if (expressible.IsReturn(exp.Method)) EvalReturn(methodGen, exp, state);
                    else if (expressible.IsEnd(exp.Method)) EvalEnd(methodGen, exp, state);
                    else if (expressible.IsLd(exp.Method)) EvalLd(methodGen, exp, state);
                    else if (expressible.IsSt(exp.Method)) EvalSt(methodGen, exp, state);
                    else if (expressible.IsX(exp.Method)) EvalExtract(methodGen, exp, state);
                    else if (expressible.IsCm(exp.Method)) EvalCm(methodGen, exp, state);
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else if (exp.Object.Type == typeof(StoreExpressible) || 
                         exp.Object.Type.EquivalentWithoutGenericArguments(typeof(StoreExpressible<>)))
                {
                    if (storeExpressible.IsAs(exp.Method)) EvalStoreAs(methodGen, exp, state);
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    // instance method call
                    EvalExpression(methodGen, exp.Object, state);
                    if (0 < state.ExtractInfoStack.Count)
                    {
                        var extractInfo = state.ExtractInfoStack.Pop();
                        EvalExtracted(methodGen, extractInfo, state);
                    }
                    if (exp.Object.Type.IsValueType)
                    {
                        // NOTE: 値型のメソッドを呼び出すには、アドレスへの変換が必要。
                        var local = methodGen.Body.ILOperator.AddLocal(exp.Object.Type);
                        methodGen.Body.ILOperator.Emit(OpCodes.Stloc, local);
                        methodGen.Body.ILOperator.Emit(OpCodes.Ldloca, local);
                    }

                    EvalArguments(methodGen, exp.Arguments, state);

                    methodGen.Body.ILOperator.Emit(OpCodes.Callvirt, exp.Method);
                }
            }
        }

        static void EvalBase(IMethodBaseGenerator methodGen, MethodCallExpression exp, EvalState state)
        {
            methodGen.Body.ILOperator.Emit(OpCodes.Ldarg_0);
            var constructorDecl = methodGen.DeclaringType.BaseType.GetConstructor(new Type[] { });
            methodGen.Body.ILOperator.Emit(OpCodes.Call, constructorDecl);
        }

        static void EvalThis(IMethodBaseGenerator methodGen, MethodCallExpression exp, EvalState state)
        {
            methodGen.Body.ILOperator.Emit(OpCodes.Ldarg_0);
        }

        static void EvalDupAddOne(IMethodBaseGenerator methodGen, MethodCallExpression exp, EvalState state)
        {
            var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
            var localDecl = methodGen.Body.Locals.First(_localDecl => _localDecl.Name == fieldInfo.Name);
            methodGen.Body.ILOperator.Emit(OpCodes.Ldloc, localDecl);
            methodGen.Body.ILOperator.Emit(OpCodes.Dup);
            methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4_1);
            methodGen.Body.ILOperator.Emit(OpCodes.Add);
            methodGen.Body.ILOperator.Emit(OpCodes.Stloc, localDecl);
        }

        static void EvalAddOneDup(IMethodBaseGenerator methodGen, MethodCallExpression exp, EvalState state)
        {
            var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
            var localDecl = methodGen.Body.Locals.First(_localDecl => _localDecl.Name == fieldInfo.Name);
            methodGen.Body.ILOperator.Emit(OpCodes.Ldloc, localDecl);
            methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4_1);
            methodGen.Body.ILOperator.Emit(OpCodes.Add);
            methodGen.Body.ILOperator.Emit(OpCodes.Dup);
            methodGen.Body.ILOperator.Emit(OpCodes.Stloc, localDecl);
        }

        static void EvalSubOneDup(IMethodBaseGenerator methodGen, MethodCallExpression exp, EvalState state)
        {
            var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
            var localDecl = methodGen.Body.Locals.First(_localDecl => _localDecl.Name == fieldInfo.Name);
            methodGen.Body.ILOperator.Emit(OpCodes.Ldloc, localDecl);
            methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4_1);
            methodGen.Body.ILOperator.Emit(OpCodes.Sub);
            methodGen.Body.ILOperator.Emit(OpCodes.Dup);
            methodGen.Body.ILOperator.Emit(OpCodes.Stloc, localDecl);
        }

        static void EvalNew(IMethodBaseGenerator methodGen, MethodCallExpression exp, EvalState state)
        {
            EvalExpression(methodGen, exp.Arguments[1], state);
            if (0 < state.ExtractInfoStack.Count)
            {
                throw new NotImplementedException();
            }
            EvalExpression(methodGen, exp.Arguments[0], state);
            if (0 < state.ExtractInfoStack.Count)
            {
                var extractInfo = state.ExtractInfoStack.Pop();
                var constructor = (ConstructorInfo)extractInfo.Value;
                methodGen.Body.ILOperator.Emit(OpCodes.Newobj, constructor);
            }
        }

        static void EvalInvoke(IMethodBaseGenerator methodGen, MethodCallExpression exp, EvalState state)
        {
            EvalExpression(methodGen, exp.Arguments[0], state);
            if (0 < state.ExtractInfoStack.Count)
            {
                throw new NotImplementedException();
            }
            EvalExpression(methodGen, exp.Arguments[2], state);
            if (0 < state.ExtractInfoStack.Count)
            {
                throw new NotImplementedException();
            }
            EvalExpression(methodGen, exp.Arguments[1], state);
            if (0 < state.ExtractInfoStack.Count)
            {
                var extractInfo = state.ExtractInfoStack.Pop();
                var method = (MethodInfo)extractInfo.Value;
                methodGen.Body.ILOperator.Emit(OpCodes.Callvirt, method);
            }
        }

        static void EvalFtn(IMethodBaseGenerator methodGen, MethodCallExpression exp, EvalState state)
        {
            if (1 < exp.Arguments.Count)
            {
                EvalExpression(methodGen, exp.Arguments[0], state);
                if (0 < state.ExtractInfoStack.Count)
                {
                    throw new NotImplementedException();
                }
            }

            EvalExpression(methodGen, exp.Arguments[exp.Arguments.Count - 1], state);
            if (0 < state.ExtractInfoStack.Count)
            {
                var extractInfo = state.ExtractInfoStack.Pop();
                var methodDecl = (IMethodDeclaration)extractInfo.Value;
                methodGen.Body.ILOperator.Emit(OpCodes.Ldftn, methodDecl);
            }
        }

        static void EvalIf(IMethodBaseGenerator methodGen, MethodCallExpression exp, EvalState state)
        {
            EvalExpression(methodGen, exp.Arguments[0], state);
            methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4_0);
            methodGen.Body.ILOperator.Emit(OpCodes.Ceq);
            var localDecl = methodGen.Body.ILOperator.AddLocal(typeof(bool));
            methodGen.Body.ILOperator.Emit(OpCodes.Stloc, localDecl);
            methodGen.Body.ILOperator.Emit(OpCodes.Ldloc, localDecl);
            var labelDecl = methodGen.Body.ILOperator.AddLabel();
            state.IfInfoStack.Push(new IfInfo(labelDecl));
            methodGen.Body.ILOperator.Emit(OpCodes.Brtrue, labelDecl);
        }

        static void EvalEndIf(IMethodBaseGenerator methodGen, MethodCallExpression exp, EvalState state)
        {
            var ifInfo = state.IfInfoStack.Pop();
            methodGen.Body.ILOperator.SetLabel(ifInfo.LabelDecl);
        }

        static void EvalReturn(IMethodBaseGenerator methodGen, MethodCallExpression exp, EvalState state)
        {
            EvalExpression(methodGen, exp.Arguments[0], state);
            methodGen.Body.ILOperator.Emit(OpCodes.Ret);
        }

        static void EvalEnd(IMethodBaseGenerator methodGen, MethodCallExpression exp, EvalState state)
        {
            methodGen.Body.ILOperator.Emit(OpCodes.Ret);
        }

        static void EvalLd(IMethodBaseGenerator methodGen, MethodCallExpression exp, EvalState state)
        {
            EvalExpression(methodGen, exp.Arguments[0], state);
            if (0 < state.ExtractInfoStack.Count)
            {
                var extractInfo = state.ExtractInfoStack.Pop();
                if (extractInfo.Type.IsArray)
                {
                    ((Array)extractInfo.Value).OfType<string>().
                    ForEach(name =>
                    {
                        var fieldInfo = new ExpressibleFieldInfo(name, typeof(object));
                        EvalMember(methodGen, Expression.Field(null, fieldInfo), state);
                    });
                }
                else
                {
                    var fieldInfo = new ExpressibleFieldInfo((string)extractInfo.Value, extractInfo.Type);
                    EvalMember(methodGen, Expression.Field(null, fieldInfo), state);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        static void EvalSt(IMethodBaseGenerator methodGen, MethodCallExpression exp, EvalState state)
        {
            if (exp.Arguments[0].NodeType == ExpressionType.MemberAccess)
            {
                var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
                state.StInfoStack.Push(new StInfo(fieldInfo.Name, fieldInfo.FieldType));
            }
            else
            {
                EvalExpression(methodGen, exp.Arguments[0], state);
                if (0 < state.ExtractInfoStack.Count)
                {
                    var extractInfo = state.ExtractInfoStack.Pop();
                    var name = (string)extractInfo.Value;
                    state.StInfoStack.Push(new StInfo(name, extractInfo.Type));
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        static void EvalStoreAs(IMethodBaseGenerator methodGen, MethodCallExpression exp, EvalState state)
        {
            EvalExpression(methodGen, exp.Object, state);
            if (0 < state.StInfoStack.Count)
            {
                var stInfo = state.StInfoStack.Pop();

                var localGen = default(ILocalGenerator);
                var parameterGen = default(IParameterGenerator);
                var fieldGen = default(IFieldGenerator);
                if ((localGen = methodGen.Body.Locals.FirstOrDefault(_localGen => _localGen.Name == stInfo.Name)) != null)
                {
                    EvalExpression(methodGen, exp.Arguments[0], state);
                    methodGen.Body.ILOperator.Emit(OpCodes.Stloc, localGen);
                }
                else if ((parameterGen = methodGen.Parameters.FirstOrDefault(_parameterGen => _parameterGen.Name == stInfo.Name)) != null)
                {
                    EvalExpression(methodGen, exp.Arguments[0], state);
                    methodGen.Body.ILOperator.Emit(OpCodes.Starg, parameterGen);
                }
                else if ((fieldGen = methodGen.DeclaringType.Fields.FirstOrDefault(_fieldGen => _fieldGen.Name == stInfo.Name)) != null)
                {
                    methodGen.Body.ILOperator.Emit(OpCodes.Ldarg_0);
                    EvalExpression(methodGen, exp.Arguments[0], state);
                    methodGen.Body.ILOperator.Emit(OpCodes.Stfld, fieldGen);
                }
                else
                {
                    EvalExpression(methodGen, exp.Arguments[0], state);
                    var local = methodGen.Body.ILOperator.AddLocal(stInfo.Name, stInfo.Type);
                    methodGen.Body.ILOperator.Emit(OpCodes.Stloc, local);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            state.ProhibitsLastAutoPop = true;
        }

        static void EvalExtract(IMethodBaseGenerator methodGen, MethodCallExpression exp, EvalState state)
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

        static void EvalExtracted(IMethodBaseGenerator methodGen, ExtractInfo extractInfo, EvalState state)
        {
            if (extractInfo.Type.IsArray)
            {
                EvalNewArray(methodGen, Expression.NewArrayInit(extractInfo.Type.GetElementType(),
                    ((Array)extractInfo.Value).Cast<object>().Select(value => (Expression)Expression.Constant(value))), state);
            }
            else if (typeof(LambdaExpression).IsAssignableFrom(extractInfo.Type))
            {
                EvalExpression(methodGen, ((LambdaExpression)extractInfo.Value).Body, state);
                if (0 < state.ExtractInfoStack.Count)
                {
                    extractInfo = state.ExtractInfoStack.Pop();
                    EvalExtracted(methodGen, extractInfo, state);
                }
                state.ProhibitsLastAutoPop = true;
            }
            else
            {
                EvalConstant(methodGen, Expression.Constant(extractInfo.Value), state);
            }
        }

        static void EvalCm(IMethodBaseGenerator methodGen, MethodCallExpression exp, EvalState state)
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

            EvalMember(methodGen, Expression.Field(null, targetFieldInfo), state);
        }

        static void EvalConstant(IMethodBaseGenerator methodGen, ConstantExpression exp, EvalState state)
        {
            string s = default(string);
            int? ni = default(int?);
            double? nd = default(double?);
            char? nc = default(char?);
            sbyte? nsb = default(sbyte?);
            bool? nb = default(bool?);
            var e = default(Enum);
            var t = default(Type);
            var mi = default(MethodInfo);
            var ci = default(ConstructorInfo);
            if (exp.Value == null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldnull);
            }
            else if ((s = exp.Value as string) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldstr, s);
            }
            else if ((ni = exp.Value as int?) != null)
            {
                // HACK: _S が付く命令は短い形式。-127 ～ 128 以外は最適化が必要。
                methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4_S, (sbyte)(int)ni);
            }
            else if ((nd = exp.Value as double?) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldc_R8, (double)nd);
            }
            else if ((nc = exp.Value as char?) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4_S, (sbyte)(char)nc);
            }
            else if ((nsb = exp.Value as sbyte?) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4_S, (sbyte)nsb);
            }
            else if ((nb = exp.Value as bool?) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4, (bool)nb ? 1 : 0);
            }
            else if ((e = exp.Value as Enum) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4, e.GetHashCode());
            }
            else if ((t = exp.Value as Type) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldtoken, t);
                methodGen.Body.ILOperator.Emit(OpCodes.Call, GetTypeFromHandle);
            }
            else if ((mi = exp.Value as MethodInfo) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldtoken, mi);
                methodGen.Body.ILOperator.Emit(OpCodes.Call, GetMethodFromHandle);
                methodGen.Body.ILOperator.Emit(OpCodes.Castclass, typeof(MethodInfo));
            }
            else if ((ci = exp.Value as ConstructorInfo) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldtoken, ci);
                methodGen.Body.ILOperator.Emit(OpCodes.Call, GetMethodFromHandle);
                methodGen.Body.ILOperator.Emit(OpCodes.Castclass, typeof(ConstructorInfo));
            }
            else
            {
                // TODO: exp.Value がオブジェクト型の場合はどうするのだ？
                throw new NotImplementedException();
            }
        }

        static void EvalUnary(IMethodBaseGenerator methodGen, UnaryExpression exp, EvalState state)
        {
            EvalExpression(methodGen, exp.Operand, state);
            switch (exp.NodeType)
            {
                case ExpressionType.Convert:
                    if (exp.Type.IsSubclassOf(typeof(ValueType)))
                    {
                        if (exp.Type == typeof(int))
                        {
                            methodGen.Body.ILOperator.Emit(OpCodes.Conv_I4);
                        }
                        else if (exp.Type == typeof(sbyte))
                        {
                            methodGen.Body.ILOperator.Emit(OpCodes.Conv_I2);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                    else if (exp.Operand.Type.IsSubclassOf(typeof(ValueType)) && !exp.Type.IsSubclassOf(typeof(ValueType)))
                    {
                        // TODO: implicit operator や explicit operator の実装。
                        methodGen.Body.ILOperator.Emit(OpCodes.Box, exp.Operand.Type);
                    }
                    else
                    {
                        // TODO: implicit operator や explicit operator の実装。
                        methodGen.Body.ILOperator.Emit(OpCodes.Castclass, exp.Type);
                    }
                    break;
                case ExpressionType.ArrayLength:
                    methodGen.Body.ILOperator.Emit(OpCodes.Ldlen);
                    methodGen.Body.ILOperator.Emit(OpCodes.Conv_I4);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        static void EvalInvoke(IMethodBaseGenerator methodGen, InvocationExpression exp, EvalState state)
        {
            EvalExpression(methodGen, exp.Expression, state);
            EvalArguments(methodGen, exp.Arguments, state);
            if (exp.Expression.Type.IsSubclassOf(typeof(Delegate)))
            {
                var invokeMethod = exp.Expression.Type.GetMethod("Invoke", BindingFlags.Public | BindingFlags.Instance);
                methodGen.Body.ILOperator.Emit(OpCodes.Callvirt, invokeMethod);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        static void EvalLambda(IMethodBaseGenerator methodGen, LambdaExpression exp, EvalState state)
        {
            // TODO: DynamicMethod 再帰して作ることになる。手間なので後回し。
            EvalExpression(methodGen, exp.Body, state);
        }

        static void EvalMember(IMethodBaseGenerator methodGen, MemberExpression exp, EvalState state)
        {
            var fieldInfo = default(FieldInfo);
            var propertyInfo = default(PropertyInfo);
            if ((fieldInfo = exp.Member as FieldInfo) != null)
            {
                // 評価の優先順は、
                // 1. Addloc でローカル変数として追加したの（名前引き）
                // 2. メソッドの引数（名前引き）
                // 3. 定義時に使った変数そのもの
                // 4. その他
                var localGen = default(ILocalGenerator);
                var parameterGen = default(IParameterGenerator);
                var fieldGen = default(IFieldGenerator);
                var portable = default(ConstantExpression);
                if ((localGen = methodGen.Body.Locals.FirstOrDefault(_localGen => _localGen.Name == fieldInfo.Name)) != null)
                {
                    methodGen.Body.ILOperator.Emit(OpCodes.Ldloc, localGen);
                }
                else if ((parameterGen = methodGen.Parameters.FirstOrDefault(_parameterGen => _parameterGen.Name == fieldInfo.Name)) != null)
                {
                    methodGen.Body.ILOperator.Emit(OpCodes.Ldarg, parameterGen);
                }
                else if ((fieldGen = methodGen.DeclaringType.Fields.FirstOrDefault(_fieldGen => _fieldGen.Name == fieldInfo.Name)) != null)
                {
                    methodGen.Body.ILOperator.Emit(OpCodes.Ldarg_0);
                    methodGen.Body.ILOperator.Emit(OpCodes.Ldfld, fieldGen);
                }
                else if ((portable = exp.Expression as ConstantExpression) != null)
                {
                    // NOTE: 同じ名前の変数を Addloc されるとやっかい。擬似的にローカル変数としても定義することを検討中。
                    var item = methodGen.AddPortableScopeItem(fieldInfo);
                    methodGen.Body.ILOperator.Emit(OpCodes.Ldarg_0);
                    methodGen.Body.ILOperator.Emit(OpCodes.Ldfld, item);
                }
                else
                {
                    EvalExpression(methodGen, exp.Expression, state);

                    if (fieldInfo.IsStatic)
                    {
                        methodGen.Body.ILOperator.Emit(OpCodes.Ldsfld, fieldInfo);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
            else if ((propertyInfo = exp.Member as PropertyInfo) != null)
            {
                EvalExpression(methodGen, exp.Expression, state);

                if (propertyInfo.IsStatic())
                {
                    throw new NotImplementedException();
                }
                else
                {
                    methodGen.Body.ILOperator.Emit(OpCodes.Callvirt, propertyInfo.GetGetMethod());
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        static void EvalNew(IMethodBaseGenerator methodGen, NewExpression exp, EvalState state)
        {
            EvalArguments(methodGen, exp.Arguments, state);
            methodGen.Body.ILOperator.Emit(OpCodes.Newobj, exp.Constructor);
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

        static void EvalNewArray(IMethodBaseGenerator methodGen, NewArrayExpression exp, EvalState state)
        {
            if (exp.NodeType == ExpressionType.NewArrayInit)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4_S, (sbyte)exp.Expressions.Count);
                methodGen.Body.ILOperator.Emit(OpCodes.Newarr, exp.Type.GetElementType());
                var localDecl = methodGen.Body.ILOperator.AddLocal(exp.Type);
                methodGen.Body.ILOperator.Emit(OpCodes.Stloc, localDecl);
                exp.Expressions
                    .ForEach((_exp, index) =>
                    {
                        methodGen.Body.ILOperator.Emit(OpCodes.Ldloc, localDecl);
                        methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4, index);
                        EvalExpression(methodGen, _exp, state);
                        if (0 < state.ExtractInfoStack.Count)
                        {
                            var extractInfo = state.ExtractInfoStack.Pop();
                            EvalExtracted(methodGen, extractInfo, state);
                        }
                        if (typeof(double).IsAssignableFrom(_exp.Type))
                        {
                            throw new NotImplementedException();
                        }
                        else
                        {
                            methodGen.Body.ILOperator.Emit(OpCodes.Stelem_Ref);
                        }
                    });
                methodGen.Body.ILOperator.Emit(OpCodes.Ldloc, localDecl);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        static void EvalParameter(IMethodBaseGenerator methodGen, ParameterExpression exp, EvalState state)
        {
            if (exp.Type == typeof(ExpressiveMethodBodyGenerator))
            {
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        #region IMethodBodyGenerator メンバ

        public IILOperator ILOperator { get { return methodGen.Body.ILOperator; } }

        public ReadOnlyCollection<ILocalGenerator> Locals
        {
            get { throw new NotImplementedException(); }
        }

        public ReadOnlyCollection<IDirectiveGenerator> Directives
        {
            get { return methodGen.Body.Directives; }
        }

        #endregion

        #region IMethodBodyDeclaration メンバ

        ReadOnlyCollection<ILocalDeclaration> IMethodBodyDeclaration.Locals
        {
            get { throw new NotImplementedException(); }
        }

        ReadOnlyCollection<IDirectiveDeclaration> IMethodBodyDeclaration.Directives
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        public void Dump()
        {
            for (int directivesIndex = 0; directivesIndex < Directives.Count; directivesIndex++)
            {
                Console.WriteLine(string.Format("L_{0:X4}: {1}", directivesIndex, Directives[directivesIndex]));
            }
        }

        internal class EvalState
        {
            public EvalState()
            {
                IfInfoStack = new Stack<IfInfo>();
                ExtractInfoStack = new Stack<ExtractInfo>();
                StInfoStack = new Stack<StInfo>();
            }

            public bool ProhibitsLastAutoPop { get; set; }
            public Stack<IfInfo> IfInfoStack { get; private set; }
            public Stack<ExtractInfo> ExtractInfoStack { get; private set; }
            public Stack<StInfo> StInfoStack { get; private set; }
        }

        internal class IfInfo
        {
            public IfInfo()
            {
            }

            public IfInfo(ILabelDeclaration labelDecl)
            {
                LabelDecl = labelDecl;
            }

            public ILabelDeclaration LabelDecl { get; private set; }
        }

        internal class ExtractInfo
        {
            public ExtractInfo()
            {
            }

            public ExtractInfo(Type type, object value)
            {
                Type = type;
                Value = value;
            }

            public Type Type { get; private set; }
            public object Value { get; private set; }
        }

        internal class StInfo
        {
            public StInfo()
            {
            }

            public StInfo(string name, Type type)
            {
                Name = name;
                Type = type;
            }

            public string Name { get; private set; }
            public Type Type { get; private set; }
        }

        internal class ExpressibleFieldInfo : FieldInfo
        {
            string name;
            Type fieldType;
            public ExpressibleFieldInfo(string name, Type fieldType)
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

        public ReadOnlyCollection<IDirectiveGenerator> ToDirectives(LambdaExpression expression)
        {
            var dummyAssemblyName = new AssemblyName("Dummy");
            var dummyAssembly = methodGen.DeclaringType.Module.Assembly.CreateInstance(dummyAssemblyName);
            var dummyModule = dummyAssembly.AddModule("Dummy");
            var dummyType = dummyModule.AddType("Dummy.Dummy", TypeAttributes.Public, typeof(object));
            var dummyMethod = dummyType.AddMethod("Dummy", MethodAttributes.Public | MethodAttributes.Static, typeof(void), Type.EmptyTypes);

            EvalTo(_ => _.X(expression), dummyMethod);

            return dummyMethod.Body.Directives;
        }

        #region IMethodBodyGenerator メンバ


        public ILocalGenerator AddLocal(string name, Type localType)
        {
            throw new NotImplementedException();
        }

        public ILocalGenerator AddLocal(Type localType)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IMethodBodyGenerator メンバ


        public ILocalGenerator AddLocal(ILocalGenerator localGen)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
