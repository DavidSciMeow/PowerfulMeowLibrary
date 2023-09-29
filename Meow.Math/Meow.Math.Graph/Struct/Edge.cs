using Meow.Math.Graph.ErrorList;

namespace Meow.Math.Graph.Struct
{
    /// <summary>
    /// 一条以<typeparamref name="T"/>为类型的边<br/>
    /// a Edge Define As Type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">边的类型<br/>Edge Type</typeparam>
    public struct Edge<T> : IEquatable<Edge<T>> where T : IEquatable<T>
    {
        /// <summary>
        /// 边起始点<br/>Edge Start Node
        /// </summary>
        public T Start;
        /// <summary>
        /// 边终点<br/>Edge End Node
        /// </summary>
        public T End;
        /// <summary>
        /// 边权重<br/>Edge Weight
        /// </summary>
        public int Weight = 1;
        /// <summary>
        /// 边是否具有方向<br/>Edge is directional
        /// </summary>
        public bool isDirectional = false;

        /// <summary>
        /// 创建一个边<br/>Create A Edge
        /// </summary>
        /// <param name="start">边起始点<br/>Edge Start Node</param>
        /// <param name="end">边终点<br/>Edge End Node</param>
        /// <param name="weight">边权重<br/>Edge Weight</param>
        /// <param name="isDirectional">边是否含有方向<br/>Edge is Directional or not</param>
        /// <exception cref="EdgeCreateWeightZeroException"></exception>
        public Edge(T start, T end, int weight = 1, bool isDirectional = false)
        {
            Start = start;
            End = end;
            Weight = weight;
            if (Weight == 0 && !Start.Equals(End)) throw new EdgeCreateWeightZeroException();
            this.isDirectional = isDirectional;
        }

        /// <inheritdoc/>
        public override readonly string ToString() => $"{Start}{(isDirectional ? $"-{Weight}>" : $"-{Weight}-")}{End}";
        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Edge<T> edge && Equals(edge);
        /// <inheritdoc/>
        public bool Equals(Edge<T> other) => isDirectional
                ? (Start.Equals(other.Start) && End.Equals(other.End)) || (Start.Equals(other.End) && End.Equals(other.Start))
                : Start.Equals(other.Start) && End.Equals(other.End);
        /// <inheritdoc/>
        public override readonly int GetHashCode() => HashCode.Combine(Start, End);
        /// <inheritdoc/>
        public static bool operator ==(Edge<T> left, Edge<T> right) => left.Equals(right);
        /// <inheritdoc/>
        public static bool operator !=(Edge<T> left, Edge<T> right) => !(left == right);
        /// <inheritdoc/>
        public readonly void Deconstruct(out T u, out T v, out int w, out bool IsDirectional)
        {
            u = Start;
            v = End;
            w = Weight;
            IsDirectional = isDirectional;
        }
        /// <inheritdoc/>
        public readonly void Deconstruct(out T u, out T v, out int w)
        {
            u = Start;
            v = End;
            w = Weight;
        }
    }

}
