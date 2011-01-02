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

            var weaveMethod = definer.WeaveMethod;
            var gen = bodyDefiner.Gen;
            var ownerType = definer.Parent.ConstructorWeaver.DeclaringType;
            var cachedMethod = definer.CachedMethod;
            var cachedSetting = definer.CachedSetting;
            var returnType = weaveMethod.Source.ReturnType;
            var parameterTypes = definer.ParameterTypes;
            var delegateInvoke = weaveMethod.DelegateType.GetMethodInstancePublic("Invoke", parameterTypes);

            gen.Eval(_ => _.If(_.Ld(cachedMethod.Name) == null));
            {
                var dynamicMethod = default(DynamicMethod);
                var dynamicMethodParameterTypes = new Type[] { ownerType }.Concat(parameterTypes).ToArray();
                gen.Eval(_ => _.Alloc(dynamicMethod).As(new DynamicMethod("dynamicMethod", _.X(returnType), _.X(dynamicMethodParameterTypes), _.X(ownerType), true)));

                var il = default(ILGenerator);
                gen.Eval(_ => _.Alloc(il).As(dynamicMethod.GetILGenerator()));
                gen.ExpressEmit(() => il,
                _gen =>
                {
                    // Not use the index 0 because it is reference of 'this'.
                    var variableIndexes = weaveMethod.Source.GetParameters().Select((parameter, index) => index + 1).ToArray();
                    _gen.Emit(_ => _.Return(_.Invoke(_.Ld<Delegate>(_.This(), cachedSetting), _.X(weaveMethod.Destination), _.LdArg(variableIndexes))));
                });

                gen.Eval(_ => _.St(cachedMethod.Name).As(dynamicMethod.CreateDelegate(_.X(weaveMethod.DelegateType), _.This())));
            }
            gen.Eval(_ => _.EndIf());
            gen.Eval(_ => _.Return(_.Invoke(_.Ld(cachedMethod.Name), _.X(delegateInvoke), _.Ld(weaveMethod.Source.ParameterNames()))));
        }
    }
}

