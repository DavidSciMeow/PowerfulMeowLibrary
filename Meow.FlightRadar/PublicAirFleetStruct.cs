namespace Meow.FlightRadar
{
    public struct FleetInfo
    {
        /// <summary>
        /// 航司旗下的飞行器
        /// </summary>
        public int NumberOfFlight;
        /// <summary>
        /// 航司前缀
        /// </summary>
        public string Predix;
        /// <summary>
        /// 航司名称
        /// </summary>
        public string AirlineOperator;
        /// <summary>
        /// 重写的字符串输出方法
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{NumberOfFlight,-4} [{Predix}] {AirlineOperator}";
    }
}
