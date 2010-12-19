/* 
 * File: SRMethodBodyGeneratorImpl.cs
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
using System.Reflection.Emit;
using System.Collections.ObjectModel;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    sealed class SRMethodBodyGeneratorImpl : SRMethodBodyDeclarationImpl, IMethodBodyGenerator
    {
        SRILOperatorImpl il;

        ReadOnlyCollection<ILocalGenerator> readonlyLocalGens;

        public SRMethodBodyGeneratorImpl(ISRMethodBaseGenerator methodBodyGen)
        {
            Initialize(new SRILOperatorImpl(methodBodyGen.GetILGenerator(), this));
        }

        void Initialize(SRILOperatorImpl il)
        {
            this.il = il;
            LocalGens = new List<SRLocalGeneratorImpl>();
            readonlyLocalGens = new ReadOnlyCollection<ILocalGenerator>(
                LocalGens.TransformEnumerateOnly(localGen => (ILocalGenerator)localGen));
        }

        public IILOperator ILOperator { get { return il; } }

        public new ReadOnlyCollection<ILocalGenerator> Locals
        {
            get { return readonlyLocalGens; }
        }

        public new ReadOnlyCollection<IDirectiveGenerator> Directives
        {
            get { return il.Directives; }
        }

        internal List<SRLocalGeneratorImpl> LocalGens { get; private set; }

        public ILocalGenerator AddLocal(ILocalGenerator localGen)
        {
            throw new NotImplementedException();
        }

        public new IMethodBaseGenerator Method
        {
            get { throw new NotImplementedException(); }
        }
    }
}

