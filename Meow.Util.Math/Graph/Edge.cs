using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Util.Math.Graph
{
    public struct Edge<T> : IEquatable<Edge<T>> where T : IEquatable<T>
    {
        /// <summary>
        /// 起点节点
        /// </summary>
        public T Start;
        /// <summary>
        /// 终点节点
        /// </summary>
        public T End;
        /// <summary>
        /// 权重
        /// </summary>
        public int Weight = 1;

        public Edge(T start, T end, int weight)
        {
            Start = start;
            End = end;
            Weight = weight;
        }
        public Edge(T start, T end)
        {
            Start = start;
            End = end;
            Weight = 1;
        }

        /// <summary>
        /// 合并头尾相连边为路径
        /// </summary>
        /// <param name="left">左操作边</param>
        /// <param name="right">右操作边</param>
        /// <returns>合并的路径</returns>
        public static NodePath<T>? Combine(Edge<T> left, Edge<T> right)
        {
            if (left.End.Equals(right.End))
            {
                var p = new NodePath<T>
                {
                    left.Start,
                    left.End,
                    right.Start
                };
                p.PathWeight = left.Weight + right.Weight;
                return p;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 路径中添加边
        /// </summary>
        /// <param name="left">路径</param>
        /// <param name="right">边</param>
        /// <returns>添加后的路径</returns>
        public static NodePath<T>? Combine(NodePath<T> left, Edge<T> right)
        {
            if(left.Count == 0) return new NodePath<T>() { right.Start, right.End };
            if (left.End.Equals(right.Start))
            {
                var p = new NodePath<T>();
                foreach (var i in left)
                {
                    p.Add(i);
                }
                p.PathWeight = left.PathWeight + right.Weight;
                return p;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 从后往前移除节点路径
        /// </summary>
        /// <param name="left">路径</param>
        /// <param name="right">要移除的边</param>
        /// <returns>移除后的路径</returns>
        public static NodePath<T>? RemoveLastFrom(NodePath<T> left, Edge<T> right)
        {
            if (left.End.Equals(right.End))
            {
                var p = new NodePath<T>();
                foreach (var i in left)
                {
                    if (!i.Equals(right.End))
                    {
                        p.Add(i);
                    }
                }
                p.PathWeight = left.PathWeight - right.Weight;
                return p;
            }
            else
            {
                return null;
            }
        }


        /// <inheritdoc/>
        public bool Equals(Edge<T> other) => Start.Equals(other.Start) && End.Equals(other.End);
        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Edge<T> edge && Equals(edge);
        /// <inheritdoc/>
        public static NodePath<T>? operator +(Edge<T> left, Edge<T> right) => Combine(left, right);
        /// <inheritdoc/>
        public static NodePath<T>? operator +(NodePath<T> left, Edge<T> right) => Combine(left, right);
        /// <inheritdoc/>
        public static NodePath<T>? operator -(NodePath<T> left, Edge<T> right) => RemoveLastFrom(left, right);
        /// <inheritdoc/>
        public static bool operator ==(Edge<T> left, Edge<T> right) => left.Equals(right);
        /// <inheritdoc/>
        public static bool operator !=(Edge<T> left, Edge<T> right) => !(left == right);
        /// <inheritdoc/>
        public override readonly int GetHashCode() => Start.GetHashCode() ^ End.GetHashCode();
        /// <inheritdoc/>
        public override readonly string ToString() => $"{Start}-({Weight})->{End}";
    }
}
