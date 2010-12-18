/* 
 * File: GlobalMethodWeaver.cs
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
using Urasandesu.NAnonym.DW;
using UNI = Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.DW
{
    class GlobalMethodWeaver : MethodWeaver
    {
        public GlobalMethodWeaver(ConstructorWeaver constructorWeaver, HashSet<WeaveMethodInfo> methodSet)
            : base(constructorWeaver, methodSet)
        {
        }

        protected override MethodWeaveDefiner GetMethodDefiner(MethodWeaver parent, WeaveMethodInfo weaveMethod)
        {
            if (weaveMethod.Mode == GlobalSetupModes.Hide)
            {
                return new GlobalHideMethodDefiner(parent, weaveMethod);
            }
            else if (weaveMethod.Mode == GlobalSetupModes.Before)
            {
                return new GlobalBeforeMethodWeaveDefiner(parent, weaveMethod);
            }
            else if (weaveMethod.Mode == GlobalSetupModes.After)
            {
                return new GlobalAfterMethodWeaveDefiner(parent, weaveMethod);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        protected override MethodWeaveBuilder GetMethodBuilder(MethodWeaveDefiner parentDefiner)
        {
            return new GlobalMethodBuilder(parentDefiner);
        }
    }

    class GlobalAfterMethodWeaveDefiner : GlobalMethodDefiner
    {
        public GlobalAfterMethodWeaveDefiner(MethodWeaver parent, WeaveMethodInfo weaveMethod)
            : base(parent, weaveMethod)
        {
        }

        protected override UNI::IMethodGenerator GetMethodInterface()
        {
            throw new NotImplementedException();
        }
    }
}

