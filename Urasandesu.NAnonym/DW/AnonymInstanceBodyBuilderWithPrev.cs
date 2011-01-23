/* 
 * File: AnonymInstanceBodyBuilderWithPrev.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2010 Akira Sugiura
 *  
 *  This software is MIT License.
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */


using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.NAnonym.Mixins.System.Reflection;
using Urasandesu.NAnonym.Mixins.Urasandesu.NAnonym.ILTools;
using SRE = System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DW
{
    class AnonymInstanceBodyBuilderWithPrev : MethodBodyWeaveBuilder
    {
        public AnonymInstanceBodyBuilderWithPrev(MethodBodyWeaveDefiner parentBodyDefiner)
            : base(parentBodyDefiner)
        {
        }

        public override void Construct()
        {
            var bodyDefiner = ParentBodyDefiner.ParentBody;
            var definer = bodyDefiner.ParentBuilder.ParentDefiner;

            var weaveMethod = definer.WeaveMethod;
            var gen = bodyDefiner.Gen;
            var ownerType = definer.Parent.ConstructorWeaver.DeclaringType;
            var cachedMethod = definer.CachedMethod;
            var cachedSetting = definer.CachedSetting;
            var returnType = weaveMethod.Source.ReturnType;
            var parameterTypes = definer.ParameterTypes;
            var baseMethod = definer.BaseMethod;
            var delegateInvoke = weaveMethod.DelegateType.GetMethodInstancePublic("Invoke", parameterTypes);

            gen.Eval(() => Dsl.If(Dsl.Load(cachedMethod.Name) == null));
            {
                var dynamicMethod = default(DynamicMethod);
                var dynamicMethodParameterTypes = new Type[] { ownerType }.Concat(parameterTypes).ToArray();
                gen.Eval(() => Dsl.Allocate(dynamicMethod).As(new DynamicMethod("dynamicMethod", Dsl.Extract(returnType), Dsl.Extract(dynamicMethodParameterTypes), Dsl.Extract(ownerType), true)));

                var il = default(ILGenerator);
                gen.Eval(() => Dsl.Allocate(il).As(dynamicMethod.GetILGenerator()));
                gen.ExpressEmit(() => il,
                _gen =>
                {
                    // Not use the index 0 because it is reference of 'this'. The index 1 is specified previous method.
                    var variableIndexes = new int[] { 1 };
                    variableIndexes = variableIndexes.Concat(weaveMethod.Source.GetParameters().Select((parameter, index) => index + 1 + variableIndexes.Length)).ToArray();
                    //_gen.Emit(() => Dsl.Return(Dsl.Invoke(Dsl.Load<Delegate>(Dsl.This(), cachedSetting), Dsl.Extract(weaveMethod.Destination), Dsl.LoadArguments(variableIndexes))));
                    _gen.Emit(() => Dsl.Return(weaveMethod.Destination.Invoke(Dsl.Load<object>(Dsl.This(), cachedSetting), new object[] { Dsl.LoadArguments(variableIndexes) })));
                });
                gen.Eval(() => Dsl.Store(cachedMethod.Name).As(dynamicMethod.CreateDelegate(Dsl.Extract(weaveMethod.DelegateType), Dsl.This())));
            }
            gen.Eval(() => Dsl.EndIf());

            var delegateForBaseConstructor = parameterTypes[0].GetConstructor(new Type[] { typeof(object), typeof(IntPtr) });
            var delegateForBase = default(Delegate);
            //gen.Eval(() => Dsl.Allocate(delegateForBase).As(Dsl.New<Delegate>(Dsl.Extract(delegateForBaseConstructor), Dsl.LoadPtr(Dsl.This(), baseMethod))));
            gen.Eval(() => Dsl.Allocate(delegateForBase).As((Delegate)delegateForBaseConstructor.Invoke(new object[] { Dsl.This(), Dsl.LoadPtr(baseMethod) })));
            var variableNames = new string[] { TypeSavable.GetName(() => delegateForBase) }.Concat(weaveMethod.Source.ParameterNames()).ToArray();
            //gen.Eval(() => Dsl.Return(Dsl.Invoke(Dsl.Load(cachedMethod.Name), Dsl.Extract(delegateInvoke), Dsl.Load(variableNames))));
            gen.Eval(() => Dsl.Return(delegateInvoke.Invoke(Dsl.Load(cachedMethod.Name), new object[] { Dsl.Load(variableNames) })));
        }
    }


    
    
    
    
    class AnonymInstanceBodyBuilderWithAlias : MethodBodyWeaveBuilder
    {
        public AnonymInstanceBodyBuilderWithAlias(MethodBodyWeaveDefiner parentBodyDefiner)
            : base(parentBodyDefiner)
        {
        }

        public override void Construct()
        {
            throw new NotImplementedException();
        }
    }

    class AnonymInstanceBodyBuilderWithAliasSet : MethodBodyWeaveBuilder
    {
        public AnonymInstanceBodyBuilderWithAliasSet(MethodBodyWeaveDefiner parentBodyDefiner)
            : base(parentBodyDefiner)
        {
        }

        public override void Construct()
        {
            throw new NotImplementedException();
        }
    }

    class AnonymInstanceBodyBuilderWithAliasList : MethodBodyWeaveBuilder
    {
        public AnonymInstanceBodyBuilderWithAliasList(MethodBodyWeaveDefiner parentBodyDefiner)
            : base(parentBodyDefiner)
        {
        }

        public override void Construct()
        {
            throw new NotImplementedException();
        }
    }
}

