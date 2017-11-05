/* 
 * File: StringMixin.cs
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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.Mixins.System
{
    public static class StringMixin
    {
        public static string ToCommandLineArgument(this string @this)
        {
            if (@this == null)
                throw new ArgumentNullException("this");

            if (-1 < @this.IndexOfAny(new[] { '\"', ' ', '\t' }))
                return new StringBuilder(@this).Replace("\"", "\\\"").Insert(0, "\"").Append("\"").ToString();
            else
                return @this;
        }

        public static string EncodeEnclosure(this string @this)
        {
            if (@this == null)
                throw new ArgumentNullException("this");

            return @this.NullableEncodeEnclosure();
        }

        public static string NullableEncodeEnclosure(this string @this)
        {
            if (@this == null)
                return null;

            return new StringBuilder(@this).Replace("\"", "\\\"").ToString();
        }

        public static string ToNullVisibleString(this string @this)
        {
            if (@this == null)
                return "null";

            return new StringBuilder(@this).Insert(0, "\"").Append("\"").ToString();
        }

        public static string JoinIfNotNullOrEmpty(string separator, IEnumerable<string> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            return string.Join(separator, values.Where(_ => !string.IsNullOrEmpty(_)).ToArray());
        }

        public static string JoinIfNotNullOrEmpty<T>(string separator, IEnumerable<T> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            return string.Join(separator, values.Select(_ => _ + "").Where(_ => !string.IsNullOrEmpty(_)).ToArray());
        }
    }
}
