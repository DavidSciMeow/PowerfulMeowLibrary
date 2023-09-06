using Meow.Math.Graph.Class;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Math.Graph.Interface
{
    /// <summary>
    /// 图节点
    /// </summary>
    /// <typeparam name="T">图节点类型(必须可比较是否一致)</typeparam>
    public interface IMapNode<T>: IEnumerable<KeyValuePair<T, double>>, IEquatable<IMapNode<T>> where T : IEquatable<T>
    {
        /// <summary>
        /// 节点唯一识别码
        /// </summary>
        T Id { get; }
        /// <summary>
        /// 内部维护邻接表
        /// </summary>
        IDictionary<T, double> LinkSet { get; }
        /// <summary>
        /// 节点的出度 σ(1)
        /// </summary>
        double Count { get; }
        /// <summary>
        /// 测试邻接表是否存在资源 σ(1)
        /// </summary>
        /// <returns>存在资源则为true否则为false</returns>
        bool Any();
        /// <summary>
        /// 按顺序取邻接节点 σ(n) + σ(1)
        /// </summary>
        /// <param name="pos">在邻接表的顺序</param>
        /// <returns>邻接的节点</returns>
        T this[int pos] { get; }
        /// <summary>
        /// 取链接的节点权重 σ(1)
        /// </summary>
        /// <param name="id">节点识别码</param>
        /// <returns>权重</returns>
        double this[T id] { get; }
        /// <summary>
        /// 判定是否链接到某邻接节点 σ(1)
        /// </summary>
        /// <param name="Id">某节点</param>
        /// <returns>链接则返回true,否则返回false</returns>
        bool IsConnectTo(T Id);
        /// <summary>
        /// 取链接的节点权重 σ(1)
        /// </summary>
        /// <param name="id">节点识别码</param>
        /// <returns>权重</returns>
        double GetWeight(T id);
        /// <summary>
        /// 链接节点 σ(1)
        /// </summary>
        /// <param name="nodename">连接到</param>
        /// <param name="weight">权重</param>
        /// <returns>是否链接成功</returns>
        bool LinkTo(T nodename, double weight = 1);
        /// <summary>
        /// 移除节点 σ(1)
        /// </summary>
        /// <param name="nodename">节点识别码</param>
        /// <returns>是否移除成功</returns>
        bool RemoveLink(T nodename);
        /// <summary>
        /// 尝试取链接的节点权重 σ(1)
        /// </summary>
        /// <param name="id">节点识别码</param>
        /// <param name="weight">权重</param>
        /// <returns>是否含有节点</returns>
        bool TryGetWeight(T id, out double weight);
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        IEnumerator IEnumerable.GetEnumerator() => LinkSet.GetEnumerator();
    }
    /// <summary>
    /// 图的边
    /// </summary>
    /// <typeparam name="T">图数据类型</typeparam>
    public interface IMapEdge<T> : IEquatable<IMapEdge<T>> where T : IEquatable<T> 
    {
        /// <summary>
        /// 边起始点
        /// </summary>
        T Start { get; set; }
        /// <summary>
        /// 边结束点
        /// </summary>
        T End { get; set; }
        /// <summary>
        /// 边权重
        /// </summary>
        double Weight { get; set; }
    }
    /// <summary>
    /// 图结构
    /// </summary>
    /// <typeparam name="T">图节点类型</typeparam>
    public interface IBGraph<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 图名称
        /// </summary>
        string GraphName { get; set; }
        /// <summary>
        /// 图邻接表
        /// </summary>
        IDictionary<T, IMapNode<T>> NodeList { get; }
        /// <summary>
        /// 边表
        /// </summary>
        ISet<IMapEdge<T>> EdgeList { get; }
        /// <summary>
        /// 节点数
        /// </summary>
        int NodeCount { get; }
        /// <summary>
        /// 连通分量
        /// </summary>
        int CC { get; }

        /// <summary>
        /// 增加某个节点 σ(1)
        /// </summary>
        /// <param name="node">节点Id</param>
        /// <returns>是否成功添加</returns>
        bool Insert(IMapNode<T> node);
        /// <summary>
        /// 增加某个节点 σ(1)
        /// </summary>
        /// <param name="node">节点Id</param>
        /// <returns>是否成功添加</returns>
        bool Insert(T node);
        /// <summary>
        /// 添加所有列表内节点 σ(1) * n
        /// </summary>
        /// <param name="nodes">节点列表</param>
        void Insert(params T[] nodes);
        /// <summary>
        /// 添加所有列表内节点 σ(1) * n
        /// </summary>
        /// <param name="nodes">节点列表</param>
        void Insert(params IMapNode<T>[] nodes);
        /// <summary>
        /// 删除某个节点 σ(1)
        /// </summary>
        /// <param name="node">节点Id</param>
        /// <returns>是否删除</returns>
        bool RemoveById(T node);
        /// <summary>
        /// 获取某个节点 σ(1)
        /// </summary>
        /// <param name="nodeid">节点Id</param>
        /// <returns>获取到的节点</returns>
        IMapNode<T> GetNodeById(T nodeid);
        /// <summary>
        /// 查询是否存在节点 σ(1)
        /// </summary>
        /// <param name="nodeid">节点Id</param>
        /// <returns>是否存在</returns>
        bool ExistNodeById(T nodeid);
        /// <summary>
        /// 链接 (双向)
        /// </summary>
        /// <param name="nodeA">节点A</param>
        /// <param name="nodeB">节点B</param>
        /// <param name="weight">权重(默认为1)</param>
        void Link(T nodeA, T nodeB, double weight = 1);
        /// <summary>
        /// 链接到(单向)
        /// </summary>
        /// <param name="nodeA">节点A</param>
        /// <param name="nodeB">节点B</param>
        /// <param name="weight">权重(默认为1)</param>
        void LinkTo(T nodeA, T nodeB, double weight = 1);
        /// <summary>
        /// 获取某个节点 σ(1)
        /// </summary>
        /// <param name="id">节点Id</param>
        /// <returns>获取到的节点</returns>
        IMapNode<T> this[T id] { get; }
        /// <summary>
        /// 是否含有路径 σ(n*k,1)
        /// </summary>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        /// <returns></returns>
        bool IsConn(T start, T end);

        /// <summary>
        /// 获得所有以此节点开始的边
        /// </summary>
        /// <param name="node">
        /// 节点
        /// </param>
        /// <returns>
        /// 符合的节点路径
        /// </returns>
        IMapEdge<T>[] GetEdgeByNodeStart(T node);
        /// <summary>
        /// 获得一个明确的边
        /// </summary>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        /// <returns></returns>
        IMapEdge<T>? GetEdge(T start, T end);
        /// <summary>
        /// 是否含有某边
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        bool ExistEdge(T start, T end);
        /// <summary>
        /// 还原一个有方向的边
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        bool RestoreEdge(T start, T end, double weight = 1);
        /// <summary>
        /// 还原一个无方向的边
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        bool RestoreEdgeAll(T start, T end, double weight = 1);
        /// <summary>
        /// 还原一个有方向的边
        /// </summary>
        /// <param name="edg"></param>
        /// <returns></returns>
        bool RestoreEdge(IMapEdge<T> edg);
        /// <summary>
        /// 还原一个无方向的边
        /// </summary>
        /// <param name="edg"></param>
        /// <returns></returns>
        bool RestoreEdgeAll(IMapEdge<T> edg);
        /// <summary>
        /// 移除一条有方向的边
        /// </summary>
        /// <param name="start">开始节点</param>
        /// <param name="end">结束节点</param>
        /// <returns></returns>
        bool RemoveEdge(T start, T end);
        /// <summary>
        /// 移除一条无方向的边
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns></returns>
        bool RemoveEdgeAll(T node1, T node2);

        /// <summary>
        /// 广度优先遍历 σ(n*k)
        /// </summary>
        /// <param name="Start">起始点</param>
        /// <returns>遍历次序</returns>
        List<T> BFS(T Start);
        /// <summary>
        /// 深度优先遍历 σ(n*(n-k))
        /// </summary>
        /// <param name="Start"></param>
        /// <returns></returns>
        List<T> DFS(T Start);
        /// <summary>
        /// 节点可达区域/连通分量判定 σ(3n) 
        /// </summary>
        /// <returns></returns>
        List<T[]> Zone();
    }


    /// <summary>
    /// 树节点
    /// </summary>
    /// <typeparam name="T">树节点类型</typeparam>
    public interface ITreeNode<T> where T : IEquatable<T>
    {
        T Id { get; }
        T? ParentName { get; }
        ISet<T> ChildsName { get; }
        bool IsRoot { get; }
        bool IsLeaf { get; }
    }
    /// <summary>
    /// 树结构
    /// </summary>
    /// <typeparam name="T">树节点类型</typeparam>
    public interface ITree<T> where T : IEquatable<T>
    {
        ITreeNode<T> Root { get; }
        IDictionary<T, ITreeNode<T>> NodeSet { get; }
        ITreeNode<T> this[T key] { get; }
    }

}
