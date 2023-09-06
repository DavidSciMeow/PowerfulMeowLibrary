using Meow.Math.Graph.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Math.Graph.Struct
{
    public struct TreeNode<T> : ITreeNode<T>, IEnumerable<T> where T : IEquatable<T>
    {
        public TreeNode(T id) => Id = id;
        public TreeNode(T id, params T[] childs)
        {
            Id = id;
            foreach(var child in childs) ChildsName.Add(child);
        }

        public T Id { get; set; }
        public T? ParentName { get; set; } = default;
        public ISet<T> ChildsName { get; set; } = new HashSet<T>();

        public readonly bool IsRoot { get => ParentName == null; } 
        public readonly bool IsLeaf { get => ChildsName.Count == 0; }

        public readonly IEnumerator<T> GetEnumerator() => ChildsName.GetEnumerator();
        readonly IEnumerator IEnumerable.GetEnumerator() => ChildsName.GetEnumerator();
        public override readonly bool Equals(object? obj) => obj is TreeNode<T> node && Id.Equals(node.Id);
        public override readonly int GetHashCode() => HashCode.Combine(Id, ParentName, ChildsName, IsRoot, IsLeaf);

        public static bool operator ==(TreeNode<T> left, TreeNode<T> right) => left.Equals(right);
        public static bool operator !=(TreeNode<T> left, TreeNode<T> right) => !(left == right);
    }
}
