//using Meow.Math.Graph.Interface;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics.CodeAnalysis;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Meow.Math.Graph.Class
//{
//    /// <summary>
//    /// 图节点基类
//    /// </summary>
//    /// <typeparam name="T">节点类型</typeparam>
//    public struct MapT : IMapT where T : IEquatable<T>
//    {
//        /// <summary>
//        /// 初始化节点
//        /// </summary>
//        /// <param name="Id">节点识别码</param>
//        public MapNode(T Id) => this.Id = Id;
//        public IDictionary<T, double> LinkSet { get; private set; } = new Dictionary<T, double>();
//        public T Id { get; set; }
//        public readonly bool IsConnectTo(T Id) => LinkSet.ContainsKey(Id);
//        public readonly double GetWeight(T id) => LinkSet[id];
//        public readonly bool TryGetWeight(T id, out double weight) => LinkSet.TryGetValue(id, out weight);
//        public readonly bool LinkTo(T nodename, double weight = 1) => LinkSet.TryAdd(nodename, weight);
//        public readonly bool RemoveLink(T nodename) => LinkSet.Remove(nodename);
//        public readonly T this[int pos] => LinkSet.ToArray()[pos].Key;
//        public readonly double this[T id] => LinkSet[id];
//        public readonly bool Any() => LinkSet.Any();
//        public readonly double Count => LinkSet.Count;
//        public readonly bool Equals(IMapT? other) => other?.Id.Equals(Id) ?? false;
//        public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is MapT other && Equals(other);
//        public override readonly int GetHashCode() => Id.GetHashCode();
//        public override readonly string ToString() => $"[Node:{Id}]";
//        public static bool operator ==(MapT left, MapT right) => left.Equals(right);
//        public static bool operator !=(MapT left, MapT right) => !left.Equals(right);
//        public readonly IEnumerator<KeyValuePair<T, double>> GetEnumerator() => LinkSet.AsEnumerable().GetEnumerator();
//    }
//}
