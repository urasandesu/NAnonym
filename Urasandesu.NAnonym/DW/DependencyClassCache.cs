/* 
 * File: DependencyClassCache.cs
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
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.ILTools.Impl.System.Reflection;

namespace Urasandesu.NAnonym.DW
{
    class DependencyClassCache : MarshalByRefObject
    {
        List<LocalClass> classList = new List<LocalClass>();
        LocalClassLoadParameter parameter;

        public void RegisterLocal(LocalClass localClass)
        {
            classList.Add(localClass);

            if (parameter == null)
            {
                var assemblyName = new AssemblyName("LocalClasses");
                assemblyName.Version = new Version(1, 0, 0, 0);
                var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
                var moduleBuilder = assemblyBuilder.DefineDynamicModule("LocalClasses", "LocalClasses.dll");
                parameter = new LocalClassLoadParameter(new SRAssemblyGeneratorImpl(assemblyBuilder, moduleBuilder));
            }

            localClass.Register();
        }

        public void LoadLocal()
        {
            foreach (var localClass in classList)
            {
                localClass.Load(parameter);
            }

            // 保存する場合はここで行う。
            //((SRAssemblyGeneratorImpl)assemblyGen).Source.Save("LocalClasses.dll");
        }
    }
}
