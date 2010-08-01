using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.CREUtilities;
using Urasandesu.NAnonym.Linq;
using System.Reflection;
using System.Threading;

namespace Urasandesu.NAnonym.Test
{
    public delegate NewAppDomainTesterParameter NewAppDomainTesterUsingAction(AppDomain newDomain);

    public abstract class NewAppDomainTester : MarshalByRefObject
    {
        public static void Using(NewAppDomainTesterUsingAction action)
        {
            var info = new AppDomainSetup();
            var appDomain = Thread.GetDomain();
            info.ApplicationBase = appDomain.BaseDirectory;
            info.ShadowCopyFiles = "true";
            var newDomain = AppDomain.CreateDomain("NewDomain", null, info);
            try
            {
                var param = action(newDomain);

                var marshalByRefTester =
                    (NewAppDomainTester)newDomain.CreateInstanceAndUnwrap(
                        param.Tester.Assembly.FullName,
                        param.Tester.FullName,
                        true,
                        BindingFlags.Default,
                        null,
                        new object[] { param },
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

        public NewAppDomainTester(NewAppDomainTesterParameter parameter)
        {
            Parameter = parameter;
        }

        protected NewAppDomainTesterParameter Parameter { get; private set; }

        Assembly assembly;
        protected Assembly Assembly
        {
            get
            {
                if (assembly == null)
                {
                    assembly = Assembly.LoadFile(Parameter.FileName);
                }
                return assembly;
            }
        }

        object instance;
        protected object Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Assembly.CreateInstance(Parameter.TypeName);
                }
                return instance;
            }
        }

        MethodInfo method;
        protected MethodInfo Method
        {
            get
            {
                if (method == null)
                {
                    var type = Instance.GetType();
                    method = type.GetMethod(Parameter.MethodName);
                }
                return method;
            }
        }

        public abstract void Verify();
    }

    [Serializable]
    public class NewAppDomainTesterParameter
    {
        public NewAppDomainTesterParameter(string fileName, string typeName, string methodName, Type tester)
        {
            FileName = fileName;
            TypeName = typeName;
            MethodName = methodName;
            Tester = tester;
        }

        public string FileName { get; private set; }
        public string TypeName { get; private set; }
        public string MethodName { get; private set; }
        public Type Tester { get; private set; }
    }

    [Serializable]
    public class NewAppDomainTesterParameter1 : NewAppDomainTesterParameter
    {
        public NewAppDomainTesterParameter1(string fileName, string typeName, string methodName, Type tester, TotableScope scope)
            : base(fileName, typeName, methodName, tester)
        {
            Scope = scope;
        }

        public TotableScope Scope { get; private set; }
    }

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
}
