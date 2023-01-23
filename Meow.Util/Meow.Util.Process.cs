using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Util.Proc
{
    public class Linux : IDisposable
    {
        public readonly Process Base;
        public readonly string Command;
        public StreamWriter StdIn;
        public readonly string WorkingDir;

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
            Base.Exited += e_Exited;

            /*(s, e) =>
            {
                if (Base != null)
                {
                    Base.Close();
                }
            };   // 注册进程结束事件  */
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            GC.SuppressFinalize(this); //禁止GC回收本例
            (Base as IDisposable).Dispose();
        }

        public bool JustStartProc() => Base.Start();
        public bool Start()
        {
            var p = Base.Start();
            Base.BeginOutputReadLine();
            return p;
        }
    }
}
