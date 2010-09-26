using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Urasandesu.NAnonym.Test.Preprocessor
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1 && File.Exists(args[0]))
            {
                File.Delete(args[0]);
            }
        }
    }
}
