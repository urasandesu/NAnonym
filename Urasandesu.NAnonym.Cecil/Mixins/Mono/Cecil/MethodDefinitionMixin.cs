/* 
 * File: MethodDefinitionMixin.cs
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
using Mono.Cecil;
using System.Reflection;
using Urasandesu.NAnonym.Cecil.Mixins.System.Collections.Generic;
using Urasandesu.NAnonym.Linq;
using Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil.Cil;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Mixins.Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil;
using Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil;
using Mono.Cecil.Cil;

namespace Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil
{
    public static class MethodDefinitionMixin
    {
        public static bool Equivalent(this MethodDefinition x, MethodInfo y)
        {
            bool equals = ((MethodReference)x).Equivalent((MethodBase)y);
            equals = equals && x.Attributes.Equivalent(y.Attributes);
            return equals;
        }

        public static bool Equivalent(this MethodDefinition x, MethodDefinition y)
        {
            bool equals = x.DeclaringType.Equivalent(y.DeclaringType);
            equals = equals && x.Name == y.Name;
            equals = equals && x.Attributes == y.Attributes;
            equals = equals && x.Parameters.Equivalent(y.Parameters);
            // TODO: CloneEx でコピー対象となった項目も必要！
            return equals;
        }

        public static bool Equivalent(this MethodDefinition x, ConstructorInfo y)
        {
            bool equals = x.IsConstructor;
            equals = equals && x.Attributes.Equivalent(y.Attributes);
            equals = equals && x.Parameters.Equivalent(y.GetParameters());
            return equals;
        }

        public static MethodDefinition DuplicateWithoutBody(this MethodDefinition source)
        {
            var destination = new MethodDefinition(source.Name, source.Attributes, source.ReturnType);
            if (source.HasGenericParameters)
            {
                source.GenericParameters.Select(_ => _.Duplicate(destination)).AddRangeTo(destination.GenericParameters);
            }
            source.Parameters.Select(_ => _.Duplicate()).AddRangeTo(destination.Parameters);
            return destination;
        }

        public static MethodDefinition Duplicate(this MethodDefinition source)
        {
            var destination = source.DuplicateWithoutBody();
            destination.Body.InitLocals = source.Body.InitLocals;
            source.Body.Variables.Select(_ => _.Duplicate()).AddRangeTo(destination.Body.Variables);
            source.Body.Instructions.Select(_ => _.Duplicate()).AddRangeTo(destination.Body.Instructions);
            source.Body.ExceptionHandlers.Select(_ => _.Duplicate()).AddRangeTo(destination.Body.ExceptionHandlers);
            return destination;
        }

        public static PortableScope CarryPortableScope(this MethodDefinition methodDef)
        {
            var scope = new PortableScope(new MCMethodGeneratorImpl(methodDef));
            return scope;
        }

        public static void ExpressBody(this MethodDefinition methodDef, Action<ExpressiveMethodBodyGenerator> expression)    // TODO: ハンドラ化したほうが良いかも？
        {
            var gen = new ExpressiveMethodBodyGenerator(new MCMethodGeneratorImpl(methodDef));
            gen.ExpressBodyEnd(expression);
        }

        public static void ExpressBodyBefore(this MethodDefinition methodDef, Action<ExpressiveMethodBodyGenerator> expression, Instruction target)
        {
            var gen = new ExpressiveMethodBodyGenerator(new MCMethodGeneratorImpl(methodDef, ILEmitMode.InsertBefore, target));
            gen.ExpressBodyEnd(expression);
        }

        public static MethodInfo ToMethodInfo(this MethodDefinition methodDef)
        {
            var assembly = Assembly.Load(methodDef.DeclaringType.Module.Assembly.FullName);
            var type = assembly.GetType(methodDef.DeclaringType.FullName);
            BindingFlags bindingAttr = methodDef.HasThis ? BindingFlags.Instance : BindingFlags.Static;
            bindingAttr |= methodDef.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic;
            return type.GetMethod(methodDef.Name, bindingAttr, null, methodDef.Parameters.Select(parameter => parameter.ParameterType.ToType()).ToArray(), null);
        }

        public static ConstructorInfo ToConstructorInfo(this MethodDefinition methodDef)
        {
            var assembly = Assembly.Load(methodDef.DeclaringType.Module.Assembly.FullName);
            var type = assembly.GetType(methodDef.DeclaringType.FullName);
            BindingFlags bindingAttr = methodDef.HasThis ? BindingFlags.Instance : BindingFlags.Static;
            bindingAttr |= methodDef.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic;
            return type.GetConstructor(bindingAttr, null, methodDef.Parameters.Select(parameter => parameter.ParameterType.ToType()).ToArray(), null);
        }
    }
}

