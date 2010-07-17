
namespace Test.Urasandesu.NAnonym.Etc
{
    public class Class1
    {
        public string Print(string value)
        {
            return "Hello, " + new Class2().Print(value) + " World !!";
        }
    }
}
