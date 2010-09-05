using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.CREUtilities
{
    // NOTE: インターフェースのメンバは Serialize できんとです。
    // NOTE: internal なクラスのメンバも Serialize できんとです。
    // NOTE: つまりこれ自身は Serialize の対象とはしない。上の階層の PortableScope で制御する。
    // NOTE: LocalDeclaration, FieldDeclaration の両方の属性を持つクラスということで落ち着いた。
    public interface IPortableScopeItem : ILocalDeclaration, IFieldDeclaration, IDeserializableManually
    {
        object Value { get; set; }
        object Source { get; }
        new string Name { get; }
    }
}
