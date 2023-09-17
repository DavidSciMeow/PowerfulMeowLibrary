namespace Meow.Math.Graph.Interface
{
    /// <summary>
    /// 可遍历节点接口<br/>Traversable Node Interface
    /// </summary>
    /// <typeparam name="T">节点类型<br/>Node Struct</typeparam>
    public interface ITraversable<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 删除一个连接节点<br/>Delete A Node
        /// <para>时间复杂度(Time Complexity) :: <i><b><see langword="O(1) ~ O(n)" /></b></i></para>
        /// </summary>
        /// <param name="node">节点名<br/>NodeName</param>
        public bool Delete(T node);
        /// <summary>
        /// 判定一个节点是否连接<br/>Determin A Node Link Or Not
        /// <para>时间复杂度(Time Complexity) :: <i><b><see langword="O(1) ~ O(n)" /></b></i></para>
        /// </summary>
        /// <param name="node">节点识别名<br/>NodeName</param>
        public bool Exist(T node);
    }

    /// <summary>
    /// 树节点接口<br/>Tree Node Interface
    /// </summary>
    /// <typeparam name="T">节点类型<br/>Node Struct</typeparam>
    public interface ITreeNode<T> : ITraversable<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 添加一个连接节点(朝表的后部)<br/>Link A Node To List's Rear Side
        /// <para>时间复杂度(Time Complexity) :: <i><b><see langword="O(n)" /></b></i></para>
        /// </summary>
        /// <param name="node">节点<br/>Node</param>
        public void AddRear(T node);
        /// <summary>
        /// 添加一个连接节点(朝表的前部)<br/>Link A Node To List's Front Side
        /// <para>时间复杂度(Time Complexity) :: <i><b><see langword="O(n)" /></b></i></para>
        /// </summary>
        /// <param name="node">节点<br/>Node</param>
        public void AddFront(T node);
    }

    /// <summary>
    /// 图节点接口<br/>Map Node Interface
    /// </summary>
    /// <typeparam name="T">节点类型<br/>Node Struct</typeparam>
    public interface IMapNode<T> : ITraversable<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 添加一个连接节点(朝哈希表内)<br/>Link A Node into HashSet
        /// <para>时间复杂度(Time Complexity) :: <i><b><see langword="O(1)" /></b></i></para>
        /// </summary>
        /// <param name="node">节点<br/>Node</param>
        public bool Add(T node, int weight);
    }
}
