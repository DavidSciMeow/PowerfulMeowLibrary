using Meow.Math.Graph.ErrorList;
using System.Text;

namespace Meow.Math.Graph.Struct
{
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
        /// 用于外部获取的节点表 <br/> the node table which is public get;
        /// </summary>
        public Dictionary<T, TreeNode<T>> NodeTable
        {
            get
            {
                Dictionary<T, TreeNode<T>> rets = new();
                foreach (var (node, nl) in AdjacencyTables)
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
                    rets.TryAdd(node, new TreeNode<T>(node, p, nl.ToArray(), sib.ToArray()));
                }
                return rets;
            }
        }
        /// <summary>
        /// 邻接矩阵 键值对为 [节点识别号, 节点的邻接节点]<br/>
        /// Adjacency Tables which structure [Key, Value] is [NodeID, Node which links Key ]
        /// </summary>
        private Dictionary<T, HashSet<T>> AdjacencyTables { get; init; } = new();
        /// <summary>
        /// 邻接矩阵 键值对为 [节点识别号, 节点的邻接节点]<br/>
        /// Adjacency Tables which structure [Key, Value] is [NodeID, Node which links Key ]
        /// </summary>
        public IEnumerable<KeyValuePair<T, HashSet<T>>> AdjacencyTable => AdjacencyTables;

        /// <summary>
        /// 添加一个节点<br/>Add A Node To Tree
        /// <para>时间复杂度(Time Complexity) :: <i><b><see langword="O(1)" /></b></i></para>
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
        /// <para>时间复杂度(Time Complexity) :: <i><b><see langword="O(n)" /></b></i></para>
        /// </summary>
        /// <param name="node">搜索起始点(留空为根)<br/>Search Starts From.. (leave default to traversal from root)</param>
        /// <returns>节点列表<br/> List Of Nodes</returns>
        /// <exception cref="NodeNotExistException"></exception>
        public List<T> BFS(T? node = default)
        {
            if (node is null || AdjacencyTables.ContainsKey(node)) throw new NodeNotExistException();
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
        /// <para>时间复杂度(Time Complexity) :: <i><b><see langword="O(n)" /></b></i></para>
        /// </summary>
        /// <param name="node">搜索起始点(留<see langword="空" />为根)<br/>Search Starts From.. (leave <see langword="default" /> to traversal from root)</param>
        /// <returns>节点列表<br/> List Of Nodes</returns>
        /// <exception cref="NodeNotExistException"></exception>
        public List<T> DFS(T? node = default)
        {
            if (node is not null && !AdjacencyTables.ContainsKey(node)) throw new NodeNotExistException();
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
        /// <summary>
        /// 获取某个节点的父节点<br/>Get Node's Parent
        /// <para>时间复杂度(Time Complexity) :: <i><b><see langword="O(n)" /></b></i></para>
        /// </summary>
        /// <param name="node">
        /// 节点识别, 留空为根节点, 返回 <i><see langword="default" />(<typeparamref name="T"/>)</i>
        /// <br/>Node ID, blank for Root, which by define returns <i><see langword="default" />(<typeparamref name="T"/>)</i>
        /// </param>
        /// <returns>节点的父节点<br/>Node's Parent</returns>
        /// <exception cref="NodeNotExistException"></exception>
        public T? GetParent(T? node = default)
        {
            if (Root.Equals(node)) return default;
            foreach(var (p,nl) in AdjacencyTables) // O(n)
            {
                if (node is not null && nl.Contains(node)) return p; // O(1)
            }
            throw new NodeNotExistException();
        }

        public IEnumerable<TreeNode<T>> LeafNode() => NodeTable.Where(k => k.Value.IsLeaf == true).Select(k => k.Value);
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
                var parent = ss.Peek();//获取栈顶元素
                bool _isEdgeVisited = true;//标记边访问
                int j = 0;
                foreach (T node in AdjacencyTables[parent])//σ(n-k) 获得下一节点
                {
                    j++;
                    if (visited.Add(node))//未访问节点则标记访问
                    {
                        char[] pred = new char[ss.Count];
                        for (int i = 1; i < ss.Count; i++)//对于任意前序节点
                        {
                            var llast = AdjacencyTables[ss.ElementAt(i)].Last();//检测节点父节点的最后一个
                            var desnode = ss.ElementAtOrDefault(i - 1);//当前遍历内节点
                            pred[ss.Count - i] = llast?.Equals(desnode) ?? false ? ' ' : '│';//是的话就不写杠
                        }
                        sb.Append(pred);//正着输出前序关系
                        sb.AppendLine($"{(j < AdjacencyTables[parent].Count ? "├" : "└")}{node}");//节点访问到
                        ss.Push(node);//入栈
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
