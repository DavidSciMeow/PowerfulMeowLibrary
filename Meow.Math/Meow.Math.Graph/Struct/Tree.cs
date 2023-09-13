using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Meow.Math.Graph.Struct
{
    /// <summary>
    /// 可遍历节点接口<br/>Traversable Node Interface
    /// </summary>
    /// <typeparam name="T">节点类型<br/>Node Struct</typeparam>
    public interface ITraversableNode<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 添加一个连接节点(朝表的后部) :: O(n)<br/>Link A Node To List's Rear Side :: O(n)
        /// </summary>
        /// <param name="node">节点<br/>Node</param>
        public void AddRear(T node);
        /// <summary>
        /// 添加一个连接节点(朝表的前部) :: O(n)<br/>Link A Node To List's Front Side :: O(n)
        /// </summary>
        /// <param name="node">节点<br/>Node</param>
        public void AddFront(T node);
        /// <summary>
        /// 删除一个连接节点 :: O(n)<br/>Delete A Node :: O(n)
        /// </summary>
        /// <param name="NodeName">节点名<br/>NodeName</param>
        public bool Delete(T NodeName);
        /// <summary>
        /// 判定一个节点是否连接 :: O(n)<br/>Determin A Node Link Or Not :: O(n)
        /// </summary>
        /// <param name="NodeName">节点识别名<br/>NodeName</param>
        public bool Exist(T NodeName);
        /// <summary>
        /// 获取链接的节点 :: O(n)<br/>Get A Linked Node :: O(n)
        /// </summary>
        /// <param name="NodeName">节点名<br/>NodeName</param>
        public T? Get(T NodeName);
    }
    /// <summary>
    /// 可遍历节点<br/>TraversableNode
    /// </summary>
    /// <typeparam name="T">节点类型<br/>Node Struct</typeparam>
    public abstract class TraversableNode<T> : ITraversableNode<T>, IEnumerable<T>, IEquatable<TraversableNode<T>?> where T : IEquatable<T>
    {
        /// <summary>
        /// 内容锁<br/> multi thread safe lock
        /// </summary>
        object ContentLock { get; set; } = new object();
        /// <summary>
        /// 构造方法<br/> Constructor
        /// </summary>
        /// <param name="id">本节点的识别<br/>ID for this node</param>
        public TraversableNode(T id) => Id = id;
        /// <summary>
        /// 当前节点包装类<br/>Current Node
        /// </summary>
        public T Id { get; protected set; }
        /// <summary>
        /// 当前节点链接的节点<br/>Node Which Linked This Node.
        /// </summary>
        protected T[] Linked { get; set; } = Array.Empty<T>();
        /// <summary>
        /// 当前节点链接的节点数(出度)<br/>out-degree of this Node
        /// </summary>
        public int Count { get => Linked.Length; }
        /// <summary>
        /// 根据节点名称获取当前的链接节点<br/>Gets the linked node based on the node name
        /// </summary>
        /// <param name="name">节点的识别<br/>Node ID</param>
        /// <returns>节点<br/>Node</returns>
        public T? this[T name] => Get(name);
        /// <summary>
        /// 根据节点顺序获取当前的链接节点<br/>Gets the linked nodes based on node order
        /// </summary>
        /// <param name="name">节点的识别<br/>Node ID</param>
        /// <returns>节点<br/>Node</returns>
        public T this[int pos] => Linked[pos];
        
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
        public bool Delete(T NodeName)
        {
            lock (ContentLock)
            {
                if (Array.Exists(Linked, d => d.Equals(NodeName)))
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
        public bool Exist(T NodeName)
        {
            lock (ContentLock)
            {
                return Array.Exists(Linked, d => d.Equals(NodeName));
            }
        }
        public T? Get(T NodeName)
        {
            lock (ContentLock)
            {
                foreach (var i in Linked)
                {
                    if (i.Equals(NodeName))
                    {
                        return i;
                    }
                }
                return default;
            }
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => Equals(obj as TraversableNode<T>);
        /// <inheritdoc/>
        public bool Equals(TraversableNode<T>? other) => other is not null && Id.Equals(other.Id);
        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator() => Linked.ToList().GetEnumerator();
        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => Linked.ToList().GetEnumerator();
        /// <inheritdoc/>
        public static bool operator ==(TraversableNode<T> left, TraversableNode<T> right) => left.Equals(right);
        /// <inheritdoc/>
        public static bool operator !=(TraversableNode<T> left, TraversableNode<T> right) => !(left == right);
    }
    /// <summary>
    /// 树节点<br/>Tree Node
    /// </summary>
    /// <typeparam name="T">树节点类型<br/>Tree Node Type</typeparam>
    public class TreeNode<T> : TraversableNode<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 生成一个树节点<br/>Generate a Tree node, which is Controlled by A Tree Structure
        /// </summary>
        /// <param name="id">节点名称<br/>Node ID</param>
        /// <param name="parents">父节点<br/>Node Parents</param>
        /// <param name="descendants">子节点<br/>Node Descendants</param>
        /// <param name="siblings">兄弟节点<br/>Node Siblings</param>
        public TreeNode(T id,
            T? parents = default, 
            T[]? descendants = null, 
            T[]? siblings = null) : base(id)
        {
            Parents = parents;
            Siblings = siblings ?? Array.Empty<T>();
            if(descendants is T[] t)
            {
                foreach(var i in t) AddRear(i);
            }
        }

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
        public T[] Descendants { get => Linked; }

        /// <summary>
        /// 判定当前树节点是否是根节点<br/>Determine node is Root or not
        /// </summary>
        public bool IsRoot => Parents is null;
        /// <summary>
        /// 判定当前树节点是否是叶节点<br/>Determine node is Leaf or not
        /// </summary>
        public bool IsLeaf => !(Descendants?.Length > 0);
    }

    /// <summary>
    /// 树结构<br/> Tree Structure
    /// </summary>
    /// <typeparam name="T">树节点类型<br/> Tree Node Type</typeparam>
    public class Tree<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 初始化一个树结构 <br/> Init A Tree
        /// </summary>
        /// <param name="root">树的根节点<br/> Root Node</param>
        public Tree(T root)
        {
            Root = root;
            AddNode(Root);
        }
        /// <summary>
        /// 树的根节点<br/> Root Node
        /// </summary>
        public T Root { get; init; }
        /// <summary>
        /// 邻接矩阵 键值对为 [节点识别号, 节点的邻接节点]<br/>
        /// Adjacency Tables which structure [Key, Value] is [NodeID, Node which links Key ]
        /// </summary>
        Dictionary<T, HashSet<T>> AdjacencyTables { get; init; } = new();
        /// <summary>
        /// 邻接矩阵 键值对为 [节点识别号, 节点的邻接节点]<br/>
        /// Adjacency Tables which structure [Key, Value] is [NodeID, Node which links Key ]
        /// </summary>
        public IEnumerable<KeyValuePair<T, HashSet<T>>> AdjacencyTable => AdjacencyTables;
        /// <summary>
        /// 添加一个节点<br/>Add A Node To Tree
        /// </summary>
        /// <param name="node">节点名<br/>NodeID</param>
        /// <param name="RootBy">父节点名(留空添加到根)<br/>Root Node ID (leave default to add into Tree's Root)</param>
        /// <returns>节点是否成功添加<br/>the Node addition completeness</returns>
        public bool AddNode(T node, T? RootBy = default)
        {
            RootBy ??= Root;
            if (!node.Equals(Root) && !AdjacencyTables.TryAdd(RootBy, new() { node })) AdjacencyTables[RootBy].Add(node);
            return AdjacencyTables.TryAdd(node, new());
        }
        /// <summary>
        /// 广度优先遍历节点<br/> Enumerate all childnode by Breadth First Search order.
        /// </summary>
        /// <param name="node">搜索起始点(留空为根)<br/>Search Starts From.. (leave default to traversal from root)</param>
        /// <returns>节点列表<br/> List Of Nodes</returns>
        public List<T> BFS(T? node = default)
        {
            if (node is not null && !AdjacencyTables.ContainsKey(node)) throw new ArgumentException($"node is not in Tree", nameof(node));
            HashSet<T> visited = new() { node ?? Root };//标记头节点
            Queue<T> queue = new();
            List<T> path = new();
            queue.Enqueue(node ?? Root);//头节点入队
            while (queue.Any()) //σ(n) 任意队列不空
            {
                var s = queue.Peek();//获取队列头元素
                path.Add(s);//添加路径
                queue.Dequeue();//末尾的元素出队
                foreach (var val in AdjacencyTables[s]) //σ(k) 获取节点的邻接节点
                {
                    if (visited.Add(val))//没添加过
                    {
                        queue.Enqueue(val);//元素入队
                    }
                }
            }
            return path;
        }
        /// <summary>
        /// 深度优先遍历节点<br/> Enumerate all childnode by Depth First Search order.
        /// </summary>
        /// <param name="node">搜索起始点(留空为根)<br/>Search Starts From.. (leave default to traversal from root)</param>
        /// <returns>节点列表<br/> List Of Nodes</returns>
        public List<T> DFS(T? node = default)
        {
            if (node is not null && !AdjacencyTables.ContainsKey(node)) throw new ArgumentException($"node is not in Tree", nameof(node));
            HashSet<T> visited = new() { node ?? Root };//首元素访问标记
            List<T> path = new() { node ?? Root };//头元素入搜索表
            Stack<T> ss = new();
            ss.Push(node ?? Root);//搜索元素入栈
            while (ss.Any())//σ(n) 若栈不空
            {
                bool _isEdgeVisited = true;//标记边访问
                foreach (var i in AdjacencyTables[ss.Peek()])//σ(n-k) 获得下一节点
                {
                    if (visited.Add(i))//未访问节点则标记访问
                    {
                        ss.Push(i);//入栈
                        path.Add(i);//添加访问表
                        _isEdgeVisited = false;//取消边访问
                        break;
                    }
                }

                if (_isEdgeVisited) ss.Pop();//边访问, 元素出栈
            }
            return path;
        }

        public T? GetParent(T? node = default)
        {
            foreach(var (p,nl) in AdjacencyTables) // O(n)
            {
                if (node is not null && nl.Contains(node)) return p; // O(1)
            }
            return default;
        }

        public Dictionary<T, TreeNode<T>> GetNodeTable()
        {
            Dictionary<T, TreeNode<T>> rets = new();
            foreach (var (node,nl) in AdjacencyTables)
            {
                var p = GetParent(node);
                var sib = new List<T>();
                if (p is not null)
                {
                    foreach (var t in AdjacencyTables[p].ToArray())
                    {
                        if (!t.Equals(node)) sib.Add(t);
                    }
                }
                rets.TryAdd(node,new TreeNode<T>(node,p,nl.ToArray(),sib.ToArray()));
            }
            return rets;
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine($"Root:{Root} | NodeNum:{AdjacencyTables.Count}");
            HashSet<T> visited = new() { Root };//首元素访问标记
            Stack<T> ss = new();
            ss.Push(Root);//搜索元素入栈
            sb.Append($"{Root}\n");
            while (ss.Any())//σ(n) 若栈不空
            {
                var node = ss.Peek();//获取栈顶元素
                bool _isEdgeVisited = true;//标记边访问
                int j = 0;
                foreach (T i in AdjacencyTables[node])//σ(n-k) 获得下一节点
                {
                    j++;
                    if (visited.Add(i))//未访问节点则标记访问
                    {
                        sb.AppendLine($"{$"{(j < AdjacencyTables[node].Count ? "├" : "└")}".PadLeft(ss.Count)}{i}");
                        ss.Push(i);//入栈
                        _isEdgeVisited = false;//取消边访问
                        break;
                    }
                }

                if (_isEdgeVisited) ss.Pop();//边访问, 元素出栈
            }
            return sb.ToString();
        }
    }
}
