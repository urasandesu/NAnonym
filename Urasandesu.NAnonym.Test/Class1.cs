using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.CREUtilities;
using Urasandesu.NAnonym.Linq;
using System.Reflection;
using System.Threading;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace Urasandesu.NAnonym.Test
{
    public delegate NewDomainTestInfo NewDomainTestInfoProvider();

    public delegate void NewDomainTestVerifier(NewDomainVerificationDelegate @delegate);

    [Serializable]
    public class NewDomainTestInfo
    {
        public string FileName { get; set; }
        public string TypeFullName { get; set; }
        public string MethodName { get; set; }
        public NewDomainTestVerifier TestVerifier { get; set; }
        public PortableScope Scope { get; set; }
        public PortableScope2 Scope2 { get; set; }
        public virtual void Verify()
        {
            TestVerifier(new NewDomainVerificationDelegate(this));
        }
    }

    public class NewDomainVerificationDelegate
    {
        readonly NewDomainTestInfo testInfo;
        public NewDomainVerificationDelegate(NewDomainTestInfo testInfo)
        {
            this.testInfo = testInfo; 
        }

        public NewDomainTestInfo TestInfo
        {
            get { return testInfo; }
        }

        Assembly assembly;
        public virtual Assembly Assembly
        {
            get
            {
                if (assembly == null)
                {
                    assembly = Assembly.LoadFile(testInfo.FileName);
                }
                return assembly;
            }
        }

        object instance;
        public virtual object Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Assembly.CreateInstance(testInfo.TypeFullName);
                }
                return instance;
            }
        }


        MethodInfo method;
        public virtual MethodInfo Method
        {
            get
            {
                if (method == null)
                {
                    var type = Instance.GetType();
                    method = type.GetMethod(testInfo.MethodName);
                }
                return method;
            }
        }


        public virtual object Invoke(params object[] parameters)
        {
            return Method.Invoke(Instance, parameters);
        }
    }

    public static class NewDomainTest
    {
        public static void Transfer(NewDomainTestInfoProvider provider)
        {
            var info = new AppDomainSetup();
            info.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            info.ShadowCopyFiles = "true";
            var newDomain = default(AppDomain);
            try
            {
                newDomain = AppDomain.CreateDomain("NewDomain", null, info);
                var testInfo = provider();

                var marshalByRefTester =
                    (NewDomainTester)newDomain.CreateInstanceAndUnwrap(
                        typeof(NewDomainTester).Assembly.FullName,
                        typeof(NewDomainTester).FullName,
                        true,
                        BindingFlags.Default,
                        null,
                        new object[] { testInfo },
                        null,
                        null,
                        null);
                newDomain.DoCallBack(new CrossAppDomainDelegate(marshalByRefTester.Verify));
            }
            finally
            {
                AppDomain.Unload(newDomain);
            }
        }
    }

    public sealed class NewDomainTester : MarshalByRefObject
    {
        NewDomainTestInfo testInfo;
        public NewDomainTester(NewDomainTestInfo testInfo)
        {
            this.testInfo = testInfo;
        }

        public void Verify()
        {
            testInfo.Verify();
        }
    }

    //public abstract class NewAppDomainTester : MarshalByRefObject
    //{
    //    public static void Using(NewAppDomainTesterUsingAction action)
    //    {
    //        var info = new AppDomainSetup();
    //        var appDomain = Thread.GetDomain();
    //        info.ApplicationBase = appDomain.BaseDirectory;
    //        info.ShadowCopyFiles = "true";
    //        var newDomain = default(AppDomain);
    //        try
    //        {
    //            newDomain = AppDomain.CreateDomain("NewDomain", null, info);
    //            var param = action(newDomain);

    //            var marshalByRefTester =
    //                (NewAppDomainTester)newDomain.CreateInstanceAndUnwrap(
    //                    param.Tester.Assembly.FullName,
    //                    param.Tester.FullName,
    //                    true,
    //                    BindingFlags.Default,
    //                    null,
    //                    new object[] { param },
    //                    null,
    //                    null,
    //                    null);
    //            newDomain.DoCallBack(new CrossAppDomainDelegate(marshalByRefTester.Verify));
    //        }
    //        finally
    //        {
    //            AppDomain.Unload(newDomain);
    //        }
    //    }

    //    public NewAppDomainTester(NewAppDomainTesterParameter parameter)
    //    {
    //        Parameter = parameter;
    //    }

    //    protected NewAppDomainTesterParameter Parameter { get; private set; }

    //    Assembly assembly;
    //    protected virtual Assembly Assembly
    //    {
    //        get
    //        {
    //            if (assembly == null)
    //            {
    //                assembly = Assembly.LoadFile(Parameter.AssemblyIdentifier);
    //            }
    //            return assembly;
    //        }
    //    }

    //    object instance;
    //    protected virtual object Instance
    //    {
    //        get
    //        {
    //            if (instance == null)
    //            {
    //                instance = Assembly.CreateInstance(Parameter.TypeIdentifier);
    //            }
    //            return instance;
    //        }
    //    }

    //    MethodInfo method;
    //    protected virtual MethodInfo Method
    //    {
    //        get
    //        {
    //            if (method == null)
    //            {
    //                var type = Instance.GetType();
    //                method = type.GetMethod(Parameter.MethodName);
    //            }
    //            return method;
    //        }
    //    }

    //    public abstract void Verify();
    //}

    //[Serializable]
    //public class NewAppDomainTesterParameter
    //{
    //    public NewAppDomainTesterParameter(string assemblyIdentifier, string typeIdentifier, string methodName, Type tester)
    //    {
    //        AssemblyIdentifier = assemblyIdentifier;
    //        TypeIdentifier = typeIdentifier;
    //        MethodName = methodName;
    //        Tester = tester;
    //    }

    //    public string AssemblyIdentifier { get; private set; }
    //    public string TypeIdentifier { get; private set; }
    //    public string MethodName { get; private set; }
    //    public Type Tester { get; private set; }
    //}

    //[Serializable]
    //public class NewAppDomainTesterParameter1 : NewAppDomainTesterParameter
    //{
    //    public NewAppDomainTesterParameter1(string fileName, string typeName, string methodName, Type tester, PortableScope scope)
    //        : base(fileName, typeName, methodName, tester)
    //    {
    //        Scope = scope;
    //    }

    //    public PortableScope Scope { get; private set; }
    //}

    //[Serializable]
    //public class NewAppDomainTesterParameter2 : NewAppDomainTesterParameter
    //{
    //    public NewAppDomainTesterParameter2(string fileName, string typeName, string methodName, Type tester, PortableScope2 scope)
    //        : base(fileName, typeName, methodName, tester)
    //    {
    //        Scope = scope;
    //    }

    //    public PortableScope2 Scope { get; private set; }
    //}

    public class Assert : NUnit.Framework.Assert
    {
        protected Assert()
        {
        }

        public static void AreEquivalent<T1, T2>(T1 expected, T2 actual, Func<T1, T2, bool> equalityComparer)
        {
            NUnit.Framework.Assert.That(actual, Is.EqualTo<T1, T2>(expected, equalityComparer));
            //return new Equivalent<T1, T2>(expected, actual);
        }
    }

    //public class Equivalent<T1, T2>
    //{
    //    T1 expected;
    //    T2 actual;

    //    public Equivalent(T1 expected, T2 actual)
    //    {
    //        this.expected = expected;
    //        this.actual = actual;
    //    }

    //    public void Using(Func<T1, T2, bool> equalityComparer)
    //    {
    //        NUnit.Framework.Assert.That(actual, Is.EqualTo<T1, T2>(expected, equalityComparer));
    //    }
    //}

    public class Is : NUnit.Framework.Is
    {
        protected Is()
        {
        }

        public static NUnit.Framework.Constraints.EqualConstraint EqualTo<T1, T2>(T1 expected, Func<T1, T2, bool> equalityComparer)
        {
            return new EqualConstraint<T1, T2>(expected, equalityComparer);
        }
    }

    public class EqualConstraint<T1, T2> : NUnit.Framework.Constraints.EqualConstraint
    {
        readonly T1 expected;
        readonly Func<T1, T2, bool> equalityComparer;

        public EqualConstraint(T1 expected, Func<T1, T2, bool> equalityComparer)
            : base(expected)
        {
            this.expected = expected;
            this.equalityComparer = equalityComparer;
        }

        public override bool Matches(object actual)
        {
            this.actual = actual;
            return equalityComparer(expected, (T2)actual);
        }
    }

    public static class TestHelper
    {
        public static void UsingTempFile(Action<string> action)
        {
            string tempFileName = Path.GetFileName(FileSystem.GetTempFileName());
            try
            {
                action(tempFileName);
            }
            finally
            {
                TryDelete(tempFileName);
            }
        }

        public static bool TryDelete(string filePath)
        {
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch
            {
                // 無視。
                return false;
            }
        }

        public static bool TryDeleteFiles(string path, string searchPattern)
        {
            return Directory.GetFiles(path, searchPattern).All(file => TryDelete(file));
        }
    }
}
