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
using SRE = System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Mixins.System.Reflection.Emit
{
    public static class OpCodeMixin
    {
        public static OpCode ToNAnonym(this SRE::OpCode opcode)
        {
            if (opcode == SRE::OpCodes.Add) return OpCodes.Add;
            else if (opcode == SRE::OpCodes.Add_Ovf) return OpCodes.Add_Ovf;
            else if (opcode == SRE::OpCodes.Add_Ovf_Un) return OpCodes.Add_Ovf_Un;
            else if (opcode == SRE::OpCodes.And) return OpCodes.And;
            else if (opcode == SRE::OpCodes.Arglist) return OpCodes.Arglist;
            else if (opcode == SRE::OpCodes.Beq) return OpCodes.Beq;
            else if (opcode == SRE::OpCodes.Beq_S) return OpCodes.Beq_S;
            else if (opcode == SRE::OpCodes.Bge) return OpCodes.Bge;
            else if (opcode == SRE::OpCodes.Bge_S) return OpCodes.Bge_S;
            else if (opcode == SRE::OpCodes.Bge_Un) return OpCodes.Bge_Un;
            else if (opcode == SRE::OpCodes.Bge_Un_S) return OpCodes.Bge_Un_S;
            else if (opcode == SRE::OpCodes.Bgt) return OpCodes.Bgt;
            else if (opcode == SRE::OpCodes.Bgt_S) return OpCodes.Bgt_S;
            else if (opcode == SRE::OpCodes.Bgt_Un) return OpCodes.Bgt_Un;
            else if (opcode == SRE::OpCodes.Bgt_Un_S) return OpCodes.Bgt_Un_S;
            else if (opcode == SRE::OpCodes.Ble) return OpCodes.Ble;
            else if (opcode == SRE::OpCodes.Ble_S) return OpCodes.Ble_S;
            else if (opcode == SRE::OpCodes.Ble_Un) return OpCodes.Ble_Un;
            else if (opcode == SRE::OpCodes.Ble_Un_S) return OpCodes.Ble_Un_S;
            else if (opcode == SRE::OpCodes.Blt) return OpCodes.Blt;
            else if (opcode == SRE::OpCodes.Blt_S) return OpCodes.Blt_S;
            else if (opcode == SRE::OpCodes.Blt_Un) return OpCodes.Blt_Un;
            else if (opcode == SRE::OpCodes.Blt_Un_S) return OpCodes.Blt_Un_S;
            else if (opcode == SRE::OpCodes.Bne_Un) return OpCodes.Bne_Un;
            else if (opcode == SRE::OpCodes.Bne_Un_S) return OpCodes.Bne_Un_S;
            else if (opcode == SRE::OpCodes.Box) return OpCodes.Box;
            else if (opcode == SRE::OpCodes.Br) return OpCodes.Br;
            else if (opcode == SRE::OpCodes.Br_S) return OpCodes.Br_S;
            else if (opcode == SRE::OpCodes.Break) return OpCodes.Break;
            else if (opcode == SRE::OpCodes.Brfalse) return OpCodes.Brfalse;
            else if (opcode == SRE::OpCodes.Brfalse_S) return OpCodes.Brfalse_S;
            else if (opcode == SRE::OpCodes.Brtrue) return OpCodes.Brtrue;
            else if (opcode == SRE::OpCodes.Brtrue_S) return OpCodes.Brtrue_S;
            else if (opcode == SRE::OpCodes.Call) return OpCodes.Call;
            else if (opcode == SRE::OpCodes.Calli) return OpCodes.Calli;
            else if (opcode == SRE::OpCodes.Callvirt) return OpCodes.Callvirt;
            else if (opcode == SRE::OpCodes.Castclass) return OpCodes.Castclass;
            else if (opcode == SRE::OpCodes.Ceq) return OpCodes.Ceq;
            else if (opcode == SRE::OpCodes.Cgt) return OpCodes.Cgt;
            else if (opcode == SRE::OpCodes.Cgt_Un) return OpCodes.Cgt_Un;
            else if (opcode == SRE::OpCodes.Ckfinite) return OpCodes.Ckfinite;
            else if (opcode == SRE::OpCodes.Clt) return OpCodes.Clt;
            else if (opcode == SRE::OpCodes.Clt_Un) return OpCodes.Clt_Un;
            else if (opcode == SRE::OpCodes.Constrained) return OpCodes.Constrained;
            else if (opcode == SRE::OpCodes.Conv_I) return OpCodes.Conv_I;
            else if (opcode == SRE::OpCodes.Conv_I1) return OpCodes.Conv_I1;
            else if (opcode == SRE::OpCodes.Conv_I2) return OpCodes.Conv_I2;
            else if (opcode == SRE::OpCodes.Conv_I4) return OpCodes.Conv_I4;
            else if (opcode == SRE::OpCodes.Conv_I8) return OpCodes.Conv_I8;
            else if (opcode == SRE::OpCodes.Conv_Ovf_I) return OpCodes.Conv_Ovf_I;
            else if (opcode == SRE::OpCodes.Conv_Ovf_I_Un) return OpCodes.Conv_Ovf_I_Un;
            else if (opcode == SRE::OpCodes.Conv_Ovf_I1) return OpCodes.Conv_Ovf_I1;
            else if (opcode == SRE::OpCodes.Conv_Ovf_I1_Un) return OpCodes.Conv_Ovf_I1_Un;
            else if (opcode == SRE::OpCodes.Conv_Ovf_I2) return OpCodes.Conv_Ovf_I2;
            else if (opcode == SRE::OpCodes.Conv_Ovf_I2_Un) return OpCodes.Conv_Ovf_I2_Un;
            else if (opcode == SRE::OpCodes.Conv_Ovf_I4) return OpCodes.Conv_Ovf_I4;
            else if (opcode == SRE::OpCodes.Conv_Ovf_I4_Un) return OpCodes.Conv_Ovf_I4_Un;
            else if (opcode == SRE::OpCodes.Conv_Ovf_I8) return OpCodes.Conv_Ovf_I8;
            else if (opcode == SRE::OpCodes.Conv_Ovf_I8_Un) return OpCodes.Conv_Ovf_I8_Un;
            else if (opcode == SRE::OpCodes.Conv_Ovf_U) return OpCodes.Conv_Ovf_U;
            else if (opcode == SRE::OpCodes.Conv_Ovf_U_Un) return OpCodes.Conv_Ovf_U_Un;
            else if (opcode == SRE::OpCodes.Conv_Ovf_U1) return OpCodes.Conv_Ovf_U1;
            else if (opcode == SRE::OpCodes.Conv_Ovf_U1_Un) return OpCodes.Conv_Ovf_U1_Un;
            else if (opcode == SRE::OpCodes.Conv_Ovf_U2) return OpCodes.Conv_Ovf_U2;
            else if (opcode == SRE::OpCodes.Conv_Ovf_U2_Un) return OpCodes.Conv_Ovf_U2_Un;
            else if (opcode == SRE::OpCodes.Conv_Ovf_U4) return OpCodes.Conv_Ovf_U4;
            else if (opcode == SRE::OpCodes.Conv_Ovf_U4_Un) return OpCodes.Conv_Ovf_U4_Un;
            else if (opcode == SRE::OpCodes.Conv_Ovf_U8) return OpCodes.Conv_Ovf_U8;
            else if (opcode == SRE::OpCodes.Conv_Ovf_U8_Un) return OpCodes.Conv_Ovf_U8_Un;
            else if (opcode == SRE::OpCodes.Conv_R_Un) return OpCodes.Conv_R_Un;
            else if (opcode == SRE::OpCodes.Conv_R4) return OpCodes.Conv_R4;
            else if (opcode == SRE::OpCodes.Conv_R8) return OpCodes.Conv_R8;
            else if (opcode == SRE::OpCodes.Conv_U) return OpCodes.Conv_U;
            else if (opcode == SRE::OpCodes.Conv_U1) return OpCodes.Conv_U1;
            else if (opcode == SRE::OpCodes.Conv_U2) return OpCodes.Conv_U2;
            else if (opcode == SRE::OpCodes.Conv_U4) return OpCodes.Conv_U4;
            else if (opcode == SRE::OpCodes.Conv_U8) return OpCodes.Conv_U8;
            else if (opcode == SRE::OpCodes.Cpblk) return OpCodes.Cpblk;
            else if (opcode == SRE::OpCodes.Cpobj) return OpCodes.Cpobj;
            else if (opcode == SRE::OpCodes.Div) return OpCodes.Div;
            else if (opcode == SRE::OpCodes.Div_Un) return OpCodes.Div_Un;
            else if (opcode == SRE::OpCodes.Dup) return OpCodes.Dup;
            else if (opcode == SRE::OpCodes.Endfilter) return OpCodes.Endfilter;
            else if (opcode == SRE::OpCodes.Endfinally) return OpCodes.Endfinally;
            else if (opcode == SRE::OpCodes.Initblk) return OpCodes.Initblk;
            else if (opcode == SRE::OpCodes.Initobj) return OpCodes.Initobj;
            else if (opcode == SRE::OpCodes.Isinst) return OpCodes.Isinst;
            else if (opcode == SRE::OpCodes.Jmp) return OpCodes.Jmp;
            else if (opcode == SRE::OpCodes.Ldarg) return OpCodes.Ldarg;
            else if (opcode == SRE::OpCodes.Ldarg_0) return OpCodes.Ldarg_0;
            else if (opcode == SRE::OpCodes.Ldarg_1) return OpCodes.Ldarg_1;
            else if (opcode == SRE::OpCodes.Ldarg_2) return OpCodes.Ldarg_2;
            else if (opcode == SRE::OpCodes.Ldarg_3) return OpCodes.Ldarg_3;
            else if (opcode == SRE::OpCodes.Ldarg_S) return OpCodes.Ldarg_S;
            else if (opcode == SRE::OpCodes.Ldarga) return OpCodes.Ldarga;
            else if (opcode == SRE::OpCodes.Ldarga_S) return OpCodes.Ldarga_S;
            else if (opcode == SRE::OpCodes.Ldc_I4) return OpCodes.Ldc_I4;
            else if (opcode == SRE::OpCodes.Ldc_I4_0) return OpCodes.Ldc_I4_0;
            else if (opcode == SRE::OpCodes.Ldc_I4_1) return OpCodes.Ldc_I4_1;
            else if (opcode == SRE::OpCodes.Ldc_I4_2) return OpCodes.Ldc_I4_2;
            else if (opcode == SRE::OpCodes.Ldc_I4_3) return OpCodes.Ldc_I4_3;
            else if (opcode == SRE::OpCodes.Ldc_I4_4) return OpCodes.Ldc_I4_4;
            else if (opcode == SRE::OpCodes.Ldc_I4_5) return OpCodes.Ldc_I4_5;
            else if (opcode == SRE::OpCodes.Ldc_I4_6) return OpCodes.Ldc_I4_6;
            else if (opcode == SRE::OpCodes.Ldc_I4_7) return OpCodes.Ldc_I4_7;
            else if (opcode == SRE::OpCodes.Ldc_I4_8) return OpCodes.Ldc_I4_8;
            else if (opcode == SRE::OpCodes.Ldc_I4_M1) return OpCodes.Ldc_I4_M1;
            else if (opcode == SRE::OpCodes.Ldc_I4_S) return OpCodes.Ldc_I4_S;
            else if (opcode == SRE::OpCodes.Ldc_I8) return OpCodes.Ldc_I8;
            else if (opcode == SRE::OpCodes.Ldc_R4) return OpCodes.Ldc_R4;
            else if (opcode == SRE::OpCodes.Ldc_R8) return OpCodes.Ldc_R8;
            else if (opcode == SRE::OpCodes.Ldelem) return OpCodes.Ldelem;
            else if (opcode == SRE::OpCodes.Ldelem_I) return OpCodes.Ldelem_I;
            else if (opcode == SRE::OpCodes.Ldelem_I1) return OpCodes.Ldelem_I1;
            else if (opcode == SRE::OpCodes.Ldelem_I2) return OpCodes.Ldelem_I2;
            else if (opcode == SRE::OpCodes.Ldelem_I4) return OpCodes.Ldelem_I4;
            else if (opcode == SRE::OpCodes.Ldelem_I8) return OpCodes.Ldelem_I8;
            else if (opcode == SRE::OpCodes.Ldelem_R4) return OpCodes.Ldelem_R4;
            else if (opcode == SRE::OpCodes.Ldelem_R8) return OpCodes.Ldelem_R8;
            else if (opcode == SRE::OpCodes.Ldelem_Ref) return OpCodes.Ldelem_Ref;
            else if (opcode == SRE::OpCodes.Ldelem_U1) return OpCodes.Ldelem_U1;
            else if (opcode == SRE::OpCodes.Ldelem_U2) return OpCodes.Ldelem_U2;
            else if (opcode == SRE::OpCodes.Ldelem_U4) return OpCodes.Ldelem_U4;
            else if (opcode == SRE::OpCodes.Ldelema) return OpCodes.Ldelema;
            else if (opcode == SRE::OpCodes.Ldfld) return OpCodes.Ldfld;
            else if (opcode == SRE::OpCodes.Ldflda) return OpCodes.Ldflda;
            else if (opcode == SRE::OpCodes.Ldftn) return OpCodes.Ldftn;
            else if (opcode == SRE::OpCodes.Ldind_I) return OpCodes.Ldind_I;
            else if (opcode == SRE::OpCodes.Ldind_I1) return OpCodes.Ldind_I1;
            else if (opcode == SRE::OpCodes.Ldind_I2) return OpCodes.Ldind_I2;
            else if (opcode == SRE::OpCodes.Ldind_I4) return OpCodes.Ldind_I4;
            else if (opcode == SRE::OpCodes.Ldind_I8) return OpCodes.Ldind_I8;
            else if (opcode == SRE::OpCodes.Ldind_R4) return OpCodes.Ldind_R4;
            else if (opcode == SRE::OpCodes.Ldind_R8) return OpCodes.Ldind_R8;
            else if (opcode == SRE::OpCodes.Ldind_Ref) return OpCodes.Ldind_Ref;
            else if (opcode == SRE::OpCodes.Ldind_U1) return OpCodes.Ldind_U1;
            else if (opcode == SRE::OpCodes.Ldind_U2) return OpCodes.Ldind_U2;
            else if (opcode == SRE::OpCodes.Ldind_U4) return OpCodes.Ldind_U4;
            else if (opcode == SRE::OpCodes.Ldlen) return OpCodes.Ldlen;
            else if (opcode == SRE::OpCodes.Ldloc) return OpCodes.Ldloc;
            else if (opcode == SRE::OpCodes.Ldloc_0) return OpCodes.Ldloc_0;
            else if (opcode == SRE::OpCodes.Ldloc_1) return OpCodes.Ldloc_1;
            else if (opcode == SRE::OpCodes.Ldloc_2) return OpCodes.Ldloc_2;
            else if (opcode == SRE::OpCodes.Ldloc_3) return OpCodes.Ldloc_3;
            else if (opcode == SRE::OpCodes.Ldloc_S) return OpCodes.Ldloc_S;
            else if (opcode == SRE::OpCodes.Ldloca) return OpCodes.Ldloca;
            else if (opcode == SRE::OpCodes.Ldloca_S) return OpCodes.Ldloca_S;
            else if (opcode == SRE::OpCodes.Ldnull) return OpCodes.Ldnull;
            else if (opcode == SRE::OpCodes.Ldobj) return OpCodes.Ldobj;
            else if (opcode == SRE::OpCodes.Ldsfld) return OpCodes.Ldsfld;
            else if (opcode == SRE::OpCodes.Ldsflda) return OpCodes.Ldsflda;
            else if (opcode == SRE::OpCodes.Ldstr) return OpCodes.Ldstr;
            else if (opcode == SRE::OpCodes.Ldtoken) return OpCodes.Ldtoken;
            else if (opcode == SRE::OpCodes.Ldvirtftn) return OpCodes.Ldvirtftn;
            else if (opcode == SRE::OpCodes.Leave) return OpCodes.Leave;
            else if (opcode == SRE::OpCodes.Leave_S) return OpCodes.Leave_S;
            else if (opcode == SRE::OpCodes.Localloc) return OpCodes.Localloc;
            else if (opcode == SRE::OpCodes.Mkrefany) return OpCodes.Mkrefany;
            else if (opcode == SRE::OpCodes.Mul) return OpCodes.Mul;
            else if (opcode == SRE::OpCodes.Mul_Ovf) return OpCodes.Mul_Ovf;
            else if (opcode == SRE::OpCodes.Mul_Ovf_Un) return OpCodes.Mul_Ovf_Un;
            else if (opcode == SRE::OpCodes.Neg) return OpCodes.Neg;
            else if (opcode == SRE::OpCodes.Newarr) return OpCodes.Newarr;
            else if (opcode == SRE::OpCodes.Newobj) return OpCodes.Newobj;
            else if (opcode == SRE::OpCodes.Nop) return OpCodes.Nop;
            else if (opcode == SRE::OpCodes.Not) return OpCodes.Not;
            else if (opcode == SRE::OpCodes.Or) return OpCodes.Or;
            else if (opcode == SRE::OpCodes.Pop) return OpCodes.Pop;
            else if (opcode == SRE::OpCodes.Readonly) return OpCodes.Readonly;
            else if (opcode == SRE::OpCodes.Refanytype) return OpCodes.Refanytype;
            else if (opcode == SRE::OpCodes.Refanyval) return OpCodes.Refanyval;
            else if (opcode == SRE::OpCodes.Rem) return OpCodes.Rem;
            else if (opcode == SRE::OpCodes.Rem_Un) return OpCodes.Rem_Un;
            else if (opcode == SRE::OpCodes.Ret) return OpCodes.Ret;
            else if (opcode == SRE::OpCodes.Rethrow) return OpCodes.Rethrow;
            else if (opcode == SRE::OpCodes.Shl) return OpCodes.Shl;
            else if (opcode == SRE::OpCodes.Shr) return OpCodes.Shr;
            else if (opcode == SRE::OpCodes.Shr_Un) return OpCodes.Shr_Un;
            else if (opcode == SRE::OpCodes.Sizeof) return OpCodes.Sizeof;
            else if (opcode == SRE::OpCodes.Starg) return OpCodes.Starg;
            else if (opcode == SRE::OpCodes.Starg_S) return OpCodes.Starg_S;
            else if (opcode == SRE::OpCodes.Stelem) return OpCodes.Stelem;
            else if (opcode == SRE::OpCodes.Stelem_I) return OpCodes.Stelem_I;
            else if (opcode == SRE::OpCodes.Stelem_I1) return OpCodes.Stelem_I1;
            else if (opcode == SRE::OpCodes.Stelem_I2) return OpCodes.Stelem_I2;
            else if (opcode == SRE::OpCodes.Stelem_I4) return OpCodes.Stelem_I4;
            else if (opcode == SRE::OpCodes.Stelem_I8) return OpCodes.Stelem_I8;
            else if (opcode == SRE::OpCodes.Stelem_R4) return OpCodes.Stelem_R4;
            else if (opcode == SRE::OpCodes.Stelem_R8) return OpCodes.Stelem_R8;
            else if (opcode == SRE::OpCodes.Stelem_Ref) return OpCodes.Stelem_Ref;
            else if (opcode == SRE::OpCodes.Stfld) return OpCodes.Stfld;
            else if (opcode == SRE::OpCodes.Stind_I) return OpCodes.Stind_I;
            else if (opcode == SRE::OpCodes.Stind_I1) return OpCodes.Stind_I1;
            else if (opcode == SRE::OpCodes.Stind_I2) return OpCodes.Stind_I2;
            else if (opcode == SRE::OpCodes.Stind_I4) return OpCodes.Stind_I4;
            else if (opcode == SRE::OpCodes.Stind_I8) return OpCodes.Stind_I8;
            else if (opcode == SRE::OpCodes.Stind_R4) return OpCodes.Stind_R4;
            else if (opcode == SRE::OpCodes.Stind_R8) return OpCodes.Stind_R8;
            else if (opcode == SRE::OpCodes.Stind_Ref) return OpCodes.Stind_Ref;
            else if (opcode == SRE::OpCodes.Stloc) return OpCodes.Stloc;
            else if (opcode == SRE::OpCodes.Stloc_0) return OpCodes.Stloc_0;
            else if (opcode == SRE::OpCodes.Stloc_1) return OpCodes.Stloc_1;
            else if (opcode == SRE::OpCodes.Stloc_2) return OpCodes.Stloc_2;
            else if (opcode == SRE::OpCodes.Stloc_3) return OpCodes.Stloc_3;
            else if (opcode == SRE::OpCodes.Stloc_S) return OpCodes.Stloc_S;
            else if (opcode == SRE::OpCodes.Stobj) return OpCodes.Stobj;
            else if (opcode == SRE::OpCodes.Stsfld) return OpCodes.Stsfld;
            else if (opcode == SRE::OpCodes.Sub) return OpCodes.Sub;
            else if (opcode == SRE::OpCodes.Sub_Ovf) return OpCodes.Sub_Ovf;
            else if (opcode == SRE::OpCodes.Sub_Ovf_Un) return OpCodes.Sub_Ovf_Un;
            else if (opcode == SRE::OpCodes.Switch) return OpCodes.Switch;
            else if (opcode == SRE::OpCodes.Tailcall) return OpCodes.Tailcall;
            else if (opcode == SRE::OpCodes.Throw) return OpCodes.Throw;
            else if (opcode == SRE::OpCodes.Unaligned) return OpCodes.Unaligned;
            else if (opcode == SRE::OpCodes.Unbox) return OpCodes.Unbox;
            else if (opcode == SRE::OpCodes.Unbox_Any) return OpCodes.Unbox_Any;
            else if (opcode == SRE::OpCodes.Volatile) return OpCodes.Volatile;
            else if (opcode == SRE::OpCodes.Xor) return OpCodes.Xor;

            throw new NotSupportedException();
        }
    }
}

