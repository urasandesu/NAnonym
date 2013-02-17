/* 
 * File: OpCodes.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2012 Akira Sugiura
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


using SRE = System.Reflection.Emit;

namespace Urasandesu.NAnonym.Reflection.Emit
{
    public class OpCodes
    {
        protected OpCodes() { }

        public static readonly SRE::OpCode Add = SRE::OpCodes.Add;
        public static readonly SRE::OpCode Add_Ovf = SRE::OpCodes.Add_Ovf;
        public static readonly SRE::OpCode Add_Ovf_Un = SRE::OpCodes.Add_Ovf_Un;
        public static readonly SRE::OpCode And = SRE::OpCodes.And;
        public static readonly SRE::OpCode Arglist = SRE::OpCodes.Arglist;
        public static readonly SRE::OpCode Beq = SRE::OpCodes.Beq;
        public static readonly SRE::OpCode Beq_S = SRE::OpCodes.Beq_S;
        public static readonly SRE::OpCode Bge = SRE::OpCodes.Bge;
        public static readonly SRE::OpCode Bge_S = SRE::OpCodes.Bge_S;
        public static readonly SRE::OpCode Bge_Un = SRE::OpCodes.Bge_Un;
        public static readonly SRE::OpCode Bge_Un_S = SRE::OpCodes.Bge_Un_S;
        public static readonly SRE::OpCode Bgt = SRE::OpCodes.Bgt;
        public static readonly SRE::OpCode Bgt_S = SRE::OpCodes.Bgt_S;
        public static readonly SRE::OpCode Bgt_Un = SRE::OpCodes.Bgt_Un;
        public static readonly SRE::OpCode Bgt_Un_S = SRE::OpCodes.Bgt_Un_S;
        public static readonly SRE::OpCode Ble = SRE::OpCodes.Ble;
        public static readonly SRE::OpCode Ble_S = SRE::OpCodes.Ble_S;
        public static readonly SRE::OpCode Ble_Un = SRE::OpCodes.Ble_Un;
        public static readonly SRE::OpCode Ble_Un_S = SRE::OpCodes.Ble_Un_S;
        public static readonly SRE::OpCode Blt = SRE::OpCodes.Blt;
        public static readonly SRE::OpCode Blt_S = SRE::OpCodes.Blt_S;
        public static readonly SRE::OpCode Blt_Un = SRE::OpCodes.Blt_Un;
        public static readonly SRE::OpCode Blt_Un_S = SRE::OpCodes.Blt_Un_S;
        public static readonly SRE::OpCode Bne_Un = SRE::OpCodes.Bne_Un;
        public static readonly SRE::OpCode Bne_Un_S = SRE::OpCodes.Bne_Un_S;
        public static readonly SRE::OpCode Box = SRE::OpCodes.Box;
        public static readonly SRE::OpCode Br = SRE::OpCodes.Br;
        public static readonly SRE::OpCode Br_S = SRE::OpCodes.Br_S;
        public static readonly SRE::OpCode Break = SRE::OpCodes.Break;
        public static readonly SRE::OpCode Brfalse = SRE::OpCodes.Brfalse;
        public static readonly SRE::OpCode Brfalse_S = SRE::OpCodes.Brfalse_S;
        public static readonly SRE::OpCode Brtrue = SRE::OpCodes.Brtrue;
        public static readonly SRE::OpCode Brtrue_S = SRE::OpCodes.Brtrue_S;
        public static readonly SRE::OpCode Call = SRE::OpCodes.Call;
        public static readonly SRE::OpCode Calli = SRE::OpCodes.Calli;
        public static readonly SRE::OpCode Callvirt = SRE::OpCodes.Callvirt;
        public static readonly SRE::OpCode Castclass = SRE::OpCodes.Castclass;
        public static readonly SRE::OpCode Ceq = SRE::OpCodes.Ceq;
        public static readonly SRE::OpCode Cgt = SRE::OpCodes.Cgt;
        public static readonly SRE::OpCode Cgt_Un = SRE::OpCodes.Cgt_Un;
        public static readonly SRE::OpCode Ckfinite = SRE::OpCodes.Ckfinite;
        public static readonly SRE::OpCode Clt = SRE::OpCodes.Clt;
        public static readonly SRE::OpCode Clt_Un = SRE::OpCodes.Clt_Un;
        public static readonly SRE::OpCode Constrained = SRE::OpCodes.Constrained;
        public static readonly SRE::OpCode Conv_I = SRE::OpCodes.Conv_I;
        public static readonly SRE::OpCode Conv_I1 = SRE::OpCodes.Conv_I1;
        public static readonly SRE::OpCode Conv_I2 = SRE::OpCodes.Conv_I2;
        public static readonly SRE::OpCode Conv_I4 = SRE::OpCodes.Conv_I4;
        public static readonly SRE::OpCode Conv_I8 = SRE::OpCodes.Conv_I8;
        public static readonly SRE::OpCode Conv_Ovf_I = SRE::OpCodes.Conv_Ovf_I;
        public static readonly SRE::OpCode Conv_Ovf_I_Un = SRE::OpCodes.Conv_Ovf_I_Un;
        public static readonly SRE::OpCode Conv_Ovf_I1 = SRE::OpCodes.Conv_Ovf_I1;
        public static readonly SRE::OpCode Conv_Ovf_I1_Un = SRE::OpCodes.Conv_Ovf_I1_Un;
        public static readonly SRE::OpCode Conv_Ovf_I2 = SRE::OpCodes.Conv_Ovf_I2;
        public static readonly SRE::OpCode Conv_Ovf_I2_Un = SRE::OpCodes.Conv_Ovf_I2_Un;
        public static readonly SRE::OpCode Conv_Ovf_I4 = SRE::OpCodes.Conv_Ovf_I4;
        public static readonly SRE::OpCode Conv_Ovf_I4_Un = SRE::OpCodes.Conv_Ovf_I4_Un;
        public static readonly SRE::OpCode Conv_Ovf_I8 = SRE::OpCodes.Conv_Ovf_I8;
        public static readonly SRE::OpCode Conv_Ovf_I8_Un = SRE::OpCodes.Conv_Ovf_I8_Un;
        public static readonly SRE::OpCode Conv_Ovf_U = SRE::OpCodes.Conv_Ovf_U;
        public static readonly SRE::OpCode Conv_Ovf_U_Un = SRE::OpCodes.Conv_Ovf_U_Un;
        public static readonly SRE::OpCode Conv_Ovf_U1 = SRE::OpCodes.Conv_Ovf_U1;
        public static readonly SRE::OpCode Conv_Ovf_U1_Un = SRE::OpCodes.Conv_Ovf_U1_Un;
        public static readonly SRE::OpCode Conv_Ovf_U2 = SRE::OpCodes.Conv_Ovf_U2;
        public static readonly SRE::OpCode Conv_Ovf_U2_Un = SRE::OpCodes.Conv_Ovf_U2_Un;
        public static readonly SRE::OpCode Conv_Ovf_U4 = SRE::OpCodes.Conv_Ovf_U4;
        public static readonly SRE::OpCode Conv_Ovf_U4_Un = SRE::OpCodes.Conv_Ovf_U4_Un;
        public static readonly SRE::OpCode Conv_Ovf_U8 = SRE::OpCodes.Conv_Ovf_U8;
        public static readonly SRE::OpCode Conv_Ovf_U8_Un = SRE::OpCodes.Conv_Ovf_U8_Un;
        public static readonly SRE::OpCode Conv_R_Un = SRE::OpCodes.Conv_R_Un;
        public static readonly SRE::OpCode Conv_R4 = SRE::OpCodes.Conv_R4;
        public static readonly SRE::OpCode Conv_R8 = SRE::OpCodes.Conv_R8;
        public static readonly SRE::OpCode Conv_U = SRE::OpCodes.Conv_U;
        public static readonly SRE::OpCode Conv_U1 = SRE::OpCodes.Conv_U1;
        public static readonly SRE::OpCode Conv_U2 = SRE::OpCodes.Conv_U2;
        public static readonly SRE::OpCode Conv_U4 = SRE::OpCodes.Conv_U4;
        public static readonly SRE::OpCode Conv_U8 = SRE::OpCodes.Conv_U8;
        public static readonly SRE::OpCode Cpblk = SRE::OpCodes.Cpblk;
        public static readonly SRE::OpCode Cpobj = SRE::OpCodes.Cpobj;
        public static readonly SRE::OpCode Div = SRE::OpCodes.Div;
        public static readonly SRE::OpCode Div_Un = SRE::OpCodes.Div_Un;
        public static readonly SRE::OpCode Dup = SRE::OpCodes.Dup;
        public static readonly SRE::OpCode Endfilter = SRE::OpCodes.Endfilter;
        public static readonly SRE::OpCode Endfinally = SRE::OpCodes.Endfinally;
        public static readonly SRE::OpCode Initblk = SRE::OpCodes.Initblk;
        public static readonly SRE::OpCode Initobj = SRE::OpCodes.Initobj;
        public static readonly SRE::OpCode Isinst = SRE::OpCodes.Isinst;
        public static readonly SRE::OpCode Jmp = SRE::OpCodes.Jmp;
        public static readonly SRE::OpCode Ldarg = SRE::OpCodes.Ldarg;
        public static readonly SRE::OpCode Ldarg_0 = SRE::OpCodes.Ldarg_0;
        public static readonly SRE::OpCode Ldarg_1 = SRE::OpCodes.Ldarg_1;
        public static readonly SRE::OpCode Ldarg_2 = SRE::OpCodes.Ldarg_2;
        public static readonly SRE::OpCode Ldarg_3 = SRE::OpCodes.Ldarg_3;
        public static readonly SRE::OpCode Ldarg_S = SRE::OpCodes.Ldarg_S;
        public static readonly SRE::OpCode Ldarga = SRE::OpCodes.Ldarga;
        public static readonly SRE::OpCode Ldarga_S = SRE::OpCodes.Ldarga_S;
        public static readonly SRE::OpCode Ldc_I4 = SRE::OpCodes.Ldc_I4;
        public static readonly SRE::OpCode Ldc_I4_0 = SRE::OpCodes.Ldc_I4_0;
        public static readonly SRE::OpCode Ldc_I4_1 = SRE::OpCodes.Ldc_I4_1;
        public static readonly SRE::OpCode Ldc_I4_2 = SRE::OpCodes.Ldc_I4_2;
        public static readonly SRE::OpCode Ldc_I4_3 = SRE::OpCodes.Ldc_I4_3;
        public static readonly SRE::OpCode Ldc_I4_4 = SRE::OpCodes.Ldc_I4_4;
        public static readonly SRE::OpCode Ldc_I4_5 = SRE::OpCodes.Ldc_I4_5;
        public static readonly SRE::OpCode Ldc_I4_6 = SRE::OpCodes.Ldc_I4_6;
        public static readonly SRE::OpCode Ldc_I4_7 = SRE::OpCodes.Ldc_I4_7;
        public static readonly SRE::OpCode Ldc_I4_8 = SRE::OpCodes.Ldc_I4_8;
        public static readonly SRE::OpCode Ldc_I4_M1 = SRE::OpCodes.Ldc_I4_M1;
        public static readonly SRE::OpCode Ldc_I4_S = SRE::OpCodes.Ldc_I4_S;
        public static readonly SRE::OpCode Ldc_I8 = SRE::OpCodes.Ldc_I8;
        public static readonly SRE::OpCode Ldc_R4 = SRE::OpCodes.Ldc_R4;
        public static readonly SRE::OpCode Ldc_R8 = SRE::OpCodes.Ldc_R8;
        public static readonly SRE::OpCode Ldelem = SRE::OpCodes.Ldelem;
        public static readonly SRE::OpCode Ldelem_I = SRE::OpCodes.Ldelem_I;
        public static readonly SRE::OpCode Ldelem_I1 = SRE::OpCodes.Ldelem_I1;
        public static readonly SRE::OpCode Ldelem_I2 = SRE::OpCodes.Ldelem_I2;
        public static readonly SRE::OpCode Ldelem_I4 = SRE::OpCodes.Ldelem_I4;
        public static readonly SRE::OpCode Ldelem_I8 = SRE::OpCodes.Ldelem_I8;
        public static readonly SRE::OpCode Ldelem_R4 = SRE::OpCodes.Ldelem_R4;
        public static readonly SRE::OpCode Ldelem_R8 = SRE::OpCodes.Ldelem_R8;
        public static readonly SRE::OpCode Ldelem_Ref = SRE::OpCodes.Ldelem_Ref;
        public static readonly SRE::OpCode Ldelem_U1 = SRE::OpCodes.Ldelem_U1;
        public static readonly SRE::OpCode Ldelem_U2 = SRE::OpCodes.Ldelem_U2;
        public static readonly SRE::OpCode Ldelem_U4 = SRE::OpCodes.Ldelem_U4;
        public static readonly SRE::OpCode Ldelema = SRE::OpCodes.Ldelema;
        public static readonly SRE::OpCode Ldfld = SRE::OpCodes.Ldfld;
        public static readonly SRE::OpCode Ldflda = SRE::OpCodes.Ldflda;
        public static readonly SRE::OpCode Ldftn = SRE::OpCodes.Ldftn;
        public static readonly SRE::OpCode Ldind_I = SRE::OpCodes.Ldind_I;
        public static readonly SRE::OpCode Ldind_I1 = SRE::OpCodes.Ldind_I1;
        public static readonly SRE::OpCode Ldind_I2 = SRE::OpCodes.Ldind_I2;
        public static readonly SRE::OpCode Ldind_I4 = SRE::OpCodes.Ldind_I4;
        public static readonly SRE::OpCode Ldind_I8 = SRE::OpCodes.Ldind_I8;
        public static readonly SRE::OpCode Ldind_R4 = SRE::OpCodes.Ldind_R4;
        public static readonly SRE::OpCode Ldind_R8 = SRE::OpCodes.Ldind_R8;
        public static readonly SRE::OpCode Ldind_Ref = SRE::OpCodes.Ldind_Ref;
        public static readonly SRE::OpCode Ldind_U1 = SRE::OpCodes.Ldind_U1;
        public static readonly SRE::OpCode Ldind_U2 = SRE::OpCodes.Ldind_U2;
        public static readonly SRE::OpCode Ldind_U4 = SRE::OpCodes.Ldind_U4;
        public static readonly SRE::OpCode Ldlen = SRE::OpCodes.Ldlen;
        public static readonly SRE::OpCode Ldloc = SRE::OpCodes.Ldloc;
        public static readonly SRE::OpCode Ldloc_0 = SRE::OpCodes.Ldloc_0;
        public static readonly SRE::OpCode Ldloc_1 = SRE::OpCodes.Ldloc_1;
        public static readonly SRE::OpCode Ldloc_2 = SRE::OpCodes.Ldloc_2;
        public static readonly SRE::OpCode Ldloc_3 = SRE::OpCodes.Ldloc_3;
        public static readonly SRE::OpCode Ldloc_S = SRE::OpCodes.Ldloc_S;
        public static readonly SRE::OpCode Ldloca = SRE::OpCodes.Ldloca;
        public static readonly SRE::OpCode Ldloca_S = SRE::OpCodes.Ldloca_S;
        public static readonly SRE::OpCode Ldnull = SRE::OpCodes.Ldnull;
        public static readonly SRE::OpCode Ldobj = SRE::OpCodes.Ldobj;
        public static readonly SRE::OpCode Ldsfld = SRE::OpCodes.Ldsfld;
        public static readonly SRE::OpCode Ldsflda = SRE::OpCodes.Ldsflda;
        public static readonly SRE::OpCode Ldstr = SRE::OpCodes.Ldstr;
        public static readonly SRE::OpCode Ldtoken = SRE::OpCodes.Ldtoken;
        public static readonly SRE::OpCode Ldvirtftn = SRE::OpCodes.Ldvirtftn;
        public static readonly SRE::OpCode Leave = SRE::OpCodes.Leave;
        public static readonly SRE::OpCode Leave_S = SRE::OpCodes.Leave_S;
        public static readonly SRE::OpCode Localloc = SRE::OpCodes.Localloc;
        public static readonly SRE::OpCode Mkrefany = SRE::OpCodes.Mkrefany;
        public static readonly SRE::OpCode Mul = SRE::OpCodes.Mul;
        public static readonly SRE::OpCode Mul_Ovf = SRE::OpCodes.Mul_Ovf;
        public static readonly SRE::OpCode Mul_Ovf_Un = SRE::OpCodes.Mul_Ovf_Un;
        public static readonly SRE::OpCode Neg = SRE::OpCodes.Neg;
        public static readonly SRE::OpCode Newarr = SRE::OpCodes.Newarr;
        public static readonly SRE::OpCode Newobj = SRE::OpCodes.Newobj;
        public static readonly SRE::OpCode Nop = SRE::OpCodes.Nop;
        public static readonly SRE::OpCode Not = SRE::OpCodes.Not;
        public static readonly SRE::OpCode Or = SRE::OpCodes.Or;
        public static readonly SRE::OpCode Pop = SRE::OpCodes.Pop;
        public static readonly SRE::OpCode Prefix1 = SRE::OpCodes.Prefix1;
        public static readonly SRE::OpCode Prefix2 = SRE::OpCodes.Prefix2;
        public static readonly SRE::OpCode Prefix3 = SRE::OpCodes.Prefix3;
        public static readonly SRE::OpCode Prefix4 = SRE::OpCodes.Prefix4;
        public static readonly SRE::OpCode Prefix5 = SRE::OpCodes.Prefix5;
        public static readonly SRE::OpCode Prefix6 = SRE::OpCodes.Prefix6;
        public static readonly SRE::OpCode Prefix7 = SRE::OpCodes.Prefix7;
        public static readonly SRE::OpCode Prefixref = SRE::OpCodes.Prefixref;
        public static readonly SRE::OpCode Readonly = SRE::OpCodes.Readonly;
        public static readonly SRE::OpCode Refanytype = SRE::OpCodes.Refanytype;
        public static readonly SRE::OpCode Refanyval = SRE::OpCodes.Refanyval;
        public static readonly SRE::OpCode Rem = SRE::OpCodes.Rem;
        public static readonly SRE::OpCode Rem_Un = SRE::OpCodes.Rem_Un;
        public static readonly SRE::OpCode Ret = SRE::OpCodes.Ret;
        public static readonly SRE::OpCode Rethrow = SRE::OpCodes.Rethrow;
        public static readonly SRE::OpCode Shl = SRE::OpCodes.Shl;
        public static readonly SRE::OpCode Shr = SRE::OpCodes.Shr;
        public static readonly SRE::OpCode Shr_Un = SRE::OpCodes.Shr_Un;
        public static readonly SRE::OpCode Sizeof = SRE::OpCodes.Sizeof;
        public static readonly SRE::OpCode Starg = SRE::OpCodes.Starg;
        public static readonly SRE::OpCode Starg_S = SRE::OpCodes.Starg_S;
        public static readonly SRE::OpCode Stelem = SRE::OpCodes.Stelem;
        public static readonly SRE::OpCode Stelem_I = SRE::OpCodes.Stelem_I;
        public static readonly SRE::OpCode Stelem_I1 = SRE::OpCodes.Stelem_I1;
        public static readonly SRE::OpCode Stelem_I2 = SRE::OpCodes.Stelem_I2;
        public static readonly SRE::OpCode Stelem_I4 = SRE::OpCodes.Stelem_I4;
        public static readonly SRE::OpCode Stelem_I8 = SRE::OpCodes.Stelem_I8;
        public static readonly SRE::OpCode Stelem_R4 = SRE::OpCodes.Stelem_R4;
        public static readonly SRE::OpCode Stelem_R8 = SRE::OpCodes.Stelem_R8;
        public static readonly SRE::OpCode Stelem_Ref = SRE::OpCodes.Stelem_Ref;
        public static readonly SRE::OpCode Stfld = SRE::OpCodes.Stfld;
        public static readonly SRE::OpCode Stind_I = SRE::OpCodes.Stind_I;
        public static readonly SRE::OpCode Stind_I1 = SRE::OpCodes.Stind_I1;
        public static readonly SRE::OpCode Stind_I2 = SRE::OpCodes.Stind_I2;
        public static readonly SRE::OpCode Stind_I4 = SRE::OpCodes.Stind_I4;
        public static readonly SRE::OpCode Stind_I8 = SRE::OpCodes.Stind_I8;
        public static readonly SRE::OpCode Stind_R4 = SRE::OpCodes.Stind_R4;
        public static readonly SRE::OpCode Stind_R8 = SRE::OpCodes.Stind_R8;
        public static readonly SRE::OpCode Stind_Ref = SRE::OpCodes.Stind_Ref;
        public static readonly SRE::OpCode Stloc = SRE::OpCodes.Stloc;
        public static readonly SRE::OpCode Stloc_0 = SRE::OpCodes.Stloc_0;
        public static readonly SRE::OpCode Stloc_1 = SRE::OpCodes.Stloc_1;
        public static readonly SRE::OpCode Stloc_2 = SRE::OpCodes.Stloc_2;
        public static readonly SRE::OpCode Stloc_3 = SRE::OpCodes.Stloc_3;
        public static readonly SRE::OpCode Stloc_S = SRE::OpCodes.Stloc_S;
        public static readonly SRE::OpCode Stobj = SRE::OpCodes.Stobj;
        public static readonly SRE::OpCode Stsfld = SRE::OpCodes.Stsfld;
        public static readonly SRE::OpCode Sub = SRE::OpCodes.Sub;
        public static readonly SRE::OpCode Sub_Ovf = SRE::OpCodes.Sub_Ovf;
        public static readonly SRE::OpCode Sub_Ovf_Un = SRE::OpCodes.Sub_Ovf_Un;
        public static readonly SRE::OpCode Switch = SRE::OpCodes.Switch;
        public static readonly SRE::OpCode Tailcall = SRE::OpCodes.Tailcall;
        public static readonly SRE::OpCode Throw = SRE::OpCodes.Throw;
        public static readonly SRE::OpCode Unaligned = SRE::OpCodes.Unaligned;
        public static readonly SRE::OpCode Unbox = SRE::OpCodes.Unbox;
        public static readonly SRE::OpCode Unbox_Any = SRE::OpCodes.Unbox_Any;
        public static readonly SRE::OpCode Volatile = SRE::OpCodes.Volatile;
        public static readonly SRE::OpCode Xor = SRE::OpCodes.Xor;

        // In the following, define the extended opcodes that are greater than 'OPDEF(CEE_UNUSED70, "unused", Pop0, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x22, NEXT)'.
        // About the definitions of opcodes, please see also %INCLUDE%\opcode.def.        
        public static readonly OpCodeEx Ldloc_Opt = new OpCodeEx("ldloc.opt", 2, 0xFE, 0x2E);
        public static readonly OpCodeEx Ldloca_Opt = new OpCodeEx("ldloca.opt", 2, 0xFE, 0x2F);        
        public static readonly OpCodeEx Stloc_Opt = new OpCodeEx("stloc.opt", 2, 0xFE, 0x30);
        public static readonly OpCodeEx Ldc_I4_Opt = new OpCodeEx("ldc.i4.opt", 2, 0xFE, 0x42);
        public static readonly OpCodeEx Unbox_Opt = new OpCodeEx("unbox.opt", 2, 0xFE, 0x9B);
        public static readonly OpCodeEx Box_Opt = new OpCodeEx("box.opt", 2, 0xFE, 0xAE);
    }
}
