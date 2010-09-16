using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.Cecil.DI
{
    // MEMO: GlobalClass が Generic Type 持っちゃうから、共通で引き回せるような口用。
    public abstract class GlobalClassBase : MarshalByRefObject
    {
        public void Load()
        {
            var modified = SetUp();
        }

        protected abstract GlobalClassBase SetUp();
    }
}
