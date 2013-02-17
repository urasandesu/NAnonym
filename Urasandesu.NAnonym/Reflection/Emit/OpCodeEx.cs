/* 
 * File: OpCodeEx.cs
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



namespace Urasandesu.NAnonym.Reflection.Emit
{
    public struct OpCodeEx
    {
        public OpCodeEx(string name, byte length, byte byte1, byte byte2)
        {
            this.m_name = name;
            this.m_length = length;
            this.m_byte1 = byte1;
            this.m_byte2 = byte2;
        }

        public static bool operator !=(OpCodeEx a, OpCodeEx b)
        {
            return !(a == b);
        }

        public static bool operator ==(OpCodeEx a, OpCodeEx b)
        {
            return a.Value == b.Value;
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var value = obj as OpCodeEx?;
            return value == null ? false : this == value;
        }

        string m_name;
        byte m_length;
        byte m_byte1;
        byte m_byte2;

        public string Name { get { return m_name; } }
        public byte Length { get { return m_length; } }
        public byte Byte1 { get { return m_byte1; } }
        public byte Byte2 { get { return m_byte2; } }
        public short Value
        {
            get
            {
                return m_byte1 == 0xff ? m_byte2 : (short)((int)m_byte2 << 8 | m_byte1);
            }
        }
    }
}
