using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.Linq;
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

        public static bool IsStatic(this PropertyInfo source)
        {
            return (source.CanRead && source.GetGetMethod().IsStatic) || (source.CanWrite && source.GetSetMethod().IsStatic);
        }

        public static void ExpressBody(this ConstructorBuilder constructorBuilder, Action<ExpressiveMethodBodyGenerator> expression, params ParameterBuilder[] parameterBuilders)
        {
            var gen = new ExpressiveMethodBodyGenerator(new SRConstructorGeneratorImpl(constructorBuilder, parameterBuilders));
            ExpressBodyEnd(gen, expression);
        }

        //public static void ExpressBody(this MethodBuilder methodBuilder, Action<ExpressiveMethodBodyGenerator> expression)
        //{
        //    var gen = new ExpressiveMethodBodyGenerator(new SRMethodGeneratorImpl(methodBuilder));
        //    ExpressBodyEnd(gen, expression);
        //}

        public static void ExpressBody(this MethodBuilder methodBuilder, Action<ExpressiveMethodBodyGenerator> expression, params ParameterBuilder[] parameterBuilders)
        {
            var gen = new ExpressiveMethodBodyGenerator(new SRMethodGeneratorImpl(methodBuilder, parameterBuilders));
            ExpressBodyEnd(gen, expression);
        }

        public static void ExpressBody(this DynamicMethod dynamicMethod, Action<ExpressiveMethodBodyGenerator> expression, params ParameterBuilder[] parameterBuilders)
        {
            var gen = new ExpressiveMethodBodyGenerator(new SRMethodGeneratorImpl(dynamicMethod, parameterBuilders));
            ExpressBodyEnd(gen, expression);
        }

        internal static void ExpressBodyEnd(ExpressiveMethodBodyGenerator methodBodyGen, Action<ExpressiveMethodBodyGenerator> expression)
        {
            expression(methodBodyGen);
            if (methodBodyGen.Directives.Last().OpCode != OpCodes.Ret)
            {
                methodBodyGen.Eval(_ => _.End());
            }
        }

        // TODO: 変換先型を明示すること（explicit operator ではないため、戻り値によるオーバーロードができない）
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

        // TODO: 変換先型を明示すること（explicit operator ではないため、戻り値によるオーバーロードができない）
        public static OpCode Cast(this SR::Emit.OpCode opcode)
        {
            if (opcode == SR::Emit.OpCodes.Add) return OpCodes.Add;
            else if (opcode == SR::Emit.OpCodes.Add_Ovf) return OpCodes.Add_Ovf;
            else if (opcode == SR::Emit.OpCodes.Add_Ovf_Un) return OpCodes.Add_Ovf_Un;
            else if (opcode == SR::Emit.OpCodes.And) return OpCodes.And;
            else if (opcode == SR::Emit.OpCodes.Arglist) return OpCodes.Arglist;
            else if (opcode == SR::Emit.OpCodes.Beq) return OpCodes.Beq;
            else if (opcode == SR::Emit.OpCodes.Beq_S) return OpCodes.Beq_S;
            else if (opcode == SR::Emit.OpCodes.Bge) return OpCodes.Bge;
            else if (opcode == SR::Emit.OpCodes.Bge_S) return OpCodes.Bge_S;
            else if (opcode == SR::Emit.OpCodes.Bge_Un) return OpCodes.Bge_Un;
            else if (opcode == SR::Emit.OpCodes.Bge_Un_S) return OpCodes.Bge_Un_S;
            else if (opcode == SR::Emit.OpCodes.Bgt) return OpCodes.Bgt;
            else if (opcode == SR::Emit.OpCodes.Bgt_S) return OpCodes.Bgt_S;
            else if (opcode == SR::Emit.OpCodes.Bgt_Un) return OpCodes.Bgt_Un;
            else if (opcode == SR::Emit.OpCodes.Bgt_Un_S) return OpCodes.Bgt_Un_S;
            else if (opcode == SR::Emit.OpCodes.Ble) return OpCodes.Ble;
            else if (opcode == SR::Emit.OpCodes.Ble_S) return OpCodes.Ble_S;
            else if (opcode == SR::Emit.OpCodes.Ble_Un) return OpCodes.Ble_Un;
            else if (opcode == SR::Emit.OpCodes.Ble_Un_S) return OpCodes.Ble_Un_S;
            else if (opcode == SR::Emit.OpCodes.Blt) return OpCodes.Blt;
            else if (opcode == SR::Emit.OpCodes.Blt_S) return OpCodes.Blt_S;
            else if (opcode == SR::Emit.OpCodes.Blt_Un) return OpCodes.Blt_Un;
            else if (opcode == SR::Emit.OpCodes.Blt_Un_S) return OpCodes.Blt_Un_S;
            else if (opcode == SR::Emit.OpCodes.Bne_Un) return OpCodes.Bne_Un;
            else if (opcode == SR::Emit.OpCodes.Bne_Un_S) return OpCodes.Bne_Un_S;
            else if (opcode == SR::Emit.OpCodes.Box) return OpCodes.Box;
            else if (opcode == SR::Emit.OpCodes.Br) return OpCodes.Br;
            else if (opcode == SR::Emit.OpCodes.Br_S) return OpCodes.Br_S;
            else if (opcode == SR::Emit.OpCodes.Break) return OpCodes.Break;
            else if (opcode == SR::Emit.OpCodes.Brfalse) return OpCodes.Brfalse;
            else if (opcode == SR::Emit.OpCodes.Brfalse_S) return OpCodes.Brfalse_S;
            else if (opcode == SR::Emit.OpCodes.Brtrue) return OpCodes.Brtrue;
            else if (opcode == SR::Emit.OpCodes.Brtrue_S) return OpCodes.Brtrue_S;
            else if (opcode == SR::Emit.OpCodes.Call) return OpCodes.Call;
            else if (opcode == SR::Emit.OpCodes.Calli) return OpCodes.Calli;
            else if (opcode == SR::Emit.OpCodes.Callvirt) return OpCodes.Callvirt;
            else if (opcode == SR::Emit.OpCodes.Castclass) return OpCodes.Castclass;
            else if (opcode == SR::Emit.OpCodes.Ceq) return OpCodes.Ceq;
            else if (opcode == SR::Emit.OpCodes.Cgt) return OpCodes.Cgt;
            else if (opcode == SR::Emit.OpCodes.Cgt_Un) return OpCodes.Cgt_Un;
            else if (opcode == SR::Emit.OpCodes.Ckfinite) return OpCodes.Ckfinite;
            else if (opcode == SR::Emit.OpCodes.Clt) return OpCodes.Clt;
            else if (opcode == SR::Emit.OpCodes.Clt_Un) return OpCodes.Clt_Un;
            else if (opcode == SR::Emit.OpCodes.Constrained) return OpCodes.Constrained;
            else if (opcode == SR::Emit.OpCodes.Conv_I) return OpCodes.Conv_I;
            else if (opcode == SR::Emit.OpCodes.Conv_I1) return OpCodes.Conv_I1;
            else if (opcode == SR::Emit.OpCodes.Conv_I2) return OpCodes.Conv_I2;
            else if (opcode == SR::Emit.OpCodes.Conv_I4) return OpCodes.Conv_I4;
            else if (opcode == SR::Emit.OpCodes.Conv_I8) return OpCodes.Conv_I8;
            else if (opcode == SR::Emit.OpCodes.Conv_Ovf_I) return OpCodes.Conv_Ovf_I;
            else if (opcode == SR::Emit.OpCodes.Conv_Ovf_I_Un) return OpCodes.Conv_Ovf_I_Un;
            else if (opcode == SR::Emit.OpCodes.Conv_Ovf_I1) return OpCodes.Conv_Ovf_I1;
            else if (opcode == SR::Emit.OpCodes.Conv_Ovf_I1_Un) return OpCodes.Conv_Ovf_I1_Un;
            else if (opcode == SR::Emit.OpCodes.Conv_Ovf_I2) return OpCodes.Conv_Ovf_I2;
            else if (opcode == SR::Emit.OpCodes.Conv_Ovf_I2_Un) return OpCodes.Conv_Ovf_I2_Un;
            else if (opcode == SR::Emit.OpCodes.Conv_Ovf_I4) return OpCodes.Conv_Ovf_I4;
            else if (opcode == SR::Emit.OpCodes.Conv_Ovf_I4_Un) return OpCodes.Conv_Ovf_I4_Un;
            else if (opcode == SR::Emit.OpCodes.Conv_Ovf_I8) return OpCodes.Conv_Ovf_I8;
            else if (opcode == SR::Emit.OpCodes.Conv_Ovf_I8_Un) return OpCodes.Conv_Ovf_I8_Un;
            else if (opcode == SR::Emit.OpCodes.Conv_Ovf_U) return OpCodes.Conv_Ovf_U;
            else if (opcode == SR::Emit.OpCodes.Conv_Ovf_U_Un) return OpCodes.Conv_Ovf_U_Un;
            else if (opcode == SR::Emit.OpCodes.Conv_Ovf_U1) return OpCodes.Conv_Ovf_U1;
            else if (opcode == SR::Emit.OpCodes.Conv_Ovf_U1_Un) return OpCodes.Conv_Ovf_U1_Un;
            else if (opcode == SR::Emit.OpCodes.Conv_Ovf_U2) return OpCodes.Conv_Ovf_U2;
            else if (opcode == SR::Emit.OpCodes.Conv_Ovf_U2_Un) return OpCodes.Conv_Ovf_U2_Un;
            else if (opcode == SR::Emit.OpCodes.Conv_Ovf_U4) return OpCodes.Conv_Ovf_U4;
            else if (opcode == SR::Emit.OpCodes.Conv_Ovf_U4_Un) return OpCodes.Conv_Ovf_U4_Un;
            else if (opcode == SR::Emit.OpCodes.Conv_Ovf_U8) return OpCodes.Conv_Ovf_U8;
            else if (opcode == SR::Emit.OpCodes.Conv_Ovf_U8_Un) return OpCodes.Conv_Ovf_U8_Un;
            else if (opcode == SR::Emit.OpCodes.Conv_R_Un) return OpCodes.Conv_R_Un;
            else if (opcode == SR::Emit.OpCodes.Conv_R4) return OpCodes.Conv_R4;
            else if (opcode == SR::Emit.OpCodes.Conv_R8) return OpCodes.Conv_R8;
            else if (opcode == SR::Emit.OpCodes.Conv_U) return OpCodes.Conv_U;
            else if (opcode == SR::Emit.OpCodes.Conv_U1) return OpCodes.Conv_U1;
            else if (opcode == SR::Emit.OpCodes.Conv_U2) return OpCodes.Conv_U2;
            else if (opcode == SR::Emit.OpCodes.Conv_U4) return OpCodes.Conv_U4;
            else if (opcode == SR::Emit.OpCodes.Conv_U8) return OpCodes.Conv_U8;
            else if (opcode == SR::Emit.OpCodes.Cpblk) return OpCodes.Cpblk;
            else if (opcode == SR::Emit.OpCodes.Cpobj) return OpCodes.Cpobj;
            else if (opcode == SR::Emit.OpCodes.Div) return OpCodes.Div;
            else if (opcode == SR::Emit.OpCodes.Div_Un) return OpCodes.Div_Un;
            else if (opcode == SR::Emit.OpCodes.Dup) return OpCodes.Dup;
            else if (opcode == SR::Emit.OpCodes.Endfilter) return OpCodes.Endfilter;
            else if (opcode == SR::Emit.OpCodes.Endfinally) return OpCodes.Endfinally;
            else if (opcode == SR::Emit.OpCodes.Initblk) return OpCodes.Initblk;
            else if (opcode == SR::Emit.OpCodes.Initobj) return OpCodes.Initobj;
            else if (opcode == SR::Emit.OpCodes.Isinst) return OpCodes.Isinst;
            else if (opcode == SR::Emit.OpCodes.Jmp) return OpCodes.Jmp;
            else if (opcode == SR::Emit.OpCodes.Ldarg) return OpCodes.Ldarg;
            else if (opcode == SR::Emit.OpCodes.Ldarg_0) return OpCodes.Ldarg_0;
            else if (opcode == SR::Emit.OpCodes.Ldarg_1) return OpCodes.Ldarg_1;
            else if (opcode == SR::Emit.OpCodes.Ldarg_2) return OpCodes.Ldarg_2;
            else if (opcode == SR::Emit.OpCodes.Ldarg_3) return OpCodes.Ldarg_3;
            else if (opcode == SR::Emit.OpCodes.Ldarg_S) return OpCodes.Ldarg_S;
            else if (opcode == SR::Emit.OpCodes.Ldarga) return OpCodes.Ldarga;
            else if (opcode == SR::Emit.OpCodes.Ldarga_S) return OpCodes.Ldarga_S;
            else if (opcode == SR::Emit.OpCodes.Ldc_I4) return OpCodes.Ldc_I4;
            else if (opcode == SR::Emit.OpCodes.Ldc_I4_0) return OpCodes.Ldc_I4_0;
            else if (opcode == SR::Emit.OpCodes.Ldc_I4_1) return OpCodes.Ldc_I4_1;
            else if (opcode == SR::Emit.OpCodes.Ldc_I4_2) return OpCodes.Ldc_I4_2;
            else if (opcode == SR::Emit.OpCodes.Ldc_I4_3) return OpCodes.Ldc_I4_3;
            else if (opcode == SR::Emit.OpCodes.Ldc_I4_4) return OpCodes.Ldc_I4_4;
            else if (opcode == SR::Emit.OpCodes.Ldc_I4_5) return OpCodes.Ldc_I4_5;
            else if (opcode == SR::Emit.OpCodes.Ldc_I4_6) return OpCodes.Ldc_I4_6;
            else if (opcode == SR::Emit.OpCodes.Ldc_I4_7) return OpCodes.Ldc_I4_7;
            else if (opcode == SR::Emit.OpCodes.Ldc_I4_8) return OpCodes.Ldc_I4_8;
            else if (opcode == SR::Emit.OpCodes.Ldc_I4_M1) return OpCodes.Ldc_I4_M1;
            else if (opcode == SR::Emit.OpCodes.Ldc_I4_S) return OpCodes.Ldc_I4_S;
            else if (opcode == SR::Emit.OpCodes.Ldc_I8) return OpCodes.Ldc_I8;
            else if (opcode == SR::Emit.OpCodes.Ldc_R4) return OpCodes.Ldc_R4;
            else if (opcode == SR::Emit.OpCodes.Ldc_R8) return OpCodes.Ldc_R8;
            else if (opcode == SR::Emit.OpCodes.Ldelem) return OpCodes.Ldelem;
            else if (opcode == SR::Emit.OpCodes.Ldelem_I) return OpCodes.Ldelem_I;
            else if (opcode == SR::Emit.OpCodes.Ldelem_I1) return OpCodes.Ldelem_I1;
            else if (opcode == SR::Emit.OpCodes.Ldelem_I2) return OpCodes.Ldelem_I2;
            else if (opcode == SR::Emit.OpCodes.Ldelem_I4) return OpCodes.Ldelem_I4;
            else if (opcode == SR::Emit.OpCodes.Ldelem_I8) return OpCodes.Ldelem_I8;
            else if (opcode == SR::Emit.OpCodes.Ldelem_R4) return OpCodes.Ldelem_R4;
            else if (opcode == SR::Emit.OpCodes.Ldelem_R8) return OpCodes.Ldelem_R8;
            else if (opcode == SR::Emit.OpCodes.Ldelem_Ref) return OpCodes.Ldelem_Ref;
            else if (opcode == SR::Emit.OpCodes.Ldelem_U1) return OpCodes.Ldelem_U1;
            else if (opcode == SR::Emit.OpCodes.Ldelem_U2) return OpCodes.Ldelem_U2;
            else if (opcode == SR::Emit.OpCodes.Ldelem_U4) return OpCodes.Ldelem_U4;
            else if (opcode == SR::Emit.OpCodes.Ldelema) return OpCodes.Ldelema;
            else if (opcode == SR::Emit.OpCodes.Ldfld) return OpCodes.Ldfld;
            else if (opcode == SR::Emit.OpCodes.Ldflda) return OpCodes.Ldflda;
            else if (opcode == SR::Emit.OpCodes.Ldftn) return OpCodes.Ldftn;
            else if (opcode == SR::Emit.OpCodes.Ldind_I) return OpCodes.Ldind_I;
            else if (opcode == SR::Emit.OpCodes.Ldind_I1) return OpCodes.Ldind_I1;
            else if (opcode == SR::Emit.OpCodes.Ldind_I2) return OpCodes.Ldind_I2;
            else if (opcode == SR::Emit.OpCodes.Ldind_I4) return OpCodes.Ldind_I4;
            else if (opcode == SR::Emit.OpCodes.Ldind_I8) return OpCodes.Ldind_I8;
            else if (opcode == SR::Emit.OpCodes.Ldind_R4) return OpCodes.Ldind_R4;
            else if (opcode == SR::Emit.OpCodes.Ldind_R8) return OpCodes.Ldind_R8;
            else if (opcode == SR::Emit.OpCodes.Ldind_Ref) return OpCodes.Ldind_Ref;
            else if (opcode == SR::Emit.OpCodes.Ldind_U1) return OpCodes.Ldind_U1;
            else if (opcode == SR::Emit.OpCodes.Ldind_U2) return OpCodes.Ldind_U2;
            else if (opcode == SR::Emit.OpCodes.Ldind_U4) return OpCodes.Ldind_U4;
            else if (opcode == SR::Emit.OpCodes.Ldlen) return OpCodes.Ldlen;
            else if (opcode == SR::Emit.OpCodes.Ldloc) return OpCodes.Ldloc;
            else if (opcode == SR::Emit.OpCodes.Ldloc_0) return OpCodes.Ldloc_0;
            else if (opcode == SR::Emit.OpCodes.Ldloc_1) return OpCodes.Ldloc_1;
            else if (opcode == SR::Emit.OpCodes.Ldloc_2) return OpCodes.Ldloc_2;
            else if (opcode == SR::Emit.OpCodes.Ldloc_3) return OpCodes.Ldloc_3;
            else if (opcode == SR::Emit.OpCodes.Ldloc_S) return OpCodes.Ldloc_S;
            else if (opcode == SR::Emit.OpCodes.Ldloca) return OpCodes.Ldloca;
            else if (opcode == SR::Emit.OpCodes.Ldloca_S) return OpCodes.Ldloca_S;
            else if (opcode == SR::Emit.OpCodes.Ldnull) return OpCodes.Ldnull;
            else if (opcode == SR::Emit.OpCodes.Ldobj) return OpCodes.Ldobj;
            else if (opcode == SR::Emit.OpCodes.Ldsfld) return OpCodes.Ldsfld;
            else if (opcode == SR::Emit.OpCodes.Ldsflda) return OpCodes.Ldsflda;
            else if (opcode == SR::Emit.OpCodes.Ldstr) return OpCodes.Ldstr;
            else if (opcode == SR::Emit.OpCodes.Ldtoken) return OpCodes.Ldtoken;
            else if (opcode == SR::Emit.OpCodes.Ldvirtftn) return OpCodes.Ldvirtftn;
            else if (opcode == SR::Emit.OpCodes.Leave) return OpCodes.Leave;
            else if (opcode == SR::Emit.OpCodes.Leave_S) return OpCodes.Leave_S;
            else if (opcode == SR::Emit.OpCodes.Localloc) return OpCodes.Localloc;
            else if (opcode == SR::Emit.OpCodes.Mkrefany) return OpCodes.Mkrefany;
            else if (opcode == SR::Emit.OpCodes.Mul) return OpCodes.Mul;
            else if (opcode == SR::Emit.OpCodes.Mul_Ovf) return OpCodes.Mul_Ovf;
            else if (opcode == SR::Emit.OpCodes.Mul_Ovf_Un) return OpCodes.Mul_Ovf_Un;
            else if (opcode == SR::Emit.OpCodes.Neg) return OpCodes.Neg;
            else if (opcode == SR::Emit.OpCodes.Newarr) return OpCodes.Newarr;
            else if (opcode == SR::Emit.OpCodes.Newobj) return OpCodes.Newobj;
            else if (opcode == SR::Emit.OpCodes.Nop) return OpCodes.Nop;
            else if (opcode == SR::Emit.OpCodes.Not) return OpCodes.Not;
            else if (opcode == SR::Emit.OpCodes.Or) return OpCodes.Or;
            else if (opcode == SR::Emit.OpCodes.Pop) return OpCodes.Pop;
            else if (opcode == SR::Emit.OpCodes.Readonly) return OpCodes.Readonly;
            else if (opcode == SR::Emit.OpCodes.Refanytype) return OpCodes.Refanytype;
            else if (opcode == SR::Emit.OpCodes.Refanyval) return OpCodes.Refanyval;
            else if (opcode == SR::Emit.OpCodes.Rem) return OpCodes.Rem;
            else if (opcode == SR::Emit.OpCodes.Rem_Un) return OpCodes.Rem_Un;
            else if (opcode == SR::Emit.OpCodes.Ret) return OpCodes.Ret;
            else if (opcode == SR::Emit.OpCodes.Rethrow) return OpCodes.Rethrow;
            else if (opcode == SR::Emit.OpCodes.Shl) return OpCodes.Shl;
            else if (opcode == SR::Emit.OpCodes.Shr) return OpCodes.Shr;
            else if (opcode == SR::Emit.OpCodes.Shr_Un) return OpCodes.Shr_Un;
            else if (opcode == SR::Emit.OpCodes.Sizeof) return OpCodes.Sizeof;
            else if (opcode == SR::Emit.OpCodes.Starg) return OpCodes.Starg;
            else if (opcode == SR::Emit.OpCodes.Starg_S) return OpCodes.Starg_S;
            else if (opcode == SR::Emit.OpCodes.Stelem) return OpCodes.Stelem;
            else if (opcode == SR::Emit.OpCodes.Stelem_I) return OpCodes.Stelem_I;
            else if (opcode == SR::Emit.OpCodes.Stelem_I1) return OpCodes.Stelem_I1;
            else if (opcode == SR::Emit.OpCodes.Stelem_I2) return OpCodes.Stelem_I2;
            else if (opcode == SR::Emit.OpCodes.Stelem_I4) return OpCodes.Stelem_I4;
            else if (opcode == SR::Emit.OpCodes.Stelem_I8) return OpCodes.Stelem_I8;
            else if (opcode == SR::Emit.OpCodes.Stelem_R4) return OpCodes.Stelem_R4;
            else if (opcode == SR::Emit.OpCodes.Stelem_R8) return OpCodes.Stelem_R8;
            else if (opcode == SR::Emit.OpCodes.Stelem_Ref) return OpCodes.Stelem_Ref;
            else if (opcode == SR::Emit.OpCodes.Stfld) return OpCodes.Stfld;
            else if (opcode == SR::Emit.OpCodes.Stind_I) return OpCodes.Stind_I;
            else if (opcode == SR::Emit.OpCodes.Stind_I1) return OpCodes.Stind_I1;
            else if (opcode == SR::Emit.OpCodes.Stind_I2) return OpCodes.Stind_I2;
            else if (opcode == SR::Emit.OpCodes.Stind_I4) return OpCodes.Stind_I4;
            else if (opcode == SR::Emit.OpCodes.Stind_I8) return OpCodes.Stind_I8;
            else if (opcode == SR::Emit.OpCodes.Stind_R4) return OpCodes.Stind_R4;
            else if (opcode == SR::Emit.OpCodes.Stind_R8) return OpCodes.Stind_R8;
            else if (opcode == SR::Emit.OpCodes.Stind_Ref) return OpCodes.Stind_Ref;
            else if (opcode == SR::Emit.OpCodes.Stloc) return OpCodes.Stloc;
            else if (opcode == SR::Emit.OpCodes.Stloc_0) return OpCodes.Stloc_0;
            else if (opcode == SR::Emit.OpCodes.Stloc_1) return OpCodes.Stloc_1;
            else if (opcode == SR::Emit.OpCodes.Stloc_2) return OpCodes.Stloc_2;
            else if (opcode == SR::Emit.OpCodes.Stloc_3) return OpCodes.Stloc_3;
            else if (opcode == SR::Emit.OpCodes.Stloc_S) return OpCodes.Stloc_S;
            else if (opcode == SR::Emit.OpCodes.Stobj) return OpCodes.Stobj;
            else if (opcode == SR::Emit.OpCodes.Stsfld) return OpCodes.Stsfld;
            else if (opcode == SR::Emit.OpCodes.Sub) return OpCodes.Sub;
            else if (opcode == SR::Emit.OpCodes.Sub_Ovf) return OpCodes.Sub_Ovf;
            else if (opcode == SR::Emit.OpCodes.Sub_Ovf_Un) return OpCodes.Sub_Ovf_Un;
            else if (opcode == SR::Emit.OpCodes.Switch) return OpCodes.Switch;
            else if (opcode == SR::Emit.OpCodes.Tailcall) return OpCodes.Tailcall;
            else if (opcode == SR::Emit.OpCodes.Throw) return OpCodes.Throw;
            else if (opcode == SR::Emit.OpCodes.Unaligned) return OpCodes.Unaligned;
            else if (opcode == SR::Emit.OpCodes.Unbox) return OpCodes.Unbox;
            else if (opcode == SR::Emit.OpCodes.Unbox_Any) return OpCodes.Unbox_Any;
            else if (opcode == SR::Emit.OpCodes.Volatile) return OpCodes.Volatile;
            else if (opcode == SR::Emit.OpCodes.Xor) return OpCodes.Xor;

            throw new NotSupportedException();
        }
    }
}
