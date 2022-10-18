namespace Meow.TrainRadar
{
    /// <summary>
    /// 火车线路类
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

            /// <summary>
            /// REP节点
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
            }
        }
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
        /// 铁路局信息
        /// </summary>
        public BureauInfo Bureau;
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
        /// 运营人
        /// </summary>
        public string Operators;
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
            return $"{LocalName} {(LocalName != LocalizedName ? $"/{LocalizedName}" : "")}\n" +
                $"[{TeleCode}]({(string.IsNullOrWhiteSpace(PinyinCode) ? "" : $" {PinyinCode}")}{(string.IsNullOrWhiteSpace(SedName) ? "" : $" {SedName}")})\n" +
                $"[{Bureau.Name}{(string.IsNullOrWhiteSpace(Operators) ? "" : $" |{Operators}")}]\n" +
                $"[{Id}::{(Status == "O"?"运营":"未运营")}]\n" +
                $"[{ServiceClass}]\n" +
                $"[{Country} {Location}]|[{X}:{Y}]\n";
        }
    }

   


}
