using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

namespace Urasandesu.NAnonym.DI
{
    public sealed class LocalClass<TBase> where TBase : class
    {
        readonly Type tbaseType = typeof(TBase);
        readonly HashSet<MethodInfo> propertySet;
        readonly HashSet<MethodInfo> methodSet;

        public void Load()
        {
            throw new NotImplementedException();
        }

        public LocalClass()
        {
            propertySet = new HashSet<MethodInfo>(GetVirtualProperties(tbaseType));
            methodSet = new HashSet<MethodInfo>(GetVirtualMethods(tbaseType));

            // どちらにしろ、最初にモック作るべきっぽい。
        }

        static IEnumerable<MethodInfo> GetVirtualProperties(Type type)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var getMethod = default(MethodInfo);
                if ((getMethod = property.GetGetMethod(true)) != null && getMethod.IsVirtual)
                {
                    yield return getMethod;
                }

                var setMethod = default(MethodInfo);
                if ((setMethod = property.GetSetMethod(true)) != null && setMethod.IsVirtual)
                {
                    yield return setMethod;
                }
            }
        }

        static IEnumerable<MethodInfo> GetVirtualMethods(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var method in methods)
            {
                if (method.IsVirtual)
                {
                    yield return method;
                }
            }
        }

        [Obsolete]
        public LocalClass<TBase> Override(Action<LocalClass<TBase>> overrider)
        {
            if (overrider == null) throw new ArgumentNullException("overrider");
            overrider(this);
            return this;
        }

        public LocalClass<TBase> Setup(Action<LocalClass<TBase>> setupper)
        {
            throw new NotImplementedException();
        }


        // MEMO: メソッド側は式木作ればアクセスできそう
        [Obsolete]
        public LocalMethod<TBase> Method(Expression<Func<TBase, Action>> expression)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public LocalMethod<TBase, TResult> Method<TResult>(Func<TBase, Func<TResult>> method)
        {
            if (method == null) throw new ArgumentNullException("method");
            
            throw new NotImplementedException();
        }

        [Obsolete]
        public LocalMethod<TBase, T, TResult> Method<T, TResult>(Func<TBase, Func<T, TResult>> expression)
        {
            throw new NotImplementedException();
        }



        public LocalMethod<TBase> Method(Action method)
        {
            throw new NotImplementedException();
        }

        public LocalMethod<TBase, TResult> Method<TResult>(Func<TResult> method)
        {
            throw new NotImplementedException();
        }

        public LocalMethod<TBase, T, TResult> Method<T, TResult>(Func<T, TResult> method)
        {
            throw new NotImplementedException();
        }



        // MEMO: プロパティは先にテスター作るしかなさげ
        [Obsolete]
        public LocalPropertyGet<TBase, T> Property<T>(Func<TBase, Func<T>> expression)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public LocalPropertySet<TBase, T> Property<T>(Func<TBase, Action<T>> propertySet)
        {
            if (propertySet == null) throw new ArgumentException("propertySet");

            throw new NotImplementedException();
        }



        public LocalPropertyGet<TBase, T> Property<T>(Func<T> propertyGet)
        {
            throw new NotImplementedException();
        }

        public LocalPropertySet<TBase, T> Property<T>(Action<T> propertySet)
        {
            throw new NotImplementedException();
        }



        public TBase New()
        {
            throw new NotImplementedException();
        }
    }
}
