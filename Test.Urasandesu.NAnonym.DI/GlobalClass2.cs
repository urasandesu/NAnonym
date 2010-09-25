using Test.Urasandesu.NAnonym.Etc;
using Urasandesu.NAnonym.Cecil.DI;

namespace Test.Urasandesu.NAnonym.Cecil.DI
{
    public class GlobalClass2 : GlobalClassBase
    {
        protected override GlobalClassBase OnSetup()
        {
            var class2 = new GlobalClass<Class2>();
            class2.
            Setup(the =>
            {
                the.Method((string value) =>
                {
                    return "Modified prefix at Class2.Print" + value + "Modified suffix at Class2.Print";
                }).
                Instead(_ => _.Print);
            });
            return class2;
        }
    }
}
