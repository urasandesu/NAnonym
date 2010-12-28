/* 
 * File: MixinTest.cs
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
using Urasandesu.NAnonym.ILTools;
using Test.Urasandesu.NAnonym.Etc;
using Urasandesu.NAnonym;
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.NAnonym.Mixins.System.Reflection;
using System.Reflection;
using System.Linq.Expressions;

namespace Test.Urasandesu.NAnonym
{
    [TestFixture]
    public class MixinTest
    {
        [Test]
        public void GetMethodTest1()
        {
            var dummy = default(Dummy);
            var dummyPrintMethodInfo = TypeSavable.GetStaticMethod<string, string>(() => dummy.Print);
            Assert.IsNotNull(typeof(ISample2).GetMethod(dummyPrintMethodInfo));
        }

        class Dummy
        {
            public string Print(string value)
            {
                throw new NotSupportedException();
            }
        }
    }

    //[TestFixture]
    //public class TypeMixinTest
    //{
    //    [Test]
    //    public void IsImplementationTest1()
    //    {
    //        var type1 = typeof(I1);
    //        var type2 = typeof(NotImplementsI1);

    //        Assert.IsFalse(type2.IsImplementation(type1));
    //    }

    //    [Test]
    //    public void IsImplementationTest2()
    //    {
    //        var type1 = typeof(I1);
    //        var type2 = typeof(ImplementsI1);

    //        Assert.IsTrue(type2.IsImplementation(type1));
    //    }

    //    [Test]
    //    public void IsImplementationTest3()
    //    {
    //        var type1 = typeof(I2);
    //        var type2 = typeof(ImplementsI2);

    //        Assert.IsTrue(type2.IsImplementation(type1));
    //    }

    //    [Test]
    //    public void IsImplementationTest4()
    //    {
    //        var type1 = typeof(I3<>);
    //        var type2 = typeof(ImplementsI3<>);

    //        Assert.IsTrue(type2.IsImplementation(type1));
    //    }

    //    [Test]
    //    public void IsImplementationTest5()
    //    {
    //        var type1 = typeof(IEnumerable<>);
    //        var type2 = typeof(ImplementsCollectionsIEnumerable<>);

    //        Assert.IsFalse(type2.IsImplementation(type1));
    //    }

    //    interface I1
    //    {
    //        void Print(string value);
    //    }

    //    class NotImplementsI1
    //    {
    //        public void Print(string value)
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    class ImplementsI1 : I1
    //    {
    //        public void Print(string value)
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    interface I2
    //    {
    //        I3<T> St<T>(string variableName);    
    //    }

    //    interface I3<T>
    //    {
    //        T As(T value);
    //    }

    //    class ImplementsI2 : I2
    //    {
    //        public I3<T> St<T>(string variableName)
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    class ImplementsI3<T> : I3<T>
    //    {
    //        public T As(T value)
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    interface IEnumerable<T>
    //    {
    //    }

    //    class ImplementsMyIEnumerable<T> : IEnumerable<T>
    //    {
    //    }

    //    class ImplementsCollectionsIEnumerable<T> : System.Collections.Generic.IEnumerable<T>
    //    {
    //        public IEnumerator<T> GetEnumerator()
    //        {
    //            throw new NotImplementedException();
    //        }

    //        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }
    //}

    //[TestFixture]
    //public class MethodInfoMixinTest
    //{
    //    [Test]
    //    public void IsImplementationTest1()
    //    {
    //        var printInfo1 = TypeSavable.GetInstanceMethod<I1, string>(_ => _.Print);
    //        var printInfo2 = TypeSavable.GetInstanceMethod<NotImplementsI1, string>(_ => _.Print);

    //        Assert.IsFalse(printInfo2.IsImplementation(printInfo1));
    //    }

    //    [Test]
    //    public void IsImplementationTest2_1()
    //    {
    //        var printInfo1 = TypeSavable.GetInstanceMethod<I1, string>(_ => _.Print);
    //        var printInfo2 = TypeSavable.GetInstanceMethod<ImplementsI1_1, string>(_ => _.Print);

    //        Assert.IsFalse(printInfo2.IsImplementation(printInfo1));
    //    }

    //    [Test]
    //    public void IsImplementationTest2_2()
    //    {
    //        var printInfo1 = TypeSavable.GetInstanceMethod<I1, string>(_ => _.Print);
    //        var printInfo2 = TypeSavable.GetInstanceMethod<ImplementsI1_2, string>(_ => _.Print);

    //        Assert.IsTrue(printInfo2.IsImplementation(printInfo1));
    //    }

    //    [Test]
    //    public void IsImplementationTest3()
    //    {
    //        var printInfo1 = TypeSavable.GetInstanceMethod<I1, string>(_ => _.Print);
    //        var printInfo2 = typeof(ImplementsI1_1).GetMethod("Test.Urasandesu.NAnonym.MethodInfoMixinTest.I1.Print", BindingFlags.Instance | BindingFlags.NonPublic);

    //        Assert.IsTrue(printInfo2.IsImplementation(printInfo1));
    //    }

    //    [Test]
    //    public void IsImplementationTest4()
    //    {
    //        var ldInfo1 = TypeSavable.GetInstanceMethod<I2, string, object>(_ => _.Ld<object>).GetGenericMethodDefinition();
    //        var ldInfo2 = TypeSavable.GetInstanceMethod<ImplementsI2, string, object>(_ => _.Ld<object>).GetGenericMethodDefinition();

    //        Assert.IsFalse(ldInfo2.IsImplementation(ldInfo1));
    //    }

    //    [Test]
    //    public void IsImplementationTest5()
    //    {
    //        var ldInfo1 = TypeSavable.GetInstanceMethod<I2, string, object>(_ => _.Ld<object>).GetGenericMethodDefinition();
    //        var ldInfo2 = typeof(ImplementsI2).GetMethod("Test.Urasandesu.NAnonym.MethodInfoMixinTest.I2.Ld", BindingFlags.Instance | BindingFlags.NonPublic).GetGenericMethodDefinition();

    //        Assert.IsTrue(ldInfo2.IsImplementation(ldInfo1));
    //    }

    //    [Test]
    //    public void IsImplementationTest6()
    //    {
    //        var ldInfo1 = TypeSavable.GetInstanceMethod<I3, FieldInfo, object>(_ => _.Ld<object>).GetGenericMethodDefinition();
    //        var ldInfo2 = TypeSavable.GetInstanceMethod<ImplementsI3, string, object>(_ => _.Ld<object>).GetGenericMethodDefinition();

    //        Assert.IsFalse(ldInfo2.IsImplementation(ldInfo1));
    //    }

    //    [Test]
    //    public void IsImplementationTest7()
    //    {
    //        var ldInfo1 = TypeSavable.GetInstanceMethod<I2, string, object>(_ => _.Ld<object>).GetGenericMethodDefinition();
    //        var ldInfo2 = TypeSavable.GetInstanceMethod<I3, FieldInfo, object>(_ => _.Ld<object>).GetGenericMethodDefinition();
    //        var ldInfo3 = TypeSavable.GetInstanceMethod<I4, Type, MethodInfo, string, object>(_ => _.Ld<object>).GetGenericMethodDefinition();

    //        var ldInfo4_1 = TypeSavable.GetInstanceMethod<ImplementsI3I4, string, object>(_ => _.Ld<object>).GetGenericMethodDefinition();
    //        var ldInfo4_2 = TypeSavable.GetInstanceMethod<ImplementsI3I4, FieldInfo, object>(_ => _.Ld<object>).GetGenericMethodDefinition();
    //        var ldInfo4_3 = TypeSavable.GetInstanceMethod<ImplementsI3I4, Type, MethodInfo, string, object>(_ => _.Ld<object>).GetGenericMethodDefinition();

    //        Assert.IsTrue(ldInfo4_1.IsImplementation(ldInfo1));
    //        Assert.IsTrue(ldInfo4_2.IsImplementation(ldInfo2));
    //        Assert.IsTrue(ldInfo4_3.IsImplementation(ldInfo3));
    //    }

    //    [Test]
    //    public void IsImplementationTest8()
    //    {
    //        var asInfo1 = typeof(I5<object>).GetMethod("As");
    //        var asInfo2 = typeof(ImplementsI5<int>).GetMethod("As");

    //        Assert.IsTrue(asInfo2.IsImplementation(asInfo1));
    //    }

    //    //[Test]
    //    //public void GetGenericDefinitionOrDefaultTest1()
    //    //{
    //    //    var asInfo1 = TypeSavable.GetInstanceMethod<I5<object>, object, object>(_ => _.As);
    //    //    var asInfo2 = typeof(I5<>).GetMethod("As");

    //    //    Assert.AreEqual(asInfo2, asInfo1.GetGenericDefinitionOrDefault());
    //    //}

    //    interface I1
    //    {
    //        void Print(string value);
    //    }

    //    class NotImplementsI1
    //    {
    //        public void Print(string value)
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    class ImplementsI1_1 : I1
    //    {
    //        public void Print(string value)
    //        {
    //            throw new NotImplementedException();
    //        }

    //        void I1.Print(string value)
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    class ImplementsI1_2 : I1
    //    {
    //        public void Print(string value)
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    interface I2
    //    {
    //        T Ld<T>(string variableName);
    //    }

    //    class ImplementsI2 : I2
    //    {
    //        public T Ld<T>(string variableName)
    //        {
    //            throw new NotImplementedException();
    //        }

    //        T I2.Ld<T>(string variableName)
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    interface I3 : I2
    //    {
    //        T Ld<T>(FieldInfo field);
    //    }

    //    class ImplementsI3 : ImplementsI2, I3
    //    {
    //        public T Ld<T>(FieldInfo field)
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    interface I4 : I2
    //    {
    //        T Ld<T>(Type type, MethodInfo method, string name);
    //    }

    //    class ImplementsI4 : ImplementsI2, I4
    //    {
    //        public T Ld<T>(Type type, MethodInfo method, string name)
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    class ImplementsI3I4 : I3, I4
    //    {
    //        readonly ImplementsI3 implementsI3;
    //        readonly ImplementsI4 implementsI4;
    //        public ImplementsI3I4(ImplementsI3 implementsI3, ImplementsI4 implementsI4)
    //        {
    //            this.implementsI3 = implementsI3;
    //            this.implementsI4 = implementsI4;
    //        }

    //        public T Ld<T>(FieldInfo field)
    //        {
    //            return implementsI3.Ld<T>(field);
    //        }

    //        public T Ld<T>(string variableName)
    //        {
    //            return implementsI3.Ld<T>(variableName);
    //        }

    //        public T Ld<T>(Type type, MethodInfo method, string name)
    //        {
    //            return implementsI4.Ld<T>(type, method, name);
    //        }
    //    }

    //    interface I5<T>
    //    {
    //        T As(T value);
    //    }

    //    class ImplementsI5<T> : I5<T>
    //    {
    //        public T As(T value)
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }
    //}
}

