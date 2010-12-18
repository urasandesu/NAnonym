/* 
 * File: WeaveMethodInfo.cs
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
using Urasandesu.NAnonym.ILTools;
using System.Linq.Expressions;
using Urasandesu.NAnonym.Mixins.System;
using BuilderType = Urasandesu.NAnonym.DW.MethodBodyWeaveBuilderType;

namespace Urasandesu.NAnonym.DW
{
    class WeaveMethodInfo
    {
        public SetupMode Mode { get; set; }
        public MethodInfo Source { get; set; }
        public MethodInfo Destination { get; set; }
        public Type DelegateType { get; set; }
        public BuilderType DestinationType { get; private set; }

        public WeaveMethodInfo()
        {
        }

        public WeaveMethodInfo(SetupMode mode, MethodInfo source, MethodInfo destination, Type delegateType)
        {
            Mode = mode;
            Source = source;
            Destination = destination;
            DelegateType = delegateType;

            DestinationType = BuilderType.None;
            if (TypeAnalyzer.IsAnonymous(destination))
            {
                DestinationType |= BuilderType.Anonym;
            }

            if (destination.IsStatic)
            {
                DestinationType |= BuilderType.Static;
            }
            else
            {
                DestinationType |= BuilderType.Instance;
            }

            if (mode == DependencySetupModes.Before)
            {
                DestinationType |= BuilderType.Before;
            }
            else if (mode == DependencySetupModes.After)
            {
                DestinationType |= BuilderType.After;
            }

            if (delegateType.EquivalentWithoutGenericArguments(typeof(FuncWithPrev<>)) ||
                delegateType.EquivalentWithoutGenericArguments(typeof(FuncWithPrev<,>)) ||
                delegateType.EquivalentWithoutGenericArguments(typeof(FuncWithPrev<,,>)) ||
                delegateType.EquivalentWithoutGenericArguments(typeof(FuncWithPrev<,,,>)) ||
                delegateType.EquivalentWithoutGenericArguments(typeof(FuncWithPrev<,,,,>)) ||
                delegateType.EquivalentWithoutGenericArguments(typeof(ActionWithPrev)) ||
                delegateType.EquivalentWithoutGenericArguments(typeof(ActionWithPrev<>)) ||
                delegateType.EquivalentWithoutGenericArguments(typeof(ActionWithPrev<,>)) ||
                delegateType.EquivalentWithoutGenericArguments(typeof(ActionWithPrev<,,>)) ||
                delegateType.EquivalentWithoutGenericArguments(typeof(ActionWithPrev<,,,>)))
            {
                DestinationType |= BuilderType.WithPrev;
            }
        }

        public override bool Equals(object obj)
        {
            return this.NullableEquals(obj, that => that.Source);
        }

        public override int GetHashCode()
        {
            return Source.NullableGetHashCode();
        }
    }
}

