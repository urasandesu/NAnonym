/* 
 * File: AnonymStaticBodyBuilder.cs
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

namespace Urasandesu.NAnonym.DW
{
    class AnonymStaticBodyBuilder : MethodBodyWeaveBuilder
    {
        public AnonymStaticBodyBuilder(MethodBodyWeaveDefiner parentBodyDefiner)
            : base(parentBodyDefiner)
        {
        }

        public override void Construct()
        {
            var bodyDefiner = ParentBodyDefiner.ParentBody;
            var definer = bodyDefiner.ParentBuilder.ParentDefiner;

            var weaveMethod = definer.WeaveMethod;
            var gen = bodyDefiner.Gen;
            var cachedMethod = definer.CachedMethod;
            var anonymousStaticMethodCache = definer.AnonymousStaticMethodCache;
            var returnType = weaveMethod.Source.ReturnType;
            var parameterTypes = definer.ParameterTypes;
            var delegateConstructor = weaveMethod.DelegateType.GetConstructor(new Type[] { typeof(object), typeof(IntPtr) });
            var delegateInvoke = weaveMethod.DelegateType.GetMethodInstancePublic("Invoke", parameterTypes);

            gen.Eval(_ => _.If(_.Ld(cachedMethod.Name) == null));
            {
                var dynamicMethod = default(DynamicMethod);
                gen.Eval(_ => _.Alloc(dynamicMethod).As(new DynamicMethod("dynamicMethod", _.X(returnType), _.X(parameterTypes), true)));

                var il = default(ILGenerator);
                gen.Eval(_ => _.Alloc(il).As(dynamicMethod.GetILGenerator()));
                gen.ExpressEmit(() => il,
                _gen =>
                {
                    _gen.Emit(_ => _.If(_.Ld<Delegate>(anonymousStaticMethodCache) == null));
                    {
                        _gen.Emit(_ => _.St<Delegate>(anonymousStaticMethodCache).As(_.New<Delegate>(_.X(delegateConstructor), new object[] { null, _.Ftn(_.X(weaveMethod.Destination)) })));
                    }
                    _gen.Emit(_ => _.EndIf());
                    var variableIndexes = weaveMethod.Source.GetParameters().Select((parameter, index) => index).ToArray();
                    _gen.Emit(_ => _.Return(_.Invoke(_.Ld<Delegate>(anonymousStaticMethodCache), _.X(delegateInvoke), _.LdArg(variableIndexes))));
                });
                gen.Eval(_ => _.St(cachedMethod.Name).As(dynamicMethod.CreateDelegate(_.X(weaveMethod.DelegateType))));
            }
            gen.Eval(_ => _.EndIf());
            gen.Eval(_ => _.Return(_.Invoke(_.Ld(cachedMethod.Name), _.X(delegateInvoke), _.Ld(weaveMethod.Source.ParameterNames()))));
        }
    }
}

