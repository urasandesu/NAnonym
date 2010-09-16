using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;
using System.IO;
//using Mono.Cecil;
using Urasandesu.NAnonym.Linq;
//using Mono.Cecil.Cil;

// 名前変えること考えよう…長げーす。
// ・NAnonym とか。Urasandesu.NAnonym

namespace Urasandesu.NAnonym.DI
{
    // MEMO: GlobalClass、LocalClass で使うユーティリティクラス的な存在になる？
    public static class DependencyUtil
    {
        //public static void BeginEdit(this AppDomain appDomain)
        //{
        //    throw new NotImplementedException();
        //}


        //public static void AcceptChanges(this AppDomain appDomain)
        //{
        //    throw new NotImplementedException();
        //}

        //public static void Inject<T>(this AppDomain appDomain) where T : GlobalClassBase
        //{
        //    // ここで Inject したのは完全な書き換えが可能になる。DLL の場所を記憶しておく必要あり。
        //    // TODO: GAC に登録されているものは無理げ。厳密名を持ってる Assembly も無理げ。
        //    // TODO: 遅延署名される Assembly には対応できないと使えない。調査の必要あり。
        //    // ShadowCopyFiles を "true" にしているのは、デバッグ実行途中で無理やり止めた場合に Unload できないことがあったため。
        //    var info = new AppDomainSetup();
        //    info.ApplicationBase = appDomain.BaseDirectory;
        //    info.ShadowCopyFiles = "true";
        //    var newDomain = AppDomain.CreateDomain("NewDomain", null, info);
        //    try
        //    {
        //        var globalClassType = typeof(T);
        //        var globalClass = (GlobalClassBase)newDomain
        //            .CreateInstanceAndUnwrap(globalClassType.Assembly.FullName, globalClassType.FullName);
        //        newDomain.DoCallBack(new CrossAppDomainDelegate(globalClass.Load));
        //    }
        //    finally
        //    {
        //        AppDomain.Unload(newDomain);
        //    }
        //}
    }
}
