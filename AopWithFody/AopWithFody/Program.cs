namespace AopWithFody
{
    class Program
    {
        static void Main()
        {
            Method1();
            Method2();
        }

        [LogMethodEntry]
        static void Method1()
        {

        }

        static void Method2()
        {

        }
    }
}
