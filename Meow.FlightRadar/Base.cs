using Meow.Util.Network.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.FlightRadar.Base
{
    /// <summary>
    /// 机场类
    /// </summary>
    public class Airport
    {
        /// <summary>
        /// ICAO代码
        /// </summary>
        public string ICAO { get; }
        /// <summary>
        /// 机场是否存在
        /// </summary>
        public bool IsAirportExist { get; }
        /// <summary>
        /// 机场是否二级管理区域
        /// </summary>
        public bool IsAirportSecondaryLoc { get; }
        /// <summary>
        /// 机场运营管理基地信息
        /// </summary>
        public FBOModel? FBOs { get; }
        /// <summary>
        /// 机场历史以及预测天气
        /// </summary>
        public WeatherModel[]? Weathers { get; }
        /// <summary>
        /// 机场(ATIS)天气字符<br/>
        /// 不支持则为空
        /// </summary>
        public string[]? LiveWeatherMsg { get; }
        /// <summary>
        /// 机场播报天气字符
        /// </summary>
        public string[]? WeatherMsg { get; }
        /// <summary>
        /// 进港
        /// </summary>
        public BoardModel[]? ArrivalsBoards { get; }
        /// <summary>
        /// 离港
        /// </summary>
        public BoardModel[]? DeparturesBoards { get; }
        /// <summary>
        /// 预进
        /// </summary>
        public BoardModel[]? EnrouteBoards { get; }
        /// <summary>
        /// 预出
        /// </summary>
        public BoardModel[]? ScheduledBoards { get; }
        
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="icao">机场的ICAO代码</param>
        public Airport(string icao)
        {
            ICAO = icao.ToUpper();
            var lad = SBase.LiveAirportDoc(icao);
            if (SBase.DeterminAirPortExist(lad))
            {
                IsAirportExist = true;
                //天气处理
                var wd = SBase.LiveAirportWeatherDoc(icao);
                if (SBase.DeterminAirportLiveWeather(wd))
                {
                    LiveWeatherMsg = SBase.GetLiveAirportWeather(wd);
                }
                WeatherMsg = SBase.GetAirportWeatherMsg(wd);
                Weathers = SBase.GetAirportWeather(wd);
                //进出港处理
                IsAirportSecondaryLoc = SBase.DeterminAirportSecondaryLoc(lad);
                ArrivalsBoards = SBase.GetBoard(lad, BoardType.arrivals);
                DeparturesBoards = SBase.GetBoard(lad, BoardType.departures);
                EnrouteBoards = SBase.GetBoard(lad, BoardType.enroute);
                ScheduledBoards = SBase.GetBoard(lad, BoardType.scheduled);
                //FBOs
                var lafbodoc = SBase.LiveAirportFBODoc(icao);
                if (SBase.DeterminFBOExist(lafbodoc))
                {
                    FBOs = SBase.GetFBOs(lafbodoc);
                }
            }
            else
            {
                IsAirportExist = false;
            }
        }

        /// <summary>
        /// 重写的字符串输出
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine($"[{ICAO}] {(IsAirportSecondaryLoc ? "_SECONDARY_" : "_MAIN_")}");
            sb.AppendLine($"\n\n---FBOs---");
            if(FBOs != null)
            {
                sb.AppendLine($"{FBOs}");
            }
            else
            {
                sb.AppendLine($"__NO PRESENTED DATA__");
            }
            sb.AppendLine($"\n\n---Weathers---");
            if (Weathers != null)
            {
                foreach (var i in Weathers)
                {
                    sb.AppendLine($"{i}");
                }
            }
            else
            {
                sb.AppendLine($"__NO PRESENTED DATA__");
            }
            sb.AppendLine($"\n\n---Boards---");
            sb.AppendLine("[ IDENTITY / TYPE ]     DEPATURE TIME      ->     ARRIVE TIME        {(->/<-) PLACE }");
            if(ArrivalsBoards != null)
            {
                sb.AppendLine($"::Arrivals::");
                foreach (var i in ArrivalsBoards)
                {
                    sb.AppendLine($"{i}");
                }
            }
            if (DeparturesBoards != null)
            {
                sb.AppendLine($"::Departures::");
                foreach (var i in DeparturesBoards)
                {
                    sb.AppendLine($"{i}");
                }
            }
            if (EnrouteBoards != null)
            {
                sb.AppendLine($"::Enroute::");
                foreach (var i in EnrouteBoards)
                {
                    sb.AppendLine($"{i}");
                }
            }
            if (ScheduledBoards != null)
            {
                sb.AppendLine($"::Scheduled::");
                foreach (var i in ScheduledBoards)
                {
                    sb.AppendLine($"{i}");
                }
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// 飞机类
    /// </summary>
    public class Flight
    {
        /// <summary>
        /// 当前飞行跟踪记录数据
        /// </summary>
        public JObject? Doc = null;
        /// <summary>
        /// 历史数据
        /// </summary>
        public List<FlightInfo> ActivityLog = new();
        /// <summary>
        /// 当前跟踪记录
        /// </summary>
        public FlightInfo? NowActivity = null;
        /// <summary>
        /// 飞行对查表信息
        /// </summary>
        public FlightTokenInfo? TokenInfo = null;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="ICAOIdent"></param>
        public Flight(string ICAOIdent)
        {
            TokenInfo = SBase.GetFlightTrack(SBase.LiveFlightDoc(ICAOIdent));
            if(TokenInfo != null)
            {
                Doc = JObject.Parse(SBase.BaseClient.MString(UrlMapping.LivePullOut(TokenInfo?.Token ?? "", true)).GetAwaiter().GetResult());
                var ft = Doc?["flights"]?.First?.First;
                var aflja = JArray.Parse(ft?["activityLog"]?["flights"]?.ToString() ?? "[]");
                foreach (var i in aflja)
                {
                    ActivityLog.Add(new(i));
                }
                NowActivity = new(ft);
            }
        }
    }
}
