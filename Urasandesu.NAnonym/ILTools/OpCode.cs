using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.ILTools
{
    public sealed class OpCode
    {
        readonly string name;

        public OpCode(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
