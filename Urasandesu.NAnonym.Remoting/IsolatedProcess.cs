/* 
 * File: IsolatedProcess.cs
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
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;

namespace Urasandesu.NAnonym.Remoting
{
    class IsolatedProcess
    {
        // This class doesn't handle any exceptions because they should be handled by the remote caller.
        public void Run(params string[] args)
        {
            //System.Diagnostics.Debugger.Launch();

            // The reason of using GUID as follows is that IPC channel cannot close immediately. So we generate new port name each time.
            var portName = "nanonym_remoting_isolatedprocess_run" + Guid.NewGuid().ToString("N");
            var properties = new Hashtable();
            properties["name"] = "ipc server " + Guid.NewGuid().ToString("N");
            properties["portName"] = portName;
            var serverSinkProvider = new BinaryServerFormatterSinkProvider() { TypeFilterLevel = TypeFilterLevel.Full };
            var serverChannel = new IpcServerChannel(properties, serverSinkProvider);
            ChannelServices.RegisterChannel(serverChannel, false);
            try
            {
                RunCore(portName, args[0]);
            }
            finally
            {
                ChannelServices.UnregisterChannel(serverChannel);
            }
        }

        static void RunCore(string portName, string remoteRunnerUrl)
        {
            var runner = new MarshalByRefRunner();
            var objectUri = typeof(MarshalByRefRunner).FullName + Guid.NewGuid().ToString("N");
            runner.Action = new Action<Delegate, object[]>((action, param) => action.DynamicInvoke(param));
            RemotingServices.Marshal(runner, objectUri);
            try
            {
                RunRemoteAction(portName, objectUri, remoteRunnerUrl);
            }
            finally
            {
                RemotingServices.Disconnect(runner);
            }
        }

        static void RunRemoteAction(string portName, string objectUri, string remoteRunnerUrl)
        {
            var clientChannel = new IpcClientChannel("ipc client " + Guid.NewGuid().ToString("N"), null);
            ChannelServices.RegisterChannel(clientChannel, false);
            try
            {
                var runnerUrl = "ipc://" + portName + "/" + objectUri;
                var remoteRunner = (MarshalByRefRunner)RemotingServices.Connect(typeof(MarshalByRefRunner), remoteRunnerUrl);
                remoteRunner.Run(runnerUrl);
            }
            catch (RemotingException)
            {
                // By some reason, host process may exit early (e.g. uncheck 'Keep Test Execution Engine Running' setting).
                // In this case, execution result comes here, so we should just ignore.
            }
            finally
            {
                ChannelServices.UnregisterChannel(clientChannel);
            }
        }
    }
}
