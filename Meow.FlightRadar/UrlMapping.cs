﻿using HtmlAgilityPack;
using System.Globalization;

namespace Meow.FlightRadar
{
    /// <summary>
    /// 基获取
    /// </summary>
    public static class UrlMapping
    {
        /// <summary>
        /// FlightAware站
        /// </summary>
        public const string FAC = "https://zh.flightaware.com/";
        /// <summary>
        /// 语言设置
        /// </summary>
        public static Langpref LangPref { get; set; } = Langpref.en_US;
        /// <summary>
        /// 全部航司
        /// </summary>
        public const string LiveFleet = $"{FAC}live/fleet/";

        /// <summary>
        /// 搜索某一机场
        /// </summary>
        /// <param name="predtword">搜索字</param>
        /// <returns></returns>
        public static string SearchAirport(string predtword) => FAC + $"ajax/ignoreall/omnisearch/airport.rvt?locale={LangPref}&searchterm={predtword}&q={predtword}";
        /// <summary>
        /// 搜索某一航班
        /// </summary>
        /// <param name="predtword">搜索字</param>
        /// <returns></returns>
        public static string SearchFlight(string predtword) => FAC + $"ajax/ignoreall/omnisearch/flight.rvt?locale={LangPref}&searchterm={predtword}&q={predtword}";
        /// <summary>
        /// 搜索注册机构(飞航公司)
        /// </summary>
        /// <param name="predtword">搜索字</param>
        /// <returns></returns>
        public static string SearchRegistration(string predtword) => FAC + $"ajax/ignoreall/omnisearch/registration.rvt?locale={LangPref}&searchterm={predtword}&q={predtword}";
        /// <summary>
        /// 搜索机场和对应的ICAO号
        /// </summary>
        /// <param name="predtword">搜索字</param>
        /// <returns></returns>
        public static string SearchAiportName(string predtword) => FAC + $"ajax/ignoreall/airport_names_yajl.rvt?locale={LangPref}&q={predtword}";
        /// <summary>
        /// 机场获取
        /// </summary>
        /// <param name="airportname">机场识别号</param>
        /// <returns></returns>
        public static string LiveAirport(string airportname) => FAC + $"live/airport/{airportname}";
        /// <summary>
        /// 机场信息获取
        /// </summary>
        /// <param name="airportname">机场识别号</param>
        /// <param name="type">信息类型</param>
        /// <returns></returns>
        public static string LiveAirPortResources(string airportname, AirportResourceType? type = null) => FAC + $"resources/airport/{airportname}/{type?.ToString() ?? string.Empty}";
        /// <summary>
        /// 机场FBO信息获取
        /// </summary>
        /// <param name="airportname">机场识别号</param>
        /// <returns></returns>
        public static string LiveAirPortResources(string airportname) => FAC + $"resources/airport/{airportname}";
        /// <summary>
        /// 航班状态
        /// </summary>
        /// <param name="opeartor">运营公司</param>
        /// <param name="airnum">航班号</param>
        /// <returns></returns>
        public static string LiveFlight(string opeartor, string airnum) => FAC + $"live/flight/{opeartor}{airnum}";
        /// <summary>
        /// 航班状态
        /// </summary>
        /// <param name="ident">识别号</param>
        /// <returns></returns>
        public static string LiveFlight(string ident) => FAC + $"live/flight/{ident}";
        /// <summary>
        /// 查看航线排班
        /// </summary>
        /// <param name="orig">始发</param>
        /// <param name="dest">终到</param>
        /// <returns></returns>
        public static string LiveRoute(string orig,string dest) => FAC + $"live/findflight?origin={orig}&destination={dest}";
        /// <summary>
        /// 航司历史计划
        /// </summary>
        /// <param name="identname">航司识别号</param>
        /// <returns></returns>
        public static string LiveFleetNow(string identname) => LiveFleet + identname;
        /// <summary>
        /// 获取实时追踪飞机逻辑
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="summary">总结报告</param>
        /// <returns></returns>
        public static string LivePullOut(string token, bool summary = true) => $"https://zh.flightaware.com/ajax/trackpoll.rvt?token={token}&locale=en_US&summary={(summary ? "1" : "0")}";
    }
}
