/* 
 * File: ProcessMixinTest.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2017 Akira Sugiura
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
using Urasandesu.NAnonym.Mixins.System.Diagnostics;

namespace Test.Urasandesu.NAnonym.Mixins.System.Diagnostics
{
    [TestFixture]
    public class ProcessMixinTest
    {
        [Test]
        public void ConvertCommandLineArgsToArguments_should_enclose_by_double_quote_and_escape_if_an_arg_contains_double_quote()
        {
            // Arrange
            var commandLineArgs = new[] { "a", "b\"b" };

            // Act
            var result = ProcessMixin.ConvertCommandLineArgsToArguments(commandLineArgs);

            // Assert
            Assert.AreEqual("\"b\\\"b\"", result);
        }

        [Test]
        public void ConvertCommandLineArgsToArguments_should_enclose_by_double_quote_if_an_arg_contains_space()
        {
            // Arrange
            var commandLineArgs = new[] { "a", "b b" };

            // Act
            var result = ProcessMixin.ConvertCommandLineArgsToArguments(commandLineArgs);

            // Assert
            Assert.AreEqual("\"b b\"", result);
        }

        [Test]
        public void ConvertCommandLineArgsToArguments_should_enclose_by_double_quote_if_an_arg_contains_tab()
        {
            // Arrange
            var commandLineArgs = new[] { "a", "b\tb" };

            // Act
            var result = ProcessMixin.ConvertCommandLineArgsToArguments(commandLineArgs);

            // Assert
            Assert.AreEqual("\"b\tb\"", result);
        }

        [Test]
        public void ConvertCommandLineArgsToArguments_should_not_enclose_by_double_quote_if_there_is_no_delimiter()
        {
            // Arrange
            var commandLineArgs = new[] { "a", "bbb" };

            // Act
            var result = ProcessMixin.ConvertCommandLineArgsToArguments(commandLineArgs);

            // Assert
            Assert.AreEqual("bbb", result);
        }

        [Test]
        public void ConvertCommandLineArgsToArguments_should_ignore_the_first_element()
        {
            // Arrange
            var commandLineArgs = new[] { "a a a" };

            // Act
            var result = ProcessMixin.ConvertCommandLineArgsToArguments(commandLineArgs);

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void ConvertCommandLineArgsToArguments_should_return_empty_if_passed_parameter_is_empty()
        {
            // Arrange
            var commandLineArgs = new string[0];

            // Act
            var result = ProcessMixin.ConvertCommandLineArgsToArguments(commandLineArgs);

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void ConvertCommandLineArgsToArguments_should_throw_ArgumentNullException_if_passed_parameter_is_null()
        {
            // Arrange
            var commandLineArgs = default(string[]);

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => ProcessMixin.ConvertCommandLineArgsToArguments(commandLineArgs));
        }
    }
}
