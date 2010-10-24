using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    public class SetupModes
    {
        protected SetupModes() { }

        public static readonly SetupMode Override = new SetupMode();
        public static readonly SetupMode Implement = new SetupMode();
    }
}
