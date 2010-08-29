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
using System.Reflection.Emit;

namespace Urasandesu.NAnonym.CREUtilities
{
    class AssemblyRawData
    {
    }

    class ModuleRawData
    {
    }

    class TypeRawData
    {
    }

    class MethodRawData
    {
    }

    // TODO: やっぱり各 **Declaration/Generator に対応する RawData がないと自然に Serialize/Deserialize できない。

    [Serializable]
    public sealed class PortableScope2
    {
        public const string NameDelimiter = "<>";
        public const string MemberAccessOperator = ".";
        public const string AltMemberAccessOperator = "$";

        // MEMO: methodDecl を復元するための情報
        // methodDecl は items 復元後に辿ることができるが、わかりやすさのためにここから行うこととする。
        string methodDeclAssemblyQualifiedName;
        PortableScope2RawData rawData;

        [NonSerialized]
        internal IMethodBaseDeclaration methodDecl;


        // MEMO: items を復元するための情報
        Dictionary<PortableScope2ItemRawData, object> itemRawDataValueDictionary;
        string itemAssemblyQualifiedName;

        [NonSerialized]
        internal List<PortableScope2Item> items;

        
        PortableScope2()
        {
            itemRawDataValueDictionary = new Dictionary<PortableScope2ItemRawData, object>();
            items = new List<PortableScope2Item>();
        }

        internal PortableScope2(IMethodBaseDeclaration methodDecl)
            : this()
        {
            this.methodDecl = methodDecl;
            rawData = new PortableScope2RawData(methodDecl);
        }

        public static PortableScope2 CarryFrom(IMethodBaseDeclaration methodDecl)
        {
            // TODO: ここでは定義されている PortableScope 向けのフィールドチェックを行わないこと。
            throw new NotImplementedException();
        }

        public static PortableScope2 CarryFrom(object instance, string methodName, params Type[] parameterTypes)
        {
            // TODO: SR で構築（デフォルトの動作）
            // HACK: ん？デフォルトの動作全部 SR 側に振っておいて、Cecil 使う場合は Cecil 用の Assembly 参照設定すればいいだけ、にすれば良くない？
            // HACK: そうすると、static な所謂 Builder メソッドは基本的に extension method として定義するのが良さそう。
            throw new NotImplementedException();
        }

        public object DockWith(object target)
        {
            Required.NotDefault(target, () => target);

            var targetType = target.GetType();
            foreach (var itemRawDataValuePair in itemRawDataValueDictionary)
            {
                var itemRawData = itemRawDataValuePair.Key;
                var value = itemRawDataValuePair.Value;

                var fieldInfo = targetType.GetField(itemRawData.FieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                Required.NotDefault(fieldInfo, () => target, "The field \"" + itemRawData.FieldName + "\" is not found.");

                fieldInfo.SetValue(target, value);
            }

            return target;
            // HACK: 気軽に何度も設定し直せるよう、DynamicMethod 化するといい。
        }

        public bool TryDockWith(object target)
        {
            // HACK: こちらが無視版。デバッグ難しそう(笑)
            // MEMO: それを言うなら、DynamicMethod のデバッグ自体難しいので無問題。
            // MEMO: IL のデバッガも存在するらしい。メモメモ。
            throw new NotImplementedException();
        }

        public bool Contains<T>(Expression<Func<T>> variableRef)
        {
            // HACK: Contains 等の比較にあらかじめ methodDecl まで計算した状態を保持しておく方法が必要。
            // HACK: 頻繁に new されるようであれば、struct 化も検討。
            return itemRawDataValueDictionary.ContainsKey(new PortableScope2ItemRawData(rawData, TypeSavable.GetParamName(variableRef)));
        }

        public T GetValue<T>(Expression<Func<T>> variableRef)
        {
            return (T)itemRawDataValueDictionary[new PortableScope2ItemRawData(rawData, TypeSavable.GetParamName(variableRef))];
        }

        public T FetchValue<T>(Expression<Func<T>> variableRef, object target)
        {
            Required.NotDefault(target, () => target);

            var targetType = target.GetType();
            var itemRawData = new PortableScope2ItemRawData(rawData, TypeSavable.GetParamName(variableRef));
            var fieldInfo = targetType.GetField(itemRawData.FieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            Required.NotDefault(fieldInfo, () => target, "The field \"" + itemRawData.FieldName + "\" is not found.");

            return (T)fieldInfo.GetValue(target);
            // HACK: 気軽に何度も取得し直せるよう、DynamicMethod 化するといい。
        }

        public T PullValue<T>(Expression<Func<T>> variableRef, object target)
        {
            // MEMO: itemRawDataValueDictionary を更新しつつ取得、的な。
            throw new NotImplementedException();
        }

        public T PushValue<T>(Expression<Func<T>> variableRef, T value, object target)
        {
            // MEMO: itemRawDataValueDictionary を更新しつつ設定、的な。
            // 設定側は FetchValue にあたる処理は作らない。DockWith 時に整合性が取れなくなるため。
            throw new NotImplementedException();
        }

        public PortableScope2 SetValue<T>(Expression<Func<T>> variableRef, T value)
        {
            itemRawDataValueDictionary[new PortableScope2ItemRawData(rawData, TypeSavable.GetParamName(variableRef))] = value;
            return this;
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


            // TODO: これだと MCPortableScope2ItemImpl を生成すればいいのか、SRPortableScope2ItemImpl を生成すればいいのかわからない？

            // TODO: Deserialize の順は以下の通り。
            //       1. rawDataValueDictionary はシステムに任せる。
            //       2. itemAssemblyQualifiedName はシステムに任せる。
            //       3. 1. と 2. から items を復元する。
            //       4. methodDecl は items が知ってる。
            //       ん？ PortableScope2 <-> IMethodBaseDeclaration って 1 対 1 じゃね？
            //       PortableScope2ItemRawData も、実際の値ではなく、参照で持たせたほうが良さげ。
            //       PortableScope2RawData を導入。わかりやすさのため、methodDecl を Deserialize するための AssemblyQualifiedName も追加。
            throw new NotImplementedException();
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
    [Obsolete]
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
