using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.Linq;
using System.Runtime.Serialization;
using System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{

    [Serializable]
    public sealed class PortableScopeItemRawData
    {
        // MEMO: ローカル変数のインデックスなしで当てる版。
        //       ・定義済みの型/メソッド/フィールド情報が必要。
        //       ・フィールド名は、TypeName<>MethodName<>ParamTypeName1<>ParamTypeName2<>...<>ParamTypeNameN<>LocalName<>LocalIndex なので、引っ張ることが可能。
        public PortableScopeItemRawData(IMethodBaseDeclaration methodDecl, PortableScopeRawData rawData, string localName)
        {
            this.methodDecl = Required.NotDefault(methodDecl, () => methodDecl);
            RawData = rawData;
            LocalName = localName;
        }

        // MEMO: ローカル変数のインデックス必須晩。
        //       ・構築中の型/メソッド/フィールド情報が必要。
        //       ・フィールド名はあらかじめ Parse できるようにする
        public PortableScopeItemRawData(IMethodBaseGenerator methodGen, string localName, int localIndex)
        {
            this.methodDecl = null;
            RawData = new PortableScopeRawData(methodGen);
            LocalName = localName;
            this.localIndex = localIndex;
            this.localIndexInitialized = true;
        }

        [NonSerialized]
        IMethodBaseDeclaration methodDecl;

        public PortableScopeRawData RawData { get; private set; }
        public string LocalName { get; private set; }

        bool localIndexInitialized;
        int localIndex;
        public int LocalIndex 
        {
            get
            {
                if (!localIndexInitialized)
                {
                    string truncatedFieldName = TruncatedFieldName;
                    var fieldDecl = methodDecl.DeclaringType.Fields.First(_fieldDecl => _fieldDecl.Name.IndexOf(truncatedFieldName) == 0);
                    this.localIndex = int.Parse(fieldDecl.Name.Substring(fieldDecl.Name.LastIndexOf(PortableScope.NameDelimiter) + PortableScope.NameDelimiter.Length));
                    localIndexInitialized = true;
                }

                return this.localIndex;
            }
        }

        string fieldName;
        public string FieldName
        {
            get 
            {
                if (this.fieldName == null)
                {
                    var fieldName = new StringBuilder();
                    fieldName.Append(TruncatedFieldName);
                    fieldName.Append(PortableScope.NameDelimiter);
                    fieldName.Append(LocalIndex);
                    this.fieldName = fieldName.ToString();
                }

                return this.fieldName;
            }
        }

        string truncatedFieldName;
        public string TruncatedFieldName
        {
            get
            {
                if (this.truncatedFieldName == null)
                {
                    var truncatedFieldName = new StringBuilder();
                    truncatedFieldName.Append(RawData.MethodName);
                    truncatedFieldName.Append(PortableScope.NameDelimiter);
                    truncatedFieldName.Append(string.Join(
                        PortableScope.NameDelimiter,
                        RawData.ParameterTypeFullNames.Select(
                            ParameterTypeFullName =>
                                ParameterTypeFullName.Replace(PortableScope.MemberAccessOperator, PortableScope.AltMemberAccessOperator)
                        ).ToArray()));
                    truncatedFieldName.Append(PortableScope.NameDelimiter);
                    truncatedFieldName.Append(LocalName);
                    this.truncatedFieldName = truncatedFieldName.ToString();
                }

                return this.truncatedFieldName;
            }
        }

        public static bool operator ==(PortableScopeItemRawData x, PortableScopeItemRawData y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(PortableScopeItemRawData x, PortableScopeItemRawData y)
        {
            return !(x == y);
        }

        public bool Equals(PortableScopeItemRawData that)
        {
            bool equals = RawData == that.RawData;
            equals = equals && LocalName == that.LocalName;
            return equals;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is PortableScopeItemRawData)) return false;

            var that = (PortableScopeItemRawData)obj;

            return Equals(that);
        }

        public override int GetHashCode()
        {
            // HACK: このクラスは Immutable なので HashCode はキャッシュ可能。
            int rawDataHashCode = RawData.GetHashCode();
            int localNameHashCode = LocalName.GetHashCodeOrDefault();

            return rawDataHashCode ^ localNameHashCode;
        }

        [OnSerializing]
        void OnSerializingAutomatically(StreamingContext context)
        {
            string fieldName = FieldName;
        }
    }
}
