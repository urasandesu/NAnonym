using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.Cecil.DI
{
    // MEMO: GlobalClass が Generic Type 持っちゃうから、共通で引き回せるような口用。
    // TODO: デザインに対象性持たせるなら、AcceptChanges で Load 呼ばせないとだめだ。
    // TODO: 名前を合わせるんだったら、AppDomain への拡張メソッドの命名も SetUp -> Load にしたほうが良さげ。
    public abstract class GlobalClassBase : MarshalByRefObject
    {
        public void Load()
        {
            var modified = SetUp();
        }

        protected abstract GlobalClassBase SetUp();
    }
}
