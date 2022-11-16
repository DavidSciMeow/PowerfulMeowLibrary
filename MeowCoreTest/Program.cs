using Meow.FlightRadar;
using Meow.FlightRadar.Base;
using System;

namespace MeowCoreTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            _ = args;
            while (true)
            {
                Console.WriteLine("Search for Airport Hit 1, Airline Hit 2");
                var ss = Console.ReadLine();
                Console.Clear();
                if("1" == ss)
                {
                    CUtil.ConsoleSearchAirport();
                }
                else if("2" == ss)
                {
                    CUtil.ConsoleSearchAirline();
                }
                Console.WriteLine("Complete, return menu with any Hit, esc to Quit");
                if(Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    return;
                }
                Console.Clear();
            }
            
        }
    }
}

