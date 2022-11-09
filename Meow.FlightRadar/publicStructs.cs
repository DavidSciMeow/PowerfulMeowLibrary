using System.Text;

namespace Meow.FlightRadar
{
    /// <summary>
    /// 机场固定运营基地
    /// </summary>
    public struct FBOModel
    {
        /// <summary>
        /// 地点
        /// </summary>
        public string Location;
        /// <summary>
        /// 坐标X
        /// </summary>
        public double CoordinateX;
        /// <summary>
        /// 坐标Y
        /// </summary>
        public double CoordinateY;
        /// <summary>
        /// 相对抬升 (英尺)
        /// </summary>
        public double ElevationFeet;
        /// <summary>
        /// 相对抬升 (米)
        /// </summary>
        public double ElevationMeter;
        /// <summary>
        /// 是否含有塔台
        /// </summary>
        public bool TowerExist;
        /// <summary>
        /// 驻港飞机<br/>
        /// (类型,数)
        /// </summary>
        public (int SingleEngine, int MultiEngine, int Jet, int Heli, int Glider, int Military) BasedAirCraft;
        /// <summary>
        /// 油量
        /// </summary>
        public string FuelLL;
        /// <summary>
        /// 跑道情况<br/>
        /// (号/长宽/跑道面状态/灯光)
        /// </summary>
        public (string Rwynum, string RwyLenth, string RwyCond, string RwyLight)[] RunwayCondition;
        /// <summary>
        /// 管制频率<br/>
        /// (管制名称,频率)
        /// </summary>
        public (string RadioName, string Freq)[] RadioFreqList;
        /// <summary>
        /// 重写的字符串表示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine($"{Location} [{CoordinateX},{CoordinateY} @{ElevationMeter}m]");
            sb.AppendLine($"--:Based AirCraft:--");
            sb.AppendLine($"SingleEngine:{BasedAirCraft.SingleEngine}");
            sb.AppendLine($"MultiEngine:{BasedAirCraft.MultiEngine}");
            sb.AppendLine($"Jet:{BasedAirCraft.Jet}");
            sb.AppendLine($"Heli:{BasedAirCraft.Heli}");
            sb.AppendLine($"Glider:{BasedAirCraft.Glider}");
            sb.AppendLine($"Military:{BasedAirCraft.Military}");
            sb.AppendLine($"--:RunWay Condition:--");
            sb.AppendLine($"RWYNUM\tRWYLENGTH\tRWYCONDITION\tRWYLIGHT");
            foreach(var (Rwynum, RwyLenth, RwyCond, RwyLight) in RunwayCondition)
            {
                sb.AppendLine($"{Rwynum}\t{RwyLenth}\t{RwyCond}\t{RwyLight}");
            }
            sb.AppendLine($"--:Tower Radio Freq.:--");
            sb.AppendLine($"TYPE\tFREQ");
            foreach (var (name,freq) in RadioFreqList)
            {
                sb.AppendLine($"{name}\t{freq}");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 重写的字符串表示(美式计量)
        /// </summary>
        /// <returns></returns>
        public string ToNotStandardString()
        {
            StringBuilder sb = new();
            sb.AppendLine($"{Location} [{CoordinateX},{CoordinateY} @{ElevationFeet}feet]");
            sb.AppendLine($"--:Based AirCraft:--");
            sb.AppendLine($"SingleEngine:{BasedAirCraft.SingleEngine}");
            sb.AppendLine($"MultiEngine:{BasedAirCraft.MultiEngine}");
            sb.AppendLine($"Jet:{BasedAirCraft.Jet}");
            sb.AppendLine($"Heli:{BasedAirCraft.Heli}");
            sb.AppendLine($"Glider:{BasedAirCraft.Glider}");
            sb.AppendLine($"Military:{BasedAirCraft.Military}");
            sb.AppendLine($"--:RunWay Condition:--");
            sb.AppendLine($"RWYNUM\tRWYLENGTH\tRWYCONDITION\tRWYLIGHT");
            foreach (var (Rwynum, RwyLenth, RwyCond, RwyLight) in RunwayCondition)
            {
                sb.AppendLine($"{Rwynum}\t{RwyLenth}\t{RwyCond}\t{RwyLight}");
            }
            sb.AppendLine($"--:Tower Radio Freq.:--");
            sb.AppendLine($"TYPE\tFREQ");
            foreach (var (name, freq) in RadioFreqList)
            {
                sb.AppendLine($"{name}\t{freq}");
            }
            return sb.ToString();
        }
    }


    /// <summary>
    /// 航班模型
    /// </summary>
    public struct BoardModel
    {
        /// <summary>
        /// 进港类型
        /// </summary>
        public BoardType Type;
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
        /// <summary>
        /// 重写的字符串表示
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"[ {Identity,-8} /{AircraftType,5} ] {Dept.Time,-22} -> {Arrv.Time,-22} " +
            $"{{{((Type==BoardType.arrivals || Type == BoardType.enroute)?"->":"<-")} {Place} }}";
    }

    /// <summary>
    /// 天气终端模型
    /// </summary>
    public struct WeatherModel
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Time;
        /// <summary>
        /// 飞行规则
        /// </summary>
        public FlightRule FlightRules;
        /// <summary>
        /// 天气类型
        /// </summary>
        public string[] WeaType;
        /// <summary>
        /// AGL高度
        /// </summary>
        public string HeightAGL;
        /// <summary>
        /// 能见度
        /// </summary>
        public string Visibility;
        /// <summary>
        /// 相对湿度
        /// </summary>
        public int RelHumid;
        /// <summary>
        /// 风向 [0静风] [1-360] [361无稳定风] 
        /// <para>[0 Calm] [1-360 deg] [361 Variable] </para>
        /// </summary>
        public int WindDir;

        /// <summary>
        /// 风速 M/s
        /// </summary>
        public double SpeedMps;
        /// <summary>
        /// 风速 kt
        /// </summary>
        public double SpeedKt;
        /// <summary>
        /// 摄氏温度
        /// </summary>
        public int TempDegC;
        /// <summary>
        /// 华氏温度
        /// </summary>
        public int TempDegF;
        /// <summary>
        /// 摄氏露点
        /// </summary>
        public int DewPointC;
        /// <summary>
        /// 华氏露点
        /// </summary>
        public int DewPointF;
        /// <summary>
        /// 气压(修正海压百帕斯卡)
        /// </summary>
        public double PressureHpa;
        /// <summary>
        /// 气压(修正海压英尺汞柱)
        /// </summary>
        public double PressureInHg;

        /// <summary>
        /// 密度高度
        /// </summary>
        public int DensityAltitude;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks;
        /// <summary>
        /// 重写的字符串表示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            foreach(var i in WeaType)
            {
                sb.Append($"{i} ");
            }
            return $"{Time:MM-dd HH:mm} [{FlightRules,-6}] " +
                    $"[{TempDegC,3}°C/{DewPointC,3}°C] [{(WindDir == 0 ? $"    CALM    ]" : $"{(WindDir == 361 ? "VARI" : WindDir),4}°@{SpeedMps,3}M/s]")} " +
                    $"[{RelHumid,3}% @ {PressureHpa,5}Hpa] DA:{DensityAltitude}ft HAGL:{(HeightAGL == "Clouds and visibility are OK." ? "CAVOK" : HeightAGL)}" +
                    $"{(string.IsNullOrWhiteSpace(sb.ToString()) ? "[N]" : $" [{sb}] ")} " +
                    $"{(string.IsNullOrWhiteSpace(Visibility) ? "[N]" : $" {Visibility} ")} " +
                    $"{(string.IsNullOrWhiteSpace(Remarks) ? "[N]" : Remarks)}";
        }
        /// <summary>
        /// 重写的字符串表示(美式计量)
        /// </summary>
        /// <returns></returns>
        public string ToNotStandardString()
        {
            StringBuilder sb = new();
            foreach (var i in WeaType)
            {
                sb.Append($"{i} ");
            }
            return $"{Time:MM-dd HH:mm} [{FlightRules,-6}] " +
                    $"[{TempDegF,3}°F/{DewPointF,3}°F] [{(WindDir == 0 ? $"    CALM    ]" : $"{(WindDir == 361 ? "VARI" : WindDir),4}°@{SpeedKt,3}kt]")} " +
                    $"[{RelHumid,3}% @ {PressureHpa,5}Hpa] DA:{DensityAltitude}ft HAGL:{(HeightAGL == "Clouds and visibility are OK."?"CAVOK":HeightAGL)}" +
                    $"{(string.IsNullOrWhiteSpace(sb.ToString()) ? "[N]" : $" [{sb}] ")} " +
                    $"{(string.IsNullOrWhiteSpace(Visibility) ? "[N]" : $" {Visibility} ")} " +
                    $"{(string.IsNullOrWhiteSpace(Remarks) ? "[N]" : Remarks)}";
        }
    }
}
