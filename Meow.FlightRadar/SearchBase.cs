using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using Meow.Util.Network.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.FlightRadar
{
    /// <summary>
    /// 搜索航班模式
    /// </summary>
    public struct SearchFlightResult
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string description;
        /// <summary>
        /// 识别号
        /// </summary>
        public string ident;
        /// <summary>
        /// 注册号
        /// </summary>
        public string reg;
        /// <summary>
        /// 航司航线
        /// </summary>
        public string major_airline;
        /// <summary>
        /// 是否尾号
        /// </summary>
        public bool might_be_a_tail;
        /// <summary>
        /// 重写的字符串表示
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"[{ident,-10}] [{((might_be_a_tail) ? "_YES_" : "_NOT_")}] [{major_airline,3}] {description}";

    }
    /// <summary>
    /// 搜索机场模式
    /// </summary>
    public struct SearchAirportResult
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string description;
        /// <summary>
        /// ICAO代码
        /// </summary>
        public string icao;
        /// <summary>
        /// IATA代码
        /// </summary>
        public string iata;
        /// <summary>
        /// /
        /// </summary>
        public string ops;
        /// <summary>
        /// 重写的字符串表示
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{{{ops,7}}} [{icao}/{(string.IsNullOrEmpty(iata) ? "    " : iata.PadLeft(4))}] {description}";

        /// <summary>
        /// 获取一个机场
        /// </summary>
        /// <returns></returns>
        public Base.Airport AirportDetail() => new(icao);
    }
    public static class SearchBase
    {
        public static SearchAirportResult[] SearchAiport(string name)
        {
            List<SearchAirportResult> l = new();
            var j = Get.String(UrlMapping.SearchAirport(name)).GetAwaiter().GetResult();
            var jo = JArray.Parse(JObject.Parse(j.ToString())?["data"]?.ToString() ?? "[]");
            foreach (var k in jo)
            {
                l.Add(new()
                {
                    description = k?["description"]?.ToString() ?? string.Empty,
                    icao = k?["icao"]?.ToString() ?? string.Empty,
                    iata = k?["iata"]?.ToString() ?? string.Empty,
                    ops = k?["ops"]?.ToString() ?? string.Empty,
                });
            }
            return l.ToArray();
        }
        public static SearchFlightResult[] SearchAirline(string name)
        {
            List<SearchFlightResult> l = new();
            var j = Get.String(UrlMapping.SearchFlight(name)).GetAwaiter().GetResult();
            var jo = JArray.Parse(JObject.Parse(j)?["data"]?.ToString() ?? "[]");
            foreach (var i in jo)
            {
                l.Add(new()
                {
                    description = i?["description"]?.ToString() ?? string.Empty,
                    ident = i?["ident"]?.ToString() ?? string.Empty,
                    reg = i?["reg"]?.ToString() ?? string.Empty,
                    major_airline = i?["major_airline"]?.ToString() ?? string.Empty,
                    might_be_a_tail = i?["might_be_a_tail"]?.ToObject<bool>() ?? false,
                });
            }
            return l.ToArray();
        }

    }
}
