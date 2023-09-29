namespace Meow.Math.Graph.ErrorList
{
    /// <summary>
    /// 节点遍历不可达错误<br/>
    /// when you try to traversal a node to node path, if this occurs represent that end node is unreachable.
    /// </summary>
    public class NodeUnreachableException : Exception
    {
        /// <inheritdoc/>
        public override string Message => "The node you try to navigate is not reachable";
    }

    /// <summary>
    /// 节点不存在错误<br/>
    /// this occurs represent that node is Not in Graph Node Set.
    /// </summary>
    public class NodeNotExistException : ArgumentException
    {
        /// <inheritdoc/>
        public override string Message => "The Node is not exist in Graph";
    }

    /// <summary>
    /// 边创建错误:若权重为0必须起始一致<br/>
    /// </summary>
    public class EdgeCreateWeightZeroException : ArgumentException
    {
        /// <inheritdoc/>
        public override string Message => "When The Edge Weight is `0` Start Must Equals End";
    }

    /// <summary>
    /// 贝尔曼福德算法负权环检出<br/>BellmanFord Algorithm Negative Weight Cycle Detected
    /// </summary>
    public class BFANWCDetectedException : ArgumentException
    {
        /// <inheritdoc/>
        public override string Message => "Graph contains negative weight cycle.";
    }
}
