/* 
 * File: ILGeneratorMixin.cs
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


using System;
using System.Diagnostics;
using Urasandesu.NAnonym.Reflection.Emit;
using SRE = System.Reflection.Emit;

namespace Urasandesu.NAnonym.Mixins.System.Reflection.Emit
{
    public static class ILGeneratorMixin
    {
        public static void Emit(this SRE::ILGenerator gen, OpCodeEx opcode, Type cls)
        {
            if (gen == null)
                throw new ArgumentNullException("gen");

            if (opcode == OpCodes.Unbox_Opt) gen.Emit_Unbox(cls);
            else if (opcode == OpCodes.Box_Opt) gen.Emit_Box(cls);
        }

        static void Emit_Unbox(this SRE::ILGenerator gen, Type cls)
        {
            Debug.Assert(gen != null);

            if (cls.IsValueType)
                gen.Emit(OpCodes.Unbox_Any, cls);
            else
                gen.Emit(OpCodes.Castclass, cls);
        }

        static void Emit_Box(this SRE::ILGenerator gen, Type cls)
        {
            Debug.Assert(gen != null);

            if (cls.IsValueType)
                gen.Emit(OpCodes.Box, cls);
        }

        public static void Emit(this SRE::ILGenerator gen, OpCodeEx opcode, int arg)
        {
            if (gen == null)
                throw new ArgumentNullException("gen");

            if (opcode == OpCodes.Ldloc_Opt) gen.Emit_Ldloc(arg);
            else if (opcode == OpCodes.Ldloca_Opt) gen.Emit_Ldloca(arg);
            else if (opcode == OpCodes.Stloc_Opt) gen.Emit_Stloc(arg);
            else if (opcode == OpCodes.Ldc_I4_Opt) gen.Emit_Ldc_I4(arg);
        }

        static void Emit_Ldloc(this SRE::ILGenerator gen, int arg)
        {
            Debug.Assert(gen != null);

            switch (arg)
            {
                case 0: gen.Emit(SRE::OpCodes.Ldloc_0); break;
                case 1: gen.Emit(SRE::OpCodes.Ldloc_1); break;
                case 2: gen.Emit(SRE::OpCodes.Ldloc_2); break;
                case 3: gen.Emit(SRE::OpCodes.Ldloc_3); break;
                default:
                    if ((int)byte.MinValue <= arg && arg <= (int)byte.MaxValue)
                        gen.Emit(SRE::OpCodes.Ldloc_S, (byte)arg);
                    else
                        gen.Emit(SRE::OpCodes.Ldloc, arg);
                    break;
            }
        }

        static void Emit_Ldloca(this SRE::ILGenerator gen, int arg)
        {
            Debug.Assert(gen != null);

            if ((int)byte.MinValue <= arg && arg <= (int)byte.MaxValue)
                gen.Emit(SRE::OpCodes.Ldloca_S, (byte)arg);
            else
                gen.Emit(SRE::OpCodes.Ldloca, arg);
        }

        static void Emit_Stloc(this SRE::ILGenerator gen, int arg)
        {
            Debug.Assert(gen != null);

            switch (arg)
            {
                case 0: gen.Emit(SRE::OpCodes.Stloc_0); break;
                case 1: gen.Emit(SRE::OpCodes.Stloc_1); break;
                case 2: gen.Emit(SRE::OpCodes.Stloc_2); break;
                case 3: gen.Emit(SRE::OpCodes.Stloc_3); break;
                default:
                    if ((int)byte.MinValue <= arg && arg <= (int)byte.MaxValue)
                        gen.Emit(SRE::OpCodes.Stloc_S, (byte)arg);
                    else
                        gen.Emit(SRE::OpCodes.Stloc, arg);
                    break;
            }
        }

        static void Emit_Ldc_I4(this SRE::ILGenerator gen, int arg)
        {
            Debug.Assert(gen != null);

            switch (arg)
            {
                case -1: gen.Emit(SRE::OpCodes.Ldc_I4_M1); break;
                case 0: gen.Emit(SRE::OpCodes.Ldc_I4_0); break;
                case 1: gen.Emit(SRE::OpCodes.Ldc_I4_1); break;
                case 2: gen.Emit(SRE::OpCodes.Ldc_I4_2); break;
                case 3: gen.Emit(SRE::OpCodes.Ldc_I4_3); break;
                case 4: gen.Emit(SRE::OpCodes.Ldc_I4_4); break;
                case 5: gen.Emit(SRE::OpCodes.Ldc_I4_5); break;
                case 6: gen.Emit(SRE::OpCodes.Ldc_I4_6); break;
                case 7: gen.Emit(SRE::OpCodes.Ldc_I4_7); break;
                case 8: gen.Emit(SRE::OpCodes.Ldc_I4_8); break;
                default:
                    if ((int)byte.MinValue <= arg && arg <= (int)byte.MaxValue)
                        gen.Emit(SRE::OpCodes.Ldc_I4_S, (byte)arg);
                    else
                        gen.Emit(SRE::OpCodes.Ldc_I4, arg);
                    break;
            }
        }
    }
}
