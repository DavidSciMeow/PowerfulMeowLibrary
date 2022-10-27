using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Meow.FlightRadar;
using Meow.Util;
using Meow.TrainRadar;
using Meow.Util.Encrypt;

namespace MeowCoreTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //var doc = FRGet.LiveAirportDoc("KPKD");
            //var issec = FRGet.DeterminAirportSecondaryLoc(doc);
            //var ibbd = FRGet.GetBoard(doc, BoardType.arrivals);

            var airportname = "SWA";
            if (FRGet.DeterminAirPortExist(airportname))
            {
                var doc = FRGet.LiveAirportWeatherDoc(airportname);

                if (FRGet.DeterminAirportLiveWeather(doc))
                {
                    var sd = FRGet.GetLiveAirportWeather(doc);
                    Console.WriteLine("Live:");
                    foreach (var i in sd)
                    {
                        Console.WriteLine(i);
                    }
                }

                Console.WriteLine("-----");
                Console.WriteLine("History:");
                var x = FRGet.GetAirportWeatherMsg(doc);
                foreach (var i in x)
                {
                    Console.WriteLine(i);
                }
            }
            else
            {
                Console.WriteLine("NO SUCH AIRPORT");
            }
        }
    }
}

