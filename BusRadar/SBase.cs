using Meow.Util.Network.Http;

namespace BusRadar
{
    public enum SType
    {
        /// <summary>
        /// 全部
        /// </summary>
        a,
        /// <summary>
        /// 车辆
        /// </summary>
        v,
        /// <summary>
        /// 线路
        /// </summary>
        l,
        /// <summary>
        /// 公司
        /// </summary>
        c,
        /// <summary>
        /// 车型
        /// </summary>
        m,
    }
    
    public class SBase
    {
        public static HttpClient Client { get; } = new();
        public static byte[] UrlGet(string url) => Client.MBinary(url).GetAwaiter().GetResult();
        public static byte[] Search(string name, Region region, SType filter = SType.a) => UrlGet($"https://buspedia.top/search?name={name}&filter={filter}&region={region})");


    }
}