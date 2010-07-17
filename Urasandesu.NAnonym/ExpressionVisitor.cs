using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Urasandesu.NAnonym
{
    // HACK: 破棄予定
    [Obsolete]
    public sealed class ExpressionVisitor
    {
        Expression expression;
        Dictionary<string, CodeTypeMember> members;

        public ExpressionVisitor(Expression expression)
        {
            this.expression = expression;
        }

        internal string GenerateSource(CodeDomProvider provider)
        {
            StringBuilder sb = new StringBuilder();
            TextWriter tWriter = new IndentedTextWriter(new StringWriter(sb));
            CodeCompileUnit ccu = GenerateCode();
            provider.GenerateCodeFromCompileUnit(ccu, tWriter, new CodeGeneratorOptions());
            provider.Dispose();

            tWriter.Close();

            return sb.ToString();
        }

        internal string GenerateSource(string language)
        {


            CodeDomProvider codeProvider = null;
            if (language == "cs")
                codeProvider = new Microsoft.CSharp.CSharpCodeProvider();
            else if (language == "vb")
                codeProvider = new Microsoft.VisualBasic.VBCodeProvider();
            else
            {

                throw new Exception("make sure you are trying to load a CodeDomProvider assembly");

            }
            return GenerateSource(codeProvider);

        }

        public string GenerateSource()
        {
            return GenerateSource("cs");
        }


        private CodeCompileUnit GenerateCode()
        {
            var code = new CodeCompileUnit();
            members = new Dictionary<string, CodeTypeMember>();

            var LambdaTypeClass = new CodeTypeDeclaration("LambdaExpression");
            var ns = new CodeNamespace("Runtime");

            ns.Types.Add(LambdaTypeClass);
            ns.Imports.Add(new CodeNamespaceImport("System"));
            // add more types in case I want to compile

            code.Namespaces.Add(ns);

            CodeObject cEvaluationResult = Visit(expression);

            var constructor = new CodeConstructor();

            if (cEvaluationResult is CodeStatement)
                constructor.Statements.Add(cEvaluationResult as CodeStatement);

            else if (cEvaluationResult is CodeExpression)
                constructor.Statements.Add(cEvaluationResult as CodeExpression);

            LambdaTypeClass.Members.Add(constructor);


            foreach (var item in members)
            {
                LambdaTypeClass.Members.Add(item.Value);
            }

            return code;

        }

        private CodeObject Visit(Expression expression)
        {
            if (expression == null)
                return null;

            switch (expression.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                    return Visit((UnaryExpression)expression);
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                    return Visit((BinaryExpression)expression);
                case ExpressionType.TypeIs:
                    return Visit((TypeBinaryExpression)expression);
                case ExpressionType.Conditional:
                    return Visit((ConditionalExpression)expression);
                case ExpressionType.Constant:
                    return Visit((ConstantExpression)expression);
                case ExpressionType.Parameter:
                    return Visit((ParameterExpression)expression);
                case ExpressionType.MemberAccess:
                    return Visit((MemberExpression)expression);
                case ExpressionType.Call:
                    return Visit((MethodCallExpression)expression);
                case ExpressionType.Lambda:
                    return Visit((LambdaExpression)expression);
                case ExpressionType.New:
                    return Visit((NewExpression)expression);
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    return Visit((NewArrayExpression)expression);
                case ExpressionType.Invoke:
                    return Visit((InvocationExpression)expression);
                case ExpressionType.MemberInit:
                    return Visit((MemberInitExpression)expression);
                case ExpressionType.ListInit:
                    return Visit((ListInitExpression)expression);
                default:
                    throw new NotSupportedException(string.Format("Unhandled expression type: '{0}'", expression.NodeType));
            }
        }

        private CodeObject Visit(MemberBinding memberBinding)
        {
            switch (memberBinding.BindingType)
            {
                case MemberBindingType.Assignment:
                    return Visit((MemberAssignment)memberBinding);
                case MemberBindingType.MemberBinding:
                    return Visit((MemberMemberBinding)memberBinding);
                case MemberBindingType.ListBinding:
                    return Visit((MemberListBinding)memberBinding);
                default:
                    throw new NotSupportedException(string.Format("Unhandled binding type '{0}'", memberBinding.BindingType));
            }
        }

        private CodeMethodInvokeExpression Visit(ElementInit elementInit)
        {
            var arguments = Visit(elementInit.Arguments);

            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(
                new CodeThisReferenceExpression(), elementInit.AddMethod.Name), arguments);
        }

        private CodeObject Visit(UnaryExpression unaryExpression)
        {
            return Visit(unaryExpression.Operand);
        }

        private CodeBinaryOperatorType ToBinaryOperatorType(ExpressionType expressionType)
        {
            switch (expressionType)
            {
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return CodeBinaryOperatorType.Add;
                case ExpressionType.And:
                    return CodeBinaryOperatorType.BitwiseAnd;
                case ExpressionType.AndAlso:
                    return CodeBinaryOperatorType.BooleanAnd;
                case ExpressionType.Or:
                    return CodeBinaryOperatorType.BitwiseOr;
                case ExpressionType.OrElse:
                    return CodeBinaryOperatorType.BooleanOr;
                case ExpressionType.Equal:
                    return CodeBinaryOperatorType.IdentityEquality;
                case ExpressionType.NotEqual:
                    return CodeBinaryOperatorType.IdentityInequality;
                case ExpressionType.GreaterThan:
                    return CodeBinaryOperatorType.GreaterThan;
                case ExpressionType.GreaterThanOrEqual:
                    return CodeBinaryOperatorType.GreaterThanOrEqual;
                case ExpressionType.LessThan:
                    return CodeBinaryOperatorType.LessThan;
                case ExpressionType.LessThanOrEqual:
                    return CodeBinaryOperatorType.LessThanOrEqual;
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return CodeBinaryOperatorType.Multiply;
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return CodeBinaryOperatorType.Subtract;
                case ExpressionType.Power:
                case ExpressionType.Divide:
                    return CodeBinaryOperatorType.Divide;
                case ExpressionType.Modulo:
                    return CodeBinaryOperatorType.Modulus;
                case ExpressionType.ExclusiveOr:
                case ExpressionType.ArrayIndex:
                case ExpressionType.Coalesce:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                default:
                    throw new NotSupportedException("no direct equivalent in codedom,so workarounds not implemented");
            }
        }

        private CodeBinaryOperatorExpression Visit(BinaryExpression binaryExpression)
        {
            var left = (CodeExpression)Visit(binaryExpression.Left);
            var right = (CodeExpression)Visit(binaryExpression.Right);

            CodeBinaryOperatorType operant = ToBinaryOperatorType(binaryExpression.NodeType);
            return new CodeBinaryOperatorExpression(left, operant, right);
        }

        private CodeObject Visit(TypeBinaryExpression typeBinaryExpression)
        {
            return Visit(typeBinaryExpression.Expression);
        }

        private CodeExpression Visit(ConstantExpression constantExpression)
        {
            if (constantExpression.Value == null)
            {
                return new CodePrimitiveExpression(null);
            }
            else if (constantExpression.Value.GetType().IsValueType || constantExpression.Value.GetType() == typeof(string))
            {
                return new CodePrimitiveExpression(constantExpression.Value);
            }
            else
            {
                return new CodeVariableReferenceExpression(constantExpression.Value.ToString());
            }
        }

        private CodeStatement Visit(ConditionalExpression conditionalExpression)
        {
            var test = (CodeExpression)Visit(conditionalExpression.Test);
            var ifTrue = (CodeExpression)Visit(conditionalExpression.IfTrue);
            var ifFalse = (CodeExpression)Visit(conditionalExpression.IfFalse);

            var ifStatement = new CodeConditionStatement(test,
                                                         new CodeStatement[] { new CodeExpressionStatement(ifTrue) },
                                                         new CodeStatement[] { new CodeExpressionStatement(ifFalse) });
            return ifStatement;
        }

        private CodeExpression Visit(ParameterExpression parameterExpression)
        {
            return new CodeArgumentReferenceExpression(parameterExpression.Name);
        }

        private CodeExpression Visit(MemberExpression memberExpression)
        {
            var codeObject = Visit(memberExpression.Expression);

            var primitiveExpression = default(CodePrimitiveExpression);
            if ((primitiveExpression = codeObject as CodePrimitiveExpression) != null)
            {
                return primitiveExpression;
            }
            else
            {
                Type memberType;
                if (memberExpression.Member.MemberType == MemberTypes.Field)
                    memberType = ((FieldInfo)memberExpression.Member).FieldType;
                else
                    memberType = ((PropertyInfo)memberExpression.Member).PropertyType;

                members[memberExpression.Member.Name] = new CodeMemberField(memberType, memberExpression.Member.Name);
                return new CodeVariableReferenceExpression(memberExpression.Member.Name);
            }
        }

        private CodeExpression Visit(MethodCallExpression methodCallExpression)
        {
            var @object = Visit(methodCallExpression.Object);
            var arguments = Visit(methodCallExpression.Arguments);

            if (@object == null)
            {
                //static method call
                return new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression(methodCallExpression.Method.DeclaringType),
                    methodCallExpression.Method.Name, arguments);
            }
            else
            {
                return new CodeMethodInvokeExpression((CodeExpression)@object, methodCallExpression.Method.Name, arguments);
            }
        }

        private CodeExpression[] Visit(ReadOnlyCollection<Expression> expressions)
        {
            var codeExpressions = new CodeExpression[expressions.Count];
            for (int i = 0; i < expressions.Count; i++)
            {
                codeExpressions[i] = (CodeExpression)Visit(expressions[i]);
            }
            return codeExpressions;
        }

        private CodeExpression Visit(MemberAssignment memberAssignment)
        {
            return (CodeExpression)Visit(memberAssignment.Expression);
        }

        private CodeObjectCreateExpression Visit(MemberMemberBinding memberMemberBinding)
        {
            var bindings = Visit(memberMemberBinding.Bindings);
            return new CodeObjectCreateExpression(memberMemberBinding.Member.Name, bindings);
        }

        private CodeObjectCreateExpression Visit(MemberListBinding memberListBinding)
        {
            var initializers = Visit(memberListBinding.Initializers);
            return new CodeObjectCreateExpression(memberListBinding.Member.Name, initializers);
        }

        private CodeExpression[] Visit(ReadOnlyCollection<MemberBinding> memberBindings)
        {
            var codeExpressions = new CodeExpression[memberBindings.Count];
            for (int i = 0; i < memberBindings.Count; i++)
            {
                codeExpressions[i] = (CodeExpression)Visit(memberBindings[i]);
            }
            return codeExpressions;
        }

        private CodeExpression[] Visit(ReadOnlyCollection<ElementInit> elementInits)
        {
            var codeExpressions = new CodeExpression[elementInits.Count];
            for (int i = 0; i < elementInits.Count; i++)
            {
                codeExpressions[i] = (CodeExpression)Visit(elementInits[i]);
            }
            return codeExpressions;
        }

        private CodeMethodReferenceExpression Visit(LambdaExpression lambda)
        {
            var body = Visit(lambda.Body);
            var lambdaMethod = new CodeMemberMethod();

            lambdaMethod.Name = lambda.Type.Name;
            if (lambdaMethod.Name.Contains("Func"))
                lambdaMethod.ReturnType = new CodeTypeReference(lambda.Body.Type);

            for (int i = 0; i < lambda.Parameters.Count; i++)
            {
                lambdaMethod.Parameters.Add(
                    new CodeParameterDeclarationExpression(lambda.Parameters[i].Type, lambda.Parameters[i].Name));
            }

            var codeExpression = default(CodeExpression);
            var codeStatement = default(CodeStatement);
            if ((codeExpression = body as CodeExpression) != null)
            {
                if (lambdaMethod.ReturnType.BaseType.Contains("Void"))
                    lambdaMethod.Statements.Add(codeExpression);
                else
                    lambdaMethod.Statements.Add(new CodeMethodReturnStatement(codeExpression));
            }
            else if ((codeStatement = body as CodeStatement) != null)
            {
                lambdaMethod.Statements.Add(codeStatement);
            }
            else
            {
                throw new NotSupportedException("investigate...");
            }

            members[lambda.Type.FullName] = lambdaMethod;
            return new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), lambdaMethod.Name);
        }

        private CodeObjectCreateExpression Visit(NewExpression newExpression)
        {
            var arguments = Visit(newExpression.Arguments);
            return new CodeObjectCreateExpression(newExpression.Type.Name, arguments);
        }

        private CodeMethodInvokeExpression Visit(MemberInitExpression memberInitExpression)
        {
            var newExpression = Visit(memberInitExpression.NewExpression);
            var bindings = Visit(memberInitExpression.Bindings);

            var methodInvokeExpression = new CodeMethodInvokeExpression();
            methodInvokeExpression.Parameters.Add(newExpression);

            var arrayCreateExpression1 = new CodeArrayCreateExpression();
            for (int i = 0; i < memberInitExpression.Bindings.Count; i++)
            {
                var stringArrayType = new CodeTypeReference(typeof(string).ToString(), 1);
                var stringType = new CodeTypeReference(typeof(string).ToString());
                stringArrayType.ArrayElementType = stringType;
                arrayCreateExpression1.CreateType = stringArrayType;

                var primitiveExpression = new CodePrimitiveExpression();
                primitiveExpression.Value = memberInitExpression.Bindings[i].Member.Name;
                arrayCreateExpression1.Initializers.Add(primitiveExpression);
            }
            arrayCreateExpression1.Size = 0;
            methodInvokeExpression.Parameters.Add(arrayCreateExpression1);


            var arrayCreateExpression2 = new CodeArrayCreateExpression();
            for (int i = 0; i < memberInitExpression.Bindings.Count; i++)
            {
                var funcArrayType = new CodeTypeReference(typeof(Func<>).ToString(), 1);
                funcArrayType.TypeArguments.Add(new CodeTypeReference(typeof(object).ToString()));
                var funcType = new CodeTypeReference(typeof(Func<>).ToString());
                funcType.TypeArguments.Add(new CodeTypeReference(typeof(object).ToString()));
                funcArrayType.ArrayElementType = funcType;
                arrayCreateExpression2.CreateType = funcArrayType;

                arrayCreateExpression2.Initializers.Add(
                    (CodeExpression)Visit(((MemberAssignment)memberInitExpression.Bindings[i]).Expression));
            }
            arrayCreateExpression2.Size = 0;
            methodInvokeExpression.Parameters.Add(arrayCreateExpression2);


            var methodReferenceExpression = new CodeMethodReferenceExpression();
            methodReferenceExpression.MethodName = "Init";
            methodInvokeExpression.Method = methodReferenceExpression;

            return methodInvokeExpression;
        }

        private CodeObject Visit(ListInitExpression listInitExpression)
        {
            var newExpression = Visit(listInitExpression.NewExpression);
            var initializers = Visit(listInitExpression.Initializers);

            // うおーい(>_<)
            return newExpression;
        }

        private CodeArrayCreateExpression Visit(NewArrayExpression newArrayExpression)
        {
            var expressions = Visit(newArrayExpression.Expressions);
            return new CodeArrayCreateExpression(new CodeTypeReference(newArrayExpression.Type), expressions);
        }

        private CodeMethodInvokeExpression Visit(InvocationExpression invocationExpression)
        {
            var arguments = Visit(invocationExpression.Arguments);
            var expression = (CodeExpression)Visit(invocationExpression.Expression);
            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(expression, "Method"), arguments);
        }
    }
}
