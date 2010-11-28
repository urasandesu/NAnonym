/* 
 * File: DependencyUtil.cs
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
using System.Reflection;
using System.Globalization;
using System.IO;
using Mono.Cecil;
using Urasandesu.NAnonym.Linq;
using Mono.Cecil.Cil;
using UND = Urasandesu.NAnonym.DW;
using System.Xml.Serialization;
using System.Configuration;

namespace Urasandesu.NAnonym.Cecil.DW
{
    // MEMO: GlobalClass、LocalClass で使うユーティリティクラス的な存在になる？
    public class DependencyUtil : UND::DependencyUtil
    {
        protected DependencyUtil()
            : base()
        {
        }

        static HashSet<GlobalClass> classSet = new HashSet<GlobalClass>();
        static AppDomain newDomain;

        static HashSet<DWAssemblySetup> setupSet;

        public static void RegisterGlobal<TGlobalClassType>() where TGlobalClassType : GlobalClass
        {
            // ここで Inject したのは完全な書き換えが可能になる。DLL の場所を記憶しておく必要あり。
            // ShadowCopyFiles を "true" にしているのは、デバッグ実行途中で無理やり止めた場合に Unload できないことがあったため。
            var info = new AppDomainSetup();
            info.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            info.ShadowCopyFiles = "true";
            newDomain = AppDomain.CreateDomain("NewDomain", null, info);
            var classType = typeof(TGlobalClassType);
            var @class = (GlobalClass)newDomain.CreateInstanceAndUnwrap(classType.Assembly.FullName, classType.FullName);
            classSet.Add(@class);
            if (setupSet == null)
            {
                setupSet = new HashSet<DWAssemblySetup>();
            }
            setupSet.Add(new DWAssemblySetup(@class.CodeBase, @class.Location));
            @class.Register();
        }

        public static void LoadGlobal()
        {
            var config = (DWConfigurationSection)ConfigurationManager.GetSection(DWConfigurationSection.Name);
            if (!File.Exists(config.AssemblySetupSetPath) && setupSet != null)
            {
                if (!Directory.Exists(config.BackupDirectoryName))
                {
                    Directory.CreateDirectory(config.BackupDirectoryName);
                }

                foreach (var assemblySetup in setupSet)
                {
                    File.Copy(
                        assemblySetup.CodeBaseLocalPath, 
                        Path.Combine(config.BackupDirectoryName, Path.GetFileName(assemblySetup.CodeBaseLocalPath)), 
                        true);

                    File.Copy(
                        assemblySetup.SymbolCodeBaseLocalPath, 
                        Path.Combine(config.BackupDirectoryName, Path.GetFileName(assemblySetup.SymbolCodeBaseLocalPath)), 
                        true);
                }

                using (var setupSetStream = new FileStream(config.AssemblySetupSetPath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    var setupSetSerializer = new XmlSerializer(typeof(DWAssemblySetupCollection));
                    var setupCollection = new DWAssemblySetupCollection();
                    setupCollection.AssemblySetupList = setupSet.ToArray();
                    setupSetSerializer.Serialize(setupSetStream, setupCollection);
                }
            }

            foreach (var @class in classSet)
            {
                @class.Load();
            }
            AppDomain.Unload(newDomain);
        }

        public static void RollbackGlobal()
        {
            // HACK: setupInfoSet って上書きしちゃっていいのかな？
            var config = (DWConfigurationSection)ConfigurationManager.GetSection(DWConfigurationSection.Name);
            if (File.Exists(config.AssemblySetupSetPath))
            {
                using (var setupSetStream = new FileStream(config.AssemblySetupSetPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var setupSetSerializer = new XmlSerializer(typeof(DWAssemblySetupCollection));
                    var setupCollection = (DWAssemblySetupCollection)setupSetSerializer.Deserialize(setupSetStream);
                    setupSet = new HashSet<DWAssemblySetup>(setupCollection.AssemblySetupList);
                }

                foreach (var assemblySetup in setupSet)
                {
                    File.Copy(
                        Path.Combine(config.BackupDirectoryName, Path.GetFileName(assemblySetup.CodeBaseLocalPath)), 
                        assemblySetup.CodeBaseLocalPath, 
                        true);

                    File.Copy(
                        Path.Combine(config.BackupDirectoryName, Path.GetFileName(assemblySetup.SymbolCodeBaseLocalPath)), 
                        assemblySetup.SymbolCodeBaseLocalPath, 
                        true);
                }

                setupSet = null;
                File.Delete(config.AssemblySetupSetPath);
            }
        }
    }
}

