using Meow.FlightRadar;
using Meow.Util.Network;
using System;

namespace MeowCoreTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            _ = args;
            Console.WriteLine("[...]");
            while (true)
            {
                Console.Write("Search for Airport Hit 1, Airline Hit 2, Fleet Hit 3, Update All Concurrent Data Hit 0");
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
                else if(ss is ConsoleKey.D3 or ConsoleKey.NumPad3)
                {
                    CUtil.ConsoleSearchFleet();
                }
                else if (ss is ConsoleKey.D0 or ConsoleKey.NumPad0)
                {
                    CUtil.UpdateAllConcurrentData();
                }
                else
                {
                    Console.WriteLine("[...No Key Pressed...]");
                    continue;
                }
                Console.WriteLine("Complete, Ctrl+C to Quit. Return Main Menu with any Other Hit.");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}

