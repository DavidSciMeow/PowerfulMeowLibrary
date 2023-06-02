using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Util.Convert
{
    public class UtilQueue<T>
    {
        List<T> Q { get; set; } = new();
        public void Enqueue(T msgs)
        {
            lock (Q)
            {
                Q.Add(msgs);
            }
        }
        public void Outqueue()
        {
            lock (Q)
            {
                Q.RemoveAt(0);
            }
        }
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
        public T this[int x]
        {
            get => Q[x];
        }
    }
}
