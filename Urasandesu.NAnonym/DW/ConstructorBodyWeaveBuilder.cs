/* 
 * File: ConstructorBodyWeaveBuilder.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools;
using SR = System.Reflection;
using SRE = System.Reflection.Emit;
using Urasandesu.NAnonym.Mixins.System.Reflection;
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.NAnonym.Mixins.System.Reflection.Emit;

namespace Urasandesu.NAnonym.DW
{
    abstract class ConstructorBodyWeaveBuilder : BodyWeaveBuilder
    {
        public new ConstructorBodyWeaveDefiner ParentBodyDefiner { get { return (ConstructorBodyWeaveDefiner)base.ParentBodyDefiner; } }
        public ConstructorBodyWeaveBuilder(ConstructorBodyWeaveDefiner parentBodyDefiner)
            : base(parentBodyDefiner)
        {
        }

        public override void Construct()
        {
            var bodyWeaver = ParentBodyDefiner.ParentBody;
            var gen = bodyWeaver.Gen;
            var injectionDefiner = bodyWeaver.ParentBuilder.ParentDefiner;
            var injection = injectionDefiner.Parent;

            gen.Eval(_ => _.If(_.Ld(injectionDefiner.CachedConstructor.Name) == null));
            {
                var dynamicMethod = default(DynamicMethod);
                gen.Eval(_ => _.Alloc(dynamicMethod).As(new DynamicMethod(
                                                            "dynamicMethod",
                                                            typeof(void),
                                                            new Type[] { _.X(injection.DeclaringType) },
                                                            _.X(injection.DeclaringType),
                                                            true)));
                var il = default(ILGenerator);
                gen.Eval(_ => _.Alloc(il).As(dynamicMethod.GetILGenerator()));
                foreach (var injectionField in injection.FieldSet)
                {
                    var targetField = TypeSavable.GetFieldInfo(injectionField.FieldReference);
                    if (!injectionDefiner.InitializedDeclaringTypeConstructor[targetField.DeclaringType])
                    {
                        injectionDefiner.InitializedDeclaringTypeConstructor[targetField.DeclaringType] = true;

                        var declaringTypeConstructor = default(ConstructorInfo);
                        gen.Eval(_ => _.Alloc(declaringTypeConstructor).As(
                                               _.X(targetField.DeclaringType).GetConstructor(
                                                                    BindingFlags.Public | BindingFlags.Instance,
                                                                    null,
                                                                    Type.EmptyTypes,
                                                                    null)));

                        gen.Eval(_ => _.St<FieldInfo>(injection.GetFieldNameForDeclaringType(targetField.DeclaringType)).As(
                                               _.X(injection.DeclaringType).GetField(
                                                                    _.X(injection.GetFieldNameForDeclaringType(targetField.DeclaringType)),
                                                                    BindingFlags.Instance | BindingFlags.NonPublic)));
                        gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg_0));
                        gen.Eval(_ => il.Emit(SRE::OpCodes.Newobj, declaringTypeConstructor));
                        gen.Eval(_ => il.Emit(SRE::OpCodes.Stfld, _.Ld<FieldInfo>(injection.GetFieldNameForDeclaringType(targetField.DeclaringType))));
                    }

                    gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg_0));
                    gen.Eval(_ => il.Emit(SRE::OpCodes.Ldfld, _.Ld<FieldInfo>(injection.GetFieldNameForDeclaringType(targetField.DeclaringType))));
                    var actualTargetField = default(FieldInfo);
                    gen.Eval(_ => _.Alloc(actualTargetField).As(
                                           _.X(targetField.DeclaringType).GetField(
                                                                    _.X(targetField.Name),
                                                                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)));

                    var macro = new ExpressiveGeneratorMacro(gen);
                    macro.EvalEmitDirectives(TypeSavable.GetName(() => il), gen.ToDirectives(injectionField.Initializer));

                    gen.Eval(_ => il.Emit(SRE::OpCodes.Stfld, actualTargetField));
                }
                gen.Eval(_ => il.Emit(SRE::OpCodes.Ret));
                gen.Eval(_ => _.St<Action>(injectionDefiner.CachedConstructor.Name).As((Action)dynamicMethod.CreateDelegate(typeof(Action), _.This())));
            }
            gen.Eval(_ => _.EndIf());
            gen.Eval(_ => _.Ld<Action>(injectionDefiner.CachedConstructor.Name).Invoke());
        }
    }
}

