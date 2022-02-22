using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Meow.Interpreter
{
    /// <summary>
    /// 基类解释器
    /// </summary>
    public class BaseInterpreter
    {
        /// <summary>
        /// 分隔符
        /// </summary>
        public char spliter { get; }
        string[] returnval;
        /// <summary>
        /// 实例化一个基类解释器
        /// </summary>
        /// <param name="BaseString">要解释的字符串</param>
        /// <param name="Spliter">分隔符</param>
        public BaseInterpreter(string BaseString, char Spliter = ' ')
        {
            List<string> ret = new();
            spliter = Spliter;
            var spl = BaseString.Trim().Split(Spliter); //分离串
            foreach (var c in spl)
            {
                if (!string.IsNullOrWhiteSpace(c)) //移除空格
                {
                    ret.Add(c);
                }
            }
            returnval = ret.ToArray();
        }
        /// <summary>
        /// 获取排序好的字符串
        /// </summary>
        /// <returns></returns>
        public string[] GetPhrase() => returnval;
        /// <summary>
        /// 重写的空格删除逻辑
        /// </summary>
        /// <returns></returns>
        public new string ToString()
        {
            StringBuilder sb = new();
            foreach (var x in returnval)
            {
                sb.Append($"{x}{spliter}");
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// 命令解释器
    /// </summary>
    public class CommandInterpreter
    {
        List<(string[] CommandPattern, Action<string[], dynamic> action)> PatternList { get; } = new();
        Action<string[], dynamic> DefaultAction;
        dynamic? @Object;
        private sealed class CommandEqualityComparer : IEqualityComparer<string>
        {
            public bool Equals(string? x, string? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null) return false;
                if (y is null) return false;
                if (x == y) return true;
                if (y == "\0") return true;
                return false;
            }
            public int GetHashCode(string? obj)
            {
                if (obj is null) return 0;
                int hashProductCode = obj.GetHashCode();
                return hashProductCode;
            }
        }

        /// <summary>
        /// 命令分隔符
        /// </summary>
        public const string CommandParser = "\0";
        /// <summary>
        /// 生成一个命令解释器
        /// </summary>
        /// <param name="Object">携带的参数</param>
        public CommandInterpreter(dynamic? @Object = null)
        {
            this.@Object = @Object;
        }
        /// <summary>
        /// 注入一个参数列
        /// </summary>
        /// <param name="Object">想携带的参数</param>
        /// <returns></returns>
        public CommandInterpreter InjectObject(dynamic? @Object)
        {
            this.@Object = @Object;
            return this;
        }
        /// <summary>
        /// 添加命令
        /// </summary>
        /// <param name="CommandPattern">命令逻辑</param>
        /// <param name="action">命令代理</param>
        /// <returns></returns>
        public CommandInterpreter InjectCommandAndAction(string[] CommandPattern, Action<string[], dynamic> action)
        {
            PatternList.Add((CommandPattern, action));
            return this;
        }
        /// <summary>
        /// 设置默认触发值
        /// </summary>
        /// <param name="action">默认触发的类型</param>
        /// <returns></returns>
        public CommandInterpreter SetDeafult(Action<string[], dynamic> action)
        {
            DefaultAction = action;
            return this;
        }
        /// <summary>
        /// 执行解析
        /// </summary>
        /// <param name="BaseString">要解析的串</param>
        /// <param name="Spliter">分析器分隔字段</param>
        public void DoInterpreter(string BaseString, char Spliter = ' ')
        {
            var d = new BaseInterpreter(BaseString, Spliter).GetPhrase();
            ((from a in PatternList where Enumerable.SequenceEqual(d, a.CommandPattern,new CommandEqualityComparer()) select a).FirstOrDefault().action ?? DefaultAction)?.Invoke(d, @Object);
        }
        
    }
}
