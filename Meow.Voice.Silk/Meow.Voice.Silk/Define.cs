using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Voice.Silk
{
    /// <summary>
    /// 编码器接口
    /// </summary>
    public interface IEncodeable
    {
        /// <summary>
        /// 编码逻辑
        /// </summary>
        /// <param name="filePath">文件路径(全限定)</param>
        /// <returns></returns>
        public Task<SilkReturn> Encode(string filePath);
    }
    /// <summary>
    /// 解码器接口
    /// </summary>
    public interface IDecodeable
    {
        /// <summary>
        /// 解码逻辑
        /// </summary>
        /// <param name="filePath">文件路径(全限定)</param>
        /// <returns></returns>
        public Task<SilkReturn> Decode(string filePath);
    }
    /// <summary>
    /// 扩展名规范
    /// </summary>
    public enum FileExtension
    {
        /// <summary>
        /// MP3格式
        /// </summary>
        mp3,
        /// <summary>
        /// WAV格式
        /// </summary>
        wav,
        /// <summary>
        /// Silk格式
        /// </summary>
        silk,
    }
}
