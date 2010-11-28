/* 
 * File: TypeAnalyzer.cs
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
using System.Runtime.CompilerServices;

namespace Urasandesu.NAnonym.ILTools
{
    public class TypeAnalyzer
    {
        protected TypeAnalyzer()
        {
        }

        public static Type SearchManuallyGenerated(Type type)
        {
            if (type.GetCustomAttributes(true).OfType<CompilerGeneratedAttribute>().FirstOrDefault() == null)
            {
                return type;
            }
            else
            {
                return SearchManuallyGenerated(type.DeclaringType);
            }
        }

        public static bool IsAnonymous(MethodBase methodBase)
        {
            return methodBase.DeclaringType.GetCustomAttributes(true).OfType<CompilerGeneratedAttribute>().FirstOrDefault() != null ||
                methodBase.GetCustomAttributes(true).OfType<CompilerGeneratedAttribute>().FirstOrDefault() != null && methodBase.Name.IndexOf("<") == 0;
        }

        public static bool IsCandidateAnonymousMethodCache(FieldInfo field)
        {
            return -1 < field.Name.IndexOf("__CachedAnonymousMethodDelegate") &&
                field.GetCustomAttributes(true).OfType<CompilerGeneratedAttribute>().FirstOrDefault() != null &&
                typeof(Delegate).IsAssignableFrom(field.FieldType);
        }

        // 実行状態から匿名メソッドのキャッシュフィールドを探し出す。
        public static FieldInfo GetCacheFieldIfAnonymousByRunningState(MethodBase methodBase)
        {
            if (!IsAnonymous(methodBase)) return null;

            var cacheField = default(FieldInfo);
            var declaringType = methodBase.DeclaringType;

            foreach (var candidateCacheField in declaringType.GetFields(BindingFlags.NonPublic | BindingFlags.Static)
                                                             .Where(field => IsCandidateAnonymousMethodCache(field)))
            {
                var candidateCachedDelegate = (Delegate)candidateCacheField.GetValue(null);
                if (candidateCachedDelegate == null) continue;

                if (candidateCachedDelegate.Method == methodBase)
                {
                    cacheField = candidateCacheField;
                    break;
                }
            }

            return cacheField;
        }
    }
}

