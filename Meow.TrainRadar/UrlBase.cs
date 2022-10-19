using Newtonsoft.Json.Linq;
using System.Text;

namespace Meow.TrainRadar
{
    public static class SearchBase
    {
        public static LangPref? Lang = null;
        static readonly string BaseUrl = "http://cnrail.geogv.org/api/v1/";
        private static string Langprefset() => $"{(Lang != null ? $"?locale={Lang}" : "")}";
        public static string GetMatchFeature(string feat) => Util.Network.Http.Get.String($"{BaseUrl}match_feature/{feat}{Langprefset()}").GetAwaiter().GetResult();
        public static string GetMatchTrain(string num) => Util.Network.Http.Get.String($"{BaseUrl}match_train/{num}{Langprefset()}").GetAwaiter().GetResult();
        public static string GetSpecificTrainRoute(string num) => Util.Network.Http.Get.String($"{BaseUrl}route/{num}{Langprefset()}").GetAwaiter().GetResult();
        public static string GetSpecificTrainStation(string num) => Util.Network.Http.Get.String($"{BaseUrl}station/{num}{Langprefset()}").GetAwaiter().GetResult();
        public static string GetSpecificRail(string num) => Util.Network.Http.Get.String($"{BaseUrl}rail/{num}{Langprefset()}").GetAwaiter().GetResult();
    }

    /// <summary>
    /// 车站,线路查找结果
    /// </summary>
    public struct FeatureMatchResult
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id;
        /// <summary>
        /// 获取的模式类型
        /// </summary>
        public SearchType Type;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 重载的字符串显示
        /// <para>$"[{Id}] {Type} : {Name}"</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"[{Id}] {Type} : {Name}";

        /// <summary>
        /// 获取详细信息
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        JObject GetInfo() => Type switch
        {
            SearchType.STATION => JObject.Parse(SearchBase.GetSpecificTrainStation(Id.ToString())),
            SearchType.RAIL => JObject.Parse(SearchBase.GetSpecificRail(Id.ToString())),
            _ => throw new Exception("Err on Type"),
        };
        /// <summary>
        /// 获取路线信息
        /// </summary>
        /// <returns></returns>
        public RailInfo? GetIfRailInfo()
        {
            if (Type == SearchType.RAIL)
            {
                var jo = GetInfo()?["data"];
                if (jo != null)
                {
                    List<REndPoint> rep = new();
                    string v = "";
                    if (jo?["diagram"]?.HasValues ?? false)
                    {
                        v = jo?["diagram"]?["template"]?.ToString() ?? "";
                        var Ja = JArray.Parse(jo?["diagram"]?["records"]?.ToString() ?? "[]");
                        foreach (var i in Ja)
                        {
                            var jopot = JArray.Parse(i?[3]?.ToString() ?? "[]");
                            List<REndPointInner> repi = new();
                            foreach (var j in jopot)
                            {
                                repi.Add(new()
                                {
                                    Type = Enum.Parse<StationType>(j?[0]?.ToString() ?? "N"),
                                    Id = int.Parse(j?[1]?.ToString() ?? "0"),
                                    Name = j?[2]?.ToString() ?? "",
                                });
                            }
                            rep.Add(new()
                            {
                                SType = i?[0]?.ToString() ?? "",
                                Miles = string.IsNullOrWhiteSpace(i?[1]?.ToString()) ? null : int.Parse(i?[1]?.ToString() ?? "0"),
                                RailType = i?[2]?.ToString() ?? "",
                                Points = repi.ToArray(),
                            });
                        }
                    }
                    return new RailInfo()
                    {
                        Name = jo?["name"]?.ToString() ?? "",
                        Linenum = int.Parse(jo?["linenum"]?.ToString() ?? "0"),
                        DesignSpeed = jo?["designSpeed"]?.ToString() ?? "",
                        Elec = jo?["elec"]?.ToString() ?? "",
                        RailService = Enum.Parse<RailServiceType>(jo?["railService"]?.ToString() ?? "N"),
                        RailType = Enum.Parse<RailSpdType>(jo?["railType"]?.ToString() ?? "N"),
                        Notes = JArray.Parse(jo?["notes"]?.ToString() ?? "[]").ToObject<string[]>() ?? Array.Empty<string>(),
                        Reference = JArray.Parse(jo?["reference"]?.ToString() ?? "[]").ToObject<string[]>() ?? Array.Empty<string>(),
                        Diagram = new()
                        {
                            Template = v,
                            Records = rep.ToArray(),
                        }
                    };
                }
            }
            return null;
        }
        /// <summary>
        /// 获得车站信息
        /// </summary>
        /// <returns></returns>
        public StationInfo? GetIfStationInfo()
        {
            if(Type == SearchType.STATION)
            {
                var jo = GetInfo();
                var spsk = (jo?["serviceClass"]?.ToString() ?? "").Split(';');
                List<ServiceType> st = new();
                foreach(var s in spsk)
                {
                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        st.Add(Enum.Parse<ServiceType>(s));
                    }
                }
                if (jo != null)
                {
                    return new StationInfo()
                    {
                        TeleCode = jo?["teleCode"]?.ToString() ?? "",
                        PinyinCode = jo?["pinyinCode"]?.ToString() ?? "",
                        Type = Enum.Parse<StationType>(jo?["type"]?.ToString() ?? ""),
                        Bureau = new()
                        {
                            Name = jo?["bureau"]?["name"]?.ToString() ?? "",
                            Logo = jo?["bureau"]?["logo"]?.ToString() ?? "",
                        },
                        Operators = jo?["operators"]?.ToString() ?? "",
                        Id = jo?["id"]?.ToString() ?? "",
                        LocalName = jo?["localName"]?.ToString() ?? "",
                        LocalizedName = jo?["localizedName"]?.ToString() ?? "",
                        SedName = jo?["sedName"]?.ToString() ?? "",
                        Status = jo?["status"]?.ToString() ?? "",
                        Country = jo?["country"]?.ToString() ?? "",
                        FirstScale = jo?["firstScale"]?.ToString() ?? "",
                        ServiceClass = st.ToArray(),
                        X = jo?["x"]?.ToObject<int>() ?? 0,
                        Y = jo?["y"]?.ToObject<int>() ?? 0,
                        Location = jo?["location"]?.ToString() ?? "",
                    };
                }
            }
            return null;
        }
    }

    /// <summary>
    /// 车次匹配结果
    /// </summary>
    public struct TrainMatchResult
    {
        /// <summary>
        /// 车次识别号
        /// </summary>
        public string Id;
        /// <summary>
        /// 搜索类型
        /// </summary>
        public SearchType Type;
        /// <summary>
        /// 搜索名
        /// </summary>
        public string Name;
        /// <summary>
        /// 始发站
        /// </summary>
        public string From;
        /// <summary>
        /// 终到站
        /// </summary>
        public string To;
        /// <summary>
        /// 重载的字符串显示
        /// <para>$"[{Id}] {Name} ({From})->({To})"</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"[{Id}] {Name} ({From})->({To})";
        /// <summary>
        /// 获取本车的路线
        /// </summary>
        /// <returns></returns>
        public RouteInfo? GetRouteInfo()
        {
            var jo = JObject.Parse(SearchBase.GetSpecificTrainRoute(Id))?["data"];
            if(jo != null)
            {
                var stops = JArray.Parse(jo?["stops"]?.ToString() ?? "[]");
                List<RouteStops> rs = new();
                foreach(var i in stops)
                {
                    rs.Add(new()
                    {
                        Id = i?[0]?.ToObject<int>() ?? -1,
                        Name = i?[1]?.ToString() ?? "",
                        Starts = i?[2]?.ToString() ?? "",
                        Ends = i?[3]?.ToString() ?? "",
                        Notes = i?[4]?.ToString() ?? "",
                        Notes2 = i?[5]?.ToString() ?? "",
                    });
                }
                return new RouteInfo()
                {
                    TrainId = jo?["trainId"]?.ToString() ?? "",
                    OperationId = jo?["operationId"]?.ToString() ?? "",
                    ServiceId = Enum.Parse<ServiceType>(jo?["serviceId"]?.ToString() ?? "Other"),
                    Operators = JArray.Parse(jo?["operators"]?.ToString() ?? "[]").ToObject<string[]>() ?? Array.Empty<string>(),
                    Note = jo?["note"]?.ToString() ?? "",
                    Frequency = jo?["frequency"]?.ToString() ?? "",
                    TimeTableFormat = jo?["timeTableFormat"]?.ToString() ?? "",
                    OpSegment = jo?["opSegment"]?.ToString() ?? "",
                    RouteType = jo?["routeType"]?.ToString() ?? "",
                    Stops = rs.ToArray(),
                };
            }
            return null;
        }
    }

    public static class TRGet
    {
        /// <summary>
        /// 搜索车站和线路
        /// </summary>
        /// <param name="feat">搜索词</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static FeatureMatchResult[] Feature(string feat)
        {
            var jo = JObject.Parse(SearchBase.GetMatchFeature(feat));
            if (jo != null)
            {
                if(jo["success"]?.ToObject<bool>() == true)
                {
                    if (!string.IsNullOrEmpty(jo?["data"]?.ToString()))
                    {
                        List<FeatureMatchResult> r = new();
                        var ja = JArray.Parse(jo?["data"]?.ToString() ?? "");
                        foreach(var i in ja)
                        {
                            r.Add(new FeatureMatchResult()
                            {
                                Id = i?[0]?.ToObject<int>() ?? -1,
                                Type = Enum.Parse<SearchType>(i?[1]?.ToString() ?? ""),
                                Name = i?[2]?.ToString() ?? "",
                            });
                        }
                        return r.ToArray();
                    }
                    else
                    {
                        throw new Exception("API return Errs: Arrays Unreadable");
                    }
                }
                else
                {
                    throw new Exception("API return Errs: Not Intrepretable");
                }
            }
            else
            {
                throw new Exception("Message Err: Not Intrepretable");
            }
        }
        /// <summary>
        /// 搜索列车
        /// </summary>
        /// <param name="feat">搜索词</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static TrainMatchResult[] Train(string feat)
        {
            var jo = JObject.Parse(SearchBase.GetMatchTrain(feat));
            if (jo != null)
            {
                if (jo["success"]?.ToObject<bool>() == true)
                {
                    if (!string.IsNullOrEmpty(jo?["data"]?.ToString()))
                    {
                        List<TrainMatchResult> r = new();
                        var ja = JArray.Parse(jo?["data"]?.ToString() ?? "");
                        foreach (var i in ja)
                        {
                            r.Add(new TrainMatchResult()
                            {
                                Id = i?[0]?.ToString() ?? "",
                                Type = Enum.Parse<SearchType>(i?[1]?.ToString() ?? ""),
                                Name = i?[2]?.ToString() ?? "",
                                From = i?[3]?.ToString() ?? "",
                                To = i?[4]?.ToString() ?? "",
                            });
                        }
                        return r.ToArray();
                    }
                    else
                    {
                        throw new Exception("API return Errs: Arrays Unreadable");
                    }
                }
                else
                {
                    throw new Exception("API return Errs: Not Intrepretable");
                }
            }
            else
            {
                throw new Exception("Message Err: Not Intrepretable");
            }
        }
    }
}