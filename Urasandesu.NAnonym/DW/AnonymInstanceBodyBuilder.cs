/* 
 * File: AnonymInstanceBodyBuilder.cs
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
    class AnonymInstanceBodyBuilder : MethodBodyWeaveBuilder
    {
        public AnonymInstanceBodyBuilder(MethodBodyWeaveDefiner parentBodyDefiner)
            : base(parentBodyDefiner)
        {
        }

        public override void Construct()
        {
            var bodyDefiner = ParentBodyDefiner.ParentBody;
            var definer = bodyDefiner.ParentBuilder.ParentDefiner;

            var wm = definer.WeaveMethod;
            var gen = bodyDefiner.Gen;
            var ownerType = definer.Parent.ConstructorWeaver.DeclaringType;
            var cache = definer.CachedMethod;
            var src = wm.Source;
            var dst = wm.Destination;
            var setting = definer.CachedSetting;
            var retType = wm.Source.ReturnType;
            var paramTypes = definer.ParameterTypes;
            var dlgInvoke = wm.DelegateType.GetMethodInstancePublic("Invoke", paramTypes);

            gen.Eval(() => Dsl.If(Dsl.Load(cache.Name) == null));
            {
                var dm = default(DynamicMethod);
                var dmParamTypes = new Type[] { ownerType }.Concat(paramTypes).ToArray();
                gen.Eval(() => Dsl.Allocate(dm).As(new DynamicMethod("", Dsl.Extract(retType), Dsl.Extract(dmParamTypes), Dsl.Extract(ownerType), true)));

                var il = default(ILGenerator);
                gen.Eval(() => Dsl.Allocate(il).As(dm.GetILGenerator()));
                gen.ExpressInternally(() => il, retType.ToTypeDecl(), dmParamTypes.Select(_ => _.ToTypeDecl()).ToArray(),
                _gen =>
                {
                    // Not use the index 0 because it is reference of 'this'.
                    var varIndexes = src.GetParameters().Select((_, index) => index + 1).ToArray();
                    //_gen.Emit(() => Dsl.Return(dst.Invoke(Dsl.Load<Delegate>(Dsl.This(), setting), new object[] { Dsl.LoadArguments(varIndexes) })));
                    _gen.Eval(() => Dsl.Return(dst.Invoke(setting.GetValue(Dsl.This()), new object[] { Dsl.LoadArguments(varIndexes) })));
                });

                gen.Eval(() => Dsl.Store(cache.Name).As(dm.CreateDelegate(Dsl.Extract(wm.DelegateType), Dsl.This())));
            }
            gen.Eval(() => Dsl.EndIf());
            //gen.Eval(() => Dsl.Return(Dsl.Invoke(Dsl.Load(cache.Name), Dsl.Extract(dlgInvoke), Dsl.Load(src.ParameterNames()))));
            gen.Eval(() => Dsl.Return(dlgInvoke.Invoke(Dsl.Load(cache.Name), new object[] { Dsl.Load(src.ParameterNames()) })));
        }
    }
}

