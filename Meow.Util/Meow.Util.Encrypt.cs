using System;
using System.Security.Cryptography;
using System.Text;

namespace Meow.Util.Encrypt
{
    /// <summary>
    /// Basic# 散列加密库
    /// </summary>
    public static class Hash
    {
        /// <summary>
        /// Basic#用于特殊加密Discuz的MD5的
        /// </summary>
        /// <param name="strSource">元数据</param>
        /// <returns></returns>
        public static string DiscuzMd5(this string strSource) => Md5(strSource, Encoding.ASCII);
        /// <summary>
        /// Basic# 加密Md5
        /// </summary>
        /// <param name="strSource">元数据</param>
        /// <param name="e">编解码方案</param>
        /// <returns></returns>
        public static string Md5(this string strSource, Encoding e) => BitConverter.ToString(MD5.Create().ComputeHash(e.GetBytes(strSource))).Replace("-", "").ToLower();
        /// <summary>
        /// Md5Salt2加密方案
        /// </summary>
        /// <param name="pwd">元数据</param>
        /// <param name="salt">盐值</param>
        /// <returns></returns>
        public static string MD5S2ExpressPwd(this string pwd, string salt) => Md5($"{Md5(pwd, Encoding.ASCII)}{salt}", Encoding.ASCII);
    }
}
