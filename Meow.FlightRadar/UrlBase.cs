using System;
using System.Data;
using System.Linq;
using System.Reflection;
using Meow.Util.Network.Http;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Globalization;

/*
 https://flightaware.com/ajax/trackpoll.rvt?token=dd1320656957446e5ca0b3670908242456b91d9b1ff13341--b6abd5f04a6bf8cea28a8fb8ec4cfbe3c4be8887&locale=zh_CN&summary=1
 */




namespace Meow.FlightRadar
{
    /// <summary>
    /// 机场资源类型
    /// </summary>
    public enum AirportResourceType
    {
        /// <summary>
        /// 天气
        /// </summary>
        weather,
    }
    /// <summary>
    /// 机场班次类型
    /// </summary>
    public enum BoardType
    {
        /// <summary>
        /// 进港
        /// </summary>
        arrivals,
        /// <summary>
        /// 离港
        /// </summary>
        departures,
        /// <summary>
        /// 正在/计划飞往
        /// </summary>
        enroute,
        /// <summary>
        /// 计划离港
        /// </summary>
        scheduled,
    }
    /// <summary>
    /// 航班模型
    /// </summary>
    public struct BoardModel
    {
        /// <summary>
        /// 呼号
        /// </summary>
        public string Identity;
        /// <summary>
        /// 机型
        /// </summary>
        public string AircraftType;
        /// <summary>
        /// 计划地点
        /// </summary>
        public string Place;
        /// <summary>
        /// 离港时间
        /// </summary>
        public (DateTime? Time, string Zone) Dept;
        /// <summary>
        /// 到港时间
        /// </summary>
        public (DateTime? Time, string Zone) Arrv;
    }

    /// <summary>
    /// 天气终端模型
    /// </summary>
    public struct WeatherModel
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string Date;
        /// <summary>
        /// 时间
        /// </summary>
        public string Time;
        /// <summary>
        /// 时区
        /// </summary>
        public string Zone;
        /// <summary>
        /// 飞行规则
        /// </summary>
        public string FlightRules;
        /// <summary>
        /// 风向
        /// </summary>
        public string WindDir;
        /// <summary>
        /// 风速
        /// </summary>
        public string Speed;
        /// <summary>
        /// 天气类型
        /// </summary>
        public string WeaType;
        /// <summary>
        /// AGL高度
        /// </summary>
        public string HeightAGL;
        /// <summary>
        /// 能见度
        /// </summary>
        public string Visibility;
        /// <summary>
        /// 摄氏温度
        /// </summary>
        public string TempDegC;
        /// <summary>
        /// 华氏温度
        /// </summary>
        public string TempDegF;
        /// <summary>
        /// 摄氏露点
        /// </summary>
        public string DewPointC;
        /// <summary>
        /// 华氏露点
        /// </summary>
        public string DewPointF;
        /// <summary>
        /// 相对湿度
        /// </summary>
        public string RelHumid;
        /// <summary>
        /// 气压(修正海压百帕斯卡)
        /// </summary>
        public string Pressure;
        /// <summary>
        /// 密度高度
        /// </summary>
        public string DensityAltitude;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks;
    }

    public static class FRGet
    {
        public static string CN = "http://zh.flightaware.com/";
        public static string LiveAirport(string airportname) => CN + $"live/airport/{airportname}";
        public static string LiveAirPortResources(string airportname, AirportResourceType? type = null) => CN + $"resources/airport/{airportname}/{type.ToString() ?? ""}";
       
        public static HtmlDocument LiveAirportDoc(string airportname) => new HtmlWeb().Load(LiveAirport(airportname));
        public static HtmlDocument LiveAirportWeatherDoc(string airportname) => new HtmlWeb().Load(LiveAirPortResources(airportname, AirportResourceType.weather));
        public static bool DeterminAirPortExist(string airportname)
        {
            var c = LiveAirportDoc(airportname).GetElementbyId($"mainBody").SelectNodes(".//div//div//div//div//h3");
            return !(c?[0]?.HasClass("blue") ?? true);
        }

        public static bool DeterminAirportSecondaryLoc(HtmlDocument liveairportdoc)
        {
            var a = liveairportdoc.GetElementbyId("polling-airport_board")
                       .SelectNodes(".//div//div//div")[0]
                       .GetClasses();
            foreach (var s in a)
            {
                if (s == "bignotice")
                {
                    return true;
                }
            }
            return false;
        }
        public static BoardModel[] GetBoard(HtmlDocument liveairportdoc, BoardType type)
        {
            List<BoardModel> board = new();
            foreach (HtmlNode table in liveairportdoc.GetElementbyId($"{type}-board").SelectNodes(".//table"))
            {
                foreach (HtmlNode row in table.SelectNodes("tr"))//每个行
                {
                    try
                    {
                        var n = row.SelectNodes("th|td");
                        DateTimeFormatInfo dtfmt = new();
                        dtfmt.ShortTimePattern = "hh:mm";
                        var _a = n[3].InnerText.Replace("&nbsp;", " ").Split(" ");
                        var _b = n[5].InnerText.Replace("&nbsp;", " ").Split(" ");
                        var ddt = Convert.ToDateTime(_a[0], dtfmt);
                        var adt = Convert.ToDateTime(_b[0], dtfmt);
                        board.Add(new BoardModel()
                        {
                            Identity = n[0].InnerText,
                            AircraftType = n[1].InnerText,
                            Place = n[2].InnerText,
                            Dept = (ddt == DateTime.MinValue ? null : ddt, _a[1]),
                            Arrv = (adt == DateTime.MinValue ? null : adt, _b[1]),
                        });
                    }
                    catch
                    {
                        //node err
                    }
                }
            }
            return board.ToArray();
        }
        public static (BoardModel[] arrivals, BoardModel[] departures, BoardModel[] enroute, BoardModel[] scheduled) GetAllBoard(HtmlDocument liveairportdoc)
        {
             
            List<BoardModel> arrivals = new();
            List<BoardModel> departures = new();
            List<BoardModel> enroute = new();
            List<BoardModel> scheduled = new();
            foreach (HtmlNode table in liveairportdoc.GetElementbyId($"arrivals-board").SelectNodes(".//table"))
            {
                foreach (HtmlNode row in table.SelectNodes("tr"))//每个行
                {
                    try
                    {
                        var n = row.SelectNodes("th|td");
                        DateTimeFormatInfo dtfmt = new();
                        dtfmt.ShortTimePattern = "hh:mm";
                        var _a = n[3].InnerText.Replace("&nbsp;", " ").Split(" ");
                        var _b = n[5].InnerText.Replace("&nbsp;", " ").Split(" ");
                        var ddt = Convert.ToDateTime(_a[0], dtfmt);
                        var adt = Convert.ToDateTime(_b[0], dtfmt);
                        arrivals.Add(new BoardModel()
                        {
                            Identity = n[0].InnerText,
                            AircraftType = n[1].InnerText,
                            Place = n[2].InnerText,
                            Dept = (ddt == DateTime.MinValue ? null : ddt, _a[1]),
                            Arrv = (adt == DateTime.MinValue ? null : adt, _b[1]),
                        });
                    }
                    catch
                    {
                        //node err
                    }
                }
            }
            foreach (HtmlNode table in liveairportdoc.GetElementbyId($"departures-board").SelectNodes(".//table"))
            {
                foreach (HtmlNode row in table.SelectNodes("tr"))//每个行
                {
                    try
                    {
                        var n = row.SelectNodes("th|td");
                        DateTimeFormatInfo dtfmt = new();
                        dtfmt.ShortTimePattern = "hh:mm";
                        var _a = n[3].InnerText.Replace("&nbsp;", " ").Split(" ");
                        var _b = n[5].InnerText.Replace("&nbsp;", " ").Split(" ");
                        var ddt = Convert.ToDateTime(_a[0], dtfmt);
                        var adt = Convert.ToDateTime(_b[0], dtfmt);
                        departures.Add(new BoardModel()
                        {
                            Identity = n[0].InnerText,
                            AircraftType = n[1].InnerText,
                            Place = n[2].InnerText,
                            Dept = (ddt == DateTime.MinValue ? null : ddt, _a[1]),
                            Arrv = (adt == DateTime.MinValue ? null : adt, _b[1]),
                        });
                    }
                    catch
                    {
                        //node err
                    }
                }
            }
            foreach (HtmlNode table in liveairportdoc.GetElementbyId($"enroute-board").SelectNodes(".//table"))
            {
                foreach (HtmlNode row in table.SelectNodes("tr"))//每个行
                {
                    try
                    {
                        var n = row.SelectNodes("th|td");
                        DateTimeFormatInfo dtfmt = new();
                        dtfmt.ShortTimePattern = "hh:mm";
                        var _a = n[3].InnerText.Replace("&nbsp;", " ").Split(" ");
                        var _b = n[5].InnerText.Replace("&nbsp;", " ").Split(" ");
                        var ddt = Convert.ToDateTime(_a[0], dtfmt);
                        var adt = Convert.ToDateTime(_b[0], dtfmt);
                        enroute.Add(new BoardModel()
                        {
                            Identity = n[0].InnerText,
                            AircraftType = n[1].InnerText,
                            Place = n[2].InnerText,
                            Dept = (ddt == DateTime.MinValue ? null : ddt, _a[1]),
                            Arrv = (adt == DateTime.MinValue ? null : adt, _b[1]),
                        });
                    }
                    catch
                    {
                        //node err
                    }
                }
            }
            foreach (HtmlNode table in liveairportdoc.GetElementbyId($"scheduled-board").SelectNodes(".//table"))
            {
                foreach (HtmlNode row in table.SelectNodes("tr"))//每个行
                {
                    try
                    {
                        var n = row.SelectNodes("th|td");
                        DateTimeFormatInfo dtfmt = new();
                        dtfmt.ShortTimePattern = "hh:mm";
                        var _a = n[3].InnerText.Replace("&nbsp;", " ").Split(" ");
                        var _b = n[5].InnerText.Replace("&nbsp;", " ").Split(" ");
                        var ddt = Convert.ToDateTime(_a[0], dtfmt);
                        var adt = Convert.ToDateTime(_b[0], dtfmt);
                        scheduled.Add(new BoardModel()
                        {
                            Identity = n[0].InnerText,
                            AircraftType = n[1].InnerText,
                            Place = n[2].InnerText,
                            Dept = (ddt == DateTime.MinValue ? null : ddt, _a[1]),
                            Arrv = (adt == DateTime.MinValue ? null : adt, _b[1]),
                        });
                    }
                    catch
                    {
                        //node err
                    }
                }
            }
            return (arrivals.ToArray(), departures.ToArray(), enroute.ToArray(), scheduled.ToArray());
        }

        public static string[]? GetLiveAirportWeather(HtmlDocument liveairportweatherdoc)
        {
            return liveairportweatherdoc?.GetElementbyId($"mainBody")?.SelectNodes(".//div//div//div//div//div//table//thead")?[0]?.Attributes["title"]?.Value?.Split("\n");
        }
        public static bool DeterminAirportLiveWeather(HtmlDocument liveairportweatherdoc)
        {
            if (GetLiveAirportWeather(liveairportweatherdoc) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static WeatherModel[] GetAirportWeather(HtmlDocument liveairportweatherdoc)
        {
            List<WeatherModel> w = new();
            HtmlNodeCollection nc;
            if (DeterminAirportLiveWeather(liveairportweatherdoc))
            {
                nc = liveairportweatherdoc.GetElementbyId($"mainBody").SelectNodes(".//div//div//div//div//div//table")[1].SelectNodes("//tbody");
            }
            else
            {
                nc = liveairportweatherdoc.GetElementbyId($"mainBody").SelectNodes(".//div//div//div//div//div//table//tbody");
            }
            foreach (HtmlNode table in nc)
            {
                foreach (HtmlNode row in table.SelectNodes("tr"))//每个行
                {
                    try
                    {
                        var n = row.SelectNodes("th|td");
                        w.Add(new WeatherModel()
                        {
                            Date = n[0].InnerText,
                            Time = n[1].InnerText,
                            FlightRules = n[2].InnerText,
                            WindDir = n[3].InnerText.Replace("&#176;", ""),
                            Speed = n[4].InnerText,
                            WeaType = n[5].InnerText,
                            HeightAGL = n[6].InnerText,
                            Visibility = n[7].InnerText,
                            TempDegC = n[8].InnerText.Replace("&#176;", ""),
                            TempDegF = n[9].InnerText.Replace("&#176;", ""),
                            DewPointC = n[10].InnerText.Replace("&#176;", ""),
                            DewPointF = n[11].InnerText.Replace("&#176;", ""),
                            RelHumid = n[12].InnerText,
                            Pressure = n[13].InnerText,
                            DensityAltitude = n[14].InnerText,
                            Remarks = n[15].InnerText,
                        });
                    }
                    catch
                    {
                        //node err
                    }
                }
            }
            return w.ToArray();
        }
        public static string[] GetAirportWeatherMsg(HtmlDocument liveairportweatherdoc)
        {
            List<string> aprt = new();
            HtmlNodeCollection nc;
            if (DeterminAirportLiveWeather(liveairportweatherdoc))
            {
                nc = liveairportweatherdoc.GetElementbyId($"mainBody").SelectNodes(".//div//div//div//div//div//table")[1].SelectNodes("//tbody");
            }
            else
            {
                nc = liveairportweatherdoc.GetElementbyId($"mainBody").SelectNodes(".//div//div//div//div//div//table//tbody");
            }
            foreach (HtmlNode table in nc)
            {
                foreach (HtmlNode row in table.SelectNodes("tr"))//每个行
                {
                    try
                    {
                        var n = row.SelectNodes("th|td");
                        aprt.Add(row.Attributes["title"].Value.ToString().Trim());
                    }
                    catch
                    {
                        //node err
                    }
                }
            }
            return aprt.ToArray();
        }
    }
}
