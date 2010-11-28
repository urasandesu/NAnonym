/* 
 * File: ConsoleProxy.cs
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
using System.IO;

namespace Urasandesu.NAnonym.Test
{
    public class ConsoleProxy : MarshalByRefObject
    {
        public ConsoleColor BackgroundColor { get { return Console.BackgroundColor; } set { Console.BackgroundColor = value; } }
        public int BufferHeight { get { return Console.BufferHeight; } set { Console.BufferHeight = value; } }
        public int BufferWidth { get { return Console.BufferWidth; } set { Console.BufferWidth = value; } }
        public bool CapsLock { get { return Console.CapsLock; } }
        public int CursorLeft { get { return Console.CursorLeft; } set { Console.CursorLeft = value; } }
        public int CursorSize { get { return Console.CursorSize; } set { Console.CursorSize = value; } }
        public int CursorTop { get { return Console.CursorTop; } set { Console.CursorTop = value; } }
        public bool CursorVisible { get { return Console.CursorVisible; } set { Console.CursorVisible = value; } }
        public TextWriter Error { get { return Console.Error; } }
        public ConsoleColor ForegroundColor { get { return Console.ForegroundColor; } set { Console.ForegroundColor = value; } }
        public TextReader In { get { return Console.In; } }
        public Encoding InputEncoding { get { return Console.InputEncoding; } set { Console.InputEncoding = value; } }
        public bool KeyAvailable { get { return Console.KeyAvailable; } }
        public int LargestWindowHeight { get { return Console.LargestWindowHeight; } }
        public int LargestWindowWidth { get { return Console.LargestWindowWidth; } }
        public bool NumberLock { get { return Console.NumberLock; } }
        public TextWriter Out { get { return Console.Out; } }
        public Encoding OutputEncoding { get { return Console.OutputEncoding; } set { Console.OutputEncoding = value; } }
        public string Title { get { return Console.Title; } set { Console.Title = value; } }
        public bool TreatControlCAsInput { get { return Console.TreatControlCAsInput; } set { Console.TreatControlCAsInput = value; } }
        public int WindowHeight { get { return Console.WindowHeight; } set { Console.WindowHeight = value; } }
        public int WindowLeft { get { return Console.WindowLeft; } set { Console.WindowLeft = value; } }
        public int WindowTop { get { return Console.WindowTop; } set { Console.WindowTop = value; } }
        public int WindowWidth { get { return Console.WindowWidth; } set { Console.WindowWidth = value; } }
        public event ConsoleCancelEventHandler CancelKeyPress { add { Console.CancelKeyPress += value; } remove { Console.CancelKeyPress -= value; } }
        public void Beep() { Console.Beep(); }
        public void Beep(int frequency, int duration) { Console.Beep(frequency, duration); }
        public void Clear() { Console.Clear(); }
        public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop) { Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop); }
        public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor) { Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop, sourceChar, sourceForeColor, sourceBackColor); }
        public Stream OpenStandardError() { return Console.OpenStandardError(); }
        public Stream OpenStandardError(int bufferSize) { return Console.OpenStandardError(bufferSize); }
        public Stream OpenStandardInput() { return Console.OpenStandardInput(); }
        public Stream OpenStandardInput(int bufferSize) { return Console.OpenStandardInput(bufferSize); }
        public Stream OpenStandardOutput() { return Console.OpenStandardOutput(); }
        public Stream OpenStandardOutput(int bufferSize) { return Console.OpenStandardOutput(bufferSize); }
        public int Read() { return Console.Read(); }
        public ConsoleKeyInfo ReadKey() { return Console.ReadKey(); }
        public ConsoleKeyInfo ReadKey(bool intercept) { return Console.ReadKey(intercept); }
        public string ReadLine() { return Console.ReadLine(); }
        public void ResetColor() { Console.ResetColor(); }
        public void SetBufferSize(int width, int height) { Console.SetBufferSize(width, height); }
        public void SetCursorPosition(int left, int top) { Console.SetCursorPosition(left, top); }
        public void SetError(TextWriter newError) { Console.SetError(newError); }
        public void SetIn(TextReader newIn) { Console.SetIn(newIn); }
        public void SetOut(TextWriter newOut) { Console.SetOut(newOut); }
        public void SetWindowPosition(int left, int top) { Console.SetWindowPosition(left, top); }
        public void SetWindowSize(int width, int height) { Console.SetWindowSize(width, height); }
        public void Write(bool value) { Console.Write(value); }
        public void Write(char value) { Console.Write(value); }
        public void Write(char[] buffer) { Console.Write(buffer); }
        public void Write(decimal value) { Console.Write(value); }
        public void Write(double value) { Console.Write(value); }
        public void Write(float value) { Console.Write(value); }
        public void Write(int value) { Console.Write(value); }
        public void Write(long value) { Console.Write(value); }
        public void Write(object value) { Console.Write(value); }
        public void Write(string value) { Console.Write(value); }
        public void Write(uint value) { Console.Write(value); }
        public void Write(ulong value) { Console.Write(value); }
        public void Write(string format, object arg0) { Console.Write(format, arg0); }
        public void Write(string format, params object[] arg) { Console.Write(format, arg); }
        public void Write(char[] buffer, int index, int count) { Console.Write(buffer, index, count); }
        public void Write(string format, object arg0, object arg1) { Console.Write(format, arg0, arg1); }
        public void Write(string format, object arg0, object arg1, object arg2) { Console.Write(format, arg0, arg1, arg2); }
        public void Write(string format, object arg0, object arg1, object arg2, object arg3) { Console.Write(format, arg0, arg1, arg2, arg3); }
        public void WriteLine() { Console.WriteLine(); }
        public void WriteLine(bool value) { Console.WriteLine(value); }
        public void WriteLine(char value) { Console.WriteLine(value); }
        public void WriteLine(char[] buffer) { Console.WriteLine(buffer); }
        public void WriteLine(decimal value) { Console.WriteLine(value); }
        public void WriteLine(double value) { Console.WriteLine(value); }
        public void WriteLine(float value) { Console.WriteLine(value); }
        public void WriteLine(int value) { Console.WriteLine(value); }
        public void WriteLine(long value) { Console.WriteLine(value); }
        public void WriteLine(object value) { Console.WriteLine(value); }
        public void WriteLine(string value) { Console.WriteLine(value); }
        public void WriteLine(uint value) { Console.WriteLine(value); }
        public void WriteLine(ulong value) { Console.WriteLine(value); }
        public void WriteLine(string format, object arg0) { Console.WriteLine(format, arg0); }
        public void WriteLine(string format, params object[] arg) { Console.WriteLine(format, arg); }
        public void WriteLine(char[] buffer, int index, int count) { Console.WriteLine(buffer, index, count); }
        public void WriteLine(string format, object arg0, object arg1) { Console.WriteLine(format, arg0, arg1); }
        public void WriteLine(string format, object arg0, object arg1, object arg2) { Console.WriteLine(format, arg0, arg1, arg2); }
        public void WriteLine(string format, object arg0, object arg1, object arg2, object arg3) { Console.WriteLine(format, arg0, arg1, arg2, arg3); }
    }
}

