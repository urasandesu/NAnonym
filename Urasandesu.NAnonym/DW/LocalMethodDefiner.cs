/* 
 * File: LocalMethodDefiner.cs
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
using System.Collections.ObjectModel;
using System.Reflection;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.DW
{
    abstract class LocalMethodDefiner : MethodWeaveDefiner
    {
        public LocalMethodDefiner(MethodWeaver parent, WeaveMethodInfo injectionMethod)
            : base(parent, injectionMethod)
        {
            anonymousStaticMethodCache = TypeAnalyzer.GetCacheFieldIfAnonymousByRunningState(injectionMethod.Destination);
        }

        public override void Create()
        {
            cachedMethod = Parent.ConstructorWeaver.DeclaringTypeGenerator.AddField(
                LocalClass.CacheFieldPrefix + "Method" + Parent.IncreaseMethodCacheSequence(), WeaveMethod.DelegateType, FieldAttributes.Private);

            cachedSetting = Parent.ConstructorWeaver.FieldsForDeclaringType.ContainsKey(WeaveMethod.Destination.DeclaringType) ?
                                            Parent.ConstructorWeaver.FieldsForDeclaringType[WeaveMethod.Destination.DeclaringType] :
                                            default(IFieldGenerator);

            methodInterface = GetMethodInterface();

            int parameterPosition = 1;
            var methodParameters = new List<IParameterGenerator>();
            foreach (var parameterName in WeaveMethod.Source.ParameterNames())
            {
                methodParameters.Add(MethodInterface.AddParameter(parameterPosition++, ParameterAttributes.In, parameterName));
            }
            this.methodParameters = new ReadOnlyCollection<IParameterGenerator>(methodParameters);
        }

        readonly FieldInfo anonymousStaticMethodCache;
        public override FieldInfo AnonymousStaticMethodCache
        {
            get { return anonymousStaticMethodCache; }
        }

        IFieldGenerator cachedMethod;
        public override IFieldGenerator CachedMethod
        {
            get { return cachedMethod; }
        }

        IFieldGenerator cachedSetting;
        public override IFieldGenerator CachedSetting
        {
            get { return cachedSetting; }
        }

        IMethodBaseGenerator methodInterface;
        public override IMethodBaseGenerator MethodInterface
        {
            get { return methodInterface; }
        }

        ReadOnlyCollection<IParameterGenerator> methodParameters;
        public override ReadOnlyCollection<IParameterGenerator> MethodParameters
        {
            get { return methodParameters; }
        }
    }
}

