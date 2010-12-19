/* 
 * File: GenerativeEmitter.cs
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
using System.Runtime.Serialization;

namespace Urasandesu.NAnonym.ILTools
{
    public class GenerativeEmitter : ExpressiveGenerator
    {
        public GenerativeEmitter(ExpressiveGenerator gen, string ilName)
            : base(new EmittingGenerator(gen, ilName))
        {
        }

        class EmittingGenerator : IMethodBaseGenerator
        {
            readonly ExpressiveGenerator gen;
            readonly IMethodBodyGenerator body;
            public EmittingGenerator(ExpressiveGenerator gen, string ilName)
            {
                this.gen = gen;
                body = new EmittingBodyGenerator(gen, ilName);
            }

            public IMethodBodyGenerator Body
            {
                get { return body; }
            }

            public ITypeGenerator DeclaringType
            {
                get { throw new NotImplementedException(); }
            }

            public ReadOnlyCollection<IParameterGenerator> Parameters
            {
                get { throw new NotImplementedException(); }
            }

            public IPortableScopeItem AddPortableScopeItem(FieldInfo fieldInfo)
            {
                throw new NotImplementedException();
            }

            public IMethodBaseGenerator ExpressBody(Action<ExpressiveGenerator> bodyExpression)
            {
                throw new NotImplementedException();
            }

            public IParameterGenerator AddParameter(int position, ParameterAttributes attributes, string parameterName)
            {
                throw new NotImplementedException();
            }

            public PortableScope CarryPortableScope()
            {
                throw new NotImplementedException();
            }

            IMethodBodyDeclaration IMethodBaseDeclaration.Body
            {
                get { throw new NotImplementedException(); }
            }

            ITypeDeclaration IMethodBaseDeclaration.DeclaringType
            {
                get { throw new NotImplementedException(); }
            }

            ReadOnlyCollection<IParameterDeclaration> IMethodBaseDeclaration.Parameters
            {
                get { throw new NotImplementedException(); }
            }

            public IPortableScopeItem NewPortableScopeItem(PortableScopeItemRawData itemRawData, object value)
            {
                throw new NotImplementedException();
            }

            public string Name
            {
                get { throw new NotImplementedException(); }
            }

            public object Source
            {
                get { throw new NotImplementedException(); }
            }

            public void OnDeserialized(StreamingContext context)
            {
                throw new NotImplementedException();
            }
        }

        class EmittingBodyGenerator : IMethodBodyGenerator
        {
            readonly ExpressiveGenerator gen;
            readonly IILOperator ilOperator;
            public EmittingBodyGenerator(ExpressiveGenerator gen, string ilName)
            {
                this.gen = gen;
                this.ilOperator = new EmittingILOperator(gen, ilName);
            }

            public IMethodBaseGenerator Method
            {
                get { throw new NotImplementedException(); }
            }

            public IILOperator ILOperator
            {
                get { return ilOperator; }
            }

            public ReadOnlyCollection<ILocalGenerator> Locals
            {
                get { throw new NotImplementedException(); }
            }

            public ReadOnlyCollection<IDirectiveGenerator> Directives
            {
                get { throw new NotImplementedException(); }
            }

            public ILocalGenerator AddLocal(ILocalGenerator localGen)
            {
                throw new NotImplementedException();
            }

            IMethodBaseDeclaration IMethodBodyDeclaration.Method
            {
                get { throw new NotImplementedException(); }
            }

            ReadOnlyCollection<ILocalDeclaration> IMethodBodyDeclaration.Locals
            {
                get { throw new NotImplementedException(); }
            }

            ReadOnlyCollection<IDirectiveDeclaration> IMethodBodyDeclaration.Directives
            {
                get { throw new NotImplementedException(); }
            }
        }

        class EmittingILOperator : IILOperator
        {
            readonly string ilName;
            readonly ExpressiveGenerator gen;
            public EmittingILOperator(ExpressiveGenerator gen, string ilName)
            {
                this.gen = gen;
                this.ilName = ilName;
            }

            public object Source
            {
                get { throw new NotImplementedException(); }
            }

            public ILocalGenerator AddLocal(string name, Type localType)
            {
                throw new NotImplementedException();
            }

            public ILocalGenerator AddLocal(Type localType)
            {
                throw new NotImplementedException();
            }

            public ILocalGenerator AddLocal(Type localType, bool pinned)
            {
                throw new NotImplementedException();
            }

            public ILabelGenerator AddLabel()
            {
                throw new NotImplementedException();
            }

            public void Emit(OpCode opcode)
            {
                gen.Eval(_ => _.Ld<ILGenerator>(_.X(ilName)).Emit(_.Cm(opcode.ToClr(), typeof(SRE::OpCodes))));
            }

            public void Emit(OpCode opcode, byte arg)
            {
                throw new NotImplementedException();
            }

            public void Emit(OpCode opcode, ConstructorInfo con)
            {
                throw new NotImplementedException();
            }

            public void Emit(OpCode opcode, double arg)
            {
                throw new NotImplementedException();
            }

            public void Emit(OpCode opcode, FieldInfo field)
            {
                throw new NotImplementedException();
            }

            public void Emit(OpCode opcode, float arg)
            {
                throw new NotImplementedException();
            }

            public void Emit(OpCode opcode, int arg)
            {
                throw new NotImplementedException();
            }

            public void Emit(OpCode opcode, ILabelDeclaration label)
            {
                throw new NotImplementedException();
            }

            public void Emit(OpCode opcode, ILabelDeclaration[] labels)
            {
                throw new NotImplementedException();
            }

            public void Emit(OpCode opcode, ILocalDeclaration local)
            {
                throw new NotImplementedException();
            }

            public void Emit(OpCode opcode, long arg)
            {
                throw new NotImplementedException();
            }

            public void Emit(OpCode opcode, MethodInfo meth)
            {
                gen.Eval(_ => _.Ld<ILGenerator>(_.X(ilName)).Emit(_.Cm(opcode.ToClr(), typeof(SRE::OpCodes)), _.X((MethodInfo)meth)));
            }

            public void Emit(OpCode opcode, sbyte arg)
            {
                throw new NotImplementedException();
            }

            public void Emit(OpCode opcode, short arg)
            {
                throw new NotImplementedException();
            }

            public void Emit(OpCode opcode, string str)
            {
                gen.Eval(_ => _.Ld<ILGenerator>(_.X(ilName)).Emit(_.Cm(opcode.ToClr(), typeof(SRE::OpCodes)), _.X((string)str)));
            }

            public void Emit(OpCode opcode, Type cls)
            {
                throw new NotImplementedException();
            }

            public void Emit(OpCode opcode, IConstructorDeclaration constructorDecl)
            {
                throw new NotImplementedException();
            }

            public void Emit(OpCode opcode, IMethodDeclaration methodDecl)
            {
                throw new NotImplementedException();
            }

            public void Emit(OpCode opcode, IParameterDeclaration parameterDecl)
            {
                throw new NotImplementedException();
            }

            public void Emit(OpCode opcode, IFieldDeclaration fieldDecl)
            {
                throw new NotImplementedException();
            }

            public void Emit(OpCode opcode, IPortableScopeItem scopeItem)
            {
                throw new NotImplementedException();
            }

            public void SetLabel(ILabelDeclaration loc)
            {
                throw new NotImplementedException();
            }
        }
    }
}
