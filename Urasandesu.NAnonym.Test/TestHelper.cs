/* 
 * File: TestHelper.cs
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
using Microsoft.VisualBasic.FileIO;
using System.Reflection;

namespace Urasandesu.NAnonym.Test
{
    public delegate NewDomainTestInfo NewDomainTestInfoProvider();

    public delegate void NewDomainTestVerifier(NewDomainTestTarget target);

    public static class TestHelper
    {
        public static void UsingTempFile(Action<string> action)
        {
            string tempFileName = Path.GetFileNameWithoutExtension(FileSystem.GetTempFileName()) + ".dll";
            try
            {
                action(tempFileName);
            }
            finally
            {
                TryDelete(tempFileName);
            }
        }

        public static bool TryDelete(string filePath)
        {
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch
            {
                // 無視。
                return false;
            }
        }

        public static bool TryDeleteFiles(string path, string searchPattern)
        {
            return Directory.GetFiles(path, searchPattern).All(file => TryDelete(file));
        }

        public static void UsingNewDomain(NewDomainTestInfoProvider provider)
        {
            var domain = default(AppDomain);
            try
            {
                var testInfo = provider();

                domain = AppDomain.CreateDomain(testInfo.FriendlyName, null, AppDomain.CurrentDomain.SetupInformation);
                var marshalByRefTester =
                    (MarshalByRefTester)domain.CreateInstanceAndUnwrap(
                        typeof(MarshalByRefTester).Assembly.FullName,
                        typeof(MarshalByRefTester).FullName,
                        true,
                        BindingFlags.Default,
                        null,
                        new object[] { testInfo },
                        null,
                        null,
                        null);
                domain.DoCallBack(new CrossAppDomainDelegate(marshalByRefTester.Verify));
            }
            finally
            {
                if (domain != null)
                {
                    AppDomain.Unload(domain);
                }
            }
        }

        public static void ThrowException(string value)
        {
            throw new Exception(value);
        }

        public static void ThrowException(string value, object param)
        {
            throw new Exception(string.Format(value, param));
        }

        public static void ThrowException(object o)
        {
            throw new Exception(string.Format("{0}", o));
        }

        public static int GetValue(int value)
        {
            return value;
        }

        public static string GetValue(string value)
        {
            return value;
        }
    }
}

