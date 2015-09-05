/* 
 * File: DelegateMixinTest.cs
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


using NUnit.Framework;
using System;
using System.Reflection.Emit;
using Urasandesu.NAnonym.Mixins.System;

namespace Test.Urasandesu.NAnonym.Mixins.System
{
    [TestFixture]
    public class DelegateMixinTest
    {
        [Test]
        public void Cast_can_cast_same_signature_delegate()
        {
            // Arrange
            var src = (Delegate)new Predicate<int>(i => i == 42);

            // Act
            var dst = src.Cast<Func<int, bool>>(GetType().Module);

            // Assert
            Assert.IsTrue(dst(42));
        }

        
        
        [Test]
        public void Cast_can_cast_same_signature_dynamic_method()
        {
            // Arrange
            var src = default(Delegate);
            {
                var dynMethod = new DynamicMethod("Invoke", typeof(bool), new[] { typeof(int) });
                var gen = dynMethod.GetILGenerator();
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldc_I4_S, (byte)42);
                gen.Emit(OpCodes.Ceq);
                gen.Emit(OpCodes.Ret);
                src = dynMethod.CreateDelegate(typeof(Predicate<int>));
            }

            // Act
            var dst = src.Cast<Func<int, bool>>(GetType().Module);

            // Assert
            Assert.IsTrue(dst(42));
        }
    }
}
