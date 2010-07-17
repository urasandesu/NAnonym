using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MC = Mono.Cecil;
using System.Reflection;
using System.Reflection.Emit;
using SR = System.Reflection;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;

namespace Urasandesu.NAnonym.CREUtilities
{
    public sealed class ExpressiveILProcessor
    {
        readonly MethodDefinition methodDef;
        readonly ILProcessor ilProcessor;

        readonly MethodInfo AddlocDefinition;
        readonly MethodInfo LdlocDefinition;

        readonly Stack EvaluationStack = new Stack();

        public ExpressiveILProcessor(MethodDefinition methodDef)
        {
            this.methodDef = methodDef;
            this.ilProcessor = methodDef.Body.GetILProcessor();
            AddlocDefinition = ExpressiveType.GetMethodInfo<object, object>(() => Addloc).GetGenericMethodDefinition();
            LdlocDefinition = ExpressiveType.GetMethodInfo<object, object>(() => Ldloc).GetGenericMethodDefinition();
        }

        public void Emit(Expression<Action<ExpressiveILProcessor>> exp)
        {
            Emit(exp.Body);
        }

        // HACK: Visit じゃなくてやっぱり Emit だなー。
        // HACK: Emit せずに値を取り出す Evaluate も欲しー。
        void Emit(ReadOnlyCollection<Expression> exps)
        {
            foreach (var exp in exps)
            {
                Emit(exp);
            }
        }

        void Emit(Expression exp)
        {
            if (exp == null) return;

            switch (exp.NodeType)
            {
                case ExpressionType.Add:
                    break;
                case ExpressionType.AddChecked:
                    break;
                case ExpressionType.And:
                    break;
                case ExpressionType.AndAlso:
                    break;
                case ExpressionType.ArrayIndex:
                    break;
                case ExpressionType.ArrayLength:
                    break;
                case ExpressionType.Call:
                    Emit((MethodCallExpression)exp);
                    break;
                case ExpressionType.Coalesce:
                    break;
                case ExpressionType.Conditional:
                    break;
                case ExpressionType.Constant:
                    Emit((ConstantExpression)exp);
                    break;
                case ExpressionType.Convert:
                    break;
                case ExpressionType.ConvertChecked:
                    break;
                case ExpressionType.Divide:
                    break;
                case ExpressionType.Equal:
                    break;
                case ExpressionType.ExclusiveOr:
                    break;
                case ExpressionType.GreaterThan:
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    break;
                case ExpressionType.Invoke:
                    break;
                case ExpressionType.Lambda:
                    break;
                case ExpressionType.LeftShift:
                    break;
                case ExpressionType.LessThan:
                    break;
                case ExpressionType.LessThanOrEqual:
                    break;
                case ExpressionType.ListInit:
                    break;
                case ExpressionType.MemberAccess:
                    break;
                case ExpressionType.MemberInit:
                    break;
                case ExpressionType.Modulo:
                    break;
                case ExpressionType.Multiply:
                    break;
                case ExpressionType.MultiplyChecked:
                    break;
                case ExpressionType.Negate:
                    break;
                case ExpressionType.NegateChecked:
                    break;
                case ExpressionType.New:
                    Emit((NewExpression)exp);
                    break;
                case ExpressionType.NewArrayBounds:
                    break;
                case ExpressionType.NewArrayInit:
                    break;
                case ExpressionType.Not:
                    break;
                case ExpressionType.NotEqual:
                    break;
                case ExpressionType.Or:
                    break;
                case ExpressionType.OrElse:
                    break;
                case ExpressionType.Parameter:
                    Emit((ParameterExpression)exp);
                    break;
                case ExpressionType.Power:
                    break;
                case ExpressionType.Quote:
                    break;
                case ExpressionType.RightShift:
                    break;
                case ExpressionType.Subtract:
                    break;
                case ExpressionType.SubtractChecked:
                    break;
                case ExpressionType.TypeAs:
                    break;
                case ExpressionType.TypeIs:
                    break;
                case ExpressionType.UnaryPlus:
                    break;
                default:
                    break;
            }
        }


        bool IsAddloc(MethodInfo target)
        {
            if (target.Name == AddlocDefinition.Name && target == AddlocDefinition.MakeGenericMethod(target.GetGenericArguments()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool IsLdloc(MethodInfo target)
        {
            if (target.Name == LdlocDefinition.Name && target == LdlocDefinition.MakeGenericMethod(target.GetGenericArguments()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void Emit(MethodCallExpression exp)
        {
            // 評価の順番は、Object -> Arguments -> Method。
            if (exp.Object == null)
            {
                // static method call
                Emit(exp.Arguments);
                Direct.Emit(Mono.Cecil.Cil.OpCodes.Call, methodDef.Module.Import(exp.Method));
            }
            else
            {
                // instance method call
                Emit(exp.Object);

                // 先読み（or 後読み）が必要になる？
                // TODO: Last とは限らない。
                // TODO: exp.Object で Dictionary 引っ張ればいいのか？
                //       → 状態変数はなるべくもたせたくないんだが…。
                //       → Emit にも戻り値付ければ解決できる？
                //       → 評価スタックに最後に積まれた Instruction 返すようにすると便利かも。
                var lastInstruction = methodDef.Body.Instructions.LastOrDefault();
                if (lastInstruction != null && lastInstruction.OpCode == MC.Cil.OpCodes.Ldloc)
                {
                    var variable = (VariableDefinition)lastInstruction.Operand;
                    if (variable.VariableType.IsValueType)
                    {
                        Direct.Replace(lastInstruction, Instruction.Create(MC.Cil.OpCodes.Ldloca, (VariableDefinition)lastInstruction.Operand));
                    }
                }

                if (IsAddloc(exp.Method))
                {
                    Emit(exp.Arguments[1]);
                    var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
                    var variable = new VariableDefinition(fieldInfo.Name, methodDef.Module.Import(fieldInfo.FieldType));
                    methodDef.Body.Variables.Add(variable);
                    methodDef.Body.InitLocals = true;
                    Direct.Emit(MC.Cil.OpCodes.Stloc, variable);
                }
                else if (IsLdloc(exp.Method))
                {
                    var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
                    var variable = methodDef.Body.Variables.First(_variable => _variable.Name == fieldInfo.Name);
                    Direct.Emit(MC.Cil.OpCodes.Ldloc, variable);
                }
                else
                {
                    Emit(exp.Arguments);

                    Direct.Emit(MC.Cil.OpCodes.Callvirt, methodDef.Module.Import(exp.Method));
                }
            }
        }

        void Emit(ConstantExpression exp)
        {
            if (exp.Value.GetType() == typeof(string))
            {
                Direct.Emit(Mono.Cecil.Cil.OpCodes.Ldstr, (string)exp.Value);
            }
            else if (exp.Value.GetType() == typeof(int))
            {
                // HACK: _S が付く命令は短い形式。-127 ～ 128 以外は最適化が必要。
                Direct.Emit(MC.Cil.OpCodes.Ldc_I4_S, (sbyte)(int)exp.Value);
            }
            else if (exp.Value.GetType() == typeof(char))
            {
                Direct.Emit(MC.Cil.OpCodes.Ldc_I4_S, (sbyte)(char)exp.Value);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        void Emit(NewExpression exp)
        {
            Emit(exp.Arguments);
            Direct.Emit(MC.Cil.OpCodes.Newobj, methodDef.Module.Import(exp.Constructor));
            if (exp.Members != null)
            {
                //// 使わなくても構築は問題なくできそう？
                //// → 匿名型の初期化では見た目と異なり、コンストラクタの引数で設定するよう変更されている。
                //var variable = new VariableDefinition(methodDef.Module.Import(exp.Constructor.DeclaringType));
                //Direct.Emit(MC.Cil.OpCodes.Stloc, variable);
                //foreach (var member in exp.Members)
                //{
                //    Direct.Emit(MC.Cil.OpCodes.Ldloc, variable);
                //}
            }
        }

        void Emit(ParameterExpression exp)
        {
            if (exp.Type == typeof(ExpressiveILProcessor))
            {
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public ILProcessor Direct
        {
            get
            {
                return ilProcessor;
            }
        }

        public VariableDefinition Locals<T>(Expression<Func<T>> variable)
        {
            throw new NotImplementedException();
        }

        Hashtable LocalVariablesHashtable = new Hashtable();

        // TODO: そもそも式木の中ならすでに遅延評価されてるんじゃん。↓でいいんじゃね？
        // TODO: 行けた！こちら側に I/F を統一！
        public void Addloc<T>(T variable, T value)
        {
            // MEMO: 式木の中でしか利用できないダミーメソッド
            throw new NotSupportedException("This method can use only in Expression Tree.");
        }

        public T Ldloc<T>(T variable)
        {
            // MEMO: 式木の中でしか利用できないダミーメソッド
            throw new NotSupportedException("This method can use only in Expression Tree.");
        }

        public T Stloc<T>(T variable, T value)
        {
            // MEMO: 式木の中でしか利用できないダミーメソッド
            throw new NotSupportedException("This method can use only in Expression Tree.");
        }
    }
}
