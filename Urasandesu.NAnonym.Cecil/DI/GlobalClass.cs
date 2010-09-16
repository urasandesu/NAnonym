using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Mono.Cecil;
using System.Reflection;
using Urasandesu.NAnonym.Cecil.ILTools;
//using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.Cecil.DI
{
    public class GlobalClass<TBase> : GlobalClassBase where TBase : class
    {
        readonly ModuleDefinition tbaseModule;
        readonly TypeDefinition tbaseType;

        // TODO: メソッド名とかを LocalClass 第2版に合わせたほうが良い。
        // TODO: SetUp じゃなくて Define にしよう。
        public GlobalClass()
        {
            // HACK: 成功してからコピーするとか、なにか起きたらロールバックするとかの機構があると良い。
            tbaseModule = ModuleDefinition.ReadModule(new Uri(typeof(TBase).Assembly.CodeBase).LocalPath);
            tbaseType = tbaseModule.GetType(typeof(TBase).FullName);
        }

        // MEMO: テスト中。
        public GlobalClass<TBase> Override(Action<GlobalClass<TBase>> overrider)
        {
            if (overrider == null) throw new ArgumentNullException("overrider");
            overrider(this);
            return this;
        }

        // TODO: 最終的にはこちらの I/F にする。
        public GlobalClass<TBase> SetUp(Action<GlobalClass<TBase>> overrider)
        {
            if (overrider == null) throw new ArgumentNullException("overrider");
            overrider(this);
            return this;
        }

        public GlobalMethod<TBase, T, TResult> Method<T, TResult>(Expression<Func<TBase, Func<T, TResult>>> expression)
        {
            var method = (MethodInfo)((ConstantExpression)(
                (MethodCallExpression)(((UnaryExpression)expression.Body).Operand)).Arguments[2]).Value;
            var targetMethod = tbaseType.Methods.FirstOrDefault(_method => _method.Equivalent(method));
            return new GlobalMethod<TBase, T, TResult>(targetMethod);
        }

        //public GlobalMethod<TBase, T, TResult> Method<T, TResult>(Func<T, TResult> expression)
        //{
        //    throw new NotImplementedException();
        //}

        protected override GlobalClassBase SetUp()
        {
            tbaseModule.Write(new Uri(typeof(TBase).Assembly.CodeBase).LocalPath);
            return this;
        }



    }
}
