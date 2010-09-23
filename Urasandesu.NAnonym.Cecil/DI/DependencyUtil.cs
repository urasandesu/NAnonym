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

// 名前変えること考えよう…長げーす。
// ・NAnonym とか。Urasandesu.NAnonym

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
        //public static void BeginEdit(this AppDomain appDomain)
        //{
        //    throw new NotImplementedException();
        //}

        static HashSet<GlobalClassBase> globalClassSet = new HashSet<GlobalClassBase>();
        static AppDomain newDomain;

        public static void Load()
        {
            foreach (var globalClass in globalClassSet)
            {
                newDomain.DoCallBack(globalClass.Load);
            }
            AppDomain.Unload(newDomain);
        }

        public static void Setup<T>() where T : GlobalClassBase
        {
            // ここで Inject したのは完全な書き換えが可能になる。DLL の場所を記憶しておく必要あり。
            // TODO: GAC に登録されているものは無理げ。厳密名を持ってる Assembly も無理げ。
            // TODO: 遅延署名される Assembly には対応できないと使えない。調査の必要あり。
            // ShadowCopyFiles を "true" にしているのは、デバッグ実行途中で無理やり止めた場合に Unload できないことがあったため。
            var info = new AppDomainSetup();
            info.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            info.ShadowCopyFiles = "true";
            newDomain = AppDomain.CreateDomain("NewDomain", null, info);
            var globalClassType = typeof(T);
            var globalClass = (GlobalClassBase)newDomain
                .CreateInstanceAndUnwrap(globalClassType.Assembly.FullName, globalClassType.FullName);
            globalClassSet.Add(globalClass);
            newDomain.DoCallBack(globalClass.Setup);

            //try
            //{
            //    var globalClassType = typeof(T);
            //    var globalClass = (GlobalClassBase)newDomain
            //        .CreateInstanceAndUnwrap(globalClassType.Assembly.FullName, globalClassType.FullName);
            //    newDomain.DoCallBack(new CrossAppDomainDelegate(globalClass.Setup));
            //}
            //finally
            //{
            //    // TODO: DependencyUtil.Load で AppDomain.Unload すること。
            //    // TODO: 自動で Try ～ Finally してくれる Using メソッド追加。
            //    AppDomain.Unload(newDomain);
            //}
        }
    }
}
