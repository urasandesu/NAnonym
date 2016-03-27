/* 
 * File: ProcessMixin.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2015 Akira Sugiura
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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Urasandesu.NAnonym.Mixins.System.Diagnostics
{
    public static class ProcessMixin
    {
        const int ErrorCancelled = 1223;

        public static bool RestartCurrentProcess()
        {
            return RestartCurrentProcessWith(null);
        }

        public static bool RestartCurrentProcessWith(Action<ProcessStartInfo> additionalSetup)
        {
            var curProc = Process.GetCurrentProcess();
            var startInfo = curProc.StartInfo;
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = Environment.CurrentDirectory;
            startInfo.FileName = curProc.MainModule.FileName;
            var commandLineArgs = Environment.GetCommandLineArgs().Skip(1).ToArray();
            if (commandLineArgs.Any())
                startInfo.Arguments = "\"" + string.Join("\" \"", commandLineArgs.Select(_ => _.Replace("\"", "\\\"")).ToArray()) + "\"";
            if (additionalSetup != null)
                additionalSetup(startInfo);
            try
            {
                Process.Start(startInfo);
            }
            catch (Win32Exception ex)
            {
                if (ex.NativeErrorCode == ErrorCancelled)
                    return false;
                throw;
            }

            curProc.CloseMainWindow();

            return true;
        }
    }
}
