using Test.Urasandesu.NAnonym.Etc;
using Urasandesu.NAnonym.Cecil.DI;

namespace Test.Urasandesu.NAnonym.Cecil.DI
{
    public class GlobalClass2 : GlobalClass
    {
        protected override GlobalClass OnRegister()
        {
            var class2GlobalClass = new GlobalClass<Class2>();
            class2GlobalClass.Setup(the =>
            {
                the.Method<string, string>(_ => _.Print).IsReplacedBy(
                value =>
                {
                    return "Modified prefix at Class2.Print" + value + "Modified suffix at Class2.Print";
                });
            });
            return class2GlobalClass;
        }

        protected override string CodeBase
        {
            get { return typeof(Class2).Assembly.CodeBase; }
        }

        protected override string Location
        {
            get { return typeof(Class2).Assembly.Location; }
        }
    }

    public class GlobalClass3_1 : GlobalClass
    {
        protected override GlobalClass OnRegister()
        {
            var class3GlobalClass = new GlobalClass<Class3>();
            class3GlobalClass.Setup(the =>
            {
                the.Method<int, int, int>(_ => _.Add).IsReplacedBy(
                (x, y) =>
                {
                    return x + y + x + y;
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

    public class GlobalClass3_2 : GlobalClass
    {
        protected override GlobalClass OnRegister()
        {
            var class3GlobalClass = new GlobalClass<Class3>();
            class3GlobalClass.Setup(the =>
            {
                int value = 0;

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

    public class GlobalClass3_3 : GlobalClass
    {
        protected override GlobalClass OnRegister()
        {
            int value = 0;
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

    public class GlobalClass3_4 : GlobalClass
    {
        int value = 0;
        protected override GlobalClass OnRegister()
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
