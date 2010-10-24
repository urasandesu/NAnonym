using Urasandesu.NAnonym.DI;
using UND = Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    public class SetupModes : UND::SetupModes
    {
        protected SetupModes() : base() { }

        public static readonly SetupMode Replace = new SetupMode();
        public static readonly SetupMode Before = new SetupMode();
        public static readonly SetupMode After = new SetupMode();
    }
}
