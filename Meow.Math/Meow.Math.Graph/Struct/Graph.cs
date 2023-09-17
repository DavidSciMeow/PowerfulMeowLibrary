using Meow.Math.Graph.ErrorList;

namespace Meow.Math.Graph.Struct
{
    /// <summary>
    /// 图结构<br/> Graph Structure
    /// </summary>
    /// <typeparam name="T">图节点类型<br/> Graph Node Type</typeparam>
    public class Graph<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 节点搜索表 键值对为 [节点识别号, 节点本身]<br/>
        /// NodeTables which structure [Key, Value] is [NodeID, That Node itself ]
        /// </summary>
        public IEnumerable<KeyValuePair<T, GraphNode<T>>> NodeTable => Nodes;
        /// <summary>
        /// 节点搜索表 键值对为 [节点识别号, 节点本身]<br/>
        /// NodeTables which structure [Key, Value] is [NodeID, That Node itself ]
        /// </summary>
        Dictionary<T, GraphNode<T>> Nodes { get; init; } = new();

        /// <summary>
        /// 添加节点 <br/> Add One Node
        /// </summary>
        /// <para>时间复杂度(Time Complexity) :: <i><b><see langword="O(1)" /></b></i></para>
        /// <param name="node">要添加的节点识别名<br/> Node Id </param>
        /// <returns>节点是否创建成功<br/> is Node be created</returns>
        public bool Add(T node) => Nodes.TryAdd(node, new(node));
        /// <summary>
        /// 添加节点 <br/> Add this Node
        /// <para>时间复杂度(Time Complexity) :: <i><b><see langword="O(1)" /></b></i></para>
        /// </summary>
        /// <param name="node">要添加的节点<br/> Node will be add </param>
        /// <returns>节点是否添加成功<br/> is Node be created</returns>
        public bool Add(GraphNode<T> node) => Nodes.TryAdd(node.Id, node);
        /// <summary>
        /// 检查是否存在节点<br/>Exist Node or not.
        /// <para>时间复杂度(Time Complexity) :: <i><b><see langword="O(1)" /></b></i></para>
        /// </summary>
        /// <param name="node">节点id <br/> Node ID</param>
        /// <returns>返回存在(<b><see langword="true" /></b>)与否(<b><see langword="false" /></b>)<br/><i><b><see langword="true" /></b></i> if exist other wise <i><b><see langword="false" /></b></i> </returns>
        public bool Exist(T node) => Nodes.ContainsKey(node);
        /// <summary>
        /// 获得一个节点Id标记的节点<br/>Aquire a node with a Node Id
        /// <para>时间复杂度(Time Complexity) :: <i><b><see langword="O(1)" /></b></i></para>
        /// </summary>
        /// <param name="node">节点Id<br/>Node ID</param>
        /// <returns>一个节点结构<br/> a Graph Node Structure</returns>
        public GraphNode<T> this[T node] => Nodes[node];


        /// <summary>
        /// 广度优先遍历节点<br/> Enumerate all childnode by Breadth First Search order.
        /// <para>时间复杂度(Time Complexity) :: <i><b><see langword="O(n)" /></b></i></para>
        /// </summary>
        /// <param name="node">搜索起始点<br/>Search Starts From</param>
        /// <returns>节点列表<br/> List Of Nodes</returns>
        /// <exception cref="NodeNotExistException"></exception>
        public List<T> BFS(T node)
        {
            if (node is null || !Nodes.ContainsKey(node)) throw new NodeNotExistException();
            HashSet<T> visited = new() { node };//标记头节点
            Queue<T> queue = new();
            List<T> path = new();
            queue.Enqueue(node);//头节点入队
            while (queue.Any()) //σ(n) 任意队列不空
            {
                var s = queue.Peek();//获取队列头元素
                path.Add(s);//添加路径
                queue.Dequeue();//末尾的元素出队
                foreach (var (i,_) in Nodes[s]) //σ(k) 获取节点的邻接节点
                {
                    if (visited.Add(i)) queue.Enqueue(i);//没添加过的元素入队
                }
            }
            return path;
        }
        /// <summary>
        /// 深度优先遍历节点<br/> Enumerate all childnode by Depth First Search order.
        /// <para>时间复杂度(Time Complexity) :: <i><b><see langword="O(n)" /></b></i></para>
        /// </summary>
        /// <param name="node">搜索起始点<br/>Search Starts From</param>
        /// <returns>节点列表<br/> List Of Nodes</returns>
        /// <exception cref="NodeNotExistException"></exception>
        public List<T> DFS(T node)
        {
            if (node is null || !Nodes.ContainsKey(node)) throw new NodeNotExistException();
            HashSet<T> visited = new() { node };//首元素访问标记
            List<T> path = new() { node };//头元素入搜索表
            Stack<T> ss = new();
            ss.Push(node);//搜索元素入栈
            while (ss.Any())//σ(n) 若栈不空
            {
                bool _isEdgeVisited = true;//标记边访问
                foreach (var (i, _) in Nodes[ss.Peek()])//σ(n-k) 获得下一节点
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
        /// 使用<b>迪杰斯特拉算法</b>计算两个点的最短路径<br/>
        /// calculate the least cost path using <i><b>Dijkstra Algorithm.</b></i>
        /// <para>
        /// 时间复杂度(Time Complexity) :: <i><b><see langword="σ(n+k) ~ O(n^2-2n)" /></b></i><br/>
        /// </para>
        /// </summary>
        /// <param name="start">起始点<br/>Start Point</param>
        /// <param name="end">终止点<br/>End Point</param>
        /// <param name="HeuristicFunc">曼哈顿距离, [终点节点, 当前节点](优化用)<br/>Manhattan distance between [End, NowPositon]</param>
        /// <returns>生成的最短路径 <br/> the Least cost path</returns>
        /// <exception cref="NodeNotExistException"></exception>
        /// <exception cref="NodeUnreachableException"></exception>
        public List<T> Dijkstra(T start, T end, Func<T, T, int>? HeuristicFunc = null)
        {
            if (!Exist(start) || !Exist(end)) throw new NodeNotExistException();//不存在这两个节点

            Queue<T> queue = new();//生成队列
            queue.Enqueue(start);//头节点入队列
            var edges = new Dictionary<T, T>();//经过的边
            var pathcost = new Dictionary<T, int> { { start, 0 } };//经过的每个节点的数据

            while (queue.Any())//σ(n)队列不空, 进入搜索循环
            {
                var current = queue.Dequeue();//取队列节点
                if (current.Equals(end)) break; //已经搜索到, 退出搜索循环
                foreach(var i in this[current])//任意邻接节点
                {
                    var nowcost = pathcost[current] + i.Value;//计算节点当前权重
                    if(!pathcost.ContainsKey(i.Key) || nowcost < pathcost[i.Key])//若节点不存在记录过的权重, 或者新权重更小则松弛节点
                    {
                        pathcost[i.Key] = nowcost + (HeuristicFunc?.Invoke(end, i.Key) ?? 0);//权重更新
                        queue.Enqueue(i.Key);//新节点入队列
                        edges[i.Key] = current;//原节点链接
                    }
                }
            }

            if (edges.ContainsKey(end))
            {
                Stack<T> path = new();//形成路径
                path.Push(end);//末尾位置入栈
                var node = edges[end];//取末尾位置链接点 σ(1)
                while (!node.Equals(start))//前序寻找 最大次数σ(k)
                {
                    path.Push(node);//节点入栈
                    node = edges[node];//前序节点更新
                }
                path.Push(start);//已完成寻找,添加头节点

                return path.ToList();//返回路径
            }
            else
            {
                throw new NodeUnreachableException();//节点全遍历后不可达
            }
        }


    }

}
