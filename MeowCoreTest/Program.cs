using Meow.Util.Math;
using System;

namespace MeowCoreTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Fraction[] f = { 0.5, 1, 1.5, 2, -1 };
            Array.Sort(f);
            foreach(var i in f)
            {
                Console.Write($"{i} ");
            }
            Console.WriteLine();
            foreach (var i in f)
            {
                Console.Write($"{(double)i} ");
            }
        }
    }
}


