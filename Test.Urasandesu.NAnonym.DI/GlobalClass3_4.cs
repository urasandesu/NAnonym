using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.Cecil.DI;
using Test.Urasandesu.NAnonym.Etc;

namespace Test.Urasandesu.NAnonym.Cecil.DI
{
    public class GlobalClass3_4 : GlobalClass
    {
        int value = 0;
        protected override DependencyClass OnRegister()
        {
            var class3GlobalClass = new GlobalClass<Class3>();
            class3GlobalClass.Setup(the =>
            {
                the.Method<int, int, int>(_ => _.Add).IsReplacedBy(
                (x, y) =>
                {
                    return value += x + y;
                });
            });
            return class3GlobalClass;
        }

        protected override string CodeBase
        {
            get { return typeof(Class3).Assembly.CodeBase; }
        }

        protected override string Location
        {
            get { return typeof(Class3).Assembly.Location; }
        }
    }
}
