/* 
 * File: ExpressionToInlineValue.cs
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
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Urasandesu.NAnonym;
using System.Collections.ObjectModel;
using System.Reflection;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.Formulas
{
    public static class ExpressionToInlineValue
    {
        public static void ConvertTo(this Expression exp, ExpressionToInlineValueState state)
        {
            ConvertExpression(exp, state);
        }

        public static T Convert<T>(this Expression exp)
        {
            var state = new ExpressionToInlineValueState();
            exp.ConvertTo(state);
            return (T)state.Result;
        }

        public static void ConvertExpression(Expression exp, ExpressionToInlineValueState state)
        {
            switch (exp.NodeType)
            {
                case ExpressionType.Add:
                    throw new NotImplementedException();
                case ExpressionType.AddChecked:
                    throw new NotImplementedException();
                case ExpressionType.And:
                    throw new NotImplementedException();
                case ExpressionType.AndAlso:
                    throw new NotImplementedException();
                case ExpressionType.ArrayIndex:
                    throw new NotImplementedException();
                case ExpressionType.ArrayLength:
                    throw new NotImplementedException();
                case ExpressionType.Call:
                    ConvertMethodCall((MethodCallExpression)exp, state);
                    return;
                case ExpressionType.Coalesce:
                    throw new NotImplementedException();
                case ExpressionType.Conditional:
                    throw new NotImplementedException();
                case ExpressionType.Constant:
                    ConvertConstant((ConstantExpression)exp, state);
                    return;
                case ExpressionType.Convert:
                    throw new NotImplementedException();
                case ExpressionType.ConvertChecked:
                    throw new NotImplementedException();
                case ExpressionType.Divide:
                    throw new NotImplementedException();
                case ExpressionType.Equal:
                    throw new NotImplementedException();
                case ExpressionType.ExclusiveOr:
                    throw new NotImplementedException();
                case ExpressionType.GreaterThan:
                    throw new NotImplementedException();
                case ExpressionType.GreaterThanOrEqual:
                    throw new NotImplementedException();
                case ExpressionType.Invoke:
                    throw new NotImplementedException();
                case ExpressionType.Lambda:
                    throw new NotImplementedException();
                case ExpressionType.LeftShift:
                    throw new NotImplementedException();
                case ExpressionType.LessThan:
                    throw new NotImplementedException();
                case ExpressionType.LessThanOrEqual:
                    throw new NotImplementedException();
                case ExpressionType.ListInit:
                    throw new NotImplementedException();
                case ExpressionType.MemberAccess:
                    ConvertMember((MemberExpression)exp, state);
                    return;
                case ExpressionType.MemberInit:
                    throw new NotImplementedException();
                case ExpressionType.Modulo:
                    throw new NotImplementedException();
                case ExpressionType.Multiply:
                    throw new NotImplementedException();
                case ExpressionType.MultiplyChecked:
                    throw new NotImplementedException();
                case ExpressionType.Negate:
                    throw new NotImplementedException();
                case ExpressionType.NegateChecked:
                    throw new NotImplementedException();
                case ExpressionType.New:
                    throw new NotImplementedException();
                case ExpressionType.NewArrayBounds:
                    throw new NotImplementedException();
                case ExpressionType.NewArrayInit:
                    throw new NotImplementedException();
                case ExpressionType.Not:
                    throw new NotImplementedException();
                case ExpressionType.NotEqual:
                    throw new NotImplementedException();
                case ExpressionType.Or:
                    throw new NotImplementedException();
                case ExpressionType.OrElse:
                    throw new NotImplementedException();
                case ExpressionType.Parameter:
                    throw new NotImplementedException();
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
                case ExpressionType.TypeAs:
                    throw new NotImplementedException();
                case ExpressionType.TypeIs:
                    throw new NotImplementedException();
                case ExpressionType.UnaryPlus:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        public static void ConvertMethodCall(MethodCallExpression exp, ExpressionToInlineValueState state)
        {
            var parameter = new object[] { };
            if (exp.Arguments != null && 0 < exp.Arguments.Count)
            {
                ConvertArguments(exp.Arguments, state);
                parameter = new object[state.Arguments.Count];
                state.Arguments.MoveTo(parameter);
            }

            if (exp.Object == null)
            {
                state.Result = exp.Method.Invoke(null, parameter);
            }
            else
            {
                ConvertExpression(exp.Object, state);
                var instance = state.Result;
                state.Result = exp.Method.Invoke(instance, parameter);
            }
        }

        public static void ConvertArguments(ReadOnlyCollection<Expression> exps, ExpressionToInlineValueState state)
        {
            var arguments = new List<object>();
            foreach (var exp in exps)
            {
                ConvertExpression(exp, state);
                arguments.Add(state.Result);
            }
            arguments.AddRangeTo(state.Arguments);
        }

        public static void ConvertConstant(ConstantExpression exp, ExpressionToInlineValueState state)
        {
            state.Result = exp.Value;
        }

        public static void ConvertMember(MemberExpression exp, ExpressionToInlineValueState state)
        {
            var fieldInfo = default(FieldInfo);
            var propertyInfo = default(PropertyInfo);
            if ((fieldInfo = exp.Member as FieldInfo) != null)
            {
                if (exp.Expression == null)
                {
                    state.Result = fieldInfo.GetValue(null);
                    return;
                }
                else
                {
                    ConvertExpression(exp.Expression, state);
                    var instance = state.Result;
                    state.Result = fieldInfo.GetValue(instance);
                    return;
                }
            }
            else if ((propertyInfo = exp.Member as PropertyInfo) != null)
            {
                if (exp.Expression == null)
                {
                    state.Result = propertyInfo.GetValue(null, null);
                    return;
                }
                else
                {
                    ConvertExpression(exp.Expression, state);
                    var instance = state.Result;
                    state.Result = propertyInfo.GetValue(instance, null);
                    return;
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}

