/* 
 * File: ReflectiveILEmitter.cs
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
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{
    public class ReflectiveILEmitter : ReflectiveDesignerDecorator
    {
        //IMethodReservedWords reservedWords = new ILReservedWords();
        //IILAllocReservedWords allocReservedWords = new ILAllocReservedWords();

        public ReflectiveILEmitter(ReflectiveMethodDesigner2 gen, string ilName)
            : base(new MethodBaseEmitterDecorator(gen, ilName))
        {
        }

        //protected override IMethodReservedWords ReservedWords
        //{
        //    get
        //    {
        //        return reservedWords;
        //    }
        //}

        //protected override IMethodAllocReservedWords AllocReservedWords
        //{
        //    get
        //    {
        //        return allocReservedWords;
        //    }
        //}

        public void Emit(Expression<Action> exp)
        {
            Eval(Method, exp.Body, state);
        }

        protected override void EvalMethodCall(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            if (exp.Object == null)
            {
                if (exp.Method.DeclaringType.IsDefined(typeof(ILReservedWordsAttribute), false))
                {
                    if (exp.Method.IsDefined(typeof(ILReservedWordLdAttribute), false)) EvalEmitLd(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(ILReservedWordStAttribute), false)) EvalEmitSt(method, exp, state);
                    else
                    {
                        base.EvalMethodCall(method, exp, state);
                    }
                }
                else
                {
                    base.EvalMethodCall(method, exp, state);
                }
            }
            else
            {
                if (exp.Object.Type.IsDefined(typeof(ILAllocReservedWordsAttribute), false))
                {
                    if (exp.Method.IsDefined(typeof(MethodAllocReservedWordAsAttribute), false)) EvalEmitAllocAs(method, exp, state);
                    else
                    {
                        base.EvalMethodCall(method, exp, state);
                    }
                }
                else
                {
                    base.EvalMethodCall(method, exp, state);
                }
            }
        }

        protected virtual void EvalEmitLd(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            if (exp.Arguments.Count == 2)
            {
                EvalExpression(method, exp.Arguments[0], state);
                if (0 < state.ExtractInfos.Count)
                {
                    throw new NotImplementedException();
                }
            }

            var extractExp = Expression.Call(
                                DslExtract_T.MakeGenericMethod(exp.Arguments[exp.Arguments.Count - 1].Type),
                                new Expression[] 
                                { 
                                    exp.Arguments[exp.Arguments.Count - 1]
                                }
                             );

            EvalExtract(method, extractExp, state);

            var opcode = exp.Arguments.Count == 1 ? OpCodes.Ldsfld : OpCodes.Ldfld;

            if (0 < state.ExtractInfos.Count)
            {
                var extractInfo = state.ExtractInfos.Pop();
                var fieldInfo = default(FieldInfo);
                var fieldDecl = default(IFieldDeclaration);
                if ((fieldInfo = extractInfo.Value as FieldInfo) != null)
                {
                    method.Body.ILOperator.Emit(opcode, fieldInfo);
                }
                else if ((fieldDecl = extractInfo.Value as IFieldDeclaration) != null)
                {
                    method.Body.ILOperator.Emit(opcode, fieldDecl);
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

        protected virtual void EvalEmitSt(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            if (exp.Arguments.Count == 1)
            {
                var extractExp = Expression.Call(
                                    DslExtract_object.MakeGenericMethod(typeof(FieldInfo)),
                                    new Expression[] 
                                    { 
                                        exp.Arguments[0]
                                    }
                                 );

                EvalExtract(method, extractExp, state);
                if (0 < state.ExtractInfos.Count)
                {
                    var extractInfo = state.ExtractInfos.Pop();
                    var fieldInfo = (FieldInfo)extractInfo.Value;
                    state.AllocInfos.Push(new EmitAllocInfo(fieldInfo));
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else if (exp.Arguments.Count == 2)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected virtual void EvalEmitAllocAs(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            EvalExpression(method, exp.Object, state);
            if (0 < state.AllocInfos.Count)
            {
                var allocInfo = (EmitAllocInfo)state.AllocInfos.Pop();

                EvalExpression(method, exp.Arguments[0], state);
                method.Body.ILOperator.Emit(OpCodes.Stsfld, allocInfo.Field);
            }
            else
            {
                throw new NotImplementedException();
            }
            state.ProhibitsLastAutoPop = true;
        }






        class MethodBaseEmitterDecorator : ReflectiveMethodBaseDecorator
        {
            readonly MethodBodyEmitterDecorator bodyDecorator;
            public MethodBaseEmitterDecorator(ReflectiveMethodDesigner2 gen, string ilName)
                : base(gen)
            {
                ILName = ilName;
                bodyDecorator = new MethodBodyEmitterDecorator(this);
            }

            public override ReflectiveMethodBodyDecorator BodyDecorator
            {
                get { return bodyDecorator; }
            }

            public string ILName { get; private set; }
        }

        class MethodBodyEmitterDecorator : ReflectiveMethodBodyDecorator
        {
            readonly ILOperationEmitterDecorator ilOperationDecorator;
            public MethodBodyEmitterDecorator(MethodBaseEmitterDecorator methodDecorator)
                : base(methodDecorator)
            {
                this.ilOperationDecorator = new ILOperationEmitterDecorator(this);
            }

            public MethodBaseEmitterDecorator MethodEmitterDecorator
            {
                get { return (MethodBaseEmitterDecorator)methodDecorator; }
            }

            public override ReflectiveMethodBaseDecorator MethodDecorator
            {
                get { return methodDecorator; }
            }

            public override ReflectiveILOperationDecorator ILOperationDecorator
            {
                get { return ilOperationDecorator; }
            }
        }

        class ILOperationEmitterDecorator : ReflectiveILOperationDecorator
        {
            internal int localIndex;
            internal int labelIndex;

            public ILOperationEmitterDecorator(MethodBodyEmitterDecorator bodyDecorator)
                : base(bodyDecorator)
            {
            }

            public MethodBodyEmitterDecorator BodyEmitterDecorator { get { return (MethodBodyEmitterDecorator)bodyDecorator; } }
            public string ILName { get { return BodyEmitterDecorator.MethodEmitterDecorator.ILName; } }

            public override object Source { get { throw new NotImplementedException(); } }
            public override ILocalGenerator AddLocal(string name, Type localType) { throw new NotImplementedException(); }
            public override ILocalGenerator AddLocal(Type localType) 
            {
                var local = new LocalEmitterDecorator(this, localType);
                Gen.Eval(() => Dsl.Store<LocalBuilder>(local.Name).As(Dsl.Load<ILGenerator>(ILName).DeclareLocal(Dsl.Extract(localType))));
                return local;
            }
            public override ILocalGenerator AddLocal(Type localType, bool pinned) { throw new NotImplementedException(); }
            public override ILabelGenerator AddLabel() 
            {
                var label = new LabelEmitterDecorator(this);
                Gen.Eval(() => Dsl.Store<Label>(label.Name).As(Dsl.Load<ILGenerator>(ILName).DefineLabel()));
                return label;
            }
            public override void Emit(OpCode opcode) { Gen.Eval(() => Dsl.Load<ILGenerator>(ILName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)))); }
            public override void Emit(OpCode opcode, byte arg) { throw new NotImplementedException(); }
            public override void Emit(OpCode opcode, ConstructorInfo con) 
            {
                var local = new LocalEmitterDecorator(this, typeof(ConstructorInfo));
                Gen.Eval(() => Dsl.Store<ConstructorInfo>(local.Name).As(Dsl.Extract(con.DeclaringType).GetConstructor(Dsl.Extract(con.ExportBinding()), null, Dsl.Extract(con.ParameterTypes()), null)));
                Gen.Eval(() => Dsl.Load<ILGenerator>(ILName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Load<ConstructorInfo>(local.Name))); 
            }
            public override void Emit(OpCode opcode, double arg) { throw new NotImplementedException(); }
            public override void Emit(OpCode opcode, FieldInfo field) 
            {
                var local = new LocalEmitterDecorator(this, typeof(FieldInfo));
                Gen.Eval(() => Dsl.Store<FieldInfo>(local.Name).As(Dsl.Extract(field.DeclaringType).GetField(Dsl.Extract(field.Name), Dsl.Extract(field.ExportBinding()))));
                Gen.Eval(() => Dsl.Load<ILGenerator>(ILName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Load<FieldInfo>(local.Name))); 
            }
            public override void Emit(OpCode opcode, float arg) { throw new NotImplementedException(); }
            public override void Emit(OpCode opcode, int arg) { Gen.Eval(() => Dsl.Load<ILGenerator>(ILName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Extract(arg))); }
            public override void Emit(OpCode opcode, ILabelDeclaration label) { Gen.Eval(() => Dsl.Load<ILGenerator>(ILName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Load<Label>(label.Name))); }
            public override void Emit(OpCode opcode, ILabelDeclaration[] labels) { throw new NotImplementedException(); }
            public override void Emit(OpCode opcode, ILocalDeclaration local) { Gen.Eval(() => Dsl.Load<ILGenerator>(ILName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Load<LocalBuilder>(local.Name))); }
            public override void Emit(OpCode opcode, long arg) { throw new NotImplementedException(); }
            public override void Emit(OpCode opcode, MethodInfo meth) 
            {
                var local = new LocalEmitterDecorator(this, typeof(MethodInfo));
                Gen.Eval(() => Dsl.Store<MethodInfo>(local.Name).As(Dsl.Extract(meth.DeclaringType).GetMethod(Dsl.Extract(meth.Name), Dsl.Extract(meth.ExportBinding()), null, Dsl.Extract(meth.ParameterTypes()), null)));
                Gen.Eval(() => Dsl.Load<ILGenerator>(ILName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Load<MethodInfo>(local.Name))); 
            }
            public override void Emit(OpCode opcode, sbyte arg) { Gen.Eval(() => Dsl.Load<ILGenerator>(ILName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Extract(arg))); }
            public override void Emit(OpCode opcode, short arg) { Gen.Eval(() => Dsl.Load<ILGenerator>(ILName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Extract(arg))); }
            public override void Emit(OpCode opcode, string str) { Gen.Eval(() => Dsl.Load<ILGenerator>(ILName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Extract(str))); }
            public override void Emit(OpCode opcode, Type cls) { Gen.Eval(() => Dsl.Load<ILGenerator>(ILName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Extract(cls))); }
            public override void Emit(OpCode opcode, IConstructorDeclaration constructorDecl) { throw new NotImplementedException(); }
            public override void Emit(OpCode opcode, IMethodDeclaration methodDecl) { throw new NotImplementedException(); }
            public override void Emit(OpCode opcode, IParameterDeclaration parameterDecl) { Gen.Eval(() => Dsl.Load<ILGenerator>(ILName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Extract<short>(parameterDecl.Position))); }
            public override void Emit(OpCode opcode, IFieldDeclaration fieldDecl) 
            {
                var local = new LocalEmitterDecorator(this, typeof(MethodInfo));
                Gen.Eval(() => Dsl.Store<FieldInfo>(local.Name).As(Dsl.Extract(fieldDecl.DeclaringType.Source).GetField(Dsl.Extract(fieldDecl.Name), Dsl.Extract(fieldDecl.ExportBinding()))));
                Gen.Eval(() => Dsl.Load<ILGenerator>(ILName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Load<FieldInfo>(local.Name)));
            }
            public override void Emit(OpCode opcode, IPortableScopeItem scopeItem) { throw new NotImplementedException(); }
            public override void SetLabel(ILabelDeclaration loc) 
            {
                var label = (LabelEmitterDecorator)loc;
                Gen.Eval(() => Dsl.Load<ILGenerator>(ILName).MarkLabel(Dsl.Load<Label>(label.Name)));
            }
        }

        class LocalEmitterDecorator : ReflectiveLocalDecorator
        {
            public LocalEmitterDecorator(ILOperationEmitterDecorator ilOperationDecorator, Type type)
                : base(ilOperationDecorator, type, ilOperationDecorator.localIndex++)
            {
            }

            public LocalEmitterDecorator(ILOperationEmitterDecorator ilOperationDecorator, string name, Type type)
                : base(ilOperationDecorator, type, ilOperationDecorator.localIndex++)
            {
            }
        }

        class LabelEmitterDecorator : ReflectiveLabelDecorator
        {
            public LabelEmitterDecorator(ILOperationEmitterDecorator ilOperationDecorator)
                : base(ilOperationDecorator, ilOperationDecorator.labelIndex++)
            {
            }

            public LabelEmitterDecorator(ILOperationEmitterDecorator ilOperationDecorator, string name)
                : base(ilOperationDecorator, name, ilOperationDecorator.labelIndex++)
            {
            }
        }

        protected class EmitAllocInfo : AllocInfo
        {
            public EmitAllocInfo(FieldInfo field)
                : base(field.Name, field.FieldType)
            {
                Field = field;
            }

            public FieldInfo Field { get; private set; }
        }


        class ILAllocReservedWords : IILAllocReservedWords
        {
            public object As(object value)
            {
                throw new NotSupportedException();
            }
        }

        class ILAllocReservedWords<T> : IILAllocReservedWords<T>
        {
            public T As(T value)
            {
                throw new NotSupportedException();
            }
        }
    }
}
