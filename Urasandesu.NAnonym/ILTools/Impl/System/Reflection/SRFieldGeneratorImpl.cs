using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRFieldGeneratorImpl : SRFieldDeclarationImpl, IFieldGenerator
    {
        FieldBuilder fieldBuilder;
        public SRFieldGeneratorImpl(FieldBuilder fieldBuilder)
            : base(fieldBuilder)
        {
            this.fieldBuilder = fieldBuilder;
        }

        internal FieldBuilder FieldBuilder { get { return (FieldBuilder)base.FieldInfo; } }

    }
}
