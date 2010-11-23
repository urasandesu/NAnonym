using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Urasandesu.NAnonym.Etc
{
    public interface ISample2
    {
        string Print(string value);
    }

    public interface ISample3
    {
        int Add(int x, int y);
    }

    public class Sample3
    {
        public virtual int Add(int x, int y)
        {
            return x + y;
        }
    }
}
