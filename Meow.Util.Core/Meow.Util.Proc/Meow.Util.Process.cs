using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Util.Proc
{
    /// <summary>
    /// 所有进程类基类
    /// </summary>
    public abstract class ProcBase : IDisposable
    {
        /// <summary>
        /// 进程底
        /// </summary>
        protected Process Base { get; } = new();
        /// <summary>
        /// 进程命令簇
        /// </summary>
        protected string? Command { get; set; }
        /// <summary>
        /// 关联进程是否启动
        /// </summary>
        protected bool Started { get; set; } = false;
        /// <summary>
        /// 标准输入流
        /// </summary>
        protected StreamWriter? StdIn { get; set; }
        /// <summary>
        /// 标准输出流
        /// </summary>
        protected StreamReader? StdOut { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this); //禁止GC回收本例
            Base.Close(); //关闭进程本身
            (Base as IDisposable).Dispose(); //销毁资源
        }
    }

    /// <summary>
    /// Linux*Bash*进程
    /// </summary>
    public sealed class Linux : ProcBase
    {
        /// <summary>
        /// 创建一个Linux*Bash*进程
        /// </summary>
        /// <param name="command">脚本</param>
        /// <param name="whenDataRecieved">数据回送回调</param>
        /// <param name="whenErrDataRecieved">错误回送回调</param>
        public Linux(string command, DataReceivedEventHandler? whenDataRecieved = null, DataReceivedEventHandler? whenErrDataRecieved = null)
        {
            Command = command;
            Base.StartInfo.FileName = "bash";
            Base.StartInfo.UseShellExecute = false;
            Base.StartInfo.RedirectStandardInput = true;
            Base.StartInfo.RedirectStandardOutput = true;
            Base.StartInfo.RedirectStandardError = true;
            Base.StartInfo.CreateNoWindow = true;
            StdIn = Base.StandardInput;
            StdOut = Base.StandardOutput;
            if (whenDataRecieved != null)
            {
                Base.OutputDataReceived += whenDataRecieved;
            }
            else
            {
                Base.OutputDataReceived += (s, e) => Console.WriteLine(e.Data);
            }
            if (whenErrDataRecieved != null)
            {
                Base.ErrorDataReceived += whenErrDataRecieved;
            }
            else
            {
                Base.ErrorDataReceived += (s, e) =>
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Data);
                    Console.ForegroundColor = default;
                };
            }
        }

        /// <summary>
        /// 启动当前进程
        /// </summary>
        public void Start()
        {
            Base.Start();
            Base.StandardInput.WriteLine(Command);
            Started = true;
            Base.WaitForExit();
        }
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command">命令</param>
        public void WriteCommand(string command)
        {
            StdIn?.Flush();
            StdIn?.WriteLine(command);
        }
    }
}
