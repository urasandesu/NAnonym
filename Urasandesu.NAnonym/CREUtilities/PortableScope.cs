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
using System.Threading;

namespace Urasandesu.NAnonym.CREUtilities
{
    [Serializable]
    public sealed class PortableScope : DeserializableManually
    {
        public const string NameDelimiter = "<>";
        public const string MemberAccessOperator = ".";
        public const string AltMemberAccessOperator = "$";

        // MEMO: DeserializableManually でデシリアライズの順序が制御できるため、無駄に自動デシリアライズする必要は無くなったはず？？
        [NonSerialized]
        PortableScopeRawData rawData;

        internal IMethodBaseDeclaration methodDecl; // テスト用のモックも兼ねる

        [NonSerialized]
        Dictionary<PortableScopeItemRawData, IPortableScopeItem> itemRawDataItemDictionary;


        List<PortableScopeItemRawData> itemRawDatas;
        List<IPortableScopeItem> items;

        [NonSerialized]
        ReadOnlyCollection<IPortableScopeItem> readonlyItems;


        internal PortableScope(IMethodBaseDeclaration methodDecl)
            : base(true)
        {
            this.methodDecl = methodDecl;
            rawData = new PortableScopeRawData(methodDecl);
            itemRawDatas = new List<PortableScopeItemRawData>();
            items = new List<IPortableScopeItem>();
            Initialize(this, default(StreamingContext));
        }

        static void Initialize(PortableScope that, StreamingContext context)
        {
            that.itemRawDataItemDictionary = new Dictionary<PortableScopeItemRawData, IPortableScopeItem>();
            var methodDecl = default(IMethodBaseDeclaration);
            if ((methodDecl = context.Context as IMethodBaseDeclaration) != null)
            {
                that.rawData = new PortableScopeRawData(methodDecl);
                for (int i = 0; i < that.itemRawDatas.Count; i++)
                {
                    that.items[i].OnDeserialized(context);
                    that.itemRawDataItemDictionary.Add(that.itemRawDatas[i], that.items[i]);
                }
            }
            that.readonlyItems = new ReadOnlyCollection<IPortableScopeItem>(that.items);
        }

        public static PortableScope CarryFrom(IMethodBaseDeclaration methodDecl)
        {
            // TODO: ここでは定義されている PortableScope 向けのフィールドチェックを行わないこと。
            throw new NotImplementedException();
        }

        public static PortableScope CarryFrom(object instance, string methodName, params Type[] parameterTypes)
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
            foreach (var itemRawDataItemPair in itemRawDataItemDictionary)
            {
                var itemRawData = itemRawDataItemPair.Key;
                var item = itemRawDataItemPair.Value;

                var fieldInfo = targetType.GetField(itemRawData.FieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                Required.NotDefault(fieldInfo, () => target, "The field \"" + itemRawData.FieldName + "\" is not found.");

                fieldInfo.SetValue(target, item.Value);
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
            return itemRawDataItemDictionary.ContainsKey(new PortableScopeItemRawData(methodDecl, rawData, TypeSavable.GetParamName(variableRef)));
        }

        public T GetValue<T>(Expression<Func<T>> variableRef)
        {
            return (T)itemRawDataItemDictionary[new PortableScopeItemRawData(methodDecl, rawData, TypeSavable.GetParamName(variableRef))].Value;
        }

        public T FetchValue<T>(Expression<Func<T>> variableRef, object target)
        {
            Required.NotDefault(target, () => target);

            var targetType = target.GetType();
            var itemRawData = new PortableScopeItemRawData(methodDecl, rawData, TypeSavable.GetParamName(variableRef));
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

        public PortableScope SetValue<T>(Expression<Func<T>> variableRef, T value)
        {
            var itemRawData = new PortableScopeItemRawData(methodDecl, rawData, TypeSavable.GetParamName(variableRef));
            var item = methodDecl.NewPortableScopeItem(itemRawData, value);
            if (!itemRawDataItemDictionary.ContainsKey(itemRawData))
            {
                itemRawDatas.Add(itemRawData);
                items.Add(item);
            }
            itemRawDataItemDictionary[itemRawData] = item;
            return this;
        }

        public ReadOnlyCollection<IPortableScopeItem> Items
        {
            get
            {
                return readonlyItems;
            }
        }

        protected override void OnDeserializedManually(StreamingContext context)
        {
            methodDecl.OnDeserialized(context);
            Initialize(this, new StreamingContext(context.State, methodDecl));
        }
    }
}
