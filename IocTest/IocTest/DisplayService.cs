using System;
using IocTest.Core;

namespace IocTest
{
    public class DisplayService : IDisplayService
    {
        public void SetColor(string color)
        {
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color);
        }

        public void Write(string message)
        {
            Console.WriteLine(message);
        }

        public void ResetColor()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}