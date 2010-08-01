using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.Test;
using System.Reflection;
using NUnit.Framework;
using Assert = Urasandesu.NAnonym.Test.Assert;

namespace Test.Urasandesu.NAnonym.DI
{
    public class Action12Tester : NewAppDomainTester
    {
        public Action12Tester(NewAppDomainTesterParameter param)
            : base(param)
        {
        }

        public override void Verify()
        {
            try
            {
                Method.Invoke(Instance, null);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("Hello, World!!", e.InnerException.Message);
            }
        }
    }

    public class Action2LocalVariable2Tester : NewAppDomainTester
    {
        public Action2LocalVariable2Tester(NewAppDomainTesterParameter param)
            : base(param)
        {
        }

        public override void Verify()
        {
            try
            {
                Method.Invoke(Instance, null);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("i.ToString() = 100", e.InnerException.Message);
            }
        }
    }

    public class Action2LocalVariable3Tester : NewAppDomainTester
    {
        public Action2LocalVariable3Tester(NewAppDomainTesterParameter param)
            : base(param)
        {
        }

        public override void Verify()
        {
            try
            {
                Method.Invoke(Instance, null);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("s.ToString() = aaaaa", e.InnerException.Message);
            }
        }
    }

    public class Action2LocalVariable4Tester : NewAppDomainTester
    {
        public Action2LocalVariable4Tester(NewAppDomainTesterParameter param)
            : base(param)
        {
        }

        public override void Verify()
        {
            try
            {
                Method.Invoke(Instance, null);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("s.ToString() = AAAAAAAAAA", e.InnerException.Message);
            }
        }
    }

    public class Action2LocalVariable5Tester : NewAppDomainTester
    {
        public Action2LocalVariable5Tester(NewAppDomainTesterParameter param)
            : base(param)
        {
        }

        public override void Verify()
        {
            try
            {
                Method.Invoke(Instance, null);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("brtrue", e.InnerException.Message);
            }
        }
    }

    public class Action2LocalVariable6Tester : NewAppDomainTester
    {
        public Action2LocalVariable6Tester(NewAppDomainTesterParameter param)
            : base(param)
        {
        }

        public override void Verify()
        {
            try
            {
                Method.Invoke(Instance, null);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual( 
@"1 + 1 = 2
++i = 1
i++ = 1
--i = 1
", e.InnerException.Message);
            }
        }
    }

    public class Action2LocalVariable7Tester : NewAppDomainTester
    {
        public Action2LocalVariable7Tester(NewAppDomainTesterParameter param)
            : base(param)
        {
        }

        public override void Verify()
        {
            try
            {
                Method.Invoke(Instance, null);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(
@"Name = dynamicMethod
Return Type = System.String
Parameter Length = 1
Parameter[0] Type = Int32 
", e.InnerException.Message);
            }
        }
    }

    public class Func1Parameters2Tester : NewAppDomainTester
    {
        public Func1Parameters2Tester(NewAppDomainTesterParameter param)
            : base(param)
        {
        }

        public override void Verify()
        {
            object result = Method.Invoke(Instance, new object[] { 10 });
            Assert.AreEqual(110, result);
        }
    }

    public class Func1Parameters3Tester : NewAppDomainTester
    {
        public Func1Parameters3Tester(NewAppDomainTesterParameter param)
            : base(param)
        {
        }

        public override void Verify()
        {
            object result = Method.Invoke(Instance, new object[] { 10 });
            Assert.AreEqual(10, result);
        }
    }

    public class Action2LocalVariable8Tester : NewAppDomainTester
    {
        public Action2LocalVariable8Tester(NewAppDomainTesterParameter param)
            : base(param)
        {
        }

        public override void Verify()
        {
            try
            {
                Method.Invoke(Instance, null);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("GetValue(10) = 10", e.InnerException.Message);
            }
        }
    }

    public class Action2LocalVariable9Tester : NewAppDomainTester
    {
        public Action2LocalVariable9Tester(NewAppDomainTesterParameter param)
            : base(param)
        {
        }

        public override void Verify()
        {
            try
            {
                Method.Invoke(Instance, null);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(
@"Name = .ctor
IsPublic = True
Parameter Count = 2
Parameter[0] = System.Object object
Parameter[1] = IntPtr method
", e.InnerException.Message);
            }
        }
    }

    public class Action2LocalVariable10Tester : NewAppDomainTester
    {
        public Action2LocalVariable10Tester(NewAppDomainTesterParameter param)
            : base(param)
        {
        }

        public override void Verify()
        {
            try
            {
                var scope = ((NewAppDomainTesterParameter1)Parameter).Scope;
                scope.Bind(Instance);
                Method.Invoke(Instance, null);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("200", e.InnerException.Message);
            }
        }
    }

    public class Action2LocalVariable11Tester : NewAppDomainTester
    {
        public Action2LocalVariable11Tester(NewAppDomainTesterParameter param)
            : base(param)
        {
        }

        public override void Verify()
        {
            try
            {
                var scope = ((NewAppDomainTesterParameter1)Parameter).Scope;
                scope.Bind(Instance);
                Method.Invoke(Instance, null);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("[1, aiueo]", e.InnerException.Message);
            }
        }
    }

    public class Action2LocalVariable12Tester : NewAppDomainTester
    {
        public Action2LocalVariable12Tester(NewAppDomainTesterParameter param)
            : base(param)
        {
        }

        public override void Verify()
        {
            try
            {
                Method.Invoke(Instance, null);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(
@"Cached Field Name: CS$<>9__CachedAnonymousMethodDelegate8b
Cached Field Type: System.Action`1[[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
", e.InnerException.Message);
            }
        }
    }
}
