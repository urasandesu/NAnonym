using System;
using System.Reflection;

namespace Urasandesu.NAnonym.Test
{
    public delegate NewDomainTestInfo NewDomainTestInfoProvider();

    public delegate void NewDomainTestVerifier(NewDomainTestTarget target);

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
                    (MarshalByRefTester)newDomain.CreateInstanceAndUnwrap(
                        typeof(MarshalByRefTester).Assembly.FullName,
                        typeof(MarshalByRefTester).FullName,
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
}
