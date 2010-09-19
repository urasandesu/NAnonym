using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.ILTools
{
    // TODO: PortableScope で扱うフィールド用の属性が欲しい。
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class PortableScopeAttribute : Attribute
    {
    }
}
