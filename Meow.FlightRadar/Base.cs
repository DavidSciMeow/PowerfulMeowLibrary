using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.FlightRadar.Base
{
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
            var lad = SAirport.LiveAirportDoc(icao);
            if (SAirport.DeterminAirPortExist(lad))
            {
                IsAirportExist = true;
                //天气处理
                var wd = SAirport.LiveAirportWeatherDoc(icao);
                if (SAirport.DeterminAirportLiveWeather(wd))
                {
                    LiveWeatherMsg = SAirport.GetLiveAirportWeather(wd);
                }
                WeatherMsg = SAirport.GetAirportWeatherMsg(wd);
                Weathers = SAirport.GetAirportWeather(wd);
                //进出港处理
                IsAirportSecondaryLoc = SAirport.DeterminAirportSecondaryLoc(lad);
                ArrivalsBoards = SAirport.GetBoard(lad, BoardType.arrivals);
                DeparturesBoards = SAirport.GetBoard(lad, BoardType.departures);
                EnrouteBoards = SAirport.GetBoard(lad, BoardType.enroute);
                ScheduledBoards = SAirport.GetBoard(lad, BoardType.scheduled);
                //FBOs
                var lafbodoc = SAirport.LiveAirportFBODoc(icao);
                if (SAirport.DeterminFBOExist(lafbodoc))
                {
                    FBOs = SAirport.GetFBOs(lafbodoc);
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
}
