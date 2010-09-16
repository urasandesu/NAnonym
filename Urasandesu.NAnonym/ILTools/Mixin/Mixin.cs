using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Mono.Cecil;
//using Mono.Cecil.Cil;
using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.Linq;
//using Urasandesu.NAnonym.ILTools.Impl.Mono.Cecil;
using Urasandesu.NAnonym.ILTools.Impl.System.Reflection;
using SR = System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{
    // 拡張メソッドを partial 宣言ってありかも！？
    // TODO: クラスを分割。まずは Duplicate 系を Clonable へ。Equivalent 系を Comparable へ。Get* 系を Compatible へ。
    // MEMO: オーバーロードは必ず同じクラスで宣言するべし。縦断してしまうとすぐコンパイルエラーにならずわかりにくいバグになる。
    // MEMO: ファイルだけ Mixin させる型で分割して、実体は全て Mixin の partial クラスにすれば上記のバグは出にくい。無理やりクラス分割する必要ないかも。
    // MEMO: ファイル分割したいのはバージョン管理システムの制限のため。
    public static partial class Mixin
    {

        //// だめだ。このまま Write すると更新されてしまう。やっぱり Copy メソッドいるし。
        //private static IEnumerable<MethodDefinition> GetMethodDefs(this TypeSpecification typeSpec)
        //{
        //    throw new NotImplementedException();
        //}

        //public static bool Equivalent(this MethodReference x, MethodBase y)
        //{
        //    bool equals = x.DeclaringType.Equivalent(y.DeclaringType);
        //    equals = equals && x.Name == y.Name;
        //    equals = equals && x.Parameters.Equivalent(y.GetParameters());
        //    return equals;
        //}

        //public static bool Equivalent(this MethodDefinition x, MethodInfo y)
        //{
        //    bool equals = Equivalent((MethodReference)x, (MethodBase)y);
        //    equals = equals && x.Attributes.Equivalent(y.Attributes);
        //    return equals;
        //}

        //public static bool Equivalent(this MethodDefinition x, MethodDefinition y)
        //{
        //    bool equals = x.DeclaringType.Equivalent(y.DeclaringType);
        //    equals = equals && x.Name == y.Name;
        //    equals = equals && x.Attributes == y.Attributes;
        //    equals = equals && x.Parameters.Equivalent(y.Parameters);
        //    // TODO: CloneEx でコピー対象となった項目も必要！
        //    return equals;
        //}

        //public static bool Equivalent(this MethodDefinition x, ConstructorInfo y)
        //{
        //    bool equals = x.IsConstructor;
        //    equals = equals && x.Attributes.Equivalent(y.Attributes);
        //    equals = equals && x.Parameters.Equivalent(y.GetParameters());
        //    return equals;
        //}

        //public static bool Equivalent(this Mono.Cecil.MethodAttributes x, System.Reflection.MethodAttributes y)
        //{
        //    return (int)x == (int)y;
        //}

        //public static Mono.Cecil.MethodAttributes ToMethodAttributes(this System.Reflection.MethodAttributes attribute)
        //{
        //    return (Mono.Cecil.MethodAttributes)(int)attribute;
        //}

        //#region ここ一筋縄じゃ行かなかった。例えば、BindingFlags.IgnoreCase だったら、あらかじめ name は全て小文字化しておきたい、とか。
        ////// ここ一筋縄じゃ行かなかった。例えば、BindingFlags.IgnoreCase だったら、あらかじめ name は全て小文字化しておきたい、とか。
        ////// BindingFlags でフィルターかませる感じで。
        ////public static bool IsInside(this MethodDefinition x, string name, BindingFlags flag)
        ////{
        ////    flag = 
        ////        flag == BindingFlags.Default ?
        ////            BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod : 
        ////            flag;

        ////    bool isInside = true;
        ////    if ((flag & BindingFlags.DeclaredOnly) == BindingFlags.DeclaredOnly) { }
        ////    if ((flag & BindingFlags.Instance) == BindingFlags.Instance) { }
        ////    if ((flag & BindingFlags.Static) == BindingFlags.Static) { }
        ////    if ((flag & BindingFlags.Public) == BindingFlags.Public) { }
        ////    if ((flag & BindingFlags.NonPublic) == BindingFlags.NonPublic) { }
        ////    if ((flag & BindingFlags.FlattenHierarchy) == BindingFlags.FlattenHierarchy) { }
        ////    if ((flag & BindingFlags.InvokeMethod) == BindingFlags.InvokeMethod) { }
        ////    if ((flag & BindingFlags.CreateInstance) == BindingFlags.CreateInstance) { }
        ////    if ((flag & BindingFlags.GetField) == BindingFlags.GetField) { }
        ////    if ((flag & BindingFlags.SetField) == BindingFlags.SetField) { }
        ////    if ((flag & BindingFlags.GetProperty) == BindingFlags.GetProperty) { }
        ////    if ((flag & BindingFlags.SetProperty) == BindingFlags.SetProperty) { }
        ////    if ((flag & BindingFlags.PutDispProperty) == BindingFlags.PutDispProperty) { }
        ////    if ((flag & BindingFlags.PutRefDispProperty) == BindingFlags.PutRefDispProperty) { }
        ////    if ((flag & BindingFlags.ExactBinding) == BindingFlags.ExactBinding) { }
        ////    if ((flag & BindingFlags.SuppressChangeType) == BindingFlags.SuppressChangeType) { }
        ////    if ((flag & BindingFlags.OptionalParamBinding) == BindingFlags.OptionalParamBinding) { }
        ////    if ((flag & BindingFlags.IgnoreReturn) == BindingFlags.IgnoreReturn) { }


        ////    throw new NotImplementedException();
        ////}
        //#endregion

        //public static bool Equivalent(this IEnumerable<ParameterDefinition> first, IEnumerable<Type> second)
        //{
        //    var comparer =
        //        Iterable.CreateEqualityComparerNullable(default(ParameterDefinition), default(Type),
        //        (firstItem, secondItem) =>
        //        {
        //            return firstItem.ParameterType.Equivalent(secondItem);
        //        });
        //    return first.Equivalent(second, comparer);
        //}

        //public static bool Equivalent(this IEnumerable<ParameterDefinition> first, IEnumerable<ParameterInfo> second)
        //{
        //    var comparer =
        //        Iterable.CreateEqualityComparerNullable(default(ParameterDefinition), default(ParameterInfo),
        //        (firstItem, secondItem) =>
        //        {
        //            return firstItem.Equivalent(secondItem);
        //        });
        //    return first.Equivalent(second, comparer);
        //}

        //public static bool Equivalent(this IEnumerable<ParameterDefinition> first, IEnumerable<ParameterDefinition> second)
        //{
        //    var comparer =
        //        Iterable.CreateEqualityComparerNullable(default(ParameterDefinition), default(ParameterDefinition),
        //        (firstItem, secondItem) =>
        //        {
        //            return firstItem.Equivalent(secondItem);
        //        });
        //    return first.Equivalent(second, comparer);
        //}

        //public static bool Equivalent(this ParameterDefinition x, ParameterInfo y)
        //{
        //    return x.Name == y.Name && x.ParameterType.Equivalent(y.ParameterType);
        //}

        //public static bool Equivalent(this ParameterDefinition x, ParameterDefinition y)
        //{
        //    return x.Name == y.Name && x.ParameterType.Equivalent(y.ParameterType);
        //}

        //public static bool Equivalent(this FieldDefinition x, FieldInfo y)
        //{
        //    return x.Name == y.Name && x.FieldType.Equivalent(y.FieldType);
        //}

        //public static AssemblyDefinition Duplicate(this AssemblyDefinition source)
        //{
        //    throw new NotImplementedException();
        //}

        //public static ModuleDefinition Duplicate(this ModuleDefinition source)
        //{
        //    throw new NotImplementedException();
        //}

        //public static TypeDefinition Duplicate(this TypeDefinition source)
        //{
        //    var destination = new TypeDefinition(source.Namespace, source.Name, source.Attributes, source.BaseType);
        //    return destination;
        //}

        //public static GenericInstanceType Duplicate(this GenericInstanceType source)
        //{
        //    throw new NotImplementedException();
        //}

        //public static MethodReference DuplicateWithoutBody(this MethodReference source)
        //{
        //    var sourceDef = default(MethodDefinition);
        //    var sourceGen = default(GenericInstanceMethod);
        //    if ((sourceDef = source as MethodDefinition) != null)
        //    {
        //        return sourceDef.DuplicateWithoutBody();
        //    }
        //    else if ((sourceGen = source as GenericInstanceMethod) != null)
        //    {
        //        return sourceGen.DuplicateWithoutBody();
        //    }
        //    else
        //    {
        //        throw new NotSupportedException();
        //    }
        //}

        //public static MethodReference Duplicate(this MethodReference source)
        //{
        //    var sourceDef = default(MethodDefinition);
        //    var sourceGen = default(GenericInstanceMethod);
        //    if ((sourceDef = source as MethodDefinition) != null)
        //    {
        //        return sourceDef.Duplicate();
        //    }
        //    else if ((sourceGen = source as GenericInstanceMethod) != null)
        //    {
        //        return sourceGen.Duplicate();
        //    }
        //    else
        //    {
        //        throw new NotSupportedException();
        //    }
        //}

        //public static MethodDefinition DuplicateWithoutBody(this MethodDefinition source)
        //{
        //    var destination = new MethodDefinition(source.Name, source.Attributes, source.ReturnType);
        //    if (source.HasGenericParameters)
        //    {
        //        source.GenericParameters.Select(_ => _.Duplicate(destination)).AddRangeTo(destination.GenericParameters);
        //    }
        //    source.Parameters.Select(_ => _.Duplicate()).AddRangeTo(destination.Parameters);
        //    return destination;
        //}

        //public static MethodDefinition Duplicate(this MethodDefinition source)
        //{
        //    var destination = source.DuplicateWithoutBody();
        //    destination.Body.InitLocals = source.Body.InitLocals;
        //    source.Body.Variables.Select(_ => _.Duplicate()).AddRangeTo(destination.Body.Variables);
        //    source.Body.Instructions.Select(_ => _.Duplicate()).AddRangeTo(destination.Body.Instructions);
        //    source.Body.ExceptionHandlers.Select(_ => _.Duplicate()).AddRangeTo(destination.Body.ExceptionHandlers);
        //    return destination;
        //}

        //public static GenericInstanceMethod DuplicateWithoutBody(this GenericInstanceMethod source)
        //{
        //    throw new NotImplementedException();
        //}

        //public static GenericInstanceMethod Duplicate(this GenericInstanceMethod source)
        //{
        //    var destination = new GenericInstanceMethod(source.ElementMethod.Duplicate());
        //    //source.genericpa
        //    throw new NotImplementedException();
        //}

        //public static ParameterDefinition Duplicate(this ParameterDefinition source)
        //{
        //    var destination = new ParameterDefinition(source.Name, source.Attributes, source.ParameterType);
        //    return destination;
        //}

        //public static GenericParameter Duplicate(this GenericParameter source, IGenericParameterProvider owner)
        //{
        //    var destination = new GenericParameter(source.Name, owner);
        //    return destination;
        //}

        //public static Instruction Duplicate(this Instruction source)
        //{
        //    byte? byteOperand = null;
        //    CallSite callSiteOperand = null;
        //    double? doubleOperand = null;
        //    FieldReference fieldReferenceOperand = null;
        //    float? floatOperand = null;
        //    Instruction instructionOperand = null;
        //    Instruction[] instructionArrayOperand = null;
        //    int? intOperand = null;
        //    long? longOperand = null;
        //    MethodReference methodReferenceOperand = null;
        //    ParameterDefinition parameterDefinitionOperand = null;
        //    sbyte? sbyteOperand = null;
        //    string stringOperand = null;
        //    TypeReference typeReferenceOperand = null;
        //    VariableDefinition variableDefinitionOperand = null;

        //    if ((byteOperand = source.Operand as byte?) != null) return Instruction.Create(source.OpCode, byteOperand.Value);
        //    else if ((callSiteOperand = source.Operand as CallSite) != null) return Instruction.Create(source.OpCode, callSiteOperand);
        //    else if ((doubleOperand = source.Operand as double?) != null) return Instruction.Create(source.OpCode, doubleOperand.Value);
        //    else if ((fieldReferenceOperand = source.Operand as FieldReference) != null) return Instruction.Create(source.OpCode, fieldReferenceOperand);
        //    else if ((floatOperand = source.Operand as float?) != null) return Instruction.Create(source.OpCode, floatOperand.Value);
        //    else if ((instructionOperand = source.Operand as Instruction) != null) return Instruction.Create(source.OpCode, instructionOperand);
        //    else if ((instructionArrayOperand = source.Operand as Instruction[]) != null) return Instruction.Create(source.OpCode, instructionArrayOperand);
        //    else if ((intOperand = source.Operand as int?) != null) return Instruction.Create(source.OpCode, intOperand.Value);
        //    else if ((longOperand = source.Operand as long?) != null) return Instruction.Create(source.OpCode, longOperand.Value);
        //    else if ((methodReferenceOperand = source.Operand as MethodReference) != null) return Instruction.Create(source.OpCode, methodReferenceOperand);
        //    else if ((parameterDefinitionOperand = source.Operand as ParameterDefinition) != null) return Instruction.Create(source.OpCode, parameterDefinitionOperand);
        //    else if ((sbyteOperand = source.Operand as sbyte?) != null) return Instruction.Create(source.OpCode, sbyteOperand.Value);
        //    else if ((stringOperand = source.Operand as string) != null) return Instruction.Create(source.OpCode, stringOperand);
        //    else if ((typeReferenceOperand = source.Operand as TypeReference) != null) return Instruction.Create(source.OpCode, typeReferenceOperand);
        //    else if ((variableDefinitionOperand = source.Operand as VariableDefinition) != null) return Instruction.Create(source.OpCode, variableDefinitionOperand);
        //    else return Instruction.Create(source.OpCode);
        //}

        //public static VariableDefinition Duplicate(this VariableDefinition source)
        //{
        //    var destination = new VariableDefinition(source.Name, source.VariableType);
        //    return destination;
        //}

        //public static ExceptionHandler Duplicate(this ExceptionHandler source)
        //{
        //    var destination = new ExceptionHandler(source.HandlerType);
        //    destination.CatchType = source.CatchType;
        //    destination.FilterEnd = source.FilterEnd;
        //    destination.FilterStart = source.FilterStart;
        //    destination.HandlerEnd = source.HandlerEnd;
        //    destination.HandlerStart = source.HandlerStart;
        //    destination.HandlerType = source.HandlerType;
        //    destination.TryEnd = source.TryEnd;
        //    destination.TryStart = source.TryStart;
        //    return destination;
        //}

        //public static FieldDefinition Duplicate(this FieldDefinition source)
        //{
        //    var destination = new FieldDefinition(source.Name, source.Attributes, source.FieldType);
        //    return destination;
        //}

        //public static PropertyDefinition Duplicate(this PropertyDefinition source)
        //{
        //    // Property のコピーは外側だけのコピーだけではなく、中身の get_ メソッド、 set_ メソッドのコピーが必要！
        //    var destination = new PropertyDefinition(source.Name, source.Attributes, source.PropertyType);
        //    return destination;
        //}











        public static bool IsStatic(this PropertyInfo source)
        {
            return (source.CanRead && source.GetGetMethod().IsStatic) || (source.CanWrite && source.GetSetMethod().IsStatic);
        }

        //public static PortableScope CarryPortableScope(this MethodDefinition methodDef)
        //{
        //    var scope = new PortableScope((MCMethodGeneratorImpl)methodDef);
        //    return scope;
        //}

        //public static void ExpressBody(this MethodDefinition methodDef, Action<ExpressiveMethodBodyGenerator> expression)    // TODO: ハンドラ化したほうが良いかも？
        //{
        //    var gen = new ExpressiveMethodBodyGenerator((MCMethodGeneratorImpl)methodDef);
        //    expression(gen);
        //    gen.Eval(_ => _.End());
        //}

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

        public static SR::Emit.OpCode Cast(this OpCode opcode)
        {
            if (opcode == OpCodes.Add) return SR::Emit.OpCodes.Add;
            else if (opcode == OpCodes.Add_Ovf) return SR::Emit.OpCodes.Add_Ovf;
            else if (opcode == OpCodes.Add_Ovf_Un) return SR::Emit.OpCodes.Add_Ovf_Un;
            else if (opcode == OpCodes.And) return SR::Emit.OpCodes.And;
            else if (opcode == OpCodes.Arglist) return SR::Emit.OpCodes.Arglist;
            else if (opcode == OpCodes.Beq) return SR::Emit.OpCodes.Beq;
            else if (opcode == OpCodes.Beq_S) return SR::Emit.OpCodes.Beq_S;
            else if (opcode == OpCodes.Bge) return SR::Emit.OpCodes.Bge;
            else if (opcode == OpCodes.Bge_S) return SR::Emit.OpCodes.Bge_S;
            else if (opcode == OpCodes.Bge_Un) return SR::Emit.OpCodes.Bge_Un;
            else if (opcode == OpCodes.Bge_Un_S) return SR::Emit.OpCodes.Bge_Un_S;
            else if (opcode == OpCodes.Bgt) return SR::Emit.OpCodes.Bgt;
            else if (opcode == OpCodes.Bgt_S) return SR::Emit.OpCodes.Bgt_S;
            else if (opcode == OpCodes.Bgt_Un) return SR::Emit.OpCodes.Bgt_Un;
            else if (opcode == OpCodes.Bgt_Un_S) return SR::Emit.OpCodes.Bgt_Un_S;
            else if (opcode == OpCodes.Ble) return SR::Emit.OpCodes.Ble;
            else if (opcode == OpCodes.Ble_S) return SR::Emit.OpCodes.Ble_S;
            else if (opcode == OpCodes.Ble_Un) return SR::Emit.OpCodes.Ble_Un;
            else if (opcode == OpCodes.Ble_Un_S) return SR::Emit.OpCodes.Ble_Un_S;
            else if (opcode == OpCodes.Blt) return SR::Emit.OpCodes.Blt;
            else if (opcode == OpCodes.Blt_S) return SR::Emit.OpCodes.Blt_S;
            else if (opcode == OpCodes.Blt_Un) return SR::Emit.OpCodes.Blt_Un;
            else if (opcode == OpCodes.Blt_Un_S) return SR::Emit.OpCodes.Blt_Un_S;
            else if (opcode == OpCodes.Bne_Un) return SR::Emit.OpCodes.Bne_Un;
            else if (opcode == OpCodes.Bne_Un_S) return SR::Emit.OpCodes.Bne_Un_S;
            else if (opcode == OpCodes.Box) return SR::Emit.OpCodes.Box;
            else if (opcode == OpCodes.Br) return SR::Emit.OpCodes.Br;
            else if (opcode == OpCodes.Br_S) return SR::Emit.OpCodes.Br_S;
            else if (opcode == OpCodes.Break) return SR::Emit.OpCodes.Break;
            else if (opcode == OpCodes.Brfalse) return SR::Emit.OpCodes.Brfalse;
            else if (opcode == OpCodes.Brfalse_S) return SR::Emit.OpCodes.Brfalse_S;
            else if (opcode == OpCodes.Brtrue) return SR::Emit.OpCodes.Brtrue;
            else if (opcode == OpCodes.Brtrue_S) return SR::Emit.OpCodes.Brtrue_S;
            else if (opcode == OpCodes.Call) return SR::Emit.OpCodes.Call;
            else if (opcode == OpCodes.Calli) return SR::Emit.OpCodes.Calli;
            else if (opcode == OpCodes.Callvirt) return SR::Emit.OpCodes.Callvirt;
            else if (opcode == OpCodes.Castclass) return SR::Emit.OpCodes.Castclass;
            else if (opcode == OpCodes.Ceq) return SR::Emit.OpCodes.Ceq;
            else if (opcode == OpCodes.Cgt) return SR::Emit.OpCodes.Cgt;
            else if (opcode == OpCodes.Cgt_Un) return SR::Emit.OpCodes.Cgt_Un;
            else if (opcode == OpCodes.Ckfinite) return SR::Emit.OpCodes.Ckfinite;
            else if (opcode == OpCodes.Clt) return SR::Emit.OpCodes.Clt;
            else if (opcode == OpCodes.Clt_Un) return SR::Emit.OpCodes.Clt_Un;
            else if (opcode == OpCodes.Constrained) return SR::Emit.OpCodes.Constrained;
            else if (opcode == OpCodes.Conv_I) return SR::Emit.OpCodes.Conv_I;
            else if (opcode == OpCodes.Conv_I1) return SR::Emit.OpCodes.Conv_I1;
            else if (opcode == OpCodes.Conv_I2) return SR::Emit.OpCodes.Conv_I2;
            else if (opcode == OpCodes.Conv_I4) return SR::Emit.OpCodes.Conv_I4;
            else if (opcode == OpCodes.Conv_I8) return SR::Emit.OpCodes.Conv_I8;
            else if (opcode == OpCodes.Conv_Ovf_I) return SR::Emit.OpCodes.Conv_Ovf_I;
            else if (opcode == OpCodes.Conv_Ovf_I_Un) return SR::Emit.OpCodes.Conv_Ovf_I_Un;
            else if (opcode == OpCodes.Conv_Ovf_I1) return SR::Emit.OpCodes.Conv_Ovf_I1;
            else if (opcode == OpCodes.Conv_Ovf_I1_Un) return SR::Emit.OpCodes.Conv_Ovf_I1_Un;
            else if (opcode == OpCodes.Conv_Ovf_I2) return SR::Emit.OpCodes.Conv_Ovf_I2;
            else if (opcode == OpCodes.Conv_Ovf_I2_Un) return SR::Emit.OpCodes.Conv_Ovf_I2_Un;
            else if (opcode == OpCodes.Conv_Ovf_I4) return SR::Emit.OpCodes.Conv_Ovf_I4;
            else if (opcode == OpCodes.Conv_Ovf_I4_Un) return SR::Emit.OpCodes.Conv_Ovf_I4_Un;
            else if (opcode == OpCodes.Conv_Ovf_I8) return SR::Emit.OpCodes.Conv_Ovf_I8;
            else if (opcode == OpCodes.Conv_Ovf_I8_Un) return SR::Emit.OpCodes.Conv_Ovf_I8_Un;
            else if (opcode == OpCodes.Conv_Ovf_U) return SR::Emit.OpCodes.Conv_Ovf_U;
            else if (opcode == OpCodes.Conv_Ovf_U_Un) return SR::Emit.OpCodes.Conv_Ovf_U_Un;
            else if (opcode == OpCodes.Conv_Ovf_U1) return SR::Emit.OpCodes.Conv_Ovf_U1;
            else if (opcode == OpCodes.Conv_Ovf_U1_Un) return SR::Emit.OpCodes.Conv_Ovf_U1_Un;
            else if (opcode == OpCodes.Conv_Ovf_U2) return SR::Emit.OpCodes.Conv_Ovf_U2;
            else if (opcode == OpCodes.Conv_Ovf_U2_Un) return SR::Emit.OpCodes.Conv_Ovf_U2_Un;
            else if (opcode == OpCodes.Conv_Ovf_U4) return SR::Emit.OpCodes.Conv_Ovf_U4;
            else if (opcode == OpCodes.Conv_Ovf_U4_Un) return SR::Emit.OpCodes.Conv_Ovf_U4_Un;
            else if (opcode == OpCodes.Conv_Ovf_U8) return SR::Emit.OpCodes.Conv_Ovf_U8;
            else if (opcode == OpCodes.Conv_Ovf_U8_Un) return SR::Emit.OpCodes.Conv_Ovf_U8_Un;
            else if (opcode == OpCodes.Conv_R_Un) return SR::Emit.OpCodes.Conv_R_Un;
            else if (opcode == OpCodes.Conv_R4) return SR::Emit.OpCodes.Conv_R4;
            else if (opcode == OpCodes.Conv_R8) return SR::Emit.OpCodes.Conv_R8;
            else if (opcode == OpCodes.Conv_U) return SR::Emit.OpCodes.Conv_U;
            else if (opcode == OpCodes.Conv_U1) return SR::Emit.OpCodes.Conv_U1;
            else if (opcode == OpCodes.Conv_U2) return SR::Emit.OpCodes.Conv_U2;
            else if (opcode == OpCodes.Conv_U4) return SR::Emit.OpCodes.Conv_U4;
            else if (opcode == OpCodes.Conv_U8) return SR::Emit.OpCodes.Conv_U8;
            else if (opcode == OpCodes.Cpblk) return SR::Emit.OpCodes.Cpblk;
            else if (opcode == OpCodes.Cpobj) return SR::Emit.OpCodes.Cpobj;
            else if (opcode == OpCodes.Div) return SR::Emit.OpCodes.Div;
            else if (opcode == OpCodes.Div_Un) return SR::Emit.OpCodes.Div_Un;
            else if (opcode == OpCodes.Dup) return SR::Emit.OpCodes.Dup;
            else if (opcode == OpCodes.Endfilter) return SR::Emit.OpCodes.Endfilter;
            else if (opcode == OpCodes.Endfinally) return SR::Emit.OpCodes.Endfinally;
            else if (opcode == OpCodes.Initblk) return SR::Emit.OpCodes.Initblk;
            else if (opcode == OpCodes.Initobj) return SR::Emit.OpCodes.Initobj;
            else if (opcode == OpCodes.Isinst) return SR::Emit.OpCodes.Isinst;
            else if (opcode == OpCodes.Jmp) return SR::Emit.OpCodes.Jmp;
            else if (opcode == OpCodes.Ldarg) return SR::Emit.OpCodes.Ldarg;
            else if (opcode == OpCodes.Ldarg_0) return SR::Emit.OpCodes.Ldarg_0;
            else if (opcode == OpCodes.Ldarg_1) return SR::Emit.OpCodes.Ldarg_1;
            else if (opcode == OpCodes.Ldarg_2) return SR::Emit.OpCodes.Ldarg_2;
            else if (opcode == OpCodes.Ldarg_3) return SR::Emit.OpCodes.Ldarg_3;
            else if (opcode == OpCodes.Ldarg_S) return SR::Emit.OpCodes.Ldarg_S;
            else if (opcode == OpCodes.Ldarga) return SR::Emit.OpCodes.Ldarga;
            else if (opcode == OpCodes.Ldarga_S) return SR::Emit.OpCodes.Ldarga_S;
            else if (opcode == OpCodes.Ldc_I4) return SR::Emit.OpCodes.Ldc_I4;
            else if (opcode == OpCodes.Ldc_I4_0) return SR::Emit.OpCodes.Ldc_I4_0;
            else if (opcode == OpCodes.Ldc_I4_1) return SR::Emit.OpCodes.Ldc_I4_1;
            else if (opcode == OpCodes.Ldc_I4_2) return SR::Emit.OpCodes.Ldc_I4_2;
            else if (opcode == OpCodes.Ldc_I4_3) return SR::Emit.OpCodes.Ldc_I4_3;
            else if (opcode == OpCodes.Ldc_I4_4) return SR::Emit.OpCodes.Ldc_I4_4;
            else if (opcode == OpCodes.Ldc_I4_5) return SR::Emit.OpCodes.Ldc_I4_5;
            else if (opcode == OpCodes.Ldc_I4_6) return SR::Emit.OpCodes.Ldc_I4_6;
            else if (opcode == OpCodes.Ldc_I4_7) return SR::Emit.OpCodes.Ldc_I4_7;
            else if (opcode == OpCodes.Ldc_I4_8) return SR::Emit.OpCodes.Ldc_I4_8;
            else if (opcode == OpCodes.Ldc_I4_M1) return SR::Emit.OpCodes.Ldc_I4_M1;
            else if (opcode == OpCodes.Ldc_I4_S) return SR::Emit.OpCodes.Ldc_I4_S;
            else if (opcode == OpCodes.Ldc_I8) return SR::Emit.OpCodes.Ldc_I8;
            else if (opcode == OpCodes.Ldc_R4) return SR::Emit.OpCodes.Ldc_R4;
            else if (opcode == OpCodes.Ldc_R8) return SR::Emit.OpCodes.Ldc_R8;
            else if (opcode == OpCodes.Ldelem) return SR::Emit.OpCodes.Ldelem;
            else if (opcode == OpCodes.Ldelem_I) return SR::Emit.OpCodes.Ldelem_I;
            else if (opcode == OpCodes.Ldelem_I1) return SR::Emit.OpCodes.Ldelem_I1;
            else if (opcode == OpCodes.Ldelem_I2) return SR::Emit.OpCodes.Ldelem_I2;
            else if (opcode == OpCodes.Ldelem_I4) return SR::Emit.OpCodes.Ldelem_I4;
            else if (opcode == OpCodes.Ldelem_I8) return SR::Emit.OpCodes.Ldelem_I8;
            else if (opcode == OpCodes.Ldelem_R4) return SR::Emit.OpCodes.Ldelem_R4;
            else if (opcode == OpCodes.Ldelem_R8) return SR::Emit.OpCodes.Ldelem_R8;
            else if (opcode == OpCodes.Ldelem_Ref) return SR::Emit.OpCodes.Ldelem_Ref;
            else if (opcode == OpCodes.Ldelem_U1) return SR::Emit.OpCodes.Ldelem_U1;
            else if (opcode == OpCodes.Ldelem_U2) return SR::Emit.OpCodes.Ldelem_U2;
            else if (opcode == OpCodes.Ldelem_U4) return SR::Emit.OpCodes.Ldelem_U4;
            else if (opcode == OpCodes.Ldelema) return SR::Emit.OpCodes.Ldelema;
            else if (opcode == OpCodes.Ldfld) return SR::Emit.OpCodes.Ldfld;
            else if (opcode == OpCodes.Ldflda) return SR::Emit.OpCodes.Ldflda;
            else if (opcode == OpCodes.Ldftn) return SR::Emit.OpCodes.Ldftn;
            else if (opcode == OpCodes.Ldind_I) return SR::Emit.OpCodes.Ldind_I;
            else if (opcode == OpCodes.Ldind_I1) return SR::Emit.OpCodes.Ldind_I1;
            else if (opcode == OpCodes.Ldind_I2) return SR::Emit.OpCodes.Ldind_I2;
            else if (opcode == OpCodes.Ldind_I4) return SR::Emit.OpCodes.Ldind_I4;
            else if (opcode == OpCodes.Ldind_I8) return SR::Emit.OpCodes.Ldind_I8;
            else if (opcode == OpCodes.Ldind_R4) return SR::Emit.OpCodes.Ldind_R4;
            else if (opcode == OpCodes.Ldind_R8) return SR::Emit.OpCodes.Ldind_R8;
            else if (opcode == OpCodes.Ldind_Ref) return SR::Emit.OpCodes.Ldind_Ref;
            else if (opcode == OpCodes.Ldind_U1) return SR::Emit.OpCodes.Ldind_U1;
            else if (opcode == OpCodes.Ldind_U2) return SR::Emit.OpCodes.Ldind_U2;
            else if (opcode == OpCodes.Ldind_U4) return SR::Emit.OpCodes.Ldind_U4;
            else if (opcode == OpCodes.Ldlen) return SR::Emit.OpCodes.Ldlen;
            else if (opcode == OpCodes.Ldloc) return SR::Emit.OpCodes.Ldloc;
            else if (opcode == OpCodes.Ldloc_0) return SR::Emit.OpCodes.Ldloc_0;
            else if (opcode == OpCodes.Ldloc_1) return SR::Emit.OpCodes.Ldloc_1;
            else if (opcode == OpCodes.Ldloc_2) return SR::Emit.OpCodes.Ldloc_2;
            else if (opcode == OpCodes.Ldloc_3) return SR::Emit.OpCodes.Ldloc_3;
            else if (opcode == OpCodes.Ldloc_S) return SR::Emit.OpCodes.Ldloc_S;
            else if (opcode == OpCodes.Ldloca) return SR::Emit.OpCodes.Ldloca;
            else if (opcode == OpCodes.Ldloca_S) return SR::Emit.OpCodes.Ldloca_S;
            else if (opcode == OpCodes.Ldnull) return SR::Emit.OpCodes.Ldnull;
            else if (opcode == OpCodes.Ldobj) return SR::Emit.OpCodes.Ldobj;
            else if (opcode == OpCodes.Ldsfld) return SR::Emit.OpCodes.Ldsfld;
            else if (opcode == OpCodes.Ldsflda) return SR::Emit.OpCodes.Ldsflda;
            else if (opcode == OpCodes.Ldstr) return SR::Emit.OpCodes.Ldstr;
            else if (opcode == OpCodes.Ldtoken) return SR::Emit.OpCodes.Ldtoken;
            else if (opcode == OpCodes.Ldvirtftn) return SR::Emit.OpCodes.Ldvirtftn;
            else if (opcode == OpCodes.Leave) return SR::Emit.OpCodes.Leave;
            else if (opcode == OpCodes.Leave_S) return SR::Emit.OpCodes.Leave_S;
            else if (opcode == OpCodes.Localloc) return SR::Emit.OpCodes.Localloc;
            else if (opcode == OpCodes.Mkrefany) return SR::Emit.OpCodes.Mkrefany;
            else if (opcode == OpCodes.Mul) return SR::Emit.OpCodes.Mul;
            else if (opcode == OpCodes.Mul_Ovf) return SR::Emit.OpCodes.Mul_Ovf;
            else if (opcode == OpCodes.Mul_Ovf_Un) return SR::Emit.OpCodes.Mul_Ovf_Un;
            else if (opcode == OpCodes.Neg) return SR::Emit.OpCodes.Neg;
            else if (opcode == OpCodes.Newarr) return SR::Emit.OpCodes.Newarr;
            else if (opcode == OpCodes.Newobj) return SR::Emit.OpCodes.Newobj;
            else if (opcode == OpCodes.Nop) return SR::Emit.OpCodes.Nop;
            else if (opcode == OpCodes.Not) return SR::Emit.OpCodes.Not;
            else if (opcode == OpCodes.Or) return SR::Emit.OpCodes.Or;
            else if (opcode == OpCodes.Pop) return SR::Emit.OpCodes.Pop;
            else if (opcode == OpCodes.Readonly) return SR::Emit.OpCodes.Readonly;
            else if (opcode == OpCodes.Refanytype) return SR::Emit.OpCodes.Refanytype;
            else if (opcode == OpCodes.Refanyval) return SR::Emit.OpCodes.Refanyval;
            else if (opcode == OpCodes.Rem) return SR::Emit.OpCodes.Rem;
            else if (opcode == OpCodes.Rem_Un) return SR::Emit.OpCodes.Rem_Un;
            else if (opcode == OpCodes.Ret) return SR::Emit.OpCodes.Ret;
            else if (opcode == OpCodes.Rethrow) return SR::Emit.OpCodes.Rethrow;
            else if (opcode == OpCodes.Shl) return SR::Emit.OpCodes.Shl;
            else if (opcode == OpCodes.Shr) return SR::Emit.OpCodes.Shr;
            else if (opcode == OpCodes.Shr_Un) return SR::Emit.OpCodes.Shr_Un;
            else if (opcode == OpCodes.Sizeof) return SR::Emit.OpCodes.Sizeof;
            else if (opcode == OpCodes.Starg) return SR::Emit.OpCodes.Starg;
            else if (opcode == OpCodes.Starg_S) return SR::Emit.OpCodes.Starg_S;
            else if (opcode == OpCodes.Stelem) return SR::Emit.OpCodes.Stelem;
            else if (opcode == OpCodes.Stelem_I) return SR::Emit.OpCodes.Stelem_I;
            else if (opcode == OpCodes.Stelem_I1) return SR::Emit.OpCodes.Stelem_I1;
            else if (opcode == OpCodes.Stelem_I2) return SR::Emit.OpCodes.Stelem_I2;
            else if (opcode == OpCodes.Stelem_I4) return SR::Emit.OpCodes.Stelem_I4;
            else if (opcode == OpCodes.Stelem_I8) return SR::Emit.OpCodes.Stelem_I8;
            else if (opcode == OpCodes.Stelem_R4) return SR::Emit.OpCodes.Stelem_R4;
            else if (opcode == OpCodes.Stelem_R8) return SR::Emit.OpCodes.Stelem_R8;
            else if (opcode == OpCodes.Stelem_Ref) return SR::Emit.OpCodes.Stelem_Ref;
            else if (opcode == OpCodes.Stfld) return SR::Emit.OpCodes.Stfld;
            else if (opcode == OpCodes.Stind_I) return SR::Emit.OpCodes.Stind_I;
            else if (opcode == OpCodes.Stind_I1) return SR::Emit.OpCodes.Stind_I1;
            else if (opcode == OpCodes.Stind_I2) return SR::Emit.OpCodes.Stind_I2;
            else if (opcode == OpCodes.Stind_I4) return SR::Emit.OpCodes.Stind_I4;
            else if (opcode == OpCodes.Stind_I8) return SR::Emit.OpCodes.Stind_I8;
            else if (opcode == OpCodes.Stind_R4) return SR::Emit.OpCodes.Stind_R4;
            else if (opcode == OpCodes.Stind_R8) return SR::Emit.OpCodes.Stind_R8;
            else if (opcode == OpCodes.Stind_Ref) return SR::Emit.OpCodes.Stind_Ref;
            else if (opcode == OpCodes.Stloc) return SR::Emit.OpCodes.Stloc;
            else if (opcode == OpCodes.Stloc_0) return SR::Emit.OpCodes.Stloc_0;
            else if (opcode == OpCodes.Stloc_1) return SR::Emit.OpCodes.Stloc_1;
            else if (opcode == OpCodes.Stloc_2) return SR::Emit.OpCodes.Stloc_2;
            else if (opcode == OpCodes.Stloc_3) return SR::Emit.OpCodes.Stloc_3;
            else if (opcode == OpCodes.Stloc_S) return SR::Emit.OpCodes.Stloc_S;
            else if (opcode == OpCodes.Stobj) return SR::Emit.OpCodes.Stobj;
            else if (opcode == OpCodes.Stsfld) return SR::Emit.OpCodes.Stsfld;
            else if (opcode == OpCodes.Sub) return SR::Emit.OpCodes.Sub;
            else if (opcode == OpCodes.Sub_Ovf) return SR::Emit.OpCodes.Sub_Ovf;
            else if (opcode == OpCodes.Sub_Ovf_Un) return SR::Emit.OpCodes.Sub_Ovf_Un;
            else if (opcode == OpCodes.Switch) return SR::Emit.OpCodes.Switch;
            else if (opcode == OpCodes.Tailcall) return SR::Emit.OpCodes.Tailcall;
            else if (opcode == OpCodes.Throw) return SR::Emit.OpCodes.Throw;
            else if (opcode == OpCodes.Unaligned) return SR::Emit.OpCodes.Unaligned;
            else if (opcode == OpCodes.Unbox) return SR::Emit.OpCodes.Unbox;
            else if (opcode == OpCodes.Unbox_Any) return SR::Emit.OpCodes.Unbox_Any;
            else if (opcode == OpCodes.Volatile) return SR::Emit.OpCodes.Volatile;
            else if (opcode == OpCodes.Xor) return SR::Emit.OpCodes.Xor;

            throw new NotSupportedException();
        }
    }
}
