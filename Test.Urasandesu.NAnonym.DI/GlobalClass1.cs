using Test.Urasandesu.NAnonym.Etc;
using Urasandesu.NAnonym.Cecil.DI;

namespace Test.Urasandesu.NAnonym.DI
{
    public class GlobalClass1 : GlobalClassBase
    {
        protected override GlobalClassBase SetUp()
        {
            var class1 = new GlobalClass<Class1>();
            class1.SetUp(the =>
            {
                // TODO: 最終的にはこちらの I/F にする。
                //the.Method((string value) =>
                //{
                //    return "Modified prefix at Class1.Print" + new Class2().Print(value) + "Modified suffix at Class1.Print";
                //})
                //.Instead(_ => _.Print);

                the.Method<string, string>(_ => _.Print).As(value =>
                {
                    return "Modified prefix at Class1.Print" + new Class2().Print(value) + "Modified suffix at Class1.Print";
                });
                //the.Method<string, string>(_ => _.Print).As(NewPrint);
            });
            class1.Load();
            return class1;
        }

        string NewPrint(string value)
        {
            return "Modified prefix at Class1.Print" + new Class2().Print(value) + "Modified suffix at Class1.Print";
        }
    }
}
