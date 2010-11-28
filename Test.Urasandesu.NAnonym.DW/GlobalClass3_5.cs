using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.DW;
using Urasandesu.NAnonym.Cecil.DW;
using Test.Urasandesu.NAnonym.Etc;

namespace Test.Urasandesu.NAnonym.Cecil.DW
{
    public class GlobalClass3_5 : GlobalClass
    {
        protected override DependencyClass OnRegister()
        {
            var class3GlobalClass = new GlobalClass<Class3>();
            class3GlobalClass.Setup(the =>
            {
                var simpleType1 = default(SimpleType1);
                the.Field(() => simpleType1).As(_ => new SimpleType1());
                the.Method<int, int, int>(_ => _.Add).IsReplacedBy(
                (x, y) =>
                {
                    int result = simpleType1.Increase() + x + y;
                    return result;
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

    public class GlobalClass3_6 : GlobalClass
    {
        protected override DependencyClass OnRegister()
        {
            var class3GlobalClass = new GlobalClass<Class3>();
            class3GlobalClass.Setup(the =>
            {
                the.Method<int, int, int>(_ => _.Add).IsReplacedBy(
                (base_Add, x, y) =>
                {
                    return base_Add(x, y) + x + y;
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

    public class GlobalClass3_7 : GlobalClass
    {
        protected override DependencyClass OnRegister()
        {
            var class3GlobalClass = new GlobalClass<Class3>();
            class3GlobalClass.Setup(the =>
            {
                int value = 2;

                the.Field(() => value).As(value);
                the.Method<int, int, int>(_ => _.Add).IsReplacedBy(
                (base_Add, x, y) =>
                {
                    return value += base_Add(x, y) + x + y;
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
