using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools.Impl.System.Reflection;
using SR = System.Reflection;
using Urasandesu.NAnonym.Linq;
using Urasandesu.NAnonym.ILTools.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{
    public class ExpressiveMethodBodyGenerator : IMethodBodyGenerator
    {
        static readonly Expressible expressible = new Expressible();
        static readonly MethodInfo GetTypeFromHandle = typeof(Type).GetMethod(
                                                            "GetTypeFromHandle",
                                                            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                                                            null,
                                                            new System.Type[] 
                                                            {
                                                                typeof(RuntimeTypeHandle) 
                                                            },
                                                            null);

        
        readonly IMethodBaseGenerator methodGen;
        readonly EvalState state;

        internal ExpressiveMethodBodyGenerator(IMethodBaseGenerator methodGen)
        {
            this.methodGen = methodGen;
            state = new EvalState();
        }

        public void Eval(Expression<Action<Expressible>> exp)
        {
            EvalExpression(methodGen, exp.Body, state);
            if (exp.Body.Type != typeof(void) && !state.NoPop)
            {
                // NOTE: void ではないということは評価スタックに情報が残っているということ。
                //       pop するのは、基本的に 1 回の Emit(Expression<Action<ExpressiveILProcessor>>) で完結するようにしたいため。
                methodGen.Body.ILOperator.Emit(OpCodes.Pop);
            }
        }

        static void EvalArguments(IMethodBaseGenerator methodGen, ReadOnlyCollection<Expression> exps, EvalState state)
        {
            foreach (var exp in exps)
            {
                EvalExpression(methodGen, exp, state);
            }
        }

        static void EvalExpression(IMethodBaseGenerator methodGen, Expression exp, EvalState state)
        {
            state.NoPop = false;
            if (exp == null) return;

            switch (exp.NodeType)
            {
                case ExpressionType.Add:
                case ExpressionType.ArrayIndex:
                case ExpressionType.Multiply:
                case ExpressionType.Equal:
                    EvalBinary(methodGen, (BinaryExpression)exp, state);
                    return;
                case ExpressionType.AddChecked:
                    break;
                case ExpressionType.And:
                    break;
                case ExpressionType.AndAlso:
                    break;
                case ExpressionType.Call:
                    EvalMethodCall(methodGen, (MethodCallExpression)exp, state);
                    return;
                case ExpressionType.Coalesce:
                    break;
                case ExpressionType.Conditional:
                    break;
                case ExpressionType.Constant:
                    EvalConstant(methodGen, (ConstantExpression)exp, state);
                    return;
                case ExpressionType.ArrayLength:
                case ExpressionType.Convert:
                    EvalUnary(methodGen, (UnaryExpression)exp, state);
                    return;
                case ExpressionType.ConvertChecked:
                    break;
                case ExpressionType.Divide:
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
                    EvalMember(methodGen, (MemberExpression)exp, state);
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
                    EvalNew(methodGen, (NewExpression)exp, state);
                    return;
                case ExpressionType.NewArrayBounds:
                    break;
                case ExpressionType.NewArrayInit:
                    EvalNewArray(methodGen, (NewArrayExpression)exp, state);
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
                    EvalParameter(methodGen, (ParameterExpression)exp, state);
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

        static void EvalBinary(IMethodBaseGenerator methodGen, BinaryExpression exp, EvalState state)
        {
            EvalExpression(methodGen, exp.Left, state);
            EvalExpression(methodGen, exp.Right, state);

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
                    methodGen.Body.ILOperator.Emit(OpCodes.Add);
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
                    methodGen.Body.ILOperator.Emit(OpCodes.Mul);
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
                    methodGen.Body.ILOperator.Emit(OpCodes.Ldelem_Ref);
                }
            }
            else if (exp.NodeType == ExpressionType.Equal)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ceq);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        static void EvalMethodCall(IMethodBaseGenerator methodGen, MethodCallExpression exp, EvalState state)
        {
            // 評価の順番は、Object -> Arguments -> Method。
            if (exp.Object == null)
            {
                // static method call
                EvalArguments(methodGen, exp.Arguments, state);
                methodGen.Body.ILOperator.Emit(OpCodes.Call, exp.Method);
            }
            else
            {
                if (exp.Object.Type == typeof(Expressible))
                {
                    if (expressible.IsBase(exp.Method))
                    {
                        methodGen.Body.ILOperator.Emit(OpCodes.Ldarg_0);
                        var constructorDecl = methodGen.DeclaringType.BaseType.GetConstructor(new Type[] { });
                        methodGen.Body.ILOperator.Emit(OpCodes.Call, constructorDecl);
                    }
                    else if (expressible.IsAddloc(exp.Method))
                    {
                        // TODO: 同じ名前の変数は、同じ Scope 内であれば隠せる or 弾く工夫が必要
                        EvalExpression(methodGen, exp.Arguments[1], state);
                        var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
                        var local = methodGen.Body.ILOperator.AddLocal(fieldInfo.Name, fieldInfo.FieldType);
                        methodGen.Body.ILOperator.Emit(OpCodes.Stloc, local);
                    }
                    else if (expressible.IsStfld(exp.Method))
                    {
                        methodGen.Body.ILOperator.Emit(OpCodes.Ldarg_0);
                        EvalExpression(methodGen, exp.Arguments[1], state);
                        var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
                        var fieldDecl = methodGen.DeclaringType.GetField(fieldInfo.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        methodGen.Body.ILOperator.Emit(OpCodes.Stfld, fieldDecl);
                        state.NoPop = true;
                    }
                    else if (expressible.IsDupAddOne(exp.Method))
                    {
                        var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
                        var localDecl = methodGen.Body.Locals.First(_localDecl => _localDecl.Name == fieldInfo.Name);
                        methodGen.Body.ILOperator.Emit(OpCodes.Ldloc, localDecl);
                        methodGen.Body.ILOperator.Emit(OpCodes.Dup);
                        methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4_1);
                        methodGen.Body.ILOperator.Emit(OpCodes.Add);
                        methodGen.Body.ILOperator.Emit(OpCodes.Stloc, localDecl);
                    }
                    else if (expressible.IsAddOneDup(exp.Method))
                    {
                        var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
                        var localDecl = methodGen.Body.Locals.First(_localDecl => _localDecl.Name == fieldInfo.Name);
                        methodGen.Body.ILOperator.Emit(OpCodes.Ldloc, localDecl);
                        methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4_1);
                        methodGen.Body.ILOperator.Emit(OpCodes.Add);
                        methodGen.Body.ILOperator.Emit(OpCodes.Dup);
                        methodGen.Body.ILOperator.Emit(OpCodes.Stloc, localDecl);
                    }
                    else if (expressible.IsSubOneDup(exp.Method))
                    {
                        var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
                        var localDecl = methodGen.Body.Locals.First(_localDecl => _localDecl.Name == fieldInfo.Name);
                        methodGen.Body.ILOperator.Emit(OpCodes.Ldloc, localDecl);
                        methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4_1);
                        methodGen.Body.ILOperator.Emit(OpCodes.Sub);
                        methodGen.Body.ILOperator.Emit(OpCodes.Dup);
                        methodGen.Body.ILOperator.Emit(OpCodes.Stloc, localDecl);
                    }
                    else if (expressible.IsExpandAndLdarg(exp.Method))
                    {
                        var expanded = Expression.Lambda(exp.Arguments[0]).Compile();
                        string parameterName = (string)expanded.DynamicInvoke();
                        var parameterGen = methodGen.Parameters.First(_parameterGen => _parameterGen.Name == parameterName);
                        methodGen.Body.ILOperator.Emit(OpCodes.Ldarg, parameterGen);
                    }
                    else if (expressible.IsExpand(exp.Method))
                    {
                        var expanded = Expression.Lambda(exp.Arguments[0]).Compile();
                        EvalConstant(methodGen, Expression.Constant(expanded.DynamicInvoke()), state);
                    }
                    else if (expressible.IsReturn(exp.Method))
                    {
                        EvalExpression(methodGen, exp.Arguments[0], state);
                        methodGen.Body.ILOperator.Emit(OpCodes.Ret);
                    }
                    else if (expressible.IsEnd(exp.Method))
                    {
                        methodGen.Body.ILOperator.Emit(OpCodes.Ret);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    // instance method call
                    EvalExpression(methodGen, exp.Object, state);
                    if (exp.Object.Type.IsValueType)
                    {
                        // NOTE: 値型のメソッドを呼び出すには、アドレスへの変換が必要。
                        var local = methodGen.Body.ILOperator.AddLocal(exp.Object.Type);
                        methodGen.Body.ILOperator.Emit(OpCodes.Stloc, local);
                        methodGen.Body.ILOperator.Emit(OpCodes.Ldloca, local);
                    }

                    EvalArguments(methodGen, exp.Arguments, state);

                    methodGen.Body.ILOperator.Emit(OpCodes.Callvirt, exp.Method);
                }
            }
        }

        static void EvalConstant(IMethodBaseGenerator methodGen, ConstantExpression exp, EvalState state)
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
                methodGen.Body.ILOperator.Emit(OpCodes.Ldnull);
            }
            else if ((s = exp.Value as string) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldstr, s);
            }
            else if ((ni = exp.Value as int?) != null)
            {
                // HACK: _S が付く命令は短い形式。-127 ～ 128 以外は最適化が必要。
                methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4_S, (sbyte)(int)ni);
            }
            else if ((nd = exp.Value as double?) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldc_R8, (double)nd);
            }
            else if ((nc = exp.Value as char?) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4_S, (sbyte)(char)nc);
            }
            else if ((nb = exp.Value as bool?) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4, (bool)nb ? 1 : 0);
            }
            else if ((e = exp.Value as Enum) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4, e.GetHashCode());
            }
            else if ((t = exp.Value as Type) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldtoken, t);
                methodGen.Body.ILOperator.Emit(OpCodes.Call, GetTypeFromHandle);
            }
            else
            {
                // TODO: exp.Value がオブジェクト型の場合はどうするのだ？
                throw new NotImplementedException();
            }
        }

        static void EvalUnary(IMethodBaseGenerator methodGen, UnaryExpression exp, EvalState state)
        {
            EvalExpression(methodGen, exp.Operand, state);
            switch (exp.NodeType)
            {
                case ExpressionType.Convert:
                    if (exp.Type == typeof(int))
                    {
                        methodGen.Body.ILOperator.Emit(OpCodes.Conv_I4);
                    }
                    else if (typeof(double).IsAssignableFrom(exp.Type))
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        methodGen.Body.ILOperator.Emit(OpCodes.Box, exp.Operand.Type);
                    }
                    break;
                case ExpressionType.ArrayLength:
                    methodGen.Body.ILOperator.Emit(OpCodes.Ldlen);
                    methodGen.Body.ILOperator.Emit(OpCodes.Conv_I4);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        static void EvalMember(IMethodBaseGenerator methodGen, MemberExpression exp, EvalState state)
        {
            var fieldInfo = default(FieldInfo);
            var propertyInfo = default(PropertyInfo);
            if ((fieldInfo = exp.Member as FieldInfo) != null)
            {
                // 評価の優先順は、
                // 1. Addloc でローカル変数として追加したの（名前引き）
                // 2. メソッドの引数（名前引き）
                // 3. 定義時に使った変数そのもの
                // 4. その他
                var localDecl = default(ILocalGenerator);
                var parameterGen = default(IParameterGenerator);
                var fieldDecl = default(IFieldDeclaration);
                var constantExpression = default(ConstantExpression);
                if ((localDecl = methodGen.Body.Locals.FirstOrDefault(_localDecl => _localDecl.Name == fieldInfo.Name)) != null)
                {
                    methodGen.Body.ILOperator.Emit(OpCodes.Ldloc, localDecl);
                }
                else if ((parameterGen = methodGen.Parameters.FirstOrDefault(_parameterGen => _parameterGen.Name == fieldInfo.Name)) != null)
                {
                    methodGen.Body.ILOperator.Emit(OpCodes.Ldarg, parameterGen);
                }
                else if ((fieldDecl = methodGen.DeclaringType.GetField(fieldInfo.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)) != null)
                {
                    methodGen.Body.ILOperator.Emit(OpCodes.Ldarg_0);
                    methodGen.Body.ILOperator.Emit(OpCodes.Ldfld, fieldDecl);
                }
                else if ((constantExpression = exp.Expression as ConstantExpression) != null)
                {
                    // NOTE: 同じ名前の変数を Addloc されるとやっかい。擬似的にローカル変数としても定義することを検討中。
                    var item = methodGen.AddPortableScopeItem(fieldInfo);
                    methodGen.Body.ILOperator.Emit(OpCodes.Ldarg_0);
                    methodGen.Body.ILOperator.Emit(OpCodes.Ldfld, item);
                }
                else
                {
                    EvalExpression(methodGen, exp.Expression, state);

                    if (fieldInfo.IsStatic)
                    {
                        methodGen.Body.ILOperator.Emit(OpCodes.Ldsfld, fieldInfo);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
            else if ((propertyInfo = exp.Member as PropertyInfo) != null)
            {
                EvalExpression(methodGen, exp.Expression, state);

                if (propertyInfo.IsStatic())
                {
                    throw new NotImplementedException();
                }
                else
                {
                    methodGen.Body.ILOperator.Emit(OpCodes.Callvirt, propertyInfo.GetGetMethod());
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        static void EvalNew(IMethodBaseGenerator methodGen, NewExpression exp, EvalState state)
        {
            EvalArguments(methodGen, exp.Arguments, state);
            methodGen.Body.ILOperator.Emit(OpCodes.Newobj, exp.Constructor);
            if (exp.Members != null)
            {
                //// 使わなくても構築は問題なくできそう？
                //// → 匿名型の初期化では見た目と異なり、コンストラクタの引数で設定するよう変更されている。
                //var variable = new VariableDefinition(methodDef.Module.Import(exp.Constructor.DeclaringType));
                //Direct.Emit(MC::Cil.OpCodes.Stloc, variable);
                //foreach (var member in exp.Members)
                //{
                //    Direct.Emit(MC::Cil.OpCodes.Ldloc, variable);
                //}
            }
        }

        static void EvalNewArray(IMethodBaseGenerator methodGen, NewArrayExpression exp, EvalState state)
        {
            if (exp.NodeType == ExpressionType.NewArrayInit)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4_S, (sbyte)exp.Expressions.Count);
                methodGen.Body.ILOperator.Emit(OpCodes.Newarr, exp.Type.GetElementType());
                var localDecl = methodGen.Body.ILOperator.AddLocal(exp.Type);
                methodGen.Body.ILOperator.Emit(OpCodes.Stloc, localDecl);
                exp.Expressions
                    .ForEach((_exp, index) =>
                    {
                        methodGen.Body.ILOperator.Emit(OpCodes.Ldloc, localDecl);
                        methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4, index);
                        EvalExpression(methodGen, _exp, state);
                        if (typeof(double).IsAssignableFrom(_exp.Type))
                        {
                            throw new NotImplementedException();
                        }
                        else
                        {
                            methodGen.Body.ILOperator.Emit(OpCodes.Stelem_Ref);
                        }
                    });
                methodGen.Body.ILOperator.Emit(OpCodes.Ldloc, localDecl);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        static void EvalParameter(IMethodBaseGenerator methodGen, ParameterExpression exp, EvalState state)
        {
            if (exp.Type == typeof(ExpressiveMethodBodyGenerator))
            {
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        #region IMethodBodyGenerator メンバ

        public IILOperator ILOperator { get { return methodGen.Body.ILOperator; } }

        public ReadOnlyCollection<ILocalGenerator> Locals
        {
            get { throw new NotImplementedException(); }
        }

        public ReadOnlyCollection<IDirectiveGenerator> Directives
        {
            get { return methodGen.Body.Directives; }
        }

        #endregion

        #region IMethodBodyDeclaration メンバ

        ReadOnlyCollection<ILocalDeclaration> IMethodBodyDeclaration.Locals
        {
            get { throw new NotImplementedException(); }
        }

        ReadOnlyCollection<IDirectiveDeclaration> IMethodBodyDeclaration.Directives
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }

    class EvalState
    {
        public bool NoPop { get; set; }
    }

}