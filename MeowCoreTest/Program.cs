using System;
using System.Linq;

namespace MeowCoreTest
{
    internal class Program
    {
		public static void Main(string[] args)
        {
            while (true)
            {
                var k = Meow.Weather.CN.Interpreter.RectifyWord(Console.ReadLine());
                Console.WriteLine(k.Data[0].ImgUrl);
            }
        }
    }
}

