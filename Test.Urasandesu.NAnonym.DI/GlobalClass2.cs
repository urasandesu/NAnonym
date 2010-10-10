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
}
