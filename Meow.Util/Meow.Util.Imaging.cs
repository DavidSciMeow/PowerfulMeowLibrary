using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Util.Imaging
{
    /// <summary>
    /// Base64 常用工具集
    /// </summary>
    public class Base64
    {
        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string Encode(string source) => Encode(Encoding.UTF8, source);
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encodeType">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string Encode(Encoding encodeType, string source)
        {
            byte[] bytes = encodeType.GetBytes(source);
            string encode;
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }
        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Decode(string result) => Decode(Encoding.UTF8, result);
        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encodeType">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Decode(Encoding encodeType, string result)
        {
            byte[] bytes = Convert.FromBase64String(result);
            string decode;
            try
            {
                decode = encodeType.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }
        /// <summary>
        /// Image 转成 base64
        /// </summary>
        /// <param name="imagePath"></param>
        public static string ToBase64(string imagePath)
        {
            try
            {
                Bitmap bmp = new(imagePath);
                MemoryStream ms = new();
                var suffix = imagePath.Substring(imagePath.LastIndexOf('.') + 1,
                    imagePath.Length - imagePath.LastIndexOf('.') - 1).ToLower();
                var suffixName = suffix == "png"
                    ? ImageFormat.Png
                    : suffix == "jpg" || suffix == "jpeg"
                        ? ImageFormat.Jpeg
                        : suffix == "bmp"
                            ? ImageFormat.Bmp
                            : suffix == "gif"
                                ? ImageFormat.Gif
                                : ImageFormat.Jpeg;

                bmp.Save(ms, suffixName);
                byte[] arr = new byte[ms.Length]; ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length); ms.Close();
                bmp.Dispose();
                return Convert.ToBase64String(arr);
            }
            catch (Exception)
            {
                return null;
            }

        }
        /// <summary>
        /// bitmap转换Base64 
        /// </summary>
        /// <param name="bmp">Bitmap实例</param>
        /// <returns></returns>
        public static string ToBase64(Bitmap bmp)
        {
            try
            {
                MemoryStream ms = new();
                bmp.Save(ms, ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length]; ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length); ms.Close();
                bmp.Dispose();
                return Convert.ToBase64String(arr);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
