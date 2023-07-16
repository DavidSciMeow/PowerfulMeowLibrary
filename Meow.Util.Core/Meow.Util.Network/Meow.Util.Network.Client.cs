using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Util.Network
{
    public class CoreClient
    {
        HttpClient[] ListClient { get; set; }
        bool[] ListInTask { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Send"></param>
        /// <param name="Recv"></param>
        /// <param name="MaxClientAllow"></param>
        public CoreClient(Action<double, double?> Send, Action<double, double?> Recv, uint MaxClientAllow = 10)
        {
            ListClient = new HttpClient[MaxClientAllow];
            ListInTask = new bool[MaxClientAllow];
            for (int i = 0; i < MaxClientAllow; i++)
            {
                ListClient[i] = GetClient(Send, Recv);
                ListInTask[i] = false;
            }
        }

        (int, HttpClient) TryGetFreeClient()
        {
            while (true)//<-stuck
            {
                for (int i = 0; i < ListInTask.Length; i++)
                {
                    if (ListInTask[i])
                    {
                        ListInTask[i] = true;
                        return (i, ListClient[i]);
                    }
                }
                Task.Delay(1);//<-stuckIntervi
            }
        }

        public Task<Stream> GetStream(string url, Action<Stream> ActionContinue)
        {
            return Task.Factory.StartNew(() =>
            {
                (int i, HttpClient c) = TryGetFreeClient();
                var tsk = c.GetStreamAsync(url);
                tsk.Wait();
                ListInTask[i] = false;
                ActionContinue.Invoke(tsk.Result);
                return tsk.Result;
            });
        }
        public Task<Stream>[] InterpreterTask(List<string> List, Action<Stream> ActionContinue)
        {
            List<Task<Stream>> tl = new();
            foreach (var url in List)
            {
                tl.Add(GetStream(url, ActionContinue));
            }
            return tl.ToArray();
        }

        /// <summary>
        /// 获取默认Client
        /// </summary>
        /// <param name="Send"></param>
        /// <param name="Recv"></param>
        /// <returns></returns>
        static HttpClient GetClient(Action<double, double?> Send, Action<double, double?> Recv)
        {
            HttpClient C;
            ProgressMessageHandler pmh = new(new HttpClientHandler());
            pmh.HttpSendProgress += (s, e) =>
            {
                Send?.Invoke(e.BytesTransferred, e.TotalBytes);
            };
            pmh.HttpReceiveProgress += (s, e) =>
            {
                Recv?.Invoke(e.BytesTransferred, e.TotalBytes);
            };
            C = new(pmh);
            return C;
        }
    }

    public class TFile
    {
        static HttpClient hc = new();

        public string url;
        public int BufferLength;
        public long ContentLength;
        public long TotalRecv;
        public int NowRecv;

        public event EventHandler<int> StreamReceivedPercent = null!;

        public TFile(string url, int bufferLength = 1024)
        {
            this.url = url;
            BufferLength = bufferLength;
        }

        public Task<string> Get()
        {
            return Task.Run(async () =>
            {
                var hresp = hc.Send(new HttpRequestMessage(HttpMethod.Head, url));
                ContentLength = hresp.Content.Headers.ContentLength ?? -1;

                StringBuilder sb = new();

                var resp = hc.Send(new HttpRequestMessage(HttpMethod.Get, url));
                var rs = await resp.Content.ReadAsStreamAsync();
                byte[] bytes = new byte[BufferLength];
                while ((NowRecv = await rs.ReadAsync(bytes)) != 0)
                {
                    TotalRecv += NowRecv;
                    if (ContentLength != -1)
                    {
                        StreamReceivedPercent?.Invoke(null, (int)(TotalRecv / (double)ContentLength * 100));
                    }
                    sb.Append(Encoding.UTF8.GetString(bytes));
                }
                return sb.ToString();
            });
        }
    }
}
