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
            var li = new Flight("CES7777");
            Console.WriteLine(li?.NowActivity?.Origin?.ToString());
            Console.WriteLine(li?.NowActivity?.Destination?.ToString());
            Console.WriteLine(li?.NowActivity?.FlightPlan?.ToString());
            Console.WriteLine(li?.NowActivity?.GetBoolMap());
            Console.WriteLine(li?.NowActivity?.GetTimeTables());
            Console.WriteLine(li?.NowActivity?.GetTrack());


            /*
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
            */
        }
    }
}

