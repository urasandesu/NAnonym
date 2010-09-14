using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.ILTools
{
    // TODO: 名前空間のエイリアス使うときは::使ったほうが見やすいな。
    // TODO: PortableScope で扱うフィールド用の属性が欲しい。
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class PortableScopeAttribute : Attribute
    {
    }
}
