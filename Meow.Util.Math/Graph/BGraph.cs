using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Meow.Util.Math.Graph
{
    /// <summary>
    /// 图
    /// </summary>
    /// <typeparam name="T">节点类型 (必须可比较是否相同)</typeparam>
    [Serializable]
    public struct BGraph<T> where T : IEquatable<T>
    {
        public BGraph() { }

        /// <summary>
        /// 图名称
        /// </summary>
        public string GraphName { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// 节点表
        /// </summary>
        public Dictionary<T, Node<T>> NodeList { get; set; } = new();
        /// <summary>
        /// 边表
        /// </summary>
        public readonly HashSet<Edge<T>> EdgeList { get => GetEdgeList(); }

        #region 节点操作 σ(1)
        /// <summary>
        /// 增加某个节点 σ(1)
        /// </summary>
        /// <param name="node">节点Id</param>
        /// <returns>是否成功添加</returns>
        public readonly bool Insert(Node<T> node) => NodeList.TryAdd(node.Id, node);
        /// <summary>
        /// 增加某个节点 σ(1)
        /// </summary>
        /// <param name="node">节点Id</param>
        /// <returns>是否成功添加</returns>
        public readonly bool Insert(T node) => NodeList.TryAdd(node, new Node<T>(node));
        /// <summary>
        /// 添加所有列表内节点 σ(1) * n
        /// </summary>
        /// <param name="nodes">节点列表</param>
        public readonly void Insert(params T[] nodes)
        {
            foreach (var node in nodes)
            {
                NodeList.Add(node, new Node<T>(node));
            }
        }
        /// <summary>
        /// 添加所有列表内节点 σ(1) * n
        /// </summary>
        /// <param name="nodes">节点列表</param>
        public readonly void Insert(params Node<T>[] nodes)
        {
            foreach (var node in nodes)
            {
                NodeList.Add(node.Id, node);
            }
        }
        /// <summary>
        /// 删除某个节点 σ(1)
        /// </summary>
        /// <param name="node">节点Id</param>
        /// <returns>是否删除</returns>
        public readonly bool RemoveById(T node) => NodeList.Remove(node);
        /// <summary>
        /// 获取某个节点 σ(1)
        /// </summary>
        /// <param name="nodeid">节点Id</param>
        /// <returns>获取到的节点</returns>
        public readonly Node<T> GetNodeById(T nodeid) => NodeList.TryGetValue(nodeid, out var o) ? o : throw new($"[{typeof(T)}:{nodeid}] node is not Exist in set");
        /// <summary>
        /// 查询是否存在节点 σ(1)
        /// </summary>
        /// <param name="nodeid">节点Id</param>
        /// <returns>是否存在</returns>
        public readonly bool ExistNodeById(T nodeid) => NodeList.ContainsKey(nodeid);
        /// <summary>
        /// 链接 (双向)
        /// </summary>
        /// <param name="nodeA">节点A</param>
        /// <param name="nodeB">节点B</param>
        /// <param name="weight">权重(默认为1)</param>
        public readonly void Link(T nodeA, T nodeB, int weight = 1)
        {
            _ = NodeList.TryGetValue(nodeA, out var _NodeA) ? _NodeA : throw new($"node [{nodeA}] is not in Set");
            _ = NodeList.TryGetValue(nodeB, out var _NodeB) ? _NodeB : throw new($"node [{nodeB}] is not in Set");
            _NodeA.LinkTo(nodeB, weight);
            _NodeB.LinkTo(nodeA, weight);
        }
        /// <summary>
        /// 链接到(单向)
        /// </summary>
        /// <param name="nodeA">节点A</param>
        /// <param name="nodeB">节点B</param>
        /// <param name="weight">权重(默认为1)</param>
        public readonly void LinkTo(T nodeA, T nodeB, int weight = 1)
        {
            _ = NodeList.TryGetValue(nodeA, out var _NodeA) ? _NodeA : throw new($"node [{nodeA}] is not in Set");
            _ = NodeList.TryGetValue(nodeB, out var _NodeB) ? _NodeB : throw new($"node [{nodeB}] is not in Set");
            _NodeA.LinkTo(nodeB, weight);
        }
        /// <summary>
        /// 获取某个节点 σ(1)
        /// </summary>
        /// <param name="id">节点Id</param>
        /// <returns>获取到的节点</returns>
        /// 
        public readonly Node<T> this[T id] => NodeList[id];
        /// <summary>
        /// 某边的权重 σ(1)
        /// </summary>
        /// <param name="from">起始顶点</param>
        /// <param name="to">终止顶点</param>
        /// <returns>边权重</returns>
        public readonly int this[T from, T to] => GetNodeById(from).GetWeight(to);
        /// <summary>
        /// 某边的权重 σ(1)
        /// </summary>
        /// <param name="e">查找的边</param>
        /// <returns>边权重</returns>
        public readonly int this[Edge<T> e] => GetNodeById(e.Start).GetWeight(e.End);
        #endregion

        #region 边操作
        /// <summary>
        /// 获得所有以此节点开始的边
        /// </summary>
        /// <param name="node">
        /// 节点
        /// </param>
        /// <returns>
        /// 符合的节点路径
        /// </returns>
        public readonly Edge<T>[] GetEdgeFromNode(T node)
        {
            List<Edge<T>> lst = new();
            foreach (var i in GetNodeById(node))
            {
                lst.Add(new(node, i.Key, i.Value));
            }
            return lst.ToArray();
        }
        /// <summary>
        /// 获得一个明确的边
        /// </summary>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        /// <returns></returns>
        public readonly Edge<T> GetSpecificEdge(T start, T end) => NodeList[start].IsConnectTo(end) ? new(start, end, NodeList[start].GetWeight(end)) : throw new($"[{start}<->{end}] Not Connected");
        /// <summary>
        /// 还原一个有方向的边
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public readonly bool RestoreEdge(T start, T end, int weight = 1) => NodeList[start].LinkTo(end, weight);
        /// <summary>
        /// 还原一个有方向的边
        /// </summary>
        /// <param name="edg"></param>
        /// <returns></returns>
        public readonly bool RestoreEdge(Edge<T> edg) => NodeList[edg.Start].LinkTo(edg.End, edg.Weight);
        /// <summary>
        /// 移除一条边
        /// </summary>
        /// <param name="start">开始节点</param>
        /// <param name="end">结束节点</param>
        /// <returns></returns>
        public readonly bool RemoveEdge(T start, T end) => NodeList[start].RemoveLink(end);
        /// <summary>
        /// 双向移除一条边
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns></returns>
        public readonly bool RemoveCompleteEdge(T node1, T node2) => NodeList[node1].RemoveLink(node2) && NodeList[node2].RemoveLink(node1);
        /// <summary>
        /// 获得图的边遍历模式
        /// </summary>
        /// <returns></returns>
        public readonly HashSet<Edge<T>> GetEdgeList()
        {
            HashSet<Edge<T>> hs = new();
            foreach (var i in NodeList)
            {
                foreach (var j in GetEdgeFromNode(i.Key))
                {
                    hs.Add(j);
                }
            }
            return hs;
        }
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
        #endregion

        #region 图遍历
        /// <summary>
        /// 广度优先遍历 σ(n*k)
        /// </summary>
        /// <param name="Start">起始点</param>
        /// <returns>遍历次序</returns>
        public readonly List<T> BFS(T Start) 
        {
            HashSet<T> visited = new() { Start };//标记头节点
            Queue<T> queue = new();
            List<T> path = new();
            Result<List<T>> ret = new(path)
            {
                TaskStart = DateTime.Now,//记录开始时间
                MemoryStart = Process.GetCurrentProcess().WorkingSet64 / 1024.0 / 1024.0,//记录开始内存大小
            };
            queue.Enqueue(Start);//头节点入队
            while (queue.Any()) //σ(n) 任意队列不空
            {
                ret.OuterCircleTime++;//外循环变量叠加
                var s = queue.Peek();//获取队列头元素
                path.Add(s);//添加路径
                queue.Dequeue();//末尾的元素出队
                foreach (var val in GetNodeById(s)) //σ(k) 获取节点的邻接节点
                {
                    ret.InnerCircleTime++;//内循环变量叠加
                    if (visited.Add(val.Key))//没添加过
                    {
                        queue.Enqueue(val.Key);//元素入队
                    }
                }
            }
            ret.TaskEnd = DateTime.Now;//记录结束时间
            ret.MemoryEnd = Process.GetCurrentProcess().WorkingSet64 / 1024.0 / 1024.0;//记录结束内存大小
            return path;
        }
        /// <summary>
        /// 深度优先遍历 σ(n*(n-k))
        /// </summary>
        /// <param name="Start"></param>
        /// <returns></returns>
        public readonly List<T> DFS(T Start)
        {
            HashSet<T> visited = new() { Start };//首元素访问标记
            List<T> path = new() { Start };//头元素入搜索表
            Result<List<T>> ret = new(path)
            {
                TaskStart = DateTime.Now,//记录开始时间
                MemoryStart = Process.GetCurrentProcess().WorkingSet64 / 1024.0 / 1024.0,//记录开始内存大小
            };
            Stack<T> ss = new();
            ss.Push(Start);//搜索元素入栈
            while (ss.Any())//σ(n) 若栈不空
            {
                ret.OuterCircleTime++;//外循环次数
                var node = ss.Peek();//获取栈顶元素
                bool _isEdgeVisited = true;//标记边访问
                foreach(var i in GetNodeById(node))//σ(n-k) 获得下一节点
                {
                    ret.InnerCircleTime++;//内循环次数
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
            ret.TaskEnd = DateTime.Now;//记录结束时间
            ret.MemoryEnd = Process.GetCurrentProcess().WorkingSet64 / 1024.0 / 1024.0;//记录结束内存大小
            return path;
        }
        #endregion








        /// <summary>
        /// 保存图(二进制序列化器)
        /// </summary>
        /// <param name="path">路径</param>
        public readonly void Save(string path)
        {
            MemoryStream rems = new();
            new BinaryFormatter().Serialize(rems, this);
            File.WriteAllBytes(path, rems.GetBuffer());
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
    }
}
