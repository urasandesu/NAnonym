/* 
 * File: GlobalMethodDefiner.cs
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
using Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil;
using Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.DW;
using SR = System.Reflection;
using TypeAnalyzer = Urasandesu.NAnonym.Cecil.ILTools.TypeAnalyzer;
using UNI = Urasandesu.NAnonym.ILTools;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Urasandesu.NAnonym.Linq;


namespace Urasandesu.NAnonym.Cecil.DW
{
    abstract class GlobalMethodDefiner : MethodWeaveDefiner
    {
        public static readonly string VersionName = "version";
        public static readonly string AutoNamedAliasPattern = string.Format(@"^{0}(?<{1}>\d+)", Regex.Escape(GlobalClass.MethodPrefix), VersionName);
        public static readonly Regex AutoNamedAliasRegex = new Regex(AutoNamedAliasPattern, RegexOptions.Compiled);


        public GlobalMethodDefiner(MethodWeaver parent, WeaveMethodInfo injectionMethod)
            : base(parent, injectionMethod)
        {
            anonymousStaticMethodCache = TypeAnalyzer.GetCacheFieldIfAnonymousByDirective(injectionMethod.Destination);
        }

        public override void Create()
        {
            cachedMethod = Parent.ConstructorWeaver.DeclaringTypeGenerator.AddField(
                                        GlobalClass.CacheFieldPrefix + "Method" + Parent.IncreaseMethodCacheSequence(),
                                        WeaveMethod.DelegateType, 
                                        SR::FieldAttributes.Private);

            var declaringTypeDef = ((MCTypeGeneratorImpl)Parent.ConstructorWeaver.DeclaringTypeGenerator).TypeDef;
            var cachedSettingGen = declaringTypeDef.Fields.FirstOrDefault(fieldDef => fieldDef.FieldType.Resolve().GetFullName() == WeaveMethod.Destination.DeclaringType.FullName);
            if (cachedSettingGen != null)
            {
                cachedSetting = new MCFieldGeneratorImpl(cachedSettingGen);
            }

            methodInterface = GetMethodInterface();
        }

        protected override UNI::IMethodGenerator GetMethodInterface()
        {
            // TODO: エイリアス名が明示的に指定されている場合の処理
            var declaringTypeDef = ((MCTypeGeneratorImpl)Parent.ConstructorWeaver.DeclaringTypeGenerator).TypeDef;
            var source = declaringTypeDef.Methods.FirstOrDefault(methodDef => methodDef.Equivalent(WeaveMethod.Source));
            string sourceName = source.Name;
            var latestVersion = declaringTypeDef.Methods.
                                                  Select(methodDef => methodDef.Name).
                                                  Select(name => AutoNamedAliasRegex.Match(name)).
                                                  Where(autoNamedAliasMatch => autoNamedAliasMatch.Success).
                                                  MaxOrDefault(autoNamedAliasMatch => (int?)int.Parse(autoNamedAliasMatch.Groups[VersionName].Value));
            source.Name = GlobalClass.MethodPrefix + (latestVersion == null ? 0 : latestVersion + 1) + source.Name;
            baseMethod = new MCMethodGeneratorImpl(source);

            var destination = source.DuplicateWithoutBody();
            destination.Name = sourceName;
            declaringTypeDef.Methods.Add(destination);
            var destinationGen = new MCMethodGeneratorImpl(destination);

            return destinationGen;
        }

        readonly FieldInfo anonymousStaticMethodCache;
        public override FieldInfo AnonymousStaticMethodCache
        {
            get { return anonymousStaticMethodCache; }
        }

        public override ReadOnlyCollection<UNI::IParameterGenerator> MethodParameters
        {
            get { throw new NotImplementedException(); }
        }

        UNI::IMethodDeclaration baseMethod;
        public override UNI::IMethodDeclaration BaseMethod
        {
            get { return baseMethod; }
        }

        UNI::IFieldGenerator cachedMethod;
        public override UNI::IFieldGenerator CachedMethod
        {
            get { return cachedMethod; }
        }

        UNI::IFieldGenerator cachedSetting;
        public override UNI::IFieldGenerator CachedSetting
        {
            get { return cachedSetting; }
        }

        UNI::IMethodBaseGenerator methodInterface;
        public override UNI::IMethodBaseGenerator MethodInterface
        {
            get { return methodInterface; }
        }
    }
}

