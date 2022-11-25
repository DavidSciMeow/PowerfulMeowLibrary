using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System.Globalization;
using Meow.Util.Network.Http;
using Meow.FlightRadar.Base;

namespace Meow.FlightRadar
{
    /// <summary>
    /// 静态工具类
    /// </summary>
    public static class SBase
    {
        /*-*/
        /// <summary>
        /// 用于LOAD文件的实例
        /// </summary>
        private readonly static HtmlWeb HW = new();
        /*-*/
        public static FleetInfo[]? AllFleet { get; set; }
        /*--Base--*/
        /// <summary>
        /// 默认的机场文件
        /// </summary>
        /// <param name="ICAOIdent">机场ICAO注册号</param>
        /// <returns></returns>
        public static HtmlDocument LiveAirportDoc(string ICAOIdent) => HW.Load(UrlMapping.LiveAirport(ICAOIdent));
        /// <summary>
        /// 默认的机场天气预报终端
        /// </summary>
        /// <param name="ICAOIdent">机场ICAO注册号</param>
        /// <returns></returns>
        public static HtmlDocument LiveAirportWeatherDoc(string ICAOIdent) => HW.Load(UrlMapping.LiveAirPortResources(ICAOIdent, AirportResourceType.weather));
        /// <summary>
        /// 运营人信息
        /// </summary>
        /// <param name="ICAOIdent">>机场ICAO注册号</param>
        /// <returns></returns>
        public static HtmlDocument LiveAirportFBODoc(string ICAOIdent) => HW.Load(UrlMapping.LiveAirPortResources(ICAOIdent));
        /// <summary>
        /// 获取实时航班
        /// </summary>
        /// <param name="FlightNum"></param>
        /// <returns></returns>
        public static HtmlDocument LiveFlightDoc(string FlightNum) => HW.Load(UrlMapping.LiveFlight(FlightNum));
        /// <summary>
        /// 获取所有航司
        /// </summary>
        /// <returns></returns>
        public static HtmlDocument LiveAllFleetDoc() => HW.Load(UrlMapping.LiveFleet);
        /// <summary>
        /// 获取某航司的在场班次
        /// </summary>
        /// <param name="Fleet">航司名</param>
        /// <returns></returns>
        public static HtmlDocument LiveFleetDoc(string Fleet) => HW.Load(UrlMapping.LiveFleetNow(Fleet));
        /*--Kits--*/
        /// <summary>
        /// 判定机场是否存在
        /// </summary>
        /// <param name="ICAOIdent">机场ICAO注册号</param>
        /// <returns></returns>
        public static bool DeterminAirPortExist(HtmlDocument liveairportdoc)
        {
            var c = liveairportdoc.GetElementbyId($"mainBody").SelectNodes(".//div//div//div//div//h3");
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
        /// 判定机场含有可提取的运营人信息
        /// </summary>
        /// <param name="liveairportfbodoc">获取的LiveAirportFBODoc</param>
        /// <returns></returns>
        public static bool DeterminFBOExist(HtmlDocument liveairportfbodoc)
        {
            return liveairportfbodoc.GetElementbyId("mainBody")
                .SelectSingleNode("//div/div[1]/div/div[3]/div/h3") == null;
        }
        /*--Fbo/Board--*/
        /// <summary>
        /// 获取机场的FBO信息
        /// </summary>
        /// <param name="liveairportfbodoc"></param>
        /// <returns></returns>
        public static FBOModel GetFBOs(HtmlDocument liveairportfbodoc)
        {
            var fbomap = new FBOModel();

            var n1 = liveairportfbodoc.DocumentNode
                .SelectSingleNode("/html[1]/div[1]/div[1]/div[1]/div[1]/div[3]/div[1]/div[3]/div[1]/table[1]")
                .SelectNodes("tr")[0]
                .SelectNodes("td");

            fbomap.Location = n1[0].InnerText.Replace("\t", "").Replace("\n", " ").Trim();
            var cae = n1[1].InnerText.Replace("\t", "").Replace("\n", " ").Trim().Replace("  ", "|").Split("|");
            var cood = cae?[0]?.Split(',');
            fbomap.CoordinateX = double.Parse(cood?[0]?.Trim() ?? "0.0");
            fbomap.CoordinateY = double.Parse(cood?[1]?.Trim() ?? "0.0");
            var elev = cae?[1]?.Replace("feet","").Replace("meter", "").Replace("s", "").Split('/');
            fbomap.ElevationFeet = double.Parse(elev?[0]?.Trim() ?? "0.0");
            fbomap.ElevationFeet = double.Parse(elev?[1]?.Trim() ?? "0.0");
            //var loct = n1[2].InnerHtml.Split("&nbsp;")[0].Trim().Replace("<br>"," ");
            //var date = DateTime.ParseExact(loct, "dddd HH:mm", CultureInfo.DefaultThreadCurrentCulture);
            //Console.WriteLine(loct);
            fbomap.TowerExist = n1[3].InnerText.Replace("\t", "").Replace("\n", " ").Trim().ToLower() == "yes";
            var rwy = n1[4].InnerText.Replace("\t", "").Replace(" ", "").Trim().Split("\n");
            List<(string, string, string, string)> ls = new();
            for (int i = 0; i < rwy.Length; i+=4)
            {
                ls.Add(new(rwy[i], rwy[i + 1], rwy[i + 2], rwy[i + 3]));
            }
            fbomap.RunwayCondition = ls.ToArray();
            fbomap.FuelLL = n1[5].InnerText.Replace("\t", "").Replace("\n", " ").Trim();

            var basedaircraft = liveairportfbodoc.DocumentNode
                .SelectSingleNode("/html[1]/div[1]/div[1]/div[1]/div[1]/table")
                .SelectNodes("tr")[0]
                .SelectNodes("td");

            fbomap.BasedAirCraft.SingleEngine = int.Parse(basedaircraft?[0]?.InnerText?.Trim() ?? "0");
            fbomap.BasedAirCraft.MultiEngine = int.Parse(basedaircraft?[1]?.InnerText?.Trim() ?? "0");
            fbomap.BasedAirCraft.Jet = int.Parse(basedaircraft?[2]?.InnerText?.Trim() ?? "0");
            fbomap.BasedAirCraft.Heli = int.Parse(basedaircraft?[3]?.InnerText?.Trim() ?? "0");
            fbomap.BasedAirCraft.Glider = int.Parse(basedaircraft?[4]?.InnerText?.Trim() ?? "0");
            fbomap.BasedAirCraft.Military = int.Parse(basedaircraft?[5]?.InnerText?.Trim() ?? "0");

            var RadioTable = liveairportfbodoc.DocumentNode
                .SelectSingleNode("/html[1]/div[1]/div[1]/div[2]/div[1]/table")
                .SelectNodes("tr");

            List<(string, string)> freqlist = new();
            foreach (HtmlNode row in RadioTable)//每个行
            {
                var n2 = row.SelectNodes("td");
                freqlist.Add(new(n2[0].InnerText, n2[1].InnerText));
            }

            return fbomap;
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
                            Type = type,
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
        /*--Weather--*/
        /// <summary>
        /// 获取机场天气(ATIS/模型类)
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

                        double PressureHpa = -1;
                        double PressureInHg = -1;
                        {
                            var pres = n[13].InnerText.Trim();
                            if (pres.EndsWith("mb"))
                            {
                                PressureHpa = Math.Round(double.Parse(pres.Replace("mb", string.Empty).Trim()), 2);
                                PressureInHg = Math.Round(double.Parse(pres.Replace("mb", string.Empty).Trim()) / 33.8639, 2);
                            }
                            else if (pres.EndsWith("in Hg"))
                            {
                                PressureHpa = Math.Round(double.Parse(pres.Replace("in Hg", string.Empty).Trim()) * 33.8639, 2);
                                PressureInHg = Math.Round(double.Parse(pres.Replace("in Hg", string.Empty).Trim()), 2);
                            }
                        } //1 inHg = 33.8639 Hpa
                        DateTime dt;
                        {
                            var datetime = $"{DateTime.Now.Year}-{n[0].InnerText} {n[1].InnerText}"
                            .Replace("月", string.Empty)
                            .Replace("十一", "11")
                            .Replace("十二", "12")
                            .Replace("一", "01")
                            .Replace("二", "02")
                            .Replace("三", "03")
                            .Replace("四", "04")
                            .Replace("五", "05")
                            .Replace("六", "06")
                            .Replace("七", "07")
                            .Replace("八", "08")
                            .Replace("九", "09")
                            .Replace("十", "10");
                            dt = DateTime.ParseExact(datetime, "yyyy-dd-MM HH:mm", CultureInfo.InvariantCulture);
                        } //DateTime Convert
                        int winddir;
                        {
                            var wind = n[3].InnerText.Replace("&#176;", string.Empty).Trim();
                            if (wind.ToLower().Equals("calm"))
                            {
                                winddir = 0;
                            }
                            else if (wind.ToLower().Equals("variable"))
                            {
                                winddir = 361;
                            }
                            else
                            {
                                var x = int.Parse(wind);
                                winddir = (x == 0 ? 360 : x);
                            }
                        } //winddir Expression
                        double speedmps = 0;
                        double speedkt = 0;
                        {

                            var spdn = n[4].InnerText;
                            if (spdn.Contains("kt"))
                            {
                                speedkt = Math.Round(double.Parse(spdn.Replace("kt", string.Empty).Trim()), 2);
                                speedmps = Math.Round(speedkt * 0.5144444, 2);
                            }
                            else if (spdn.Contains("mps"))
                            {
                                speedmps = Math.Round(double.Parse(spdn.Replace("mps", string.Empty).Trim()), 2);
                                speedkt = Math.Round(speedmps / 0.5144444, 2);
                            }
                        } //1 kt = 0.5144444 m/s

                        int tempc = int.Parse(n[8].InnerText.Replace("&#176;", string.Empty));
                        int tempf = int.Parse(n[9].InnerText.Replace("&#176;", string.Empty));
                        int dpc = int.Parse(n[10].InnerText.Replace("&#176;", string.Empty));
                        int dpf = int.Parse(n[11].InnerText.Replace("&#176;", string.Empty));
                        int relhum = int.Parse(n[12].InnerText.Replace("%", string.Empty));
                        int densa = int.Parse(n[14].InnerText.Replace(",", string.Empty).Replace("ft", string.Empty).Trim());

                        w.Add(new WeatherModel()
                        {
                            Time = dt,
                            FlightRules = Enum.Parse<FlightRule>(n?[2]?.InnerText ?? "None"),
                            WindDir = winddir,
                            SpeedKt = speedkt,
                            SpeedMps = speedmps,
                            WeaType = n?[5]?.InnerHtml?.Split("<br>")[..^1] ?? Array.Empty<string>(),
                            Visibility = n?[7]?.InnerText ?? string.Empty,
                            TempDegC = tempc,
                            TempDegF = tempf,
                            DewPointC = dpc,
                            DewPointF = dpf,
                            RelHumid = relhum,
                            PressureHpa = PressureHpa,
                            PressureInHg = PressureInHg,
                            DensityAltitude = densa,
                            Remarks = n?[15]?.InnerText ?? string.Empty,
                        });
                    }
                    catch
                    {
                        //node err
                    }
                }
            }
            w.Sort((x, y) => y.Time.CompareTo(x.Time));
            return w.ToArray();
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
        /*--FlightInfo--*/
        /// <summary>
        /// 获取追踪飞机的信息
        /// </summary>
        /// <param name="liveflightdoc">获取的liveflightdoc</param>
        /// <returns></returns>
        public static FlightTokenInfo? GetFlightTrack(HtmlDocument liveflightdoc)
        {
            var scripts = liveflightdoc.DocumentNode.SelectNodes("//script[not(@type)]");
            foreach(var s in scripts)
            {
                if (s.InnerText.Contains("trackpollGlobals"))
                {
                    var json = s.InnerText.Replace("var trackpollGlobals = ", "").Replace(";", "").Trim();
                    var jo = JObject.Parse(json);
                    return new FlightTokenInfo(jo);
                }
            }
            return null;
        }
        /*--Fleet--*/
        /// <summary>
        /// 获取所有航司
        /// </summary>
        /// <param name="livefleetdoc">获取的livefleetdoc</param>
        /// <returns></returns>
        public static FleetInfo[] GetAllFleet(HtmlDocument livefleetdoc)
        {
            List<FleetInfo> w = new();
            var nc = livefleetdoc.GetElementbyId($"mainBody").SelectNodes("//div[1]//table[2]//tbody");
            foreach (var i in nc[0].SelectNodes("tr"))
            {
                var n = i.SelectNodes("th|td");
                w.Add(new()
                {
                    NumberOfFlight = int.Parse(n[0].InnerText),
                    Predix = n[1].InnerText,
                    AirlineOperator = n[2].InnerText
                });
            }
            return w.ToArray();
        }
    }
}
