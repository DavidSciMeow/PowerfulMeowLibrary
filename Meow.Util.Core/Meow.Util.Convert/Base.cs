using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Util.Convert
{
    /// <summary>
    /// 一个扩展方案队列
    /// </summary>
    /// <typeparam name="T">队列内泛型</typeparam>
    public class UQueue<T>
    {
        List<T> Q { get; set; } = new();
        /// <summary>
        /// 入队 (普通队列操作)
        /// </summary>
        /// <param name="msgs"></param>
        public void Enqueue(T msgs)
        {
            lock (Q)
            {
                Q.Add(msgs);
            }
        }
        /// <summary>
        /// 出队 (普通队列操作/无返回值)
        /// </summary>
        public void Outqueue()
        {
            lock (Q)
            {
                Q.RemoveAt(0);
            }
        }
        /// <summary>
        /// 出队 (队列操作/返回队头元素)
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            T ret;
            lock (Q)
            {
                ret = Q[0];
                Outqueue();
            }
            return ret;
        }
        /// <summary>
        /// 选择队列内元素
        /// </summary>
        /// <param name="x">元素位置</param>
        /// <returns></returns>
        public T this[int x]
        {
            get => Q[x];
        }
    }
}
