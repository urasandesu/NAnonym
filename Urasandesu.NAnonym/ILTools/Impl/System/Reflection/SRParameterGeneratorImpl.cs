using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRParameterGeneratorImpl : SRParameterDeclarationImpl, IParameterGenerator
    {
        ParameterBuilder parameterBuilder;
        public SRParameterGeneratorImpl(ParameterBuilder parameterBuilder)
            : base(parameterBuilder.Name, ExportType(parameterBuilder), parameterBuilder.Position)
        {
            this.parameterBuilder = parameterBuilder;
        }

        static Type ExportType(ParameterBuilder parameterBuilder)
        {
            var parameterBuilderType = typeof(ParameterBuilder);
            var m_methodBuilderField = parameterBuilderType.GetField("m_methodBuilder", BindingFlags.NonPublic | BindingFlags.Instance);
            var m_methodBuilder = (MethodBuilder)m_methodBuilderField.GetValue(parameterBuilder);

            var methodBuilderType = typeof(MethodBuilder);
            var m_parameterTypesField = methodBuilderType.GetField("m_parameterTypes", BindingFlags.NonPublic | BindingFlags.Instance);
            var m_parameterTypes = (Type[])m_parameterTypesField.GetValue(m_methodBuilder);

            return m_parameterTypes[parameterBuilder.Position - 1];
        }

        //public static explicit operator SRParameterGeneratorImpl(ParameterBuilder parameterBuilder)
        //{
        //    return new SRParameterGeneratorImpl(parameterBuilder);
        //}

        #region IParameterGenerator メンバ

        public new ITypeGenerator ParameterType
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
