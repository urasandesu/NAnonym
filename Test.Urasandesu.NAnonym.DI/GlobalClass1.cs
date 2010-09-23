using System;
using Test.Urasandesu.NAnonym.Etc;
using Urasandesu.NAnonym.Cecil.DI;

namespace Test.Urasandesu.NAnonym.Cecil.DI
{
    public class GlobalClass1 : GlobalClassBase
    {
        protected override GlobalClassBase OnSetup()
        {
            var class1 = new GlobalClass<Class1>();
            class1.
            Setup(the =>
            {
                the.Method((string value) =>
                {
                    return "Modified prefix at Class1.Print" + new Class2().Print(value) + "Modified suffix at Class1.Print";
                }).
                Instead(_ => _.Print);

                //the.Method<string, string>(_ => _.Print).As(value =>
                //{
                //    return "Modified prefix at Class1.Print" + new Class2().Print(value) + "Modified suffix at Class1.Print";
                //});
                //the.Method<string, string>(_ => _.Print).As(NewPrint);
            });
            //class1.Setup();
            return class1;
        }

        string NewPrint(string value)
        {
            return "Modified prefix at Class1.Print" + new Class2().Print(value) + "Modified suffix at Class1.Print";
        }
    }
}
