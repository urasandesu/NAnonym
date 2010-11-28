/* 
 * File: OpCodeMixin.cs
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
using MCC = Mono.Cecil.Cil;
using UNI = Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil.Cil
{
    public static class OpCodeMixin
    {
        public static UNI::OpCode ToNAnonym(this MCC::OpCode opcode)
        {
            if (opcode == MCC::OpCodes.Add) return UNI::OpCodes.Add;
            else if (opcode == MCC::OpCodes.Add_Ovf) return UNI::OpCodes.Add_Ovf;
            else if (opcode == MCC::OpCodes.Add_Ovf_Un) return UNI::OpCodes.Add_Ovf_Un;
            else if (opcode == MCC::OpCodes.And) return UNI::OpCodes.And;
            else if (opcode == MCC::OpCodes.Arglist) return UNI::OpCodes.Arglist;
            else if (opcode == MCC::OpCodes.Beq) return UNI::OpCodes.Beq;
            else if (opcode == MCC::OpCodes.Beq_S) return UNI::OpCodes.Beq_S;
            else if (opcode == MCC::OpCodes.Bge) return UNI::OpCodes.Bge;
            else if (opcode == MCC::OpCodes.Bge_S) return UNI::OpCodes.Bge_S;
            else if (opcode == MCC::OpCodes.Bge_Un) return UNI::OpCodes.Bge_Un;
            else if (opcode == MCC::OpCodes.Bge_Un_S) return UNI::OpCodes.Bge_Un_S;
            else if (opcode == MCC::OpCodes.Bgt) return UNI::OpCodes.Bgt;
            else if (opcode == MCC::OpCodes.Bgt_S) return UNI::OpCodes.Bgt_S;
            else if (opcode == MCC::OpCodes.Bgt_Un) return UNI::OpCodes.Bgt_Un;
            else if (opcode == MCC::OpCodes.Bgt_Un_S) return UNI::OpCodes.Bgt_Un_S;
            else if (opcode == MCC::OpCodes.Ble) return UNI::OpCodes.Ble;
            else if (opcode == MCC::OpCodes.Ble_S) return UNI::OpCodes.Ble_S;
            else if (opcode == MCC::OpCodes.Ble_Un) return UNI::OpCodes.Ble_Un;
            else if (opcode == MCC::OpCodes.Ble_Un_S) return UNI::OpCodes.Ble_Un_S;
            else if (opcode == MCC::OpCodes.Blt) return UNI::OpCodes.Blt;
            else if (opcode == MCC::OpCodes.Blt_S) return UNI::OpCodes.Blt_S;
            else if (opcode == MCC::OpCodes.Blt_Un) return UNI::OpCodes.Blt_Un;
            else if (opcode == MCC::OpCodes.Blt_Un_S) return UNI::OpCodes.Blt_Un_S;
            else if (opcode == MCC::OpCodes.Bne_Un) return UNI::OpCodes.Bne_Un;
            else if (opcode == MCC::OpCodes.Bne_Un_S) return UNI::OpCodes.Bne_Un_S;
            else if (opcode == MCC::OpCodes.Box) return UNI::OpCodes.Box;
            else if (opcode == MCC::OpCodes.Br) return UNI::OpCodes.Br;
            else if (opcode == MCC::OpCodes.Br_S) return UNI::OpCodes.Br_S;
            else if (opcode == MCC::OpCodes.Break) return UNI::OpCodes.Break;
            else if (opcode == MCC::OpCodes.Brfalse) return UNI::OpCodes.Brfalse;
            else if (opcode == MCC::OpCodes.Brfalse_S) return UNI::OpCodes.Brfalse_S;
            else if (opcode == MCC::OpCodes.Brtrue) return UNI::OpCodes.Brtrue;
            else if (opcode == MCC::OpCodes.Brtrue_S) return UNI::OpCodes.Brtrue_S;
            else if (opcode == MCC::OpCodes.Call) return UNI::OpCodes.Call;
            else if (opcode == MCC::OpCodes.Calli) return UNI::OpCodes.Calli;
            else if (opcode == MCC::OpCodes.Callvirt) return UNI::OpCodes.Callvirt;
            else if (opcode == MCC::OpCodes.Castclass) return UNI::OpCodes.Castclass;
            else if (opcode == MCC::OpCodes.Ceq) return UNI::OpCodes.Ceq;
            else if (opcode == MCC::OpCodes.Cgt) return UNI::OpCodes.Cgt;
            else if (opcode == MCC::OpCodes.Cgt_Un) return UNI::OpCodes.Cgt_Un;
            else if (opcode == MCC::OpCodes.Ckfinite) return UNI::OpCodes.Ckfinite;
            else if (opcode == MCC::OpCodes.Clt) return UNI::OpCodes.Clt;
            else if (opcode == MCC::OpCodes.Clt_Un) return UNI::OpCodes.Clt_Un;
            else if (opcode == MCC::OpCodes.Constrained) return UNI::OpCodes.Constrained;
            else if (opcode == MCC::OpCodes.Conv_I) return UNI::OpCodes.Conv_I;
            else if (opcode == MCC::OpCodes.Conv_I1) return UNI::OpCodes.Conv_I1;
            else if (opcode == MCC::OpCodes.Conv_I2) return UNI::OpCodes.Conv_I2;
            else if (opcode == MCC::OpCodes.Conv_I4) return UNI::OpCodes.Conv_I4;
            else if (opcode == MCC::OpCodes.Conv_I8) return UNI::OpCodes.Conv_I8;
            else if (opcode == MCC::OpCodes.Conv_Ovf_I) return UNI::OpCodes.Conv_Ovf_I;
            else if (opcode == MCC::OpCodes.Conv_Ovf_I_Un) return UNI::OpCodes.Conv_Ovf_I_Un;
            else if (opcode == MCC::OpCodes.Conv_Ovf_I1) return UNI::OpCodes.Conv_Ovf_I1;
            else if (opcode == MCC::OpCodes.Conv_Ovf_I1_Un) return UNI::OpCodes.Conv_Ovf_I1_Un;
            else if (opcode == MCC::OpCodes.Conv_Ovf_I2) return UNI::OpCodes.Conv_Ovf_I2;
            else if (opcode == MCC::OpCodes.Conv_Ovf_I2_Un) return UNI::OpCodes.Conv_Ovf_I2_Un;
            else if (opcode == MCC::OpCodes.Conv_Ovf_I4) return UNI::OpCodes.Conv_Ovf_I4;
            else if (opcode == MCC::OpCodes.Conv_Ovf_I4_Un) return UNI::OpCodes.Conv_Ovf_I4_Un;
            else if (opcode == MCC::OpCodes.Conv_Ovf_I8) return UNI::OpCodes.Conv_Ovf_I8;
            else if (opcode == MCC::OpCodes.Conv_Ovf_I8_Un) return UNI::OpCodes.Conv_Ovf_I8_Un;
            else if (opcode == MCC::OpCodes.Conv_Ovf_U) return UNI::OpCodes.Conv_Ovf_U;
            else if (opcode == MCC::OpCodes.Conv_Ovf_U_Un) return UNI::OpCodes.Conv_Ovf_U_Un;
            else if (opcode == MCC::OpCodes.Conv_Ovf_U1) return UNI::OpCodes.Conv_Ovf_U1;
            else if (opcode == MCC::OpCodes.Conv_Ovf_U1_Un) return UNI::OpCodes.Conv_Ovf_U1_Un;
            else if (opcode == MCC::OpCodes.Conv_Ovf_U2) return UNI::OpCodes.Conv_Ovf_U2;
            else if (opcode == MCC::OpCodes.Conv_Ovf_U2_Un) return UNI::OpCodes.Conv_Ovf_U2_Un;
            else if (opcode == MCC::OpCodes.Conv_Ovf_U4) return UNI::OpCodes.Conv_Ovf_U4;
            else if (opcode == MCC::OpCodes.Conv_Ovf_U4_Un) return UNI::OpCodes.Conv_Ovf_U4_Un;
            else if (opcode == MCC::OpCodes.Conv_Ovf_U8) return UNI::OpCodes.Conv_Ovf_U8;
            else if (opcode == MCC::OpCodes.Conv_Ovf_U8_Un) return UNI::OpCodes.Conv_Ovf_U8_Un;
            else if (opcode == MCC::OpCodes.Conv_R_Un) return UNI::OpCodes.Conv_R_Un;
            else if (opcode == MCC::OpCodes.Conv_R4) return UNI::OpCodes.Conv_R4;
            else if (opcode == MCC::OpCodes.Conv_R8) return UNI::OpCodes.Conv_R8;
            else if (opcode == MCC::OpCodes.Conv_U) return UNI::OpCodes.Conv_U;
            else if (opcode == MCC::OpCodes.Conv_U1) return UNI::OpCodes.Conv_U1;
            else if (opcode == MCC::OpCodes.Conv_U2) return UNI::OpCodes.Conv_U2;
            else if (opcode == MCC::OpCodes.Conv_U4) return UNI::OpCodes.Conv_U4;
            else if (opcode == MCC::OpCodes.Conv_U8) return UNI::OpCodes.Conv_U8;
            else if (opcode == MCC::OpCodes.Cpblk) return UNI::OpCodes.Cpblk;
            else if (opcode == MCC::OpCodes.Cpobj) return UNI::OpCodes.Cpobj;
            else if (opcode == MCC::OpCodes.Div) return UNI::OpCodes.Div;
            else if (opcode == MCC::OpCodes.Div_Un) return UNI::OpCodes.Div_Un;
            else if (opcode == MCC::OpCodes.Dup) return UNI::OpCodes.Dup;
            else if (opcode == MCC::OpCodes.Endfilter) return UNI::OpCodes.Endfilter;
            else if (opcode == MCC::OpCodes.Endfinally) return UNI::OpCodes.Endfinally;
            else if (opcode == MCC::OpCodes.Initblk) return UNI::OpCodes.Initblk;
            else if (opcode == MCC::OpCodes.Initobj) return UNI::OpCodes.Initobj;
            else if (opcode == MCC::OpCodes.Isinst) return UNI::OpCodes.Isinst;
            else if (opcode == MCC::OpCodes.Jmp) return UNI::OpCodes.Jmp;
            else if (opcode == MCC::OpCodes.Ldarg) return UNI::OpCodes.Ldarg;
            else if (opcode == MCC::OpCodes.Ldarg_0) return UNI::OpCodes.Ldarg_0;
            else if (opcode == MCC::OpCodes.Ldarg_1) return UNI::OpCodes.Ldarg_1;
            else if (opcode == MCC::OpCodes.Ldarg_2) return UNI::OpCodes.Ldarg_2;
            else if (opcode == MCC::OpCodes.Ldarg_3) return UNI::OpCodes.Ldarg_3;
            else if (opcode == MCC::OpCodes.Ldarg_S) return UNI::OpCodes.Ldarg_S;
            else if (opcode == MCC::OpCodes.Ldarga) return UNI::OpCodes.Ldarga;
            else if (opcode == MCC::OpCodes.Ldarga_S) return UNI::OpCodes.Ldarga_S;
            else if (opcode == MCC::OpCodes.Ldc_I4) return UNI::OpCodes.Ldc_I4;
            else if (opcode == MCC::OpCodes.Ldc_I4_0) return UNI::OpCodes.Ldc_I4_0;
            else if (opcode == MCC::OpCodes.Ldc_I4_1) return UNI::OpCodes.Ldc_I4_1;
            else if (opcode == MCC::OpCodes.Ldc_I4_2) return UNI::OpCodes.Ldc_I4_2;
            else if (opcode == MCC::OpCodes.Ldc_I4_3) return UNI::OpCodes.Ldc_I4_3;
            else if (opcode == MCC::OpCodes.Ldc_I4_4) return UNI::OpCodes.Ldc_I4_4;
            else if (opcode == MCC::OpCodes.Ldc_I4_5) return UNI::OpCodes.Ldc_I4_5;
            else if (opcode == MCC::OpCodes.Ldc_I4_6) return UNI::OpCodes.Ldc_I4_6;
            else if (opcode == MCC::OpCodes.Ldc_I4_7) return UNI::OpCodes.Ldc_I4_7;
            else if (opcode == MCC::OpCodes.Ldc_I4_8) return UNI::OpCodes.Ldc_I4_8;
            else if (opcode == MCC::OpCodes.Ldc_I4_M1) return UNI::OpCodes.Ldc_I4_M1;
            else if (opcode == MCC::OpCodes.Ldc_I4_S) return UNI::OpCodes.Ldc_I4_S;
            else if (opcode == MCC::OpCodes.Ldc_I8) return UNI::OpCodes.Ldc_I8;
            else if (opcode == MCC::OpCodes.Ldc_R4) return UNI::OpCodes.Ldc_R4;
            else if (opcode == MCC::OpCodes.Ldc_R8) return UNI::OpCodes.Ldc_R8;
            else if (opcode == MCC::OpCodes.Ldelem_Any) return UNI::OpCodes.Ldelem;
            else if (opcode == MCC::OpCodes.Ldelem_I) return UNI::OpCodes.Ldelem_I;
            else if (opcode == MCC::OpCodes.Ldelem_I1) return UNI::OpCodes.Ldelem_I1;
            else if (opcode == MCC::OpCodes.Ldelem_I2) return UNI::OpCodes.Ldelem_I2;
            else if (opcode == MCC::OpCodes.Ldelem_I4) return UNI::OpCodes.Ldelem_I4;
            else if (opcode == MCC::OpCodes.Ldelem_I8) return UNI::OpCodes.Ldelem_I8;
            else if (opcode == MCC::OpCodes.Ldelem_R4) return UNI::OpCodes.Ldelem_R4;
            else if (opcode == MCC::OpCodes.Ldelem_R8) return UNI::OpCodes.Ldelem_R8;
            else if (opcode == MCC::OpCodes.Ldelem_Ref) return UNI::OpCodes.Ldelem_Ref;
            else if (opcode == MCC::OpCodes.Ldelem_U1) return UNI::OpCodes.Ldelem_U1;
            else if (opcode == MCC::OpCodes.Ldelem_U2) return UNI::OpCodes.Ldelem_U2;
            else if (opcode == MCC::OpCodes.Ldelem_U4) return UNI::OpCodes.Ldelem_U4;
            else if (opcode == MCC::OpCodes.Ldelema) return UNI::OpCodes.Ldelema;
            else if (opcode == MCC::OpCodes.Ldfld) return UNI::OpCodes.Ldfld;
            else if (opcode == MCC::OpCodes.Ldflda) return UNI::OpCodes.Ldflda;
            else if (opcode == MCC::OpCodes.Ldftn) return UNI::OpCodes.Ldftn;
            else if (opcode == MCC::OpCodes.Ldind_I) return UNI::OpCodes.Ldind_I;
            else if (opcode == MCC::OpCodes.Ldind_I1) return UNI::OpCodes.Ldind_I1;
            else if (opcode == MCC::OpCodes.Ldind_I2) return UNI::OpCodes.Ldind_I2;
            else if (opcode == MCC::OpCodes.Ldind_I4) return UNI::OpCodes.Ldind_I4;
            else if (opcode == MCC::OpCodes.Ldind_I8) return UNI::OpCodes.Ldind_I8;
            else if (opcode == MCC::OpCodes.Ldind_R4) return UNI::OpCodes.Ldind_R4;
            else if (opcode == MCC::OpCodes.Ldind_R8) return UNI::OpCodes.Ldind_R8;
            else if (opcode == MCC::OpCodes.Ldind_Ref) return UNI::OpCodes.Ldind_Ref;
            else if (opcode == MCC::OpCodes.Ldind_U1) return UNI::OpCodes.Ldind_U1;
            else if (opcode == MCC::OpCodes.Ldind_U2) return UNI::OpCodes.Ldind_U2;
            else if (opcode == MCC::OpCodes.Ldind_U4) return UNI::OpCodes.Ldind_U4;
            else if (opcode == MCC::OpCodes.Ldlen) return UNI::OpCodes.Ldlen;
            else if (opcode == MCC::OpCodes.Ldloc) return UNI::OpCodes.Ldloc;
            else if (opcode == MCC::OpCodes.Ldloc_0) return UNI::OpCodes.Ldloc_0;
            else if (opcode == MCC::OpCodes.Ldloc_1) return UNI::OpCodes.Ldloc_1;
            else if (opcode == MCC::OpCodes.Ldloc_2) return UNI::OpCodes.Ldloc_2;
            else if (opcode == MCC::OpCodes.Ldloc_3) return UNI::OpCodes.Ldloc_3;
            else if (opcode == MCC::OpCodes.Ldloc_S) return UNI::OpCodes.Ldloc_S;
            else if (opcode == MCC::OpCodes.Ldloca) return UNI::OpCodes.Ldloca;
            else if (opcode == MCC::OpCodes.Ldloca_S) return UNI::OpCodes.Ldloca_S;
            else if (opcode == MCC::OpCodes.Ldnull) return UNI::OpCodes.Ldnull;
            else if (opcode == MCC::OpCodes.Ldobj) return UNI::OpCodes.Ldobj;
            else if (opcode == MCC::OpCodes.Ldsfld) return UNI::OpCodes.Ldsfld;
            else if (opcode == MCC::OpCodes.Ldsflda) return UNI::OpCodes.Ldsflda;
            else if (opcode == MCC::OpCodes.Ldstr) return UNI::OpCodes.Ldstr;
            else if (opcode == MCC::OpCodes.Ldtoken) return UNI::OpCodes.Ldtoken;
            else if (opcode == MCC::OpCodes.Ldvirtftn) return UNI::OpCodes.Ldvirtftn;
            else if (opcode == MCC::OpCodes.Leave) return UNI::OpCodes.Leave;
            else if (opcode == MCC::OpCodes.Leave_S) return UNI::OpCodes.Leave_S;
            else if (opcode == MCC::OpCodes.Localloc) return UNI::OpCodes.Localloc;
            else if (opcode == MCC::OpCodes.Mkrefany) return UNI::OpCodes.Mkrefany;
            else if (opcode == MCC::OpCodes.Mul) return UNI::OpCodes.Mul;
            else if (opcode == MCC::OpCodes.Mul_Ovf) return UNI::OpCodes.Mul_Ovf;
            else if (opcode == MCC::OpCodes.Mul_Ovf_Un) return UNI::OpCodes.Mul_Ovf_Un;
            else if (opcode == MCC::OpCodes.Neg) return UNI::OpCodes.Neg;
            else if (opcode == MCC::OpCodes.Newarr) return UNI::OpCodes.Newarr;
            else if (opcode == MCC::OpCodes.Newobj) return UNI::OpCodes.Newobj;
            else if (opcode == MCC::OpCodes.Nop) return UNI::OpCodes.Nop;
            else if (opcode == MCC::OpCodes.Not) return UNI::OpCodes.Not;
            else if (opcode == MCC::OpCodes.Or) return UNI::OpCodes.Or;
            else if (opcode == MCC::OpCodes.Pop) return UNI::OpCodes.Pop;
            else if (opcode == MCC::OpCodes.Readonly) return UNI::OpCodes.Readonly;
            else if (opcode == MCC::OpCodes.Refanytype) return UNI::OpCodes.Refanytype;
            else if (opcode == MCC::OpCodes.Refanyval) return UNI::OpCodes.Refanyval;
            else if (opcode == MCC::OpCodes.Rem) return UNI::OpCodes.Rem;
            else if (opcode == MCC::OpCodes.Rem_Un) return UNI::OpCodes.Rem_Un;
            else if (opcode == MCC::OpCodes.Ret) return UNI::OpCodes.Ret;
            else if (opcode == MCC::OpCodes.Rethrow) return UNI::OpCodes.Rethrow;
            else if (opcode == MCC::OpCodes.Shl) return UNI::OpCodes.Shl;
            else if (opcode == MCC::OpCodes.Shr) return UNI::OpCodes.Shr;
            else if (opcode == MCC::OpCodes.Shr_Un) return UNI::OpCodes.Shr_Un;
            else if (opcode == MCC::OpCodes.Sizeof) return UNI::OpCodes.Sizeof;
            else if (opcode == MCC::OpCodes.Starg) return UNI::OpCodes.Starg;
            else if (opcode == MCC::OpCodes.Starg_S) return UNI::OpCodes.Starg_S;
            else if (opcode == MCC::OpCodes.Stelem_Any) return UNI::OpCodes.Stelem;
            else if (opcode == MCC::OpCodes.Stelem_I) return UNI::OpCodes.Stelem_I;
            else if (opcode == MCC::OpCodes.Stelem_I1) return UNI::OpCodes.Stelem_I1;
            else if (opcode == MCC::OpCodes.Stelem_I2) return UNI::OpCodes.Stelem_I2;
            else if (opcode == MCC::OpCodes.Stelem_I4) return UNI::OpCodes.Stelem_I4;
            else if (opcode == MCC::OpCodes.Stelem_I8) return UNI::OpCodes.Stelem_I8;
            else if (opcode == MCC::OpCodes.Stelem_R4) return UNI::OpCodes.Stelem_R4;
            else if (opcode == MCC::OpCodes.Stelem_R8) return UNI::OpCodes.Stelem_R8;
            else if (opcode == MCC::OpCodes.Stelem_Ref) return UNI::OpCodes.Stelem_Ref;
            else if (opcode == MCC::OpCodes.Stfld) return UNI::OpCodes.Stfld;
            else if (opcode == MCC::OpCodes.Stind_I) return UNI::OpCodes.Stind_I;
            else if (opcode == MCC::OpCodes.Stind_I1) return UNI::OpCodes.Stind_I1;
            else if (opcode == MCC::OpCodes.Stind_I2) return UNI::OpCodes.Stind_I2;
            else if (opcode == MCC::OpCodes.Stind_I4) return UNI::OpCodes.Stind_I4;
            else if (opcode == MCC::OpCodes.Stind_I8) return UNI::OpCodes.Stind_I8;
            else if (opcode == MCC::OpCodes.Stind_R4) return UNI::OpCodes.Stind_R4;
            else if (opcode == MCC::OpCodes.Stind_R8) return UNI::OpCodes.Stind_R8;
            else if (opcode == MCC::OpCodes.Stind_Ref) return UNI::OpCodes.Stind_Ref;
            else if (opcode == MCC::OpCodes.Stloc) return UNI::OpCodes.Stloc;
            else if (opcode == MCC::OpCodes.Stloc_0) return UNI::OpCodes.Stloc_0;
            else if (opcode == MCC::OpCodes.Stloc_1) return UNI::OpCodes.Stloc_1;
            else if (opcode == MCC::OpCodes.Stloc_2) return UNI::OpCodes.Stloc_2;
            else if (opcode == MCC::OpCodes.Stloc_3) return UNI::OpCodes.Stloc_3;
            else if (opcode == MCC::OpCodes.Stloc_S) return UNI::OpCodes.Stloc_S;
            else if (opcode == MCC::OpCodes.Stobj) return UNI::OpCodes.Stobj;
            else if (opcode == MCC::OpCodes.Stsfld) return UNI::OpCodes.Stsfld;
            else if (opcode == MCC::OpCodes.Sub) return UNI::OpCodes.Sub;
            else if (opcode == MCC::OpCodes.Sub_Ovf) return UNI::OpCodes.Sub_Ovf;
            else if (opcode == MCC::OpCodes.Sub_Ovf_Un) return UNI::OpCodes.Sub_Ovf_Un;
            else if (opcode == MCC::OpCodes.Switch) return UNI::OpCodes.Switch;
            else if (opcode == MCC::OpCodes.Tail) return UNI::OpCodes.Tailcall;
            else if (opcode == MCC::OpCodes.Throw) return UNI::OpCodes.Throw;
            else if (opcode == MCC::OpCodes.Unaligned) return UNI::OpCodes.Unaligned;
            else if (opcode == MCC::OpCodes.Unbox) return UNI::OpCodes.Unbox;
            else if (opcode == MCC::OpCodes.Unbox_Any) return UNI::OpCodes.Unbox_Any;
            else if (opcode == MCC::OpCodes.Volatile) return UNI::OpCodes.Volatile;
            else if (opcode == MCC::OpCodes.Xor) return UNI::OpCodes.Xor;

            throw new NotSupportedException();
        }
    }
}

