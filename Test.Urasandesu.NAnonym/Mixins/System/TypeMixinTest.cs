using System;
using NUnit.Framework;
using Urasandesu.NAnonym.Mixins.System;

namespace Test.Urasandesu.NAnonym.Mixins.System
{
    [TestFixture]
    public class TypeMixinTest
    {
        [Test]
        public void ForciblyNewTest_ShouldReturnInstance_EvenIfNonPublic()
        {
            // Arrange
            // nop

            // Act
            var actual = TypeMixin.ForciblyNew<ClassWithNonPublicConstructor>();

            // Assert
            Assert.IsNotNull(actual);
        }


        [Test]
        public void GetConstructorDelegateTest_ShouldReturnDelegate_IfOverloadExists()
        {
            // Arrange
            // nop

            // Act
            var actual = typeof(ClassWithNonPublicConstructor).GetConstructorDelegate(new Type[] { typeof(double) });

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<ClassWithNonPublicConstructor>(actual.DynamicInvoke(1.1));
        }

        
        
        class ClassWithNonPublicConstructor
        {
            ClassWithNonPublicConstructor() { }
            public ClassWithNonPublicConstructor(int value) { }
            ClassWithNonPublicConstructor(double value) { }
        }
    }
}
