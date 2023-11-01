using SkiaSharp;
using System;
using System.IO;
using System.Text;

namespace Meow.Util.Imaging
{
    /// <summary>
    /// 图片处理
    /// </summary>
    public static class Skia
    {
        /// <summary>
        /// 将文件转换成Base64格式
        /// </summary>
        public static string FileToBase64(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                byte[] byteArray = new byte[fs.Length];
                fs.Read(byteArray, 0, byteArray.Length);
                return Convert.ToBase64String(byteArray);
            }
        }
        /// <summary>
        /// 读取图片
        /// </summary>
        /// <param name="fp">文件指针</param>
        /// <returns></returns>
        public static SKBitmap Read(string fp) => SKBitmap.Decode(new SKManagedStream(File.OpenRead(fp)));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="skb"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static string PrintBW(this SKBitmap skb, int width = 100, int height = 50)
        {
            StringBuilder sb = new StringBuilder();
            skb = skb.Resize(new SKSizeI(width, height), SKFilterQuality.High);
            for (int i = 0; i < skb.Height; i++) 
            {
                for (int j = 0; j < skb.Width; j++)
                {
                    var pkc = skb.GetPixel(j, i);
                    pkc.ToHsl(out var _, out var _, out var l);
                    if (l > 50) sb.Append('█');
                    else sb.Append(' ');
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
        /// <summary>
        /// 保存位图图像
        /// </summary>
        /// <param name="i"></param>
        /// <param name="path">路径</param>
        /// <param name="f">图像格式</param>
        /// <param name="Quality">质量参数</param>
        public static void Save(this SKBitmap i, string path, SKEncodedImageFormat f = SKEncodedImageFormat.Jpeg, int Quality = 100) => i.Encode(f, Quality).SaveTo(File.OpenWrite(path));
        /// <summary>
        /// 将Bitmap对象转换为Base64
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ToBase64String(this SKBitmap b) => SKImage.FromBitmap(b).ToBase64String();
        /// <summary>
        /// 将Base64字符串转换成Image对象
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static SKBitmap Base64ToSKBitmap(this string base64String) => SKBitmap.Decode(Convert.FromBase64String(base64String));
        /// <summary>
        /// 将Image对象转换为Base64
        /// </summary>
        /// <param name="i">等待转换图像</param>
        /// <returns></returns>
        public static string ToBase64String(this SKImage i)
        {
            MemoryStream ms = new MemoryStream();
            i.Encode(SKEncodedImageFormat.Jpeg, 100).SaveTo(ms);
            byte[] arr = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(arr, 0, (int)ms.Length);
            ms.Close();
            return Convert.ToBase64String(arr);
        }
    }
}
