using System.Text;
using Meow.Math.Graph.Class;
using Meow.Math.Graph.Interface;
using Meow.Math.Graph.Struct;

namespace Meow.Math.Graph
{
    /// <summary>
    /// 实例化一个图
    /// </summary>
    /// <typeparam name="T">图节点类型</typeparam>
    public struct BGraph<T> : IBGraph<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 实例化一个图
        /// </summary>
        public BGraph() { }
        public string GraphName { get; set; } = Guid.NewGuid().ToString();
        public IDictionary<T, IMapNode<T>> NodeList { get; } = new Dictionary<T, IMapNode<T>>();
        public ISet<IMapEdge<T>> EdgeList
        {
            get
            {
                HashSet<IMapEdge<T>> hs = new();
                foreach (var i in NodeList)
                {
                    foreach (var j in GetEdgeByNodeStart(i.Key))
                    {
                        hs.Add(j);
                    }
                }
                return hs;
            }
        }
        public readonly int NodeCount => NodeList.Count;
        public readonly int CC => Zone().Count;
        public readonly bool Insert(IMapNode<T> node) => NodeList.TryAdd(node.Id, node);
        public readonly bool Insert(T node) => NodeList.TryAdd(node, new MapNode<T>(node));
        public readonly void Insert(params T[] nodes)
        {
            foreach (var node in nodes) NodeList.Add(node, new MapNode<T>(node));
        }
        public readonly void Insert(params IMapNode<T>[] nodes)
        {
            foreach (var node in nodes) NodeList.Add(node.Id, node);
        }
        public readonly bool RemoveById(T node) => NodeList.Remove(node);
        public readonly IMapNode<T> GetNodeById(T nodeid) => NodeList.TryGetValue(nodeid, out var o) ? o : throw new ArgumentNullException(nameof(nodeid));
        public readonly bool ExistNodeById(T nodeid) => NodeList.ContainsKey(nodeid);
        public readonly void Link(T nodeA, T nodeB, double weight = 1)
        {
            _ = NodeList.TryGetValue(nodeA, out var _NodeA) ? _NodeA : throw new($"node [{nodeA}] is not in Set");
            _ = NodeList.TryGetValue(nodeB, out var _NodeB) ? _NodeB : throw new($"node [{nodeB}] is not in Set");
            _NodeA.LinkTo(nodeB, weight);
            _NodeB.LinkTo(nodeA, weight);
        }
        public readonly void LinkTo(T nodeA, T nodeB, double weight = 1)
        {
            _ = NodeList.TryGetValue(nodeA, out var _NodeA) ? _NodeA : throw new($"node [{nodeA}] is not in Set");
            _ = NodeList.TryGetValue(nodeB, out var _NodeB) ? _NodeB : throw new($"node [{nodeB}] is not in Set");
            _NodeA.LinkTo(nodeB, weight);
        }
        public readonly IMapNode<T> this[T id] => NodeList[id];
        public readonly bool IsConn(T start, T end) => ExistNodeById(start) && ExistNodeById(end) && (CC <= 1 || DFS(start).Contains(end));
        public readonly IMapEdge<T>[] GetEdgeByNodeStart(T node)
        {
            List<IMapEdge<T>> lst = new();
            foreach (var i in GetNodeById(node)) lst.Add(new MapEdge<T>(node, i.Key, i.Value));
            return lst.ToArray();
        }
        public readonly IMapEdge<T>? GetEdge(T start, T end) => NodeList[start].IsConnectTo(end) ? new MapEdge<T>(start, end, NodeList[start].GetWeight(end)) : null;
        public readonly bool TryGetEdge(T start, T end, out IMapEdge<T>? edg)
        {
            if (NodeList[start].IsConnectTo(end))
            {
                edg = new MapEdge<T>(start, end, NodeList[start].GetWeight(end));
                return true;
            }
            else
            {
                edg = null;
                return false;
            }
        }
        public readonly bool ExistEdge(T start, T end) => NodeList[start].IsConnectTo(end);
        public readonly bool RestoreEdge(T start, T end, double weight = 1) => NodeList[start].LinkTo(end, weight);
        public readonly bool RestoreEdgeAll(T start, T end, double weight = 1) => NodeList[start].LinkTo(end, weight) && NodeList[end].LinkTo(start, weight);
        public readonly bool RestoreEdge(IMapEdge<T> edg) => NodeList[edg.Start].LinkTo(edg.End, edg.Weight);
        public readonly bool RestoreEdgeAll(IMapEdge<T> edg) => NodeList[edg.Start].LinkTo(edg.End, edg.Weight) && NodeList[edg.End].LinkTo(edg.Start, edg.Weight);
        public readonly bool RemoveEdge(T start, T end) => NodeList[start].RemoveLink(end);
        public readonly bool RemoveEdgeAll(T node1, T node2) => NodeList[node1].RemoveLink(node2) && NodeList[node2].RemoveLink(node1);


        public readonly List<T> BFS(T Start)
        {
            HashSet<T> visited = new() { Start };//标记头节点
            Queue<T> queue = new();
            List<T> path = new();
            queue.Enqueue(Start);//头节点入队
            while (queue.Any()) //σ(n) 任意队列不空
            {
                var s = queue.Peek();//获取队列头元素
                path.Add(s);//添加路径
                queue.Dequeue();//末尾的元素出队
                foreach (var val in GetNodeById(s)) //σ(k) 获取节点的邻接节点
                {
                    if (visited.Add(val.Key))//没添加过
                    {
                        queue.Enqueue(val.Key);//元素入队
                    }
                }
            }
            return path;
        }
        public readonly List<T> DFS(T Start)
        {
            HashSet<T> visited = new() { Start };//首元素访问标记
            List<T> path = new() { Start };//头元素入搜索表
            Stack<T> ss = new();
            ss.Push(Start);//搜索元素入栈
            while (ss.Any())//σ(n) 若栈不空
            {
                var node = ss.Peek();//获取栈顶元素
                bool _isEdgeVisited = true;//标记边访问
                foreach (var i in GetNodeById(node))//σ(n-k) 获得下一节点
                {
                    if (visited.Add(i.Key))//未访问节点则标记访问
                    {
                        ss.Push(i.Key);//入栈
                        path.Add(i.Key);//添加访问表
                        _isEdgeVisited = false;//取消边访问
                        break;
                    }
                }

                if (_isEdgeVisited) ss.Pop();//边访问, 元素出栈
            }
            return path;
        }
        public readonly List<T[]> Zone()
        {
            List<T[]> k = new();
            Dictionary<T, bool> Sets = new();
            foreach (var i in NodeList) Sets.Add(i.Key, false); //σ(n) 初始化元素
            var fset = from a in Sets where a.Value == false select a.Key; //σ(n) 选择元素
            while (fset.Any()) //有没有标记的点
            {
                var ls = DFS(fset.ToArray()[0]); //深度遍历节点
                foreach (var i in ls) Sets[i] = true;//深度搜索遍历并标记未访问点
                k.Add(ls.ToArray());//添加到区域组
                fset = from a in Sets where a.Value == false select a.Key; //σ(n) 更新选择元素
            }
            return k;
        }

        public override readonly string ToString() => 
            $"-------------\n" +
            $"Graph:\n" +
            $"Name:{GraphName}\n" +
            $"NodeCount:{NodeCount}, Zone:{CC}\n" +
            $"-------------";
        /// <summary>
        /// 输出所有边
        /// </summary>
        /// <returns>所有边的表示模式</returns>
        public readonly string PrintEdge()
        {
            StringBuilder sb = new();
            foreach (var e in EdgeList)
            {
                sb.AppendLine(e.ToString());
            }
            return sb.ToString();
        }
        /// <summary>
        /// 保存为单源点拓补图
        /// </summary>
        /// <param name="path">路径</param>
        public readonly void SaveAsMappedNode(string path,string sepnode = "\n")
        {
            StringBuilder snode = new();
            StringBuilder snodelink = new();
            foreach (var i in NodeList)
            {
                snode.Append($"{i.Key}{sepnode}");
                foreach (var n in i.Value)
                {
                    if (GetNodeById(n.Key).LinkSet.ContainsKey(i.Key))
                    {
                        snodelink.Append($"{i.Key}-{n.Key}{(n.Value == 1 ? "" : $":{n.Value}")}{sepnode}");
                    }
                    else
                    {
                        snodelink.Append($"{i.Key}>{n.Key}{(n.Value == 1 ? "" : $":{n.Value}")}{sepnode}");
                    }
                }
            }
            File.WriteAllText(path, $"{snode}${sepnode}{snodelink}!");
        }

        /// <summary>
        /// 获得节点的生成树
        /// </summary>
        /// <param name="Node">节点</param>
        /// <returns>生成树</returns>
        public readonly List<T[]> NodeBFSLvList(T Node)
        {
            HashSet<T> visited = new() { Node };//标记头节点
            Queue<T> queue = new();
            List<T[]> path = new();
            Tree<T> tree = new(Node);
            queue.Enqueue(Node);//头节点入队
            while (queue.Any()) //σ(n) 任意队列不空
            {
                var s = queue.Peek();//获取队列头元素
                queue.Dequeue();//末尾的元素出队
                var nl = new List<T>();
                foreach (var val in GetNodeById(s)) //σ(k) 获取节点的邻接节点
                {
                    if (visited.Add(val.Key))//没添加过
                    {
                        tree.Add(val.Key, s);
                        queue.Enqueue(val.Key);//元素入队
                        nl.Add(val.Key);
                    }
                }
                if(nl.Count != 0) path.Add(nl.ToArray());
            }
            return path;
        }
        
        public readonly MapNodePath<T> Dijkstra(T Start, T End)
        {
            MapNodePath<T> pt = new(Start);//保存路径
            if (!IsConn(Start, End))//判定是否链接
            {
                pt.Remove(Start);//无连接
            }
            else
            {
                var visited = new HashSet<T>() { Start };//已访问点
                IMapNode<T> _n = GetNodeById(Start);//获得节点
                while (!_n.Id.Equals(End))//未达到终点
                {
                    
                }
            }
            return pt;
        }


    }
}
