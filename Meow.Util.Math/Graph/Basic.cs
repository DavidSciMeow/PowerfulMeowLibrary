using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Util.Math.Graph
{


    /// <summary>
    /// 一般节点
    /// </summary>
    /// <typeparam name="T">节点类型</typeparam>
    public struct Node<T> : IEnumerable<KeyValuePair<T, int>>, IEquatable<Node<T>> where T : IEquatable<T>
    {
        public readonly T Id;
        public Dictionary<T, int> LinkSet { get; private set; } = new();
        bool isSorted = false; // 当前节点的链接节点是否已排序
        public readonly int Count => LinkSet.Count; //出度
        public Node(T Id) => this.Id = Id;
        public readonly T this[int id] => LinkSet.ToArray()[id].Key;
        //取节点的链接节点权重 σ(1)
        public readonly int GetWeight(T id) => LinkSet[id];
        public readonly bool TryGetWeight(T id, out int w) => LinkSet.TryGetValue(id, out w);
        //获取某个节点(按顺序)
        public T GetShortestVet(int id)
        {
            if (!isSorted) SortNode();
            return LinkSet.ToArray()[id].Key;
        }

        //链接节点 σ(1)
        public bool LinkTo(T nodename, int weight = 1)
        {
            if(LinkSet.TryAdd(nodename, weight))
            {
                isSorted = false;
                return true;
            }
            else
            {
                return false;
            }
        }
        //移除节点 σ(1)
        public bool RemoveLink(T nodename)
        {
            if (LinkSet.Remove(nodename))
            {
                isSorted = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        //小端点枚举[快排/堆排] O(n ~ nlogn ~ n^2)
        //发生无向图的时候进行一般设定排序, 不添加更改节点时不进行排序
        //节点排序
        public void SortNode()
        {
            List<KeyValuePair<T, int>> lst = LinkSet.ToList();
            lst.Sort((a, b) => a.Value.CompareTo(b.Value));
            LinkSet = lst.ToDictionary(v => v.Key, v => v.Value);
            isSorted = true;
        }

        //接口&基础实现定义
        public IEnumerator<KeyValuePair<T, int>> GetEnumerator()
        {
            if(!isSorted) SortNode();
            return LinkSet.AsEnumerable().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Node<T> other && Equals(other);
        public override readonly int GetHashCode() => Id.GetHashCode();
        public override readonly string ToString() => $"[Node:{Id}]";
        public readonly bool Equals(Node<T> other) => Id.Equals(other.Id);
        public static bool operator ==(Node<T> left, Node<T> right) => left.Equals(right);
        public static bool operator !=(Node<T> left, Node<T> right) => !(left.Equals(right));
    }

    /// <summary>
    /// 图
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BGraph<T> where T : IEquatable<T>
    {
        public string? GraphName;
        public Dictionary<T, Node<T>> NodeSet = new();

        public void Insert(Node<T> node) => NodeSet.Add(node.Id, node);
        public void Insert(T nodeid) => NodeSet.Add(nodeid, new Node<T>(nodeid));
        public void Insert(params T[] nodes)
        {
            foreach(var node in nodes)
            {
                NodeSet.Add(node, new Node<T>(node));
            }
        }
        public void Insert(params Node<T>[] nodes)
        {
            foreach(var node in nodes)
            {
                NodeSet.Add(node.Id, node);
            }
        }
        public bool RemoveById(T node) => NodeSet.Remove(node);
        public Node<T> this[T id] => NodeSet[id];
        public Node<T> GetNodeById(T nodeid) => NodeSet.TryGetValue(nodeid, out var o) ? o : throw new($"[{typeof(T)}:{nodeid}] node is not Exist in set");
        public bool ExistNodeById(T nodeid) => NodeSet.ContainsKey(nodeid);
        public void Link(T nodeA, T nodeB, int weight = 1)
        {
            _ = NodeSet.TryGetValue(nodeA, out var _NodeA) ? _NodeA : throw new($"node [{nodeA}] is not in Set");
            _ = NodeSet.TryGetValue(nodeB, out var _NodeB) ? _NodeB : throw new($"node [{nodeB}] is not in Set");
            _NodeA.LinkTo(nodeB, weight);
            _NodeB.LinkTo(nodeA, weight);
        }
        public void LinkTo(T nodeA, T nodeB, int weight = 1)
        {
            _ = NodeSet.TryGetValue(nodeA, out var _NodeA) ? _NodeA : throw new($"node [{nodeA}] is not in Set");
            _ = NodeSet.TryGetValue(nodeB, out var _NodeB) ? _NodeB : throw new($"node [{nodeB}] is not in Set");
            _NodeA.LinkTo(nodeB, weight);
        }
        /// <summary>
        /// DFS + 回溯算法 
        /// </summary>
        /// <param name="node">出发点</param>
        /// <returns></returns>
        public List<NodePath<T>> TrackFrom(T node)
        {
            var _node = GetNodeById(node);
            var d = new Dictionary<T, int>();
            var _tlist = new List<NodePath<T>>();
            var lst = new NodePath<T>();
            int i = 0;
            int weight = 0;
            while (true) //遍历算法
            {
                bool _end = false;
                while (true) //单路径寻找
                {
                    if (d.TryAdd(_node.Id, i)) //未访问过
                    {
                        lst.Add(_node.Id);
                        while (true) //移动函数
                        {
                            var _next = _node[i];
                            if (!d.ContainsKey(_next))
                            {
                                weight += _node.GetWeight(_next); //获取路径移动权值
                                _node = GetNodeById(_next); //移动节点
                                i = 0; //恢复选择
                                break;
                            }

                            i++; //步进
                            d[_node.Id] = i; //存入步进位置
                            if (i >= _node.Count) //若没有可选路径
                            {
                                _end = true; //路径移动结束
                                break;
                            }
                        }

                    }

                    if (_end)
                    {
                        d.Remove(_node.Id);
                        lst.PathWeight = weight;//存储权值
                        _tlist.Add(lst.GetClone());//存储一条路径
                        lst.Remove(lst[^1]);//删除当前最后节点
                        break;
                    }
                }

                while (true) //回溯函数 (条件:上翻但不折出)
                {
                    if (d.Count > 0) //可以前移
                    {
                        var _pre = d.Last().Key;
                        var _mw = GetNodeById(_pre).GetWeight(_node.Id);//获取回溯路径权值
                        weight -= _mw;//恢复权值
                        _node = GetNodeById(_pre);//前移一个节点
                        i = d[d.Last().Key] + 1;
                        d.Remove(_pre);
                        lst.Remove(_pre);

                        if (i < _node.Count) //前移合适
                        {
                            break;
                        }
                    }
                    else //无可用路径
                    {
                        break;
                    }
                }
                

                if (d.Count <= 0) break; //没有可供回溯的点程序结束
            }
            return _tlist;
        }


        public void InteractiveTrack(T start, T end)
        {
            DateTime _st = DateTime.Now;
            List<NodePath<T>> tlist = TrackFrom(start);
            DateTime _et = DateTime.Now;
            Console.WriteLine("-----所有路径-----");
            Console.WriteLine($"权重 \t| 路径");
            Console.WriteLine("------------------");
            tlist.Sort();
            foreach (var t in tlist)
            {
                Console.WriteLine($"{t.PathWeight} \t| {t}");
            }
            Console.WriteLine("-----起始点终点路径-----");
            bool a = false;
            List<NodePath<T>> slist = new();
            foreach (var t in tlist)
            {
                if (t.End.Equals(end))
                {
                    a = true;
                    slist.Add(t);
                    Console.WriteLine($"{t.PathWeight} \t| {t}");
                }
            }
            Console.WriteLine($"{(!a ? ((end.Equals(start)) ? "起始点与终点相同" : "不可达") : $"-----最短路径-----\n{slist[0].PathWeight}\t| {slist[0]}")}");
            Console.WriteLine();
            Console.WriteLine($"图节点数:{NodeSet.Count}\n图路径数(源点):{tlist.Count}\n回溯遍历用时:{_et-_st}");
        }
    }

    public struct NodePath<T> : IComparable<NodePath<T>> where T : IEquatable<T>
    {
        List<T> Nodes { get; init; }
        public int PathWeight { get; set; }
        public NodePath()
        {
            Nodes = new List<T>();
            PathWeight = 0;
        }
        public NodePath(List<T> array, int weight)
        {
            Nodes = array;
            PathWeight = weight;
        }
        public readonly int Count => Nodes.Count;
        public readonly T Start => Nodes[0];
        public readonly T End => Nodes[^1];

        public readonly void Add(T node) => Nodes.Add(node);
        public readonly bool Remove(T node) => Nodes.Remove(node);
        public readonly T this[int index] => Nodes[index];
        public readonly T[] ToArray() => Nodes.ToArray();
        public override string ToString()
        {
            bool t = false;
            StringBuilder sb = new();
            foreach (var node in Nodes)
            {
                sb.Append($"{(t ? "->" : "")}{node}");
                t = true;
            }
            return sb.ToString();
        }
        public readonly NodePath<T> GetClone()
        {
            var clone = new NodePath<T>();
            foreach(var node in Nodes)
            {
                clone.Add(node);
            }
            clone.PathWeight = PathWeight;
            return clone;
        }
        public readonly int CompareTo(NodePath<T> other)
        {
            if (other is NodePath<T> i)
            {
                return PathWeight.CompareTo(i.PathWeight);
            }
            throw new ArgumentException($"entity is null");
        }
    }
}


