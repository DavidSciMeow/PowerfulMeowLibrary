namespace Meow.TrainRadar
{
    /// <summary>
    /// 铁路速度类型
    /// </summary>
    public enum RailSpdType
    {
        /// <summary>
        /// 异常代码专用
        /// </summary>
        N,
        /// <summary>
        /// 普速
        /// </summary>
        CONV,
        /// <summary>
        /// 高速
        /// </summary>
        HSR,
        /// <summary>
        /// 快速
        /// </summary>
        RR,
        /// <summary>
        /// ?
        /// </summary>
        LINK,
    }
    /// <summary>
    /// 列车类型
    /// </summary>
    public enum RailServiceType
    {
        /// <summary>
        /// 异常代码专用
        /// </summary>
        N,
        /// <summary>
        /// 客运
        /// </summary>
        P,
        /// <summary>
        /// 货运
        /// </summary>
        F,
        /// <summary>
        /// 两运
        /// </summary>
        PF,
    }
    /// <summary>
    /// 车站类型
    /// </summary>
    public enum StationType
    {
        /// <summary>
        /// 车站
        /// </summary>
        S,
        /// <summary>
        /// 线路所
        /// </summary>
        X,
        /// <summary>
        /// 线路
        /// </summary>
        R,
        /// <summary>
        /// 桥
        /// </summary>
        P,
    }
    /// <summary>
    /// 服务类型
    /// </summary>
    public enum ServiceType
    {
        /// <summary>
        /// 其他
        /// </summary>
        Others=1,
        /// <summary>
        /// 高铁
        /// </summary>
        G=2,
        /// <summary>
        /// 动车
        /// </summary>
        D=3,
        /// <summary>
        /// 城际
        /// </summary>
        C=4,
        /// <summary>
        /// 直达特快
        /// </summary>
        Z=5,
        /// <summary>
        /// 特快
        /// </summary>
        T=6,
        /// <summary>
        /// 快速
        /// </summary>
        K=7,
        /// <summary>
        /// 普快
        /// </summary>
        P_Plus=8,
        /// <summary>
        /// 普客
        /// </summary>
        P=9,
        /// <summary>
        /// 国际联运
        /// </summary>
        InternationalTrans=13,
        /// <summary>
        /// 港铁
        /// </summary>
        HKRail=15,
    }
    /// <summary>
    /// 语言枚举
    /// </summary>
    public enum LangPref
    {
        /// <summary>
        /// 中文中国
        /// </summary>
        zhcn,
        /// <summary>
        /// 日语日本
        /// </summary>
        jajp,
        /// <summary>
        /// 英语美国
        /// </summary>
        enus,
    }
    /// <summary>
    /// 搜索类型
    /// </summary>
    public enum SearchType
    {
        /// <summary>
        /// 车站
        /// </summary>
        STATION,
        /// <summary>
        /// 线路
        /// </summary>
        RAIL,
        /// <summary>
        /// 列车
        /// </summary>
        TRAIN,
    }
}
