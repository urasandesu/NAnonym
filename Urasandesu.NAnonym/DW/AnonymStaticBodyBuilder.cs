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
using Urasandesu.NAnonym.ILTools;

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

            var wm = definer.WeaveMethod;
            var gen = bodyDefiner.Gen;
            var cache = definer.CachedMethod;
            var anonymCache = definer.AnonymousStaticMethodCache;
            var src = wm.Source;
            var dst = wm.Destination;
            var retType = src.ReturnType;
            var paramTypes = definer.ParameterTypes;
            var dlgType = wm.DelegateType;
            var dlgCtor = dlgType.GetConstructor(new Type[] { typeof(object), typeof(IntPtr) });
            var dlgInvoke = dlgType.GetMethodInstancePublic("Invoke", paramTypes);

            gen.Eval(() => Dsl.If(cache.GetValue(Dsl.This()) == null));
            {
                var dm = default(DynamicMethod);
                gen.Eval(() => Dsl.Allocate(dm).As(new DynamicMethod("", Dsl.Extract(retType), Dsl.Extract(paramTypes), true)));

                var il = default(ILGenerator);
                gen.Eval(() => Dsl.Allocate(il).As(dm.GetILGenerator()));
                gen.ExpressEmit(() => il,
                _gen =>
                {
                    _gen.Emit(() => Dsl.If(anonymCache.GetValue(null) == null));
                    {
                        _gen.Emit(() => anonymCache.SetValue(null, dlgCtor.Invoke(new object[] { null, Dsl.LoadPtr(dst) })));
                    }
                    _gen.Emit(() => Dsl.EndIf());
                    var varIndexes = src.GetParameters().Select((_, index) => index).ToArray();
                    _gen.Emit(() => Dsl.Return(dlgInvoke.Invoke(anonymCache.GetValue(null), new object[] { Dsl.LoadArguments(varIndexes) })));
                });
                //gen.Eval(() => Dsl.Store(cache.Name).As(dm.CreateDelegate(Dsl.Extract(dlgType))));
                gen.Eval(() => cache.SetValue(Dsl.This(), dm.CreateDelegate(Dsl.Extract(dlgType))));
            }
            gen.Eval(() => Dsl.EndIf());
            //gen.Eval(() => Dsl.Return(dlgInvoke.Invoke(Dsl.Load(cache.Name), new object[] { Dsl.Load(src.ParameterNames()) })));
            gen.Eval(() => Dsl.Return(dlgInvoke.Invoke(cache.GetValue(Dsl.This()), new object[] { Dsl.Load(src.ParameterNames()) })));
        }
    }
}

