using System;
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
        public Process? Base;
        /// <summary>
        /// 进程命令簇
        /// </summary>
        public string? Command;
        /// <summary>
        /// 标准输入流
        /// </summary>
        public StreamWriter? StdIn;
        /// <summary>
        /// 工作文档目录
        /// </summary>
        public string? WorkingDir;
        /// <summary>
        /// 只是启动进程
        /// </summary>
        /// <returns></returns>
        public abstract bool JustStartProc();
        /// <summary>
        /// 启动进程且打开输入流
        /// </summary>
        /// <returns></returns>
        public abstract bool Start();
        /// <inheritdoc/>
        public void Dispose()
        {
            GC.SuppressFinalize(this); //禁止GC回收本例
            (Base as IDisposable).Dispose();
        }
    }
    /// <summary>
    /// Linux端进程管理
    /// </summary>
    public class Linux : ProcBase
    {
        /// <summary>
        /// 实例化一个linux进程
        /// </summary>
        /// <param name="command"></param>
        /// <param name="e_ODR"></param>
        /// <param name="e_Exited"></param>
        /// <param name="workingDir"></param>
        public Linux(string command, DataReceivedEventHandler e_ODR, EventHandler e_Exited, string workingDir)
        {
            Command = command;
            WorkingDir = workingDir;
            Base = new Process();
            Base.StartInfo.FileName = Command;      // 命令  
            Base.StartInfo.CreateNoWindow = true;         // 不创建新窗口  
            Base.StartInfo.RedirectStandardInput = true;  // 重定向输入  
            Base.StartInfo.RedirectStandardOutput = true; // 重定向标准输出  
            StdIn = Base.StandardInput; // 定位至stdin;
            Base.EnableRaisingEvents = true;                      // 启用Exited事件  
            Base.StartInfo.WorkingDirectory = WorkingDir;
            Base.OutputDataReceived += e_ODR;
            Base.Exited += e_Exited; // 注册进程结束事件
        }
        /// <summary>
        /// 只是启动进程
        /// </summary>
        /// <returns></returns>
        public override bool JustStartProc() => Base?.Start() ?? false;
        /// <summary>
        /// 启动进程且打开输入流
        /// </summary>
        /// <returns></returns>
        public override bool Start()
        {
            var p = Base?.Start() ?? false;
            Base?.BeginOutputReadLine();
            return p;
        }
    }
}
