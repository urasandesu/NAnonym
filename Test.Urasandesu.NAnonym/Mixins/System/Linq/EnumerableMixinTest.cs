/* 
 * File: EnumerableMixinTest.cs
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


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Urasandesu.NAnonym.Mixins.System.Collections.Generic;
using Urasandesu.NAnonym.Mixins.System.Linq;

namespace Test.Urasandesu.NAnonym.Mixins.System.Linq
{
    [TestFixture]
    public class EnumerableMixinTest
    {
        [Test]
        public void MutableForEachTest_ShouldEnumerateEntryAsMutable1()
        {
            // Arrange
            var entries = new Hashtable();
            entries.Add("a", 1);
            entries.Add("d", 4);
            entries.Add("b", 2);
            entries.Add("c", 3);
            entries.Add("e", 5);

            // Act
            entries.MutableForEach((@this, entry) =>
            {
                @this[entry.Key] = (int)@this[entry.Key] * (int)@this[entry.Key];
            });

            // Assert
            CollectionAssert.AreEqual(
                new int[] { 1, 4, 9, 16, 25 }.AsEnumerable(),
                entries.OfType<DictionaryEntry>().Select(_ => (int)_.Value).OrderBy(_ => _)
            );
        }


        [Test]
        public void MutableForEachTest_ShouldEnumerateEntryAsMutable2()
        {
            // Arrange
            var entries = new Dictionary<string, int>();
            entries.Add("a", 1);
            entries.Add("d", 4);
            entries.Add("b", 2);
            entries.Add("c", 3);
            entries.Add("e", 5);

            // Act
            entries.MutableForEach((@this, entry) =>
            {
                @this[entry.Key] = @this[entry.Key] * @this[entry.Key];
            });

            // Assert
            CollectionAssert.AreEqual(
                new int[] { 1, 4, 9, 16, 25 }.AsEnumerable(),
                entries.Select(_ => _.Value).OrderBy(_ => _)
            );
        }

        
        [Test]
        public void AddRangeTest_ShouldAddAllElement_IfDistinctDictionaryIsPassed()
        {
            // Arrange
            var dictionary = new Dictionary<string, int>();
            dictionary.Add("a", 1);
            dictionary.Add("d", 4);
            dictionary.Add("b", 2);
            dictionary.Add("c", 3);
            dictionary.Add("e", 5);
            var target = new Dictionary<string, int>();

            // Act
            target.AddRange(dictionary);

            // Assert
            CollectionAssert.AreEqual(
                new int[] { 1, 2, 3, 4, 5 }.AsEnumerable(),
                target.Union(dictionary, (_1, _2) => _1.Is(_2)).
                       OrderBy(_ => _.Key).
                       Select(_=>_.Value)
            );
        }
    }
}
