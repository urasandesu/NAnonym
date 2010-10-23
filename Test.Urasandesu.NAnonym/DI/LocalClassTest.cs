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
using Urasandesu.NAnonym.DI;
using System.Xml.Linq;
using System.Xml;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using Test.Urasandesu.NAnonym.Etc;

namespace Test.Urasandesu.NAnonym.ILTools
{
    [TestFixture]
    public class LocalClassTest
    {
        [Test]
        public void Test1()
        {
            var sample2LocalClass = new LocalClass<ISample2>();
            sample2LocalClass.Setup(
            the =>
            {
                the.Method<string, string>(_ => _.Print).IsImplementedBy(
                value =>
                {
                    return value + value + value;
                });
            });

            DependencyUtil.RegisterLocal(sample2LocalClass);
            DependencyUtil.LoadLocal();

            var sample2 = sample2LocalClass.New();

            Assert.AreEqual("aiueoaiueoaiueo", sample2.Print("aiueo"));
        }




        [Test]
        public void Test2()
        {
            var sample3LocalClass = new LocalClass<ISample3>();
            sample3LocalClass.Setup(
            the =>
            {
                the.Method<int, int, int>(_ => _.Add).IsImplementedBy(
                (x, y) =>
                {
                    return x + y;
                });
            });

            DependencyUtil.RegisterLocal(sample3LocalClass);
            DependencyUtil.LoadLocal();

            var sample3 = sample3LocalClass.New();

            Assert.AreEqual(2, sample3.Add(1, 1));
        }




        [Test]
        public void Test3()
        {
            var sample3LocalClass = new LocalClass<ISample3>();
            sample3LocalClass.Setup(
            the =>
            {
                int value = 0;
                the.Field(() => value, 0);
                the.Method<int, int, int>(_ => _.Add).IsImplementedBy(
                (x, y) =>
                {
                    return value += x + y;
                });
            });

            DependencyUtil.RegisterLocal(sample3LocalClass);
            DependencyUtil.LoadLocal();

            var sample3 = sample3LocalClass.New();

            Assert.AreEqual(2, sample3.Add(1, 1));
            Assert.AreEqual(4, sample3.Add(1, 1));
            Assert.AreEqual(6, sample3.Add(1, 1));
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
