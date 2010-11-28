using Urasandesu.NAnonym.DW;
using UND = Urasandesu.NAnonym.DW;

namespace Urasandesu.NAnonym.Cecil.DW
{
    public class SetupModes : UND::SetupModes
    {
        protected SetupModes() : base() { }

        public static readonly SetupMode Replace = new SetupMode();
        public static readonly SetupMode Before = new SetupMode();
        public static readonly SetupMode After = new SetupMode();
    }
}
