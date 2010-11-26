﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools.Impl.System.Reflection;
using Urasandesu.NAnonym.ILTools.Mixins.Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.ILTools.Mixins.System.Reflection.Emit
{
    public static class ModuleBuilderMixin
    {
        public static ITypeGenerator AddType(this ModuleBuilder moduleBuilder, string name)
        {
            var typeBuilder = moduleBuilder.DefineType(name);
            return new SRTypeGeneratorImpl(typeBuilder);
        }
    }
}
