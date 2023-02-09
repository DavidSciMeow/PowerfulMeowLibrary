using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Voice.Silk
{
    /// <summary>
    /// 进程创建器
    /// </summary>
    public class SilkUtilProcess
    {
        Process p = new();
        /// <summary>
        /// 标准进程创造器
        /// </summary>
        /// <param name="proc">进程名</param>
        /// <param name="cmd">参数</param>
        public SilkUtilProcess(string proc, string cmd)
        {
            p = Process.Start(new ProcessStartInfo(proc, cmd)) ?? new();
            p.Start();
        }
        /// <summary>
        /// 等待完成并退出
        /// </summary>
        public void WaitAndExit()
        {
            p.WaitForExit();
            p.Close();
        }
    }

    /// <summary>
    /// 转换好的Silk模式
    /// </summary>
    public struct SilkReturn
    {
        /// <summary>
        /// 比特模式的数据
        /// </summary>
        public byte[] Data { get; }
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="data">数据</param>
        public SilkReturn(byte[] data) => Data = data;
        /// <summary>
        /// 转换为文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        public void ConvertFile(string Path) => File.WriteAllBytes(Path, Data);
        /// <summary>
        /// 转换至Base64模式
        /// </summary>
        /// <returns></returns>
        public string ConvertTOBase64() => Convert.ToBase64String(Data);
    }
}
