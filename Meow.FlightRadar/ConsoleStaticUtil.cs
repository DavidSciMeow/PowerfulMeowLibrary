using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.FlightRadar
{
    /// <summary>
    /// Console Util
    /// 控制台查询用程序
    /// </summary>
    public class CUtil
    {
        /// <summary>
        /// 获取机场的天气情况实体类
        /// </summary>
        public static void ConsoleGetAirportWeather()
        {
            Console.Write("Input the ICAO Code:");
            var airportname = Console.ReadLine() ?? string.Empty;
            var lad = SAirport.LiveAirportDoc(airportname);
            if (SAirport.DeterminAirPortExist(lad))
            {
                var doc = SAirport.LiveAirportWeatherDoc(airportname);
                var mod = SAirport.GetAirportWeather(doc);
                foreach (var i in mod)
                {
                    Console.WriteLine(i.ToString());
                }
            }
            Console.WriteLine();
        }
        /// <summary>
        /// 获取机场进出港安排表
        /// </summary>
        public static void ConsoleGetLiveAirportBoard()
        {
            Console.Write("Input the ICAO Code:");
            var doc = SAirport.LiveAirportDoc(Console.ReadLine()??string.Empty);
            var issec = SAirport.DeterminAirportSecondaryLoc(doc);
            Console.WriteLine($"{(issec ? "SECONDARY OBJECTIVE" : "LIVE OBJECTIVE")}");
            var ibbd = SAirport.GetBoard(doc, BoardType.arrivals);
            Console.WriteLine("[ IDENTITY / TYPE ]     DEPATURE TIME      ->     ARRIVE TIME        {(->/<-) PLACE }");
            Console.WriteLine("--- ARRIVALS ---");
            foreach (var i in ibbd)
            {
                Console.WriteLine(i.ToString());
            }
            var obbd = SAirport.GetBoard(doc, BoardType.departures);
            Console.WriteLine("--- DEPARTURES ---");
            foreach (var i in obbd)
            {
                Console.WriteLine(i.ToString());
            }
            var ebbd = SAirport.GetBoard(doc, BoardType.enroute);
            Console.WriteLine("--- ENROUNTE PLAN ---");
            foreach (var i in ebbd)
            {
                Console.WriteLine(i.ToString());
            }
            var sbbd = SAirport.GetBoard(doc, BoardType.scheduled);
            Console.WriteLine("--- SCHEDULED PLAN ---");
            foreach (var i in sbbd)
            {
                Console.WriteLine(i.ToString());
            }
            Console.WriteLine("\n");
        }
        /// <summary>
        /// 搜索机场
        /// </summary>
        public static void ConsoleSearchAirport()
        {
            Console.Write("Input the predicate word:");
            var l = SearchBase.SearchAiport(Console.ReadLine()??string.Empty);
            Console.WriteLine("\t{__OPS__} [ICAO/IATA] Name");
            Console.WriteLine("-----------------------------");
            int i = 0;
            foreach (var v in l)
            {
                Console.WriteLine($"{i++}\t{v}");
            }
            Console.WriteLine("\n");
            Console.WriteLine("--Search Complete--");
            Console.Write("select num:");
            var k = Console.ReadLine();
            if (int.TryParse(k, out var num))
            {
                var li = l[num].AirportDetail();
                Console.WriteLine(li.ToString());
            }
            else
            {
                Console.WriteLine($"var {k} is not a specific number");
            }
        }
        /// <summary>
        /// 输出一个机场
        /// </summary>
        public static void ConsoleOutputAirport()
        {
            Console.Write("Input ICAO code:");
            var l = new Base.Airport(Console.ReadLine() ?? string.Empty);
            Console.WriteLine(l.ToString());
        }
        /// <summary>
        /// 搜索航班
        /// </summary>
        public static void ConsoleSearchAirline()
        {
            Console.Write("Input the predicate word:");
            var l = SearchBase.SearchAirline(Console.ReadLine() ?? string.Empty);
            Console.WriteLine("--Search Complete--");
            Console.WriteLine("[ AL IDENT ] [TAIL?] [MAJ]  NAME");
            Console.WriteLine("-----------------------------");
            foreach (var v in l)
            {
                Console.WriteLine(v.ToString());
            }
            Console.WriteLine("\n");
        }
        /// <summary>
        /// 获取机场的天气情况通波字符串
        /// </summary>
        public static void ConsoleGetAirportWeatherMsg()
        {
            Console.Write("Input the ICAO Code:");
            var airportname = Console.ReadLine()??string.Empty;
            var lad = SAirport.LiveAirportDoc(airportname);
            if (SAirport.DeterminAirPortExist(lad))
            {
                var doc = SAirport.LiveAirportWeatherDoc(airportname);
                Console.WriteLine("-- Getting NetWork Info --");
                if (SAirport.DeterminAirportLiveWeather(doc))
                {
                    var sd = SAirport.GetLiveAirportWeather(doc);
                    Console.WriteLine("ATIS:");
                    foreach (var i in sd ?? Array.Empty<string>())
                    {
                        Console.WriteLine(i);
                    }
                }
                var x = SAirport.GetAirportWeatherMsg(doc);
                Console.WriteLine("---Getting History And Predict---");
                Console.WriteLine("LOCAL:");
                foreach (var i in x)
                {
                    Console.WriteLine(i);
                }
            }
            else
            {
                Console.WriteLine("NO SUCH AIRPORT");
            }
            Console.WriteLine("\n");
        }
    }
}
