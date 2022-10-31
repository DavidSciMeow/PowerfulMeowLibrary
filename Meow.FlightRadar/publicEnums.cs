using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.FlightRadar
{
    /// <summary>
    /// 语言首选项
    /// </summary>
    public enum Langpref
    {
        /// <summary>
        /// 中文
        /// </summary>
        zh_CN,
        /// <summary>
        /// 英文
        /// </summary>
        en_US,
    }
    /// <summary>
    /// 搜索类型
    /// </summary>
    public enum SerachType
    {
        /// <summary>
        /// 机场
        /// </summary>
        airport,
        /// <summary>
        /// 航班
        /// </summary>
        flight,
        /// <summary>
        /// 航线
        /// </summary>
        registration,
    }
    /// <summary>
    /// 机场资源类型
    /// </summary>
    public enum AirportResourceType
    {
        /// <summary>
        /// 天气
        /// </summary>
        weather,
    }
    /// <summary>
    /// 机场班次类型
    /// </summary>
    public enum BoardType
    {
        /// <summary>
        /// 进港
        /// </summary>
        arrivals,
        /// <summary>
        /// 离港
        /// </summary>
        departures,
        /// <summary>
        /// 正在/计划飞往
        /// </summary>
        enroute,
        /// <summary>
        /// 计划离港
        /// </summary>
        scheduled,
    }
}
