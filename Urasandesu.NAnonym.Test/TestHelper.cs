using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Reflection;

namespace Urasandesu.NAnonym.Test
{
    public delegate NewDomainTestInfo NewDomainTestInfoProvider();

    public delegate void NewDomainTestVerifier(NewDomainTestTarget target);

    public static class TestHelper
    {
        public static void UsingTempFile(Action<string> action)
        {
            string tempFileName = Path.GetFileNameWithoutExtension(FileSystem.GetTempFileName()) + ".dll";
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

        public static void UsingNewDomain(NewDomainTestInfoProvider provider)
        {
            var domain = default(AppDomain);
            try
            {
                var testInfo = provider();

                domain = AppDomain.CreateDomain(testInfo.FriendlyName, null, AppDomain.CurrentDomain.SetupInformation);
                var marshalByRefTester =
                    (MarshalByRefTester)domain.CreateInstanceAndUnwrap(
                        typeof(MarshalByRefTester).Assembly.FullName,
                        typeof(MarshalByRefTester).FullName,
                        true,
                        BindingFlags.Default,
                        null,
                        new object[] { testInfo },
                        null,
                        null,
                        null);
                domain.DoCallBack(new CrossAppDomainDelegate(marshalByRefTester.Verify));
            }
            finally
            {
                if (domain != null)
                {
                    AppDomain.Unload(domain);
                }
            }
        }

        public static void ThrowException(string value)
        {
            throw new Exception(value);
        }

        public static void ThrowException(string value, object param)
        {
            throw new Exception(string.Format(value, param));
        }

        public static void ThrowException(object o)
        {
            throw new Exception(string.Format("{0}", o));
        }

        public static int GetValue(int value)
        {
            return value;
        }
    }
}
