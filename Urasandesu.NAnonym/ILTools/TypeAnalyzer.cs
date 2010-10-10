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
