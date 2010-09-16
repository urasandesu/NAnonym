using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace Urasandesu.NAnonym.Test
{
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
