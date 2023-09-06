using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Math.Graph.Struct
{
    /// <summary>
    /// 节点路径结构
    /// </summary>
    /// <typeparam name="T">节点类型 (必须可比较是否相同)</typeparam>
    public readonly struct MapNodePath<T> : IEnumerable<KeyValuePair<T, double>>, IEquatable<MapNodePath<T>>, IComparable<MapNodePath<T>> where T : IEquatable<T>
    {
        /// <summary>
        /// 实例化路径结构(初始)
        /// </summary>
        public MapNodePath(T start) { Nodes.Add(new(start, 0)); }
        /// <summary>
        /// 节点列表
        /// </summary>
        public List<KeyValuePair<T, double>> Nodes { get; init; } = new();
        /// <summary>
        /// 节点记录表
        /// </summary>
        public HashSet<T> Sets { get; init; } = new();

        /// <summary>
        /// 路径总权值
        /// </summary>
        public readonly double TotalWeight
        {
            get
            {
                double k = 0;
                foreach (var i in Nodes) k += i.Value;
                return k;
            }
        }
        /// <summary>
        /// 路径内节点总数
        /// </summary>
        public readonly int Count => Nodes.Count;
        /// <summary>
        /// 开始节点
        /// </summary>
        public readonly T Start => Nodes[0].Key;
        /// <summary>
        /// 结束节点
        /// </summary>
        public readonly T End => Nodes[^1].Key;
        /// <summary>
        /// 是否未连接
        /// </summary>
        public readonly bool IsNotConnected => TotalWeight == 0;

        /// <summary>
        /// 找到元素node在路径中的位置
        /// </summary>
        /// <param name="node">要寻找的元素</param>
        /// <returns>位置序号</returns>
        public readonly int FindPos(T node)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i].Key.Equals(node))
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// 移除从start到end的路径元素
        /// </summary>
        /// <param name="start">起始位置</param>
        /// <param name="end">结束位置</param>
        public readonly void RemoveRange(int start, int end) => Nodes.RemoveRange(start, end - start);
        /// <summary>
        /// 移除从start到end的路径元素
        /// </summary>
        /// <param name="start">起始位置</param>
        /// <param name="end">结束位置</param>
        public readonly void RemoveToEnd(int start) => Nodes.RemoveRange(start, Nodes.Count - start);
        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="node">节点</param>
        public readonly void Add(T node, double weight)
        {
            if(Sets.Add(node)) Nodes.Add(new(node, weight));
        }
        /// <summary>
        /// 移除节点
        /// </summary>
        /// <param name="node">节点</param>
        /// <returns>是否移除成功</returns>
        public readonly void Remove(T node)
        {
            Sets.Remove(node);
            for (int i = 0; i < Nodes.Count; i++)
                if (node.Equals(Nodes[i])) Nodes.RemoveAt(i);
        }
        /// <summary>
        /// 移除节点
        /// </summary>
        /// <param name="nodepos">节点位置</param>
        /// <returns>是否移除成功</returns>
        public readonly void Remove(int nodepos)
        {
            Sets.Remove(Nodes[nodepos].Key);
            Nodes.RemoveAt(nodepos);
        }
        /// <summary>
        /// 清除路径
        /// </summary>
        public readonly void Clear()
        {
            Sets.Clear();
            Nodes.Clear();
        }
        /// <summary>
        /// 是否存在某个节点
        /// </summary>
        /// <param name="node">节点</param>
        /// <returns>是否存在</returns>
        public readonly bool Exist(T node) => Sets.Contains(node);

        /// <summary>
        /// 获取某个节点
        /// </summary>
        /// <param name="index">节点的位置</param>
        /// <returns>获取到的节点</returns>
        public readonly T this[int index] => Nodes[index].Key;
        /// <summary>
        /// 节点输出数组
        /// </summary>
        /// <returns>输出的节点数组</returns>
        public readonly T[] ToArray()
        {
            T[] ls = new T[Nodes.Count];
            for (int i = 0; i < Nodes.Count; i++)
            {
                ls[i] = Nodes[i].Key;
            }
            return ls;
        }

        /// <summary>
        /// 获得一份深拷贝
        /// </summary>
        /// <returns>深拷贝组</returns>
        public readonly MapNodePath<T> Clone()
        {
            var clone = new MapNodePath<T>();
            foreach (var node in Nodes) clone.Add(node.Key, node.Value);
            return clone;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (!IsNotConnected)
            {
                StringBuilder sb = new();
                sb.Append($"Path:{Start}");
                double v = 0;
                for (int i = 1; i < Nodes.Count; i++)
                {
                    var node = Nodes[i];
                    v += Nodes[i].Value;
                    sb.Append($"-[{Nodes[i].Value}]->{node.Key}");
                }
                sb.Append($"\n-- Weight:({v})");
                return sb.ToString();
            }
            else
            {
                return "Not Connected Path";
            }
        }
        /// <inheritdoc/>
        public readonly int CompareTo(MapNodePath<T> other) => other is MapNodePath<T> i ? TotalWeight.CompareTo(i.TotalWeight) : throw new ArgumentException($"entity is null");
        /// <inheritdoc/>
        public readonly bool Equals(MapNodePath<T> other)
        {

            if (Nodes.Count != other.Nodes.Count) return false;//路径数目不一致
            if (IsNotConnected) return true; //均为未连接
            for (int i = 0; i < Nodes.Count; i++)
            {
                T? node1 = Nodes[i].Key;
                T? node2 = other.Nodes[i].Key;
                if (node1 != null && node2 != null)
                {
                    if (!node1.Equals(node2))
                    {
                        return false;//节点不一致
                    }
                }
                else
                {
                    return false;//含空路径元素
                }
            }
            return true;//完全一致
        }
        /// <inheritdoc/>
        public override readonly bool Equals(object? obj) => obj is MapNodePath<T> i && i.Equals(this);
        /// <inheritdoc/>
        public override readonly int GetHashCode()
        {
            int p = 0;
            foreach (var node in Nodes) p ^= node.GetHashCode();
            return p;
        }
        /// <inheritdoc/>
        public readonly IEnumerator<KeyValuePair<T, double>> GetEnumerator() => Nodes.GetEnumerator();
        /// <inheritdoc/>
        readonly IEnumerator IEnumerable.GetEnumerator() => Nodes.GetEnumerator();

        /// <inheritdoc/>
        public static bool operator ==(MapNodePath<T> left, MapNodePath<T> right) => left.Equals(right);
        /// <inheritdoc/>
        public static bool operator !=(MapNodePath<T> left, MapNodePath<T> right) => !(left == right);
        /// <inheritdoc/>
        public static bool operator <(MapNodePath<T> left, MapNodePath<T> right) => left.CompareTo(right) < 0;
        /// <inheritdoc/>
        public static bool operator <=(MapNodePath<T> left, MapNodePath<T> right) => left.CompareTo(right) <= 0;
        /// <inheritdoc/>
        public static bool operator >(MapNodePath<T> left, MapNodePath<T> right) => left.CompareTo(right) > 0;
        /// <inheritdoc/>
        public static bool operator >=(MapNodePath<T> left, MapNodePath<T> right) => left.CompareTo(right) >= 0;
    }
}
