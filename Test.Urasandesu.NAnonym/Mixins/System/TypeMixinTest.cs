/* 
 * File: TypeMixinTest.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2012 Akira Sugiura
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
using NUnit.Framework;
using Urasandesu.NAnonym.Mixins.System;
using System.Runtime.Serialization;

namespace Test.Urasandesu.NAnonym.Mixins.System
{
    [TestFixture]
    public class TypeMixinTest
    {
        [Test]
        public void ForciblyNew_should_return_instance_even_if_NonPublic()
        {
            // Arrange
            // nop

            // Act
            var actual = TypeMixin.ForciblyNew<ClassWithNonPublicConstructor>();

            // Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        public void ForciblyNew_should_return_null_if_specified_signature_constructor_does_not_exist()
        {
            // Arrange
            // nop

            // Act
            var actual = typeof(ClassWithNonPublicConstructor).ForciblyNew(42m);

            // Assert
            Assert.IsNull(actual);
        }

        [Test]
        public void ForciblyNew_should_throw_ArgumentNullException_if_null_is_passed_as_t()
        {
            // Arrange
            // nop

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => TypeMixin.ForciblyNew(null, new object[0]));
        }



        [Test]
        public void GetMemberDelegate_should_return_creation_delegate_if_condition_for_its_constructor_is_passed()
        {
            // Arrange
            // nop

            // Act
            var actual = typeof(ClassWithNonPublicConstructor).GetMemberDelegate(".ctor", new Type[] { typeof(double) });

            // Assert
            Assert.IsNotNull(actual);
            var obj = actual(null, new object[] { 1.1 });
            Assert.IsInstanceOf<ClassWithNonPublicConstructor>(obj);
            Assert.AreEqual(1.1, ((ClassWithNonPublicConstructor)obj).Value);
        }


        [Test]
        public void GetMemberDelegate_should_return_placement_delegate_if_condition_for_its_constructor_is_passed()
        {
            // Arrange
            var target = (ClassWithNonPublicConstructor)FormatterServices.GetUninitializedObject(typeof(ClassWithNonPublicConstructor));

            // Act
            var actual = typeof(ClassWithNonPublicConstructor).GetMemberDelegate(".ctor", new Type[] { typeof(double) });

            // Assert
            Assert.IsNotNull(actual);
            var obj = actual(target, new object[] { 1.1 });
            Assert.IsInstanceOf<ClassWithNonPublicConstructor>(obj);
            Assert.AreSame(target, obj);
            Assert.AreEqual(1.1, target.Value);
        }


        [Test]
        public void GetMemberDelegate_should_return_field_getter_if_condition_for_its_field_is_passed()
        {
            // Arrange
            var target = new ClassWithNonPublicField();

            // Act
            var actual = typeof(ClassWithNonPublicField).GetMemberDelegate("m_value", Type.EmptyTypes);
            target.Value = 42;

            // Assert
            Assert.AreEqual(42, actual(target, null));
        }


        [Test]
        public void GetMemberDelegate_should_return_field_setter_if_condition_for_its_field_is_passed()
        {
            // Arrange
            var target = new ClassWithNonPublicField();

            // Act
            var actual = typeof(ClassWithNonPublicField).GetMemberDelegate("m_value", Type.EmptyTypes);
            actual(target, new object[] { 42 });

            // Assert
            Assert.AreEqual(42, target.Value);
        }


        [Test]
        public void GetMemberDelegate_should_return_method_delegate_if_condition_for_its_method_is_passed()
        {
            // Arrange
            var target = new ClassWithNonPublicMethod();

            // Act
            var actual = typeof(ClassWithNonPublicMethod).GetMemberDelegate("Add", new Type[] { typeof(double), typeof(double) });

            // Assert
            Assert.IsNotNull(actual);
            Assert.LessOrEqual(Math.Abs(2.2 - (double)actual(target, new object[] { 1.1, 1.1 })), 0.001);
        }


        [Test]
        public void GetMemberDelegate_should_return_property_getter_if_condition_for_its_property_is_passed()
        {
            // Arrange
            var target = new ClassWithNonPublicProperty();

            // Act
            var actual = typeof(ClassWithNonPublicProperty).GetMemberDelegate("Value", Type.EmptyTypes);
            target.m_value = 42;

            // Assert
            Assert.AreEqual(42, actual(target, null));
        }


        [Test]
        public void GetMemberDelegate_should_return_property_setter_if_condition_for_its_property_is_passed()
        {
            // Arrange
            var target = new ClassWithNonPublicProperty();

            // Act
            var actual = typeof(ClassWithNonPublicProperty).GetMemberDelegate("Value", Type.EmptyTypes);
            actual(target, new object[] { 42 });

            // Assert
            Assert.AreEqual(42, target.m_value);
        }


        [Test]
        public void GetConstructorDelegate_should_return_delegate_if_value_overload_exists()
        {
            // Arrange
            // nop

            // Act
            var actual = typeof(ClassWithNonPublicConstructor).GetConstructorDelegate(new Type[] { typeof(double) });

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<ClassWithNonPublicConstructor>(actual(null, new object[] { 1.1 }));
        }


        [Test]
        public void GetConstructorDelegate_should_return_delegate_if_Value_ByRef_overload_exists()
        {
            // Arrange
            // nop

            // Act
            var actual = typeof(ClassWithNonPublicConstructor).GetConstructorDelegate(new Type[] { typeof(int).MakeByRefType() });

            // Assert
            Assert.IsNotNull(actual);
            var args = new object[] { 32 };
            Assert.IsInstanceOf<ClassWithNonPublicConstructor>(actual(null, args));
            Assert.AreEqual(42, args[0]);
        }


        [Test]
        public void GetConstructorDelegate_should_return_delegate_if_overload_exists()
        {
            // Arrange
            // nop

            // Act
            var actual = typeof(ClassWithNonPublicConstructor).GetConstructorDelegate(new Type[] { typeof(Type[]) });

            // Assert
            Assert.IsNotNull(actual);
            var args = new object[] { new Type[] { typeof(int) } };
            Assert.IsInstanceOf<ClassWithNonPublicConstructor>(actual(null, args));
        }


        [Test]
        public void GetFieldGetterDelegate_should_return_getter_if_exists()
        {
            // Arrange
            var target = new ClassWithNonPublicField();

            // Act
            var get_m_value = typeof(ClassWithNonPublicField).GetFieldGetterDelegate("m_value");
            target.Value = 42;

            // Assert
            Assert.AreEqual(42, get_m_value(target, null));
        }


        [Test]
        public void GetFieldGetterDelegate_should_return_getter_if_static()
        {
            // Arrange
            // nop

            // Act
            var get_ms_staticValue = typeof(ClassWithNonPublicField).GetFieldGetterDelegate("ms_staticValue");
            ClassWithNonPublicField.StaticValue = 42;

            // Assert
            Assert.AreEqual(42, get_ms_staticValue(null, null));
        }


        [Test]
        public void GetFieldSetterDelegate_should_return_setter_if_exists()
        {
            // Arrange
            var target = new ClassWithNonPublicField();

            // Act
            var set_m_value = typeof(ClassWithNonPublicField).GetFieldSetterDelegate("m_value");
            set_m_value(target, new object[] { 42 });

            // Assert
            Assert.AreEqual(42, target.Value);
        }


        [Test]
        public void GetFieldSetterDelegate_should_return_setter_if_static()
        {
            // Arrange
            // nop

            // Act
            var set_ms_staticValue = typeof(ClassWithNonPublicField).GetFieldSetterDelegate("ms_staticValue");
            set_ms_staticValue(null, new object[] { 10 });

            // Assert
            Assert.AreEqual(10, ClassWithNonPublicField.StaticValue);
        }


        [Test]
        public void GetMethodDelegate_should_return_delegate_if_value_overload_exists()
        {
            // Arrange
            var target = new ClassWithNonPublicMethod();

            // Act
            var actual = typeof(ClassWithNonPublicMethod).GetMethodDelegate("Add", new Type[] { typeof(double), typeof(double) });

            // Assert
            Assert.IsNotNull(actual);
            Assert.LessOrEqual(Math.Abs(2.2 - (double)actual(target, new object[] { 1.1, 1.1 })), 0.001);
        }


        [Test]
        public void GetMethodDelegate_should_return_delegate_if_value_ByRef_overload_exists()
        {
            // Arrange
            var target = new ClassWithNonPublicMethod();

            // Act
            var actual = typeof(ClassWithNonPublicMethod).GetMethodDelegate("Add", new Type[] { typeof(double), typeof(double), typeof(double).MakeByRefType() });

            // Assert
            Assert.IsNotNull(actual);
            var args = new object[] { 1.1, 2.2, 0.0 };
            actual(target, args);
            Assert.LessOrEqual(Math.Abs(3.3 - (double)args[2]), 0.001);
        }


        [Test]
        public void GetMethodDelegate_should_return_delegate_if_overload_exists()
        {
            // Arrange
            var target = new ClassWithNonPublicMethod();

            // Act
            var actual = typeof(ClassWithNonPublicMethod).GetMethodDelegate("TryParse", new Type[] { typeof(string), typeof(ClassWithNonPublicMethod).MakeByRefType() });

            // Assert
            Assert.IsNotNull(actual);

            var args = new object[] { };

            args = new object[] { "failed", null };
            Assert.IsFalse((bool)actual(target, args));
            Assert.IsNull(args[1]);

            args = new object[] { "ClassWithNonPublicMethod", null };
            Assert.IsTrue((bool)actual(target, args));
            Assert.IsNotNull(args[1]);
        }


        [Test]
        public void GetMethodDelegate_should_return_delegate_if_static()
        {
            // Arrange
            // nop

            // Act
            var actual = typeof(ClassWithNonPublicMethod).GetMethodDelegate("Convert", new Type[] { typeof(string), typeof(Exception).MakeByRefType() });

            // Assert
            Assert.IsNotNull(actual);

            var result = default(ClassWithNonPublicMethod);
            var args = new object[] { };

            args = new object[] { "failed", null };
            result = (ClassWithNonPublicMethod)actual(null, args);
            Assert.IsNull(result);
            Assert.IsInstanceOf<InvalidCastException>(args[1]);

            args = new object[] { "ClassWithNonPublicMethod", null };
            result = (ClassWithNonPublicMethod)actual(null, args);
            Assert.IsNotNull(result);
            Assert.IsNull(args[1]);
        }


        [Test]
        public void GetPropertyGetterDelegate_should_return_getter_if_exists()
        {
            // Arrange
            var target = new ClassWithNonPublicProperty();

            // Act
            var get_Value = typeof(ClassWithNonPublicProperty).GetPropertyGetterDelegate("Value");
            target.m_value = 42;

            // Assert
            Assert.AreEqual(42, get_Value(target, null));
        }


        [Test]
        public void GetPropertyGetterDelegate_should_return_getter_if_static()
        {
            // Arrange
            // nop

            // Act
            var get_StaticValue = typeof(ClassWithNonPublicProperty).GetPropertyGetterDelegate("StaticValue");
            ClassWithNonPublicProperty.ms_staticValue = 42;

            // Assert
            Assert.AreEqual(42, get_StaticValue(null, null));
        }


        [Test]
        public void GetPropertySetterDelegate_should_return_setter_if_exists()
        {
            // Arrange
            var target = new ClassWithNonPublicProperty();

            // Act
            var set_Value = typeof(ClassWithNonPublicProperty).GetPropertySetterDelegate("Value");
            set_Value(target, new object[] { 42 });

            // Assert
            Assert.AreEqual(42, target.m_value);
        }


        [Test]
        public void GetPropertySetterDelegate_should_return_setter_if_static()
        {
            // Arrange
            // nop

            // Act
            var set_StaticValue = typeof(ClassWithNonPublicProperty).GetPropertySetterDelegate("StaticValue");
            set_StaticValue(null, new object[] { 10 });

            // Assert
            Assert.AreEqual(10, ClassWithNonPublicProperty.ms_staticValue);
        }



        class ClassWithNonPublicConstructor
        {
            public object Value { get; private set; }
            ClassWithNonPublicConstructor() { }
            ClassWithNonPublicConstructor(double value) { Value = value; }
            ClassWithNonPublicConstructor(ref int value) { value = value + 10; Value = value; }
            ClassWithNonPublicConstructor(Type[] types) { Value = types; }
        }

        class ClassWithNonPublicField
        {
            int m_value;
            public int Value { get { return m_value; } set { m_value = value; } }
            static int ms_staticValue;
            public static int StaticValue { get { return ms_staticValue; } set { ms_staticValue = value; } }
        }

        class ClassWithNonPublicMethod
        {
            double Add(double x, double y)
            {
                return x + y;
            }

            void Add(double x, double y, out double result)
            {
                result = x + y;
            }

            bool TryParse(string s, out ClassWithNonPublicMethod result)
            {
                if (s == typeof(ClassWithNonPublicMethod).Name)
                {
                    result = new ClassWithNonPublicMethod();
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }

            static ClassWithNonPublicMethod Convert(string s, out Exception innerException)
            {
                if (s == typeof(ClassWithNonPublicMethod).Name)
                {
                    innerException = null;
                    return new ClassWithNonPublicMethod();
                }
                else
                {
                    innerException = new InvalidCastException();
                    return null;
                }
            }
        }

        class ClassWithNonPublicProperty
        {
            public int m_value;
            int Value { get { return m_value; } set { m_value = value; } }
            public static int ms_staticValue;
            static int StaticValue { get { return ms_staticValue; } set { ms_staticValue = value; } }
        }


        [Test]
        public void Default_should_return_default_value_if_value_type_is_passed()
        {
            // Arrange
            // nop

            // Act
            var actual = typeof(DateTime).Default();

            // Assert
            Assert.AreEqual(default(DateTime), actual);
        }


        [Test]
        public void Default_should_return_default_value_if_class_type_is_passed()
        {
            // Arrange
            // nop

            // Act
            var actual = typeof(string).Default();

            // Assert
            Assert.IsNull(actual);
        }



        [Test]
        public void MakeGenericType_should_replace_all_generic_parameter()
        {
            // Arrange
            var declType = typeof(TypeAndMethod<>);
            var declMethod = declType.GetMethod("Foo");
            var t = typeof(Func<,,,>).MakeGenericType(typeof(int), typeof(DateTime), declType.GetGenericArguments()[0], declMethod.GetGenericArguments()[0]);

            // Act
            var result = t.MakeGenericType(declType, new[] { typeof(decimal) }, declMethod, new[] { typeof(string) });

            // Assert
            Assert.AreEqual(typeof(Func<int, DateTime, decimal, string>), result);
        }

        [Test]
        public void MakeGenericType_should_replace_element_type()
        {
            // Arrange
            var declType = typeof(TypeAndMethod<>);
            var declMethod = declType.GetMethod("Foo");

            var arrType = declType.GetGenericArguments()[0].MakeArrayType();
            var arrGenericType = typeof(Action<>).MakeGenericType(arrType);
            var genericArrType = typeof(Action<>).MakeGenericType(declType.GetGenericArguments()[0]).MakeArrayType();
            var mdArrType = declMethod.GetGenericArguments()[0].MakeArrayType(2);
            var arrArrType = declMethod.GetGenericArguments()[0].MakeArrayType().MakeArrayType();

            var t = typeof(Func<,,,,>).MakeGenericType(arrType, arrGenericType, genericArrType, mdArrType, arrArrType);

            // Act
            var result = t.MakeGenericType(declType, new[] { typeof(decimal) }, declMethod, new[] { typeof(string) });

            // Assert
            Assert.AreEqual(typeof(Func<decimal[], Action<decimal[]>, Action<decimal>[], string[,], string[][]>), result);
        }

        class TypeAndMethod<T>
        {
            public static M Foo<M>(int i, DateTime dt, T t)
            {
                // `TypeMixin.MakeGenericType(Type, Type, Type[], MethodBase, Type[])` is intended for getting the generic instance that is parcially 
                // specified generic parameters at runtime. 
                // For example, the above `MakeGenericType_should_replace_all_generic_parameter` simulates getting `typeof(Func<int, DateTime, T, M>)` here.
                throw new NotImplementedException();
            }
        }
    }
}
