using System;
using System.Runtime.Serialization;
namespace Urasandesu.NAnonym.CREUtilities
{
    public interface IDeserializableManually
    {
        void OnDeserialized(StreamingContext context);
    }
}
