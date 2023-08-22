namespace Meow.Util.Math.Graph
{
    public struct Result<T>
    {
        public T Value { get; init; }
        public Result(T value) : this() => Value = value;
        public DateTime TaskStart { get; set; }
        public DateTime TaskEnd { get; set; }
        public double InnerCircleTime { get; set; }
        public double OuterCircleTime { get; set; }
        public double MemoryStart { get; set; }
        public double MemoryEnd { get; set; }
        public readonly double COE { get => System.Math.Log(InnerCircleTime, OuterCircleTime); }
        public readonly TimeSpan TimeDiff { get => TaskEnd - TaskStart; }
        public override readonly string ToString() => $"TotalTime:{TimeDiff.TotalMilliseconds}ms\n" +
                $"Estimate MemoryUsage:{System.Math.Round(MemoryEnd - MemoryStart,5)} MB\n" +
                $"Estimate σ(n^{COE}) = [ln{OuterCircleTime}/ln{InnerCircleTime}]\n" +
                $"Estimate O(n^{System.Math.Floor(COE)}) ~> Ω(n^{System.Math.Ceiling(COE)})";
    }
}
