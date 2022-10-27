using System.Text;

namespace Meow.TrainRadar
{
    /// <summary>
    /// 铁路信息
    /// </summary>
    public struct RailInfo
    {
        /// <summary>
        /// 线路名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 线路数量
        /// </summary>
        public int Linenum;
        /// <summary>
        /// 设计时速
        /// </summary>
        public string? DesignSpeed;
        /// <summary>
        /// 电器化情况
        /// </summary>
        public string Elec;
        /// <summary>
        /// 列车类型
        /// </summary>
        public RailServiceType RailService;
        /// <summary>
        /// 铁路类型
        /// </summary>
        public RailSpdType RailType;
        /// <summary>
        /// 线路图
        /// </summary>
        public RailDiagram Diagram;
        /// <summary>
        /// 其他备注
        /// </summary>
        public string[] Notes;
        /// <summary>
        /// 参考文献
        /// </summary>
        public string[] Reference;

        /// <summary>
        /// 扩展ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var n = new StringBuilder();
            if(Reference.Length > 0)
            {
                n.Append("选编:");
                foreach (var i in Reference)
                {
                    n.Append($" {i}");
                }
                n.Append(" | ");
            }
            if(Notes.Length > 0)
            {
                n.Append("备注:");
                foreach (var i in Notes)
                {
                    n.Append($" {i}");
                }
            }
            return $"[{Name}]\n" +
                $"线路数量: {Linenum}\n" +
                $"设计时速: {DesignSpeed}\n" +
                $"电气化: {Elec}\n" +
                $"铁路服务类型: {RailService}\n" +
                $"铁路类型: {RailType}\n" +
                $"{n}";
        }
    }
    /// <summary>
    /// 经停站类
    /// </summary>
    public struct RouteStops
    {
        /// <summary>
        /// 站ID
        /// </summary>
        public string Id;
        /// <summary>
        /// 站名
        /// </summary>
        public string Name;
        /// <summary>
        /// 到站时间
        /// </summary>
        public string Starts;
        /// <summary>
        /// 发车时间
        /// </summary>
        public string Ends;
        /// <summary>
        /// 其他事项
        /// </summary>
        public string Notes;
        /// <summary>
        /// 其他事项2
        /// </summary>
        public string Notes2;
    }
    /// <summary>
    /// 路线图
    /// </summary>
    public struct RailDiagram
    {
        /// <summary>
        /// 模板类型
        /// </summary>
        public string Template;
        /// <summary>
        /// 逻辑线路
        /// </summary>
        public REndPoint[] Records;
    }
    /// <summary>
    /// 轨道节点
    /// </summary>
    public struct REndPoint
    {
        /// <summary>
        /// 绘图类型
        /// </summary>
        public string SType;
        /// <summary>
        /// 里程
        /// </summary>
        public int? Miles;
        /// <summary>
        /// 线路类型
        /// </summary>
        public string RailType;
        /// <summary>
        /// 内部结构
        /// </summary>
        public REndPointInner[] Points;
    }
    /// <summary>
    /// REP内部结构
    /// </summary>
    public struct REndPointInner
    {
        /// <summary>
        /// 当前节点类型
        /// </summary>
        public StationType Type;
        /// <summary>
        /// Id
        /// </summary>
        public int Id;
        /// <summary>
        /// 线路名称
        /// </summary>
        public string Name;
    }
    /// <summary>
    /// 铁路管理局信息
    /// </summary>
    public struct BureauInfo
    {
        /// <summary>
        /// 铁路局信息
        /// </summary>
        public string Name;
        /// <summary>
        /// 铁路局Logo
        /// </summary>
        public string Logo;
    }
    /// <summary>
    /// 火车站信息
    /// </summary>
    public struct StationInfo
    {
        /// <summary>
        /// 电报码
        /// </summary>
        public string TeleCode;
        /// <summary>
        /// 拼音码
        /// </summary>
        public string PinyinCode;
        /// <summary>
        /// 类型*
        /// </summary>
        public StationType Type;
        /// <summary>
        /// 运营人
        /// </summary>
        public BureauInfo Operators;
        /// <summary>
        /// 识别码
        /// </summary>
        public string Id;
        /// <summary>
        /// 位置地名*
        /// </summary>
        public string LocalName;
        /// <summary>
        /// 位置地名*
        /// </summary>
        public string LocalizedName;
        /// <summary>
        /// 位置地名*
        /// </summary>
        public string SedName;
        /// <summary>
        /// 状态*
        /// </summary>
        public string Status;
        /// <summary>
        /// 国家号
        /// </summary>
        public string Country;
        /// <summary>
        /// *
        /// </summary>
        public string FirstScale;
        /// <summary>
        /// 服务级别*
        /// </summary>
        public ServiceType[] ServiceClass;
        /// <summary>
        /// *
        /// </summary>
        public int X;
        /// <summary>
        /// *
        /// </summary>
        public int Y;
        /// <summary>
        /// 具体地址
        /// </summary>
        public string Location;

        /// <summary>
        /// 重载的字符串显示
        /// <para>
        /// {LocalName}<br/>
        /// [{TeleCode}] {PinyinCode} {SedName}")}<br/>
        /// [{Bureau.Name} |{Operators}]<br/>
        /// [{Id}::{Status}]" <br/>
        /// [{ServiceClass}]" <br/>
        /// [{Country} {Location}]|[{X}:{Y}]"<br/>
        /// </para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach(var i in ServiceClass)
            {
                sb.Append($"{(i==ServiceType.P_Plus?"P+":i)} ");
            }
            return $"- {LocalName} {(LocalName != LocalizedName ? $"/{LocalizedName}" : "")} - \n" +
                $"[{Id}::{TeleCode}]({(string.IsNullOrWhiteSpace(PinyinCode) ? "" : $"{PinyinCode}")}{(string.IsNullOrWhiteSpace(SedName) ? "" : $" | {SedName}")})\n" +
                $"管局信息:[{Operators.Name}]\n" +
                $"运营状态:[{(Status == "O"?"运营":"未运营")}]\n" +
                $"服务类型[:{sb}]\n" +
                $"车站地址:[{Country} {Location}]\n";
        }
    }
    /// <summary>
    /// 运行路线信息
    /// </summary>
    public struct RouteInfo
    {
        /// <summary>
        /// 火车ID
        /// </summary>
        public string TrainId;
        /// <summary>
        /// 车次
        /// </summary>
        public string OperationId;
        /// <summary>
        /// 车类型
        /// </summary>
        public ServiceType ServiceId;
        /// <summary>
        /// 经停站列表
        /// </summary>
        public RouteStops[] Stops;
        /// <summary>
        /// 运营商
        /// </summary>
        public string[] Operators;
        /// <summary>
        /// 备注
        /// </summary>
        public string Note;
        /// <summary>
        /// 班次
        /// </summary>
        public string Frequency;
        /// <summary>
        /// 时间表格式类型
        /// </summary>
        public string TimeTableFormat;
        /// <summary>
        /// 分段运营 *
        /// </summary>
        public string OpSegment;
        /// <summary>
        /// *
        /// </summary>
        public string RouteType;

        /// <summary>
        /// 重载的字符串显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            for (int i = 0; i < Stops.Length; i++)
            {
                RouteStops v = Stops[i];
                sb.AppendLine(i != 0 ? $"|\n|-[到:{v.Starts}]" : "");
                sb.AppendLine($"|- ({v.Name}) ");
                sb.AppendLine(i != (Stops.Length-1) ? $"|-[开:{v.Ends}]\n|": "");
            }
            return $"{OperationId}次 :: [ {ServiceId} | {RouteType} ]\n{sb}";
        }
    }

}
