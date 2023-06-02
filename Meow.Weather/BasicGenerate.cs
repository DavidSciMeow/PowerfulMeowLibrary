using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Meow.Util.Network.Http;

namespace Meow.Weather.CN
{
    public sealed class Interpreter
    {
        readonly static StaticHttp Client = new(avoidSSLCertifaction: true, autoDecomp: false);
        public static MWG RectifyWord(string input)
        {
            return input switch
            {
                "cdwh0" => new 主要功能.天气图.基本天气分析.中国.标准气压地面(),
                "cdwh9" => new 主要功能.天气图.基本天气分析.中国.气压925百帕(),
                "cdwh8" => new 主要功能.天气图.基本天气分析.中国.气压850百帕(),
                "cdwh7" => new 主要功能.天气图.基本天气分析.中国.气压700百帕(),
                "cdwh5" => new 主要功能.天气图.基本天气分析.中国.气压500百帕(),
                "cdwh2" => new 主要功能.天气图.基本天气分析.中国.气压200百帕(),
                "cdwh1" => new 主要功能.天气图.基本天气分析.中国.气压100百帕(),

                "adwh0" => new 主要功能.天气图.基本天气分析.亚欧.标准气压地面(),
                "adwh9" => new 主要功能.天气图.基本天气分析.亚欧.气压925百帕(),
                "adwh8" => new 主要功能.天气图.基本天气分析.亚欧.气压850百帕(),
                "adwh7" => new 主要功能.天气图.基本天气分析.亚欧.气压700百帕(),
                "adwh5" => new 主要功能.天气图.基本天气分析.亚欧.气压500百帕(),
                "adwh2" => new 主要功能.天气图.基本天气分析.亚欧.气压200百帕(),
                "adwh1" => new 主要功能.天气图.基本天气分析.亚欧.气压100百帕(),

                "ndwh0" => new 主要功能.天气图.基本天气分析.北半球.标准气压地面(),
                "ndwh9" => new 主要功能.天气图.基本天气分析.北半球.气压925百帕(),
                "ndwh8" => new 主要功能.天气图.基本天气分析.北半球.气压850百帕(),
                "ndwh7" => new 主要功能.天气图.基本天气分析.北半球.气压700百帕(),
                "ndwh5" => new 主要功能.天气图.基本天气分析.北半球.气压500百帕(),
                "ndwh2" => new 主要功能.天气图.基本天气分析.北半球.气压200百帕(),
                "ndwh1" => new 主要功能.天气图.基本天气分析.北半球.气压100百帕(),

                "cdrh0" => new 主要功能.天气图.叠加卫星云图.中国.标准气压地面(),
                "cdrh9" => new 主要功能.天气图.叠加卫星云图.中国.气压925百帕(),
                "cdrh8" => new 主要功能.天气图.叠加卫星云图.中国.气压850百帕(),
                "cdrh7" => new 主要功能.天气图.叠加卫星云图.中国.气压700百帕(),
                "cdrh5" => new 主要功能.天气图.叠加卫星云图.中国.气压500百帕(),
                "cdrh2" => new 主要功能.天气图.叠加卫星云图.中国.气压200百帕(),
                "cdrh1" => new 主要功能.天气图.叠加卫星云图.中国.气压100百帕(),

                "adch0" => new 主要功能.天气图.叠加卫星云图.亚欧.标准气压地面(),
                "adch9" => new 主要功能.天气图.叠加卫星云图.亚欧.气压925百帕(),
                "adch8" => new 主要功能.天气图.叠加卫星云图.亚欧.气压850百帕(),
                "adch7" => new 主要功能.天气图.叠加卫星云图.亚欧.气压700百帕(),
                "adch5" => new 主要功能.天气图.叠加卫星云图.亚欧.气压500百帕(),
                "adch2" => new 主要功能.天气图.叠加卫星云图.亚欧.气压200百帕(),
                "adch1" => new 主要功能.天气图.叠加卫星云图.亚欧.气压100百帕(),

                "r1hp" => new 主要功能.降水量.近1小时降水量(),
                "r6hp" => new 主要功能.降水量.近6小时降水量(),
                "r24hp" => new 主要功能.降水量.近24小时降水量(),
                "r10dp" => new 主要功能.降水量.近10天降水量(),
                "r20dp" => new 主要功能.降水量.近20天降水量(),
                "r30dp" => new 主要功能.降水量.近30天降水量(),
                "r10pa" => new 主要功能.降水量.近10天降水量距平(),
                "r20pa" => new 主要功能.降水量.近20天降水量距平(),
                "r30pa" => new 主要功能.降水量.近30天降水量距平(),

                "t1hp" => new 主要功能.气温.全国逐时气温(),
                "tdta" => new 主要功能.气温.全国逐日气温平均(),
                "tdtmx" => new 主要功能.气温.全国逐日气温最高(),
                "tdtmn" => new 主要功能.气温.全国逐日气温最低(),
                "th10d" => new 主要功能.气温.近10天最高气温(),
                "th20d" => new 主要功能.气温.近20天最高气温(),
                "th30d" => new 主要功能.气温.近30天最高气温(),
                "tl10d" => new 主要功能.气温.近10天最低气温(),
                "tl20d" => new 主要功能.气温.近20天最低气温(),
                "tl30d" => new 主要功能.气温.近30天最低气温(),
                "tm10d" => new 主要功能.气温.近10天距平(),
                "tm20d" => new 主要功能.气温.近20天距平(),
                "tm30d" => new 主要功能.气温.近30天距平(),

                "owd" => new 主要功能.风.逐小时极大风速(),

                "oltng" => new 主要功能.强对流.地闪(),
                "ohvr" => new 主要功能.强对流.短时强降雨(),
                "og" => new 主要功能.强对流.雷暴大风(),
                "hail" => new 主要功能.强对流.冰雹(),

                "sf4tc" => new 主要功能.卫星云图.风云4A真彩色(),
                "sf4i" => new 主要功能.卫星云图.风云4A红外(),
                "sf4v" => new 主要功能.卫星云图.风云4A可见光(),
                "sf4wv" => new 主要功能.卫星云图.风云4A水汽(),
                "sf2v" => new 主要功能.卫星云图.风云2G可见光(),
                "sf2wv" => new 主要功能.卫星云图.风云2G增强水汽(),
                "sf2i1" => new 主要功能.卫星云图.风云2G增强红外一(),
                "sf2i2" => new 主要功能.卫星云图.风云2G增强红外二(),
                "sf2bvi" => new 主要功能.卫星云图.风云2G黑白可见光(),
                "sf2bvmi" => new 主要功能.卫星云图.风云2G黑白中红外(),
                "sf2dc" => new 主要功能.卫星云图.风云2G圆盘彩色(),
                "sf2di1" => new 主要功能.卫星云图.风云2G圆盘红外一(),
                "sf2di2" => new 主要功能.卫星云图.风云2G圆盘红外二(),
                "sf2di3" => new 主要功能.卫星云图.风云2G圆盘中红外(),
                "sf2dwv" => new 主要功能.卫星云图.风云2G圆盘水汽(),
                "sf2dv" => new 主要功能.卫星云图.风云2G圆盘可见光(),

                "rdch" => new 主要功能.雷达拼图.全国(),
                "rdhb" => new 主要功能.雷达拼图.华北(),
                "rddb" => new 主要功能.雷达拼图.东北(),
                "rdhd" => new 主要功能.雷达拼图.华东(),
                "rdhz" => new 主要功能.雷达拼图.华中(),
                "rdhn" => new 主要功能.雷达拼图.华南(),
                "rdxn" => new 主要功能.雷达拼图.西南(),
                "rdxb" => new 主要功能.雷达拼图.西北(),

                "spf" => new 主要功能.能见度.能见度(),

                "efog" => new 主要功能.环境气象.雾(),
                "ehaze" => new 主要功能.环境气象.霾(),
                "edust" => new 主要功能.环境气象.沙尘(),
                "eap24" => new 主要功能.环境气象.污染气象条件._24小时(),
                "eap48" => new 主要功能.环境气象.污染气象条件._48小时(),
                "eap72" => new 主要功能.环境气象.污染气象条件._72小时(),

                "asmm10" => new 主要功能.农业气象.土壤水分10cm(),
                "asmm20" => new 主要功能.农业气象.土壤水分20cm(),
                "asmm30" => new 主要功能.农业气象.土壤水分30cm(),
                "asmm40" => new 主要功能.农业气象.土壤水分40cm(),
                "asmm50" => new 主要功能.农业气象.土壤水分50cm(),
                "adm" => new 主要功能.农业气象.农业干旱检测(),
                "aib" => new 主要功能.农业气象.大豆发育期(),
                "aio" => new 主要功能.农业气象.油菜发育期(),
                "aipa" => new 主要功能.农业气象.牧草发育期(),
                "aipo" => new 主要功能.农业气象.马铃薯发育期(),
                "aisuc" => new 主要功能.农业气象.夏玉米发育期(),
                "aiww" => new 主要功能.农业气象.冬小麦发育期(),
                "aisw" => new 主要功能.农业气象.春小麦发育期(),
                "aic" => new 主要功能.农业气象.棉花发育期(),
                "aip" => new 主要功能.农业气象.花生发育期(),
                "aier" => new 主要功能.农业气象.早稻发育期(),
                "ailr" => new 主要功能.农业气象.晚稻发育期(),
                "aispc" => new 主要功能.农业气象.春玉米发育期(),
                "airq" => new 主要功能.农业气象.一季稻发育期(),

                "nn5hh" => new 主要功能.GRAPES全球.北半球500hpa(),
                "nn8ht" => new 主要功能.GRAPES全球.北半球850hpa(),
                "nnslp" => new 主要功能.GRAPES全球.北半球海平面气压(),
                "na5" => new 主要功能.GRAPES全球.亚欧500高800风(),
                "neslp" => new 主要功能.GRAPES全球.东亚海平面气压(),
                "ne5h8w" => new 主要功能.GRAPES全球.东亚500高800风(),
                "ne24hr" => new 主要功能.GRAPES全球.东亚24小时降水(),
                "necp" => new 主要功能.GRAPES全球.东亚积累降水(),
                "nt2m" => new 主要功能.GRAPES全球.东亚2m温度(),
                "nt2mmx" => new 主要功能.GRAPES全球.东亚2m最高温度(),
                "nt2mmn" => new 主要功能.GRAPES全球.东亚2m最低温度(),
                "nt2mrh" => new 主要功能.GRAPES全球.东亚2m相对湿度(),
                "neldfsl" => new 主要功能.GRAPES全球.东亚雷达组合反射率(),
                "nf4i" => new 主要功能.GRAPES全球.FY4A红外亮温(),
                "nf4wv" => new 主要功能.GRAPES全球.FY4A水汽亮温(),

                "ngsr1" => new 主要功能.GRAPES全球集合.累积降水24h概率预报1mm(),
                "ngsr10" => new 主要功能.GRAPES全球集合.累积降水24h概率预报10mm(),
                "ngsr25" => new 主要功能.GRAPES全球集合.累积降水24h概率预报25mm(),
                "ngsr50" => new 主要功能.GRAPES全球集合.累积降水24h概率预报50mm(),
                "ngsr100" => new 主要功能.GRAPES全球集合.累积降水24h概率预报100mm(),
                "ngsrjd" => new 主要功能.GRAPES全球集合.累积降水24h极端预报指数(),
                "ngsra" => new 主要功能.GRAPES全球集合.累积降水24h集合平均和离散度(),
                "ngsryp" => new 主要功能.GRAPES全球集合.累积降水24h邮票图(),
                "ngsrmx" => new 主要功能.GRAPES全球集合.累积降水24h分位数最大值(),
                "ngsmt" => new 主要功能.GRAPES全球集合.位势高度500hPa面条图(),
                "ngsa" => new 主要功能.GRAPES全球集合.位势高度500hPa集合平均和离散度(),
                "ngst2mf" => new 主要功能.GRAPES全球集合.温度2米极端预报指数(),
                "ngst2ma" => new 主要功能.GRAPES全球集合.温度2米集合平均和离散度(),
                "ngs10jd" => new 主要功能.GRAPES全球集合.全风速10米极端预报指数(),
                "ngs10a" => new 主要功能.GRAPES全球集合.全风速10米集合平均和离散度(),
                "ngsp10" => new 主要功能.GRAPES全球集合.全风速10米概率预报10米秒(),
                "ngsp17" => new 主要功能.GRAPES全球集合.全风速10米概率预报17米秒(),

                "nsci" => new 主要功能.GRAPES台风预报.台风强度(),
                "nsrain" => new 主要功能.GRAPES台风预报.累计降水24h(),
                "nswind" => new 主要功能.GRAPES台风预报.累计大风24h(),
                "nsrt" => new 主要功能.GRAPES台风预报.台风路径(),
                "nsw10m" => new 主要功能.GRAPES台风预报.风场10m(),
                "nsrdr" => new 主要功能.GRAPES台风预报.组合雷达反射率(),
                "nsf4i" => new 主要功能.GRAPES台风预报.FY4A云图模拟红外亮温(),
                "nsf4wv" => new 主要功能.GRAPES台风预报.FY4A云图模拟红外亮温(),

                "ogbw" => new 主要功能.海浪模式.全球浪高浪向叠加图(),
                "oatlw" => new 主要功能.海浪模式.大西洋浪高浪向叠加图(),
                "oindw" => new 主要功能.海浪模式.印度洋浪高浪向叠加图(),
                "opacw" => new 主要功能.海浪模式.太平洋浪高浪向叠加图(),

                _ => throw new("输入不正确"),
            };
        }
        public static async Task<JObject> RectifyWord(string[] input)
        {
            var provcode = await DoGetProvInfo(input[0]);
            string citycode;
            if (input.Length > 1)
            {
                citycode = await DoGetCityInfo(input[1], provcode);
            }
            else
            {
                citycode = await DoGetCityInfo(provcode);
            }
            var sb = new StringBuilder();
            sb.Append('{');
            try {
                var sx = await DoGetPercBasicInfo(citycode);
                sb.Append($"\"basic\":{(string.IsNullOrWhiteSpace(sx) || string.IsNullOrEmpty(sx) ? "\"null\"" : sx)},"); 
            } catch { }
            try {
                var sx1 = await DoGetPercAQIInfo(citycode);
                sb.Append($"\"aqi\":{(string.IsNullOrWhiteSpace(sx1) || string.IsNullOrEmpty(sx1) ? "\"null\"" : sx1)},"); 
            } catch { }
            try {
                var sx2 = await DoGetPercPassedInfo(citycode);
                sb.Append($"\"passed\":{(string.IsNullOrWhiteSpace(sx2) || string.IsNullOrEmpty(sx2) ? "\"null\"" : sx2)},"); 
            } catch { }
            try {
                var sx3 = await DoGetPercWeatherInfo(citycode);
                sb.Append($"\"predict\":{(string.IsNullOrWhiteSpace(sx3) || string.IsNullOrEmpty(sx3) ? "\"null\"" : sx3)},"); 
            } catch { }
            try {
                var sx4 = await DoGetPercTempchartInfo(citycode);
                sb.Append($"\"tempchart1w\":{(string.IsNullOrWhiteSpace(sx4) || string.IsNullOrEmpty(sx4) ? "\"null\"" : sx4)},"); 
            } catch { }
            sb.Append('}');
            return JObject.Parse(sb.ToString().Replace(",}", "}"));
        }
        private static async Task<string> DoGetProvInfo(string pattern)
        {
            var prov = await Client.GetString($"http://www.nmc.cn/f/rest/province");
            var jo = JArray.Parse(prov);
            foreach(var d in jo)
            {
                if (d["name"].ToString().Contains(pattern))
                {
                    return d["code"].ToString();
                }
            }
            throw new("Para Err");
        }
        private static async Task<string> DoGetCityInfo(string pattern,string para)
        {
            var prov = await Client.GetString($"http://www.nmc.cn/f/rest/province/{para}");
            var jo = JArray.Parse(prov);
            foreach (var d in jo)
            {
                if (d["city"].ToString().Contains(pattern))
                {
                    return d["code"].ToString();
                }
            }
            throw new("Para Err");
        }
        private static async Task<string> DoGetCityInfo(string para) => JArray.Parse(await Client.GetString($"http://www.nmc.cn/f/rest/province/{para}")).First["code"].ToString();
        private static async Task<string> DoGetPercBasicInfo(string para) => await Client.GetString($"http://www.nmc.cn/f/rest/real/{para}");
        private static async Task<string> DoGetPercAQIInfo(string para) => await Client.GetString($"http://www.nmc.cn/f/rest/aqi/{para}");
        private static async Task<string> DoGetPercPassedInfo(string para) => await Client.GetString($"http://www.nmc.cn/f/rest/passed/{para}");
        private static async Task<string> DoGetPercTempchartInfo(string para) => await Client.GetString($"http://www.nmc.cn/f/rest/tempchart/{para}");
        private static async Task<string> DoGetPercWeatherInfo(string para) => await Client.GetString($"http://www.nmc.cn/f/rest/weather/?stationid={para}");
    }
    public class WImage
    {
        public string DataTime;
        public string ImgUrl;
    }
    public abstract class MWG
    {
        public List<WImage> Data = new();
        public MWG(string input)
        {
            HtmlDocument doc = new HtmlWeb().Load($"http://www.nmc.cn/publish/{input}");
            try
            {
                var r = (doc.GetElementbyId("timeWrap")?.ChildNodes) ?? (doc.GetElementbyId("fffmmWrap")?.ChildNodes); //fffmmWrap
                foreach (var d in r)
                {
                    var rx = d.Attributes["data-img"].Value;
                    var rt = d.Attributes["data-time"].Value;
                    Data.Add(new() { DataTime = rt, ImgUrl = rx });
                }
            }
            catch (Exception ex)
            {
                throw new("可能的错误由于网页不正确,断点检查", ex);
            }
        }
    }
    public class Observations : MWG{ public Observations(string input) : base($"observations/{input}") { } }
    public class Satellite : MWG { public Satellite(string input) : base($"satellite/{input}") { } }
    public class Radar : MWG{ public Radar(string input) : base($"radar/{input}") { } }
    public class Agro : MWG{ public Agro(string input) : base($"agro/{input}") { } }

    namespace 主要功能
    {
        namespace 天气图
        {
            namespace 基本天气分析
            {
                namespace 中国
                {
                    public sealed class 标准气压地面 : Observations { public 标准气压地面() : base($"china/dm/weatherchart-h000.htm") { } }
                    public sealed class 气压925百帕 : Observations { public 气压925百帕() : base($"china/dm/weatherchart-h925.htm") { } }
                    public sealed class 气压850百帕 : Observations { public 气压850百帕() : base($"china/dm/weatherchart-h850.htm") { } }
                    public sealed class 气压700百帕 : Observations { public 气压700百帕() : base($"china/dm/weatherchart-h700.htm") { } }
                    public sealed class 气压500百帕 : Observations { public 气压500百帕() : base($"china/dm/weatherchart-h500.htm") { } }
                    public sealed class 气压200百帕 : Observations { public 气压200百帕() : base($"china/dm/weatherchart-h200.htm") { } }
                    public sealed class 气压100百帕 : Observations { public 气压100百帕() : base($"china/dm/weatherchart-h100.htm") { } }

                }
                namespace 亚欧
                {
                    public sealed class 标准气压地面 : Observations { public 标准气压地面() : base($"asia/dm/weatherchart-h000.htm") { } }
                    public sealed class 气压925百帕 : Observations { public 气压925百帕() : base($"asia/dm/weatherchart-h925.htm") { } }
                    public sealed class 气压850百帕 : Observations { public 气压850百帕() : base($"asia/dm/weatherchart-h850.htm") { } }
                    public sealed class 气压700百帕 : Observations { public 气压700百帕() : base($"asia/dm/weatherchart-h700.htm") { } }
                    public sealed class 气压500百帕 : Observations { public 气压500百帕() : base($"asia/dm/weatherchart-h500.htm") { } }
                    public sealed class 气压200百帕 : Observations { public 气压200百帕() : base($"asia/dm/weatherchart-h200.htm") { } }
                    public sealed class 气压100百帕 : Observations { public 气压100百帕() : base($"asia/dm/weatherchart-h100.htm") { } }

                }
                namespace 北半球
                {
                    public sealed class 标准气压地面 : Observations { public 标准气压地面() : base($"north/dm/weatherchart-h000.htm") { } }
                    public sealed class 气压925百帕 : Observations { public 气压925百帕() : base($"north/dm/weatherchart-h925.htm") { } }
                    public sealed class 气压850百帕 : Observations { public 气压850百帕() : base($"north/dm/weatherchart-h850.htm") { } }
                    public sealed class 气压700百帕 : Observations { public 气压700百帕() : base($"north/dm/weatherchart-h700.htm") { } }
                    public sealed class 气压500百帕 : Observations { public 气压500百帕() : base($"north/dm/weatherchart-h500.htm") { } }
                    public sealed class 气压200百帕 : Observations { public 气压200百帕() : base($"north/dm/weatherchart-h200.htm") { } }
                    public sealed class 气压100百帕 : Observations { public 气压100百帕() : base($"north/dm/weatherchart-h100.htm") { } }

                }
            }
            namespace 叠加卫星云图
            {
                namespace 中国
                {
                    public sealed class 标准气压地面 : Observations { public 标准气压地面() : base($"china/dm/cloud-h000.htm") { } }
                    public sealed class 气压925百帕 : Observations { public 气压925百帕() : base($"china/dm/cloud-h925.htm") { } }
                    public sealed class 气压850百帕 : Observations { public 气压850百帕() : base($"china/dm/cloud-h850.htm") { } }
                    public sealed class 气压700百帕 : Observations { public 气压700百帕() : base($"china/dm/cloud-h700.htm") { } }
                    public sealed class 气压500百帕 : Observations { public 气压500百帕() : base($"china/dm/cloud-h500.htm") { } }
                    public sealed class 气压200百帕 : Observations { public 气压200百帕() : base($"china/dm/cloud-h200.htm") { } }
                    public sealed class 气压100百帕 : Observations { public 气压100百帕() : base($"china/dm/cloud-h100.htm") { } }

                }
                namespace 亚欧
                {
                    public sealed class 标准气压地面 : Observations { public 标准气压地面() : base($"asia/dm/cloud-h000.htm") { } }
                    public sealed class 气压925百帕 : Observations { public 气压925百帕() : base($"asia/dm/cloud-h925.htm") { } }
                    public sealed class 气压850百帕 : Observations { public 气压850百帕() : base($"asia/dm/cloud-h850.htm") { } }
                    public sealed class 气压700百帕 : Observations { public 气压700百帕() : base($"asia/dm/cloud-h700.htm") { } }
                    public sealed class 气压500百帕 : Observations { public 气压500百帕() : base($"asia/dm/cloud-h500.htm") { } }
                    public sealed class 气压200百帕 : Observations { public 气压200百帕() : base($"asia/dm/cloud-h200.htm") { } }
                    public sealed class 气压100百帕 : Observations { public 气压100百帕() : base($"asia/dm/cloud-h100.htm") { } }

                }
            }
        }
        namespace 降水量
        {
            public sealed class 近1小时降水量 : Observations { public 近1小时降水量() : base($"hourly-precipitation.html") { } }
            public sealed class 近6小时降水量 : Observations { public 近6小时降水量() : base($"6hour-precipitation.html") { } }
            public sealed class 近24小时降水量 : Observations { public 近24小时降水量() : base($"24hour-precipitation.html") { } }
            public sealed class 近10天降水量 : Observations { public 近10天降水量() : base($"precipitation-10day.html") { } }
            public sealed class 近20天降水量 : Observations { public 近20天降水量() : base($"precipitation-20day.html") { } }
            public sealed class 近30天降水量 : Observations { public 近30天降水量() : base($"precipitation-30day.html") { } }
            public sealed class 近10天降水量距平 : Observations { public 近10天降水量距平() : base($"precipitation-10pa.html") { } }
            public sealed class 近20天降水量距平 : Observations { public 近20天降水量距平() : base($"precipitation-20pa.html") { } }
            public sealed class 近30天降水量距平 : Observations { public 近30天降水量距平() : base($"precipitation-30pa.html") { } }
        }
        namespace 气温
        {
            public sealed class 全国逐时气温 : Observations { public 全国逐时气温() : base($"hourly-temperature.html") { } }
            public sealed class 全国逐日气温平均 : Observations { public 全国逐日气温平均() : base($"day-temperature/avg.html") { } }
            public sealed class 全国逐日气温最高 : Observations { public 全国逐日气温最高() : base($"day-temperature/max.html") { } }
            public sealed class 全国逐日气温最低 : Observations { public 全国逐日气温最低() : base($"day-temperature/min.html") { } }
            public sealed class 近10天最高气温 : Observations { public 近10天最高气温() : base($"high-10days.html") { } }
            public sealed class 近20天最高气温 : Observations { public 近20天最高气温() : base($"high-20days.html") { } }
            public sealed class 近30天最高气温 : Observations { public 近30天最高气温() : base($"high-30days.html") { } }
            public sealed class 近10天最低气温 : Observations { public 近10天最低气温() : base($"low-10days.html") { } }
            public sealed class 近20天最低气温 : Observations { public 近20天最低气温() : base($"low-20days.html") { } }
            public sealed class 近30天最低气温 : Observations { public 近30天最低气温() : base($"low-30days.html") { } }
            public sealed class 近10天距平 : Observations { public 近10天距平() : base($"mta-10days.html") { } }
            public sealed class 近20天距平 : Observations { public 近20天距平() : base($"mta-20days.html") { } }
            public sealed class 近30天距平 : Observations { public 近30天距平() : base($"mta-30days.html") { } }
        }
        namespace 风
        {
            public sealed class 逐小时极大风速 : Observations { public 逐小时极大风速() : base($"hail.html") { } }
        }
        namespace 强对流
        {
            public sealed class 地闪 : Observations { public 地闪() : base($"lighting.html") { } }
            public sealed class 短时强降雨 : Observations { public 短时强降雨() : base($"heavyrain.html") { } }
            public sealed class 雷暴大风 : Observations { public 雷暴大风() : base($"gale.html") { } }
            public sealed class 冰雹 : Observations { public 冰雹() : base($"hail.html") { } }
        }
        namespace 卫星云图
        {
            public sealed class 风云4A真彩色 : Satellite { public 风云4A真彩色() : base("FY4A-true-color.htm") { } }
            public sealed class 风云4A红外 : Satellite { public 风云4A红外() : base("FY4A-infrared.htm") { } }
            public sealed class 风云4A可见光 : Satellite { public 风云4A可见光() : base("FY4A-visible.htm") { } }
            public sealed class 风云4A水汽 : Satellite { public 风云4A水汽() : base("FY4A-water-vapour.htm") { } }
            public sealed class 风云2G可见光 : Satellite { public 风云2G可见光() : base("fy2evisible.html") { } }
            public sealed class 风云2G增强水汽 : Satellite { public 风云2G增强水汽() : base("fy2e/water_vapor.html") { } }
            public sealed class 风云2G增强红外一 : Satellite { public 风云2G增强红外一() : base("fy2e/infrared_1.html") { } }
            public sealed class 风云2G增强红外二 : Satellite { public 风云2G增强红外二() : base("fy2e/infrared_2.html") { } }
            public sealed class 风云2G黑白可见光 : Satellite { public 风云2G黑白可见光() : base("fy2e_bawhite/visible_light.html") { } }
            public sealed class 风云2G黑白中红外 : Satellite { public 风云2G黑白中红外() : base("fy2e_bawhite/mid-infrared.html") { } }
            public sealed class 风云2G圆盘彩色 : Satellite { public 风云2G圆盘彩色() : base("fy2c-disc-color.html") { } }
            public sealed class 风云2G圆盘红外一 : Satellite { public 风云2G圆盘红外一() : base("fy2c-disc-ir1.html") { } }
            public sealed class 风云2G圆盘红外二 : Satellite { public 风云2G圆盘红外二() : base("fy2c-disc-ir2.html") { } }
            public sealed class 风云2G圆盘中红外 : Satellite { public 风云2G圆盘中红外() : base("fy2c-disc-ir3.html") { } }
            public sealed class 风云2G圆盘水汽 : Satellite { public 风云2G圆盘水汽() : base("fy2c-disc-wv.html") { } }
            public sealed class 风云2G圆盘可见光 : Satellite { public 风云2G圆盘可见光() : base("fy2c-disc-vis.html") { } }
        }
        namespace 雷达拼图
        {
            public sealed class 全国 : Radar { public 全国() : base("chinaall.html") { } }
            public sealed class 华北 : Radar { public 华北() : base("huabei.html") { } }
            public sealed class 东北 : Radar { public 东北() : base("dongbei.html") { } }
            public sealed class 华东 : Radar { public 华东() : base("huadong.html") { } }
            public sealed class 华中 : Radar { public 华中() : base("huazhong.html") { } }
            public sealed class 华南 : Radar { public 华南() : base("huanan.html") { } }
            public sealed class 西南 : Radar { public 西南() : base("xinan.html") { } }
            public sealed class 西北 : Radar { public 西北() : base("xibei.html") { } }
        }
        namespace 能见度
        {
            public sealed class 能见度 : MWG { public 能见度() : base($"sea/seaplatform1.html") { } }
        }
        namespace 环境气象
        {
            public sealed class 雾 : MWG{ public 雾() : base($"fog.html") { } }
            public sealed class 霾 : MWG{ public 霾() : base($"haze.html") { } }
            public sealed class 沙尘 : MWG{ public 沙尘() : base($"severeweather/dust.html") { } }

            namespace 污染气象条件
            {
                #pragma warning disable IDE1006 // Naming Styles
                public sealed class _24小时 : MWG{ public _24小时() : base($"environment/air_pollution-24.html") { } }
                public sealed class _48小时 : MWG{ public _48小时() : base($"environment/air_pollution-48.html") { } }
                public sealed class _72小时 : MWG{ public _72小时() : base($"environment/air_pollution-72.html") { } }
                #pragma warning restore IDE1006 // Naming Styles
            }
        }
        namespace 农业气象
        {
            public sealed class 农业干旱检测 : Agro { public 农业干旱检测() : base($"disastersmonitoring/Agricultural_Drought_Monitoring.htm") { } }
            public sealed class 土壤水分10cm : Agro { public 土壤水分10cm() : base($"soil-moisture-monitoring-10cm.html") { } }
            public sealed class 土壤水分20cm : Agro { public 土壤水分20cm() : base($"soil-moisture-monitoring-20cm.html") { } }
            public sealed class 土壤水分30cm : Agro { public 土壤水分30cm() : base($"soil-moisture-monitoring-30cm.html") { } }
            public sealed class 土壤水分40cm : Agro { public 土壤水分40cm() : base($"soil-moisture-monitoring-40cm.html") { } }
            public sealed class 土壤水分50cm : Agro { public 土壤水分50cm() : base($"soil-moisture-monitoring-50cm.html") { } }
            public sealed class 大豆发育期 : Agro { public 大豆发育期() : base($"information/soybean.html") { } }
            public sealed class 油菜发育期 : Agro { public 油菜发育期() : base($"information/oilseedrape.html") { } }
            public sealed class 牧草发育期 : Agro { public 牧草发育期() : base($"information/pasture.html") { } }
            public sealed class 马铃薯发育期 : Agro { public 马铃薯发育期() : base($"information/potato.html") { } }
            public sealed class 夏玉米发育期 : Agro { public 夏玉米发育期() : base($"information/summer-corn.html") { } }
            public sealed class 冬小麦发育期 : Agro { public 冬小麦发育期() : base($"information/winter-wheat.html") { } }
            public sealed class 春小麦发育期 : Agro { public 春小麦发育期() : base($"information/spring-wheat.html") { } }
            public sealed class 棉花发育期 : Agro { public 棉花发育期() : base($"information/cotton.html") { } }
            public sealed class 花生发育期 : Agro { public 花生发育期() : base($"information/peanut.html") { } }
            public sealed class 早稻发育期 : Agro { public 早稻发育期() : base($"information/earlyrice.html") { } }
            public sealed class 晚稻发育期 : Agro { public 晚稻发育期() : base($"information/laterice.html") { } }
            public sealed class 春玉米发育期 : Agro { public 春玉米发育期() : base($"information/spring-corn.html") { } }
            public sealed class 一季稻发育期 : Agro { public 一季稻发育期() : base($"information/rice-quarter.html") { } }

        }
        namespace GRAPES全球
        {
            public sealed class 北半球500hpa : MWG { public 北半球500hpa() : base($"nwpc/grapes_gfs/nh/500hPa-hgt.htm") { } }
            public sealed class 北半球850hpa : MWG { public 北半球850hpa() : base($"nwpc/grapes_gfs/nh/850hPa-temp.htm") { } }
            public sealed class 北半球海平面气压 : MWG { public 北半球海平面气压() : base($"nwpc/grapes_gfs/nh/sea-level-pressure.htm") { } }
            public sealed class 亚欧500高800风 : MWG { public 亚欧500高800风() : base($"nwpc/grapes_gfs/ae/500hPa+hgt+850hPa+wind.htm") { } }
            public sealed class 东亚海平面气压 : MWG { public 东亚海平面气压() : base($"nwpc/grapes_gfs/ea/slp.htm") { } }
            public sealed class 东亚500高800风 : MWG { public 东亚500高800风() : base($"nwpc/grapes_gfs/ea/500hPa+hgt+850hPa+wind.htm") { } }
            public sealed class 东亚24小时降水 : MWG { public 东亚24小时降水() : base($"nwpc/grapes_gfs/ea/24hour-rain.htm") { } }
            public sealed class 东亚积累降水 : MWG { public 东亚积累降水() : base($"nwpc/grapes_gfs/ea/cumulative_precipitation.htm") { } }
            public sealed class 东亚2m温度 : MWG { public 东亚2m温度() : base($"nwpc/grapes_gfs/ea/t2m.htm") { } }
            public sealed class 东亚2m最高温度 : MWG { public 东亚2m最高温度() : base($"nwpc/grapes_gfs/ea/wind/T2mMax.htm") { } }
            public sealed class 东亚2m最低温度 : MWG { public 东亚2m最低温度() : base($"nwpc/grapes_gfs/ea/wind/T2mMin.htm") { } }
            public sealed class 东亚2m相对湿度 : MWG { public 东亚2m相对湿度() : base($"nwpc/grapes_gfs/ea/wind/RH2m.htm") { } }
            public sealed class 东亚雷达组合反射率 : MWG { public 东亚雷达组合反射率() : base($"nwpc/grapes_gfs/ea/ldzhfsl.htm") { } }
            public sealed class FY4A红外亮温 : MWG { public FY4A红外亮温() : base($"nwpc/grapes_gfs/ea/grapes_mswx/11um.htm") { } }
            public sealed class FY4A水汽亮温 : MWG { public FY4A水汽亮温() : base($"nwpc/grapes_gfs/ea/grapes_mswx/6.8um.htm") { } }
        }
        namespace GRAPES全球集合
        {
            public sealed class 累积降水24h概率预报1mm : MWG { public 累积降水24h概率预报1mm() : base($"nwpc/grapes_gfs/shuzhiyubao/GRAPESquanqiujiheyubao_new/24hleijijiangshui/gailvyubao/1mm/index.html") { } }
            public sealed class 累积降水24h概率预报10mm : MWG { public 累积降水24h概率预报10mm() : base($"nwpc/grapes_gfs/shuzhiyubao/GRAPESquanqiujiheyubao_new/24hleijijiangshui/gailvyubao/10mm/index.html") { } }
            public sealed class 累积降水24h概率预报25mm : MWG { public 累积降水24h概率预报25mm() : base($"nwpc/grapes_gfs/shuzhiyubao/GRAPESquanqiujiheyubao_new/24hleijijiangshui/gailvyubao/25mm/index.html") { } }
            public sealed class 累积降水24h概率预报50mm : MWG { public 累积降水24h概率预报50mm() : base($"nwpc/grapes_gfs/shuzhiyubao/GRAPESquanqiujiheyubao_new/24hleijijiangshui/gailvyubao/50mm/index.html") { } }
            public sealed class 累积降水24h概率预报100mm : MWG { public 累积降水24h概率预报100mm() : base($"nwpc/grapes_gfs/shuzhiyubao/GRAPESquanqiujiheyubao_new/24hleijijiangshui/gailvyubao/100mm/index.html") { } }
            public sealed class 累积降水24h极端预报指数 : MWG { public 累积降水24h极端预报指数() : base($"nwpc/grapes_gfs/shuzhiyubao/GRAPESquanqiujiheyubao_new/24hleijijiangshui/jiduanyubaozhishu/index.html") { } }
            public sealed class 累积降水24h集合平均和离散度 : MWG { public 累积降水24h集合平均和离散度() : base($"nwpc/grapes_gfs/shuzhiyubao/GRAPESquanqiujiheyubao_new/24hleijijiangshui/jihepingjunhelisandu/index.html") { } }
            public sealed class 累积降水24h邮票图 : MWG { public 累积降水24h邮票图() : base($"nwpc/grapes_gfs/shuzhiyubao/GRAPESquanqiujiheyubao_new/24hleijijiangshui/youpiaotu/index.html") { } }
            public sealed class 累积降水24h分位数最大值 : MWG { public 累积降水24h分位数最大值() : base($"nwpc/grapes_gfs/shuzhiyubao/GRAPESquanqiujiheyubao_new/zuidazhi/fenweishuchanpin/24hleijijiangshui/index.html") { } }
            public sealed class 位势高度500hPa面条图 : MWG { public 位势高度500hPa面条图() : base($"nwpc/grapes_gfs/shuzhiyubao/GRAPESquanqiujiheyubao_new/500hPaweishigaodu/miantiaotu/index.html") { } }
            public sealed class 位势高度500hPa集合平均和离散度 : MWG { public 位势高度500hPa集合平均和离散度() : base($"nwpc/grapes_gfs/shuzhiyubao/GRAPESquanqiujiheyubao_new/500hPaweishigaodu/jihepingjunhelisandu/index.html") { } }
            public sealed class 温度2米极端预报指数 : MWG { public 温度2米极端预报指数() : base($"nwpc/grapes_gfs/shuzhiyubao/GRAPESquanqiujiheyubao_new/2miwendu/jiduanyubaozhishu/index.html") { } }
            public sealed class 温度2米集合平均和离散度 : MWG { public 温度2米集合平均和离散度() : base($"nwpc/grapes_gfs/shuzhiyubao/GRAPESquanqiujiheyubao_new/2miwendu/jihepingjunhelisandu/index.html") { } }
            public sealed class 全风速10米极端预报指数 : MWG { public 全风速10米极端预报指数() : base($"nwpc/grapes_gfs/shuzhiyubao/GRAPESquanqiujiheyubao_new/10miquanfengsu/jiduanyubaozhishu/index.html") { } }
            public sealed class 全风速10米集合平均和离散度 : MWG { public 全风速10米集合平均和离散度() : base($"nwpc/grapes_gfs/shuzhiyubao/GRAPESquanqiujiheyubao_new/10miquanfengsu/jihepingjunhelisandu/index.html") { } }
            public sealed class 全风速10米概率预报10米秒 : MWG { public 全风速10米概率预报10米秒() : base($"nwpc/grapes_gfs/shuzhiyubao/GRAPESquanqiujiheyubao_new/10miquanfengsu/gailvyubao/10.8/index.html") { } }
            public sealed class 全风速10米概率预报17米秒 : MWG { public 全风速10米概率预报17米秒() : base($"nwpc/grapes_gfs/shuzhiyubao/GRAPESquanqiujiheyubao_new/10miquanfengsu/gailvyubao/17.2/index.html") { } }
        }
        namespace GRAPES台风预报
        {
            public sealed class 台风强度 : MWG { public 台风强度() : base($"nwpc/stym/cnpw-01/img1.html") { } }
            public sealed class 累计降水24h : MWG { public 累计降水24h() : base($"shuzhiyubao/GRAPEStaifengyubao/GRAPES_TYM/leijijiangshui/24xiaoshi/index.html") { } }
            public sealed class 累计大风24h : MWG { public 累计大风24h() : base($"shuzhiyubao/GRAPEStaifengyubao/GRAPES_TYM/leijidafeng/24xiaoshi/index.html") { } }
            public sealed class 台风路径 : MWG { public 台风路径() : base($"shuzhiyubao/GRAPES_TYMquyutaifengmoshi/taifenglujing/index.html") { } }
            public sealed class 风场10m : MWG { public 风场10m() : base($"shuzhiyubao/GRAPES_TYMquyutaifengmoshi/10mfengchang/index.html") { } }
            public sealed class 组合雷达反射率 : MWG { public 组合雷达反射率() : base($"shuzhiyubao/GRAPES_TYMquyutaifengmoshi/zuheleidafanshelv/index.html") { } }
            public sealed class FY4A云图模拟红外亮温 : MWG { public FY4A云图模拟红外亮温() : base($"shuzhiyubao/GRAPES_TYMquyutaifengmoshi/FY2Gweixingyuntumoni/11umliangwen(hongwai)/index.html") { } }
            public sealed class FY4A云图模拟水汽亮温 : MWG { public FY4A云图模拟水汽亮温() : base($"shuzhiyubao/GRAPES_TYMquyutaifengmoshi/FY2Gweixingyuntumoni/6.8/index.html") { } }
        }
        namespace 海浪模式
        {
            public sealed class 全球浪高浪向叠加图 : MWG { public 全球浪高浪向叠加图() : base($"nwp/ww3/globe/index.html") { } }
            public sealed class 大西洋浪高浪向叠加图 : MWG { public 大西洋浪高浪向叠加图() : base($"nwp/ww3/atlantic/waveheight-wavespeed-meandirection.html") { } }
            public sealed class 印度洋浪高浪向叠加图 : MWG { public 印度洋浪高浪向叠加图() : base($"nwp/ww3/indianocean/waveheight-wavespeed-meandirection_0000.html") { } }
            public sealed class 太平洋浪高浪向叠加图 : MWG { public 太平洋浪高浪向叠加图() : base($"nwp/ww3/pacific/waveheight-wavespeed-meandirection.html") { } }
        }
    }
}

