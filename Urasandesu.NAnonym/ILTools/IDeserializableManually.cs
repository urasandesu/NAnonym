using System;
using System.Runtime.Serialization;
namespace Urasandesu.NAnonym.ILTools
{
    public interface IDeserializableManually
    {
        void OnDeserialized(StreamingContext context);
    }
}
