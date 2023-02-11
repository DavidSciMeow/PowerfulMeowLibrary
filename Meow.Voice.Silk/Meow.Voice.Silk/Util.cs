using System.Diagnostics;

namespace Meow.Voice.Silk
{
    /// <summary>
    /// 进程创建器
    /// </summary>
    public class SilkUtilProcess : IDisposable
    {
        readonly Process p = new();
        private bool disposedValue;
        private bool exited = false;

        /// <summary>
        /// 标准进程创造器
        /// </summary>
        /// <param name="proc">进程名</param>
        /// <param name="cmd">参数</param>
        public SilkUtilProcess(string proc, string cmd)
        {
            p = Process.Start(new ProcessStartInfo(proc, cmd)) ?? new();
            p.EnableRaisingEvents = true;
            p.Start();
            p.Exited += (sender, args) => { exited = true; };
        }
        /// <summary>
        /// 等待完成并退出
        /// </summary>
        public void WaitAndExit()
        {
            while (true)
            {
                if (exited)
                {
                    return;
                }
            }
        }
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
                    p.Close();
                }
                p.Dispose();
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

    /// <summary>
    /// 转换好的Silk模式
    /// </summary>
    public struct SilkReturn
    {
        private readonly string FileName;//文件名
        private readonly FileExtension Ext;//后缀

        /// <summary>
        /// 比特模式的数据
        /// </summary>
        public byte[] Data { get; }
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="ext">转换的文件格式</param>
        public SilkReturn(byte[] data, string fileName, FileExtension ext)
        {
            Data = data;
            FileName = fileName;
            Ext = ext;
        }

        /// <summary>
        /// 转换为文件
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        public void ConvertFile(string FilePath) => File.WriteAllBytes(Path.Combine(FilePath, $"{FileName}.{Ext}"), Data);
        /// <summary>
        /// 转换至Base64模式
        /// </summary>
        /// <returns></returns>
        public string ConvertTOBase64() => Convert.ToBase64String(Data);
    }
}
