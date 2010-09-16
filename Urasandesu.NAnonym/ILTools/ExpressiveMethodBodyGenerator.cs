using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Reflection.Emit;
//using Mono.Cecil;
using Urasandesu.NAnonym.ILTools.Impl.System.Reflection;
//using Urasandesu.NAnonym.ILTools.Impl.Mono.Cecil;
//using Mono.Cecil.Cil;
using SR = System.Reflection;
//using MC = Mono.Cecil;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.ILTools
{
    // TODO: PortableScope の対応が一通り終わったところで Assembly 分割を行う。
    //       Urasandesu.NAnonym.
    public class ExpressiveMethodBodyGenerator : IMethodBodyGenerator
    {
        readonly ITypeDeclaration declaringTypeDecl;
        readonly IMethodBaseGenerator methodGen;
        readonly IMethodBodyGenerator bodyGen;
        readonly IILOperator il;
        readonly Expressible expressible;

        readonly MethodInfo GetTypeFromHandle;

        internal ExpressiveMethodBodyGenerator(IMethodBaseGenerator methodGen)
        {
            this.methodGen = methodGen;
            declaringTypeDecl = methodGen.DeclaringType; // NOTE: Generator から来てないとまずいってことだね。
            bodyGen = methodGen.Body;
            il = bodyGen.GetILOperator();

            expressible = new Expressible();

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

        //public ExpressiveMethodBodyGenerator(ConstructorBuilder constructorBuilder)
        //    : this((SRConstructorGeneratorImpl)constructorBuilder)
        //{
        //}

        //public ExpressiveMethodBodyGenerator(DynamicMethod dynamicMethod)
        //    : this((SRMethodGeneratorImpl)dynamicMethod)
        //{
        //}

        //public ExpressiveMethodBodyGenerator(MethodDefinition methodDef)
        //    : this((MCMethodGeneratorImpl)methodDef)
        //{
        //}

        public void Eval(Expression<Action<Expressible>> exp)
        {
            Eval(exp.Body);
            if (exp.Body.Type != typeof(void))
            {
                // NOTE: void ではないということは評価スタックに情報が残っているということ。
                //       pop するのは、基本的に 1 回の Emit(Expression<Action<ExpressiveILProcessor>>) で完結するようにしたいため。
                il.Emit(OpCodes.Pop);
            }
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
            if (exp == null) return;

            switch (exp.NodeType)
            {
                case ExpressionType.Add:
                case ExpressionType.ArrayIndex:
                case ExpressionType.Multiply:
                    Eval((BinaryExpression)exp);
                    return;
                case ExpressionType.AddChecked:
                    break;
                case ExpressionType.And:
                    break;
                case ExpressionType.AndAlso:
                    break;
                case ExpressionType.Call:
                    Eval((MethodCallExpression)exp);
                    return;
                case ExpressionType.Coalesce:
                    break;
                case ExpressionType.Conditional:
                    break;
                case ExpressionType.Constant:
                    Eval((ConstantExpression)exp);
                    return;
                case ExpressionType.ArrayLength:
                case ExpressionType.Convert:
                    Eval((UnaryExpression)exp);
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
                    Eval((MemberExpression)exp);
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
                    Eval((NewExpression)exp);
                    return;
                case ExpressionType.NewArrayBounds:
                    break;
                case ExpressionType.NewArrayInit:
                    Eval((NewArrayExpression)exp);
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
                    Eval((ParameterExpression)exp);
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

        void Eval(BinaryExpression exp)
        {
            Eval(exp.Left);
            Eval(exp.Right);

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
                    il.Emit(OpCodes.Add);
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
                    il.Emit(OpCodes.Mul);
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
                    il.Emit(OpCodes.Ldelem_Ref);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        void Eval(MethodCallExpression exp)
        {
            // 評価の順番は、Object -> Arguments -> Method。
            if (exp.Object == null)
            {
                // static method call
                Eval(exp.Arguments);
                il.Emit(OpCodes.Call, exp.Method);
            }
            else
            {
                if (exp.Object.Type == typeof(Expressible))
                {
                    if (expressible.IsBase(exp.Method))
                    {
                        il.Emit(OpCodes.Ldarg_0);
                        var constructorDecl = declaringTypeDecl.BaseType.GetConstructor(new Type[] { });
                        il.Emit(OpCodes.Call, constructorDecl);
                    }
                    else if (expressible.IsAddloc(exp.Method))
                    {
                        // TODO: 同じ名前の変数は、同じ Scope 内であれば隠せる or 弾く工夫が必要
                        Eval(exp.Arguments[1]);
                        var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
                        var local = il.AddLocal(fieldInfo.Name, fieldInfo.FieldType);
                        il.Emit(OpCodes.Stloc, local);
                    }
                    else if (expressible.IsDupAddOne(exp.Method))
                    {
                        var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
                        var localDecl = bodyGen.Locals.First(_localDecl => _localDecl.Name == fieldInfo.Name);
                        il.Emit(OpCodes.Ldloc, localDecl);
                        il.Emit(OpCodes.Dup);
                        il.Emit(OpCodes.Ldc_I4_1);
                        il.Emit(OpCodes.Add);
                        il.Emit(OpCodes.Stloc, localDecl);
                    }
                    else if (expressible.IsAddOneDup(exp.Method))
                    {
                        var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
                        var localDecl = bodyGen.Locals.First(_localDecl => _localDecl.Name == fieldInfo.Name);
                        il.Emit(OpCodes.Ldloc, localDecl);
                        il.Emit(OpCodes.Ldc_I4_1);
                        il.Emit(OpCodes.Add);
                        il.Emit(OpCodes.Dup);
                        il.Emit(OpCodes.Stloc, localDecl);
                    }
                    else if (expressible.IsSubOneDup(exp.Method))
                    {
                        var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
                        var localDecl = bodyGen.Locals.First(_localDecl => _localDecl.Name == fieldInfo.Name);
                        il.Emit(OpCodes.Ldloc, localDecl);
                        il.Emit(OpCodes.Ldc_I4_1);
                        il.Emit(OpCodes.Sub);
                        il.Emit(OpCodes.Dup);
                        il.Emit(OpCodes.Stloc, localDecl);
                    }
                    else if (expressible.IsExpand(exp.Method))
                    {
                        var expand = Expression.Lambda(exp.Arguments[0]).Compile();
                        Eval(Expression.Constant(expand.DynamicInvoke()));
                    }
                    else if (expressible.IsReturn(exp.Method))
                    {
                        Eval(exp.Arguments[0]);
                        il.Emit(OpCodes.Ret);
                    }
                    else if (expressible.IsEnd(exp.Method))
                    {
                        il.Emit(OpCodes.Ret);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    // instance method call
                    Eval(exp.Object);
                    if (exp.Object.Type.IsValueType)
                    {
                        // NOTE: 値型のメソッドを呼び出すには、アドレスへの変換が必要。
                        var local = il.AddLocal(exp.Object.Type);
                        il.Emit(OpCodes.Stloc, local);
                        il.Emit(OpCodes.Ldloca, local);
                    }

                    Eval(exp.Arguments);

                    il.Emit(OpCodes.Callvirt, exp.Method);
                }
            }
        }

        void Eval(ConstantExpression exp)
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
                il.Emit(OpCodes.Ldnull);
            }
            else if ((s = exp.Value as string) != null)
            {
                il.Emit(OpCodes.Ldstr, s);
            }
            else if ((ni = exp.Value as int?) != null)
            {
                // HACK: _S が付く命令は短い形式。-127 ～ 128 以外は最適化が必要。
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)(int)ni);
            }
            else if ((nd = exp.Value as double?) != null)
            {
                il.Emit(OpCodes.Ldc_R8, (double)nd);
            }
            else if ((nc = exp.Value as char?) != null)
            {
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)(char)nc);
            }
            else if ((nb = exp.Value as bool?) != null)
            {
                il.Emit(OpCodes.Ldc_I4, (bool)nb ? 1 : 0);
            }
            else if ((e = exp.Value as Enum) != null)
            {
                il.Emit(OpCodes.Ldc_I4, e.GetHashCode());
            }
            else if ((t = exp.Value as Type) != null)
            {
                il.Emit(OpCodes.Ldtoken, t);
                il.Emit(OpCodes.Call, GetTypeFromHandle);
            }
            else
            {
                // TODO: exp.Value がオブジェクト型の場合はどうするのだ？
                throw new NotImplementedException();
            }
        }

        void Eval(UnaryExpression exp)
        {
            Eval(exp.Operand);
            switch (exp.NodeType)
            {
                case ExpressionType.Convert:
                    if (exp.Type == typeof(int))
                    {
                        il.Emit(OpCodes.Conv_I4);
                    }
                    else if (typeof(double).IsAssignableFrom(exp.Type))
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        il.Emit(OpCodes.Box, exp.Operand.Type);
                    }
                    break;
                case ExpressionType.ArrayLength:
                    il.Emit(OpCodes.Ldlen);
                    il.Emit(OpCodes.Conv_I4);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        void Eval(MemberExpression exp)
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
                var parameterDecl = default(IParameterDeclaration);
                //var variable = default(VariableDefinition);
                //var parameter = default(ParameterDefinition);
                var constantExpression = default(ConstantExpression);
                var constantField = default(FieldInfo);
                if ((localDecl = bodyGen.Locals.FirstOrDefault(_localDecl => _localDecl.Name == fieldInfo.Name)) != null)
                {
                    il.Emit(OpCodes.Ldloc, localDecl);
                }
                else if ((parameterDecl = ((IMethodBaseDeclaration)methodGen).Parameters.FirstOrDefault(_parameterDecl => _parameterDecl.Name == fieldInfo.Name)) != null)
                {
                    il.Emit(OpCodes.Ldarg, parameterDecl);
                }
                else if ((constantExpression = exp.Expression as ConstantExpression) != null)
                {
                    //string fieldName = PortableScope.MakeFieldName(methodGen, fieldInfo.Name);
                    //var fieldDecl = ((ITypeGenerator)declaringTypeDecl).AddField(fieldName, fieldInfo.FieldType, SR::FieldAttributes.Public | SR::FieldAttributes.SpecialName);
                    //il.Emit(OpCodes.Ldarg_0);
                    //il.Emit(OpCodes.Ldfld, fieldDecl);
                    // NOTE: 同じ名前の変数を Addloc されるとやっかい。擬似的にローカル変数としても定義することを検討中。
                    var item = methodGen.AddPortableScopeItem(fieldInfo);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldfld, item);
                }
                else
                {
                    Eval(exp.Expression);

                    if (fieldInfo.IsStatic)
                    {
                        il.Emit(OpCodes.Ldsfld, fieldInfo);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
            else if ((propertyInfo = exp.Member as PropertyInfo) != null)
            {
                Eval(exp.Expression);

                if (propertyInfo.IsStatic())
                {
                    throw new NotImplementedException();
                }
                else
                {
                    il.Emit(OpCodes.Callvirt, propertyInfo.GetGetMethod());
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        void Eval(NewExpression exp)
        {
            Eval(exp.Arguments);
            il.Emit(OpCodes.Newobj, exp.Constructor);
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

        void Eval(NewArrayExpression exp)
        {
            if (exp.NodeType == ExpressionType.NewArrayInit)
            {
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)exp.Expressions.Count);
                il.Emit(OpCodes.Newarr, exp.Type.GetElementType());
                var localDecl = il.AddLocal(exp.Type);
                il.Emit(OpCodes.Stloc, localDecl);
                exp.Expressions
                    .ForEach((_exp, index) =>
                    {
                        il.Emit(OpCodes.Ldloc, localDecl);
                        il.Emit(OpCodes.Ldc_I4, index);
                        Eval(_exp);
                        if (typeof(double).IsAssignableFrom(_exp.Type))
                        {
                            throw new NotImplementedException();
                        }
                        else
                        {
                            il.Emit(OpCodes.Stelem_Ref);
                        }
                    });
                il.Emit(OpCodes.Ldloc, localDecl);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        void Eval(ParameterExpression exp)
        {
            if (exp.Type == GetType())
            {
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        #region IMethodBodyGenerator メンバ

        public IILOperator GetILOperator()
        {
            return il;
        }

        public ReadOnlyCollection<ILocalGenerator> Locals
        {
            get { throw new NotImplementedException(); }
        }

        public ReadOnlyCollection<IDirectiveGenerator> Directives
        {
            get { throw new NotImplementedException(); }
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

}