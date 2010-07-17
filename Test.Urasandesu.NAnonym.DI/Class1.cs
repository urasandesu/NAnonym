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
        public Action12Tester(NewAppDomainTesterParam param)
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
        public Action2LocalVariable2Tester(NewAppDomainTesterParam param)
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
        public Action2LocalVariable3Tester(NewAppDomainTesterParam param)
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
        public Action2LocalVariable4Tester(NewAppDomainTesterParam param)
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
}
