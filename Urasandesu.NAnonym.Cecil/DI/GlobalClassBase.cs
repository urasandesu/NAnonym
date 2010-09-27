using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.Cecil.DI
{
    // MEMO: GlobalClass が Generic Type 持っちゃうから、共通で引き回せるような口用。
    public abstract class GlobalClassBase : MarshalByRefObject
    {
        GlobalClassBase modified;

        internal void Setup()
        {
            modified = OnSetup();
            if (modified != null)
            {
                modified.Setup();
            }
        }

        protected virtual GlobalClassBase OnSetup()
        {
            return null;
        }

        internal void Load()
        {
            OnLoad(modified);
            if (modified != null)
            {
                modified.Load();
            }
        }

        protected virtual void OnLoad(GlobalClassBase modified)
        {
        }

        protected internal abstract string CodeBase { get; }
        protected internal abstract string Location { get; }
    }
}
