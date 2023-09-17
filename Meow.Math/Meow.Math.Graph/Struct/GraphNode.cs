using Meow.Math.Graph.Interface;
using System.Collections;

namespace Meow.Math.Graph.Struct
{
    /// <summary>
    /// 图节点<br/> Graph Node Structure
    /// </summary>
    /// <param name="Id"> 节点识别号<br/>Graph Node Id </param>
    /// <typeparam name="T">图节点类型<br/> Graph Node Type</typeparam>
    public readonly record struct GraphNode<T>(T Id) : IMapNode<T>, IEnumerable<KeyValuePair<T, int>> where T : IEquatable<T>
    {
        /// <summary>
        /// 节点识别号<br/>Graph Node Id
        /// </summary>
        public T Id { get; init; } = Id;
        /// <summary>
        /// 邻接矩阵 键值对为 [节点识别号, 节点的权重]<br/>
        /// Adjacency Tables which structure [Key, Value] is [Node ID (which linked), Edges Weight]
        /// </summary>
        public Dictionary<T, int> Edges { get; init; } = new();

        public readonly bool Add(T node, int weight) => Edges.TryAdd(node, weight);
        public readonly bool Delete(T node) => Edges.Remove(node);
        public readonly bool Exist(T node) => Edges.ContainsKey(node);
        public readonly IEnumerator<KeyValuePair<T, int>> GetEnumerator() => ((IEnumerable<KeyValuePair<T, int>>)Edges).GetEnumerator();
        readonly IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Edges).GetEnumerator();
    }

}
