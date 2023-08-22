using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Util.Math.Graph
{
    /// <summary>
    /// 一般节点
    /// </summary>
    /// <typeparam name="T">节点类型 (必须可比较是否相同)</typeparam>
    [Serializable]
    public struct Node<T> : IEnumerable<KeyValuePair<T, int>>, IEquatable<Node<T>> where T : IEquatable<T>
    {
        /// <summary>
        /// 唯一识别元素码
        /// </summary>
        public readonly T Id;
        /// <summary>
        /// 内部维护邻接表
        /// </summary>
        public Dictionary<T, int> LinkSet { get; private set; } = new();
        /// <summary>
        /// 出度
        /// </summary>
        public readonly int Count => LinkSet.Count;
        /// <summary>
        /// 初始化节点
        /// </summary>
        /// <param name="Id">节点识别码</param>
        public Node(T Id) => this.Id = Id;
        /// <summary>
        /// 按顺序获取节点 σ(n) + σ(1)
        /// </summary>
        /// <param name="pos">顺序</param>
        /// <returns>获取到的节点</returns>
        public readonly T this[int pos] => LinkSet.ToArray()[pos].Key;

        /// <summary>
        /// 测试是否和节点相连
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public readonly bool IsConnectTo(T Id) => LinkSet.ContainsKey(Id);
        /// <summary>
        /// 取链接的节点权重 σ(1)
        /// </summary>
        /// <param name="id">节点识别码</param>
        /// <returns>权重</returns>
        public readonly int GetWeight(T id) => LinkSet[id];
        /// <summary>
        /// 链接节点 σ(1)
        /// </summary>
        /// <param name="nodename">连接到</param>
        /// <param name="weight">权重</param>
        /// <returns>是否链接成功</returns>
        public readonly bool LinkTo(T nodename, int weight = 1) => LinkSet.TryAdd(nodename, weight);
        /// <summary>
        /// 移除节点 σ(1)
        /// </summary>
        /// <param name="nodename">节点识别码</param>
        /// <returns>是否移除成功</returns>
        public readonly bool RemoveLink(T nodename) => LinkSet.Remove(nodename);
        /// <summary>
        /// 测试邻接表是否存在资源 σ(1)
        /// </summary>
        /// <returns></returns>
        public readonly bool Any() => LinkSet.Any();

        /// <inheritdoc/>
        public readonly IEnumerator<KeyValuePair<T, int>> GetEnumerator() => LinkSet.AsEnumerable().GetEnumerator();
        /// <inheritdoc/>
        readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        /// <inheritdoc/>
        public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Node<T> other && Equals(other);
        /// <inheritdoc/>
        public override readonly int GetHashCode() => Id.GetHashCode();
        /// <inheritdoc/>
        public override readonly string ToString() => $"[Node:{Id}]";
        /// <inheritdoc/>
        public readonly bool Equals(Node<T> other) => Id.Equals(other.Id);
        /// <inheritdoc/>
        public static bool operator ==(Node<T> left, Node<T> right) => left.Equals(right);
        /// <inheritdoc/>
        public static bool operator !=(Node<T> left, Node<T> right) => !(left.Equals(right));
    }
}
