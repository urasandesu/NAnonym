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
using Urasandesu.NAnonym.CREUtilities.Impl;
using Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil;
using Urasandesu.NAnonym.CREUtilities.Impl.System.Reflection;

namespace Urasandesu.NAnonym.CREUtilities
{
    // TODO: フォルダ構成 + partial クラスで、implicit operator や mixin を構成するのはありかも？
    // TODO: IDisposable にして、using 内で利用するようにしたほうがそれっぽい。
    [Obsolete]
    public sealed class ExpressiveILManipulator// : IDisposable
    {
        //readonly MethodDefinition methodDef;
        //readonly ILProcessor ilProcessor;

        readonly ITypeDeclaration declaringTypeMaker;
        readonly IMethodBaseGenerator methodMaker;
        readonly IMethodBodyGenerator bodyMaker;
        readonly IILOperator ilOperator;

        //readonly ExpressiveEvaluable evaluable;
        readonly Expressible expressible;

        readonly MethodInfo GetTypeFromHandle;

        ExpressiveILManipulator(IMethodBaseGenerator methodMaker)
        {
            this.methodMaker = methodMaker;
            declaringTypeMaker = methodMaker.DeclaringType; // NOTE: Generator から来てないとまずいってことだね。
            bodyMaker = methodMaker.Body;
            ilOperator = bodyMaker.GetILOperator();

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

        public ExpressiveILManipulator(ConstructorBuilder constructorBuilder)
            : this((SRConstructorGeneratorImpl)constructorBuilder)
        {
            //methodDecl = (ConstructorBuilder_ConstructorDeclaration)constructorBuilder;
            //declaringTypeDecl = methodDecl.DeclaringType;
            //bodyDecl = methodDecl.Body;
            //ilOperator = bodyDecl.GetILOperator();
        }

        public ExpressiveILManipulator(MethodDefinition methodDef)
            : this((MCMethodGeneratorImpl)methodDef)
        {
            //this.methodDef = methodDef;
            //this.methodDef.Body.InitLocals = true;
            //this.ilProcessor = methodDef.Body.GetILProcessor();

            //methodDecl = (MethodDefinition_MethodDeclaration)methodDef;

            //bodyDecl = methodDecl.Body;
            //ilOperator = bodyDecl.GetILOperator();
            //ilOperator = new MethodDefinitionILOperator(methodDef);
            // TODO: var variableDictionary = new Dictionary<MethodBase, SharedScope>(); みたいな Field を Inject する！
            // TODO: デフォルトの動作としては、同一メモリ上のモデル（LambdaExpression.Compile() と同じイメージ）
            //       ↑こちらをやりたい場合は Mono.Cecil を使うんじゃなくて、System.Reflection を使ったほうがいい（AppDomain にロード*しない*ことが Mono.Cecil の利点だからね）。
            //       ↑Expression Visitor の中身は Mono.Cecil と System.Reflection を簡単に入れ替えられる仕組みを入れておいたほうが良いね。


            ////evaluable = new ExpressiveEvaluable();
            //expressible = new Expressible();

            //GetTypeFromHandle =
            //    typeof(System.Type).GetMethod(
            //        "GetTypeFromHandle",
            //        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
            //        null,
            //        new System.Type[] 
            //        {
            //            typeof(RuntimeTypeHandle) 
            //        },
            //        null);
        }

        public void Eval(Expression<Action<Expressible>> exp)
        {
            Eval(exp.Body);
            if (exp.Body.Type != typeof(void))
            {
                // NOTE: void ではないということは評価スタックに情報が残っているということ。
                //       pop するのは、基本的に 1 回の Emit(Expression<Action<ExpressiveILProcessor>>) で完結するようにしたいため。
                Direct.Emit(OpCodes.Pop);
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
                    Direct.Emit(OpCodes.Add);
                    //_Direct.Emit(MC.Cil.OpCodes.Add);
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
                    Direct.Emit(OpCodes.Mul);
                    //_Direct.Emit(MC.Cil.OpCodes.Mul);
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
                    Direct.Emit(OpCodes.Ldelem_Ref);
                    //_Direct.Emit(MC.Cil.OpCodes.Ldelem_Ref);
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
                Direct.Emit(OpCodes.Call, exp.Method);
            }
            else
            {
                if (exp.Object.Type == typeof(Expressible))
                {
                    if (expressible.IsBase(exp.Method))
                    {
                        Direct.Emit(OpCodes.Ldarg_0);
                        var constructorDecl = declaringTypeMaker.BaseType.GetConstructor(new Type[] { });
                        Direct.Emit(OpCodes.Call, constructorDecl);
                        // TODO: ベースクラスのコンストラクタを呼ぶよ。
                        // TODO: ベースクラスは、methodDecl.DeclaringType.BaseType とかから取るとして、コンストラクタはどう取るのだ？
                        // TODO: 結局、ConstructorDeclaration とかも必要になるのか・・・orz。
                        // TODO: GetConstructor と同じ引数でそのまま呼べる感じの EmitCallCtor とか作ればいいかな？
                        //       → MethodBase 系は同じ話になる。
                    }
                    else if (expressible.IsAddloc(exp.Method))
                    {
                        Eval(exp.Arguments[1]);
                        var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
                        var local = Direct.AddLocal(fieldInfo.Name, fieldInfo.FieldType);
                        Direct.Emit(OpCodes.Stloc, local);
                    }
                    else if (expressible.IsDupAddOne(exp.Method))
                    {
                        var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
                        var localDecl = bodyMaker.Locals.First(_localDecl => _localDecl.Name == fieldInfo.Name);
                        Direct.Emit(OpCodes.Ldloc, localDecl);
                        Direct.Emit(OpCodes.Dup);
                        Direct.Emit(OpCodes.Ldc_I4_1);
                        Direct.Emit(OpCodes.Add);
                        Direct.Emit(OpCodes.Stloc, localDecl);
                    }
                    else if (expressible.IsAddOneDup(exp.Method))
                    {
                        var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
                        var localDecl = bodyMaker.Locals.First(_localDecl => _localDecl.Name == fieldInfo.Name);
                        Direct.Emit(OpCodes.Ldloc, localDecl);
                        Direct.Emit(OpCodes.Ldc_I4_1);
                        Direct.Emit(OpCodes.Add);
                        Direct.Emit(OpCodes.Dup);
                        Direct.Emit(OpCodes.Stloc, localDecl);
                    }
                    else if (expressible.IsSubOneDup(exp.Method))
                    {
                        var fieldInfo = (FieldInfo)((MemberExpression)exp.Arguments[0]).Member;
                        var localDecl = bodyMaker.Locals.First(_localDecl => _localDecl.Name == fieldInfo.Name);
                        Direct.Emit(OpCodes.Ldloc, localDecl);
                        Direct.Emit(OpCodes.Ldc_I4_1);
                        Direct.Emit(OpCodes.Sub);
                        Direct.Emit(OpCodes.Dup);
                        Direct.Emit(OpCodes.Stloc, localDecl);
                    }
                    else if (expressible.IsReturn(exp.Method))
                    {
                        Eval(exp.Arguments[0]);
                        Direct.Emit(OpCodes.Ret);
                    }
                    else if (expressible.IsEnd(exp.Method))
                    {
                        Direct.Emit(OpCodes.Ret);
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
                        var local = ilOperator.AddLocal(exp.Object.Type);
                        Direct.Emit(OpCodes.Stloc, local);
                        Direct.Emit(OpCodes.Ldloca, local);
                    }

                    Eval(exp.Arguments);

                    Direct.Emit(OpCodes.Callvirt, exp.Method);
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
                Direct.Emit(OpCodes.Ldnull);
                //_Direct.Emit(MC.Cil.OpCodes.Ldnull);
            }
            else if ((s = exp.Value as string) != null)
            {
                Direct.Emit(OpCodes.Ldstr, s);
            }
            else if ((ni = exp.Value as int?) != null)
            {
                // HACK: _S が付く命令は短い形式。-127 ～ 128 以外は最適化が必要。
                //_Direct.Emit(MC.Cil.OpCodes.Ldc_I4_S, (sbyte)(int)ni);
                Direct.Emit(OpCodes.Ldc_I4_S, (sbyte)(int)ni);
            }
            else if ((nd = exp.Value as double?) != null)
            {
                Direct.Emit(OpCodes.Ldc_R8, (double)nd);
                //_Direct.Emit(MC.Cil.OpCodes.Ldc_R8, (double)nd);
            }
            else if ((nc = exp.Value as char?) != null)
            {
                Direct.Emit(OpCodes.Ldc_I4_S, (sbyte)(char)nc);
                //_Direct.Emit(MC.Cil.OpCodes.Ldc_I4_S, (sbyte)(char)nc);
            }
            else if ((nb = exp.Value as bool?) != null)
            {
                Direct.Emit(OpCodes.Ldc_I4, (bool)nb ? 1 : 0);
                //_Direct.Emit(MC.Cil.OpCodes.Ldc_I4, (bool)nb ? 1 : 0);
            }
            else if ((e = exp.Value as Enum) != null)
            {
                Direct.Emit(OpCodes.Ldc_I4, e.GetHashCode());
                //_Direct.Emit(MC.Cil.OpCodes.Ldc_I4, e.GetHashCode());
            }
            else if ((t = exp.Value as Type) != null)
            {
                Direct.Emit(OpCodes.Ldtoken, t);
                Direct.Emit(OpCodes.Call, GetTypeFromHandle);
                //_Direct.Emit(MC.Cil.OpCodes.Ldtoken, methodDef.Module.Import(t));
                //_Direct.Emit(MC.Cil.OpCodes.Call, methodDef.Module.Import(GetTypeFromHandle));
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
                        Direct.Emit(OpCodes.Conv_I4);
                        //_Direct.Emit(MC.Cil.OpCodes.Conv_I4);
                    }
                    else if (typeof(double).IsAssignableFrom(exp.Type))
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        Direct.Emit(OpCodes.Box, exp.Operand.Type);
                        //_Direct.Emit(MC.Cil.OpCodes.Box, methodDef.Module.Import(exp.Operand.Type));
                    }
                    break;
                case ExpressionType.ArrayLength:
                    Direct.Emit(OpCodes.Ldlen);
                    Direct.Emit(OpCodes.Conv_I4);
                    //_Direct.Emit(MC.Cil.OpCodes.Ldlen);
                    //_Direct.Emit(MC.Cil.OpCodes.Conv_I4);
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
                var variable = default(VariableDefinition);
                var parameter = default(ParameterDefinition);
                var constantExpression = default(ConstantExpression);
                var constantField = default(FieldInfo);
                if ((localDecl = bodyMaker.Locals.FirstOrDefault(_localDecl => _localDecl.Name == fieldInfo.Name)) != null)
                {
                    Direct.Emit(OpCodes.Ldloc, localDecl);
                }
                else if ((parameterDecl = ((IMethodBaseDeclaration)methodMaker).Parameters.FirstOrDefault(_parameterDecl => _parameterDecl.Name == fieldInfo.Name)) != null)
                {
                    Direct.Emit(OpCodes.Ldarg, parameterDecl);
                }
                else if ((constantExpression = exp.Expression as ConstantExpression) != null)
                {
                    string fieldName = TotableScope.MakeFieldName(methodMaker, fieldInfo.Name);
                    var fieldDecl = ((ITypeGenerator)declaringTypeMaker).AddField(fieldName, fieldInfo.FieldType, SR.FieldAttributes.Public | SR.FieldAttributes.SpecialName);
                    Direct.Emit(OpCodes.Ldarg_0);
                    Direct.Emit(OpCodes.Ldfld, fieldDecl);
                }
                else
                {
                    Eval(exp.Expression);

                    if (fieldInfo.IsStatic)
                    {
                        Direct.Emit(OpCodes.Ldsfld, fieldInfo);
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
                    Direct.Emit(OpCodes.Callvirt, propertyInfo.GetGetMethod());
                    //_Direct.Emit(MC.Cil.OpCodes.Callvirt, methodDef.Module.Import(propertyInfo.GetGetMethod()));
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
            Direct.Emit(OpCodes.Newobj, exp.Constructor);
            //_Direct.Emit(MC.Cil.OpCodes.Newobj, methodDef.Module.Import(exp.Constructor));
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

        void Eval(NewArrayExpression exp)
        {
            if (exp.NodeType == ExpressionType.NewArrayInit)
            {
                Direct.Emit(OpCodes.Ldc_I4_S, (sbyte)exp.Expressions.Count);
                Direct.Emit(OpCodes.Newarr, exp.Type.GetElementType());
                //_Direct.Emit(MC.Cil.OpCodes.Ldc_I4_S, (sbyte)exp.Expressions.Count);
                //_Direct.Emit(MC.Cil.OpCodes.Newarr, methodDef.Module.Import(exp.Type.GetElementType()));
                var localDecl = ilOperator.AddLocal(exp.Type);
                //var variable = new VariableDefinition(methodDef.Module.Import(exp.Type));
                //methodDef.Body.Variables.Add(variable);
                Direct.Emit(OpCodes.Stloc, localDecl);
                //_Direct.Emit(MC.Cil.OpCodes.Stloc, variable);
                exp.Expressions
                    .ForEach((_exp, index) =>
                    {
                        Direct.Emit(OpCodes.Ldloc, localDecl);
                        Direct.Emit(OpCodes.Ldc_I4, index);
                        //_Direct.Emit(MC.Cil.OpCodes.Ldloc, variable);
                        //_Direct.Emit(MC.Cil.OpCodes.Ldc_I4, index);
                        Eval(_exp);
                        if (typeof(double).IsAssignableFrom(_exp.Type))
                        {
                            throw new NotImplementedException();
                        }
                        else
                        {
                            Direct.Emit(OpCodes.Stelem_Ref);
                            //_Direct.Emit(MC.Cil.OpCodes.Stelem_Ref);
                        }
                    });
                Direct.Emit(OpCodes.Ldloc, localDecl);
                //_Direct.Emit(MC.Cil.OpCodes.Ldloc, variable);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        void Eval(ParameterExpression exp)
        {
            if (exp.Type == typeof(ExpressiveILManipulator))
            {
            }
            else
            {
                throw new NotImplementedException();
            }
        }


        public ILProcessor _Direct
        {
            get
            {
                throw new NotSupportedException();
                //return ilProcessor;
            }
        }

        public IILOperator Direct
        {
            get
            {
                return ilOperator;
            }
        }

        public VariableDefinition Locals<T>(Expression<Func<T>> variable)
        {
            throw new NotImplementedException();
        }


        
        //#region IDisposable メンバ

        //bool disposed = false;

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //void Dispose(bool disposing)
        //{
        //    if (!disposed)
        //    {
        //        if (disposing)
        //        {
        //            // Dispose managed resources.
        //            var directive = ilOperator.Directives.LastOrDefault();
        //            if (directive != null && directive.OpCode != OpCodes.Ret)
        //            {
        //                Direct.Emit(OpCodes.Ret);
        //            }
        //        }

        //        // Release unmanaged resources.
        //    }
        //    disposed = true;         
        //}

        //#endregion
    }
}
