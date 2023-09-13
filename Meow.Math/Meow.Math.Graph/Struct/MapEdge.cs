//using Meow.Math.Graph.Interface;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Meow.Math.Graph.Class
//{
//    /// <summary>
//    /// 实例边
//    /// </summary>
//    /// <typeparam name="T">图类型</typeparam>
//    public struct MapEdge<T> : IMapEdge<T> where T : IEquatable<T>
//    {
//        public T Start { get; set; }
//        public T End { get; set; }
//        public double Weight { get; set; }

//        /// <summary>
//        /// 实例化边
//        /// </summary>
//        /// <param name="start">边起点</param>
//        /// <param name="end">边终点</param>
//        /// <param name="weight">边权重(默认为1)</param>
//        public MapEdge(T start, T end, double weight = 1)
//        {
//            Start = start;
//            End = end;
//            Weight = weight;
//        }

//        public readonly bool Equals(IMapEdge<T>? other) => other is not null && Start.Equals(other.Start) && End.Equals(other.End);
//        public override readonly bool Equals(object? obj) => obj is MapEdge<T> edge && Equals(edge);
//        public static bool operator ==(MapEdge<T> left, MapEdge<T> right) => left.Equals(right);
//        public static bool operator !=(MapEdge<T> left, MapEdge<T> right) => !(left == right);
//        public override readonly int GetHashCode() => Start.GetHashCode() ^ End.GetHashCode();
//        public override readonly string ToString() => $"{Start}-({Weight})->{End}";
//    }
//}
