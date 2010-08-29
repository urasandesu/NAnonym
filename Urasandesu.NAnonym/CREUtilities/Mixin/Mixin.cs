using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.Linq;
using Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil;
using Urasandesu.NAnonym.CREUtilities.Impl.System.Reflection;

namespace Urasandesu.NAnonym.CREUtilities
{
    // 拡張メソッドを partial 宣言ってありかも！？
    // TODO: クラスを分割。まずは Duplicate 系を Clonable へ。Equivalent 系を Comparable へ。Get* 系を Compatible へ。
    // MEMO: オーバーロードは必ず同じクラスで宣言するべし。縦断してしまうとすぐコンパイルエラーにならずわかりにくいバグになる。
    // MEMO: ファイルだけ Mixin させる型で分割して、実体は全て Mixin の partial クラスにすれば上記のバグは出にくい。無理やりクラス分割する必要ないかも。
    // MEMO: ファイル分割したいのはバージョン管理システムの制限のため。
    public static partial class Mixin
    {

        //public static bool Equivalent(this TypeReference x, Type y)
        //{
        //    // HACK: 暫定。x が配列の場合（例えば Type[]）、配列ではない型に解決されてしまう。本来であれば、ArrayType で Resolve が override されているべきにも思うが・・・？
        //    switch (x.Scope.MetadataScopeType)
        //    {
        //        case MetadataScopeType.AssemblyNameReference:
        //            var assemblyNameReference = x.Scope as AssemblyNameReference;
        //            return assemblyNameReference.FullName == y.Assembly.FullName && x.FullName == y.FullName;
        //        case MetadataScopeType.ModuleDefinition:
        //            var moduleDefinition = x.Scope as ModuleDefinition;
        //            return moduleDefinition.Assembly.Name.FullName == y.Assembly.FullName && x.FullName == y.FullName;
        //        case MetadataScopeType.ModuleReference:
        //        default:
        //            var resolvedX = x.Resolve();
        //            return resolvedX.Module.Assembly.Name.FullName == y.Assembly.FullName && resolvedX.FullName == y.FullName;
        //    }
        //}

        //public static bool Equivalent(this TypeReference x, TypeReference y)
        //{
        //    return x.Module.Assembly.Name.FullName == y.Module.Assembly.Name.FullName && x.FullName == y.FullName;
        //}

        //public static IEnumerable<MethodDefinition> GetMethodDefs(this TypeReference typeRef)
        //{
        //    var typeDef = default(TypeDefinition);
        //    var typeSpec = default(TypeSpecification);
        //    if ((typeDef = typeRef as TypeDefinition) != null)
        //    {
        //        return typeDef.Methods;
        //    }
        //    else if ((typeSpec = typeRef as TypeSpecification) != null)
        //    {
        //        return typeSpec.GetMethodDefs();
        //    }
        //    else
        //    {
        //        throw new NotSupportedException();
        //    }
        //}

        // だめだ。このまま Write すると更新されてしまう。やっぱり Copy メソッドいるし。
        private static IEnumerable<MethodDefinition> GetMethodDefs(this TypeSpecification typeSpec)
        {
            //foreach (var methodDef in typeSpec.ElementType.GetMethodDefs())
            //{
            //    foreach (var parameterDef in methodDef.Parameters)
            //    {
            //        if (parameterDef.ParameterType.IsGenericParameter)
            //        {
            //            int index = typeSpec.ElementType.GenericParameters
            //                            .Cast<TypeReference>()
            //                            .IndexOf(
            //                                parameterDef.ParameterType, 
            //                                Iterable.CreateEqualityComparerNullable(default(TypeReference), EqualsEx)
            //                            );

            //            typeSpec.GenericParameters[index];
            //        }
            //    }
            //}
            throw new NotImplementedException();
        }

        //public static IEnumerable<MethodSpecification> GetMethodSpecs(this TypeReference typeRef)
        //{
        //    var typeDef = default(TypeDefinition);
        //    var typeSpec = default(TypeSpecification);
        //    if ((typeDef = typeRef as TypeDefinition) != null)
        //    {
        //        return typeDef.Methods;
        //    }
        //    else if ((typeSpec = typeRef as TypeSpecification) != null)
        //    {
        //        return typeSpec.ElementType.GetMethodDefs();
        //    }
        //    else
        //    {
        //        throw new NotSupportedException();
        //    }
        //}

        //private static IEnumerable<MethodSpecification> GetMethodSpec(this TypeSpecification typeSpec)
        //{

        //}

        //public static MethodDefinition ToMethodDef(this MethodInfo method)
        //{
        //    method.DeclaringType.ToTypeRef
        //    return (MethodDefinition)typeRef.Module.LookupToken(method.MetadataToken);
        //}

        //public static MethodSpecification ToMethodSpec(this MethodInfo method)
        //{
        //    var genericInstanceMethod = new GenericInstanceMethod(typeRef.GetMethodDef(method));
        //    method.GetGenericArguments().ForEach(
        //        getGenericArgument => genericInstanceMethod.GenericArguments.Add(getGenericArgument.ToTypeRef()));
        //    return genericInstanceMethod;
        //}

        //public static MethodReference ToMethodRef(this MethodInfo method)
        //{
        //    return method.IsGenericMethod && !method.IsGenericMethodDefinition ?
        //        (MethodReference)typeRef.GetMethodSpec(method) :
        //        (MethodReference)typeRef.GetMethodDef(method);
        //}

        public static bool Equivalent(this MethodReference x, MethodBase y)
        {
            bool equals = x.DeclaringType.Equivalent(y.DeclaringType);
            equals = equals && x.Name == y.Name;
            equals = equals && x.Parameters.Equivalent(y.GetParameters());
            return equals;
        }

        public static bool Equivalent(this MethodDefinition x, MethodInfo y)
        {
            bool equals = Equivalent((MethodReference)x, (MethodBase)y);
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

        public static bool Equivalent(this Mono.Cecil.MethodAttributes x, System.Reflection.MethodAttributes y)
        {
            return (int)x == (int)y;
        }

        public static Mono.Cecil.MethodAttributes ToMethodAttributes(this System.Reflection.MethodAttributes attribute)
        {
            return (Mono.Cecil.MethodAttributes)(int)attribute;
        }

        #region ここ一筋縄じゃ行かなかった。例えば、BindingFlags.IgnoreCase だったら、あらかじめ name は全て小文字化しておきたい、とか。
        //// ここ一筋縄じゃ行かなかった。例えば、BindingFlags.IgnoreCase だったら、あらかじめ name は全て小文字化しておきたい、とか。
        //// BindingFlags でフィルターかませる感じで。
        //public static bool IsInside(this MethodDefinition x, string name, BindingFlags flag)
        //{
        //    flag = 
        //        flag == BindingFlags.Default ?
        //            BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod : 
        //            flag;

        //    bool isInside = true;
        //    if ((flag & BindingFlags.DeclaredOnly) == BindingFlags.DeclaredOnly) { }
        //    if ((flag & BindingFlags.Instance) == BindingFlags.Instance) { }
        //    if ((flag & BindingFlags.Static) == BindingFlags.Static) { }
        //    if ((flag & BindingFlags.Public) == BindingFlags.Public) { }
        //    if ((flag & BindingFlags.NonPublic) == BindingFlags.NonPublic) { }
        //    if ((flag & BindingFlags.FlattenHierarchy) == BindingFlags.FlattenHierarchy) { }
        //    if ((flag & BindingFlags.InvokeMethod) == BindingFlags.InvokeMethod) { }
        //    if ((flag & BindingFlags.CreateInstance) == BindingFlags.CreateInstance) { }
        //    if ((flag & BindingFlags.GetField) == BindingFlags.GetField) { }
        //    if ((flag & BindingFlags.SetField) == BindingFlags.SetField) { }
        //    if ((flag & BindingFlags.GetProperty) == BindingFlags.GetProperty) { }
        //    if ((flag & BindingFlags.SetProperty) == BindingFlags.SetProperty) { }
        //    if ((flag & BindingFlags.PutDispProperty) == BindingFlags.PutDispProperty) { }
        //    if ((flag & BindingFlags.PutRefDispProperty) == BindingFlags.PutRefDispProperty) { }
        //    if ((flag & BindingFlags.ExactBinding) == BindingFlags.ExactBinding) { }
        //    if ((flag & BindingFlags.SuppressChangeType) == BindingFlags.SuppressChangeType) { }
        //    if ((flag & BindingFlags.OptionalParamBinding) == BindingFlags.OptionalParamBinding) { }
        //    if ((flag & BindingFlags.IgnoreReturn) == BindingFlags.IgnoreReturn) { }


        //    throw new NotImplementedException();
        //}
        #endregion

        public static bool Equivalent(this IEnumerable<ParameterDefinition> first, IEnumerable<Type> second)
        {
            var comparer =
                Iterable.CreateEqualityComparerNullable(default(ParameterDefinition), default(Type),
                (firstItem, secondItem) =>
                {
                    return firstItem.ParameterType.Equivalent(secondItem);
                });
            return first.Equivalent(second, comparer);
        }

        public static bool Equivalent(this IEnumerable<ParameterDefinition> first, IEnumerable<ParameterInfo> second)
        {
            var comparer =
                Iterable.CreateEqualityComparerNullable(default(ParameterDefinition), default(ParameterInfo),
                (firstItem, secondItem) =>
                {
                    return firstItem.Equivalent(secondItem);
                });
            return first.Equivalent(second, comparer);
        }

        public static bool Equivalent(this IEnumerable<ParameterDefinition> first, IEnumerable<ParameterDefinition> second)
        {
            var comparer =
                Iterable.CreateEqualityComparerNullable(default(ParameterDefinition), default(ParameterDefinition),
                (firstItem, secondItem) =>
                {
                    return firstItem.Equivalent(secondItem);
                });
            return first.Equivalent(second, comparer);
        }

        public static bool Equivalent(this ParameterDefinition x, ParameterInfo y)
        {
            return x.Name == y.Name && x.ParameterType.Equivalent(y.ParameterType);
        }

        public static bool Equivalent(this ParameterDefinition x, ParameterDefinition y)
        {
            return x.Name == y.Name && x.ParameterType.Equivalent(y.ParameterType);
        }

        public static bool Equivalent(this FieldDefinition x, FieldInfo y)
        {
            return x.Name == y.Name && x.FieldType.Equivalent(y.FieldType);
        }

        public static AssemblyDefinition Duplicate(this AssemblyDefinition source)
        {
            throw new NotImplementedException();
        }

        public static ModuleDefinition Duplicate(this ModuleDefinition source)
        {
            throw new NotImplementedException();
        }

        public static TypeDefinition Duplicate(this TypeDefinition source)
        {
            var destination = new TypeDefinition(source.Namespace, source.Name, source.Attributes, source.BaseType);
            return destination;
        }

        public static GenericInstanceType Duplicate(this GenericInstanceType source)
        {
            throw new NotImplementedException();
        }

        public static MethodReference DuplicateWithoutBody(this MethodReference source)
        {
            var sourceDef = default(MethodDefinition);
            var sourceGen = default(GenericInstanceMethod);
            if ((sourceDef = source as MethodDefinition) != null)
            {
                return sourceDef.DuplicateWithoutBody();
            }
            else if ((sourceGen = source as GenericInstanceMethod) != null)
            {
                return sourceGen.DuplicateWithoutBody();
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public static MethodReference Duplicate(this MethodReference source)
        {
            var sourceDef = default(MethodDefinition);
            var sourceGen = default(GenericInstanceMethod);
            if ((sourceDef = source as MethodDefinition) != null)
            {
                return sourceDef.Duplicate();
            }
            else if ((sourceGen = source as GenericInstanceMethod) != null)
            {
                return sourceGen.Duplicate();
            }
            else
            {
                throw new NotSupportedException();
            }
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

        public static GenericInstanceMethod DuplicateWithoutBody(this GenericInstanceMethod source)
        {
            throw new NotImplementedException();
        }

        public static GenericInstanceMethod Duplicate(this GenericInstanceMethod source)
        {
            var destination = new GenericInstanceMethod(source.ElementMethod.Duplicate());
            //source.genericpa
            throw new NotImplementedException();
        }

        public static ParameterDefinition Duplicate(this ParameterDefinition source)
        {
            var destination = new ParameterDefinition(source.Name, source.Attributes, source.ParameterType);
            return destination;
        }

        public static GenericParameter Duplicate(this GenericParameter source, IGenericParameterProvider owner)
        {
            var destination = new GenericParameter(source.Name, owner);
            return destination;
        }

        public static Instruction Duplicate(this Instruction source)
        {
            byte? byteOperand = null;
            CallSite callSiteOperand = null;
            double? doubleOperand = null;
            FieldReference fieldReferenceOperand = null;
            float? floatOperand = null;
            Instruction instructionOperand = null;
            Instruction[] instructionArrayOperand = null;
            int? intOperand = null;
            long? longOperand = null;
            MethodReference methodReferenceOperand = null;
            ParameterDefinition parameterDefinitionOperand = null;
            sbyte? sbyteOperand = null;
            string stringOperand = null;
            TypeReference typeReferenceOperand = null;
            VariableDefinition variableDefinitionOperand = null;

            if ((byteOperand = source.Operand as byte?) != null) return Instruction.Create(source.OpCode, byteOperand.Value);
            else if ((callSiteOperand = source.Operand as CallSite) != null) return Instruction.Create(source.OpCode, callSiteOperand);
            else if ((doubleOperand = source.Operand as double?) != null) return Instruction.Create(source.OpCode, doubleOperand.Value);
            else if ((fieldReferenceOperand = source.Operand as FieldReference) != null) return Instruction.Create(source.OpCode, fieldReferenceOperand);
            else if ((floatOperand = source.Operand as float?) != null) return Instruction.Create(source.OpCode, floatOperand.Value);
            else if ((instructionOperand = source.Operand as Instruction) != null) return Instruction.Create(source.OpCode, instructionOperand);
            else if ((instructionArrayOperand = source.Operand as Instruction[]) != null) return Instruction.Create(source.OpCode, instructionArrayOperand);
            else if ((intOperand = source.Operand as int?) != null) return Instruction.Create(source.OpCode, intOperand.Value);
            else if ((longOperand = source.Operand as long?) != null) return Instruction.Create(source.OpCode, longOperand.Value);
            else if ((methodReferenceOperand = source.Operand as MethodReference) != null) return Instruction.Create(source.OpCode, methodReferenceOperand);
            else if ((parameterDefinitionOperand = source.Operand as ParameterDefinition) != null) return Instruction.Create(source.OpCode, parameterDefinitionOperand);
            else if ((sbyteOperand = source.Operand as sbyte?) != null) return Instruction.Create(source.OpCode, sbyteOperand.Value);
            else if ((stringOperand = source.Operand as string) != null) return Instruction.Create(source.OpCode, stringOperand);
            else if ((typeReferenceOperand = source.Operand as TypeReference) != null) return Instruction.Create(source.OpCode, typeReferenceOperand);
            else if ((variableDefinitionOperand = source.Operand as VariableDefinition) != null) return Instruction.Create(source.OpCode, variableDefinitionOperand);
            else return Instruction.Create(source.OpCode);
        }

        public static VariableDefinition Duplicate(this VariableDefinition source)
        {
            var destination = new VariableDefinition(source.Name, source.VariableType);
            return destination;
        }

        public static ExceptionHandler Duplicate(this ExceptionHandler source)
        {
            var destination = new ExceptionHandler(source.HandlerType);
            destination.CatchType = source.CatchType;
            destination.FilterEnd = source.FilterEnd;
            destination.FilterStart = source.FilterStart;
            destination.HandlerEnd = source.HandlerEnd;
            destination.HandlerStart = source.HandlerStart;
            destination.HandlerType = source.HandlerType;
            destination.TryEnd = source.TryEnd;
            destination.TryStart = source.TryStart;
            return destination;
        }

        public static FieldDefinition Duplicate(this FieldDefinition source)
        {
            var destination = new FieldDefinition(source.Name, source.Attributes, source.FieldType);
            return destination;
        }

        public static PropertyDefinition Duplicate(this PropertyDefinition source)
        {
            // Property のコピーは外側だけのコピーだけではなく、中身の get_ メソッド、 set_ メソッドのコピーが必要！
            var destination = new PropertyDefinition(source.Name, source.Attributes, source.PropertyType);
            return destination;
        }

        public static bool IsStatic(this PropertyInfo source)
        {
            return (source.CanRead && source.GetGetMethod().IsStatic) || (source.CanWrite && source.GetSetMethod().IsStatic);
        }

        public static PortableScope CreateScope(this MethodDefinition methodDef)
        {
            return new PortableScope((MCMethodGeneratorImpl)methodDef);
        }

        //public static PortableScope CreatePortableScope(this MethodDefinition methodDef, object instance)
        //{
        //    throw new NotImplementedException();
        //}

        public static PortableScope2 CarryPortableScope2(this MethodDefinition methodDef)
        {
            var scope = new PortableScope2((MCMethodGeneratorImpl)methodDef);
            return scope;
        }

        public static void ExpressBody(this MethodDefinition methodDef, Action<ExpressiveMethodBodyGenerator> expression)    // TODO: ハンドラ化したほうが良いかも？
        {
            var gen = new ExpressiveMethodBodyGenerator((MCMethodGeneratorImpl)methodDef);
            expression(gen);
            gen.Eval(_ => _.End());
        }

        public static void ExpressBody(this ConstructorBuilder constructorBuilder, Action<ExpressiveMethodBodyGenerator> expression)
        {
            var gen = new ExpressiveMethodBodyGenerator((SRConstructorGeneratorImpl)constructorBuilder);
            expression(gen);
            gen.Eval(_ => _.End());
        }

        public static void ExpressBody(this DynamicMethod dynamicMethod, Action<ExpressiveMethodBodyGenerator> expression)
        {
            var gen = new ExpressiveMethodBodyGenerator((SRMethodGeneratorImpl)dynamicMethod);
            expression(gen);
            gen.Eval(_ => _.End());
        }
    }
}
