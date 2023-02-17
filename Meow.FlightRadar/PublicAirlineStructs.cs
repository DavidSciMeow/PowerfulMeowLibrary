using Meow.Util.Network.Http;
using Newtonsoft.Json.Linq;
using System.Text;
using Meow.Util;

namespace Meow.FlightRadar
{
    /// <summary>
    /// 经纬度结构体
    /// </summary>
    public struct FLatLon
    {
        /// <summary>
        /// 纬度
        /// </summary>
        public double? Latitude;
        /// <summary>
        /// 经度
        /// </summary>
        public double? Longitude;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="latitude">纬度</param>
        /// <param name="longitude">经度</param>
        public FLatLon(double? latitude, double? longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        /// <summary>
        /// 重写的字符串输出方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[{(Latitude > 0 ? $"{Latitude} °N" : $"{-Latitude} °S")}, {(Longitude > 0 ? $"{Longitude} °E" : $"{-Longitude} °W")}]";
        }
    }
    /// <summary>
    /// 机场所在时区
    /// </summary>
    public struct FLocation
    {
        /// <summary>
        /// 所在时区
        /// </summary>
        public string? TimeZone;
        /// <summary>
        /// 是否可查询机场
        /// </summary>
        public bool? IsValidAirportCode;
        /// <summary>
        /// 机场识别号
        /// </summary>
        public string? AltIdent;
        /// <summary>
        /// IATA识别号
        /// </summary>
        public string? IATA;
        /// <summary>
        /// 名称
        /// </summary>
        public string? FriendlyName;
        /// <summary>
        /// 所在地
        /// </summary>
        public string? FriendlyLocation;
        /// <summary>
        /// 坐标经纬度
        /// </summary>
        public FLatLon Coord;
        /// <summary>
        /// 机场ICAO代码
        /// </summary>
        public string? ICAO;
        /// <summary>
        /// 登机口
        /// </summary>
        public string? Gate;
        /// <summary>
        /// 航站楼
        /// </summary>
        public string? Terminal;
        /// <summary>
        /// 延迟字段
        /// </summary>
        public string? Delays;
        /// <summary>
        /// 获取这个机场信息
        /// </summary>
        /// <returns></returns>
        public Base.Airport? GetAirport()
        {
            if((ICAO != null) && (IsValidAirportCode ?? false))
            {
                return new Base.Airport(ICAO);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="j">传入的JToken</param>
        public FLocation(JToken? j)
        {
            TimeZone = j?["TZ"]?.ToString();
            IsValidAirportCode = j?["isValidAirportCode"]?.ToObject<bool?>();
            AltIdent = j?["altIdent"]?.ToString();
            IATA = j?["iata"]?.ToString();
            FriendlyName = j?["friendlyName"]?.ToString();
            FriendlyLocation = j?["friendlyLocation"]?.ToString();
            if (j?["isLatLon"]?.ToObject<bool?>() ?? false)
            {
                Coord = new(
                j?["coord"]?[0]?.ToObject<double?>(),
                j?["coord"]?[1]?.ToObject<double?>()
                );
            }
            else
            {
                Coord = new(
                j?["coord"]?[1]?.ToObject<double?>(),
                j?["coord"]?[0]?.ToObject<double?>()
                );
            }
            ICAO = j?["icao"]?.ToString();
            Gate = j?["gate"]?.ToString();
            Terminal = j?["terminal"]?.ToString();
            Delays = j?["delays"]?.ToString();
        }
        /// <summary>
        /// 重写的字符串输出方法
        /// </summary>
        /// <returns></returns>
        public override string ToString() => 
            $"{((IsValidAirportCode ?? false) ? $"=[{ICAO}] {IATA}=" : $"-[{ICAO}] {IATA}-")} " +
            $"{((!string.IsNullOrWhiteSpace(AltIdent)) ? $"[{AltIdent}]" : "")} @ {Coord}\n" +
            $"TZ/LOC: [{TimeZone}] / {FriendlyLocation} {FriendlyName} {Terminal} {Gate}";
    }
    /// <summary>
    /// 飞行时间结构体
    /// </summary>
    public struct FTimes
    {
        /// <summary>
        /// 计划时间
        /// </summary>
        public long? Scheduled;
        /// <summary>
        /// 预计时间
        /// </summary>
        public long? Estimated;
        /// <summary>
        /// 实际时间
        /// </summary>
        public long? Actual;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="j">传入的JToken</param>
        public FTimes(JToken? j)
        {
            Scheduled = j?["scheduled"]?.ToObject<long?>();
            Estimated = j?["estimated"]?.ToObject<long?>();
            Actual = j?["actual"]?.ToObject<long?>();
        }
        /// <summary>
        /// 重写的字符串输出方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Actual != null)
            {
                return $"A:{{{Actual?.Second()}}}";
            }
            else if (Scheduled == null && Estimated == null)
            {
                return $"N:[-]";
            }
            else if (Scheduled == null)
            {
                return $"E:[{Estimated?.Second()}]";
            }
            else if ((Scheduled != null && Scheduled == Estimated) || Estimated == null)
            {
                return $"S:[{Scheduled?.Second()}]";
            }
            else
            {
                return $"S/E:[{Scheduled?.Second()}, [{(Estimated?.Second() - Scheduled?.Second())?.TotalSeconds}s]]";
            }
        }
    }
    /// <summary>
    /// 路程
    /// </summary>
    public struct FDistance
    {
        /// <summary>
        /// 已经过路程
        /// </summary>
        public long? Elapsed = null;
        /// <summary>
        /// 剩余路程
        /// </summary>
        public long? Remaining = null;
        /// <summary>
        /// 实际路程
        /// </summary>
        public long? Actual = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="j">传入的JToken</param>
        public FDistance(JToken? j)
        {
            if (j?.HasValues ?? false)
            {
                Elapsed = j?["elapsed"]?.ToObject<long?>();
                Remaining = j?["remaining"]?.ToObject<long?>();
                Actual = j?["actual"]?.ToObject<long?>();
            }
        }
        /// <summary>
        /// 重写的字符串输出方法
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"T:-{Elapsed}+{Remaining} A:{Actual}";
    }
    /// <summary>
    /// 飞行计划结构体
    /// </summary>
    public struct FPlan
    {
        /// <summary>
        /// 上报速度 kt
        /// </summary>
        public int? Speed = null;
        /// <summary>
        /// 上报高度 FL
        /// </summary>
        public int? Altitude = null;
        /// <summary>
        /// 上报路线 VectorRa
        /// </summary>
        public string? Route = null;
        /// <summary>
        /// 直线距离
        /// </summary>
        public int? DirectDistance = null;
        /// <summary>
        /// 计划距离
        /// </summary>
        public int? PlannedDistance = null;
        /// <summary>
        /// 离场时间
        /// </summary>
        public long? Departure = null;
        /// <summary>
        /// 预计途中时间
        /// </summary>
        public int? ETE = null;
        /// <summary>
        /// 预计燃油消耗
        /// </summary>
        public (long? Gallons, long? Pounds)? FuelBurn = new(null, null);
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="j">传入的JToken</param>
        public FPlan(JToken? j)
        {
            if(j?.HasValues ?? false)
            {
                Speed = j?["speed"]?.ToObject<int?>();
                Altitude = j?["altitude"]?.ToObject<int?>();
                Route = j?["route"]?.ToString();
                DirectDistance = j?["directDistance"]?.ToObject<int?>();
                PlannedDistance = j?["plannedDistance"]?.ToObject<int?>();
                Departure = j?["departure"]?.ToObject<long?>();
                ETE = j?["ete"]?.ToObject<int?>();
                if (j?["fuelBurn"]?.HasValues ?? false)
                {
                    FuelBurn = new(
                        j?["fuelBurn"]?["gallons"]?.ToObject<int?>(),
                        j?["fuelBurn"]?["pounds"]?.ToObject<int?>()
                        );
                }
            }
        }
        /// <summary>
        /// 重写的字符串输出方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            if(Speed != null)
            {
                sb.AppendLine($"SPD :{Speed} kt");
            }
            else
            {
                sb.AppendLine($"SPD : _NO INFO_");
            }
            if(Altitude != null)
            {
                sb.AppendLine($"ALT : FL{Altitude}");
            }
            else
            {
                sb.AppendLine($"ALT : _NO INFO_");
            }
            if(Departure != null)
            {
                sb.AppendLine($"Dep Time: {Departure?.Second()}");
            }
            else
            {
                sb.AppendLine($"Dep Time: _NO INFO_");
            }
            if (Route != null)
            {
                sb.AppendLine($"ETE :: {ETE}s");
                sb.AppendLine($"Route ::[ {Route} ]");
            }
            else
            {
                sb.AppendLine("_NO Route INFO_");
            }
            if (PlannedDistance != null && DirectDistance != null)
            {
                sb.AppendLine($"DD:{DirectDistance} | PD:{DirectDistance}");
            }
            else
            {
                if (DirectDistance != null)
                {
                    sb.AppendLine($"DD:{DirectDistance} | PD: _NONE_ ");
                }
                if (PlannedDistance != null)
                {
                    sb.AppendLine($"DD: _NONE_ | PD:{DirectDistance}");
                }
            }
            if(FuelBurn?.Gallons != null && FuelBurn?.Pounds != null)
            {
                sb.AppendLine($"FUEL:{FuelBurn?.Gallons} gal/{FuelBurn?.Pounds} lb.");
            }
            else
            {
                if (FuelBurn?.Gallons != null)
                {
                    sb.AppendLine($"FUEL:{FuelBurn?.Gallons} gal");
                }
                if (FuelBurn?.Pounds != null)
                {
                    sb.AppendLine($"FUEL:{FuelBurn?.Pounds} lb.");
                }
            }
            return sb.ToString();
        }
    }
    /// <summary>
    /// 飞机类型结构体
    /// </summary>
    public struct FAircraftType
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string? Type;
        /// <summary>
        /// 是否医疗用(紧急)
        /// </summary>
        public bool? Lifeguard;
        /// <summary>
        /// 是否重型机
        /// </summary>
        public bool? Heavy;
        /// <summary>
        /// 尾号
        /// </summary>
        public string? Tail;
        /// <summary>
        /// 所有者
        /// </summary>
        public string? Owner;
        /// <summary>
        /// 所有者位置
        /// </summary>
        public string? OwnerLocation;
        /// <summary>
        /// 所有者网站
        /// </summary>
        public string? OwnerUrl;
        /// <summary>
        /// 所有者类型
        /// </summary>
        public string? OwnerType;
        /// <summary>
        /// 所有者是否可通信
        /// </summary>
        public bool? CanMessage;
        /// <summary>
        /// 一般名称类型
        /// </summary>
        public string? FriendlyType;
        /// <summary>
        /// 制造厂商
        /// </summary>
        public string? ManuFacturer;
        /// <summary>
        /// 型号
        /// </summary>
        public string? Model;
        /// <summary>
        /// 引擎型号(长)
        /// </summary>
        public string? EngTotalType;
        /// <summary>
        /// 引擎数
        /// </summary>
        public string? EngCount;
        /// <summary>
        /// 引擎类型
        /// </summary>
        public string? EngType;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="j">传入的JToken</param>
        public FAircraftType(JToken? j)
        {
            Type = j?["type"]?.ToString();
            Lifeguard = j?["lifeguard"]?.ToObject<bool?>();
            Heavy = j?["heavy"]?.ToObject<bool?>();
            Tail = j?["tail"]?.ToString();
            Owner = j?["owner"]?.ToString();
            OwnerLocation = j?["ownerLocation"]?.ToString();
            OwnerUrl = j?["ownerUrl"]?.ToString();
            OwnerType = j?["owner_type"]?.ToString();
            CanMessage = j?["canMessage"]?.ToObject<bool?>();
            FriendlyType = j?["friendlyType"]?.ToString();
            ManuFacturer = j?["typeDetails"]?["manufacturer"]?.ToString();
            Model = j?["typeDetails"]?["model"]?.ToString();
            EngTotalType = j?["typeDetails"]?["type"]?.ToString();
            EngCount = j?["typeDetails"]?["engCount"]?.ToString();
            EngType = j?["typeDetails"]?["engType"]?.ToString();
        }
        /// <summary>
        /// 重写的字符串输出方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine($"[{Type}] {FriendlyType}");
            if (!string.IsNullOrWhiteSpace(ManuFacturer))
            {
                sb.AppendLine($"ManuBy:[{ManuFacturer}]");
            }
            if (!string.IsNullOrWhiteSpace(Tail))
            {
                sb.AppendLine($"[{Tail} {(Heavy ?? false ? "Heavy" : "")} {(Lifeguard ?? false ? "Lifeguard" : "")}]");
            }
            if(!string.IsNullOrWhiteSpace(Owner))
            {
                sb.AppendLine($"Owner:[{OwnerLocation} {Owner} [{(CanMessage ?? false ? "CANMSG" : "")}] / {OwnerType}]");
            }
            return sb.ToString();
        }
    }
    /// <summary>
    /// 航司信息
    /// </summary>
    public struct FAirline
    {
        /// <summary>
        /// 全名
        /// </summary>
        public string? FullName;
        /// <summary>
        /// 短名
        /// </summary>
        public string? ShortName;
        /// <summary>
        /// ICAO识别号
        /// </summary>
        public string? ICAO;
        /// <summary>
        /// IATA识别号
        /// </summary>
        public string? IATA;
        /// <summary>
        /// 呼号
        /// </summary>
        public string? CallSign;
        /// <summary>
        /// 航司官网地址
        /// </summary>
        public string? Url;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="j">传入的JToken</param>
        public FAirline(JToken? j)
        {
            FullName = j?["fullName"]?.ToString();
            ShortName = j?["shortName"]?.ToString();
            ICAO = j?["icao"]?.ToString();
            IATA = j?["iata"]?.ToString();
            CallSign = j?["callsign"]?.ToString();
            Url = j?["url"]?.ToString();
        }
        /// <summary>
        /// 重写的字符串输出方法
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"*{ICAO}[{IATA}] {{{CallSign}}} :: {ShortName}[{FullName}] ";
    }
    /// <summary>
    /// 共用呼号结构体
    /// </summary>
    public struct FCodeShare
    {
        /// <summary>
        /// 呼号
        /// </summary>
        public string? Ident;
        /// <summary>
        /// 显示呼号
        /// </summary>
        public string? DisplayIdent;
        /// <summary>
        /// IATA呼号
        /// </summary>
        public string? IATAIdent;
        /// <summary>
        /// 航司
        /// </summary>
        public FAirline? Airline;
        /// <summary>
        /// 一般呼号
        /// </summary>
        public string? FriendlyIdent;
        /// <summary>
        /// 缩略图
        /// </summary>
        public (string? ImageUrl,string? LinkUrl)? Thumbnail;
        /// <summary>
        /// 其他链接
        /// </summary>
        public FLinks? Links;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="j">传入的JToken</param>
        public FCodeShare(JToken? j)
        {
            Ident = j?["ident"]?.ToString();
            DisplayIdent = j?["displayIdent"]?.ToString();
            IATAIdent = j?["iataIdent"]?.ToString();
            Airline = new(j?["airline"]);
            FriendlyIdent = j?["friendlyIdent"]?.ToString();
            Thumbnail = new(
                j?["thumbnail"]?["imageUrl"]?.ToString(),
                j?["thumbnail"]?["linkUrl"]?.ToString()
            );
            Links = new(j?["links"]);
        }
        /// <summary>
        /// 重写的字符串输出方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[{Ident}::{DisplayIdent}::{IATAIdent}::{FriendlyIdent}]\n" +
                $"{Airline}\n";
        }
    }
    /// <summary>
    /// 其他链接结构体
    /// </summary>
    public struct FLinks
    {
        /// <summary>
        /// 运营者页面
        /// </summary>
        public string? Operated;
        /// <summary>
        /// 永久链接页面
        /// </summary>
        public string? Permanent;
        /// <summary>
        /// 跟踪记录页
        /// </summary>
        public string? TrackLog;
        /// <summary>
        /// 飞机历史页
        /// </summary>
        public string? FlightHistory;
        /// <summary>
        /// 购买历史页面
        /// </summary>
        public string? BuyFlightHistory;
        /// <summary>
        /// 报告误失
        /// </summary>
        public string? ReportInaccuracies;
        /// <summary>
        /// Facebook
        /// </summary>
        public string? Facebook;
        /// <summary>
        /// Twitter
        /// </summary>
        public string? Twitter;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="j">传入的JToken</param>
        public FLinks(JToken? j)
        {
            Operated = j?["operated"]?.ToString();
            Permanent = j?["permanent"]?.ToString();
            TrackLog = j?["trackLog"]?.ToString();
            FlightHistory = j?["flightHistory"]?.ToString();
            BuyFlightHistory = j?["buyFlightHistory"]?.ToString();
            ReportInaccuracies = j?["reportInaccuracies"]?.ToString();
            Facebook = j?["facebook"]?.ToString();
            Twitter = j?["twitter"]?.ToString();
        }
    }
    /// <summary>
    /// 飞行跟踪结构体
    /// </summary>
    public struct FTrack
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public long? Timestamp;
        /// <summary>
        /// 坐标
        /// </summary>
        public FLatLon Coord;
        /// <summary>
        /// 高度(FL)
        /// </summary>
        public int? Alt;
        /// <summary>
        /// 地速
        /// </summary>
        public int? GS;
        /// <summary>
        /// 发现类型
        /// </summary>
        public string? Type;
        /// <summary>
        /// 是否独立
        /// </summary>
        public bool? Isolated;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="j">传入的JToken</param>
        public FTrack(JToken? j)
        {
            Timestamp = j?["timestamp"]?.ToObject<long?>();
            Coord = new(
                j?["coord"]?[1]?.ToObject<double?>(),
                j?["coord"]?[0]?.ToObject<double?>()
                );
            Alt = j?["alt"]?.ToObject<int?>()*100;
            GS = j?["gs"]?.ToObject<int?>();
            Type = j?["type"]?.ToString();
            Isolated = j?["isolated"]?.ToObject<bool?>();
        }
        /// <summary>
        /// 重写的字符串输出方法
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"[{Type}] {Timestamp?.Second()}@{Coord} {Alt}ft {GS}kt";
    }

    /// <summary>
    /// 飞行跟踪前置信息获取
    /// </summary>
    public struct FlightTokenInfo
    {
        /// <summary>
        /// 申请位置
        /// </summary>
        public string Token;
        /// <summary>
        /// 周期
        /// </summary>
        public int? Interval;
        /// <summary>
        /// 是否单程
        /// </summary>
        public bool SingalFlight;
        /// <summary>
        /// 用户令牌
        /// </summary>
        public string? UserToken;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="jo">获取的数据层</param>
        public FlightTokenInfo(JToken? jo)
        {
            Token = jo?["TOKEN"]?.ToString() ?? "";
            Interval = jo?["INTERVAL"]?.ToObject<int>();
            SingalFlight = jo?["SINGLE_FLIGHT"]?.ToObject<bool>() ?? false;
            UserToken = jo?["USERTOKEN"]?.ToString().Trim();
        }
        /// <summary>
        /// 重写的字符串输出方法
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"[{UserToken}::{Interval}-{(SingalFlight ? "▉" : " ")}]\n::[{Token}]";
    }

    /// <summary>
    /// 飞机类
    /// </summary>
    public struct FlightInfo
    {
        /// <summary>
        /// 点对点模式
        /// </summary>
        public readonly bool? ADHOC;
        /// <summary>
        /// 点对点模式可用状态
        /// </summary>
        public readonly bool? ADHOCAvaliable;
        /// <summary>
        /// 飞机类型(厂商模式)
        /// </summary>
        public FAircraftType? AirCraft;
        /// <summary>
        /// 飞机类型
        /// </summary>
        public string? AirCraftType;
        /// <summary>
        /// 飞机类型(一般受众模式)
        /// </summary>
        public string? AirCraftTypeFriendly;
        /// <summary>
        /// 备选状态
        /// </summary>
        public readonly bool? AireonCandidate;
        /// <summary>
        /// 航司
        /// </summary>
        public FAirline? Airline;
        /// <summary>
        /// 高度*100ft
        /// </summary>
        public string? Altitude;
        /// <summary>
        /// 高度转换状态
        /// </summary>
        public string? AltitudeChange;
        /// <summary>
        /// ATC识别号
        /// </summary>
        public string? ATCIdent;
        /// <summary>
        /// 平均延误(秒)
        /// </summary>
        public (int? Departure, int? Arrival) AverageDelays;
        /// <summary>
        /// 搜索状态
        /// </summary>
        public readonly bool? Blocked;
        /// <summary>
        /// 用户搜索状态
        /// </summary>
        public readonly bool? BlockedForUser;
        /// <summary>
        /// 搜索屏蔽信息
        /// </summary>
        public string? BlockMessage;
        /// <summary>
        /// 客舱信息
        /// </summary>
        public (string? Text, string? Url) CabinInfo;
        /// <summary>
        /// 航班是否取消
        /// </summary>
        public readonly bool? Cancelled;
        /// <summary>
        /// 航班可编辑状态
        /// </summary>
        public readonly bool? CanEdit;
        /// <summary>
        /// 驾驶舱信息
        /// </summary>
        public string? CockpitInformation;
        /// <summary>
        /// 曾用班号
        /// </summary>
        public FCodeShare? CodeShare;
        /// <summary>
        /// 坐标信息
        /// </summary>
        public string? Coord;
        /// <summary>
        /// 终到位置
        /// </summary>
        public FLocation? Destination;
        /// <summary>
        /// 显示识别号
        /// </summary>
        public string? DisplayIdent;
        /// <summary>
        /// 距离参数
        /// </summary>
        public FDistance? Distance;
        /// <summary>
        /// 是否备降
        /// </summary>
        public readonly bool? Diverted;
        /// <summary>
        /// 加密后航班识别号
        /// </summary>
        public string? EncryptedFlightId;
        /// <summary>
        /// 航班识别号
        /// </summary>
        public string? FlightId;
        /// <summary>
        /// 飞行计划
        /// </summary>
        public FPlan? FlightPlan;
        /// <summary>
        /// 飞机状态
        /// </summary>
        public string? FlightStatus;
        /// <summary>
        /// 飞行性能提示是否可用
        /// </summary>
        public readonly bool? FPASAvaliable;
        /// <summary>
        /// 识别号
        /// </summary>
        public readonly string? FriendlyIdent;
        /// <summary>
        /// 频率参考单位超控
        /// </summary>
        public readonly bool? FRUOverride;
        /// <summary>
        /// 引导航空 (?)
        /// </summary>
        public readonly bool? GA;
        /// <summary>
        /// 到登机口时间
        /// </summary>
        public FTimes? GateArrivalTimes;
        /// <summary>
        /// 计划立场时间
        /// </summary>
        public FTimes? GateDepartureTimes;
        /// <summary>
        /// 预选
        /// </summary>
        public readonly bool? GlobalCandidate;
        /// <summary>
        /// 通用识别
        /// </summary>
        public readonly bool? GlobalIdent;
        /// <summary>
        /// 通用飞机细节参考
        /// </summary>
        public readonly bool? GlobalFlightFeatures;
        /// <summary>
        /// 通用五遍参考
        /// </summary>
        public readonly bool? GlobalLegSharing;
        /// <summary>
        /// 其他服务
        /// </summary>
        public object? GlobalServices;
        /// <summary>
        /// 通用观察逻辑
        /// </summary>
        public readonly bool? GlobalVisualizer;
        /// <summary>
        /// 地速
        /// </summary>
        public string? Groundspeed;
        /// <summary>
        /// 航向
        /// </summary>
        public string? Heading;
        /// <summary>
        /// Hex识别号
        /// </summary>
        public string? HexId;
        /// <summary>
        /// 是否可查历史记录
        /// </summary>
        public readonly bool? Historical;
        /// <summary>
        /// IATA识别号
        /// </summary>
        public string? IATAIdent;
        /// <summary>
        /// 图标 (?)
        /// </summary>
        public string? ICON;
        /// <summary>
        /// 识别号
        /// </summary>
        public string? Ident;
        /// <summary>
        /// 进港飞机信息
        /// </summary>
        public (string? FlightId, string? LinkUrl)? InBoundFlight;
        /// <summary>
        /// 刷新时间
        /// </summary>
        public string? Internal;
        /// <summary>
        /// 是否区间
        /// </summary>
        public readonly bool? Interregional;
        /// <summary>
        /// 降落时间
        /// </summary>
        public FTimes? LandingTimes;
        /// <summary>
        /// 相关链接
        /// </summary>
        public FLinks? Links;
        /// <summary>
        /// 起飞地点
        /// </summary>
        public FLocation? Origin;
        /// <summary>
        /// 永久链接
        /// </summary>
        public string? PermaLink;
        /// <summary>
        /// 关车时间
        /// </summary>
        public string? PoweredOff;
        /// <summary>
        /// 开车时间
        /// </summary>
        public string? PoweredOn;
        /// <summary>
        /// 预测是否可用
        /// </summary>
        public readonly bool? PredictedAvaliable;
        /// <summary>
        /// 预测时间
        /// </summary>
        public (long? @out, long? off, long? on, long? @in) PredictedTimes;
        /// <summary>
        /// 过时的尾号
        /// </summary>
        public string? RedactedBlockedTail;
        /// <summary>
        /// 是否过时呼号
        /// </summary>
        public readonly bool? RedactedCallsign;
        /// <summary>
        /// 是否过时尾号
        /// </summary>
        public readonly bool? RedactedTail;
        /// <summary>
        /// 相关缩略图
        /// </summary>
        public List<(string? Thumbnail, string? Target)> RelatedThumbnails = new();
        /// <summary>
        /// 其他定义(注)
        /// </summary>
        public string? Remarks;
        /// <summary>
        /// 结果未知
        /// </summary>
        public readonly bool? ResultUnknown;
        /// <summary>
        /// 归一化时间戳
        /// </summary>
        public long? RoundedTimestamp;
        /// <summary>
        /// 起降跑道信息
        /// </summary>
        public (string? Origin, string? Destination)? Runways;
        /// <summary>
        /// 速度信息
        /// </summary>
        public JToken? SpeedInformation;
        /// <summary>
        /// 地面时间可见
        /// </summary>
        public readonly bool? ShowSurfaceTimes;
        /// <summary>
        /// 地面追踪可用状态
        /// </summary>
        public JToken? SurfaceTrackAvailable;
        /// <summary>
        /// 起飞时间
        /// </summary>
        public FTimes? TakeOffTimes;
        /// <summary>
        /// 入跑道时间
        /// </summary>
        public string? TaxiIn;
        /// <summary>
        /// 出跑道时间
        /// </summary>
        public string? TaxiOut;
        /// <summary>
        /// 缩略图
        /// </summary>
        public (string? ImageUrl, string? LinkUrl)? Thumbnail;
        /// <summary>
        /// 时间戳(请求)
        /// </summary>
        public long? Timestamp;
        /// <summary>
        /// 追踪记录
        /// </summary>
        public List<FTrack?> Track = new();
        /// <summary>
        /// 更新类型
        /// </summary>
        public string? UpdateType;
        /// <summary>
        /// 是否使用共享地址
        /// </summary>
        public readonly bool? UsingShareUrl;
        /// <summary>
        /// 航路点
        /// </summary>
        public List<FLatLon> Waypoints = new();
        /// <summary>
        /// 天气情况
        /// </summary>
        public JToken? Weather;

        /// <summary>
        /// 初始化一个飞机
        /// </summary>
        /// <param name="i"></param>
        public FlightInfo(JToken? i)
        {
            ADHOC = i?["adhoc"]?.ToObject<bool?>();
            ADHOCAvaliable = i?["adhocAvailable"]?.ToObject<bool?>();
            AirCraft = new(i?["aircraft"]);
            AirCraftType = i?["aircraftType"]?.ToString();//****
            AirCraftTypeFriendly = i?["aircraftTypeFriendly"]?.ToString();//****
            AireonCandidate = i?["aireonCandidate"]?.ToObject<bool?>();
            Airline = new(i?["airline"]);
            Altitude = i?["altitude"]?.ToString();
            AltitudeChange = i?["altitudeChange"]?.ToString();
            ATCIdent = i?["atcIdent"]?.ToString();
            if (i?["averageDelays"]?.HasValues ?? false)
            {
                AverageDelays = new(
                    i?["averageDelays"]?["departure"]?.ToObject<int?>(),
                    i?["averageDelays"]?["arrival"]?.ToObject<int?>()
                    );
            }
            else
            {
                AverageDelays = new(null, null);
            }
            Blocked = i?["blocked"]?.ToObject<bool?>();
            BlockedForUser = i?["blockedForUser"]?.ToObject<bool?>();
            BlockMessage = i?["blockMessage"]?.ToString();
            if (i?["cabinInfo"]?.HasValues ?? false)
            {
                CabinInfo = new(
                    i?["cabinInfo"]?["text"]?.ToString(),
                    i?["cabinInfo"]?["links"]?.ToString()
                    );
            }
            else
            {
                CabinInfo = new(null, null);
            }
            CanEdit = i?["canEdit"]?.ToObject<bool?>();//****
            Cancelled = i?["cancelled"]?.ToObject<bool?>();
            CockpitInformation = i?["cockpitInformation"]?.ToString();
            CodeShare = new(i?["codeShare"]);
            Coord = i?["coord"]?.ToString();//*****
            Destination = new(i?["destination"]);
            DisplayIdent = i?["displayIdent"]?.ToString();
            Distance = new(i?["distance"]);
            Diverted = i?["diverted"]?.ToObject<bool?>();
            EncryptedFlightId = i?["encryptedFlightId"]?.ToString();
            FlightId = i?["flightId"]?.ToString();
            FlightPlan = new(i?["flightPlan"]);
            FlightStatus = i?["flightStatus"]?.ToString();
            FPASAvaliable = i?["fpasAvailable"]?.ToObject<bool?>();
            FriendlyIdent = i?["friendlyIdent"]?.ToString();
            FRUOverride = i?["fruOverride"]?.ToObject<bool?>();
            GA = i?["ga"]?.ToObject<bool?>();
            GateArrivalTimes = new(i?["gateArrivalTimes"]);
            GateDepartureTimes = new(i?["gateDepartureTimes"]);
            GlobalCandidate = i?["globalCandidate"]?.ToObject<bool?>();
            GlobalIdent = i?["globalIdent"]?.ToObject<bool?>();
            GlobalFlightFeatures = i?["globalFlightFeatures"]?.ToObject<bool?>();
            GlobalLegSharing = i?["globalLegSharing"]?.ToObject<bool?>();
            GlobalServices = i?["globalServices"]?.ToObject<object?>();
            GlobalVisualizer = i?["globalVisualizer"]?.ToObject<bool?>();
            Groundspeed = i?["groundspeed"]?.ToString();
            Heading = i?["heading"]?.ToString();
            HexId = i?["hexid"]?.ToString();
            Historical = i?["historical"]?.ToObject<bool?>();
            IATAIdent = i?["iataIdent"]?.ToString();
            ICON = i?["icon"]?.ToString();
            Ident = i?["ident"]?.ToString();
            if (i?["inboundFlight"]?.HasValues ?? false)
            {
                InBoundFlight = new(
                    i?["inboundFlight"]?["flightId"]?.ToString(),
                    i?["inboundFlight"]?["linkUrl"]?.ToString()
                );
            }
            else
            {
                InBoundFlight = null;
            }
            Internal = i?["internal"]?.ToString();
            Interregional = i?["interregional"]?.ToObject<bool?>();
            LandingTimes = new(i?["landingTimes"]);
            Links = new(i?["links"]);
            Origin = new(i?["origin"]);
            PermaLink = i?["permaLink"]?.ToString();//*****
            PoweredOff = i?["poweredOff"]?.ToString();
            PoweredOn = i?["poweredOn"]?.ToString();
            PredictedAvaliable = i?["predictedAvailable"]?.ToObject<bool?>();
            if (i?["predictedTimes"]?.HasValues ?? false)
            {
                PredictedTimes = new(
                    i?["predictedTimes"]?["out"]?.ToObject<long?>(),
                    i?["predictedTimes"]?["off"]?.ToObject<long?>(),
                    i?["predictedTimes"]?["on"]?.ToObject<long?>(),
                    i?["predictedTimes"]?["in"]?.ToObject<long?>()
                    );
            }
            else
            {
                PredictedTimes = new(null, null, null, null);
            }
            RedactedBlockedTail = i?["redactedBlockedTail"]?.ToString();
            RedactedCallsign = i?["redactedCallsign"]?.ToObject<bool?>();
            RedactedTail = i?["redactedTail"]?.ToObject<bool?>();
            foreach (var j in JArray.Parse(string.IsNullOrWhiteSpace(i?["relatedThumbnails"]?.ToString()) ? "[]" : i?["relatedThumbnails"]?.ToString() ?? ""))
            {
                RelatedThumbnails.Add(new(j?["thumbnail"]?.ToString(), new(j?["target"]?.ToString())));
            }
            Remarks = i?["remarks"]?.ToString();
            ResultUnknown = i?["resultUnknown"]?.ToObject<bool?>();
            RoundedTimestamp = i?["roundedTimestamp"]?.ToObject<long?>();
            if (i?["runways"]?.HasValues ?? false)
            {
                Runways = new(
                   i?["runways"]?["origin"]?.ToString(),
                   i?["runways"]?["destination"]?.ToString()
                   );
            }
            else
            {
                Runways = null;
            }
            SpeedInformation = i?["speedInformation"];//?.ToString();
            ShowSurfaceTimes = i?["showSurfaceTimes"]?.ToObject<bool?>();
            SurfaceTrackAvailable = i?["surfaceTrackAvailable"];//?.ToObject<object?>();
            TakeOffTimes = new(i?["takeoffTimes"]);
            TaxiIn = i?["taxiIn"]?.ToString();
            TaxiOut = i?["taxiOut"]?.ToString();
            if (i?["thumbnail"]?.HasValues ?? false)
            {
                Thumbnail = new(
                    i?["thumbnail"]?["imageUrl"]?.ToString(),
                    i?["thumbnail"]?["linkUrl"]?.ToString()
                    );
            }
            else
            {
                Thumbnail = null;
            }
            Timestamp = i?["timestamp"]?.ToObject<long?>();
            foreach (var j in JArray.Parse(string.IsNullOrWhiteSpace(i?["track"]?.ToString()) ? "[]" : i?["track"]?.ToString() ?? ""))
            {
                Track.Add(new(j));
            }
            UpdateType = i?["updateType"]?.ToString();
            UsingShareUrl = i?["usingShareUrl"]?.ToObject<bool?>();
            foreach (var j in JArray.Parse(string.IsNullOrWhiteSpace(i?["waypoints"]?.ToString()) ? "[]" : i?["waypoints"]?.ToString() ?? ""))
            {
                Waypoints.Add(new(j?[1]?.ToObject<double?>(), j?[0]?.ToObject<double?>()));
            }
            Weather = i?["weather"];//?.ToString();
        }

        /// <summary>
        /// 获取路径信息
        /// </summary>
        /// <returns></returns>
        public string GetTrack()
        {
            StringBuilder sb = new();
            foreach (var i in Track)
            {
                sb.AppendLine(i?.ToString());
            }
            return sb.ToString();
        }
        public string GetBoolMap()
        {
            StringBuilder sb = new();
            sb.AppendLine("┌───────┬──────┬───┬──┬──┐");
            sb.AppendLine("│AAACDFF│GGGGGG│HIP│RR│RS│");
            sb.AppendLine("│DSCAIPR│ACIFLV│INA│DD│EW│");
            sb.AppendLine("│HT NVAU│_ADFSI│STV│ST│SS│");
            sb.AppendLine("│OA_CESO│_NNS_S│TEI│GA│TF│");
            sb.AppendLine("│CT_LDSD│_DT__U│YRL│NL│TT│");
            sb.AppendLine("├───────┼──────┼───┼──┼──┤");
            sb.Append('│');
            sb.Append(ADHOC ?? false ? "▉" : " ");
            sb.Append((ADHOCAvaliable ?? false ? "▉" : " "));
            sb.Append((AireonCandidate ?? false ? "▉" : " "));
            sb.Append((Cancelled ?? false ? "▉" : " "));
            sb.Append((Diverted ?? false ? "▉" : " "));
            sb.Append((FPASAvaliable ?? false ? "▉" : " "));
            sb.Append((FRUOverride ?? false ? "▉" : " "));
            sb.Append('│');
            sb.Append((GA ?? false ? "▉" : " "));
            sb.Append((GlobalCandidate ?? false ? "▉" : " "));
            sb.Append((GlobalIdent ?? false ? "▉" : " "));
            sb.Append((GlobalFlightFeatures ?? false ? "▉" : " "));
            sb.Append((GlobalLegSharing ?? false ? "▉" : " "));
            sb.Append((GlobalVisualizer ?? false ? "▉" : " "));
            sb.Append('│');
            sb.Append((Historical ?? false ? "▉" : " "));
            sb.Append((Interregional ?? false ? "▉" : " "));
            sb.Append((PredictedAvaliable ?? false ? "▉" : " "));
            sb.Append('│');
            sb.Append((RedactedCallsign ?? false ? "▉" : " "));
            sb.Append((RedactedTail ?? false ? "▉" : " "));
            sb.Append('│');
            sb.Append((ResultUnknown ?? false ? "▉" : " "));
            sb.Append((ShowSurfaceTimes ?? false ? "▉" : " "));
            sb.AppendLine("│");
            sb.AppendLine("└───────┴──────┴───┴──┴──┘");
            return sb.ToString();
        }
        public string GetTimeTables()
        {
            StringBuilder sb = new();
            if (!string.IsNullOrWhiteSpace(PoweredOn))
            {
                sb.AppendLine($"PWR--ON:{PoweredOn}");
            }
            if (!string.IsNullOrWhiteSpace(PoweredOff))
            {
                sb.AppendLine($"PWR-OFF:{PoweredOff}");
            }
            if (!string.IsNullOrWhiteSpace(TaxiIn))
            {
                sb.AppendLine($"TAXI-IN:{TaxiIn}");
            }
            if (!string.IsNullOrWhiteSpace(TaxiOut))
            {
                sb.AppendLine($"TAXIOUT:{TaxiOut}");
            }
            sb.AppendLine($"TAKEOFF:{TakeOffTimes}");
            sb.AppendLine($"LANDING:{LandingTimes}");
            sb.AppendLine($"GATEARI:{GateArrivalTimes}");
            sb.AppendLine($"GATEDEP:{GateDepartureTimes}");
            return sb.ToString();
        }
        public string GetIdent()
        {
            if (!string.IsNullOrWhiteSpace(Ident) && DisplayIdent == Ident)
            {
                if(!string.IsNullOrWhiteSpace(FriendlyIdent))
                {
                    if (!string.IsNullOrWhiteSpace(IATAIdent))
                    {
                        return $"{FriendlyIdent} [{Ident}/{IATAIdent}]";
                    }
                    else
                    {
                        return $"{FriendlyIdent} [{Ident}]";
                    }
                }
                else
                {
                    return $"[{Ident}]";
                }
            }
            else if(!string.IsNullOrWhiteSpace(FriendlyIdent) && string.IsNullOrWhiteSpace(Ident))
            {
                return $"{FriendlyIdent}";
            }
            else
            {
                return FlightId ?? "";
            }
        }
        public string GetOrigDest()
        {
            return $"[{Origin?.ICAO} {Origin?.FriendlyName}] --> [{Destination?.ICAO} {Destination?.FriendlyName}]";
        }
        public override string ToString()
        {
            return $"{GetIdent()}\n" +
                $"From->To\n" +
                $"{GetOrigDest()}\n" +
                $"FlightPlan:\n" +
                $"{FlightPlan}\n" +
                $"TimeTables:\n" +
                $"{GetTimeTables()}\n" +
                $"FlightBoolMap:\n" +
                $"{GetBoolMap()}\n" +
                $"Track:\n" +
                $"{GetTrack()}";
        }
    }
}
