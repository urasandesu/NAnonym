/* 
 * File: Index.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2016 Akira Sugiura
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

namespace Urasandesu.NAnonym.Mixins.System
{
    public struct Index : IEquatable<Index>, IComparable<Index>
    {
        public static readonly Index InvalidValue = NewInvalidValue();

        static Index NewInvalidValue()
        {
            var index = new Index();
            index.Value = -1;
            return index;
        }

        public Index(int value)
            : this()
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("value", value, "The parameter must be equal or larger than 0.");

            Value = value;
        }

        public int Value { get; private set; }

        public static Index New<T>(T obj, Func<T, int> extractor)
        {
            return obj != null ? new Index(extractor(obj)) : InvalidValue;
        }

        public static T GetOrDefault<T>(T[] array)
        {
            throw new NotImplementedException();
        }

        public static T Get<T>(T[] array)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = default(Index?);
            if ((other = obj as Index?) == null)
                return false;

            return ((IEquatable<Index>)this).Equals(other.Value);
        }

        public bool Equals(Index other)
        {
            if (Value != other.Value)
                return false;

            return true;
        }

        public static bool operator ==(Index lhs, Index rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Index lhs, Index rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public int CompareTo(Index other)
        {
            return Value.CompareTo(other.Value);
        }
    }
}

