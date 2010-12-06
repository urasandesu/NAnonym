/* 
 * File: GlobalClass3.cs
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
using Urasandesu.NAnonym.DW;
using Urasandesu.NAnonym.Cecil.DW;
using Test.Urasandesu.NAnonym.Etc;

namespace Test.Urasandesu.NAnonym.Cecil.DW
{
    public class GlobalClass3_1 : GlobalClass
    {
        protected override DependencyClass OnRegister()
        {
            var class3GlobalClass = new GlobalClass<Class3_1>();
            class3GlobalClass.Setup(the =>
            {
                the.Method<int, int, int>(_ => _.Add).IsReplacedBy(
                (x, y) =>
                {
                    return x + y + x + y;
                });
            });
            return class3GlobalClass;
        }

        protected override string CodeBase
        {
            get { return typeof(Class3_1).Assembly.CodeBase; }
        }

        protected override string Location
        {
            get { return typeof(Class3_1).Assembly.Location; }
        }
    }

    public class GlobalClass3_2 : GlobalClass
    {
        protected override DependencyClass OnRegister()
        {
            var class3GlobalClass = new GlobalClass<Class3_2>();
            class3GlobalClass.Setup(the =>
            {
                int value = 2;

                the.Field(() => value).As(value);
                the.Method<int, int, int>(_ => _.Add).IsReplacedBy(
                (x, y) =>
                {
                    return value += x + y;
                });
            });
            return class3GlobalClass;
        }

        protected override string CodeBase
        {
            get { return typeof(Class3_2).Assembly.CodeBase; }
        }

        protected override string Location
        {
            get { return typeof(Class3_2).Assembly.Location; }
        }
    }

    public class GlobalClass3_3 : GlobalClass
    {
        protected override DependencyClass OnRegister()
        {
            int value = 0;
            var class3GlobalClass = new GlobalClass<Class3_3>();
            class3GlobalClass.Setup(the =>
            {
                the.Field(() => value).As(value);
                the.Method<int, int, int>(_ => _.Add).IsReplacedBy(
                (x, y) =>
                {
                    return value += x + y;
                });
            });
            return class3GlobalClass;
        }

        protected override string CodeBase
        {
            get { return typeof(Class3_3).Assembly.CodeBase; }
        }

        protected override string Location
        {
            get { return typeof(Class3_3).Assembly.Location; }
        }
    }

    public class GlobalClass3_4 : GlobalClass
    {
        int value = 0;
        protected override DependencyClass OnRegister()
        {
            var class3GlobalClass = new GlobalClass<Class3_4>();
            class3GlobalClass.Setup(the =>
            {
                the.Field(() => value).As(value);
                the.Method<int, int, int>(_ => _.Add).IsReplacedBy(
                (x, y) =>
                {
                    return value += x + y;
                });
            });
            return class3GlobalClass;
        }

        protected override string CodeBase
        {
            get { return typeof(Class3_4).Assembly.CodeBase; }
        }

        protected override string Location
        {
            get { return typeof(Class3_4).Assembly.Location; }
        }
    }

    public class GlobalClass3_5 : GlobalClass
    {
        protected override DependencyClass OnRegister()
        {
            var class3GlobalClass = new GlobalClass<Class3_5>();
            class3GlobalClass.Setup(the =>
            {
                var simpleType1 = default(SimpleType1);
                the.Field(() => simpleType1).As(_ => new SimpleType1());
                the.Method<int, int, int>(_ => _.Add).IsReplacedBy(
                (x, y) =>
                {
                    int result = simpleType1.Increase() + x + y;
                    return result;
                });
            });
            return class3GlobalClass;
        }

        protected override string CodeBase
        {
            get { return typeof(Class3_5).Assembly.CodeBase; }
        }

        protected override string Location
        {
            get { return typeof(Class3_5).Assembly.Location; }
        }
    }

    public class GlobalClass3_6 : GlobalClass
    {
        protected override DependencyClass OnRegister()
        {
            var class3GlobalClass = new GlobalClass<Class3_6>();
            class3GlobalClass.Setup(the =>
            {
                the.Method<int, int, int>(_ => _.Add).IsReplacedBy(
                (base_Add, x, y) =>
                {
                    return base_Add(x, y) + x + y;
                });
            });
            return class3GlobalClass;
        }

        protected override string CodeBase
        {
            get { return typeof(Class3_6).Assembly.CodeBase; }
        }

        protected override string Location
        {
            get { return typeof(Class3_6).Assembly.Location; }
        }
    }

    public class GlobalClass3_7 : GlobalClass
    {
        protected override DependencyClass OnRegister()
        {
            var class3GlobalClass = new GlobalClass<Class3_7>();
            class3GlobalClass.Setup(the =>
            {
                int value = 2;

                the.Field(() => value).As(value);
                the.Method<int, int, int>(_ => _.Add).IsReplacedBy(
                (base_Add, x, y) =>
                {
                    return value += base_Add(x, y) + x + y;
                });
            });
            return class3GlobalClass;
        }

        protected override string CodeBase
        {
            get { return typeof(Class3_7).Assembly.CodeBase; }
        }

        protected override string Location
        {
            get { return typeof(Class3_7).Assembly.Location; }
        }
    }

    //public class GlobalClass4_1 : GlobalClass
    //{
    //    protected override DependencyClass OnRegister()
    //    {
    //        var globalClass4Class = new GlobalClass<Class4_1>();
    //        globalClass4Class.Setup(the =>
    //        {
    //            the.Method<int, int, int>(_ => _.Add).IsReplacedBy(
    //            (x, y) =>
    //            {
    //                return x - y;
    //            });

    //            the.Method<int, int, int>(_ => _.Minus).IsOverridedBy(
    //            (x, y) =>
    //            {
    //                return x + y;
    //            });
    //        });
    //        return globalClass4Class;
    //    }

    //    protected override string CodeBase
    //    {
    //        get { return typeof(Class4_1).Assembly.CodeBase; }
    //    }

    //    protected override string Location
    //    {
    //        get { return typeof(Class4_1).Assembly.Location; }
    //    }
    //}
}
