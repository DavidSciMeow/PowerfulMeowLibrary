using Meow.FlightRadar;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MeowCoreTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            _ = args;
            //var k = Meow.Util.Network.Http.Get.String(UrlMapping.LiveFleet).GetAwaiter().GetResult();
            //Console.WriteLine(k);
            while (true)
            {
                //CUtil.ConsoleSearchAirline();
                CUtil.ConsoleSearchAirport();
                //CUtil.ConsoleOutputAirport();
                //CUtil.ConsoleGetAirportWeatherMsg();
                //CUtil.ConsoleGetAirportWeather();
                //CUtil.ConsoleGetLiveAirportBoard();
            }
        }
    }
}

