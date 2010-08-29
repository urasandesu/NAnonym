using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.CREUtilities
{
    // NOTE: インターフェースのメンバは Serialize できんとです。
    // NOTE: internal なクラスのメンバも Serialize できんとです。
    // NOTE: つまりこれ自身は Serialize の対象とはしない。上の階層の PortableScope2 で制御する。
    // NOTE: LocalDeclaration, FieldDeclaration の両方の属性を持つクラスということで落ち着いた。
    public abstract class PortableScope2Item : ILocalDeclaration, IFieldDeclaration
    {
        public object Value { get; set; }

        #region ILocalDeclaration メンバ

        public abstract string Name { get; }

        #endregion

        #region IMemberDeclaration メンバ

        string IMemberDeclaration.Name
        {
            get { return IMemberDeclaration_Name; }
        }

        protected abstract string IMemberDeclaration_Name { get; }

        #endregion
    }
}
