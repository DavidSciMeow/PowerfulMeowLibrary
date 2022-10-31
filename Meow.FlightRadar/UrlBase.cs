using HtmlAgilityPack;
using System.Globalization;

namespace Meow.FlightRadar
{
    public static class FRGet
    {
        public const string CN = "http://zh.flightaware.com/";
        public static Langpref LangPref { get; set; } = Langpref.zh_CN;
        
        /// <summary>
        /// 搜索某一机场
        /// </summary>
        /// <param name="predtword">搜索字</param>
        /// <returns></returns>
        public static string SearchAirport(string predtword) => CN + $"ajax/ignoreall/omnisearch/airport.rvt?locale={LangPref}&searchterm={predtword}&q={predtword}";
        /// <summary>
        /// 搜索某一航班
        /// </summary>
        /// <param name="predtword">搜索字</param>
        /// <returns></returns>
        public static string SearchFlight(string predtword) => CN + $"ajax/ignoreall/omnisearch/flight.rvt?locale={LangPref}&searchterm={predtword}&q={predtword}";
        /// <summary>
        /// 搜索注册机构(飞航公司)
        /// </summary>
        /// <param name="predtword">搜索字</param>
        /// <returns></returns>
        public static string SearchRegistration(string predtword) => CN + $"ajax/ignoreall/omnisearch/registration.rvt?locale={LangPref}&searchterm={predtword}&q={predtword}";
        /// <summary>
        /// 搜索机场和对应的ICAO号
        /// </summary>
        /// <param name="predtword">搜索字</param>
        /// <returns></returns>
        public static string SearchAiportName(string predtword) => CN + $"ajax/ignoreall/airport_names_yajl.rvt?locale={LangPref}&q={predtword}";


        /// <summary>
        /// 机场获取
        /// </summary>
        /// <param name="airportname">机场识别号</param>
        /// <returns></returns>
        public static string LiveAirport(string airportname) => CN + $"live/airport/{airportname}";
        /// <summary>
        /// 机场信息获取
        /// </summary>
        /// <param name="airportname">机场识别号</param>
        /// <param name="type">信息类型</param>
        /// <returns></returns>
        public static string LiveAirPortResources(string airportname, AirportResourceType? type = null) => CN + $"resources/airport/{airportname}/{type?.ToString() ?? ""}";
        /// <summary>
        /// 航班状态
        /// </summary>
        /// <param name="opeartor">运营公司</param>
        /// <param name="airnum">航班号</param>
        /// <returns></returns>
        public static string LiveFlight(string opeartor, string airnum) => CN + $"live/flight/{opeartor}{airnum}";
        /// <summary>
        /// 航班状态
        /// </summary>
        /// <param name="ident">识别号</param>
        /// <returns></returns>
        public static string LiveFlight(string ident) => CN + $"live/flight/{ident}";
        /// <summary>
        /// 效果等同于LiveFlight
        /// </summary>
        /// <param name="ident">飞机识别号</param>
        /// <returns></returns>
        [Obsolete("使用LiveFlight替代")]
        public static string LiveIdent(string ident) => CN + $"live/form.rvt?ident={ident}";
        /// <summary>
        /// 查看航线排班
        /// </summary>
        /// <param name="orig">始发</param>
        /// <param name="dest">终到</param>
        /// <returns></returns>
        public static string LiveRoute(string orig,string dest) => CN + $"live/findflight?origin={orig}&destination={dest}";


    }

    //ajax/ignoreall/airport_names_yajl.rvt?locale=zh_CN&q=PEK&code=1&callback=flightfindercallback //列表/机场城市 
}
