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
    [Serializable]
    public abstract class PortableScope2Item
    {
        protected PortableScope2Item()
        {
            fieldDecl = default(IFieldDeclaration);
        }

        [NonSerialized]
        IFieldDeclaration fieldDecl;
        public IFieldDeclaration Field
        {
            get
            {
                return fieldDecl;
            }
        }

        // 復元に必要なのは
        public string TypeFullName { get; private set; }
        public string FieldName { get; private set; }
        //public BindingFlags FieldBindingAttribute { get; private set; }   // AddPortableField で決め打ちできる。使う人が知る必要はない。

        // NOTE: 擬似 Local 変数情報へ変換可能というイメージで
        [NonSerialized]
        ILocalDeclaration local;
        public ILocalDeclaration Local  // Cecil 側でしかできない？ひとまず遅延評価させるしかないかも？
        {
            get
            {
                if (local == null)
                {
                    local = ParseLocal();
                }
                return local;
            }
        }
        public object Value { get; private set; }

        protected abstract ILocalDeclaration ParseLocal();   // NOTE: 引数はなにが必要？

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    class MCPortableScope2ItemImpl : PortableScope2Item
    {
        protected override ILocalDeclaration ParseLocal()
        {
            throw new NotImplementedException();
        }
    }

    class SRPortableScope2ItemImpl : PortableScope2Item
    {
        protected override ILocalDeclaration ParseLocal()
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public abstract class PortableScope2 : IEnumerable<PortableScope2Item>
    {
        public static PortableScope2 CarryOut(MethodDefinition methodDef)
        {
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

        public T Get<T>(Expression<Func<T>> variableRef)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(Expression<Func<T>> variableRef, T value)
        {
            throw new NotImplementedException();
        }

        #region IEnumerable<PortableScope2Item> メンバ

        public IEnumerator<PortableScope2Item> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable メンバ

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
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
