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
using System.Collections.Generic;
using Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil;
using Urasandesu.NAnonym.ILTools;
using UND = Urasandesu.NAnonym.DW;

namespace Urasandesu.NAnonym.Cecil.DW
{
    class DependencyClassCache : UND::DependencyClassCache
    {
        List<Tuple2<DWAssemblySetup, GlobalClass>> setupClassList = new List<Tuple2<DWAssemblySetup, GlobalClass>>();
        Dictionary<DWAssemblySetup, GlobalClassLoadParameter> setupAssemblyGenDictionary = new Dictionary<DWAssemblySetup, GlobalClassLoadParameter>();

        public DWAssemblySetup RegisterGlobal<TGlobalClassType>() where TGlobalClassType : GlobalClass
        {
            var globalClass = (GlobalClass)Activator.CreateInstance<TGlobalClassType>();
            var assemblySetup = new DWAssemblySetup(globalClass.CodeBase, globalClass.Location);

            setupClassList.Add(Tuple.Create(assemblySetup, globalClass));

            if (!setupAssemblyGenDictionary.ContainsKey(assemblySetup))
            {
                var assemblyDef = AssemblyDefinition.ReadAssembly(assemblySetup.CodeBaseLocalPath, new ReaderParameters() { ReadSymbols = true });
                var assemblyGen =  new MCAssemblyGeneratorImpl(assemblyDef);
                setupAssemblyGenDictionary.Add(assemblySetup, new GlobalClassLoadParameter(assemblyGen));
            }

            globalClass.Register();
            return assemblySetup;
        }

        public void LoadGlobal()
        {
            foreach (var setupClassTuple in setupClassList)
            {
                var assemblySetup = setupClassTuple.Item1;
                var globalClass = setupClassTuple.Item2;
                var assemblyGen = setupAssemblyGenDictionary[assemblySetup];
                globalClass.Load(assemblyGen);
            }

            foreach (var setupAssemblyGenPair in setupAssemblyGenDictionary)
            {
                var assemblySetup = setupAssemblyGenPair.Key;
                var parameter = setupAssemblyGenPair.Value;

                ((MCAssemblyGeneratorImpl)parameter.Assembly).AssemblyDef.Write(assemblySetup.CodeBaseLocalPath, new WriterParameters() { WriteSymbols = true });
            }
        }
    }
}
