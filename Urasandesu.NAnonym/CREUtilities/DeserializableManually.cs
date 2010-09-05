using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading;

namespace Urasandesu.NAnonym.CREUtilities
{
    [Serializable]
    public abstract class DeserializableManually : IDeserializableManually
    {
        const int False = 0;
        const int True = 1;

        [NonSerialized]
        static HashSet<object> gurdsAutomaticDeserializations = new HashSet<object>();

        [NonSerialized]
        int deserialized;

        object sync;

        bool deserializesManually;

        protected DeserializableManually(bool deserializesManually)
        {
            this.deserializesManually = deserializesManually;
            sync = new object();
        }

        [OnDeserializing]
        void OnDeserializingAutomatically(StreamingContext context)
        {
            if (deserializesManually)
                StartGurdAutomaticDeserialization();
        }

        public void OnDeserialized(StreamingContext context)
        {
            if (Interlocked.CompareExchange(ref deserialized, True, False) == True)
                return;
            OnDeserializedManually(context);
        }

        [OnDeserialized]
        void OnDeserializedAutomatically(StreamingContext context)
        {
            if (deserializesManually)
                ExitGurdAutomaticDeserialization();
            if (GurdsAutomaticDeserialization)
                return;
            OnDeserialized(context);
        }

        protected abstract void OnDeserializedManually(StreamingContext context);

        void StartGurdAutomaticDeserialization()
        {
            lock (sync)
            {
                if (!gurdsAutomaticDeserializations.Contains(this))
                    gurdsAutomaticDeserializations.Add(this);
            }
        }

        void ExitGurdAutomaticDeserialization()
        {
            lock (sync)
            {
                if (gurdsAutomaticDeserializations.Contains(this))
                    gurdsAutomaticDeserializations.Remove(this);
            }
        }

        bool GurdsAutomaticDeserialization
        {
            get
            {
                lock (sync)
                {
                    return gurdsAutomaticDeserializations.Contains(this);
                }
            }
        }
    }
}
