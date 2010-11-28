using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using Mono.Cecil;
using MC = Mono.Cecil;
using Urasandesu.NAnonym.ILTools;
using UNI = Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Cecil.Mixins.System;
using Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.Cecil.Mixins.System.Collections.Generic;

namespace Urasandesu.NAnonym.Cecil.ILTools
{
    // MEMO: これは Cecil 無いとどうにも解析できない？
    // NOTE: Global クラス側だけにあれば良い機能（SN 付いてない Assembly 書き換えるって機能がそもそも Cecil にしかない）。
    // NOTE: つまり Global クラスは Cecil 側になる。
    // TODO: 名前空間をもっと上の段階で区切ったほうがいい。
    // TODO: Urasandesu.NAnonym.CREUtilities.MC, Urasandesu.NAnonym.CREUtilities.SR みたいな。
    public class TypeAnalyzer : UNI::TypeAnalyzer
    {
        protected TypeAnalyzer()
            : base()
        {
        }

        //public static bool IsAnonymous(MethodBase methodBase)
        //{
        //    return methodBase.GetCustomAttributes(true).OfType<CompilerGeneratedAttribute>().FirstOrDefault() != null &&
        //        methodBase.Name.IndexOf("<") == 0;
        //}

        //public static bool IsCandidateAnonymousMethodCache(FieldInfo field)
        //{
        //    return -1 < field.Name.IndexOf("__CachedAnonymousMethodDelegate") &&
        //        field.GetCustomAttributes(true).OfType<CompilerGeneratedAttribute>().FirstOrDefault() != null;
        //}

        // IL 命令の並びから匿名メソッドのキャッシュフィールドを探し出す。
        public static FieldInfo GetCacheFieldIfAnonymousByDirective(MethodBase methodBase)
        {
            if (!IsAnonymous(methodBase)) return null;


            var cacheField = default(FieldInfo);
            var declaringType = methodBase.DeclaringType;
            var declaringTypeDef = declaringType.ToTypeDef();

            var candidateNameCacheFieldDictionary = new Dictionary<string, FieldInfo>();
            foreach (var candidateCacheField in declaringType.GetFields(BindingFlags.NonPublic | BindingFlags.Static)
                                                             .Where(field => IsCandidateAnonymousMethodCache(field)))
            {
                candidateNameCacheFieldDictionary.Add(candidateCacheField.Name, candidateCacheField);
            }


            string declaringMethodName = methodBase.Name.Substring(methodBase.Name.IndexOf("<") + 1, methodBase.Name.IndexOf(">") - 1);
            foreach (var candidateMethod in declaringTypeDef.Methods.Where(method => -1 < method.Name.IndexOf(declaringMethodName)))
            {
                int candidatePoint = 0; // HACK: enum 化
                var candidateCacheField = default(FieldDefinition);
                foreach (var instruction in candidateMethod.Body.Instructions)
                {
                    if (candidatePoint == 0 &&
                        instruction.OpCode == MC::Cil.OpCodes.Ldsfld &&
                        candidateNameCacheFieldDictionary.ContainsKey((candidateCacheField = (FieldDefinition)instruction.Operand).Name))
                    {
                        candidatePoint = 1;
                    }
                    else if (candidatePoint == 1 &&
                        instruction.OpCode == MC::Cil.OpCodes.Brtrue_S)
                    {
                        candidatePoint = 2;
                    }
                    else if (candidatePoint == 2 &&
                        instruction.OpCode == MC::Cil.OpCodes.Ldnull)
                    {
                        candidatePoint = 3;
                    }
                    else if (candidatePoint == 3 &&
                        instruction.OpCode == MC::Cil.OpCodes.Ldftn &&
                        ((MethodReference)instruction.Operand).Equivalent(methodBase))
                    {
                        candidatePoint = 4;
                        break;
                    }
                    else if (candidatePoint == 3 || candidatePoint == 2 || candidatePoint == 1)
                    {
                        candidatePoint = 0;
                    }
                }
                if (candidatePoint == 4)
                {
                    cacheField = candidateNameCacheFieldDictionary[candidateCacheField.Name];
                    break;
                }
            }
            return cacheField;
        }
    }
}
