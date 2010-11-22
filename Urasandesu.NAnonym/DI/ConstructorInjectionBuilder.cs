﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    class ConstructorInjectionBuilder : InjectionBuilder
    {
        public new ConstructorInjectionDefiner ParentDefiner { get { return (ConstructorInjectionDefiner)base.ParentDefiner; } }
        public ConstructorInjectionBuilder(ConstructorInjectionDefiner parentDefiner)
            : base(parentDefiner)
        {
        }

        public override void Construct()
        {
            throw new NotImplementedException();
        }
    }
}