/* 
 * File: DependencyUtilTest.cs
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
using Urasandesu.NAnonym.DW;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Linq;
//using Mono.Cecil;
using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym;
using Test.Urasandesu.NAnonym.Etc;
using SRE = System.Reflection.Emit;

namespace Test.Urasandesu.NAnonym.DW
{
    [TestFixture]
    public class DependencyUtilTest
    {
        [Test]
        public void Hoge()
        {
            foreach (var value in typeof(SRE::OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static).Select(fieldInfo => string.Format("{0}", fieldInfo.GetValue(null))).OrderBy(_value => _value))
            {
                Console.WriteLine(value);
            }
        }

        class A<T>
        {
            public void Method1<S>(T t, S s)
            {
            }

            public void Method2(T t)
            {
            }

            A<T>[] field1 = null;
        }
    }
}

