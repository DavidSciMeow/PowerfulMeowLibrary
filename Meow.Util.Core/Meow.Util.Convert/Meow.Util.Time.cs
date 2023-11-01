using System;

namespace Meow.Util.Convert
{
    /// <summary>
    /// 时间函数库
    /// </summary>
    public static class Time
    {
        /// <summary>
        /// TimeStamp起始点
        /// </summary>
        private static readonly DateTime UnixTimeStampStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// 秒制时间戳转换时间类
        /// </summary>
        /// <param name="sec">秒制TimeStamp</param>
        /// <returns></returns>
        public static DateTime Second(this long sec) =>
            UnixTimeStampStart.AddSeconds(sec).ToLocalTime();
        /// <summary>
        /// 毫秒制时间戳转换时间类
        /// </summary>
        /// <param name="millisec">毫秒制时间戳</param>
        /// <returns></returns>
        public static DateTime MilliSecond(this long millisec) =>
            UnixTimeStampStart.AddMilliseconds(millisec).ToLocalTime();
        /// <summary>
        /// Ticks转换时间类
        /// </summary>
        /// <param name="value">Ticks值</param>
        /// <returns></returns>
        public static DateTime Ticks(this long value) =>
            TimeZoneInfo.ConvertTimeFromUtc(new DateTime(value),
            TimeZoneInfo.Local);
        /// <summary>
        /// 时间类转换成秒制时间戳
        /// </summary>
        /// <param name="dateTime">DateTime对象</param>
        /// <returns></returns>
        public static long ToSecTimeStamp(this DateTime dateTime) =>
            (long)(dateTime.ToUniversalTime() - UnixTimeStampStart).TotalSeconds;
        /// <summary>
        /// 时间类转换成毫秒制时间戳
        /// </summary>
        /// <param name="dateTime">DateTime对象</param>
        /// <returns></returns>
        public static long ToMiSecTimeStamp(this DateTime dateTime) =>
            (long)(dateTime.ToUniversalTime() - UnixTimeStampStart).TotalMilliseconds;
    }
}
