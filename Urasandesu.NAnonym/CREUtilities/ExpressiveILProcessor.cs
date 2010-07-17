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
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.CREUtilities
{
    public sealed class ExpressiveEvaluable
    {
        readonly MethodInfo AddlocInfo;
        readonly MethodInfo StlocInfo;

        public ExpressiveEvaluable()
        {
            AddlocInfo = ExpressiveType.GetMethodInfo<object>(() => Addloc).GetGenericMethodDefinition();
            StlocInfo = ExpressiveType.GetMethodInfo<object, object>(() => Stloc).GetGenericMethodDefinition();
        }

        public bool IsAddloc(MethodInfo target)
        {
            if (target.Name == AddlocInfo.Name && target == AddlocInfo.MakeGenericMethod(target.GetGenericArguments()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsStloc(MethodInfo target)
        {
            if (target.Name == StlocInfo.Name && target == StlocInfo.MakeGenericMethod(target.GetGenericArguments()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Addloc<T>(T variable)
        {
        }

        public T Stloc<T>(T variable)
        {
            return default(T);
        }
    }

    public sealed class ExpressiveEmittable
    {
        readonly MethodInfo AddlocInfo;
        readonly MethodInfo StlocInfo;
        readonly MethodInfo DupAddOneInfo;
        readonly MethodInfo AddOneDupInfo;
        readonly MethodInfo SubOneDupInfo;
        readonly MethodInfo ReturnInfo;

        public ExpressiveEmittable()
        {
            AddlocInfo = ExpressiveType.GetMethodInfo<object, object>(() => Addloc).GetGenericMethodDefinition();
            StlocInfo = ExpressiveType.GetMethodInfo<object, object, object>(() => Stloc).GetGenericMethodDefinition();
            DupAddOneInfo = ExpressiveType.GetMethodInfo<object, object>(() => DupAddOne).GetGenericMethodDefinition();
            AddOneDupInfo = ExpressiveType.GetMethodInfo<object, object>(() => AddOneDup).GetGenericMethodDefinition();
            SubOneDupInfo = ExpressiveType.GetMethodInfo<object, object>(() => SubOneDup).GetGenericMethodDefinition();
            ReturnInfo = ExpressiveType.GetMethodInfo<object>(() => Return).GetGenericMethodDefinition();
        }

        public bool IsAddloc(MethodInfo target)
        {
            if (target.Name == AddlocInfo.Name && target == AddlocInfo.MakeGenericMethod(target.GetGenericArguments()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsStloc(MethodInfo target)
        {
            if (target.Name == StlocInfo.Name && target == StlocInfo.MakeGenericMethod(target.GetGenericArguments()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsDupAddOne(MethodInfo target)
        {
            if (target.Name == DupAddOneInfo.Name && target == DupAddOneInfo.MakeGenericMethod(target.GetGenericArguments()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsAddOneDup(MethodInfo target)
        {
            if (target.Name == AddOneDupInfo.Name && target == AddOneDupInfo.MakeGenericMethod(target.GetGenericArguments()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsSubOneDup(MethodInfo target)
        {
            if (target.Name == SubOneDupInfo.Name && target == SubOneDupInfo.MakeGenericMethod(target.GetGenericArguments()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsReturn(MethodInfo target)
        {
            if (target.Name == ReturnInfo.Name && target == ReturnInfo.MakeGenericMethod(target.GetGenericArguments()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Addloc<T>(T variable, T value)
        {
        }

        public T Stloc<T>(T variable, T value)
        {
            return default(T);
        }

        public T DupAddOne<T>(T variable)
        {
            return default(T);
        }

        public T AddOneDup<T>(T variable)
        {
            return default(T);
        }

        public T SubOneDup<T>(T variable)
        {
            return default(T);
        }

        public void Return<T>(T variable)
        {
        }
    }

    // TODO: IDisposable にして、using 内で利用するようにしたほうがそれっぽい。
    public sealed class ExpressiveILProcessor
    {
        readonly MethodDefinition methodDef;
        readonly ILProcessor ilProcessor;

        readonly ExpressiveEvaluable evaluable;
        readonly ExpressiveEmittable emittable;

        readonly MethodInfo GetTypeFromHandle;

        public ExpressiveILProcessor(MethodDefinition methodDef)
        {
            this.methodDef = methodDef;
            this.methodDef.Body.InitLocals = true;
            this.ilProcessor = methodDef.Body.GetILProcessor();

            evaluable = new ExpressiveEvaluable();
            emittable = new ExpressiveEmittable();

            GetTypeFromHandle = 
                typeof(System.Type).GetMethod(
                    "GetTypeFromHandle",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    new System.Type[] 
                    {
                        typeof(RuntimeTypeHandle) 
                    },
                    null);
        }

        // MEMO: このパターンが出てきたら、value に変えちゃうよ、って感じで。
        public void Eval(Expression<Action<ExpressiveEvaluable>> exp, object value)
        {
            // TODO: 別の MethodDefinition に Instruction 作成していって、比較に使えば良い。
            // TODO: Seek するためのポインタが必要。
        }

        void Eval(ReadOnlyCollection<Expression> exps)
        {
            foreach (var exp in exps)
            {
                Eval(exp);
            }
        }

        void Eval(Expression exp)
        {
            throw new NotImplementedException();
        }

        public void Emit(Expression<Action<ExpressiveEmittable>> exp)
        {
            Emit(exp.Body);
            if (exp.Body.Type != typeof(void))
            {
                // NOTE: void ではないということは評価スタックに情報が残っているということ。
                //       pop するのは、基本的に 1 回の Emit(Expression<Action<ExpressiveILProcessor>>) で完結するようにしたいため。
                Direct.Emit(MC.Cil.OpCodes.Pop);
            }
        }

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
                case ExpressionType.ArrayIndex:
                case ExpressionType.Multiply:
                    Emit((BinaryExpression)exp);
                    return;
                case ExpressionType.AddChecked:
                    break;
                case ExpressionType.And:
                    break;
                case ExpressionType.AndAlso:
                    break;
                case ExpressionType.Call:
                    Emit((MethodCallExpression)exp);
                    return;
                case ExpressionType.Coalesce:
                    break;
                case ExpressionType.Conditional:
                    break;
                case ExpressionType.Constant:
                    Emit((ConstantExpression)exp);
                    return;
                case ExpressionType.ArrayLength:
                case ExpressionType.Convert:
                    Emit((UnaryExpression)exp);
                    return;
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
                    Emit((MemberExpression)exp);
                    return;
                case ExpressionType.MemberInit:
                    break;
                case ExpressionType.Modulo:
                    break;
                case ExpressionType.MultiplyChecked:
                    break;
                case ExpressionType.Negate:
                    break;
                case ExpressionType.NegateChecked:
                    break;
                case ExpressionType.New:
                    Emit((NewExpression)exp);
                    return;
                case ExpressionType.NewArrayBounds:
                    break;
                case ExpressionType.NewArrayInit:
                    Emit((NewArrayExpression)exp);
                    return;
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
                    return;
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
            throw new NotImplementedException();
        }

        void Emit(BinaryExpression exp)
        {
            Emit(exp.Left);
            Emit(exp.Right);

            // TODO: ?? 演算子とか演算子のオーバーロードとか。
            if (exp.Conversion != null) throw new NotImplementedException();
            if (exp.Method != null) throw new NotImplementedException();

            if (exp.NodeType == ExpressionType.Coalesce)
            {
                throw new NotImplementedException();
            }
            else if (exp.NodeType == ExpressionType.Add)
            {
                if (exp.Left.Type == typeof(int) && exp.Left.Type == typeof(int))
                {
                    Direct.Emit(MC.Cil.OpCodes.Add);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else if (exp.NodeType == ExpressionType.Multiply)
            {
                if (exp.Left.Type == typeof(int) && exp.Left.Type == typeof(int))
                {
                    Direct.Emit(MC.Cil.OpCodes.Mul);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else if (exp.NodeType == ExpressionType.ArrayIndex)
            {
                if (typeof(double).IsAssignableFrom(exp.Left.Type.GetElementType()))
                {
                    throw new NotImplementedException();
                }
                else
                {
                    Direct.Emit(MC.Cil.OpCodes.Ldelem_Ref);
                }
            }
            else
            {
                throw new NotImplementedException();
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
                if (exp.Object.Type == typeof(ExpressiveEmittable))
                {
                    if (emittable.IsAddloc(exp.Method))
                    {
                        Emit(exp.Arguments[1]);
                        var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
                        var variable = new VariableDefinition(fieldInfo.Name, methodDef.Module.Import(fieldInfo.FieldType));
                        methodDef.Body.Variables.Add(variable);
                        Direct.Emit(MC.Cil.OpCodes.Stloc, variable);
                    }
                    else if (emittable.IsDupAddOne(exp.Method))
                    {
                        var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
                        var variable = methodDef.Body.Variables.First(_variable => _variable.Name == fieldInfo.Name);
                        Direct.Emit(MC.Cil.OpCodes.Ldloc, variable);
                        Direct.Emit(MC.Cil.OpCodes.Dup);
                        Direct.Emit(MC.Cil.OpCodes.Ldc_I4_1);
                        Direct.Emit(MC.Cil.OpCodes.Add);
                        Direct.Emit(MC.Cil.OpCodes.Stloc, variable);
                    }
                    else if (emittable.IsAddOneDup(exp.Method))
                    {
                        var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
                        var variable = methodDef.Body.Variables.First(_variable => _variable.Name == fieldInfo.Name);
                        Direct.Emit(MC.Cil.OpCodes.Ldloc, variable);
                        Direct.Emit(MC.Cil.OpCodes.Ldc_I4_1);
                        Direct.Emit(MC.Cil.OpCodes.Add);
                        Direct.Emit(MC.Cil.OpCodes.Dup);
                        Direct.Emit(MC.Cil.OpCodes.Stloc, variable);
                    }
                    else if (emittable.IsSubOneDup(exp.Method))
                    {
                        var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
                        var variable = methodDef.Body.Variables.First(_variable => _variable.Name == fieldInfo.Name);
                        Direct.Emit(MC.Cil.OpCodes.Ldloc, variable);
                        Direct.Emit(MC.Cil.OpCodes.Ldc_I4_1);
                        Direct.Emit(MC.Cil.OpCodes.Sub);
                        Direct.Emit(MC.Cil.OpCodes.Dup);
                        Direct.Emit(MC.Cil.OpCodes.Stloc, variable);
                    }
                    else if (emittable.IsReturn(exp.Method))
                    {
                        Emit(exp.Arguments[0]);
                        Direct.Emit(MC.Cil.OpCodes.Ret);
                    }
                    //else if (IsEval(exp.Method))
                    //{
                    //    Emit(Expression.Constant(Evaluate(exp.Arguments[0])));
                    //}
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    // instance method call
                    Emit(exp.Object);
                    if (exp.Object.Type.IsValueType)
                    {
                        // NOTE: 値型のメソッドを呼び出すには、アドレスへの変換が必要。
                        var variable = new VariableDefinition(methodDef.Module.Import(exp.Object.Type));
                        methodDef.Body.Variables.Add(variable);
                        Direct.Emit(MC.Cil.OpCodes.Stloc, variable);
                        Direct.Emit(MC.Cil.OpCodes.Ldloca, variable);
                    }

                    Emit(exp.Arguments);

                    Direct.Emit(MC.Cil.OpCodes.Callvirt, methodDef.Module.Import(exp.Method));
                }
            }
        }

        void Emit(ConstantExpression exp)
        {
            string s = default(string);
            int? ni = default(int?);
            double? nd = default(double?);
            char? nc = default(char?);
            bool? nb = default(bool?);
            var e = default(Enum);
            var t = default(Type);
            if (exp.Value == null)
            {
                Direct.Emit(MC.Cil.OpCodes.Ldnull);
            }
            else if ((s = exp.Value as string) != null)
            {
                Direct.Emit(MC.Cil.OpCodes.Ldstr, s);
            }
            else if ((ni = exp.Value as int?) != null)
            {
                // HACK: _S が付く命令は短い形式。-127 ～ 128 以外は最適化が必要。
                Direct.Emit(MC.Cil.OpCodes.Ldc_I4_S, (sbyte)(int)ni);
            }
            else if ((nd = exp.Value as double?) != null)
            {
                Direct.Emit(MC.Cil.OpCodes.Ldc_R8, (double)nd);
            }
            else if ((nc = exp.Value as char?) != null)
            {
                Direct.Emit(MC.Cil.OpCodes.Ldc_I4_S, (sbyte)(char)nc);
            }
            else if ((nb = exp.Value as bool?) != null)
            {
                Direct.Emit(MC.Cil.OpCodes.Ldc_I4, (bool)nb ? 1 : 0);
            }
            else if ((e = exp.Value as Enum) != null)
            {
                Direct.Emit(MC.Cil.OpCodes.Ldc_I4, e.GetHashCode());
            }
            else if ((t = exp.Value as Type) != null)
            {
                Direct.Emit(MC.Cil.OpCodes.Ldtoken, methodDef.Module.Import(t));
                Direct.Emit(MC.Cil.OpCodes.Call, methodDef.Module.Import(GetTypeFromHandle));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        void Emit(UnaryExpression exp)
        {
            Emit(exp.Operand);
            switch (exp.NodeType)
            {
                case ExpressionType.Convert:
                    if (exp.Type == typeof(int))
                    {
                        Direct.Emit(MC.Cil.OpCodes.Conv_I4);
                    }
                    else if (typeof(double).IsAssignableFrom(exp.Type))
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        Direct.Emit(MC.Cil.OpCodes.Box, methodDef.Module.Import(exp.Operand.Type));
                    }
                    break;
                case ExpressionType.ArrayLength:
                    Direct.Emit(MC.Cil.OpCodes.Ldlen);
                    Direct.Emit(MC.Cil.OpCodes.Conv_I4);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        void Emit(MemberExpression exp)
        {
            var fieldInfo = default(FieldInfo);
            var propertyInfo = default(PropertyInfo);
            if ((fieldInfo = exp.Member as FieldInfo) != null)
            {
                var variable = default(VariableDefinition);
                var parameter = default(ParameterDefinition);
                if ((variable = methodDef.Body.Variables.FirstOrDefault(_variable => _variable.Name == fieldInfo.Name)) != null)
                {
                    Direct.Emit(MC.Cil.OpCodes.Ldloc, variable);
                }
                else if ((parameter = methodDef.Parameters.FirstOrDefault(_parameter => _parameter.Name == fieldInfo.Name)) != null)
                {
                    Direct.Emit(MC.Cil.OpCodes.Ldarg, parameter);
                }
                else
	            {
                    Emit(exp.Expression);

                    if (fieldInfo.IsStatic)
                    {
                        Direct.Emit(MC.Cil.OpCodes.Ldsfld, methodDef.Module.Import(fieldInfo));
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
            else if ((propertyInfo = exp.Member as PropertyInfo) != null)
            {
                Emit(exp.Expression);

                if (propertyInfo.IsStatic())
                {
                    throw new NotImplementedException();
                }
                else
                {
                    Direct.Emit(MC.Cil.OpCodes.Callvirt, methodDef.Module.Import(propertyInfo.GetGetMethod()));
                }
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

        void Emit(NewArrayExpression exp)
        {
            if (exp.NodeType == ExpressionType.NewArrayInit)
            {
                Direct.Emit(MC.Cil.OpCodes.Ldc_I4_S, (sbyte)exp.Expressions.Count);
                Direct.Emit(MC.Cil.OpCodes.Newarr, methodDef.Module.Import(exp.Type.GetElementType()));
                var variable = new VariableDefinition(methodDef.Module.Import(exp.Type));
                methodDef.Body.Variables.Add(variable);
                Direct.Emit(MC.Cil.OpCodes.Stloc, variable);
                exp.Expressions
                    .ForEach((_exp, index) =>
                {
                    Direct.Emit(MC.Cil.OpCodes.Ldloc, variable);
                    Direct.Emit(MC.Cil.OpCodes.Ldc_I4, index);
                    Emit(_exp);
                    if (typeof(double).IsAssignableFrom(_exp.Type))
                    {
                        throw new NotImplementedException();
                    }
                    else
	                {
                        Direct.Emit(MC.Cil.OpCodes.Stelem_Ref);
	                }
                });
                Direct.Emit(MC.Cil.OpCodes.Ldloc, variable);
            }
            else
            {
                throw new NotImplementedException();
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

        object Evaluate(Expression exp)
        {
            if (exp == null) return null;

            switch (exp.NodeType)
            {
                case ExpressionType.Add:
                    return Evaluate((BinaryExpression)exp);
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
                    break;
                case ExpressionType.Coalesce:
                    break;
                case ExpressionType.Conditional:
                    break;
                case ExpressionType.Constant:
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
                    return Evaluate((MemberExpression)exp);
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
            throw new NotImplementedException();
        }

        object Evaluate(BinaryExpression exp)
        {
            object left = Evaluate(exp.Left);
            object right = Evaluate(exp.Right);

            // TODO: ?? 演算子とか演算子のオーバーロードとか。
            if (exp.Conversion != null) throw new NotImplementedException();
            if (exp.Method != null) throw new NotImplementedException();

            if (exp.NodeType == ExpressionType.Coalesce)
            {
                throw new NotImplementedException();
            }
            else if (exp.NodeType == ExpressionType.Add)
            {
                int? niLeft = default(int?);
                int? niRight = default(int?);
                if ((niLeft = left as int?) != null && (niRight = right as int?) != null)
                {
                    return (int)niLeft + (int)niRight;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        object Evaluate(MemberExpression exp)
        {
            var fieldInfo = default(FieldInfo);
            var propertyInfo = default(PropertyInfo);
            if ((fieldInfo = exp.Member as FieldInfo) != null)
            {
                throw new NotImplementedException();
            }
            else if ((propertyInfo = exp.Member as PropertyInfo) != null)
            {
                throw new NotImplementedException();
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
    }
}
