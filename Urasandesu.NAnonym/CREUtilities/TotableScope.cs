using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Mono.Cecil;

namespace Urasandesu.NAnonym.CREUtilities
{
    [Serializable]
    public sealed class TotableScope
    {
        readonly string prefix;
        readonly Dictionary<string, object> variables;

        //public TotableScope(MethodDefinition methodDef)
        //{
        //    prefix = MakeFieldNamePrefix(methodDef);
        //    variables = new Dictionary<string, object>();
        //}

        public TotableScope(IMethodGenerator methodDecl)
        {
            prefix = MakeFieldNamePrefix(methodDecl);
            variables = new Dictionary<string, object>();
        }

        public void Define<T>(Expression<Func<T>> variableRef, T value)
        {
            var fieldInfo = (FieldInfo)((MemberExpression)variableRef.Body).Member;

            // TODO: 指定された MethodDefinition
            // 名前を一意にするのもいいけど CustomAttribute で探したほうがいいのかも？
            //method.DeclaringType.Fields.Select(field=>field.CustomAttributes.Where(attribute=>attribute.
            // メソッド名$引数1$引数2$…$引数n$ローカル変数名 みたいな名前なら、まず被らないか。
            //string fieldName = MakeFieldName(method, fieldInfo.Name);
            //if (method.DeclaringType.GetField(fieldName) == null)
            //{
            //    // TODO: Required に持っていけるといい。
            //    throw new ArgumentException();
            //}

            string fieldName = prefix + fieldInfo.Name;
            variables[fieldName] = value;   // 上書きでいいや。
            //if (!variables.ContainsKey(fieldName))
            //{
            //    variables.Add(fieldInfo, value);
            //}
        }

        //// TODO: あとでまとめる
        //public static string MakeFieldNamePrefix(MethodInfo method)
        //{
        //    var fieldName = new StringBuilder();
        //    fieldName.Append(method.Name);
        //    fieldName.Append("<>");
        //    fieldName.Append(method.ReturnType.FullName.Replace(".", "$"));
        //    fieldName.Append("<>");
        //    fieldName.Append(string.Join("<>", method.GetParameters().Select(parameter => parameter.ParameterType.FullName.Replace(".", "$")).ToArray()));
        //    fieldName.Append("<>");
        //    return fieldName.ToString();
        //}

        //public static string MakeFieldNamePrefix(MethodDefinition method)
        //{
        //    var fieldName = new StringBuilder();
        //    fieldName.Append(method.Name);
        //    fieldName.Append("<>");
        //    fieldName.Append(method.ReturnType.FullName.Replace(".", "$"));
        //    fieldName.Append("<>");
        //    fieldName.Append(string.Join("<>", method.Parameters.Select(parameter => parameter.ParameterType.FullName.Replace(".", "$")).ToArray()));
        //    fieldName.Append("<>");
        //    return fieldName.ToString();
        //}

        public static string MakeFieldNamePrefix(IMethodBaseGenerator methodDecl)
        {
            var fieldName = new StringBuilder();
            fieldName.Append(methodDecl.Name);
            fieldName.Append("<>");
            //fieldName.Append(methodDecl.ReturnType.FullName.Replace(".", "$"));
            //fieldName.Append("<>");
            fieldName.Append(string.Join("<>", methodDecl.Parameters.Select(parameter => parameter.ParameterType.FullName.Replace(".", "$")).ToArray()));
            fieldName.Append("<>");
            return fieldName.ToString();
        }

        //public static string MakeFieldName(MethodInfo method, string localVariableName)
        //{
        //    return MakeFieldNamePrefix(method) + localVariableName;
        //}

        //public static string MakeFieldName(MethodDefinition method, string localVariableName)
        //{
        //    return MakeFieldNamePrefix(method) + localVariableName;
        //}

        public static string MakeFieldName(IMethodBaseGenerator methodDecl, string localVariableName)
        {
            return MakeFieldNamePrefix(methodDecl) + localVariableName;
        }

        public void Bind(object instance)
        {
            var t = instance.GetType();
            foreach (var fieldNameValuePair in variables)
            {
                string fieldName = fieldNameValuePair.Key;
                object value = fieldNameValuePair.Value;

                t.GetField(fieldName).SetValue(instance, value);
            }
        }

        // TODO: static メンバ用。
        public void Bind<T>()
        {
            throw new NotImplementedException();
        }
    }
}
