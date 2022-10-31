using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.FlightRadar
{

    /// <summary>
    /// 航班模型
    /// </summary>
    public struct BoardModel
    {
        /// <summary>
        /// 呼号
        /// </summary>
        public string Identity;
        /// <summary>
        /// 机型
        /// </summary>
        public string AircraftType;
        /// <summary>
        /// 计划地点
        /// </summary>
        public string Place;
        /// <summary>
        /// 离港时间
        /// </summary>
        public (DateTime? Time, string Zone) Dept;
        /// <summary>
        /// 到港时间
        /// </summary>
        public (DateTime? Time, string Zone) Arrv;
    }

    /// <summary>
    /// 天气终端模型
    /// </summary>
    public struct WeatherModel
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string Date;
        /// <summary>
        /// 时间
        /// </summary>
        public string Time;
        /// <summary>
        /// 时区
        /// </summary>
        public string Zone;
        /// <summary>
        /// 飞行规则
        /// </summary>
        public string FlightRules;
        /// <summary>
        /// 风向
        /// </summary>
        public string WindDir;
        /// <summary>
        /// 风速
        /// </summary>
        public string Speed;
        /// <summary>
        /// 天气类型
        /// </summary>
        public string WeaType;
        /// <summary>
        /// AGL高度
        /// </summary>
        public string HeightAGL;
        /// <summary>
        /// 能见度
        /// </summary>
        public string Visibility;
        /// <summary>
        /// 摄氏温度
        /// </summary>
        public string TempDegC;
        /// <summary>
        /// 华氏温度
        /// </summary>
        public string TempDegF;
        /// <summary>
        /// 摄氏露点
        /// </summary>
        public string DewPointC;
        /// <summary>
        /// 华氏露点
        /// </summary>
        public string DewPointF;
        /// <summary>
        /// 相对湿度
        /// </summary>
        public string RelHumid;
        /// <summary>
        /// 气压(修正海压百帕斯卡)
        /// </summary>
        public string Pressure;
        /// <summary>
        /// 密度高度
        /// </summary>
        public string DensityAltitude;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks;
    }
}
