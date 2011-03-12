/* 
 * File: MockMethodBodyGenerator.cs
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
using NUnit.Framework;
using Assert = Urasandesu.NAnonym.Test.Assert;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Test;
using Test.Urasandesu.NAnonym.Etc;
using Urasandesu.NAnonym.Formulas;
using Urasandesu.NAnonym;
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.NAnonym.Mixins.System.Reflection;
using Urasandesu.NAnonym.ILTools.Impl.System.Reflection;
using System.Collections.ObjectModel;

namespace Test.Urasandesu.NAnonym.ILTools
{
    class MockMethodBodyGenerator : IMethodBodyGenerator
    {
        public MockMethodBodyGenerator()
        {
            ILOperatorProvider = () => new MockILOperator();
        }

        public Func<IILOperator> ILOperatorProvider { get; set; }

        public IMethodBaseGenerator Method
        {
            get { throw new NotImplementedException(); }
        }

        public IILOperator ILOperator
        {
            get { return ILOperatorProvider(); }
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
}
