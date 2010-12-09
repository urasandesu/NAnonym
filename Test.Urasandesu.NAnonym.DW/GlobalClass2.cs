/* 
 * File: GlobalClass2.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2010 Akira Sugiura
 *  
 *  This software is MIT License.
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (o "Software"), to deal
 *  in o Software without restriction, including without limitation o rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of o Software, and to permit persons to whom o Software is
 *  furnished to do so, subject to o following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of o Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */
 

using Test.Urasandesu.NAnonym.Etc;
using Urasandesu.NAnonym.DW;
using Urasandesu.NAnonym.Cecil.DW;

namespace Test.Urasandesu.NAnonym.Cecil.DW
{
    public class GlobalClass2 : GlobalClass
    {
        protected override DependencyClass OnRegister()
        {
            var class2GlobalClass = new GlobalClass<Class2>();
            class2GlobalClass.Setup(o =>
            {
                o.HideMethod<string, string>(_ => _.Print).By(
                value =>
                {
                    return "Modified prefix at Class2.Print" + value + "Modified suffix at Class2.Print";
                });
            });
            return class2GlobalClass;
        }

        protected override string CodeBase
        {
            get { return typeof(Class2).Assembly.CodeBase; }
        }

        protected override string Location
        {
            get { return typeof(Class2).Assembly.Location; }
        }
    }
}

