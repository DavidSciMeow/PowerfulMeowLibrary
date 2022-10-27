using System;
using Meow.TrainRadar;

namespace MeowCoreTest
{
    internal class Program
    {
        public static void TrainSearchConsole()
        {
            Console.Write("\n输入待查字符 (按 Ctrl-C 退出):");
            var s = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(s))
            {
                Console.WriteLine("请输入字符");
                return;
            }
            var d = new Search(s);
            if (d.HaveValue)
            {
                if (d.Data is FeatureMatchResult[])
                {
                    FeatureMatchResult[] array = (FeatureMatchResult[])d.Data;
                    for (int i = 0; i < array.Length; i++)
                    {
                        FeatureMatchResult a = array[i];
                        Console.WriteLine($"[{i}] {a}");
                    }
                    Console.Write("-- 选择一项 (输入 -1 取消):");
                    var ss = Console.ReadLine();
                    if (int.TryParse(ss, out var ii))
                    {
                        if (ii == -1) { }
                        else if (ii < 0 || ii >= array.Length)
                        {
                            Console.WriteLine("输入项目不在列表内");
                        }
                        else
                        {
                            FeatureMatchResult a = array[ii];
                            if (a.Type == SearchType.STATION)
                            {
                                Console.WriteLine(a.GetStationInfo()?.ToString());
                            }
                            else if (a.Type == SearchType.RAIL)
                            {
                                Console.WriteLine(a.GetRailInfo()?.ToString());
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"输入 {ii} 非数字");
                    }
                }
                else
                {
                    TrainMatchResult[] array = (TrainMatchResult[])d.Data;
                    for (int i = 0; i < array.Length; i++)
                    {
                        TrainMatchResult a = array[i];
                        Console.WriteLine($"[{i}] {a}");
                    }
                    Console.Write("-- 选择一项 (输入 -1 取消):");
                    var ss = Console.ReadLine();
                    if (int.TryParse(ss, out var ii))
                    {
                        if (ii == -1) { }
                        else if (ii < 0 || ii >= array.Length)
                        {
                            Console.WriteLine("输入项目不在列表内");
                        }
                        else
                        {
                            TrainMatchResult a = array[ii];
                            Console.WriteLine(a.GetRouteInfo()?.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine($"输入 {ii} 非数字");
                    }
                }
            }
            else
            {
                Console.WriteLine("搜索无结果");
            }
            Console.WriteLine();
        }
        public static void Main(string[] args)
        {
            SearchBase.Lang = LangPref.zhcn;
            while (true)
            {
                TrainSearchConsole();
            }
            //var doc = FRGet.LiveAirportDoc("KPKD");
            //var issec = FRGet.DeterminAirportSecondaryLoc(doc);
            //var ibbd = FRGet.GetBoard(doc, BoardType.arrivals);

            //var airportname = "SWA";
            //if (FRGet.DeterminAirPortExist(airportname))
            //{
            //    var doc = FRGet.LiveAirportWeatherDoc(airportname);

            //    if (FRGet.DeterminAirportLiveWeather(doc))
            //    {
            //        var sd = FRGet.GetLiveAirportWeather(doc);
            //        Console.WriteLine("Live:");
            //        foreach (var i in sd)
            //        {
            //            Console.WriteLine(i);
            //        }
            //    }

            //    Console.WriteLine("-----");
            //    Console.WriteLine("History:");
            //    var x = FRGet.GetAirportWeatherMsg(doc);
            //    foreach (var i in x)
            //    {
            //        Console.WriteLine(i);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("NO SUCH AIRPORT");
            //}
        }
    }
}

