using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Urasandesu.NAnonym.Etc
{
    public class MethodTestClass1
    {
        public void Action1()
        {
            Console.WriteLine("Hello, World!!");
        }

        public void Action2LocalVariable()
        {
            int i = 100;
            Console.WriteLine("i.ToString() = {0}", i.ToString());
        }

        public void Action3Exception()
        {
            int i = 100;
            int j = 0;
            try
            {
                Console.WriteLine("i / j = {0}", i / j);
            }
            catch (DivideByZeroException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Action4Generic<T>()
        {
            Console.WriteLine(typeof(T).ToString());
        }

        public void Action5NoBody()
        {
            Action1();
            Action2LocalVariable();
            Action3Exception();
            Action4Generic<string>();
        }

        public int Func1Parameters(int value)
        {
            return value + value * value;
        }
    }
}
