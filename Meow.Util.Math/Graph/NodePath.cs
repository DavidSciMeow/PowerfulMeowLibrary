using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Util.Math.Graph
{
    /// <summary>
    /// 节点路径结构
    /// </summary>
    /// <typeparam name="T">节点类型 (必须可比较是否相同)</typeparam>
    public struct NodePath<T> : IEnumerable<T>, IEquatable<NodePath<T>>, IComparable<NodePath<T>> where T : IEquatable<T>
    {
        /// <summary>
        /// 节点列表
        /// </summary>
        public List<T> Nodes { get; init; }
        /// <summary>
        /// 路径总权值
        /// </summary>
        public int PathWeight { get; set; }
        /// <summary>
        /// 实例化路径结构(初始)
        /// </summary>
        public NodePath()
        {
            Nodes = new List<T>();
            PathWeight = 0;
        }
        /// <summary>
        /// 实例化路径结构(深拷贝)
        /// </summary>
        /// <param name="array">路径列</param>
        /// <param name="weight">路径权值</param>
        public NodePath(List<T> array, int weight)
        {
            Nodes = array;
            PathWeight = weight;
        }
        /// <summary>
        /// 路径内节点总数
        /// </summary>
        public readonly int Count => Nodes.Count;
        /// <summary>
        /// 开始节点
        /// </summary>
        public readonly T Start => Nodes[0];
        /// <summary>
        /// 结束节点
        /// </summary>
        public readonly T End => Nodes[^1];

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="node">节点</param>
        public readonly void Add(T node) => Nodes.Add(node);
        /// <summary>
        /// 移除节点
        /// </summary>
        /// <param name="node">节点</param>
        /// <returns>是否移除成功</returns>
        public readonly bool Remove(T node) => Nodes.Remove(node);
        /// <summary>
        /// 清除路径
        /// </summary>
        public readonly void Clear() => Nodes.Clear();
        /// <summary>
        /// 获取某个节点
        /// </summary>
        /// <param name="index">节点的位置</param>
        /// <returns>获取到的节点</returns>
        public readonly T this[int index] => Nodes[index];
        /// <summary>
        /// 节点输出数组
        /// </summary>
        /// <returns>输出的节点数组</returns>
        public readonly T[] ToArray() => Nodes.ToArray();
        /// <summary>
        /// 获得一份深拷贝
        /// </summary>
        /// <returns>深拷贝组</returns>
        public readonly NodePath<T> Clone()
        {
            var clone = new NodePath<T>();
            foreach (var node in Nodes)
            {
                clone.Add(node);
            }
            clone.PathWeight = PathWeight;
            return clone;
        }
        /// <summary>
        /// 合并路径
        /// </summary>
        /// <param name="patha">路径1</param>
        /// <param name="pathb">路径2</param>
        /// <returns>合并后的路径</returns>
        public static NodePath<T> Combine(NodePath<T> patha, NodePath<T> pathb)
        {
            var ret = new NodePath<T>
            {
                PathWeight = patha.PathWeight + pathb.PathWeight
            };
            foreach (var i in patha)
            {
                if (!ret.Contains(i))
                {
                    ret.Add(i);
                }
            }
            foreach (var i in pathb)
            {
                if (!ret.Contains(i))
                {
                    ret.Add(i);
                }
            }
            return ret;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            bool t = false;
            StringBuilder sb = new();
            foreach (var node in Nodes)
            {
                sb.Append($"{(t ? "->" : "")}{node}");
                t = true;
            }
            return sb.ToString();
        }
        /// <inheritdoc/>
        public readonly int CompareTo(NodePath<T> other)
        {
            if (other is NodePath<T> i)
            {
                return PathWeight.CompareTo(i.PathWeight);
            }
            throw new ArgumentException($"entity is null");
        }
        /// <inheritdoc/>
        public readonly bool Equals(NodePath<T> other)
        {
            if(Nodes.Count != other.Nodes.Count) return false;//路径数目不一致
            for (int i = 0; i < Nodes.Count; i++)
            {
                T? node1 = Nodes[i];
                T? node2 = other.Nodes[i];
                if(node1 != null && node2 != null)
                {
                    if(!node1.Equals(node2))
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
        public override readonly bool Equals(object? obj) => obj is NodePath<T> i && i.Equals(this);
        /// <inheritdoc/>
        public override readonly int GetHashCode()
        {
            int p = 0;
            foreach(var node in Nodes)
            {
                p ^= node.GetHashCode();
            }
            return p;
        }
        /// <inheritdoc/>
        public readonly IEnumerator<T> GetEnumerator() => Nodes.GetEnumerator();
        /// <inheritdoc/>
        readonly IEnumerator IEnumerable.GetEnumerator() => Nodes.GetEnumerator();

        /// <inheritdoc/>
        public static NodePath<T> operator +(NodePath<T> left, NodePath<T> right) => Combine(left, right);
        
        /// <inheritdoc/>
        public static bool operator ==(NodePath<T> left, NodePath<T> right) => left.Equals(right);
        /// <inheritdoc/>
        public static bool operator !=(NodePath<T> left, NodePath<T> right) => !(left == right);
        /// <inheritdoc/>
        public static bool operator <(NodePath<T> left, NodePath<T> right) => left.CompareTo(right) < 0;
        /// <inheritdoc/>
        public static bool operator <=(NodePath<T> left, NodePath<T> right) => left.CompareTo(right) <= 0;
        /// <inheritdoc/>
        public static bool operator >(NodePath<T> left, NodePath<T> right) => left.CompareTo(right) > 0;
        /// <inheritdoc/>
        public static bool operator >=(NodePath<T> left, NodePath<T> right) => left.CompareTo(right) >= 0;
    }
}
