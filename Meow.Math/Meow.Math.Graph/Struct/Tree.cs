using Meow.Math.Graph.Interface;
using System.Collections;
using System.Text;

namespace Meow.Math.Graph.Struct
{
    /// <summary>
    /// 树结构
    /// </summary>
    /// <typeparam name="T">节点类型</typeparam>
    public struct Tree<T> : ITree<T>, IEnumerable<KeyValuePair<T, ITreeNode<T>>> where T : IEquatable<T>
    {
        public Tree(ITreeNode<T> root)
        {
            Root = root;
            NodeSet.Add(Root.Id, root);
        }
        public Tree(T root) : this(new TreeNode<T>(root)) { }
        public IDictionary<T, ITreeNode<T>> NodeSet { get; init; } = new Dictionary<T, ITreeNode<T>>();
        public ITreeNode<T> Root { get; private set; }
        public readonly ITreeNode<T> this[T key] => GetNodeById(key);

        public readonly bool Add(T Node, ITreeNode<T>? RootBy = null)
        {
            RootBy ??= Root;
            var _node = new TreeNode<T>(Node) { ParentName = RootBy.Id };
            if (NodeSet.TryAdd(Node, _node))
            {
                RootBy.ChildsName.Add(_node.Id);
                return true;
            }
            return false;
        }
        public readonly bool Add(T Node, T RootBy) => Add(Node, this[RootBy]);
        public readonly bool AddToRoot(T Node) => Add(Node, Root);
        public readonly bool ExistNode(T key) => NodeSet.ContainsKey(key);

        //public readonly List<T> GetBranch()
        //{

        //}
        public readonly List<T> BFS(T? node = default)
        {
            HashSet<T> visited = new() { node ?? Root.Id };//标记头节点
            Queue<T> queue = new();
            List<T> path = new();
            queue.Enqueue(node ?? Root.Id);//头节点入队
            while (queue.Any()) //σ(n) 任意队列不空
            {
                var s = queue.Peek();//获取队列头元素
                path.Add(s);//添加路径
                queue.Dequeue();//末尾的元素出队
                foreach (var val in GetNodeById(s)) //σ(k) 获取节点的邻接节点
                {
                    if (visited.Add(val))//没添加过
                    {
                        queue.Enqueue(val);//元素入队
                    }
                }
            }
            return path;
        }
        public readonly List<T> DFS(T? node = default)
        {
            HashSet<T> visited = new() { node ?? Root.Id };//首元素访问标记
            List<T> path = new() { node ?? Root.Id };//头元素入搜索表
            Stack<T> ss = new();
            ss.Push(node ?? Root.Id);//搜索元素入栈
            while (ss.Any())//σ(n) 若栈不空
            {
                bool _isEdgeVisited = true;//标记边访问
                foreach (var i in GetNodeById(ss.Peek()))//σ(n-k) 获得下一节点
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
        public readonly int NodeDepth(T Node)
        {
            if (!ExistNode(Node)) return -1;
            if (Node.Equals(Root.Id)) return 0;
            int k = 0;
            HashSet<T> visited = new() { Root.Id };//首元素访问标记
            Stack<T> ss = new();
            ss.Push(Root.Id);//搜索元素入栈
            while (ss.Any())//σ(n) 若栈不空
            {
                var node = ss.Peek();//获取栈顶元素
                bool _isEdgeVisited = true;//标记边访问
                foreach (var i in GetNodeById(node))//σ(n-k) 获得下一节点
                {
                    if (visited.Add(i))//未访问节点则标记访问
                    {
                        ss.Push(i);//入栈
                        k = ss.Count;
                        _isEdgeVisited = false;//取消边访问
                        if (Node.Equals(i)) return k;
                        break;
                    }
                }

                if (_isEdgeVisited) ss.Pop();//边访问, 元素出栈
                if (ss.Count == 1) k = 0;
            }
            return k;
        }

        public readonly IEnumerator<KeyValuePair<T, ITreeNode<T>>> GetEnumerator() => NodeSet.GetEnumerator();
        readonly IEnumerator IEnumerable.GetEnumerator() => NodeSet.GetEnumerator();
        public override readonly string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine($"Root:{Root.Id} | NodeNum:{NodeSet?.Count}");
            HashSet<T> visited = new() { Root.Id };//首元素访问标记
            Stack<T> ss = new();
            ss.Push(Root.Id);//搜索元素入栈
            sb.Append($"{Root.Id}\n");
            while (ss.Any())//σ(n) 若栈不空
            {
                var node = ss.Peek();//获取栈顶元素
                bool _isEdgeVisited = true;//标记边访问
                int j = 0;
                foreach (T i in GetNodeById(node))//σ(n-k) 获得下一节点
                {
                    j++;
                    if (visited.Add(i))//未访问节点则标记访问
                    {
                        sb.AppendLine($"{$"{(j < GetSibling(i).Count ? "├" : "└")}".PadLeft(ss.Count)}{i}");
                        ss.Push(i);//入栈
                        _isEdgeVisited = false;//取消边访问
                        break;
                    }
                }

                if (_isEdgeVisited) ss.Pop();//边访问, 元素出栈
            }
            return sb.ToString();
        }

        public readonly ITreeNode<T> GetNodeById(T key) => NodeSet.ContainsKey(key) ? NodeSet[key] : throw new("No such Node");
        public readonly List<ITreeNode<T>> GetSibling(T Node)
        {
            List<ITreeNode<T>> lt = new();
            foreach(var i in GetNodeById(GetNodeById(Node).ParentName ?? Root.Id)) lt.Add(GetNodeById(i));
            return lt;
        }
        public readonly ITreeNode<T> GetParent(T Node) => GetNodeById(GetNodeById(Node).ParentName ?? Root.Id);
        public readonly List<ITreeNode<T>> GetDescendants(T Node)
        {
            List<ITreeNode<T>> lt = new();
            foreach (var i in GetNodeById(Node)) lt.Add(GetNodeById(i));
            return lt;
        }
    }
}
