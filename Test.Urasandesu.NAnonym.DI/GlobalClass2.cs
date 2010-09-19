using Test.Urasandesu.NAnonym.Etc;
using Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.Cecil.DI;

namespace Test.Urasandesu.NAnonym.DI
{
    public class GlobalClass2 : GlobalClassBase
    {
        protected override GlobalClassBase SetUp()
        {
            var class2 = new GlobalClass<Class2>();
            class2.SetUp(the =>
            {
                // TODO: 最終的にはこちらの I/F にする。
                //the.Method((string value) =>
                //{
                //    return "Modified prefix at Class2.Print" + value + "Modified suffix at Class2.Print";
                //})
                //.Instead(_ => _.Print);
                the.Method<string, string>(_ => _.Print).As(value =>
                {
                    return "Modified prefix at Class2.Print" + value + "Modified suffix at Class2.Print";
                });
            });
            class2.Load();
            return class2;
        }
    }
}
