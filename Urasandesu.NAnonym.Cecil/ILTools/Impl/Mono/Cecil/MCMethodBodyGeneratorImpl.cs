/* 
 * File: MCMethodBodyGeneratorImpl.cs
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
using MO = Mono.Collections;
using MC = Mono.Cecil;
using MCC = Mono.Cecil.Cil;
using System.Collections.ObjectModel;
using Urasandesu.NAnonym.Linq;
using UN = Urasandesu.NAnonym;
using UNI = Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.ILTools;
using Mono.Cecil.Cil;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    sealed class MCMethodBodyGeneratorImpl : MCMethodBodyDeclarationImpl, UNI::IMethodBodyGenerator
    {
        readonly MCC::MethodBody bodyDef;
        readonly ReadOnlyCollection<ILocalGenerator> locals;
        ReadOnlyCollection<IDirectiveGenerator> directives;
        readonly MCILOperatorImpl ilOperator;

        public MCMethodBodyGeneratorImpl(MCC::MethodBody bodyDef)
            : this(bodyDef, ILEmitMode.Normal, null)
        {
        }

        public MCMethodBodyGeneratorImpl(MCC::MethodBody bodyDef, ILEmitMode mode, Instruction target)
        {
            this.bodyDef = bodyDef;
            ilOperator = new MCILOperatorImpl(bodyDef.GetILProcessor(), mode, target);
            locals = new ReadOnlyCollection<ILocalGenerator>(
                bodyDef.Variables.TransformEnumerateOnly(variableDef => (ILocalGenerator)new MCLocalGeneratorImpl(variableDef)));
            directives = new ReadOnlyCollection<IDirectiveGenerator>(
                bodyDef.Instructions.TransformEnumerateOnly(instruction => (IDirectiveGenerator)new MCDirectiveGeneratorImpl(instruction)));
        }

        public IILOperator ILOperator { get { return ilOperator; } }

        public new ReadOnlyCollection<ILocalGenerator> Locals
        {
            get { return locals; }
        }

        public new ReadOnlyCollection<IDirectiveGenerator> Directives
        {
            get { return directives; }
        }

        public ILocalGenerator AddLocal(ILocalGenerator localGen)
        {
            throw new NotImplementedException();
        }
    }
}

