using Meow.Math.Graph.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Math.Graph.Struct
{
    public struct Tree<T> : ITree<T>, IEnumerable<KeyValuePair<T, ITreeNode<T>>> where T : IEquatable<T>
    {
        public Tree(ITreeNode<T> root) => Root = root;
        public Tree(T root) => Root = new TreeNode<T>(root);
        public IDictionary<T, ITreeNode<T>> NodeSet { get; } = new Dictionary<T, ITreeNode<T>>();
        public ITreeNode<T> Root { get; set; }
        public readonly ITreeNode<T> this[T key] => NodeSet[key];

        public readonly void Add(T Node, ITreeNode<T>? RootBy = default)
        {
            RootBy ??= Root;
            var _node = new TreeNode<T>(Node)
            {
                ParentName = RootBy.Id
            };
            RootBy.ChildsName.Add(_node.Id);
        }
        public readonly void Add(T Node, T? RootBy = default)
        {
            RootBy ??= Root.Id;
            var _node = new TreeNode<T>(Node)
            {
                ParentName = RootBy
            };
            this[RootBy].ChildsName.Add(_node.Id);
        }

        public readonly IEnumerator<KeyValuePair<T, ITreeNode<T>>> GetEnumerator() => NodeSet.GetEnumerator();
        readonly IEnumerator IEnumerable.GetEnumerator() => NodeSet.GetEnumerator();

        public override readonly string ToString()
        {
            StringBuilder sb = new();
            T? _node = Root.Id;
            Stack<T?> q = new();
            q.Push(Root.Id);
            while(q.Any())
            {
                if (_node != null)
                {
                    if (q.TryPop(out var trace)) //不存在，或无效的回溯点
                    { 
                        q.Push(trace); //压栈空节点指针;
                    } 
                    else
                    {
                        q.Push(default);//压栈当前节点指针，同时记录下一个回溯点位置;
                    }
                    if (trace?.Equals(Root) ?? false) //回溯点位置索引为 0
                    {
                        //输出层次缩进、画路径，打印节点内容;
                    }
                    _node = 
                    //进入下一层;
                }
            }
            return sb.ToString();
        }
    }
}
