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
        IILReservedWords reservedWords = new ILReservedWords();
        IILAllocReservedWords allocReservedWords = new ILAllocReservedWords();

        public ReflectiveILEmitter(ReflectiveMethodDesigner gen, string ilName)
            : base(new MethodBaseEmitterDecorator(gen, ilName))
        {
        }

        protected override IMethodReservedWords ReservedWords
        {
            get
            {
                return reservedWords;
            }
        }

        protected override IMethodAllocReservedWords AllocReservedWords
        {
            get
            {
                return allocReservedWords;
            }
        }

        public void Emit(Expression<Action<IILReservedWords>> exp)
        {
            Eval(Method, exp.Body, state);
        }

        protected override void EvalMethodCall(IMethodBaseGenerator method, MethodCallExpression exp, EvalState state)
        {
            if (exp.Object == null)
            {
                base.EvalMethodCall(method, exp, state);
            }
            else
            {
                if (exp.Object.Type.IsDefined(typeof(ILReservedWordsAttribute), false))
                {
                    if (exp.Method.IsDefined(typeof(ILReservedWordLdAttribute), false)) EvalEmitLd(method, exp, state);
                    else if (exp.Method.IsDefined(typeof(ILReservedWordStAttribute), false)) EvalEmitSt(method, exp, state);
                    else
                    {
                        base.EvalMethodCall(method, exp, state);
                    }
                }
                else if (exp.Object.Type.IsDefined(typeof(ILAllocReservedWordsAttribute), false))
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
                if (0 < state.ExtractInfoStack.Count)
                {
                    throw new NotImplementedException();
                }
            }

            var extractExp = Expression.Call(
                                Expression.Constant(ReservedWords),
                                ReservedWordXInfo_T.MakeGenericMethod(exp.Arguments[exp.Arguments.Count - 1].Type),
                                new Expression[] 
                                { 
                                    exp.Arguments[exp.Arguments.Count - 1]
                                }
                             );

            EvalExtract(method, extractExp, state);

            var opcode = exp.Arguments.Count == 1 ? OpCodes.Ldsfld : OpCodes.Ldfld;

            if (0 < state.ExtractInfoStack.Count)
            {
                var extractInfo = state.ExtractInfoStack.Pop();
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
                                    Expression.Constant(ReservedWords),
                                    ReservedWordXInfo_object.MakeGenericMethod(typeof(FieldInfo)),
                                    new Expression[] 
                                    { 
                                        exp.Arguments[0]
                                    }
                                 );

                EvalExtract(method, extractExp, state);
                if (0 < state.ExtractInfoStack.Count)
                {
                    var extractInfo = state.ExtractInfoStack.Pop();
                    var fieldInfo = (FieldInfo)extractInfo.Value;
                    state.AllocInfoStack.Push(new EmitAllocInfo(fieldInfo));
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
            if (0 < state.AllocInfoStack.Count)
            {
                var allocInfo = (EmitAllocInfo)state.AllocInfoStack.Pop();

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
            public MethodBaseEmitterDecorator(ReflectiveMethodDesigner gen, string ilName)
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
                Gen.Eval(_ => _.St<LocalBuilder>(local.Name).As(_.Ld<ILGenerator>(ILName).DeclareLocal(_.X(localType))));
                return local;
            }
            public override ILocalGenerator AddLocal(Type localType, bool pinned) { throw new NotImplementedException(); }
            public override ILabelGenerator AddLabel() 
            {
                var label = new LabelEmitterDecorator(this);
                Gen.Eval(_ => _.St<Label>(label.Name).As(_.Ld<ILGenerator>(ILName).DefineLabel()));
                return label;
            }
            public override void Emit(OpCode opcode) { Gen.Eval(_ => _.Ld<ILGenerator>(ILName).Emit(_.Cm(opcode.ToClr(), typeof(SRE::OpCodes)))); }
            public override void Emit(OpCode opcode, byte arg) { throw new NotImplementedException(); }
            public override void Emit(OpCode opcode, ConstructorInfo con) 
            {
                var local = new LocalEmitterDecorator(this, typeof(ConstructorInfo));
                Gen.Eval(_ => _.St<ConstructorInfo>(local.Name).As(_.X(con.DeclaringType).GetConstructor(_.X(con.ExportBinding()), null, _.X(con.ParameterTypes()), null)));
                Gen.Eval(_ => _.Ld<ILGenerator>(ILName).Emit(_.Cm(opcode.ToClr(), typeof(SRE::OpCodes)), _.Ld<ConstructorInfo>(local.Name))); 
            }
            public override void Emit(OpCode opcode, double arg) { throw new NotImplementedException(); }
            public override void Emit(OpCode opcode, FieldInfo field) 
            {
                var local = new LocalEmitterDecorator(this, typeof(FieldInfo));
                Gen.Eval(_ => _.St<FieldInfo>(local.Name).As(_.X(field.DeclaringType).GetField(_.X(field.Name), _.X(field.ExportBinding()))));
                Gen.Eval(_ => _.Ld<ILGenerator>(ILName).Emit(_.Cm(opcode.ToClr(), typeof(SRE::OpCodes)), _.Ld<FieldInfo>(local.Name))); 
            }
            public override void Emit(OpCode opcode, float arg) { throw new NotImplementedException(); }
            public override void Emit(OpCode opcode, int arg) { Gen.Eval(_ => _.Ld<ILGenerator>(ILName).Emit(_.Cm(opcode.ToClr(), typeof(SRE::OpCodes)), _.X(arg))); }
            public override void Emit(OpCode opcode, ILabelDeclaration label) { Gen.Eval(_ => _.Ld<ILGenerator>(ILName).Emit(_.Cm(opcode.ToClr(), typeof(SRE::OpCodes)), _.Ld<Label>(label.Name))); }
            public override void Emit(OpCode opcode, ILabelDeclaration[] labels) { throw new NotImplementedException(); }
            public override void Emit(OpCode opcode, ILocalDeclaration local) { Gen.Eval(_ => _.Ld<ILGenerator>(ILName).Emit(_.Cm(opcode.ToClr(), typeof(SRE::OpCodes)), _.Ld<LocalBuilder>(local.Name))); }
            public override void Emit(OpCode opcode, long arg) { throw new NotImplementedException(); }
            public override void Emit(OpCode opcode, MethodInfo meth) 
            {
                var local = new LocalEmitterDecorator(this, typeof(MethodInfo));
                Gen.Eval(_ => _.St<MethodInfo>(local.Name).As(_.X(meth.DeclaringType).GetMethod(_.X(meth.Name), _.X(meth.ExportBinding()), null, _.X(meth.ParameterTypes()), null)));
                Gen.Eval(_ => _.Ld<ILGenerator>(ILName).Emit(_.Cm(opcode.ToClr(), typeof(SRE::OpCodes)), _.Ld<MethodInfo>(local.Name))); 
            }
            public override void Emit(OpCode opcode, sbyte arg) { Gen.Eval(_ => _.Ld<ILGenerator>(ILName).Emit(_.Cm(opcode.ToClr(), typeof(SRE::OpCodes)), _.X(arg))); }
            public override void Emit(OpCode opcode, short arg) { Gen.Eval(_ => _.Ld<ILGenerator>(ILName).Emit(_.Cm(opcode.ToClr(), typeof(SRE::OpCodes)), _.X(arg))); }
            public override void Emit(OpCode opcode, string str) { Gen.Eval(_ => _.Ld<ILGenerator>(ILName).Emit(_.Cm(opcode.ToClr(), typeof(SRE::OpCodes)), _.X(str))); }
            public override void Emit(OpCode opcode, Type cls) { Gen.Eval(_ => _.Ld<ILGenerator>(ILName).Emit(_.Cm(opcode.ToClr(), typeof(SRE::OpCodes)), _.X(cls))); }
            public override void Emit(OpCode opcode, IConstructorDeclaration constructorDecl) { throw new NotImplementedException(); }
            public override void Emit(OpCode opcode, IMethodDeclaration methodDecl) { throw new NotImplementedException(); }
            public override void Emit(OpCode opcode, IParameterDeclaration parameterDecl) { Gen.Eval(_ => _.Ld<ILGenerator>(ILName).Emit(_.Cm(opcode.ToClr(), typeof(SRE::OpCodes)), _.X<short>(parameterDecl.Position))); }
            public override void Emit(OpCode opcode, IFieldDeclaration fieldDecl) 
            {
                var local = new LocalEmitterDecorator(this, typeof(MethodInfo));
                Gen.Eval(_ => _.St<FieldInfo>(local.Name).As(_.X(fieldDecl.DeclaringType.Source).GetField(_.X(fieldDecl.Name), _.X(fieldDecl.ExportBinding()))));
                Gen.Eval(_ => _.Ld<ILGenerator>(ILName).Emit(_.Cm(opcode.ToClr(), typeof(SRE::OpCodes)), _.Ld<FieldInfo>(local.Name)));
            }
            public override void Emit(OpCode opcode, IPortableScopeItem scopeItem) { throw new NotImplementedException(); }
            public override void SetLabel(ILabelDeclaration loc) 
            {
                var label = (LabelEmitterDecorator)loc;
                Gen.Eval(_ => _.Ld<ILGenerator>(ILName).MarkLabel(_.Ld<Label>(label.Name)));
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

        class ILReservedWords : IILReservedWords
        {
            public T Ld<T>(FieldInfo field)
            {
                throw new NotSupportedException();
            }

            public T Ld<T>(object instance, IFieldDeclaration field)
            {
                throw new NotSupportedException();
            }

            public IILAllocReservedWords<T> St<T>(FieldInfo field)
            {
                throw new NotSupportedException();
            }

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
                throw new NotImplementedException();
            }
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
