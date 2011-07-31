/* 
 * File: ILBuilder.cs
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
using Urasandesu.NAnonym.Formulas;
using System.Linq.Expressions;
using System.Reflection;
using Urasandesu.NAnonym.Linq;
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{
    public class ILBuilder : FormulaAdapter
    {
        IMethodBodyGenerator bodyGen;
        IILOperator il;
        public ILBuilder(IMethodBaseGenerator methodGen, ITypeDeclaration returnType)
            : base(new NoActionVisitor())
        {
            MethodGenerator = methodGen;
            bodyGen = methodGen.Body;
            il = bodyGen.ILOperator;

            ReturnType = returnType;
        }

        public IMethodBaseGenerator MethodGenerator { get; private set; }
        public ITypeDeclaration ReturnType { get; private set; }

        public override void Visit(BaseNewFormula formula)
        {
            base.Visit(formula);
            il.Emit(OpCodes.Ldarg_0);
            var ci = default(IConstructorDeclaration);
            if (MethodGenerator.DeclaringType.BaseType != null)
            {
                ci = MethodGenerator.DeclaringType.BaseType.GetConstructor(new Type[] { });
            }
            else
            {
                throw new NotImplementedException();
            }
            il.Emit(OpCodes.Call, ci);
        }

        public override void Visit(BlockFormula formula)
        {
            foreach (var local in formula.Locals)
            {
                local.Local = il.AddLocal(local.LocalName, local.TypeDeclaration);
            }
            base.Visit(formula);
        }

        public override void Visit(ConstantFormula formula)
        {
            base.Visit(formula);
            string s = default(string);
            short? ns = default(short?);
            int? ni = default(int?);
            double? nd = default(double?);
            char? nc = default(char?);
            sbyte? nsb = default(sbyte?);
            bool? nb = default(bool?);
            var e = default(Enum);
            var t = default(Type);
            var mi = default(MethodInfo);
            var ci = default(ConstructorInfo);
            var fi = default(FieldInfo);
            if (formula.ConstantValue == null)
            {
                il.Emit(OpCodes.Ldnull);
            }
            else if ((s = formula.ConstantValue as string) != null)
            {
                il.Emit(OpCodes.Ldstr, s);
            }
            else if ((ns = formula.ConstantValue as short?) != null)
            {
                il.Emit(OpCodes.Ldc_I4, (int)ns);
                il.Emit(OpCodes.Conv_I2);
            }
            else if ((ni = formula.ConstantValue as int?) != null)
            {
                // HACK: _S が付く命令は短い形式。-127 ～ 128 以外は最適化が必要。
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)(int)ni);
            }
            else if ((nd = formula.ConstantValue as double?) != null)
            {
                il.Emit(OpCodes.Ldc_R8, (double)nd);
            }
            else if ((nc = formula.ConstantValue as char?) != null)
            {
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)(char)nc);
            }
            else if ((nsb = formula.ConstantValue as sbyte?) != null)
            {
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)nsb);
            }
            else if ((nb = formula.ConstantValue as bool?) != null)
            {
                il.Emit(OpCodes.Ldc_I4, (bool)nb ? 1 : 0);
            }
            else if ((e = formula.ConstantValue as Enum) != null)
            {
                il.Emit(OpCodes.Ldc_I4, e.GetHashCode());
            }
            else if ((t = formula.ConstantValue as Type) != null)
            {
                il.Emit(OpCodes.Ldtoken, t);
                il.Emit(OpCodes.Call, MethodInfoMixin.GetTypeFromHandleInfo);
            }
            else if ((mi = formula.ConstantValue as MethodInfo) != null)
            {
                il.Emit(OpCodes.Ldtoken, mi);
                il.Emit(OpCodes.Call, MethodInfoMixin.GetMethodFromHandleInfo);
                il.Emit(OpCodes.Castclass, typeof(MethodInfo));
            }
            else if ((ci = formula.ConstantValue as ConstructorInfo) != null)
            {
                il.Emit(OpCodes.Ldtoken, ci);
                il.Emit(OpCodes.Call, MethodInfoMixin.GetMethodFromHandleInfo);
                il.Emit(OpCodes.Castclass, typeof(ConstructorInfo));
            }
            else if ((fi = formula.ConstantValue as FieldInfo) != null)
            {
                il.Emit(OpCodes.Ldtoken, fi);
                il.Emit(OpCodes.Call, MethodInfoMixin.GetFieldFromHandleInfo);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override void Visit(AddFormula formula)
        {
            base.Visit(formula);
            il.Emit(OpCodes.Add);
        }

        public override void Visit(MultiplyFormula formula)
        {
            base.Visit(formula);
            il.Emit(OpCodes.Mul);
        }

        public override void Visit(ExclusiveOrFormula formula)
        {
            base.Visit(formula);
            il.Emit(OpCodes.Xor);
        }

        public override void Visit(SubtractFormula formula)
        {
            base.Visit(formula);
            il.Emit(OpCodes.Sub);
        }

        public override void Visit(LessThanFormula formula)
        {
            base.Visit(formula);
            il.Emit(OpCodes.Clt);
        }

        public override void Visit(LessThanOrEqualFormula formula)
        {
            base.Visit(formula);
            il.Emit(OpCodes.Clt);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Ceq);
        }

        public override void Visit(ReturnFormula formula)
        {
            base.Visit(formula);
            var label = il.AddLabel();
            formula.Label = label;
            il.Emit(OpCodes.Br, label);
        }

        public override void Visit(EndFormula formula)
        {
            base.Visit(formula);
            formula.Returns.ForEach(_ => il.SetLabel(_.Label));
            il.Emit(OpCodes.Ret);
        }

        public override void Visit(AssignFormula formula)
        {
            if (formula.Left != null && (formula.Left.NodeType == NodeType.Property || formula.Left.NodeType == NodeType.ReflectiveProperty))
            {
                var property = (PropertyFormula)formula.Left;
                if (property.Instance != null) VisitPropertyInstanceCore(property, property.Instance);
                if (formula.Right != null) VisitAssignRightCore(formula, formula.Right);
                if (property.Member.SetMethod.IsStatic || property.Member.DeclaringType.IsValueType)
                {
                    il.Emit(OpCodes.Call, property.Member.SetMethod);
                }
                else
                {
                    il.Emit(OpCodes.Callvirt, property.Member.SetMethod);
                }
            }
            else if (formula.Left != null && (formula.Left.NodeType == NodeType.Field || formula.Left.NodeType == NodeType.ReflectiveField))
            {
                var field = (FieldFormula)formula.Left;
                if (field.Instance != null) VisitFieldInstanceCore(field, field.Instance);
                if (formula.Right != null) VisitAssignRightCore(formula, formula.Right);
                if (field.Member.IsStatic)
                {
                    il.Emit(OpCodes.Stsfld, field.Member);
                }
                else
                {
                    il.Emit(OpCodes.Stfld, field.Member);
                }
            }
            else
            {
                base.Visit(formula);
            }
        }

        protected override void VisitAssignLeftCore(AssignFormula formula, Formula left)
        {
            if (left.NodeType == NodeType.Variable)
            {
                var variable = (VariableFormula)left;
                if (variable.Resolved.NodeType == NodeType.Local)
                {
                    var local = (LocalNode)variable.Resolved;
                    il.Emit(OpCodes.Stloc, local.Local);
                    if (!formula.TypeDeclaration.Equals(typeof(void).ToTypeDecl()))
                    {
                        il.Emit(OpCodes.Ldloc, local.Local);
                    }
                }
                else
                {
                    base.VisitAssignLeftCore(formula, left);
                }
            }
            else
            {
                base.VisitAssignLeftCore(formula, left);
            }
        }

        public override void Visit(LocalNode formula)
        {
            base.Visit(formula);
            il.Emit(OpCodes.Ldloc, formula.Local);
        }

        public override void Visit(ArgumentFormula formula)
        {
            base.Visit(formula);
            var parameter = default(IParameterDeclaration);
            if (MethodGenerator.IsStatic)
            {
                if (-1 < formula.ArgumentPosition)
                {
                    parameter = MethodGenerator.Parameters.ElementAtOrDefault(formula.ArgumentPosition);
                    il.Emit(OpCodes.Ldarg, parameter);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                if (formula.ArgumentPosition == 0)
                {
                    // NOTE: The pointer of 'this'.
                    il.Emit(OpCodes.Ldarg_0);
                }
                else if (-1 < formula.ArgumentPosition)
                {
                    parameter = MethodGenerator.Parameters.ElementAtOrDefault(formula.ArgumentPosition - 1);
                    il.Emit(OpCodes.Ldarg, parameter);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public override void Visit(TypeAsFormula formula)
        {
            base.Visit(formula);
            il.Emit(OpCodes.Isinst, formula.TypeDeclaration);
            if (formula.TypeDeclaration.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, formula.TypeDeclaration);
            }
        }

        protected override void VisitConvertOperandCore(ConvertFormula formula, Formula operand)
        {
            var success = false;
            var expectedType = formula.TypeDeclaration;

            if (expectedType.IsValueType)
            {
                if (expectedType.EqualsWithoutGenericArguments(typeof(Nullable<>).ToTypeDecl()))
                {
                    var getValueOrDefault = expectedType.Methods.First(_ => _.Name == "GetValueOrDefault");
                    var local = il.AddLocal(expectedType);
                    il.Emit(OpCodes.Stloc, local);
                    il.Emit(OpCodes.Ldloca, local);
                    il.Emit(OpCodes.Call, getValueOrDefault);
                    success = true;
                }
            }

            base.VisitConvertOperandCore(formula, operand);

            if (expectedType.IsValueType)
            {
                if (operand.TypeDeclaration.EqualsWithoutGenericArguments(typeof(Nullable<>).ToTypeDecl()))
                {
                    var getValue = operand.TypeDeclaration.Properties.First(_ => _.Name == "Value").GetMethod;
                    var local = il.AddLocal(operand.TypeDeclaration);
                    il.Emit(OpCodes.Stloc, local);
                    il.Emit(OpCodes.Ldloca, local);
                    il.Emit(OpCodes.Call, getValue);
                    success = true;
                }
            }
            else if (operand.TypeDeclaration.IsValueType && !expectedType.IsValueType)
            {
                il.Emit(OpCodes.Box, operand.TypeDeclaration);
                success = true;
            }
            else if (operand.TypeDeclaration.IsAssignableExplicitlyFrom(expectedType))
            {
                il.Emit(OpCodes.Castclass, expectedType);
                success = true;
            }

            if (!success)
            {
                throw new NotImplementedException();
            }
        }

        //public override void Visit(ConvertFormula formula)
        //{
        //    base.Visit(formula);
        //    var operand = formula.Operand;
        //    var expectedType = formula.TypeDeclaration;
        //    if (expectedType.IsValueType)
        //    {
        //        if (operand.TypeDeclaration.EqualsWithoutGenericArguments(typeof(Nullable<>).ToTypeDecl()))
        //        {
        //            var getValue = operand.TypeDeclaration.Properties.First(_ => _.Name == "Value").GetMethod;
        //            var local = il.AddLocal(operand.TypeDeclaration);
        //            il.Emit(OpCodes.Stloc, local);
        //            il.Emit(OpCodes.Ldloca, local);
        //            il.Emit(OpCodes.Call, getValue);
        //        }
        //        else if (expectedType.EqualsWithoutGenericArguments(typeof(Nullable<>).ToTypeDecl()))
        //        {
        //            var getValueOrDefault = expectedType.Methods.First(_ => _.Name == "GetValueOrDefault");
        //            var local = il.AddLocal(operand.TypeDeclaration);
        //            il.Emit(OpCodes.Stloc, local);
        //            il.Emit(OpCodes.Ldloca, local);
        //            il.Emit(OpCodes.Call, getValueOrDefault);
        //        }
        //        else
        //        {
        //            throw new NotImplementedException();
        //            //var conv = operand.TypeDeclaration.Methods.
        //            //                    Where(_ => _.Name == "op_Explicit" || _.Name == "op_Implicit").
        //            //                    Where(_ => expectedType.IsAssignableExplicitlyFrom(_.ReturnType)).
        //            //                    Where(_ => _.Parameters.Any(__ => __.ParameterType.IsAssignableExplicitlyFrom(operand.TypeDeclaration))).
        //            //                    FirstOrDefault();
        //            //if (conv == null)
        //            //{
        //            //    conv = expectedType.Methods.
        //            //                        Where(_ => _.Name == "op_Explicit" || _.Name == "op_Implicit").
        //            //                        Where(_ => expectedType.IsAssignableExplicitlyFrom(_.ReturnType)).
        //            //                        Where(_ => _.Parameters.Any(__ => __.ParameterType.IsAssignableExplicitlyFrom(operand.TypeDeclaration))).
        //            //                        FirstOrDefault();
        //            //}
        //            //var local = il.AddLocal(operand.TypeDeclaration);
        //            //il.Emit(OpCodes.Call, conv);
        //        }
        //    }
        //    else if (operand.TypeDeclaration.IsValueType && !expectedType.IsValueType)
        //    {
        //        il.Emit(OpCodes.Box, operand.TypeDeclaration);
        //    }
        //    else
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        protected override void VisitConditionalTestCore(ConditionalFormula formula, Formula test)
        {
            base.VisitConditionalTestCore(formula, test);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Ceq);
            var label = il.AddLabel();
            il.Emit(OpCodes.Brfalse, label);
            formula.Label1 = label;
        }

        protected override void VisitConditionalIfTrueCore(ConditionalFormula formula, Formula ifTrue)
        {
            base.VisitConditionalIfTrueCore(formula, ifTrue);
            var label = il.AddLabel();
            il.Emit(OpCodes.Br, label);
            formula.Label2 = label;
        }

        protected override void VisitConditionalIfFalseCore(ConditionalFormula formula, Formula ifFalse)
        {
            il.SetLabel(formula.Label1);
            base.VisitConditionalIfFalseCore(formula, ifFalse);
            il.Emit(OpCodes.Br, formula.Label2);
        }

        public override void Visit(ConditionalFormula formula)
        {
            base.Visit(formula);
            if (formula.IfFalse == null)
            {
                il.SetLabel(formula.Label1);
            }
            il.SetLabel(formula.Label2);
        }

        public override void Visit(EqualFormula formula)
        {
            base.Visit(formula);
            il.Emit(OpCodes.Ceq);
        }

        public override void Visit(NotEqualFormula formula)
        {
            base.Visit(formula);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Ceq);
        }

        protected override void VisitAndAlsoLeftCore(AndAlsoFormula formula, Formula left)
        {
            base.VisitAndAlsoLeftCore(formula, left);
            il.Emit(OpCodes.Dup);
            var label = il.AddLabel();
            il.Emit(OpCodes.Brfalse, label);
            formula.Label = label;
        }

        protected override void VisitAndAlsoRightCore(AndAlsoFormula formula, Formula right)
        {
            base.VisitAndAlsoRightCore(formula, right);
            il.Emit(OpCodes.Ceq);
            il.SetLabel(formula.Label);
        }

        public override void Visit(CallFormula formula)
        {
            base.Visit(formula);
            if (formula.Method.IsStatic || formula.Method.DeclaringType.IsValueType)
            {
                il.Emit(OpCodes.Call, formula.Method);
            }
            else
            {
                il.Emit(OpCodes.Callvirt, formula.Method);
            }
        }

        public override void Visit(InvokeFormula formula)
        {
            base.Visit(formula);
            il.Emit(OpCodes.Call, formula.Method);
        }

        public override void Visit(MethodPtrFormula formula)
        {
            base.Visit(formula);
            il.Emit(OpCodes.Ldftn, formula.Method);
        }

        public override void Visit(NewFormula formula)
        {
            base.Visit(formula);
            if (formula.NodeType != NodeType.BaseNew)
            {
                il.Emit(OpCodes.Newobj, formula.Constructor);
            }
        }

        protected override void VisitNewArrayInitFormulasCore(NewArrayInitFormula formula, FormulaCollection<Formula> formulas)
        {
            il.Emit(OpCodes.Ldc_I4_S, (sbyte)formulas.Count);
            il.Emit(OpCodes.Newarr, formula.TypeDeclaration.GetElementType());
            var arr = il.AddLocal(formula.TypeDeclaration);
            il.Emit(OpCodes.Stloc, arr);
            formulas.ForEach((_formula, index) =>
            {
                il.Emit(OpCodes.Ldloc, arr);
                il.Emit(OpCodes.Ldc_I4, index);
                _formula.Accept(this);
                if (!_formula.TypeDeclaration.IsValueType)
                {
                    il.Emit(OpCodes.Stelem_Ref);
                }
                else
                {
                    throw new NotImplementedException();
                }
            });
            il.Emit(OpCodes.Ldloc, arr);
        }

        public override void Visit(PropertyFormula formula)
        {
            base.Visit(formula);
            if (formula.Member.GetMethod.IsStatic || formula.Member.DeclaringType.IsValueType)
            {
                il.Emit(OpCodes.Call, formula.Member.GetMethod);
            }
            else
            {
                il.Emit(OpCodes.Callvirt, formula.Member.GetMethod);
            }
        }

        public override void Visit(FieldFormula formula)
        {
            base.Visit(formula);
            if (formula.Member.IsStatic)
            {
                il.Emit(OpCodes.Ldsfld, formula.Member);
            }
            else
            {
                il.Emit(OpCodes.Ldfld, formula.Member);
            }
        }
    }
}
