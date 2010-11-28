/* 
 * File: MethodWeaver.cs
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

namespace Urasandesu.NAnonym.DW
{
    abstract class MethodWeaver : Weaver
    {
        public ConstructorWeaver ConstructorWeaver { get; private set; }
        public HashSet<WeaveMethodInfo> MethodSet { get; private set; }
        public int IncreaseMethodCacheSequence()
        {
            return MethodCacheSequence++;
        }
        protected int MethodCacheSequence { get; private set; }
        public MethodWeaver(ConstructorWeaver constructorWeaver, HashSet<WeaveMethodInfo> methodSet)
        {
            ConstructorWeaver = constructorWeaver;
            MethodSet = methodSet;
        }

        public override void Apply()
        {
            foreach (var injectionMethod in MethodSet)
            {
                var definer = GetMethodDefiner(this, injectionMethod);
                definer.Create();

                var builder = GetMethodBuilder(definer);
                builder.Construct();
            }
        }

        protected abstract MethodWeaveDefiner GetMethodDefiner(MethodWeaver parent, WeaveMethodInfo injectionMethod);
        protected abstract MethodWeaveBuilder GetMethodBuilder(MethodWeaveDefiner parentDefiner);        
    }
}

