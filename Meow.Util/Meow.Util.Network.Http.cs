using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Util.Network.Http
{
    /// <summary>
    /// .net6应用实例的标准流程链接获取模式
    /// (整个生命周期使用一个)
    /// <para>https://docs.microsoft.com/zh-cn/dotnet/api/system.net.http.httpclient?view=net-6.0</para>
    /// </summary>
    public class Client
    {
        /// <summary>
        /// 初始的实例HttpClient
        /// </summary>
        public readonly static HttpClient Basic = new();
        /// <summary>
        /// 带压缩解析的HttpClient
        /// </summary>
        public readonly static HttpClient Compression = new(
            new HttpClientHandler() {
                AutomaticDecompression = DecompressionMethods.All,
            });
        /// <summary>
        /// SSLless
        /// </summary>
        public readonly static HttpClient UNAutho = new(
            new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.All,
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; },
            });
    }
    /// <summary>
    /// 文件获取枚举类
    /// </summary>
    public enum HttpFiles
    {
        /// <summary>
        /// 下载成功
        /// </summary>
        Success_Download = 0,
        /// <summary>
        /// 下载失败
        /// </summary>
        Fail_Download = 1,
        /// <summary>
        /// 已存在
        /// </summary>
        Already_Exist = 2,
        /// <summary>
        /// 程序出错
        /// </summary>
        PROGRESS_FAIL = 3
    }
    /// <summary>
    /// GET - HTTP请求
    /// </summary>
    public class Get
    {
        /// <summary>
        /// Get / [Data &lt;83Kb]
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> String(string url) => await Client.Basic.GetStringAsync(url);
        /// <summary>
        /// Get / Compression (GZIP/and.others)
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="buflength">默认的缓存区大小</param>
        /// <returns></returns>
        public static async Task<string> Block(string url, int buflength = 10240)
        {
            var resp = await Client.Compression.GetAsync(url);
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
        /// <param name="url">网络地址</param>
        /// <param name="path">本地地址</param>
        /// <returns></returns>
        public static (bool Status, HttpFiles FileStatus, string ErrorString) File(string url, string path)
        {
            if (System.IO.File.Exists(Path.GetFullPath(path)))
            {
                return (true, HttpFiles.Already_Exist, "");
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path)??"");
                try
                {
                    FileStream fs = new(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    var response = Client.Basic.GetAsync(url).GetAwaiter().GetResult();
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
                    return (true, HttpFiles.Success_Download, "");
                }
                catch (Exception ex)
                {
                    return (false, HttpFiles.PROGRESS_FAIL, ex.Message);
                }
            }
        }
    }
    /// <summary>
    /// POST - HTTP请求
    /// </summary>
    public class Post
    {
        /// <summary>
        /// 创造一个空POST请求
        /// </summary>
        /// <param name="url">网络地址</param>
        /// <returns></returns>
        public static async Task<string> Create(string url) => await Create(url, "");
        /// <summary>
        /// 创造一个字符类型的POST请求
        /// </summary>
        /// <param name="url">网络地址</param>
        /// <param name="postData">内容</param>
        /// <returns></returns>
        public static async Task<string> Create(string url, string postData)
        {
            try
            {
                var response = await Client.Basic.PostAsync(url, new StringContent(postData));
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
        /// <param name="url">网络地址</param>
        /// <param name="postData">内容</param>
        /// <returns></returns>
        public static async Task<string> Create(string url, Stream postData)
        {
            try
            {
                var response = await Client.Basic.PostAsync(url, new StreamContent(postData));
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
        /// <param name="url">地址</param>
        /// <param name="postData">发送数据格式</param>
        /// <param name="t">解码类型</param>
        /// <returns></returns>
        public static async Task<string> Create(string url, object postData,Type t)
        {
            try
            {
                var response = await Client.Basic.PostAsync(url, System.Net.Http.Json.JsonContent.Create(postData, t));
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
        /// <param name="url">地址</param>
        /// <param name="postData">发送数据格式</param>
        /// <returns></returns>
        public static async Task<string> Create(string url, object postData)
        {
            try
            {
                var response = await Client.Basic.PostAsync(url, System.Net.Http.Json.JsonContent.Create(postData));
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
}
