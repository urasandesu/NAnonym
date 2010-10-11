using Test.Urasandesu.NAnonym.Etc;
using Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.Cecil.DI;

namespace Test.Urasandesu.NAnonym.Cecil.DI
{
    public class GlobalClass1 : GlobalClass
    {
        protected override DependencyClass OnRegister()
        {
            var class1GlobalClass = new GlobalClass<Class1>();
            class1GlobalClass.Setup(the =>
            {
                the.Method<string, string>(_ => _.Print).IsReplacedBy(
                value =>
                {
                    return "Modified prefix at Class1.Print" + new Class2().Print(value) + "Modified suffix at Class1.Print";
                });
            });
            return class1GlobalClass;
        }

        protected override string CodeBase
        {
            get { return typeof(Class1).Assembly.CodeBase; }
        }

        protected override string Location
        {
            get { return typeof(Class2).Assembly.Location; }
        }
    }
}
