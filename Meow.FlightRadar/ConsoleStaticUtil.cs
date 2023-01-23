using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            var lad = SBase.LiveAirportDoc(airportname);
            if (SBase.DeterminAirPortExist(lad))
            {
                var doc = SBase.LiveAirportWeatherDoc(airportname);
                var mod = SBase.GetAirportWeather(doc);
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
            var doc = SBase.LiveAirportDoc(Console.ReadLine()??string.Empty);
            var issec = SBase.DeterminAirportSecondaryLoc(doc);
            Console.WriteLine($"{(issec ? "SECONDARY OBJECTIVE" : "LIVE OBJECTIVE")}");
            var ibbd = SBase.GetBoard(doc, BoardType.arrivals);
            Console.WriteLine("[ IDENTITY / TYPE ]     DEPATURE TIME      ->     ARRIVE TIME        {(->/<-) PLACE }");
            Console.WriteLine("--- ARRIVALS ---");
            foreach (var i in ibbd)
            {
                Console.WriteLine(i.ToString());
            }
            var obbd = SBase.GetBoard(doc, BoardType.departures);
            Console.WriteLine("--- DEPARTURES ---");
            foreach (var i in obbd)
            {
                Console.WriteLine(i.ToString());
            }
            var ebbd = SBase.GetBoard(doc, BoardType.enroute);
            Console.WriteLine("--- ENROUNTE PLAN ---");
            foreach (var i in ebbd)
            {
                Console.WriteLine(i.ToString());
            }
            var sbbd = SBase.GetBoard(doc, BoardType.scheduled);
            Console.WriteLine("--- SCHEDULED PLAN ---");
            foreach (var i in sbbd)
            {
                Console.WriteLine(i.ToString());
            }
            Console.WriteLine("\n");
        }
        /// <summary>
        /// 获取机场的天气情况通波字符串
        /// </summary>
        public static void ConsoleGetAirportWeatherMsg()
        {
            Console.Write("Input the ICAO Code:");
            var airportname = Console.ReadLine() ?? string.Empty;
            var lad = SBase.LiveAirportDoc(airportname);
            if (SBase.DeterminAirPortExist(lad))
            {
                var doc = SBase.LiveAirportWeatherDoc(airportname);
                Console.WriteLine("-- Getting NetWork Info --");
                if (SBase.DeterminAirportLiveWeather(doc))
                {
                    var sd = SBase.GetLiveAirportWeather(doc);
                    Console.WriteLine("ATIS:");
                    foreach (var i in sd ?? Array.Empty<string>())
                    {
                        Console.WriteLine(i);
                    }
                }
                var x = SBase.GetAirportWeatherMsg(doc);
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

        /// <summary>
        /// 搜索机场
        /// </summary>
        public static void ConsoleSearchAirport()
        {
            Console.WriteLine("[....]");
            Console.Write("Input the predicate word:");
            var l = SearchBase.SearchAiport(Console.ReadLine()??string.Empty);
            Console.WriteLine("Num\t{__OPS__} [ICAO/IATA] Name");
            Console.WriteLine();
            int i = 0;
            foreach (var v in l)
            {
                Console.WriteLine($"{i++}\t{v}");
            }
            Console.WriteLine("\n");
            Console.WriteLine("--Search Complete--");
            Console.Write("select num:");
            var k = Console.ReadLine();
            Console.Clear();
            if (int.TryParse(k, out var num))
            {
                Console.WriteLine("[Waiting For Data Returning...]");
                var li = l[num].Detail();
                Console.Clear();
                Console.WriteLine("[Data Returned]");
                Console.Clear();
                Console.WriteLine(li.ToString());
            }
            else
            {
                Console.WriteLine($"var {k} is not a specific number");
            }
        }
        /// <summary>
        /// 搜索航班
        /// </summary>
        public static void ConsoleSearchAirline()
        {
            Console.WriteLine("[....]");
            Console.Write("Input the predicate word:");
            var l = SearchBase.SearchAirline(Console.ReadLine() ?? string.Empty);
            Console.WriteLine("--Search Complete--");
            Console.WriteLine("Num\t[ AL IDENT ] [TAIL?] [MAJ]  NAME");
            Console.WriteLine();
            int i = 0;
            foreach (var v in l)
            {
                Console.WriteLine($"{i++}\t{v}");
            }
            Console.WriteLine("--Search Complete--");
            Console.Write("select num:");
            var k = Console.ReadLine();
            Console.Clear();
            if (int.TryParse(k, out var num))
            {
                Console.WriteLine("[Waiting For Data Returning...]");
                var li = l[num].Detail();
            Console.Clear();
                Console.WriteLine("[Data Returned]");
                Console.Clear();
                Console.WriteLine("----NowActivity----");
                Console.WriteLine(li.NowActivity?.ToString());
                Console.WriteLine("----ActivityLog----");
                foreach(var xx in li.ActivityLog)
                {
                    Console.WriteLine($"\n ---Timestamp:{xx.Timestamp}---");
                    Console.WriteLine(xx.ToString());
                }
            }
            else
            {
                Console.WriteLine($"var {k} is not a specific number");
            }
            Console.WriteLine("\n");
        }
        /// <summary>
        /// 更新全局数据
        /// </summary>
        public static void UpdateAllConcurrentData()
        {
            Console.WriteLine("[Waiting For Data Returning...]");
            if (SBase.AllFleet == null)
            {
                SBase.AllFleet = SBase.GetAllFleet(SBase.LiveAllFleetDoc());
            }
            Console.Clear();
            Console.WriteLine("[Data Returned]");
        }
        /// <summary>
        /// 搜索航司
        /// </summary>
        public static void ConsoleSearchFleet()
        {

            Console.WriteLine("[Waiting For Data Returning...]");
            if (SBase.AllFleet == null)
            {
                SBase.AllFleet = SBase.GetAllFleet(SBase.LiveAllFleetDoc());
            }
            Console.Clear();
            Console.WriteLine("[Data Returned]");
            Console.Write("Input the predicate word (leave Empty to show all):");
            var fleet = Console.ReadLine() ?? string.Empty;
            Console.Clear();
            Console.WriteLine("Num\tN#Fl [Pre] Name");
            Console.WriteLine();
            int i = 0;
            var k = from a in SBase.AllFleet where (a.AirlineOperator.ToLower().Contains(fleet.ToLower()) || a.Predix.ToLower().Contains(fleet.ToLower())) select a;
            foreach (var v in k)
            {
                Console.WriteLine($"{i++}\t{v}");
                if (i % 10 == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine($" [{i}/{k.Count()}] For Next 10 Result Hit Any Key...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine("Num\tN#Fl [Pre] Name");
                    Console.WriteLine();
                }
            }
            Console.WriteLine("\n");
            Console.WriteLine("--Search Complete--");
        }
    }
}
