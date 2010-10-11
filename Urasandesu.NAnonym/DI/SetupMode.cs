using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    public class SetupMode
    {
    }

    public class SetupModes
    {
        protected SetupModes() { }

        public static readonly SetupMode Override = new SetupMode();
        public static readonly SetupMode Implement = new SetupMode();
    }

    //enum SetupMode
    //{
    //    Override,
    //    Implement,
    //    Replace,
    //    Before,
    //    After,
    //}
}
