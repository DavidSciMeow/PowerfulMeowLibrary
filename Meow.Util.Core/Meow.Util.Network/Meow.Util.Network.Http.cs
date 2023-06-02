using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Meow.Util.Network.Http
{
    /// <summary>
    /// SSLless: <br/>
    /// HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }} <br/>
    /// Compress: <br/>
    /// HttpClientHandler { AutomaticDecompression = DecompressionMethods.All }
    /// </summary>
    public static class HttpUtil
    {
        /// <summary>
        /// Get / [Data &lt;83Kb]
        /// </summary>
        /// <param name="Client"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Task<string> MString(this HttpClient Client, string url) => Client.GetStringAsync(url);
        /// <summary>
        /// Get / Binary [byte[]]
        /// </summary>
        /// <param name="Client"></param>
        /// <param name="url">地址</param>
        /// <returns></returns>
        public static Task<byte[]> MBinary(this HttpClient Client, string url) => Client.GetByteArrayAsync(url);
        /// <summary>
        /// Get / Compression (GZIP/and.others)
        /// </summary>
        /// <param name="Client">
        /// Compress: <br/>
        /// HttpClientHandler { AutomaticDecompression = DecompressionMethods.All }
        /// </param>
        /// <param name="url">地址</param>
        /// <param name="buflength">默认的缓存区大小</param>
        /// <returns></returns>
        public static async Task<string> MBlock(this HttpClient Client, string url, int buflength = 10240)
        {
            var resp = await Client.GetAsync(url);
            StringBuilder sb = new();
            byte[] buf = new byte[buflength];
            using Stream resStream = resp.Content.ReadAsStream();
            int count;
            do
            {
                count = resStream.Read(buf, 0, buf.Length - 1);
                if (count != 0)
                {
                    sb.Append(Encoding.Default.GetString(buf, 0, count));
                }
            } while (count > 0);
            return sb.ToString();
        }
        /// <summary>
        /// 获取文件到指定目录
        /// </summary>
        /// <param name="Client"></param>
        /// <param name="url">网络地址</param>
        /// <param name="path">本地地址</param>
        /// <returns>0 = 文件已存在, 1 = 已经获取, -1 = 出现错误</returns>
        public static Task<int> MFile(this HttpClient Client, string url, string path)
        {
            return Task.Factory.StartNew(() =>
            {
                if (File.Exists(Path.GetFullPath(path)))
                {
                    return 0;
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path) ?? "");
                    try
                    {
                        FileStream fs = new(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                        var response = Client.GetAsync(url).GetAwaiter().GetResult();
                        Stream responseStream = response.Content.ReadAsStream();
                        byte[] bArr = new byte[1024];
                        int size = responseStream.Read(bArr, 0, bArr.Length);
                        while (size > 0)
                        {
                            fs.Write(bArr, 0, size);
                            size = responseStream.Read(bArr, 0, bArr.Length);
                        }
                        fs.Close();
                        responseStream.Close();
                        return 1;
                    }
                    catch
                    {
                        return -1;
                    }
                }
            });
        }

        /// <summary>
        /// 创造一个空POST请求
        /// </summary>
        /// <param name="Client"></param>
        /// <param name="url">网络地址</param>
        /// <returns></returns>
        public static async Task<string> MPost(this HttpClient Client, string url) => await MPost(Client, url, "");
        /// <summary>
        /// 创造一个字符类型的POST请求
        /// </summary>
        /// <param name="Client"></param>
        /// <param name="url">网络地址</param>
        /// <param name="postData">内容</param>
        /// <returns></returns>
        public static async Task<string> MPost(this HttpClient Client, string url, string postData)
        {
            try
            {
                var response = await Client.PostAsync(url, new StringContent(postData));
                using Stream myResponseStream = response.Content.ReadAsStream();
                using StreamReader streamReader = new(myResponseStream);
                var retString = streamReader.ReadToEnd();
                return retString;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 创造一个流POST请求
        /// </summary>
        /// <param name="Client"></param>
        /// <param name="url">网络地址</param>
        /// <param name="postData">内容</param>
        /// <returns></returns>
        public static async Task<string> MPost(this HttpClient Client, string url, Stream postData)
        {
            try
            {
                var response = await Client.PostAsync(url, new StreamContent(postData));
                using Stream myResponseStream = response.Content.ReadAsStream();
                using StreamReader streamReader = new(myResponseStream);
                var retString = streamReader.ReadToEnd();
                return retString;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 发送一个自解释的Json格式Post
        /// <para> new {id=id,...} </para>
        /// </summary>
        /// <param name="Client"></param>
        /// <param name="url">地址</param>
        /// <param name="postData">发送数据格式</param>
        /// <param name="t">解码类型</param>
        /// <returns></returns>
        public static async Task<string> MPost(this HttpClient Client, string url, object postData, Type t)
        {
            try
            {
                var response = await Client.PostAsync(url, System.Net.Http.Json.JsonContent.Create(postData, t));
                using Stream myResponseStream = response.Content.ReadAsStream();
                using StreamReader streamReader = new(myResponseStream);
                var retString = streamReader.ReadToEnd();
                return retString;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 发送一个自解释的Json格式Post
        /// <para> new {id=id,...} </para>
        /// </summary>
        /// <param name="Client"></param>
        /// <param name="url">地址</param>
        /// <param name="postData">发送数据格式</param>
        /// <returns></returns>
        public static async Task<string> MPost(this HttpClient Client, string url, object postData)
        {
            try
            {
                var response = await Client.PostAsync(url, JsonContent.Create(postData));
                using Stream myResponseStream = response.Content.ReadAsStream();
                using StreamReader streamReader = new(myResponseStream);
                var retString = streamReader.ReadToEnd();
                return retString;
            }
            catch
            {
                throw;
            }
        }
    }
    /// <summary>
    /// BasicStaticUsageHttpClient
    /// </summary>
    public class StaticHttp : IDisposable
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        private readonly HttpClient C = null!;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// 连接池最大处理时间 (默认五分钟)
        /// </summary>
        public TimeSpan PooledConnextionLifeTime { get; }
        /// <summary>
        /// 超时时间 (默认无限)
        /// </summary>
        public TimeSpan ConnectTimeout { get; }
        /// <summary>
        /// SSL检测避免 (默认不避免)
        /// </summary>
        public bool AvoidSSLCertifaction { get; }
        /// <summary>
        /// 使用压缩模式 (默认开启)
        /// </summary>
        public bool AutoDecomp { get; }
        /// <summary>
        /// 使用Cookies (默认关闭)
        /// </summary>
        public bool UseCookies { get; }
        /// <summary>
        /// 使用代理 (默认是关闭)
        /// </summary>
        public bool UseProxy { get; }
        /// <summary>
        /// 允许301重定向 (默认是开启)
        /// </summary>
        public bool AllowAutoRedirect { get; }
        /// <summary>
        /// 最大重定向次数 (默认10次)
        /// </summary>
        public int MaxAutomaticRedirections { get; }
        /// <summary>
        /// 多路复用 (默认开启)
        /// </summary>
        public bool EnableMultipleHttp2Connections { get; }



        /// <summary>
        /// 初始化函数
        /// </summary>
        /// <param name="pooledConnextionLifeTime">连接池最大处理时间 (默认五分钟)</param>
        /// <param name="connectTimeout">连接超时时间 (默认无穷[null])</param>
        /// <param name="avoidSSLCertifaction">SSL检测避免 (默认不避免)</param>
        /// <param name="autoDecomp">使用压缩模式 (默认开启)</param>
        /// <param name="useCookies">使用Cookie (默认不允许)</param>
        /// <param name="useProxy">使用代理 (默认不允许)</param>
        /// <param name="allowAutoRedirect">允许重定向 (默认允许)</param>
        /// <param name="maxAutomaticRedirections">最大重定向次数 (默认10次)</param>
        /// <param name="enableMultipleHttp2Connections">多路复用 (默认开启)</param>
        public StaticHttp(
            TimeSpan? pooledConnextionLifeTime = null, TimeSpan? connectTimeout = null,
            bool avoidSSLCertifaction = false, bool autoDecomp = true, bool useCookies = false, bool useProxy = false, bool allowAutoRedirect = true,
            int maxAutomaticRedirections = 10, bool enableMultipleHttp2Connections = true)
        {
            PooledConnextionLifeTime = pooledConnextionLifeTime ?? TimeSpan.FromMinutes(5);
            ConnectTimeout = connectTimeout ?? Timeout.InfiniteTimeSpan;
            AvoidSSLCertifaction = avoidSSLCertifaction;
            AutoDecomp = autoDecomp;
            UseCookies = useCookies;
            UseProxy = useProxy;
            AllowAutoRedirect = allowAutoRedirect;
            EnableMultipleHttp2Connections = enableMultipleHttp2Connections;
            MaxAutomaticRedirections = maxAutomaticRedirections;
            var d = new SocketsHttpHandler
            {
                PooledConnectionLifetime = PooledConnextionLifeTime,
                UseCookies = UseCookies,
                UseProxy = UseProxy,
                AllowAutoRedirect = AllowAutoRedirect,
                MaxAutomaticRedirections = MaxAutomaticRedirections,
                ConnectTimeout = ConnectTimeout,
                EnableMultipleHttp2Connections = EnableMultipleHttp2Connections,

            };
            if (AutoDecomp)
            {
                d.AutomaticDecompression = DecompressionMethods.All;
            }
            if (AvoidSSLCertifaction)
            {
                d.SslOptions.RemoteCertificateValidationCallback += (a, b, c, d) => true;
            }
            C = new HttpClient(d);
            MaxAutomaticRedirections = maxAutomaticRedirections;
           
        }


        /// <summary>
        /// 发送一个Request(使用Req)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> DoRequest(HttpRequestMessage req) => C.SendAsync(req);

        /// <summary>
        /// Get / (ResponseMessage) 微软建议模式
        /// </summary>
        /// <param name="url">地址</param>
        /// <returns></returns>
        public Task<HttpResponseMessage> Get(string url) => C.GetAsync(url);
        /// <summary>
        /// Get / (Stream) 微软建议模式
        /// </summary>
        /// <param name="url">地址</param>
        /// <returns></returns>
        public Task<Stream> GetStream(string url) => C.GetStreamAsync(url);
        /// <summary>
        /// Get / (String) 微软建议模式
        /// </summary>
        /// <param name="url">地址</param>
        /// <returns></returns>
        public Task<string> GetString(string url) => C.GetStringAsync(url);
        /// <summary>
        /// Get / [Data &lt;83Kb] (快速获取模式)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Task<string> GetSimpleString(string url) => C.MString(url);
        /// <summary>
        /// Get / Binary [byte[]]
        /// </summary>
        /// <param name="url">地址</param>
        /// <returns></returns>
        public Task<byte[]> GetBinary(string url) => C.MBinary(url);
        /// <summary>
        /// Get / Compression (GZIP/and.others)
        /// </summary>
        /// <param name="Client">
        /// Compress: <br/>
        /// HttpClientHandler { AutomaticDecompression = DecompressionMethods.All }
        /// </param>
        /// <param name="url">地址</param>
        /// <param name="buflength">默认的缓存区大小</param>
        /// <returns></returns>
        public Task<string> GetBlock(string url, int buflength = 10240) => C.MBlock(url, buflength);
        /// <summary>
        /// 获取文件到指定目录
        /// </summary>
        /// <param name="url">网络地址</param>
        /// <param name="path">本地地址</param>
        /// <returns>0 = 文件已存在, 1 = 已经获取, -1 = 出现错误</returns>
        public Task<int> GetFile(string url, string path) => C.MFile(url, path);

        /// <summary>
        /// Post / (ResponseMessage) 微软建议模式
        /// </summary>
        /// <param name="url">地址</param>
        /// <returns></returns>
        public Task<HttpResponseMessage> Post(string url, HttpContent content) => C.PostAsync(url, content);
        /// <summary>
        /// 创造一个字符类型的POST请求
        /// </summary>
        /// <param name="url">网络地址</param>
        /// <param name="postData">内容</param>
        /// <returns></returns>
        public Task<string> PostString(string url, string postData) => C.MPost(url, postData);
        /// <summary>
        /// 创造一个流POST请求
        /// </summary>
        /// <param name="url">网络地址</param>
        /// <param name="postData">内容</param>
        /// <returns></returns>
        public Task<string> PostStream(string url, Stream postData) => C.MPost(url, postData);
        /// <summary>
        /// 发送一个自解释的Json格式Post
        /// <para> new {id=id,...} </para>
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="postData">发送数据格式</param>
        /// <param name="t">解码类型</param>
        /// <returns></returns>
        public Task<string> PostJsonObjectAsType(string url, object postData, Type t) => C.MPost(url, postData, t);
        /// <summary>
        /// 发送一个自解释的Json格式Post
        /// <para> new {id=id,...} </para>
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="postData">发送数据格式</param>
        /// <returns></returns>
        public Task<string> PostJson(string url, object postData) => C.MPost(url, postData);
        /// <summary>
        /// Post / [自动解析模式] (ResponseMessage) 微软建议模式
        /// </summary>
        /// <typeparam name="T">Json实体类类型</typeparam>
        /// <param name="url">目标地址</param>
        /// <param name="postData">实体类对象</param>
        /// <param name="ct">取消标</param>
        /// <returns></returns>
        public Task<HttpResponseMessage> PostJson<T>(string url, T postData, CancellationToken ct) => C.PostAsJsonAsync(url, postData, ct);

        /// <summary>
        /// Put / [自动解析模式] (ResponseMessage) 微软建议模式
        /// </summary>
        /// <param name="url">目标地址</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public Task<HttpResponseMessage> Put(string url, HttpContent content) => C.PutAsync(url, content);
        /// <summary>
        /// PutJson / [自动解析模式] (ResponseMessage) 微软建议模式
        /// </summary>
        /// <typeparam name="T">Json实体类类型</typeparam>
        /// <param name="url">目标地址</param>
        /// <param name="postData">实体类对象</param>
        /// <param name="ct">取消标</param>
        /// <returns></returns>
        public Task<HttpResponseMessage> PutJson<T>(string url, T postData, CancellationToken ct) => C.PutAsJsonAsync(url, postData, ct);

        /// <summary>
        /// Delete / (微软建议模式)
        /// </summary>
        /// <param name="url">地址</param>
        /// <returns></returns>
        public Task<HttpResponseMessage> Delete(string url) => C.DeleteAsync(url);
        /// <summary>
        /// Patch / (微软建议模式)
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public Task<HttpResponseMessage> Patch(string url, HttpContent content) => C.PatchAsync(url, content);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="disposing"><inheritdoc/></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    C.Dispose();
                }
                disposedValue = true;
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
