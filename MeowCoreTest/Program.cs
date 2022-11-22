using Meow.FlightRadar;
using Meow.FlightRadar.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MeowCoreTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            _ = args;
            //var li = new Flight("CES7777");
            while (true)
            {
                Console.WriteLine("Search for Airport Hit 1, Airline Hit 2");
                var ss = Console.ReadKey().Key;
                Console.Clear();
                if(ss is ConsoleKey.D1 or ConsoleKey.NumPad1)
                {
                    CUtil.ConsoleSearchAirport();
                }
                else if(ss is ConsoleKey.D2 or ConsoleKey.NumPad2)
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

