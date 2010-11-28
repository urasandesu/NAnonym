/* 
 * File: MethodTestClass1.cs
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

namespace Test.Urasandesu.NAnonym.Etc
{
    public class MethodTestClass1
    {
        public void Action1()
        {
            Console.WriteLine("Hello, World!!");
        }

        public void Action2LocalVariable()
        {
            int i = 100;
            Console.WriteLine("i.ToString() = {0}", i.ToString());
        }

        public void Action3Exception()
        {
            int i = 100;
            int j = 0;
            try
            {
                Console.WriteLine("i / j = {0}", i / j);
            }
            catch (DivideByZeroException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Action4Generic<T>()
        {
            Console.WriteLine(typeof(T).ToString());
        }

        public void Action5NoBody()
        {
            Action1();
            Action2LocalVariable();
            Action3Exception();
            Action4Generic<string>();
        }

        public int Func1Parameters(int value)
        {
            return value + value * value;
        }
    }
}

