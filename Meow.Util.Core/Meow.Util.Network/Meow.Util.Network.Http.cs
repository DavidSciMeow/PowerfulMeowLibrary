using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
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
            StringBuilder sb = new StringBuilder();
            byte[] buf = new byte[buflength];
            using (Stream resStream = resp.Content.ReadAsStreamAsync().GetAwaiter().GetResult())
            {
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
                        FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                        var response = Client.GetAsync(url).GetAwaiter().GetResult();
                        Stream responseStream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
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
                using (Stream myResponseStream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult())
                {
                    using (StreamReader streamReader = new StreamReader(myResponseStream))
                    {
                        var retString = streamReader.ReadToEnd();
                        return retString;
                    }
                }
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
                using (Stream myResponseStream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult())
                {
                    using (StreamReader streamReader = new StreamReader(myResponseStream))
                    {
                        var retString = streamReader.ReadToEnd();
                        return retString;
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
