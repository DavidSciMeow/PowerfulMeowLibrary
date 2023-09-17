using Meow.Math.Graph.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Meow.Math.Graph.Struct
{
    /// <summary>
    /// 可遍历节点<br/>TraversableNode
    /// </summary>
    /// <typeparam name="T">节点类型<br/>Node Struct</typeparam>
    public struct TreeNode<T> : ITreeNode<T>, IEnumerable<T>, IEquatable<TreeNode<T>?> where T : IEquatable<T>
    {
        /// <summary>
        /// 内容锁<br/> multi thread safe lock
        /// </summary>
        object ContentLock { get; set; } = new object();
        /// <summary>
        /// 当前节点包装类<br/>Current Node
        /// </summary>
        public T Id { get; set; }
        /// <summary>
        /// 当前节点链接的节点<br/>Node Which Linked This Node.
        /// </summary>
        T[] Linked { get; set; } = Array.Empty<T>();
        /// <summary>
        /// 当前节点链接的节点数(出度)<br/>out-degree of this Node
        /// </summary>
        public readonly int Count { get => Linked.Length; }
        /// <summary>
        /// 根据节点顺序获取当前的链接节点<br/>Gets the linked nodes based on node order
        /// </summary>
        /// <param name="name">节点的识别<br/>Node ID</param>
        /// <returns>节点<br/>Node</returns>
        public readonly T this[int pos] => Linked[pos];
        /// <summary>
        /// 父节点<br/>Parents
        /// </summary>
        public T? Parents { get; init; } = default;
        /// <summary>
        /// 兄弟节点<br/>Siblings
        /// </summary>
        public T[] Siblings { get; init; }
        /// <summary>
        /// 子节点<br/>Descendants
        /// </summary>
        public readonly T[] Descendants { get => Linked; }
        /// <summary>
        /// 判定当前树节点是否是根节点<br/>Determine node is Root or not
        /// </summary>
        public readonly bool IsRoot => Parents is null;
        /// <summary>
        /// 判定当前树节点是否是叶节点<br/>Determine node is Leaf or not
        /// </summary>
        public readonly bool IsLeaf => !(Descendants?.Length > 0);

        public void AddRear(T node)
        {
            lock (ContentLock)
            {
                var retx = new T[Linked.Length + 1];
                Linked.CopyTo(retx, 0);
                retx[Linked.Length] = node;
                Linked = retx;
            }
        }
        public void AddFront(T node)
        {
            lock (ContentLock)
            {
                var retx = new T[Linked.Length + 1];
                Linked.CopyTo(retx, 1);
                retx[0] = node;
                Linked = retx;
            }
        }
        public readonly bool Delete(T node)
        {
            lock (ContentLock)
            {
                if (Array.Exists(Linked, d => d.Equals(node)))
                {
                    var r = new T[Linked.Length - 1];
                    int pos = 0;
                    foreach (var i in Linked)
                    {
                        if (i.Equals(i)) r[pos++] = i;
                    }
                    return true;
                }
                return false;
            }
        }
        public readonly bool Exist(T node)
        {
            lock (ContentLock)
            {
                return Array.Exists(Linked, d => d.Equals(node));
            }
        }

        /// <summary>
        /// 生成一个树节点<br/>Generate a Tree node, which is Controlled by A Tree Structure
        /// </summary>
        /// <param name="id">节点名称<br/>Node ID</param>
        /// <param name="parents">父节点<br/>Node Parents</param>
        /// <param name="descendants">子节点<br/>Node Descendants</param>
        /// <param name="siblings">兄弟节点<br/>Node Siblings</param>
        public TreeNode(T id, T? parents = default,  T[]? descendants = null, T[]? siblings = null)
        {
            Id = id;
            Parents = parents;
            Siblings = siblings ?? Array.Empty<T>();
            if (descendants is T[] t)
            {
                foreach (var i in t)
                {
                    lock (ContentLock)
                    {
                        var retx = new T[Linked.Length + 1];
                        Linked.CopyTo(retx, 0);
                        retx[Linked.Length] = i;
                        Linked = retx;
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override readonly int GetHashCode() => Id.GetHashCode();
        /// <inheritdoc/>
        public readonly IEnumerator<T> GetEnumerator() => Linked.ToList().GetEnumerator();

        /// <inheritdoc/>
        readonly IEnumerator IEnumerable.GetEnumerator() => Linked.ToList().GetEnumerator();
        /// <inheritdoc/>
        public readonly bool Equals(TreeNode<T>? other) => other is TreeNode<T> t && t.Id.Equals(Id);
    }

}
