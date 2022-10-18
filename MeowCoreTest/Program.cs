using System;
using System.Collections.Generic;
using System.Globalization;
using HtmlAgilityPack;
using Meow.FlightRadar;
using Meow.TrainRadar;

namespace MeowCoreTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var s = "京";
            SearchBase.Lang = LangPref.zhcn;
            var sx = TRGet.Feature(s);
            for (int i = 0; i < sx.Length; i++)
            {
                FeatureMatchResult x = sx[i];
                Console.WriteLine($"{i}:{x.Name}");
            }
            var k = int.Parse(Console.ReadLine());
            var d = sx[k].GetIfRailInfo();
            Console.WriteLine(d);




            //var doc = DocBase.LiveAirportDoc("KPKD");
            //var issec = FRGet.DeterminAirportSecondaryLoc(doc);
            //var ibbd = FRGet.GetBoard(doc, BoardType.arrivals);
            /*
            var airportname = "ZSSS";
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
            */
        }
    }
}

