using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Meow.Util.Math.Graph
{
    public struct SNode : IEquatable<SNode>
    {
        public string Name;
        readonly object _lock = new();
        public List<SNode> Nodes = new();
        public List<int> NodeHopList = new();

        public SNode(string name) => Name = name;
        public readonly int NodeCount() => Nodes.Count;
        public readonly SNode InsertNode(SNode node, int weight = 1)
        {
            lock (_lock)
            {
                Nodes.Add(node);
                NodeHopList.Add(weight);
            }
            return this;
        }
        public readonly SNode RemoveNode(SNode node)
        {
            lock (_lock)
            {
                var i = Nodes.FindIndex(n => n == node);
                Nodes.RemoveAt(i);
                NodeHopList.RemoveAt(i);
            }
            return this;
        }
        public readonly SNode AltNode(SNode node, int weight)
        {
            lock (_lock)
            {
                var i = Nodes.FindIndex(n => n == node);
                NodeHopList[i] = weight;
            }
            return this;
        }
        public readonly SNode To(params SNode[] node)
        {
            foreach (var item in node)
            {
                Nodes.Add(item);
                NodeHopList.Add(1);
            }
            return this;
        }
        public readonly SNode From(params SNode[] node)
        {
            foreach (var item in node)
            {
                Nodes.Add(item);
                NodeHopList.Add(-1);
            }
            return this;
        }

        public readonly SNode GetNextNearest(int k = 0) => Nodes[k + MinIndexAboveZero(NodeHopList.ToArray()[k..])];
        public readonly SNode GetReverseNearest(int k = 0) => Nodes[k + MinIndexMinusZero(NodeHopList.ToArray()[k..])];

        //最小队列比较时间 σ(1) 最大队列比较时间 σ(n-1)
        static int MinIndexAboveZero(int[] arr)
        {
            var i_Pos = 0;
            var value = arr[0];
            for (var i = 1; i < arr.Length; ++i)
            {
                if (arr[i] > 0)
                {
                    var _value = arr[i];
                    if (_value < value)
                    {
                        value = _value;
                        i_Pos = i;
                    }
                }
            }
            return i_Pos;
        }
        static int MinIndexMinusZero(int[] arr)
        {
            var i_Pos = 0;
            var value = arr[0];
            for (var i = 1; i < arr.Length; ++i)
            {
                if (arr[i] < 0)
                {
                    var _value = arr[i];
                    if (_value < value)
                    {
                        value = _value;
                        i_Pos = i;
                    }
                }
            }
            return i_Pos;
        }

        public readonly bool Equals(SNode other) => other.Name.Equals(Name);
        public override readonly int GetHashCode() => Name.GetHashCode();
        public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is SNode n && n.Equals(this);
        public override readonly string ToString() => Name;

        public static bool operator ==(SNode left, SNode right) => left.Equals(right);
        public static bool operator !=(SNode left, SNode right) => !(left == right);
    }
    public class DMap<T> where T : notnull
    {
        public long TotalSearchTimes = 1;
        public Dictionary<T, SNode> nl = new();
        //哈希创建节点 σ(1)
        public bool CreateSNode(T sid, SNode node) => nl.TryAdd(sid, node);
        //哈希获取节点 σ(1)
        public bool GetNode(T sid, out SNode a) => nl.TryGetValue(sid, out a) ? true : throw new("无本节点id, 请检查定义");
        //哈希设置节点 σ(2)
        public bool SetNodeProperties(T sid, T nodelink, int weight = 1)
        {
            if (GetNode(sid, out SNode val))
            {
                if (GetNode(nodelink, out SNode val2))
                {
                    val.InsertNode(val2, weight);
                    return true;
                }
            }
            return false;
        }
        //哈希删除节点 σ(2)
        public bool RemoveNodeProperties(T sid, T nodelink)
        {
            if (GetNode(sid, out SNode val))
            {
                if (GetNode(nodelink, out SNode val2))
                {
                    val.RemoveNode(val2);
                    return true;
                }
            }
            return false;
        }
        //打印路径
        public string PrintRoute(SNode[]? l)
        {
            if (l == null) return "No Route Found";
            StringBuilder sb = new();
            bool a = false;
            foreach (SNode s in l)
            {
                sb.Append($"{((a == true) ? " -> " : "")}");
                a = true;
                sb.Append(s);
            }
            return sb.ToString();
        }
        //最小全列表时间 σ(4n*(n-1)) 最大全列表时间 σ(n^2*2n*(n-1))
        public (SNode[] shortest, HashSet<SNode[]> AllNodePossible) RCS(SNode start, SNode end)
        {
            if (start == end) throw new("首尾一致");
            var lst = FS(start, end);//默认正向搜索
            if (lst == null) throw new("无最短路径");//无最短路径
            var alist = new HashSet<SNode[]>(new SNodeListEqualityComparer()) { lst };
            for (int i = 0; i < start.NodeCount(); i++)
            {
                var temp = FS(start, end, i);//增加版本正向搜索
                if (temp != null) alist.Add(temp);
                if (lst.Length > temp?.Length) lst = temp;
                temp = RS(start, end, i);//增加版本反向搜索
                if (temp != null) alist.Add(temp);
                if (lst.Length > temp?.Length) lst = temp;

            }
            return (lst, alist);
        }
        //最小搜索时间 σ(n*2) 最大搜索时间 σ(2n*(n-1))
        public SNode[]? FS(SNode start, SNode end, int k = 0)
        {
            var dk = k;
            List<SNode> list = new() { start }; //添加头节点
            while (true)
            {
                TotalSearchTimes++;
                start = start.GetNextNearest(k); //获得前置最小权重节点
                if (list.Contains(start)) //已含有节点(回头)
                {
                    list.RemoveAt(list.Count - 1);//删除错误节点
                    start = list[^1];//回滚版本
                    k++;//增加版本
                }
                else
                {
                    list.Add(start);//节点合适, 添加节点
                    k = dk;//回滚版本置初始值
                }
                if (end == start) break; //判定到达终点
            }
            return list.ToArray();
        }
        //最小搜索时间 σ(n*2) 最大搜索时间 σ(2n*(n-1))
        public SNode[]? RS(SNode start, SNode end, int k = 0)
        {
            var dk = k;
            List<SNode> list = new() { start }; //添加头节点
            while (true)
            {
                TotalSearchTimes++;
                start = start.GetReverseNearest(k); //获得后置最小权重节点
                if (list.Contains(start)) //已含有节点(回头)
                {
                    list.RemoveAt(list.Count - 1);//删除错误节点
                    start = list[^1];//回滚版本
                    k++;//增加版本
                }
                else
                {
                    list.Add(start);//节点合适, 添加节点
                    k = dk;//回滚版本置初始值
                }
                if (end == start) break; //判定到达终点
            }
            return list.ToArray();
        }
        //寻找最短路径
        public string FindShortest(T nodeStart, T nodeEnd)
        {
            StringBuilder sb = new();
            DateTime dts = DateTime.Now;
            try
            {
                GetNode(nodeStart, out var start);
                GetNode(nodeEnd, out var end);
                var (shortest, AllNodePossible) = RCS(start, end);
                DateTime dte = DateTime.Now;
                sb.AppendLine("最短距离:");
                sb.AppendLine(PrintRoute(shortest));
                sb.AppendLine("所有路线:");
                foreach (var item in AllNodePossible)
                {
                    sb.AppendLine(PrintRoute(item));
                }
                sb.AppendLine();
                sb.AppendLine($"总耗时:{(dte - dts).TotalSeconds:f8}秒");
                sb.AppendLine($"总路线数:{AllNodePossible?.Count + 1}");
                sb.AppendLine($"总数组访问次数:{TotalSearchTimes}");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                DateTime dte = DateTime.Now;
                sb.AppendLine($"总耗时:{(dte - dts).TotalSeconds:f8}秒");
                sb.AppendLine($"{ex.Message}");
                return sb.ToString();
            }

        }
        //读取文件 O(n) 行数为基准
        public static void ReadFileMap(DMap<string> DS, string filepath)
        {
            try
            {
                var lk = File.ReadAllLines(Path.Combine(filepath.Replace(" ", "")), Encoding.UTF8);
                bool setproperties = false;
                foreach (var l in lk)
                {

                    if (l.Contains("***"))
                    {
                        setproperties = true;
                        continue;
                    }

                    if (setproperties)
                    {
                        var splitor = ",";
                        var p = l.Split(splitor);
                        var from = p[0].Replace(splitor, "");
                        var to = p[1].Replace(splitor, "");
                        if (p.Length > 2)
                        {
                            var w = int.Parse(p[2].Replace(splitor, ""));
                            DS.SetNodeProperties(from, to, w);
                        }
                        else
                        {
                            DS.SetNodeProperties(from, to, 1);
                        }
                    }
                    else
                    {
                        var p = l.Split(' ');
                        DS.CreateSNode(p[0], new(p[1]));
                    }
                }
                Console.WriteLine("图已读取");
            }
            catch
            {
                Console.WriteLine("文件不存在");
            }

        }

    }
    public sealed class SNodeListEqualityComparer : IEqualityComparer<SNode[]>
    {
        public bool Equals(SNode[]? x, SNode[]? y)
        {
            if (x == null && y == null) return false;
            if (x?.Length != y?.Length) return false;
            for (int i = 0; i < (x?.Length ?? 0); i++)
            {
                if (x?[i] != y?[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode([DisallowNull] SNode[] obj)
        {
            int k = 0;
            foreach (SNode x in obj)
            {
                k ^= x.GetHashCode();
            }
            return k;
        }
    }
}
