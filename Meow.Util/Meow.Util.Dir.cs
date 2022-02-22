using System;
using System.IO;

namespace Meow.Util.Basic
{
    /// <summary>
    /// Basic# 文件夹常用库
    /// </summary>
    public class DirX
    {
        /// <summary>
        /// 获取一个网页URL的完整后缀路径
        /// </summary>
        /// <param name="url">网页URL</param>
        /// <returns></returns>
        public static string GetWebSiteDirs(string url) =>
            new Uri(url).AbsolutePath.Replace(Path.GetFileName(url), "");
    }
}
