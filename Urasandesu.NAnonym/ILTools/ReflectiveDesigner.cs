/* 
 * File: ReflectiveDesigner.cs
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
using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.Mixins.Urasandesu.NAnonym.ILTools;
using SRE = System.Reflection.Emit;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Urasandesu.NAnonym.ILTools
{
    public class ReflectiveDesigner : ExpressiveDecorator
    {
        public static readonly MethodInfo MethodInfoInvoke = TypeSavable.GetInstanceMethod<MethodInfo, object, object[], object>(_ => _.Invoke);

        public ReflectiveDesigner(ExpressiveGenerator gen)
            : base(new MethodBaseReflectiveDecorator(gen))
        {
        }

        protected override void EvalMethodCall(IMethodBaseGenerator methodGen, MethodCallExpression exp, EvalState state)
        {
            if (exp.Object == null ||
                exp.Object.Type != typeof(MethodInfo) ||
                exp.Object.NodeType != ExpressionType.MemberAccess ||
                ((MemberExpression)exp.Object).Expression.NodeType != ExpressionType.Constant)
            {
                base.EvalMethodCall(methodGen, exp, state);
            }
            else if (exp.Method == MethodInfoInvoke)
            {
                var methodInfo = default(MethodInfo);
                {
                    var extractExp = Expression.Call(
                                        Expression.Constant(ReservedWords),
                                        ReservedWordXInfo1.MakeGenericMethod(typeof(MethodInfo)),
                                        new Expression[] 
                                        { 
                                            exp.Object
                                        }
                                     );
                    base.EvalExtract(methodGen, extractExp, state);
                    if (state.ExtractInfoStack.Count == 0)
                    {
                        throw new NotSupportedException();
                    }
                    var extractInfo = state.ExtractInfoStack.Pop();
                    methodInfo = (MethodInfo)extractInfo.Value;
                }

                if (!methodInfo.IsStatic)
                {
                    base.EvalExpression(methodGen, exp.Arguments[0], state);
                }

                var arguments = new List<Expression>();
                if (exp.Arguments[1].NodeType == ExpressionType.NewArrayInit)
                {
                    base.EvalArguments(methodGen, ((NewArrayExpression)exp.Arguments[1]).Expressions, state); 
                }
                else
                {
                    throw new NotImplementedException();
                }

                if (methodInfo.IsStatic)
                {
                    methodGen.Body.ILOperator.Emit(OpCodes.Call, methodInfo);
                }
                else
                {
                    methodGen.Body.ILOperator.Emit(OpCodes.Callvirt, methodInfo);
                }

                state.ProhibitsLastAutoPop = methodInfo.ReturnType == typeof(void);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        class MethodBaseReflectiveDecorator : ExpressiveMethodBaseDecorator
        {
            readonly ExpressiveMethodBodyDecorator bodyDecorator;
            public MethodBaseReflectiveDecorator(ExpressiveGenerator gen)
                : base(gen)
            {
                bodyDecorator = new MethodBodyReflectiveDecorator(this);
            }

            public override ExpressiveMethodBodyDecorator BodyDecorator
            {
                get { return bodyDecorator; }
            }
        }

        class MethodBodyReflectiveDecorator : ExpressiveMethodBodyDecorator
        {
            readonly ExpressiveILOperationDecorator ilOperationDecorator;
            public MethodBodyReflectiveDecorator(MethodBaseReflectiveDecorator methodDecorator)
                : base(methodDecorator)
            {
                ilOperationDecorator = new ILOperationReflectiveDecorator(this);
            }

            public override ExpressiveILOperationDecorator ILOperationDecorator
            {
                get { return ilOperationDecorator; }
            }
        }

        class ILOperationReflectiveDecorator : ExpressiveILOperationDecorator
        {
            public ILOperationReflectiveDecorator(MethodBodyReflectiveDecorator bodyDecorator)
                : base(bodyDecorator)
            {
            }
        }
    }
}
