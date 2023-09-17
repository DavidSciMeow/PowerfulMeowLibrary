namespace Meow.Math.Graph.ErrorList
{
    /// <summary>
    /// 节点遍历不可达错误<br/>
    /// when you try to traversal a node to node path, if this occurs represent that end node is unreachable.
    /// </summary>
    public class NodeUnreachableException : Exception
    {
        public override string Message => "The node you try to navigate is not reachable";
    }

    /// <summary>
    /// 节点不存在错误<br/>
    /// this occurs represent that node is Not in Graph Node Set.
    /// </summary>
    public class NodeNotExistException : ArgumentException
    {
        public override string Message => "The Node is not exist in Graph";
    }
}
