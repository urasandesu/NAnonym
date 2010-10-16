using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    public abstract class DependencyClass : MarshalByRefObject
    {
        DependencyClass modified;

        internal HashSet<TargetFieldInfo> TargetFieldInfoSet { get; private set; }
        internal HashSet<TargetMethodInfo> TargetMethodInfoSet { get; private set; }

        public DependencyClass()
        {
            TargetFieldInfoSet = new HashSet<TargetFieldInfo>();
            TargetMethodInfoSet = new HashSet<TargetMethodInfo>();
        }

        // T で渡すのは難しい（オブジェクトの初期化子とか入っちゃう可能性がある。e.g. Field<Uri>(() => uri, new Uri("http://www.google.com"));）
        // ここで new されても、設定クラスと異なるヒープであるため、参照することはできない。
        // オーバーロードは Urasandesu.NAnonym.ILTools.IILOperator と同じ分しか用意しない（これらは決め打ちで IL に埋め込めるものばかり。もしくはデフォルト値）。
        // 複雑なインスタンスを生成する場合は、Expressible の I/F を解放するようにする。
        public void Field<T>(Expression<Func<T>> reference) { throw new NotImplementedException(); }
        public void Field(Expression<Func<byte>> reference, byte arg) { throw new NotImplementedException(); }
        public void Field(Expression<Func<ConstructorInfo>> reference, ConstructorInfo con) { throw new NotImplementedException(); }
        public void Field(Expression<Func<double>> reference, double arg) { throw new NotImplementedException(); }
        public void Field(Expression<Func<FieldInfo>> reference, FieldInfo field) { throw new NotImplementedException(); }
        public void Field(Expression<Func<float>> reference, float arg) { throw new NotImplementedException(); }
        public void Field(Expression<Func<int>> reference, int arg) { Field(reference, _ => _.Expand(arg)); }
        public void Field(Expression<Func<long>> reference, long arg) { throw new NotImplementedException(); }
        public void Field(Expression<Func<MethodInfo>> reference, MethodInfo meth) { throw new NotImplementedException(); }
        public void Field(Expression<Func<sbyte>> reference, sbyte arg) { throw new NotImplementedException(); }
        public void Field(Expression<Func<short>> reference, short arg) { throw new NotImplementedException(); }
        public void Field(Expression<Func<string>> reference, string str) { throw new NotImplementedException(); }
        public void Field(Expression<Func<Type>> reference, Type cls) { throw new NotImplementedException(); }
        public void Field<T>(Expression<Func<T>> reference, Expression<Func<Expressible, T>> exp) { Field((LambdaExpression)reference, (LambdaExpression)exp, typeof(T)); }
        public void Field(Expression<Func<object>> reference, Expression<Func<Expressible, object>> exp, Type type) { throw new NotImplementedException(); }
        
        public void Field(LambdaExpression reference, LambdaExpression exp, Type type) 
        {
            TargetFieldInfoSet.Add(new TargetFieldInfo(reference, exp, type));
        }

        internal void Register()
        {
            modified = OnRegister();
            if (modified != null)
            {
                modified.Register();
            }
        }

        protected virtual DependencyClass OnRegister()
        {
            return null;
        }

        internal void Load()
        {
            OnLoad(modified);
            if (modified != null)
            {
                modified.Load();
            }
        }

        protected virtual void OnLoad(DependencyClass modified)
        {
        }
    }
}
