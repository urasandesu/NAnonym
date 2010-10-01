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
using UND = Urasandesu.NAnonym.DI;
using System.Xml.Serialization;
using System.Configuration;

namespace Urasandesu.NAnonym.Cecil.DI
{
    // MEMO: GlobalClass、LocalClass で使うユーティリティクラス的な存在になる？
    public class DependencyUtil : UND::DependencyUtil
    {
        // TODO: このクラスに限らず、ユーザーに見せるクラスは「ある名前空間をインポートしたら重複してしまう」ことが無いようにする。
        // TODO: 同じ機能を持たせる場合は、継承して使えるようにする。
        protected DependencyUtil()
            : base()
        {
        }

        static HashSet<GlobalClassBase> classSet = new HashSet<GlobalClassBase>();
        static AppDomain newDomain;

        static HashSet<DIAssemblySetup> assemblySetupSet;


        public static void BeginEdit()
        {
            var config = (DIConfigurationSection)ConfigurationManager.GetSection(DIConfigurationSection.Name);
            if (File.Exists(config.AssemblySetupSetPath))
            {
                CancelEdit();
            }
        }

        public static void Setup<TGlobalClassType>() where TGlobalClassType : GlobalClassBase
        {
            // ここで Inject したのは完全な書き換えが可能になる。DLL の場所を記憶しておく必要あり。
            // TODO: GAC に登録されているものは無理げ。厳密名を持ってる Assembly も無理げ。
            // TODO: 遅延署名される Assembly には対応できないと使えない。調査の必要あり。
            // ShadowCopyFiles を "true" にしているのは、デバッグ実行途中で無理やり止めた場合に Unload できないことがあったため。
            // TODO: Rollback 機能。
            // TODO: Setup 情報は一通りコピーして、ShadowCopyFiles だけ true にする方向で。
            var info = new AppDomainSetup();
            info.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            info.ShadowCopyFiles = "true";
            newDomain = AppDomain.CreateDomain("NewDomain", null, info);
            var classType = typeof(TGlobalClassType);
            var @class = (GlobalClassBase)newDomain.CreateInstanceAndUnwrap(classType.Assembly.FullName, classType.FullName);
            classSet.Add(@class);
            if (assemblySetupSet == null)
            {
                assemblySetupSet = new HashSet<DIAssemblySetup>();
            }
            assemblySetupSet.Add(new DIAssemblySetup(@class.CodeBase, @class.Location));
            @class.Setup();
        }

        public static void Load()
        {
            var config = (DIConfigurationSection)ConfigurationManager.GetSection(DIConfigurationSection.Name);
            if (!File.Exists(config.AssemblySetupSetPath) && assemblySetupSet != null)
            {
                if (!Directory.Exists(config.BackupDirectoryName))
                {
                    Directory.CreateDirectory(config.BackupDirectoryName);
                }

                foreach (var assemblySetup in assemblySetupSet)
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

                using (var assemblySetupSetStream = new FileStream(config.AssemblySetupSetPath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    var assemblySetupSetSerializer = new XmlSerializer(typeof(DIAssemblySetupCollection));
                    var assemblySetupCollection = new DIAssemblySetupCollection();
                    assemblySetupCollection.AssemblySetupList = assemblySetupSet.ToArray();
                    assemblySetupSetSerializer.Serialize(assemblySetupSetStream, assemblySetupCollection);
                }
            }

            foreach (var @class in classSet)
            {
                @class.Load();
            }
            AppDomain.Unload(newDomain);
        }

        public static void CancelEdit()
        {
            // HACK: setupInfoSet って上書きしちゃっていいのかな？
            var config = (DIConfigurationSection)ConfigurationManager.GetSection(DIConfigurationSection.Name);
            if (File.Exists(config.AssemblySetupSetPath))
            {
                using (var assemblySetupSetStream = new FileStream(config.AssemblySetupSetPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var assemblySetupSetSerializer = new XmlSerializer(typeof(DIAssemblySetupCollection));
                    var assemblySetupCollection = (DIAssemblySetupCollection)assemblySetupSetSerializer.Deserialize(assemblySetupSetStream);
                    assemblySetupSet = new HashSet<DIAssemblySetup>(assemblySetupCollection.AssemblySetupList);
                }

                foreach (var assemblySetup in assemblySetupSet)
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

                assemblySetupSet = null;
                File.Delete(config.AssemblySetupSetPath);
            }
        }
    }
}
