using System;
using System.Runtime.Serialization;
namespace Urasandesu.NAnonym.ILTools
{
    public interface IManuallyDeserializable
    {
        void OnDeserialized(StreamingContext context);
    }
}
