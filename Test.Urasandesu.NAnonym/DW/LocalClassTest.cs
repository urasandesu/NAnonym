/* 
 * File: LocalClassTest.cs
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
using System.IO;
using System.Text;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Urasandesu.NAnonym;
using Urasandesu.NAnonym.Linq;
using Urasandesu.NAnonym.DW;
using System.Xml.Linq;
using System.Xml;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using Test.Urasandesu.NAnonym.Etc;

namespace Test.Urasandesu.NAnonym.DW
{
    [TestFixture]
    public class LocalClassTest
    {
        [SetUp]
        public void SetUp()
        {
            LocalDomain.Revert();
        }

        [Test]
        public void Test1()
        {
            var sample2LocalClass1 = new LocalClass<ISample2>();
            sample2LocalClass1.Setup(o =>
            {
                o.ImplementMethod<string, string>(_ => _.Print).By(
                value =>
                {
                    return value + value + value;
                });
            });

            var sample2LocalClass2 = new LocalClass<ISample2>();
            sample2LocalClass2.Setup(o =>
            {
                o.ImplementMethod<string, string>(_ => _.Print).By(
                value =>
                {
                    return value + " Hello, World!!";
                });
            });

            LocalDomain.Register(sample2LocalClass1);
            LocalDomain.Register(sample2LocalClass2);
            LocalDomain.Load();

            var sample21 = sample2LocalClass1.New();
            Assert.AreEqual("aiueoaiueoaiueo", sample21.Print("aiueo"));

            var sample22 = sample2LocalClass2.New();
            Assert.AreEqual("aiueo Hello, World!!", sample22.Print("aiueo"));
        }




        [Test]
        public void Test2()
        {
            var sample3LocalClass = new LocalClass<ISample3>();
            sample3LocalClass.Setup(o =>
            {
                o.ImplementMethod<int, int, int>(_ => _.Add).By(
                (x, y) =>
                {
                    return x + y;
                });
            });

            LocalDomain.Register(sample3LocalClass);
            LocalDomain.Load();

            var sample3 = sample3LocalClass.New();

            Assert.AreEqual(2, sample3.Add(1, 1));
        }




        [Test]
        public void Test3()
        {
            var sample3LocalClass = new LocalClass<ISample3>();
            sample3LocalClass.Setup(o =>
            {
                var value = default(int);
                o.DefineField(() => value).As(1);
                o.ImplementMethod<int, int, int>(_ => _.Add).By(
                (x, y) =>
                {
                    return value += x + y;
                });
            });

            LocalDomain.Register(sample3LocalClass);
            LocalDomain.Load();

            var sample3 = sample3LocalClass.New();

            Assert.AreEqual(3, sample3.Add(1, 1));
            Assert.AreEqual(5, sample3.Add(1, 1));
            Assert.AreEqual(7, sample3.Add(1, 1));
        }




        [Test]
        public void Test4()
        {
            var sample3LocalClass = new LocalClass<Sample3>();
            sample3LocalClass.Setup(o =>
            {
                o.OverrideMethod<int, int, int>(_ => _.Add).By(
                (base_Add, x, y) =>
                {
                    return base_Add(x, y) + x + y;
                });
            });

            LocalDomain.Register(sample3LocalClass);
            LocalDomain.Load();

            var sample3 = sample3LocalClass.New();

            Assert.AreEqual(4, sample3.Add(1, 1));
        }


        //[Test]
        //public void Hoge()
        //{
        //    var localClass = new LocalClass<IHoge>();
        //    // MEMO: アイディア降りてきた！Setup -> Method -> Override or Instead ってのはどう？
        //    // MEMO: よし！戻り値を明示しなくても型推論されるようになって、若干必要な指定が減りました！
        //    localClass.Setup(the =>
        //    {
        //        the.Method(() =>
        //        {
        //            Console.WriteLine("Hello, World!!");
        //        }).
        //        Implement(_ => _.Output);

        //        the.Method(() =>
        //        {
        //            return "Hello, Local Class !!";
        //        }).
        //        Implement(_ => _.Print);

        //        the.Method((string content) =>
        //        {
        //            return "Hello, " + content + " World !!";
        //        }).
        //        Implement(_ => _.Print);


        //        int this_value = 0;
        //        the.Property(() =>
        //        {
        //            return this_value;
        //        }).
        //        Implement(_ => () => _.Value);

        //        the.Property((int value) =>
        //        {
        //            this_value = value * 2;
        //        }).
        //        Implement(_ => value => _.Value = value);
        //    });

        //    //localClass.Override(the =>
        //    //{
        //    //    the.Method(_ => _.Output).As(
        //    //    () =>
        //    //    {
        //    //        Console.WriteLine("Hello, World!!");
        //    //    });

        //    //    the.Method<string>(_ => _.Print).As(
        //    //    () =>
        //    //    {
        //    //        return "Hello, Local Class !!";
        //    //    });

        //    //    the.Method<string, string>(_ => _.Print).As(
        //    //    (string content) =>
        //    //    {
        //    //        return "Hello, " + content + " World !!";
        //    //    });

        //    //    // MEMO: プロパティは先にテスター作るしかなさげ
        //    //    // MEMO: インデックス付きはどうするのだ → index 付きの overload 用意するしかなさげ
        //    //    int this_value = 0;
        //    //    the.Property<int>(_ => value => _.Value = value).As(value => { this_value = value; });
        //    //    the.Property<int>(_ => () => _.Value).As(() => this_value * 2);
        //    //});

        //    localClass.Load();

        //    var hoge = localClass.New();
        //    hoge.Value = 10;
        //    Assert.AreEqual(20, hoge.Value);

        //    Assert.AreEqual("Hello, Local Class !!", hoge.Print());
        //    Assert.AreEqual("Hello, Local Class World !!", hoge.Print("Local Class"));
        //}
    }

    interface IHoge
    {
        int Value { get; set; }
        void Output();
        string Print();
        string Print(string content);
    }


}
