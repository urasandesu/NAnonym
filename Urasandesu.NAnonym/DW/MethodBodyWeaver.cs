/* 
 * File: MethodBodyWeaver.cs
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
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DW
{
    abstract class MethodBodyWeaver : BodyWeaver
    {
        public new MethodWeaveBuilder ParentBuilder { get { return (MethodWeaveBuilder)base.ParentBuilder; } }
        public MethodBodyWeaver(ExpressiveMethodBodyGenerator gen, MethodWeaveBuilder parentBuilder)
            : base(gen, parentBuilder)
        {
        }

        public override void Apply()
        {
            var bodyDefiner = GetMethodBodyDefiner(this);
            bodyDefiner.Create();

            var bodyBuilder = GetMethodBodyBuilder(bodyDefiner);
            bodyBuilder.Construct();
        }

        protected abstract MethodBodyWeaveDefiner GetMethodBodyDefiner(MethodBodyWeaver parentBody);
        protected virtual MethodBodyWeaveBuilder GetMethodBodyBuilder(MethodBodyWeaveDefiner parentBodyDefiner)
        {
            var injectionMethod = parentBodyDefiner.ParentBody.ParentBuilder.ParentDefiner.WeaveMethod;
            if ((injectionMethod.DestinationType & MethodBodyWeaveBuilderType.AnonymousInstanceWithBase) == MethodBodyWeaveBuilderType.AnonymousInstanceWithBase)
            {
                return new AnonymousInstanceMethodBodyWeaveBuilderWithBase(parentBodyDefiner);
            }
            else if ((injectionMethod.DestinationType & MethodBodyWeaveBuilderType.AnonymousInstance) == MethodBodyWeaveBuilderType.AnonymousInstance)
            {
                return new AnonymousInstanceMethodBodyWeaveBuilder(parentBodyDefiner);
            }
            else if ((injectionMethod.DestinationType & MethodBodyWeaveBuilderType.AnonymousStaticWithBase) == MethodBodyWeaveBuilderType.AnonymousStaticWithBase)
            {
                return new AnonymousStaticMethodBodyWeaveBuilderWithBase(parentBodyDefiner);
            }
            else if ((injectionMethod.DestinationType & MethodBodyWeaveBuilderType.AnonymousStatic) == MethodBodyWeaveBuilderType.AnonymousStatic)
            {
                return new AnonymousStaticMethodBodyWeaveBuilder(parentBodyDefiner);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}

