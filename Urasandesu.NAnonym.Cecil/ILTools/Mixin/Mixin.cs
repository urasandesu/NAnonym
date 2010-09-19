using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.Linq;
//using Urasandesu.NAnonym.ILTools.Impl.Mono.Cecil;
using Urasandesu.NAnonym.ILTools.Impl.System.Reflection;
using Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil;
using MC = Mono.Cecil;
using Urasandesu.NAnonym.ILTools;
using UNI = Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.ILTools
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

        public static PortableScope CarryPortableScope(this MethodDefinition methodDef)
        {
            var scope = new PortableScope((MCMethodGeneratorImpl)methodDef);
            return scope;
        }

        public static void ExpressBody(this MethodDefinition methodDef, Action<ExpressiveMethodBodyGenerator> expression)    // TODO: ハンドラ化したほうが良いかも？
        {
            var gen = new ExpressiveMethodBodyGenerator((MCMethodGeneratorImpl)methodDef);
            expression(gen);
            if (gen.Directives.Last().OpCode != UNI::OpCodes.Ret)
            {
                gen.Eval(_ => _.End());
            }
        }

        public static UNI::OpCode Cast(this MC::Cil.OpCode opcode)
        {
            if (opcode == MC::Cil.OpCodes.Add) return UNI::OpCodes.Add;
            else if (opcode == MC::Cil.OpCodes.Add_Ovf) return UNI::OpCodes.Add_Ovf;
            else if (opcode == MC::Cil.OpCodes.Add_Ovf_Un) return UNI::OpCodes.Add_Ovf_Un;
            else if (opcode == MC::Cil.OpCodes.And) return UNI::OpCodes.And;
            else if (opcode == MC::Cil.OpCodes.Arglist) return UNI::OpCodes.Arglist;
            else if (opcode == MC::Cil.OpCodes.Beq) return UNI::OpCodes.Beq;
            else if (opcode == MC::Cil.OpCodes.Beq_S) return UNI::OpCodes.Beq_S;
            else if (opcode == MC::Cil.OpCodes.Bge) return UNI::OpCodes.Bge;
            else if (opcode == MC::Cil.OpCodes.Bge_S) return UNI::OpCodes.Bge_S;
            else if (opcode == MC::Cil.OpCodes.Bge_Un) return UNI::OpCodes.Bge_Un;
            else if (opcode == MC::Cil.OpCodes.Bge_Un_S) return UNI::OpCodes.Bge_Un_S;
            else if (opcode == MC::Cil.OpCodes.Bgt) return UNI::OpCodes.Bgt;
            else if (opcode == MC::Cil.OpCodes.Bgt_S) return UNI::OpCodes.Bgt_S;
            else if (opcode == MC::Cil.OpCodes.Bgt_Un) return UNI::OpCodes.Bgt_Un;
            else if (opcode == MC::Cil.OpCodes.Bgt_Un_S) return UNI::OpCodes.Bgt_Un_S;
            else if (opcode == MC::Cil.OpCodes.Ble) return UNI::OpCodes.Ble;
            else if (opcode == MC::Cil.OpCodes.Ble_S) return UNI::OpCodes.Ble_S;
            else if (opcode == MC::Cil.OpCodes.Ble_Un) return UNI::OpCodes.Ble_Un;
            else if (opcode == MC::Cil.OpCodes.Ble_Un_S) return UNI::OpCodes.Ble_Un_S;
            else if (opcode == MC::Cil.OpCodes.Blt) return UNI::OpCodes.Blt;
            else if (opcode == MC::Cil.OpCodes.Blt_S) return UNI::OpCodes.Blt_S;
            else if (opcode == MC::Cil.OpCodes.Blt_Un) return UNI::OpCodes.Blt_Un;
            else if (opcode == MC::Cil.OpCodes.Blt_Un_S) return UNI::OpCodes.Blt_Un_S;
            else if (opcode == MC::Cil.OpCodes.Bne_Un) return UNI::OpCodes.Bne_Un;
            else if (opcode == MC::Cil.OpCodes.Bne_Un_S) return UNI::OpCodes.Bne_Un_S;
            else if (opcode == MC::Cil.OpCodes.Box) return UNI::OpCodes.Box;
            else if (opcode == MC::Cil.OpCodes.Br) return UNI::OpCodes.Br;
            else if (opcode == MC::Cil.OpCodes.Br_S) return UNI::OpCodes.Br_S;
            else if (opcode == MC::Cil.OpCodes.Break) return UNI::OpCodes.Break;
            else if (opcode == MC::Cil.OpCodes.Brfalse) return UNI::OpCodes.Brfalse;
            else if (opcode == MC::Cil.OpCodes.Brfalse_S) return UNI::OpCodes.Brfalse_S;
            else if (opcode == MC::Cil.OpCodes.Brtrue) return UNI::OpCodes.Brtrue;
            else if (opcode == MC::Cil.OpCodes.Brtrue_S) return UNI::OpCodes.Brtrue_S;
            else if (opcode == MC::Cil.OpCodes.Call) return UNI::OpCodes.Call;
            else if (opcode == MC::Cil.OpCodes.Calli) return UNI::OpCodes.Calli;
            else if (opcode == MC::Cil.OpCodes.Callvirt) return UNI::OpCodes.Callvirt;
            else if (opcode == MC::Cil.OpCodes.Castclass) return UNI::OpCodes.Castclass;
            else if (opcode == MC::Cil.OpCodes.Ceq) return UNI::OpCodes.Ceq;
            else if (opcode == MC::Cil.OpCodes.Cgt) return UNI::OpCodes.Cgt;
            else if (opcode == MC::Cil.OpCodes.Cgt_Un) return UNI::OpCodes.Cgt_Un;
            else if (opcode == MC::Cil.OpCodes.Ckfinite) return UNI::OpCodes.Ckfinite;
            else if (opcode == MC::Cil.OpCodes.Clt) return UNI::OpCodes.Clt;
            else if (opcode == MC::Cil.OpCodes.Clt_Un) return UNI::OpCodes.Clt_Un;
            else if (opcode == MC::Cil.OpCodes.Constrained) return UNI::OpCodes.Constrained;
            else if (opcode == MC::Cil.OpCodes.Conv_I) return UNI::OpCodes.Conv_I;
            else if (opcode == MC::Cil.OpCodes.Conv_I1) return UNI::OpCodes.Conv_I1;
            else if (opcode == MC::Cil.OpCodes.Conv_I2) return UNI::OpCodes.Conv_I2;
            else if (opcode == MC::Cil.OpCodes.Conv_I4) return UNI::OpCodes.Conv_I4;
            else if (opcode == MC::Cil.OpCodes.Conv_I8) return UNI::OpCodes.Conv_I8;
            else if (opcode == MC::Cil.OpCodes.Conv_Ovf_I) return UNI::OpCodes.Conv_Ovf_I;
            else if (opcode == MC::Cil.OpCodes.Conv_Ovf_I_Un) return UNI::OpCodes.Conv_Ovf_I_Un;
            else if (opcode == MC::Cil.OpCodes.Conv_Ovf_I1) return UNI::OpCodes.Conv_Ovf_I1;
            else if (opcode == MC::Cil.OpCodes.Conv_Ovf_I1_Un) return UNI::OpCodes.Conv_Ovf_I1_Un;
            else if (opcode == MC::Cil.OpCodes.Conv_Ovf_I2) return UNI::OpCodes.Conv_Ovf_I2;
            else if (opcode == MC::Cil.OpCodes.Conv_Ovf_I2_Un) return UNI::OpCodes.Conv_Ovf_I2_Un;
            else if (opcode == MC::Cil.OpCodes.Conv_Ovf_I4) return UNI::OpCodes.Conv_Ovf_I4;
            else if (opcode == MC::Cil.OpCodes.Conv_Ovf_I4_Un) return UNI::OpCodes.Conv_Ovf_I4_Un;
            else if (opcode == MC::Cil.OpCodes.Conv_Ovf_I8) return UNI::OpCodes.Conv_Ovf_I8;
            else if (opcode == MC::Cil.OpCodes.Conv_Ovf_I8_Un) return UNI::OpCodes.Conv_Ovf_I8_Un;
            else if (opcode == MC::Cil.OpCodes.Conv_Ovf_U) return UNI::OpCodes.Conv_Ovf_U;
            else if (opcode == MC::Cil.OpCodes.Conv_Ovf_U_Un) return UNI::OpCodes.Conv_Ovf_U_Un;
            else if (opcode == MC::Cil.OpCodes.Conv_Ovf_U1) return UNI::OpCodes.Conv_Ovf_U1;
            else if (opcode == MC::Cil.OpCodes.Conv_Ovf_U1_Un) return UNI::OpCodes.Conv_Ovf_U1_Un;
            else if (opcode == MC::Cil.OpCodes.Conv_Ovf_U2) return UNI::OpCodes.Conv_Ovf_U2;
            else if (opcode == MC::Cil.OpCodes.Conv_Ovf_U2_Un) return UNI::OpCodes.Conv_Ovf_U2_Un;
            else if (opcode == MC::Cil.OpCodes.Conv_Ovf_U4) return UNI::OpCodes.Conv_Ovf_U4;
            else if (opcode == MC::Cil.OpCodes.Conv_Ovf_U4_Un) return UNI::OpCodes.Conv_Ovf_U4_Un;
            else if (opcode == MC::Cil.OpCodes.Conv_Ovf_U8) return UNI::OpCodes.Conv_Ovf_U8;
            else if (opcode == MC::Cil.OpCodes.Conv_Ovf_U8_Un) return UNI::OpCodes.Conv_Ovf_U8_Un;
            else if (opcode == MC::Cil.OpCodes.Conv_R_Un) return UNI::OpCodes.Conv_R_Un;
            else if (opcode == MC::Cil.OpCodes.Conv_R4) return UNI::OpCodes.Conv_R4;
            else if (opcode == MC::Cil.OpCodes.Conv_R8) return UNI::OpCodes.Conv_R8;
            else if (opcode == MC::Cil.OpCodes.Conv_U) return UNI::OpCodes.Conv_U;
            else if (opcode == MC::Cil.OpCodes.Conv_U1) return UNI::OpCodes.Conv_U1;
            else if (opcode == MC::Cil.OpCodes.Conv_U2) return UNI::OpCodes.Conv_U2;
            else if (opcode == MC::Cil.OpCodes.Conv_U4) return UNI::OpCodes.Conv_U4;
            else if (opcode == MC::Cil.OpCodes.Conv_U8) return UNI::OpCodes.Conv_U8;
            else if (opcode == MC::Cil.OpCodes.Cpblk) return UNI::OpCodes.Cpblk;
            else if (opcode == MC::Cil.OpCodes.Cpobj) return UNI::OpCodes.Cpobj;
            else if (opcode == MC::Cil.OpCodes.Div) return UNI::OpCodes.Div;
            else if (opcode == MC::Cil.OpCodes.Div_Un) return UNI::OpCodes.Div_Un;
            else if (opcode == MC::Cil.OpCodes.Dup) return UNI::OpCodes.Dup;
            else if (opcode == MC::Cil.OpCodes.Endfilter) return UNI::OpCodes.Endfilter;
            else if (opcode == MC::Cil.OpCodes.Endfinally) return UNI::OpCodes.Endfinally;
            else if (opcode == MC::Cil.OpCodes.Initblk) return UNI::OpCodes.Initblk;
            else if (opcode == MC::Cil.OpCodes.Initobj) return UNI::OpCodes.Initobj;
            else if (opcode == MC::Cil.OpCodes.Isinst) return UNI::OpCodes.Isinst;
            else if (opcode == MC::Cil.OpCodes.Jmp) return UNI::OpCodes.Jmp;
            else if (opcode == MC::Cil.OpCodes.Ldarg) return UNI::OpCodes.Ldarg;
            else if (opcode == MC::Cil.OpCodes.Ldarg_0) return UNI::OpCodes.Ldarg_0;
            else if (opcode == MC::Cil.OpCodes.Ldarg_1) return UNI::OpCodes.Ldarg_1;
            else if (opcode == MC::Cil.OpCodes.Ldarg_2) return UNI::OpCodes.Ldarg_2;
            else if (opcode == MC::Cil.OpCodes.Ldarg_3) return UNI::OpCodes.Ldarg_3;
            else if (opcode == MC::Cil.OpCodes.Ldarg_S) return UNI::OpCodes.Ldarg_S;
            else if (opcode == MC::Cil.OpCodes.Ldarga) return UNI::OpCodes.Ldarga;
            else if (opcode == MC::Cil.OpCodes.Ldarga_S) return UNI::OpCodes.Ldarga_S;
            else if (opcode == MC::Cil.OpCodes.Ldc_I4) return UNI::OpCodes.Ldc_I4;
            else if (opcode == MC::Cil.OpCodes.Ldc_I4_0) return UNI::OpCodes.Ldc_I4_0;
            else if (opcode == MC::Cil.OpCodes.Ldc_I4_1) return UNI::OpCodes.Ldc_I4_1;
            else if (opcode == MC::Cil.OpCodes.Ldc_I4_2) return UNI::OpCodes.Ldc_I4_2;
            else if (opcode == MC::Cil.OpCodes.Ldc_I4_3) return UNI::OpCodes.Ldc_I4_3;
            else if (opcode == MC::Cil.OpCodes.Ldc_I4_4) return UNI::OpCodes.Ldc_I4_4;
            else if (opcode == MC::Cil.OpCodes.Ldc_I4_5) return UNI::OpCodes.Ldc_I4_5;
            else if (opcode == MC::Cil.OpCodes.Ldc_I4_6) return UNI::OpCodes.Ldc_I4_6;
            else if (opcode == MC::Cil.OpCodes.Ldc_I4_7) return UNI::OpCodes.Ldc_I4_7;
            else if (opcode == MC::Cil.OpCodes.Ldc_I4_8) return UNI::OpCodes.Ldc_I4_8;
            else if (opcode == MC::Cil.OpCodes.Ldc_I4_M1) return UNI::OpCodes.Ldc_I4_M1;
            else if (opcode == MC::Cil.OpCodes.Ldc_I4_S) return UNI::OpCodes.Ldc_I4_S;
            else if (opcode == MC::Cil.OpCodes.Ldc_I8) return UNI::OpCodes.Ldc_I8;
            else if (opcode == MC::Cil.OpCodes.Ldc_R4) return UNI::OpCodes.Ldc_R4;
            else if (opcode == MC::Cil.OpCodes.Ldc_R8) return UNI::OpCodes.Ldc_R8;
            else if (opcode == MC::Cil.OpCodes.Ldelem_Any) return UNI::OpCodes.Ldelem;
            else if (opcode == MC::Cil.OpCodes.Ldelem_I) return UNI::OpCodes.Ldelem_I;
            else if (opcode == MC::Cil.OpCodes.Ldelem_I1) return UNI::OpCodes.Ldelem_I1;
            else if (opcode == MC::Cil.OpCodes.Ldelem_I2) return UNI::OpCodes.Ldelem_I2;
            else if (opcode == MC::Cil.OpCodes.Ldelem_I4) return UNI::OpCodes.Ldelem_I4;
            else if (opcode == MC::Cil.OpCodes.Ldelem_I8) return UNI::OpCodes.Ldelem_I8;
            else if (opcode == MC::Cil.OpCodes.Ldelem_R4) return UNI::OpCodes.Ldelem_R4;
            else if (opcode == MC::Cil.OpCodes.Ldelem_R8) return UNI::OpCodes.Ldelem_R8;
            else if (opcode == MC::Cil.OpCodes.Ldelem_Ref) return UNI::OpCodes.Ldelem_Ref;
            else if (opcode == MC::Cil.OpCodes.Ldelem_U1) return UNI::OpCodes.Ldelem_U1;
            else if (opcode == MC::Cil.OpCodes.Ldelem_U2) return UNI::OpCodes.Ldelem_U2;
            else if (opcode == MC::Cil.OpCodes.Ldelem_U4) return UNI::OpCodes.Ldelem_U4;
            else if (opcode == MC::Cil.OpCodes.Ldelema) return UNI::OpCodes.Ldelema;
            else if (opcode == MC::Cil.OpCodes.Ldfld) return UNI::OpCodes.Ldfld;
            else if (opcode == MC::Cil.OpCodes.Ldflda) return UNI::OpCodes.Ldflda;
            else if (opcode == MC::Cil.OpCodes.Ldftn) return UNI::OpCodes.Ldftn;
            else if (opcode == MC::Cil.OpCodes.Ldind_I) return UNI::OpCodes.Ldind_I;
            else if (opcode == MC::Cil.OpCodes.Ldind_I1) return UNI::OpCodes.Ldind_I1;
            else if (opcode == MC::Cil.OpCodes.Ldind_I2) return UNI::OpCodes.Ldind_I2;
            else if (opcode == MC::Cil.OpCodes.Ldind_I4) return UNI::OpCodes.Ldind_I4;
            else if (opcode == MC::Cil.OpCodes.Ldind_I8) return UNI::OpCodes.Ldind_I8;
            else if (opcode == MC::Cil.OpCodes.Ldind_R4) return UNI::OpCodes.Ldind_R4;
            else if (opcode == MC::Cil.OpCodes.Ldind_R8) return UNI::OpCodes.Ldind_R8;
            else if (opcode == MC::Cil.OpCodes.Ldind_Ref) return UNI::OpCodes.Ldind_Ref;
            else if (opcode == MC::Cil.OpCodes.Ldind_U1) return UNI::OpCodes.Ldind_U1;
            else if (opcode == MC::Cil.OpCodes.Ldind_U2) return UNI::OpCodes.Ldind_U2;
            else if (opcode == MC::Cil.OpCodes.Ldind_U4) return UNI::OpCodes.Ldind_U4;
            else if (opcode == MC::Cil.OpCodes.Ldlen) return UNI::OpCodes.Ldlen;
            else if (opcode == MC::Cil.OpCodes.Ldloc) return UNI::OpCodes.Ldloc;
            else if (opcode == MC::Cil.OpCodes.Ldloc_0) return UNI::OpCodes.Ldloc_0;
            else if (opcode == MC::Cil.OpCodes.Ldloc_1) return UNI::OpCodes.Ldloc_1;
            else if (opcode == MC::Cil.OpCodes.Ldloc_2) return UNI::OpCodes.Ldloc_2;
            else if (opcode == MC::Cil.OpCodes.Ldloc_3) return UNI::OpCodes.Ldloc_3;
            else if (opcode == MC::Cil.OpCodes.Ldloc_S) return UNI::OpCodes.Ldloc_S;
            else if (opcode == MC::Cil.OpCodes.Ldloca) return UNI::OpCodes.Ldloca;
            else if (opcode == MC::Cil.OpCodes.Ldloca_S) return UNI::OpCodes.Ldloca_S;
            else if (opcode == MC::Cil.OpCodes.Ldnull) return UNI::OpCodes.Ldnull;
            else if (opcode == MC::Cil.OpCodes.Ldobj) return UNI::OpCodes.Ldobj;
            else if (opcode == MC::Cil.OpCodes.Ldsfld) return UNI::OpCodes.Ldsfld;
            else if (opcode == MC::Cil.OpCodes.Ldsflda) return UNI::OpCodes.Ldsflda;
            else if (opcode == MC::Cil.OpCodes.Ldstr) return UNI::OpCodes.Ldstr;
            else if (opcode == MC::Cil.OpCodes.Ldtoken) return UNI::OpCodes.Ldtoken;
            else if (opcode == MC::Cil.OpCodes.Ldvirtftn) return UNI::OpCodes.Ldvirtftn;
            else if (opcode == MC::Cil.OpCodes.Leave) return UNI::OpCodes.Leave;
            else if (opcode == MC::Cil.OpCodes.Leave_S) return UNI::OpCodes.Leave_S;
            else if (opcode == MC::Cil.OpCodes.Localloc) return UNI::OpCodes.Localloc;
            else if (opcode == MC::Cil.OpCodes.Mkrefany) return UNI::OpCodes.Mkrefany;
            else if (opcode == MC::Cil.OpCodes.Mul) return UNI::OpCodes.Mul;
            else if (opcode == MC::Cil.OpCodes.Mul_Ovf) return UNI::OpCodes.Mul_Ovf;
            else if (opcode == MC::Cil.OpCodes.Mul_Ovf_Un) return UNI::OpCodes.Mul_Ovf_Un;
            else if (opcode == MC::Cil.OpCodes.Neg) return UNI::OpCodes.Neg;
            else if (opcode == MC::Cil.OpCodes.Newarr) return UNI::OpCodes.Newarr;
            else if (opcode == MC::Cil.OpCodes.Newobj) return UNI::OpCodes.Newobj;
            else if (opcode == MC::Cil.OpCodes.Nop) return UNI::OpCodes.Nop;
            else if (opcode == MC::Cil.OpCodes.Not) return UNI::OpCodes.Not;
            else if (opcode == MC::Cil.OpCodes.Or) return UNI::OpCodes.Or;
            else if (opcode == MC::Cil.OpCodes.Pop) return UNI::OpCodes.Pop;
            else if (opcode == MC::Cil.OpCodes.Readonly) return UNI::OpCodes.Readonly;
            else if (opcode == MC::Cil.OpCodes.Refanytype) return UNI::OpCodes.Refanytype;
            else if (opcode == MC::Cil.OpCodes.Refanyval) return UNI::OpCodes.Refanyval;
            else if (opcode == MC::Cil.OpCodes.Rem) return UNI::OpCodes.Rem;
            else if (opcode == MC::Cil.OpCodes.Rem_Un) return UNI::OpCodes.Rem_Un;
            else if (opcode == MC::Cil.OpCodes.Ret) return UNI::OpCodes.Ret;
            else if (opcode == MC::Cil.OpCodes.Rethrow) return UNI::OpCodes.Rethrow;
            else if (opcode == MC::Cil.OpCodes.Shl) return UNI::OpCodes.Shl;
            else if (opcode == MC::Cil.OpCodes.Shr) return UNI::OpCodes.Shr;
            else if (opcode == MC::Cil.OpCodes.Shr_Un) return UNI::OpCodes.Shr_Un;
            else if (opcode == MC::Cil.OpCodes.Sizeof) return UNI::OpCodes.Sizeof;
            else if (opcode == MC::Cil.OpCodes.Starg) return UNI::OpCodes.Starg;
            else if (opcode == MC::Cil.OpCodes.Starg_S) return UNI::OpCodes.Starg_S;
            else if (opcode == MC::Cil.OpCodes.Stelem_Any) return UNI::OpCodes.Stelem;
            else if (opcode == MC::Cil.OpCodes.Stelem_I) return UNI::OpCodes.Stelem_I;
            else if (opcode == MC::Cil.OpCodes.Stelem_I1) return UNI::OpCodes.Stelem_I1;
            else if (opcode == MC::Cil.OpCodes.Stelem_I2) return UNI::OpCodes.Stelem_I2;
            else if (opcode == MC::Cil.OpCodes.Stelem_I4) return UNI::OpCodes.Stelem_I4;
            else if (opcode == MC::Cil.OpCodes.Stelem_I8) return UNI::OpCodes.Stelem_I8;
            else if (opcode == MC::Cil.OpCodes.Stelem_R4) return UNI::OpCodes.Stelem_R4;
            else if (opcode == MC::Cil.OpCodes.Stelem_R8) return UNI::OpCodes.Stelem_R8;
            else if (opcode == MC::Cil.OpCodes.Stelem_Ref) return UNI::OpCodes.Stelem_Ref;
            else if (opcode == MC::Cil.OpCodes.Stfld) return UNI::OpCodes.Stfld;
            else if (opcode == MC::Cil.OpCodes.Stind_I) return UNI::OpCodes.Stind_I;
            else if (opcode == MC::Cil.OpCodes.Stind_I1) return UNI::OpCodes.Stind_I1;
            else if (opcode == MC::Cil.OpCodes.Stind_I2) return UNI::OpCodes.Stind_I2;
            else if (opcode == MC::Cil.OpCodes.Stind_I4) return UNI::OpCodes.Stind_I4;
            else if (opcode == MC::Cil.OpCodes.Stind_I8) return UNI::OpCodes.Stind_I8;
            else if (opcode == MC::Cil.OpCodes.Stind_R4) return UNI::OpCodes.Stind_R4;
            else if (opcode == MC::Cil.OpCodes.Stind_R8) return UNI::OpCodes.Stind_R8;
            else if (opcode == MC::Cil.OpCodes.Stind_Ref) return UNI::OpCodes.Stind_Ref;
            else if (opcode == MC::Cil.OpCodes.Stloc) return UNI::OpCodes.Stloc;
            else if (opcode == MC::Cil.OpCodes.Stloc_0) return UNI::OpCodes.Stloc_0;
            else if (opcode == MC::Cil.OpCodes.Stloc_1) return UNI::OpCodes.Stloc_1;
            else if (opcode == MC::Cil.OpCodes.Stloc_2) return UNI::OpCodes.Stloc_2;
            else if (opcode == MC::Cil.OpCodes.Stloc_3) return UNI::OpCodes.Stloc_3;
            else if (opcode == MC::Cil.OpCodes.Stloc_S) return UNI::OpCodes.Stloc_S;
            else if (opcode == MC::Cil.OpCodes.Stobj) return UNI::OpCodes.Stobj;
            else if (opcode == MC::Cil.OpCodes.Stsfld) return UNI::OpCodes.Stsfld;
            else if (opcode == MC::Cil.OpCodes.Sub) return UNI::OpCodes.Sub;
            else if (opcode == MC::Cil.OpCodes.Sub_Ovf) return UNI::OpCodes.Sub_Ovf;
            else if (opcode == MC::Cil.OpCodes.Sub_Ovf_Un) return UNI::OpCodes.Sub_Ovf_Un;
            else if (opcode == MC::Cil.OpCodes.Switch) return UNI::OpCodes.Switch;
            else if (opcode == MC::Cil.OpCodes.Tail) return UNI::OpCodes.Tailcall;
            else if (opcode == MC::Cil.OpCodes.Throw) return UNI::OpCodes.Throw;
            else if (opcode == MC::Cil.OpCodes.Unaligned) return UNI::OpCodes.Unaligned;
            else if (opcode == MC::Cil.OpCodes.Unbox) return UNI::OpCodes.Unbox;
            else if (opcode == MC::Cil.OpCodes.Unbox_Any) return UNI::OpCodes.Unbox_Any;
            else if (opcode == MC::Cil.OpCodes.Volatile) return UNI::OpCodes.Volatile;
            else if (opcode == MC::Cil.OpCodes.Xor) return UNI::OpCodes.Xor;

            throw new NotSupportedException();
        }

        public static MC::Cil.OpCode Cast(this UNI::OpCode opcode)
        {
            if (opcode == UNI::OpCodes.Add) return MC::Cil.OpCodes.Add;
            else if (opcode == UNI::OpCodes.Add_Ovf) return MC::Cil.OpCodes.Add_Ovf;
            else if (opcode == UNI::OpCodes.Add_Ovf_Un) return MC::Cil.OpCodes.Add_Ovf_Un;
            else if (opcode == UNI::OpCodes.And) return MC::Cil.OpCodes.And;
            else if (opcode == UNI::OpCodes.Arglist) return MC::Cil.OpCodes.Arglist;
            else if (opcode == UNI::OpCodes.Beq) return MC::Cil.OpCodes.Beq;
            else if (opcode == UNI::OpCodes.Beq_S) return MC::Cil.OpCodes.Beq_S;
            else if (opcode == UNI::OpCodes.Bge) return MC::Cil.OpCodes.Bge;
            else if (opcode == UNI::OpCodes.Bge_S) return MC::Cil.OpCodes.Bge_S;
            else if (opcode == UNI::OpCodes.Bge_Un) return MC::Cil.OpCodes.Bge_Un;
            else if (opcode == UNI::OpCodes.Bge_Un_S) return MC::Cil.OpCodes.Bge_Un_S;
            else if (opcode == UNI::OpCodes.Bgt) return MC::Cil.OpCodes.Bgt;
            else if (opcode == UNI::OpCodes.Bgt_S) return MC::Cil.OpCodes.Bgt_S;
            else if (opcode == UNI::OpCodes.Bgt_Un) return MC::Cil.OpCodes.Bgt_Un;
            else if (opcode == UNI::OpCodes.Bgt_Un_S) return MC::Cil.OpCodes.Bgt_Un_S;
            else if (opcode == UNI::OpCodes.Ble) return MC::Cil.OpCodes.Ble;
            else if (opcode == UNI::OpCodes.Ble_S) return MC::Cil.OpCodes.Ble_S;
            else if (opcode == UNI::OpCodes.Ble_Un) return MC::Cil.OpCodes.Ble_Un;
            else if (opcode == UNI::OpCodes.Ble_Un_S) return MC::Cil.OpCodes.Ble_Un_S;
            else if (opcode == UNI::OpCodes.Blt) return MC::Cil.OpCodes.Blt;
            else if (opcode == UNI::OpCodes.Blt_S) return MC::Cil.OpCodes.Blt_S;
            else if (opcode == UNI::OpCodes.Blt_Un) return MC::Cil.OpCodes.Blt_Un;
            else if (opcode == UNI::OpCodes.Blt_Un_S) return MC::Cil.OpCodes.Blt_Un_S;
            else if (opcode == UNI::OpCodes.Bne_Un) return MC::Cil.OpCodes.Bne_Un;
            else if (opcode == UNI::OpCodes.Bne_Un_S) return MC::Cil.OpCodes.Bne_Un_S;
            else if (opcode == UNI::OpCodes.Box) return MC::Cil.OpCodes.Box;
            else if (opcode == UNI::OpCodes.Br) return MC::Cil.OpCodes.Br;
            else if (opcode == UNI::OpCodes.Br_S) return MC::Cil.OpCodes.Br_S;
            else if (opcode == UNI::OpCodes.Break) return MC::Cil.OpCodes.Break;
            else if (opcode == UNI::OpCodes.Brfalse) return MC::Cil.OpCodes.Brfalse;
            else if (opcode == UNI::OpCodes.Brfalse_S) return MC::Cil.OpCodes.Brfalse_S;
            else if (opcode == UNI::OpCodes.Brtrue) return MC::Cil.OpCodes.Brtrue;
            else if (opcode == UNI::OpCodes.Brtrue_S) return MC::Cil.OpCodes.Brtrue_S;
            else if (opcode == UNI::OpCodes.Call) return MC::Cil.OpCodes.Call;
            else if (opcode == UNI::OpCodes.Calli) return MC::Cil.OpCodes.Calli;
            else if (opcode == UNI::OpCodes.Callvirt) return MC::Cil.OpCodes.Callvirt;
            else if (opcode == UNI::OpCodes.Castclass) return MC::Cil.OpCodes.Castclass;
            else if (opcode == UNI::OpCodes.Ceq) return MC::Cil.OpCodes.Ceq;
            else if (opcode == UNI::OpCodes.Cgt) return MC::Cil.OpCodes.Cgt;
            else if (opcode == UNI::OpCodes.Cgt_Un) return MC::Cil.OpCodes.Cgt_Un;
            else if (opcode == UNI::OpCodes.Ckfinite) return MC::Cil.OpCodes.Ckfinite;
            else if (opcode == UNI::OpCodes.Clt) return MC::Cil.OpCodes.Clt;
            else if (opcode == UNI::OpCodes.Clt_Un) return MC::Cil.OpCodes.Clt_Un;
            else if (opcode == UNI::OpCodes.Constrained) return MC::Cil.OpCodes.Constrained;
            else if (opcode == UNI::OpCodes.Conv_I) return MC::Cil.OpCodes.Conv_I;
            else if (opcode == UNI::OpCodes.Conv_I1) return MC::Cil.OpCodes.Conv_I1;
            else if (opcode == UNI::OpCodes.Conv_I2) return MC::Cil.OpCodes.Conv_I2;
            else if (opcode == UNI::OpCodes.Conv_I4) return MC::Cil.OpCodes.Conv_I4;
            else if (opcode == UNI::OpCodes.Conv_I8) return MC::Cil.OpCodes.Conv_I8;
            else if (opcode == UNI::OpCodes.Conv_Ovf_I) return MC::Cil.OpCodes.Conv_Ovf_I;
            else if (opcode == UNI::OpCodes.Conv_Ovf_I_Un) return MC::Cil.OpCodes.Conv_Ovf_I_Un;
            else if (opcode == UNI::OpCodes.Conv_Ovf_I1) return MC::Cil.OpCodes.Conv_Ovf_I1;
            else if (opcode == UNI::OpCodes.Conv_Ovf_I1_Un) return MC::Cil.OpCodes.Conv_Ovf_I1_Un;
            else if (opcode == UNI::OpCodes.Conv_Ovf_I2) return MC::Cil.OpCodes.Conv_Ovf_I2;
            else if (opcode == UNI::OpCodes.Conv_Ovf_I2_Un) return MC::Cil.OpCodes.Conv_Ovf_I2_Un;
            else if (opcode == UNI::OpCodes.Conv_Ovf_I4) return MC::Cil.OpCodes.Conv_Ovf_I4;
            else if (opcode == UNI::OpCodes.Conv_Ovf_I4_Un) return MC::Cil.OpCodes.Conv_Ovf_I4_Un;
            else if (opcode == UNI::OpCodes.Conv_Ovf_I8) return MC::Cil.OpCodes.Conv_Ovf_I8;
            else if (opcode == UNI::OpCodes.Conv_Ovf_I8_Un) return MC::Cil.OpCodes.Conv_Ovf_I8_Un;
            else if (opcode == UNI::OpCodes.Conv_Ovf_U) return MC::Cil.OpCodes.Conv_Ovf_U;
            else if (opcode == UNI::OpCodes.Conv_Ovf_U_Un) return MC::Cil.OpCodes.Conv_Ovf_U_Un;
            else if (opcode == UNI::OpCodes.Conv_Ovf_U1) return MC::Cil.OpCodes.Conv_Ovf_U1;
            else if (opcode == UNI::OpCodes.Conv_Ovf_U1_Un) return MC::Cil.OpCodes.Conv_Ovf_U1_Un;
            else if (opcode == UNI::OpCodes.Conv_Ovf_U2) return MC::Cil.OpCodes.Conv_Ovf_U2;
            else if (opcode == UNI::OpCodes.Conv_Ovf_U2_Un) return MC::Cil.OpCodes.Conv_Ovf_U2_Un;
            else if (opcode == UNI::OpCodes.Conv_Ovf_U4) return MC::Cil.OpCodes.Conv_Ovf_U4;
            else if (opcode == UNI::OpCodes.Conv_Ovf_U4_Un) return MC::Cil.OpCodes.Conv_Ovf_U4_Un;
            else if (opcode == UNI::OpCodes.Conv_Ovf_U8) return MC::Cil.OpCodes.Conv_Ovf_U8;
            else if (opcode == UNI::OpCodes.Conv_Ovf_U8_Un) return MC::Cil.OpCodes.Conv_Ovf_U8_Un;
            else if (opcode == UNI::OpCodes.Conv_R_Un) return MC::Cil.OpCodes.Conv_R_Un;
            else if (opcode == UNI::OpCodes.Conv_R4) return MC::Cil.OpCodes.Conv_R4;
            else if (opcode == UNI::OpCodes.Conv_R8) return MC::Cil.OpCodes.Conv_R8;
            else if (opcode == UNI::OpCodes.Conv_U) return MC::Cil.OpCodes.Conv_U;
            else if (opcode == UNI::OpCodes.Conv_U1) return MC::Cil.OpCodes.Conv_U1;
            else if (opcode == UNI::OpCodes.Conv_U2) return MC::Cil.OpCodes.Conv_U2;
            else if (opcode == UNI::OpCodes.Conv_U4) return MC::Cil.OpCodes.Conv_U4;
            else if (opcode == UNI::OpCodes.Conv_U8) return MC::Cil.OpCodes.Conv_U8;
            else if (opcode == UNI::OpCodes.Cpblk) return MC::Cil.OpCodes.Cpblk;
            else if (opcode == UNI::OpCodes.Cpobj) return MC::Cil.OpCodes.Cpobj;
            else if (opcode == UNI::OpCodes.Div) return MC::Cil.OpCodes.Div;
            else if (opcode == UNI::OpCodes.Div_Un) return MC::Cil.OpCodes.Div_Un;
            else if (opcode == UNI::OpCodes.Dup) return MC::Cil.OpCodes.Dup;
            else if (opcode == UNI::OpCodes.Endfilter) return MC::Cil.OpCodes.Endfilter;
            else if (opcode == UNI::OpCodes.Endfinally) return MC::Cil.OpCodes.Endfinally;
            else if (opcode == UNI::OpCodes.Initblk) return MC::Cil.OpCodes.Initblk;
            else if (opcode == UNI::OpCodes.Initobj) return MC::Cil.OpCodes.Initobj;
            else if (opcode == UNI::OpCodes.Isinst) return MC::Cil.OpCodes.Isinst;
            else if (opcode == UNI::OpCodes.Jmp) return MC::Cil.OpCodes.Jmp;
            else if (opcode == UNI::OpCodes.Ldarg) return MC::Cil.OpCodes.Ldarg;
            else if (opcode == UNI::OpCodes.Ldarg_0) return MC::Cil.OpCodes.Ldarg_0;
            else if (opcode == UNI::OpCodes.Ldarg_1) return MC::Cil.OpCodes.Ldarg_1;
            else if (opcode == UNI::OpCodes.Ldarg_2) return MC::Cil.OpCodes.Ldarg_2;
            else if (opcode == UNI::OpCodes.Ldarg_3) return MC::Cil.OpCodes.Ldarg_3;
            else if (opcode == UNI::OpCodes.Ldarg_S) return MC::Cil.OpCodes.Ldarg_S;
            else if (opcode == UNI::OpCodes.Ldarga) return MC::Cil.OpCodes.Ldarga;
            else if (opcode == UNI::OpCodes.Ldarga_S) return MC::Cil.OpCodes.Ldarga_S;
            else if (opcode == UNI::OpCodes.Ldc_I4) return MC::Cil.OpCodes.Ldc_I4;
            else if (opcode == UNI::OpCodes.Ldc_I4_0) return MC::Cil.OpCodes.Ldc_I4_0;
            else if (opcode == UNI::OpCodes.Ldc_I4_1) return MC::Cil.OpCodes.Ldc_I4_1;
            else if (opcode == UNI::OpCodes.Ldc_I4_2) return MC::Cil.OpCodes.Ldc_I4_2;
            else if (opcode == UNI::OpCodes.Ldc_I4_3) return MC::Cil.OpCodes.Ldc_I4_3;
            else if (opcode == UNI::OpCodes.Ldc_I4_4) return MC::Cil.OpCodes.Ldc_I4_4;
            else if (opcode == UNI::OpCodes.Ldc_I4_5) return MC::Cil.OpCodes.Ldc_I4_5;
            else if (opcode == UNI::OpCodes.Ldc_I4_6) return MC::Cil.OpCodes.Ldc_I4_6;
            else if (opcode == UNI::OpCodes.Ldc_I4_7) return MC::Cil.OpCodes.Ldc_I4_7;
            else if (opcode == UNI::OpCodes.Ldc_I4_8) return MC::Cil.OpCodes.Ldc_I4_8;
            else if (opcode == UNI::OpCodes.Ldc_I4_M1) return MC::Cil.OpCodes.Ldc_I4_M1;
            else if (opcode == UNI::OpCodes.Ldc_I4_S) return MC::Cil.OpCodes.Ldc_I4_S;
            else if (opcode == UNI::OpCodes.Ldc_I8) return MC::Cil.OpCodes.Ldc_I8;
            else if (opcode == UNI::OpCodes.Ldc_R4) return MC::Cil.OpCodes.Ldc_R4;
            else if (opcode == UNI::OpCodes.Ldc_R8) return MC::Cil.OpCodes.Ldc_R8;
            else if (opcode == UNI::OpCodes.Ldelem) return MC::Cil.OpCodes.Ldelem_Any;
            else if (opcode == UNI::OpCodes.Ldelem_I) return MC::Cil.OpCodes.Ldelem_I;
            else if (opcode == UNI::OpCodes.Ldelem_I1) return MC::Cil.OpCodes.Ldelem_I1;
            else if (opcode == UNI::OpCodes.Ldelem_I2) return MC::Cil.OpCodes.Ldelem_I2;
            else if (opcode == UNI::OpCodes.Ldelem_I4) return MC::Cil.OpCodes.Ldelem_I4;
            else if (opcode == UNI::OpCodes.Ldelem_I8) return MC::Cil.OpCodes.Ldelem_I8;
            else if (opcode == UNI::OpCodes.Ldelem_R4) return MC::Cil.OpCodes.Ldelem_R4;
            else if (opcode == UNI::OpCodes.Ldelem_R8) return MC::Cil.OpCodes.Ldelem_R8;
            else if (opcode == UNI::OpCodes.Ldelem_Ref) return MC::Cil.OpCodes.Ldelem_Ref;
            else if (opcode == UNI::OpCodes.Ldelem_U1) return MC::Cil.OpCodes.Ldelem_U1;
            else if (opcode == UNI::OpCodes.Ldelem_U2) return MC::Cil.OpCodes.Ldelem_U2;
            else if (opcode == UNI::OpCodes.Ldelem_U4) return MC::Cil.OpCodes.Ldelem_U4;
            else if (opcode == UNI::OpCodes.Ldelema) return MC::Cil.OpCodes.Ldelema;
            else if (opcode == UNI::OpCodes.Ldfld) return MC::Cil.OpCodes.Ldfld;
            else if (opcode == UNI::OpCodes.Ldflda) return MC::Cil.OpCodes.Ldflda;
            else if (opcode == UNI::OpCodes.Ldftn) return MC::Cil.OpCodes.Ldftn;
            else if (opcode == UNI::OpCodes.Ldind_I) return MC::Cil.OpCodes.Ldind_I;
            else if (opcode == UNI::OpCodes.Ldind_I1) return MC::Cil.OpCodes.Ldind_I1;
            else if (opcode == UNI::OpCodes.Ldind_I2) return MC::Cil.OpCodes.Ldind_I2;
            else if (opcode == UNI::OpCodes.Ldind_I4) return MC::Cil.OpCodes.Ldind_I4;
            else if (opcode == UNI::OpCodes.Ldind_I8) return MC::Cil.OpCodes.Ldind_I8;
            else if (opcode == UNI::OpCodes.Ldind_R4) return MC::Cil.OpCodes.Ldind_R4;
            else if (opcode == UNI::OpCodes.Ldind_R8) return MC::Cil.OpCodes.Ldind_R8;
            else if (opcode == UNI::OpCodes.Ldind_Ref) return MC::Cil.OpCodes.Ldind_Ref;
            else if (opcode == UNI::OpCodes.Ldind_U1) return MC::Cil.OpCodes.Ldind_U1;
            else if (opcode == UNI::OpCodes.Ldind_U2) return MC::Cil.OpCodes.Ldind_U2;
            else if (opcode == UNI::OpCodes.Ldind_U4) return MC::Cil.OpCodes.Ldind_U4;
            else if (opcode == UNI::OpCodes.Ldlen) return MC::Cil.OpCodes.Ldlen;
            else if (opcode == UNI::OpCodes.Ldloc) return MC::Cil.OpCodes.Ldloc;
            else if (opcode == UNI::OpCodes.Ldloc_0) return MC::Cil.OpCodes.Ldloc_0;
            else if (opcode == UNI::OpCodes.Ldloc_1) return MC::Cil.OpCodes.Ldloc_1;
            else if (opcode == UNI::OpCodes.Ldloc_2) return MC::Cil.OpCodes.Ldloc_2;
            else if (opcode == UNI::OpCodes.Ldloc_3) return MC::Cil.OpCodes.Ldloc_3;
            else if (opcode == UNI::OpCodes.Ldloc_S) return MC::Cil.OpCodes.Ldloc_S;
            else if (opcode == UNI::OpCodes.Ldloca) return MC::Cil.OpCodes.Ldloca;
            else if (opcode == UNI::OpCodes.Ldloca_S) return MC::Cil.OpCodes.Ldloca_S;
            else if (opcode == UNI::OpCodes.Ldnull) return MC::Cil.OpCodes.Ldnull;
            else if (opcode == UNI::OpCodes.Ldobj) return MC::Cil.OpCodes.Ldobj;
            else if (opcode == UNI::OpCodes.Ldsfld) return MC::Cil.OpCodes.Ldsfld;
            else if (opcode == UNI::OpCodes.Ldsflda) return MC::Cil.OpCodes.Ldsflda;
            else if (opcode == UNI::OpCodes.Ldstr) return MC::Cil.OpCodes.Ldstr;
            else if (opcode == UNI::OpCodes.Ldtoken) return MC::Cil.OpCodes.Ldtoken;
            else if (opcode == UNI::OpCodes.Ldvirtftn) return MC::Cil.OpCodes.Ldvirtftn;
            else if (opcode == UNI::OpCodes.Leave) return MC::Cil.OpCodes.Leave;
            else if (opcode == UNI::OpCodes.Leave_S) return MC::Cil.OpCodes.Leave_S;
            else if (opcode == UNI::OpCodes.Localloc) return MC::Cil.OpCodes.Localloc;
            else if (opcode == UNI::OpCodes.Mkrefany) return MC::Cil.OpCodes.Mkrefany;
            else if (opcode == UNI::OpCodes.Mul) return MC::Cil.OpCodes.Mul;
            else if (opcode == UNI::OpCodes.Mul_Ovf) return MC::Cil.OpCodes.Mul_Ovf;
            else if (opcode == UNI::OpCodes.Mul_Ovf_Un) return MC::Cil.OpCodes.Mul_Ovf_Un;
            else if (opcode == UNI::OpCodes.Neg) return MC::Cil.OpCodes.Neg;
            else if (opcode == UNI::OpCodes.Newarr) return MC::Cil.OpCodes.Newarr;
            else if (opcode == UNI::OpCodes.Newobj) return MC::Cil.OpCodes.Newobj;
            else if (opcode == UNI::OpCodes.Nop) return MC::Cil.OpCodes.Nop;
            else if (opcode == UNI::OpCodes.Not) return MC::Cil.OpCodes.Not;
            else if (opcode == UNI::OpCodes.Or) return MC::Cil.OpCodes.Or;
            else if (opcode == UNI::OpCodes.Pop) return MC::Cil.OpCodes.Pop;
            else if (opcode == UNI::OpCodes.Readonly) return MC::Cil.OpCodes.Readonly;
            else if (opcode == UNI::OpCodes.Refanytype) return MC::Cil.OpCodes.Refanytype;
            else if (opcode == UNI::OpCodes.Refanyval) return MC::Cil.OpCodes.Refanyval;
            else if (opcode == UNI::OpCodes.Rem) return MC::Cil.OpCodes.Rem;
            else if (opcode == UNI::OpCodes.Rem_Un) return MC::Cil.OpCodes.Rem_Un;
            else if (opcode == UNI::OpCodes.Ret) return MC::Cil.OpCodes.Ret;
            else if (opcode == UNI::OpCodes.Rethrow) return MC::Cil.OpCodes.Rethrow;
            else if (opcode == UNI::OpCodes.Shl) return MC::Cil.OpCodes.Shl;
            else if (opcode == UNI::OpCodes.Shr) return MC::Cil.OpCodes.Shr;
            else if (opcode == UNI::OpCodes.Shr_Un) return MC::Cil.OpCodes.Shr_Un;
            else if (opcode == UNI::OpCodes.Sizeof) return MC::Cil.OpCodes.Sizeof;
            else if (opcode == UNI::OpCodes.Starg) return MC::Cil.OpCodes.Starg;
            else if (opcode == UNI::OpCodes.Starg_S) return MC::Cil.OpCodes.Starg_S;
            else if (opcode == UNI::OpCodes.Stelem) return MC::Cil.OpCodes.Stelem_Any;
            else if (opcode == UNI::OpCodes.Stelem_I) return MC::Cil.OpCodes.Stelem_I;
            else if (opcode == UNI::OpCodes.Stelem_I1) return MC::Cil.OpCodes.Stelem_I1;
            else if (opcode == UNI::OpCodes.Stelem_I2) return MC::Cil.OpCodes.Stelem_I2;
            else if (opcode == UNI::OpCodes.Stelem_I4) return MC::Cil.OpCodes.Stelem_I4;
            else if (opcode == UNI::OpCodes.Stelem_I8) return MC::Cil.OpCodes.Stelem_I8;
            else if (opcode == UNI::OpCodes.Stelem_R4) return MC::Cil.OpCodes.Stelem_R4;
            else if (opcode == UNI::OpCodes.Stelem_R8) return MC::Cil.OpCodes.Stelem_R8;
            else if (opcode == UNI::OpCodes.Stelem_Ref) return MC::Cil.OpCodes.Stelem_Ref;
            else if (opcode == UNI::OpCodes.Stfld) return MC::Cil.OpCodes.Stfld;
            else if (opcode == UNI::OpCodes.Stind_I) return MC::Cil.OpCodes.Stind_I;
            else if (opcode == UNI::OpCodes.Stind_I1) return MC::Cil.OpCodes.Stind_I1;
            else if (opcode == UNI::OpCodes.Stind_I2) return MC::Cil.OpCodes.Stind_I2;
            else if (opcode == UNI::OpCodes.Stind_I4) return MC::Cil.OpCodes.Stind_I4;
            else if (opcode == UNI::OpCodes.Stind_I8) return MC::Cil.OpCodes.Stind_I8;
            else if (opcode == UNI::OpCodes.Stind_R4) return MC::Cil.OpCodes.Stind_R4;
            else if (opcode == UNI::OpCodes.Stind_R8) return MC::Cil.OpCodes.Stind_R8;
            else if (opcode == UNI::OpCodes.Stind_Ref) return MC::Cil.OpCodes.Stind_Ref;
            else if (opcode == UNI::OpCodes.Stloc) return MC::Cil.OpCodes.Stloc;
            else if (opcode == UNI::OpCodes.Stloc_0) return MC::Cil.OpCodes.Stloc_0;
            else if (opcode == UNI::OpCodes.Stloc_1) return MC::Cil.OpCodes.Stloc_1;
            else if (opcode == UNI::OpCodes.Stloc_2) return MC::Cil.OpCodes.Stloc_2;
            else if (opcode == UNI::OpCodes.Stloc_3) return MC::Cil.OpCodes.Stloc_3;
            else if (opcode == UNI::OpCodes.Stloc_S) return MC::Cil.OpCodes.Stloc_S;
            else if (opcode == UNI::OpCodes.Stobj) return MC::Cil.OpCodes.Stobj;
            else if (opcode == UNI::OpCodes.Stsfld) return MC::Cil.OpCodes.Stsfld;
            else if (opcode == UNI::OpCodes.Sub) return MC::Cil.OpCodes.Sub;
            else if (opcode == UNI::OpCodes.Sub_Ovf) return MC::Cil.OpCodes.Sub_Ovf;
            else if (opcode == UNI::OpCodes.Sub_Ovf_Un) return MC::Cil.OpCodes.Sub_Ovf_Un;
            else if (opcode == UNI::OpCodes.Switch) return MC::Cil.OpCodes.Switch;
            else if (opcode == UNI::OpCodes.Tailcall) return MC::Cil.OpCodes.Tail;
            else if (opcode == UNI::OpCodes.Throw) return MC::Cil.OpCodes.Throw;
            else if (opcode == UNI::OpCodes.Unaligned) return MC::Cil.OpCodes.Unaligned;
            else if (opcode == UNI::OpCodes.Unbox) return MC::Cil.OpCodes.Unbox;
            else if (opcode == UNI::OpCodes.Unbox_Any) return MC::Cil.OpCodes.Unbox_Any;
            else if (opcode == UNI::OpCodes.Volatile) return MC::Cil.OpCodes.Volatile;
            else if (opcode == UNI::OpCodes.Xor) return MC::Cil.OpCodes.Xor;

            throw new NotSupportedException();
        }
    }
}
