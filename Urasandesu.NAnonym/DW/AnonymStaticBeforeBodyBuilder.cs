/* 
 * File: AnonymStaticBeforeBodyBuilder.cs
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
using Urasandesu.NAnonym.Mixins.System.Reflection;
using SRE = System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DW
{
    class AnonymStaticBeforeBodyBuilder : MethodBodyWeaveBuilder
    {
        public AnonymStaticBeforeBodyBuilder(MethodBodyWeaveDefiner parentBodyDefiner)
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
            var anonymousStaticMethodCache = definer.AnonymousStaticMethodCache;
            var returnType = weaveMethod.Source.ReturnType;
            var parameterTypes = definer.ParameterTypes;
            var baseMethod = definer.BaseMethod;

            gen.Eval(_ => _.If(_.Ld(cachedMethod.Name) == null));
            {
                var dynamicMethod = default(DynamicMethod);
                gen.Eval(_ => _.Alloc(dynamicMethod).As(new DynamicMethod(
                                                            "dynamicMethod",
                                                            _.X(returnType),
                                                            _.X(parameterTypes),
                                                            true)));

                var delegateConstructor = default(ConstructorInfo);
                var invokeForLocal = default(MethodInfo);
                gen.Eval(_ => _.Alloc(delegateConstructor).As(_.X(weaveMethod.DelegateType).GetConstructor(
                                                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                                    null,
                                                    new Type[] 
                                                    { 
                                                        typeof(Object), 
                                                        typeof(IntPtr) 
                                                    }, null)));
                gen.Eval(_ => _.Alloc(invokeForLocal).As(_.X(weaveMethod.DelegateType).GetMethod(
                                                    "Invoke",
                                                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                                    null, _.X(parameterTypes), null)));

                var cacheField = default(FieldInfo);
                gen.Eval(_ => _.Alloc(cacheField).As(Type.GetType(_.X(anonymousStaticMethodCache.DeclaringType.AssemblyQualifiedName)).GetField(
                                                    _.X(anonymousStaticMethodCache.Name),
                                                    BindingFlags.NonPublic | BindingFlags.Static)));

                var targetMethod = default(MethodInfo);
                gen.Eval(_ => _.Alloc(targetMethod).As(Type.GetType(_.X(weaveMethod.Destination.DeclaringType.AssemblyQualifiedName)).GetMethod(
                                                    _.X(weaveMethod.Destination.Name),
                                                    BindingFlags.NonPublic | BindingFlags.Static)));

                var beforeSourceConstructor = default(ConstructorInfo);
                gen.Eval(_ => _.Alloc(beforeSourceConstructor).As(_.X(parameterTypes[0]).GetConstructor(
                                                    BindingFlags.Instance | BindingFlags.Public,
                                                    null,
                                                    new Type[]
                                                    {
                                                        typeof(Type),
                                                        typeof(MethodBase)
                                                    }, null)));

                var il = default(ILGenerator);
                gen.Eval(_ => _.Alloc(il).As(dynamicMethod.GetILGenerator()));
                var label = default(Label);
                gen.Eval(_ => _.Alloc(label).As(il.DefineLabel()));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Ldsfld, cacheField));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Brtrue_S, label));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Ldnull));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Ldftn, targetMethod));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Newobj, delegateConstructor));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Stsfld, cacheField));
                gen.Eval(_ => il.MarkLabel(label));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Ldsfld, cacheField));

                
                // TODO: この辺実装中。
                gen.Eval(_ => il.Emit(SRE::OpCodes.Ldtoken, _.This().GetType()));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Call, MethodInfoMixin.GetTypeFromHandleInfo));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Call, (MethodInfo)MethodBase.GetCurrentMethod()));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Newobj, beforeSourceConstructor));
                
                
                for (int parametersIndex = 0; parametersIndex < parameterTypes.Length; parametersIndex++)
                {
                    switch (parametersIndex)
                    {
                        case 0:
                            gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg_1));
                            break;
                        case 1:
                            gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg_2));
                            break;
                        case 2:
                            gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg_3));
                            break;
                        case 3:
                            gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg, (short)4));
                            break;
                        case 4:
                            gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg, (short)5));
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }
                gen.Eval(_ => il.Emit(SRE::OpCodes.Callvirt, invokeForLocal));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Ret));
                gen.Eval(_ => _.St(cachedMethod.Name).As(dynamicMethod.CreateDelegate(_.X(weaveMethod.DelegateType))));
            }
            gen.Eval(_ => _.EndIf());
            var invokeForInvoke = weaveMethod.DelegateType.GetMethod(
                                                    "Invoke",
                                                    BindingFlags.Public | BindingFlags.Instance,
                                                    null,
                                                    parameterTypes,
                                                    null);
            gen.Eval(_ => _.Invoke(_.Ld(cachedMethod.Name), _.X(invokeForInvoke), _.Ld(weaveMethod.Source.ParameterNames())));
            gen.Eval(_ => _.Return(_.Invoke(_.This(), (MethodInfo)_.Ftn(_.This(), _.X(baseMethod)), _.Ld(weaveMethod.Source.ParameterNames()))));
        }
    }
}
