using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.Interpreter.Args
{
    /// <summary>
    /// ArgsInterpreter // 
    /// </summary>
    public static class ArgsInterpreter
    {
        /// <summary>
        /// 主模式Args解析器
        /// </summary>
        /// <param name="args">主程序args</param>
        /// <param name="args_list">需要解析的argslist</param>
        /// <returns>一个对位解析好的模式</returns>
        public static Dictionary<string, string[]?> Interprete(this string[] args, List<(string str, int paranum)> args_list)
        {
            Dictionary<string, string[]?> dicpara = new();
            List<int> parapos = new();
            foreach (var (str, paranum) in args_list)
            {
                List<string> lst = new();
                int q = Array.IndexOf(args, str);
                parapos.Add(q);

                if (q != -1)
                {
                    for (int i = 0; i < paranum; i++)
                    {
                        lst.Add(args[q + i + 1]);
                    }
                    dicpara.Add(str, lst.ToArray());
                }
                else
                {
                    dicpara.Add(str, null);
                }
            }

            
            return dicpara;
        }
    }
}
