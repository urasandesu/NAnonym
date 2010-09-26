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

        static HashSet<GlobalSetupInfo> setupInfoSet;

        // TODO: 設定ファイル化できると良い。
        // TODO: 設定ファイル化できると良いものを他にも洗い出し。
        const string SetupInfoSetPath = "GlobalSetupInfoSet.xml";
        const string BackupDirectoryName = "Backup";

        public static void BeginEdit()
        {
            if (File.Exists(SetupInfoSetPath))
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
            if (setupInfoSet == null)
            {
                setupInfoSet = new HashSet<GlobalSetupInfo>();
            }
            setupInfoSet.Add(new GlobalSetupInfo(@class.AssemblyCodeBase, @class.AssemblyLocation));
            @class.Setup();
        }

        public static void Load()
        {
            if (!File.Exists(SetupInfoSetPath) && setupInfoSet != null)
            {
                if (!Directory.Exists(BackupDirectoryName))
                {
                    Directory.CreateDirectory(BackupDirectoryName);
                }

                foreach (var setupInfo in setupInfoSet)
                {
                    string assemblyCodeBaseLocalPath = new Uri(setupInfo.AssemblyCodeBase).LocalPath;
                    File.Copy(assemblyCodeBaseLocalPath, Path.Combine(BackupDirectoryName, Path.GetFileName(assemblyCodeBaseLocalPath)), true);

                    string assemblySymbolCodeBaseLocalPath = new Uri(setupInfo.AssemblySymbolCodeBase).LocalPath;
                    File.Copy(assemblySymbolCodeBaseLocalPath, Path.Combine(BackupDirectoryName, Path.GetFileName(assemblySymbolCodeBaseLocalPath)), true);
                }

                using (var setupInfoSetFileStream = new FileStream(SetupInfoSetPath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    var setupInfoSetSerializer = new XmlSerializer(typeof(HashSet<GlobalSetupInfo>));
                    setupInfoSetSerializer.Serialize(setupInfoSetFileStream, setupInfoSet);
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
            if (File.Exists(SetupInfoSetPath))
            {
                using (var setupInfoSetFileStream = new FileStream(SetupInfoSetPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var setupInfoSetSerializer = new XmlSerializer(typeof(HashSet<GlobalSetupInfo>));
                    setupInfoSet = (HashSet<GlobalSetupInfo>)setupInfoSetSerializer.Deserialize(setupInfoSetFileStream);
                }

                foreach (var setupInfo in setupInfoSet)
                {
                    string assemblyCodeBaseLocalPath = new Uri(setupInfo.AssemblyCodeBase).LocalPath;
                    File.Copy(Path.Combine(BackupDirectoryName, Path.GetFileName(assemblyCodeBaseLocalPath)), assemblyCodeBaseLocalPath, true);

                    string assemblySymbolCodeBaseLocalPath = new Uri(setupInfo.AssemblySymbolCodeBase).LocalPath;
                    File.Copy(Path.Combine(BackupDirectoryName, Path.GetFileName(assemblySymbolCodeBaseLocalPath)), assemblySymbolCodeBaseLocalPath, true);
                }

                setupInfoSet = null;
                File.Delete(SetupInfoSetPath);
            }
        }
    }

    public class GlobalSetupInfo
    {
        string assemblyCodeBase;
        string assemblyLocation;

        public string AssemblyCodeBase
        {
            get { return assemblyCodeBase; }
            set
            {
                assemblyCodeBase = value;
                AssemblySymbolCodeBase = assemblyCodeBase.WithoutExtension() + ".pdb";
            }
        }

        public string AssemblyLocation 
        {
            get { return assemblyLocation; }
            set
            {
                assemblyLocation = value;
                AssemblySymbolLocation = assemblyLocation.WithoutExtension() + ".pdb";
            }
        }

        [XmlIgnore]
        public string AssemblySymbolCodeBase { get; private set; }

        [XmlIgnore]
        public string AssemblySymbolLocation { get; private set; }

        public GlobalSetupInfo()
        {
        }

        public GlobalSetupInfo(string assemblyCodeBase, string assemblyLocation)
        {
            AssemblyCodeBase = assemblyCodeBase;
            AssemblyLocation = assemblyLocation;
        }



        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var that = default(GlobalSetupInfo);
            if ((that = obj as GlobalSetupInfo) == null) return false;

            return this.AssemblyCodeBase == that.AssemblyCodeBase && this.AssemblyLocation == that.AssemblyLocation;
        }

        public override int GetHashCode()
        {
            return AssemblyCodeBase.GetHashCodeOrDefault() ^ AssemblyLocation.GetHashCodeOrDefault();
        }
    }
}
