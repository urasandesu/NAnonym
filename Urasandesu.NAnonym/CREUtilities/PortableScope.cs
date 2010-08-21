using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Mono.Cecil;
using MC = Mono.Cecil;
using System.Collections;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using Urasandesu.NAnonym.Linq;
using SR = System.Reflection;

namespace Urasandesu.NAnonym.CREUtilities
{
    // TODO: 名前空間のエイリアス使うときは::使ったほうが見やすいな。
    // TODO: PortableScope で扱うフィールド用の属性が欲しい。
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class PortableScopeAttribute : Attribute
    {
    }


    // NOTE: インターフェースのメンバは Serialize できんとです。
    // NOTE: internal なクラスのメンバも Serialize できんとです。
    // NOTE: つまりこれ自身は Serialize の対象とはしない。上の階層の PortableScope2 で制御する。
    // NOTE: LocalDeclaration, FieldDeclaration の両方の属性を持つクラスということで落ち着いた。
    public abstract class PortableScope2Item : ILocalDeclaration, IFieldDeclaration
    {
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

    class MCPortableScope2ItemImpl : PortableScope2Item
    {
        // NOTE: 構築中はこちら。まだ実体化してないので、GlobalAssemblyResolver.Resolve できない。
        public MCPortableScope2ItemImpl(FieldDefinition fieldDef, MC::Cil.VariableDefinition variableDef)
        {
            throw new NotImplementedException();
        }

        // NOTE: 構築後はこちら。Deserialize 時などに利用することを想定。RawData に含まれる情報を利用し、GlobalAssemblyResolver.Resolve で一気取りする。
        public MCPortableScope2ItemImpl(PortableScope2ItemRawData rawData)
        {
            throw new NotImplementedException();
        }

        public override string Name
        {
            get { throw new NotImplementedException(); }
        }

        protected override string IMemberDeclaration_Name
        {
            get { throw new NotImplementedException(); }
        }
    }

    class SRPortableScope2ItemImpl : PortableScope2Item
    {
        // NOTE: 構築中はこちら。まだ実体化してないので、Assembly.Load できない。
        public SRPortableScope2ItemImpl(SR::Emit.FieldBuilder fieldBuilder, SR::Emit.LocalBuilder localBuilder)
        {
            throw new NotImplementedException();
        }

        // NOTE: 構築後はこちら。Deserialize 時などに利用することを想定。RawData に含まれる情報を利用し、Assembly.Load で一気取りする。
        public SRPortableScope2ItemImpl(PortableScope2ItemRawData rawData)
        {
            throw new NotImplementedException();
        }

        public override string Name
        {
            get { throw new NotImplementedException(); }
        }

        protected override string IMemberDeclaration_Name
        {
            get { throw new NotImplementedException(); }
        }
    }

    [Serializable]
    public sealed class PortableScope2ItemRawData
    {
        public const string NameDelimiter = "<>";
        public const string MemberAccessOperator = ".";
        public const string AltMemberAccessOperator = "$";

        public PortableScope2ItemRawData(IMethodBaseDeclaration methodDecl, string localName)
        {
            // TODO: AssemblyFullName, ModuleName への設定が必要！！
            throw new NotImplementedException();
            TypeFullName = methodDecl.DeclaringType.FullName;
            MethodName = methodDecl.Name;
            ParameterTypeFullNames = methodDecl.Parameters.Select(parameter => parameter.ParameterType.FullName).ToArray();
            LocalName = localName;

            var fieldName = new StringBuilder();
            fieldName.Append(TypeFullName.Replace(MemberAccessOperator, AltMemberAccessOperator));
            fieldName.Append(NameDelimiter);
            fieldName.Append(methodDecl.Name);
            fieldName.Append(NameDelimiter);
            fieldName.Append(string.Join(
                NameDelimiter, 
                ParameterTypeFullNames.Select(
                    ParameterTypeFullName => ParameterTypeFullName.Replace(MemberAccessOperator, AltMemberAccessOperator)).ToArray()));
            fieldName.Append(NameDelimiter);
            this.fieldName = fieldName.ToString();
        }

        public string AssemblyFullName { get; private set; }
        public string ModuleName { get; private set; }
        public string TypeFullName { get; private set; }
        public string MethodName { get; private set; }
        public string[] ParameterTypeFullNames { get; private set; }
        public string LocalName { get; private set; }

        [NonSerialized]
        string fieldName;

        public string FieldName { get { return fieldName; } }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (object.ReferenceEquals(this, obj)) return true;

            var that = default(PortableScope2ItemRawData);
            if ((that = obj as PortableScope2ItemRawData) == null) return false;

            bool equals = TypeFullName == that.TypeFullName;
            equals = equals && MethodName == that.MethodName;
            equals = equals && ParameterTypeFullNames.Equivalent(that.ParameterTypeFullNames);
            equals = equals && LocalName == that.LocalName;

            return equals;
        }

        public override int GetHashCode()
        {
            // HACK: このクラスは Immutable なので HashCode はキャッシュ可能。
            int typeFullNameHashCode = TypeFullName.GetHashCodeNullable();
            int methodNameHashCode = MethodName.GetHashCodeNullable();
            int parameterTypeFullNamesHashCode = ParameterTypeFullNames.GetHashCodeNullable();
            int localNameHashCode = LocalName.GetHashCodeNullable();

            return typeFullNameHashCode ^ methodNameHashCode ^ parameterTypeFullNamesHashCode ^ localNameHashCode;
        }
    }

    [Serializable]
    public class PortableScope2
    {
        Dictionary<PortableScope2ItemRawData, object> fieldNameValueDictionary;

        [NonSerialized]
        List<PortableScope2Item> portableScope2Items;

        public static PortableScope2 CarryFrom(MethodDefinition methodDef)
        {
            // NOTE: 構築順序。
            //       1. portableScope2Fields
            //       2. fieldNameValueDictionary
            throw new NotImplementedException();
        }

        public void DockWith(object target)
        {
            throw new NotImplementedException();
        }

        public bool Contains<T>(Expression<Func<T>> variableRef)
        {
            throw new NotImplementedException();
        }

        public T GetValue<T>(Expression<Func<T>> variableRef)
        {
            throw new NotImplementedException();
        }

        public void SetValue<T>(Expression<Func<T>> variableRef, T value)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<PortableScope2Item> Items
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        [OnSerializing]
        void OnSerializing(StreamingContext context)
        {
            // TODO: なにもしなくて良いのかも？
        }

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            // TODO: fieldNameValueDictionary から portableScope2Fields の再構築。
            // TODO: fieldNameValueDictionary だけだと情報が足りない。index で引ける場所に付加情報を持たせる必要がある。
            // TODO: 必要な情報は？：型の完全名、メソッドを識別できる情報、変数の名前
            // TODO: ん？この情報持ってるのが PortableScope2Field じゃねーの？
            // TODO: PortableScope2Field には、これらの情報を使って、System.Reflection or Mono.Cecil の特化クラスからオブジェクトの状態を再構築させる機能を持たせたい。
            // TODO: ということで、新しいクラスが必要。
            // TODO: 使う側の I/F 変えたくないなー。
            // TODO: いや、内部的に使えれば良いだけ。
            // TODO: いや、この辺りの情報は、fieldNameValueDictionary が持ってるんじゃね？
            // TODO: 現状は、メソッド名<>パラメータ型の完全名1<>パラメータ型の完全名2<>...<>パラメータ型の完全名n<>変数名
            // TODO: これを、定義型の完全名<>メソッド名<>パラメータ型の完全名1<>パラメータ型の完全名2<>...<>パラメータ型の完全名n<>変数名にすればいい。
            // TODO: さらに、fieldNameValueDictionary の型を Dictionary<string, object> → Dictionary<PortableScope2FieldRawData, object> にすればいい。
            // TODO: PortableScope2FieldRawData に static な Parse メソッド付ければいい。

        }

    }


    // NOTE: あー。わざわざ生成時にインスタンス必要なくしたのって、
    //       1. Serializable なデータなら AppDomain 越える前に設定したいかもしんない。
    //       2. もちろん AppDomain 超えた後に設定してもいい。
    //       って自由度持たせるためかー。
    // NOTE: そうなるとやっぱり名前がおかしい。「持ち運び可能」ってのはどうなの？
    //       ・定義から情報を作成する、括り出す、引き剥がす、分離する、…
    //       ・情報を紐付ける、設定する、…
    //       ・インスタンスを再初期化する、再構築する、結びつける、くっつける、…
    //       ことができそうな的確な名前を。
    [Serializable]
    public sealed class PortableScope
    {
        readonly string prefix;
        readonly Dictionary<string, object> variables;

        public PortableScope(IMethodGenerator methodDecl)
        {
            prefix = MakeFieldNamePrefix(methodDecl);
            variables = new Dictionary<string, object>();
        }

        public void Bind<T>(Expression<Func<T>> variableRef, T value)
        {
            var fieldInfo = (FieldInfo)((MemberExpression)variableRef.Body).Member;

            // TODO: 指定された MethodDefinition
            // 名前を一意にするのもいいけど CustomAttribute で探したほうがいいのかも？
            //method.DeclaringType.Fields.Select(field=>field.CustomAttributes.Where(attribute=>attribute.
            // メソッド名$引数1$引数2$…$引数n$ローカル変数名 みたいな名前なら、まず被らないか。
            //string fieldName = MakeFieldName(method, fieldInfo.Name);
            //if (method.DeclaringType.GetField(fieldName) == null)
            //{
            //    // TODO: Required に持っていけるといい。
            //    throw new ArgumentException();
            //}

            string fieldName = prefix + fieldInfo.Name;
            variables[fieldName] = value;   // 上書きでいいや。
            //if (!variables.ContainsKey(fieldName))
            //{
            //    variables.Add(fieldInfo, value);
            //}
        }

        public static string MakeFieldNamePrefix(IMethodBaseGenerator methodDecl)
        {
            // TODO: 名前が重複する可能性を完全に排除するため、連番をどこかに付ける。
            var fieldName = new StringBuilder();
            fieldName.Append(methodDecl.Name);
            fieldName.Append("<>");
            fieldName.Append(string.Join("<>", methodDecl.Parameters.Select(parameter => parameter.ParameterType.FullName.Replace(".", "$")).ToArray()));
            fieldName.Append("<>");
            return fieldName.ToString();
        }

        public static string MakeFieldName(IMethodBaseGenerator methodDecl, string localVariableName)
        {
            return MakeFieldNamePrefix(methodDecl) + localVariableName;
        }

        public void Reinitialize(object instance)
        {
            var t = instance.GetType();
            foreach (var fieldNameValuePair in variables)
            {
                string fieldName = fieldNameValuePair.Key;
                object value = fieldNameValuePair.Value;

                t.GetField(fieldName).SetValue(instance, value);
            }
        }

        // TODO: static メンバ用。
        public void Bind<T>()
        {
            throw new NotImplementedException();
        }
    }
}
