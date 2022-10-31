using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.FlightRadar
{
    /// <summary>
    /// 静态机场类
    /// </summary>
    public static class SAirport
    {
        /// <summary>
        /// 默认的机场文件
        /// </summary>
        /// <param name="ICAOIdent">机场ICAO注册号</param>
        /// <returns></returns>
        public static HtmlDocument LiveAirportDoc(string ICAOIdent) => new HtmlWeb().Load(FRGet.LiveAirport(ICAOIdent));
        /// <summary>
        /// 默认的机场天气预报终端
        /// </summary>
        /// <param name="ICAOIdent">机场ICAO注册号</param>
        /// <returns></returns>
        public static HtmlDocument LiveAirportWeatherDoc(string ICAOIdent) => new HtmlWeb().Load(FRGet.LiveAirPortResources(ICAOIdent, AirportResourceType.weather));

        /// <summary>
        /// 判定机场是否存在
        /// </summary>
        /// <param name="ICAOIdent">机场ICAO注册号</param>
        /// <returns></returns>
        public static bool DeterminAirPortExist(string ICAOIdent)
        {
            var c = LiveAirportDoc(ICAOIdent).GetElementbyId($"mainBody").SelectNodes(".//div//div//div//div//h3");
            return !(c?[0]?.HasClass("blue") ?? true);
        }
        /// <summary>
        /// 确定机场是否第二飞行管理区(非主要管理区提供信息不多)
        /// </summary>
        /// <param name="liveairportdoc">获取的LiveAirportDoc</param>
        /// <returns></returns>
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
        /// <summary>
        /// 判定机场是否支持天气
        /// </summary>
        /// <param name="liveairportweatherdoc">获取的LiveAirportWeatherDoc</param>
        /// <returns></returns>
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

        /// <summary>
        /// 获取机场的航班进出港情况
        /// </summary>
        /// <param name="liveairportdoc">获取的LiveAirportDoc</param>
        /// <param name="type">进出港类型</param>
        /// <returns></returns>
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
        /// <summary>
        /// 获取机场的航班进出港情况
        /// </summary>
        /// <param name="liveairportdoc">获取的LiveAirportDoc</param>
        /// <returns></returns>
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

        /// <summary>
        /// 获取机场现场(ATIS)天气(气象电报字符串)
        /// </summary>
        /// <param name="liveairportweatherdoc">获取的LiveAirportWeatherDoc</param>
        /// <returns></returns>
        public static string[]? GetLiveAirportWeather(HtmlDocument liveairportweatherdoc)
        {
            return liveairportweatherdoc?.GetElementbyId($"mainBody")?.SelectNodes(".//div//div//div//div//div//table//thead")?[0]?.Attributes["title"]?.Value?.Split("\n");
        }
        /// <summary>
        /// 获取机场天气(模型类)
        /// </summary>
        /// <param name="liveairportweatherdoc">获取的LiveAirportWeatherDoc</param>
        /// <returns></returns>
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
        /// <summary>
        /// 获取机场天气(数据同步)(气象电报字符串)
        /// </summary>
        /// <param name="liveairportweatherdoc">获取的LiveAirportWeatherDoc</param>
        /// <returns></returns>
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
