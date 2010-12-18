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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.DW;
using Urasandesu.NAnonym.Cecil.DW;
using Test.Urasandesu.NAnonym.Etc;
using System.IO;
using Urasandesu.NAnonym.Test;

namespace Test.Urasandesu.NAnonym.Cecil.DW
{
    public class GlobalClass3_1 : GlobalClass
    {
        protected override DependencyClass OnRegister()
        {
            var class3GlobalClass = new GlobalClass<Class3_1>();
            class3GlobalClass.Setup(o =>
            {
                o.HideMethod<int, int, int>(_ => _.Add).By(
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
            class3GlobalClass.Setup(o =>
            {
                int value = 2;

                o.DefineField(() => value).As(value);
                o.HideMethod<int, int, int>(_ => _.Add).By(
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
            class3GlobalClass.Setup(o =>
            {
                o.DefineField(() => value).As(value);
                o.HideMethod<int, int, int>(_ => _.Add).By(
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
            class3GlobalClass.Setup(o =>
            {
                o.DefineField(() => value).As(value);
                o.HideMethod<int, int, int>(_ => _.Add).By(
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
            class3GlobalClass.Setup(o =>
            {
                var simpleType1 = default(SimpleType1);
                o.DefineField(() => simpleType1).As(_ => new SimpleType1());
                o.HideMethod<int, int, int>(_ => _.Add).By(
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
            class3GlobalClass.Setup(o =>
            {
                o.HideMethod<int, int, int>(_ => _.Add).By(
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
            class3GlobalClass.Setup(o =>
            {
                int value = 2;

                o.DefineField(() => value).As(value);
                o.HideMethod<int, int, int>(_ => _.Add).By(
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

    public class GlobalClass4_1 : GlobalClass
    {
        protected override DependencyClass OnRegister()
        {
            var globalClass4Class = new GlobalClass<Class4_1>();
            globalClass4Class.Setup(o =>
            {
                o.BeforeMethod<int, int>(_ => _.Inverse).Run(
                (beforeSource, n) =>
                {
                    TestHelper.WriteLog("sourceType = {0}, sourceMethod = {1}, n = {2}", beforeSource.Type, beforeSource.Method, n);
                });

                o.AfterMethod<int, int>(_ => _.Inverse).Run(
                (afterSource, n, result) =>
                {
                    TestHelper.WriteLog("sourceType = {0}, sourceMethod = {1}, n = {2}, result = {3}", afterSource.Type, afterSource.Method, n, result);
                });
            });
            return globalClass4Class;
        }

        protected override string CodeBase
        {
            get { return typeof(Class4_1).Assembly.CodeBase; }
        }

        protected override string Location
        {
            get { return typeof(Class4_1).Assembly.Location; }
        }
    }
}
