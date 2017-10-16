/* 
 * File: AppDomainMixin.cs
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


using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Security.Policy;
using System.Threading;
using Urasandesu.NAnonym.Remoting;

namespace Urasandesu.NAnonym.Mixins.System
{
    public static class AppDomainMixin
    {
        public static void RunAtIsolatedDomain(this AppDomain source, Action action)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain(source.Evidence, source.SetupInformation, action);
        }

        public static void RunAtIsolatedDomain<T>(this AppDomain source, Action<T> action, T arg)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain<T>(source.Evidence, source.SetupInformation, action, arg);
        }

        public static void RunAtIsolatedDomain<T1, T2>(this AppDomain source, Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain<T1, T2>(source.Evidence, source.SetupInformation, action, arg1, arg2);
        }

        public static void RunAtIsolatedDomain<T1, T2, T3>(this AppDomain source, Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain<T1, T2, T3>(source.Evidence, source.SetupInformation, action, arg1, arg2, arg3);
        }

        public static void RunAtIsolatedDomain<T1, T2, T3, T4>(this AppDomain source, Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain<T1, T2, T3, T4>(source.Evidence, source.SetupInformation, action, arg1, arg2, arg3, arg4);
        }

        public static void RunAtIsolatedDomain(this AppDomain source, Evidence securityInfo, Action action)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain(securityInfo, source.SetupInformation, action);
        }

        public static void RunAtIsolatedDomain<T>(this AppDomain source, Evidence securityInfo, Action<T> action, T arg)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain<T>(securityInfo, source.SetupInformation, action, arg);
        }

        public static void RunAtIsolatedDomain<T1, T2>(this AppDomain source, Evidence securityInfo, Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain<T1, T2>(securityInfo, source.SetupInformation, action, arg1, arg2);
        }

        public static void RunAtIsolatedDomain<T1, T2, T3>(this AppDomain source, Evidence securityInfo, Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain<T1, T2, T3>(securityInfo, source.SetupInformation, action, arg1, arg2, arg3);
        }

        public static void RunAtIsolatedDomain<T1, T2, T3, T4>(this AppDomain source, Evidence securityInfo, Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain<T1, T2, T3, T4>(securityInfo, source.SetupInformation, action, arg1, arg2, arg3, arg4);
        }

        public static void RunAtIsolatedDomain(this AppDomain source, AppDomainSetup info, Action action)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain(source.Evidence, info, action);
        }

        public static void RunAtIsolatedDomain<T>(this AppDomain source, AppDomainSetup info, Action<T> action, T arg)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain<T>(source.Evidence, info, action, arg);
        }

        public static void RunAtIsolatedDomain<T1, T2>(this AppDomain source, AppDomainSetup info, Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain<T1, T2>(source.Evidence, info, action, arg1, arg2);
        }

        public static void RunAtIsolatedDomain<T1, T2, T3>(this AppDomain source, AppDomainSetup info, Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain<T1, T2, T3>(source.Evidence, info, action, arg1, arg2, arg3);
        }

        public static void RunAtIsolatedDomain<T1, T2, T3, T4>(this AppDomain source, AppDomainSetup info, Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain<T1, T2, T3, T4>(source.Evidence, info, action, arg1, arg2, arg3, arg4);
        }

        public static void RunAtIsolatedDomain(Evidence securityInfo, AppDomainSetup info, Action action)
        {
            RunAtIsolatedDomain(securityInfo, info, (Delegate)action);
        }

        public static void RunAtIsolatedDomain<T>(Evidence securityInfo, AppDomainSetup info, Action<T> action, T arg)
        {
            RunAtIsolatedDomain(securityInfo, info, (Delegate)action, arg);
        }

        public static void RunAtIsolatedDomain<T1, T2>(Evidence securityInfo, AppDomainSetup info, Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            RunAtIsolatedDomain(securityInfo, info, (Delegate)action, arg1, arg2);
        }

        public static void RunAtIsolatedDomain<T1, T2, T3>(Evidence securityInfo, AppDomainSetup info, Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            RunAtIsolatedDomain(securityInfo, info, (Delegate)action, arg1, arg2, arg3);
        }

        public static void RunAtIsolatedDomain<T1, T2, T3, T4>(Evidence securityInfo, AppDomainSetup info, Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            RunAtIsolatedDomain(securityInfo, info, (Delegate)action, arg1, arg2, arg3, arg4);
        }

        public static void RunAtIsolatedDomain(Evidence securityInfo, AppDomainSetup info, Delegate action, params object[] args)
        {
            if (securityInfo == null)
                throw new ArgumentNullException("securityInfo");

            if (info == null)
                throw new ArgumentNullException("info");

            if (action == null)
                throw new ArgumentNullException("action");

            if (!action.CanCrossDomain())
                throw new ArgumentException(Resources.GetString("AppDomainMixin_RunAtIsolatedXXX_ActionCanNotCrossDomain"), "action");


            var domain = default(AppDomain);
            try
            {
                domain = AppDomain.CreateDomain("Domain " + action.Method.ToString(), securityInfo, info);
                var type = typeof(MarshalByRefRunner);
                var runner = (MarshalByRefRunner)domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
                runner.Action = action;
                runner.Run(args);
            }
            catch (SerializationException e)
            {
                throw new ArgumentException(Resources.GetString("AppDomainMixin_RunAtIsolatedXXX_ActionParameterCanNotCrossDomain"), e);
            }
            finally
            {
                try
                {
                    if (domain != null)
                        AppDomain.Unload(domain);
                }
                catch { }
            }
        }



        public static void RunAtIsolatedProcess(this AppDomain source, Action action)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedProcess(source.SetupInformation, action);
        }

        public static void RunAtIsolatedProcess<T>(this AppDomain source, Action<T> action, T arg)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedProcess<T>(source.SetupInformation, action, arg);
        }

        public static void RunAtIsolatedProcess<T1, T2>(this AppDomain source, Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedProcess<T1, T2>(source.SetupInformation, action, arg1, arg2);
        }

        public static void RunAtIsolatedProcess<T1, T2, T3>(this AppDomain source, Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedProcess<T1, T2, T3>(source.SetupInformation, action, arg1, arg2, arg3);
        }

        public static void RunAtIsolatedProcess<T1, T2, T3, T4>(this AppDomain source, Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedProcess<T1, T2, T3, T4>(source.SetupInformation, action, arg1, arg2, arg3, arg4);
        }

        public static void RunAtIsolatedProcess(AppDomainSetup info, Action action)
        {
            RunAtIsolatedProcess(info, (Delegate)action);
        }

        public static void RunAtIsolatedProcess<T>(AppDomainSetup info, Action<T> action, T arg)
        {
            RunAtIsolatedProcess(info, (Delegate)action, arg);
        }

        public static void RunAtIsolatedProcess<T1, T2>(AppDomainSetup info, Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            RunAtIsolatedProcess(info, (Delegate)action, arg1, arg2);
        }

        public static void RunAtIsolatedProcess<T1, T2, T3>(AppDomainSetup info, Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            RunAtIsolatedProcess(info, (Delegate)action, arg1, arg2, arg3);
        }

        public static void RunAtIsolatedProcess<T1, T2, T3, T4>(AppDomainSetup info, Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            RunAtIsolatedProcess(info, (Delegate)action, arg1, arg2, arg3, arg4);
        }

        public static void RunAtIsolatedProcess(AppDomainSetup info, Delegate action, params object[] args)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            if (action == null)
                throw new ArgumentNullException("action");

            if (!action.CanCrossDomain())
                throw new ArgumentException(Resources.GetString("AppDomainMixin_RunAtIsolatedXXX_ActionCanNotCrossDomain"), "action");


            // The reason of using GUID as follows is that IPC channel cannot close immediately. So we generate new port name each time.
            var portName = "localhost_nanonym_mixins_system_appdomainmixin_runatisolatedprocess" + Guid.NewGuid().ToString("N");
            var properties = new Hashtable
            {
                ["name"] = "ipc server " + Guid.NewGuid().ToString("N"),
                ["portName"] = portName
            };
            var serverSinkProvider = new BinaryServerFormatterSinkProvider() { TypeFilterLevel = TypeFilterLevel.Full };
            var serverChannel = new IpcServerChannel(properties, serverSinkProvider);
            ChannelServices.RegisterChannel(serverChannel, false);
            try
            {
                RunAtIsolatedProcessCore(info.ApplicationBase, portName, action, args);
            }
            finally
            {
                ChannelServices.UnregisterChannel(serverChannel);
            }
        }

        // Some RemotingExceptions are recoverable if retrying. This field indicates the retry count. 
        // In fact, we wish Marshal.GetLastWin32Error result could be used, but the API returns no meaning code.
        // (see also https://blogs.msdn.microsoft.com/suwatch/2009/01/11/remotingexception-all-pipe-instances-are-busy-or-the-system-cannot-find-the-file-specified/)
        static int s_isolatedProcessRecoverableExceptionRetryCount = 5;
        public static int IsolatedProcessRecoverableExceptionRetryCount
        {
            get { return s_isolatedProcessRecoverableExceptionRetryCount; }
            set { Interlocked.Exchange(ref s_isolatedProcessRecoverableExceptionRetryCount, value); }
        }

        static void RunAtIsolatedProcessCore(string workingDir, string portName, Delegate action, object[] args)
        {
            var retryCount = s_isolatedProcessRecoverableExceptionRetryCount;
            do
            {
                var exKeeper = new ExceptionKeeper();
                var agent = new RemoteActionAgent(action, exKeeper, args);
                var runner = new MarshalByRefRunner { Action = agent.RunAction };
                var objectUri = typeof(MarshalByRefRunner).FullName + Guid.NewGuid().ToString("N");
                RemotingServices.Marshal(runner, objectUri);
                try
                {
                    var runnerUrl = "ipc://" + portName + "/" + objectUri;
                    var isolatedProcName = GetIsolatedProcessName();
                    var startInfo = new ProcessStartInfo(isolatedProcName, runnerUrl)
                    {
                        WorkingDirectory = workingDir,
                        WindowStyle = ProcessWindowStyle.Hidden
                    };
                    var isolatedProc = Process.Start(startInfo);
                    isolatedProc.WaitForExit();

                    if (exKeeper.SerializationException != null)
                        throw new ArgumentException(Resources.GetString("AppDomainMixin_RunAtIsolatedXXX_ActionParameterCanNotCrossDomain"), exKeeper.SerializationException);

                    if (0 < --retryCount && exKeeper.ActionException is RemotingException)
                        Thread.Sleep(1000);
                    else if (exKeeper.ActionException != null)
                        throw new TargetInvocationException(exKeeper.ActionException);
                    else
                        retryCount = 0;
                }
                finally
                {
                    RemotingServices.Disconnect(runner);
                }
            } while (0 < retryCount);
        }

        class ExceptionKeeper : MarshalByRefObject
        {
            public SerializationException SerializationException { get; set; }
            public Exception ActionException { get; set; }
        }

        [Serializable]
        class RemoteActionAgent
        {
            readonly Delegate m_action;
            readonly ExceptionKeeper m_exKeeper;
            readonly object[] m_args;

            public RemoteActionAgent(Delegate action, ExceptionKeeper exKeeper, params object[] args)
            {
                m_action = action;
                m_exKeeper = exKeeper;
                m_args = args;
                RunAction = RunActionCore;
            }

            public Action<string> RunAction { get; private set; }

            void RunActionCore(string remoteRunnerUrl)
            {
                // We want to specify the client channel to Activator.GetObject/RemotingServices.Connect directly. However, both APIs don't support it. 
                // So they will choose wrong client channel in multithreading... To avoid this, we assign one client-side processing to one application. 
                // We expect that they will always choose correct channel because there is only one client channel in the application.
                // By the way, this process only exists to isolate the environment, so we can call the simplest overloads of RunAtIsolatedDomain.
                try
                {
                    AppDomain.CurrentDomain.RunAtIsolatedDomain(remoteRunnerUrl_ =>
                    {
                        var clientChannel = new IpcClientChannel("ipc client " + Guid.NewGuid().ToString("N"), null);
                        ChannelServices.RegisterChannel(clientChannel, false);
                        try
                        {
                            var remoteRunner = (MarshalByRefRunner)RemotingServices.Connect(typeof(MarshalByRefRunner), remoteRunnerUrl_);
                            remoteRunner.Run(m_action, m_args);
                        }
                        catch (SerializationException ex)
                        {
                            m_exKeeper.SerializationException = ex;
                        }
                        catch (TargetInvocationException ex)
                        {
                            var actualEx = ex.EnumerateInnerException().SkipWhile(_ => _ is TargetInvocationException).First();
                            m_exKeeper.ActionException = actualEx;
                        }
                        finally
                        {
                            ChannelServices.UnregisterChannel(clientChannel);
                        }
                    }, remoteRunnerUrl);
                }
                catch (ArgumentException ex)
                {
                    if (!(ex.InnerException is SerializationException))
                        throw;

                    m_exKeeper.SerializationException = (SerializationException)ex.InnerException;
                }
            }
        }

        static bool s_runsAsClr20 = GetRunsAsClr20();
        static bool GetRunsAsClr20()
        {
            var runtimeVer = typeof(object).Assembly.ImageRuntimeVersion;
            if (runtimeVer == "v2.0.50727")
                return true;
            else if (runtimeVer == "v4.0.30319")
                return false;
            else
                throw new NotSupportedException(string.Format("The runtime version '{0}' is not supported.", runtimeVer));
        }
        static bool RunsAsClr20 { get { return s_runsAsClr20; } }

        static bool s_runsAsX86 = GetRunsAsX86();
        static bool GetRunsAsX86()
        {
            var pointerSize = IntPtr.Size;
            if (pointerSize == 4)
                return true;
            else if (pointerSize == 8)
                return false;
            else
                throw new NotSupportedException(string.Format("The pointer size '{0}' is not supported.", pointerSize));
        }
        static bool RunsAsX86 { get { return s_runsAsX86; } }

        static string GetIsolatedProcessName()
        {
            if (RunsAsClr20)
                if (RunsAsX86)
                    return GetIsolatedProcessNameClr20X86();
                else
                    return GetIsolatedProcessNameClr20();
            else
                if (RunsAsX86)
                    return GetIsolatedProcessNameClr40X86();
                else
                    return GetIsolatedProcessNameClr40();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static string GetIsolatedProcessNameClr20()
        {
            return typeof(NAnonymIsolatedProcess.Clr20.Program).Module.Name;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static string GetIsolatedProcessNameClr20X86()
        {
            return typeof(NAnonymIsolatedProcess.Clr20.X86.Program).Module.Name;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static string GetIsolatedProcessNameClr40()
        {
            return typeof(NAnonymIsolatedProcess.Program).Module.Name;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static string GetIsolatedProcessNameClr40X86()
        {
            return typeof(NAnonymIsolatedProcess.X86.Program).Module.Name;
        }
    }
}
