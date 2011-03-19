/* 
 * File: MCILOperatorImpl.cs
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
using Mono.Cecil;
using Mono.Cecil.Cil;
using MC = Mono.Cecil;
using MCC = Mono.Cecil.Cil;
using System.Reflection;
using System.Reflection.Emit;
using SR = System.Reflection;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;
using Urasandesu.NAnonym.Linq;
using Urasandesu.NAnonym.ILTools.Impl;
using UN = Urasandesu.NAnonym;
using UNI = Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Cecil.Mixins.Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil.Cil;
using Urasandesu.NAnonym.ILTools.Impl.System.Reflection;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    sealed class MCILOperatorImpl : UNI::IILOperator
    {
        readonly MethodDefinition methodDef;
        readonly MC::Cil.MethodBody bodyDef;
        readonly IILProcessor il;
        readonly ModuleDefinition moduleDef;

        public MCILOperatorImpl(ILProcessor il)
            : this(il, ILEmitMode.Normal, null)
        {
        }

        public MCILOperatorImpl(ILProcessor il, ILEmitMode mode, Instruction target)
        {
            switch (mode)
            {
                case ILEmitMode.Normal:
                    this.il = new NormalILProcessor(il);
                    break;
                case ILEmitMode.InsertBefore:
                    this.il = new InsertBeforeILProcessor(il, target);
                    break;
                case ILEmitMode.InsertAfter:
                    throw new NotImplementedException();
                default:
                    throw new NotSupportedException();
            }
            bodyDef = il.Body;
            methodDef = bodyDef.Method;
            moduleDef = methodDef.Module;
        }

        public object Source
        {
            get { return methodDef; }
        }

        public UNI::ILocalGenerator AddLocal(string name, Type localType)
        {
            var variableDef = new VariableDefinition(name, moduleDef.Import(localType));
            bodyDef.Variables.Add(variableDef);
            return new MCLocalGeneratorImpl(variableDef);
        }

        public UNI::ILocalGenerator AddLocal(string name, UNI::ITypeDeclaration localType)
        {
            var srimpl = default(SRTypeDeclarationImpl);
            var mcimpl = default(MCTypeDeclarationImpl);
            var variableDef = default(VariableDefinition);
            if ((srimpl = localType as SRTypeDeclarationImpl) != null)
            {
                variableDef = new VariableDefinition(name, moduleDef.Import(srimpl.type));
            }
            else if ((mcimpl = localType as MCTypeDeclarationImpl) != null)
            {
                variableDef = new VariableDefinition(name, mcimpl.typeRef);
            }
            bodyDef.Variables.Add(variableDef);
            return new MCLocalGeneratorImpl(variableDef);
        }

        public UNI::ILocalGenerator AddLocal(UNI::ITypeDeclaration localType)
        {
            var srimpl = default(SRTypeDeclarationImpl);
            var mcimpl = default(MCTypeDeclarationImpl);
            var variableDef = default(VariableDefinition);
            if ((srimpl = localType as SRTypeDeclarationImpl) != null)
            {
                variableDef = new VariableDefinition(moduleDef.Import(srimpl.type));
            }
            else if ((mcimpl = localType as MCTypeDeclarationImpl) != null)
            {
                variableDef = new VariableDefinition(mcimpl.typeRef);
            }
            bodyDef.Variables.Add(variableDef);
            return new MCLocalGeneratorImpl(variableDef);
        }

        public UNI::ILocalGenerator AddLocal(Type localType)
        {
            var variableDef = new VariableDefinition(moduleDef.Import(localType));
            bodyDef.Variables.Add(variableDef);
            return new MCLocalGeneratorImpl(variableDef);
        }

        public UNI::ILocalGenerator AddLocal(Type localType, bool pinned)
        {
            throw new NotImplementedException();
        }

        public UNI::ILabelGenerator AddLabel()
        {
            var target = Instruction.Create(MCC::OpCodes.Nop);
            return new MCLabelGeneratorImpl(target);
        }

        public void Emit(UNI::OpCode opcode)
        {
            il.Emit(opcode.ToCecil());
        }

        public void Emit(UNI::OpCode opcode, byte arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(UNI::OpCode opcode, ConstructorInfo con)
        {
            il.Emit(opcode.ToCecil(), moduleDef.Import(con));
        }

        public void Emit(UNI::OpCode opcode, double arg)
        {
            il.Emit(opcode.ToCecil(), arg);
        }

        public void Emit(UNI::OpCode opcode, FieldInfo field)
        {
            il.Emit(opcode.ToCecil(), moduleDef.Import(field));
        }

        public void Emit(UNI::OpCode opcode, float arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(UNI::OpCode opcode, int arg)
        {
            il.Emit(opcode.ToCecil(), arg);
        }

        public void Emit(UNI::OpCode opcode, UNI::ILabelDeclaration label)
        {
            il.Emit(opcode.ToCecil(), ((MCLabelDeclarationImpl)label).Target);
        }

        public void Emit(UNI::OpCode opcode, UNI::ILabelDeclaration[] labels)
        {
            throw new NotImplementedException();
        }

        public void Emit(UNI::OpCode opcode, UNI::ILocalDeclaration local)
        {
            il.Emit(opcode.ToCecil(), ((MCLocalGeneratorImpl)local).VariableDef);
        }

        public void Emit(UNI::OpCode opcode, long arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(UNI::OpCode opcode, MethodInfo meth)
        {
            il.Emit(opcode.ToCecil(), moduleDef.Import(meth));
        }

        public void Emit(UNI::OpCode opcode, sbyte arg)
        {
            il.Emit(opcode.ToCecil(), arg);
        }

        public void Emit(UNI::OpCode opcode, short arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(UNI::OpCode opcode, string str)
        {
            il.Emit(opcode.ToCecil(), str);
        }

        public void Emit(UNI::OpCode opcode, Type cls)
        {
            il.Emit(opcode.ToCecil(), moduleDef.Import(cls));
        }

        public void Emit(UNI::OpCode opcode, UNI::ITypeDeclaration type)
        {
            var srimpl = default(SRTypeDeclarationImpl);
            var mcimpl = default(MCTypeDeclarationImpl);
            if ((srimpl = type as SRTypeDeclarationImpl) != null)
            {
                il.Emit(opcode.ToCecil(), moduleDef.Import(srimpl.type));
            }
            else if ((mcimpl = type as MCTypeDeclarationImpl) != null)
            {
                il.Emit(opcode.ToCecil(), mcimpl.typeRef);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public void Emit(UNI::OpCode opcode, UNI::IConstructorDeclaration constructorDecl)
        {
            var srimpl = default(SRConstructorDeclarationImpl);
            var mcimpl = default(MCConstructorDeclarationImpl);
            if ((srimpl = constructorDecl as SRConstructorDeclarationImpl) != null)
            {
                il.Emit(opcode.ToCecil(), moduleDef.Import(srimpl.ConstructorInfo));
            }
            else if ((mcimpl = constructorDecl as MCConstructorDeclarationImpl) != null)
            {
                il.Emit(opcode.ToCecil(), mcimpl.ConstructorRef);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public void Emit(UNI::OpCode opcode, UNI::IMethodDeclaration methodDecl)
        {
            var srimpl = default(SRMethodDeclarationImpl);
            var mcimpl = default(MCMethodDeclarationImpl);
            if ((srimpl = methodDecl as SRMethodDeclarationImpl) != null)
            {
                il.Emit(opcode.ToCecil(), moduleDef.Import(srimpl.MethodInfo));
            }
            else if ((mcimpl = methodDecl as MCMethodDeclarationImpl) != null)
            {
                il.Emit(opcode.ToCecil(), mcimpl.MethodDef);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public void Emit(UNI::OpCode opcode, UNI::IParameterDeclaration parameterDecl)
        {
            il.Emit(opcode.ToCecil(), ((MCParameterGeneratorImpl)parameterDecl).ParameterDef);
        }

        public void Emit(UNI::OpCode opcode, UNI::IFieldDeclaration fieldDecl)
        {
            var srimpl = default(SRFieldDeclarationImpl);
            var mcimpl = default(MCFieldDeclarationImpl);
            if ((srimpl = fieldDecl as SRFieldDeclarationImpl) != null)
            {
                il.Emit(opcode.ToCecil(), moduleDef.Import(srimpl.FieldInfo));
            }
            else if ((mcimpl = fieldDecl as MCFieldDeclarationImpl) != null)
            {
                il.Emit(opcode.ToCecil(), mcimpl.FieldRef);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public void Emit(UNI::OpCode opcode, UNI::IPortableScopeItem scopeItem)
        {
            il.Emit(opcode.ToCecil(), ((MCPortableScopeItemImpl)scopeItem).FieldDef);
        }

        public void SetLabel(UNI::ILabelDeclaration loc)
        {
            il.Append(((MCLabelDeclarationImpl)loc).Target);
        }
    }
}

