using System;

namespace Meow.Util
{
    /// <summary>
    /// 时间函数库
    /// </summary>
    public class TimeX
    {
        /// <summary>
        /// TimeStamp起始点
        /// </summary>
        public static readonly DateTime UnixTimeStampStart = new (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// 转换时间戳到时间类
        /// </summary>
        public class TimeStampX
        {
            /// <summary>
            /// 秒制时间戳转换时间类
            /// </summary>
            /// <param name="sec">秒制TimeStamp</param>
            /// <returns></returns>
            public static DateTime Second(long sec) =>
                UnixTimeStampStart.AddSeconds(sec).ToLocalTime();
            /// <summary>
            /// 毫秒制时间戳转换时间类
            /// </summary>
            /// <param name="millisec">毫秒制时间戳</param>
            /// <returns></returns>
            public static DateTime MilliSecond(long millisec) =>
                UnixTimeStampStart.AddMilliseconds(millisec).ToLocalTime();
            /// <summary>
            /// Ticks转换时间类
            /// </summary>
            /// <param name="value">Ticks值</param>
            /// <returns></returns>
            public static DateTime Ticks(long value) =>
                TimeZoneInfo.ConvertTimeFromUtc(new DateTime(value),
                TimeZoneInfo.Local);
        }
        /// <summary>
        /// 转换时间类到时间戳
        /// </summary>
        public class DateTimeX
        {
            private readonly DateTime dateTime;
            /// <summary>
            /// 新建一个时间类辅助类
            /// </summary>
            /// <param name="dateTime">DateTime对象</param>
            public DateTimeX(DateTime dateTime)
            {
                this.dateTime = dateTime;
            }

            /// <summary>
            /// 时间类转换成秒制时间戳
            /// </summary>
            /// <returns></returns>
            public long ToSecTimeStamp() =>
                (long)(dateTime.ToUniversalTime() - UnixTimeStampStart).TotalSeconds;
            /// <summary>
            /// 时间类转换成毫秒制时间戳
            /// </summary>
            /// <returns></returns>
            public long ToMiSecTimeStamp() =>
                (long)(dateTime.ToUniversalTime() - UnixTimeStampStart).TotalMilliseconds;
            /// <summary>
            /// 时间类转换成固定字符串格式
            /// </summary>
            /// <param name="convertor">要转换的默认格式</param>
            /// <returns></returns>
            public string ToString(string convertor = "yyyy-MM-dd_hh-mm-ss") =>
                dateTime.ToString(convertor);
        }
    }
}
