using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Meow.Interpreter.Command
{
    /// <summary>
    /// 解释器工具集合
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// 表达式列转换字符列
        /// </summary>
        /// <param name="epls"></param>
        /// <returns></returns>
        public static string[] ExpressionDataToString(this Expression[] epls)
        {
            List<string> result = new();
            foreach(var i in epls)
            {
                result.Add(i.ExpressionString);
            }
            return result.ToArray();
        }
        /// <summary>
        /// 字符列转换表达式列
        /// </summary>
        /// <param name="epls"></param>
        /// <returns></returns>
        public static Expression[] StringDataToExpression(this string[] epls)
        {
            List<Expression> result = new();
            foreach(string str in epls)
            {
                result.Add(new Expression(str));
            }
            return result.ToArray();
        }
        /// <summary>
        /// 字符串转换表达式列
        /// </summary>
        /// <param name="epls">要分割的字符串</param>
        /// <param name="default_Splitor">默认分隔符为空格,如果使用请注意不要使用和命令串一致的字符</param>
        /// <param name="default_command_placeholder">默认的命令串为$,如需要输入字符串$请输入两次</param>
        /// <param name="_filter_space">默认过滤空格,输入false不进行过滤</param>
        /// <returns></returns>
        public static Expression[] ToExpression(this string epls,string default_Splitor=" ",string default_command_placeholder="$",bool _filter_space = true)
        {
            List<Expression> result = new();
            var vs = epls.Trim().Split(default_Splitor);//分割字符串
            foreach(var v in vs)
            {
                if (!_filter_space || !string.IsNullOrWhiteSpace(v))// 过滤空格
                {
                    if (v.Equals($"{default_command_placeholder}{default_command_placeholder}")) //转义分隔符
                    {
                        result.Add(new Expression(default_command_placeholder));// 增加表达式
                    }
                    else if (v.Equals(default_command_placeholder))//是命令字
                    {
                        result.Add(new Expression("",isPlaceholder:true));// 增加表达式
                    }
                    else if (v.StartsWith(default_command_placeholder) && v.EndsWith(default_command_placeholder)) //正则类
                    {
                        result.Add(new Expression(v[1..^1], isRegex: true)); //是正则表达式
                    }
                    else
                    {
                        result.Add(new Expression(v));// 增加表达式
                    }
                }
            }
            return result.ToArray();
        }
        /// <summary>
        /// 获得指令对应的动作(模糊键搜素)
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="expr">传入的表达式</param>
        /// <returns></returns>
        public static Action<string[], object?>? GetAction(this SortedDictionary<Expression[], Action<string[], object?>> dic, Expression[] expr)
        {
            //return (from a in dic where a.Key.Compare(expr) == true select a).FirstOrDefault().Value; //much more expres
            foreach (var i in dic)
            {
                if (i.Key.Compare(expr))
                {
                    return i.Value;
                }
            }
            return null;
        }
        /// <summary>
        /// 对比当前文法块和表达式块
        /// </summary>
        /// <param name="epls">表达式块</param>
        /// <param name="cont">当前文法块</param>
        /// <returns></returns>
        public static bool CEAC(this Expression[] epls, string[] cont) => CompareContextAndExpression(cont, epls);
        /// <summary>
        /// 对比当前文法块和表达式块
        /// </summary>
        /// <param name="epls">表达式块</param>
        /// <param name="cont">当前文法块</param>
        /// <returns></returns>
        public static bool CEAC(this string[] cont, Expression[] epls) => CompareContextAndExpression(cont, epls);
        /// <summary>
        /// 对比表达式块基础方法
        /// </summary>
        /// <param name="cont"></param>
        /// <param name="epls"></param>
        /// <returns></returns>
        private static bool CompareContextAndExpression(string[] cont, Expression[] epls)
        {
            if (epls.Length != cont.Length) // 命令长度不一致
            {
                return false;
            }
            else
            {
                for (int i = 0; i < epls.Length; i++) //循环所有字符
                {
                    if (epls[i] != cont[i]) //任意位置字符不对等
                    {
                        return false;
                    }
                }
                return true; //完全对比完成
            }
        }
        /// <summary>
        /// 比对两个表达式列
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool CompareNotNull(this Expression[] a, Expression[] b)
        {
            if (a.Length != b.Length) // 命令长度不一致
            {
                return false;
            }
            else
            {
                for (int i = 0; i < a.Length; i++) //循环所有字符
                {
                    if (a[i] != b[i]) //任意位置字符不对等
                    {
                        return false;
                    }
                }
                return true; //完全对比完成
            }
        }
        /// <summary>
        /// 比对两个表达式列
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Compare(this Expression[]? a, Expression[]? b)
        {
            if(a == null || b == null) //某个为空则不一致
            {
                return false;
            }
            else
            {
                return CompareNotNull(a, b);
            }
        }
    }
    /// <summary>
    /// 表达式结构
    /// </summary>
    public struct Expression : IComparable<Expression>, IComparer<Expression>
    {
        /// <summary>
        /// 命令字
        /// </summary>
        public string ExpressionString;
        /// <summary>
        /// 是否为参数列
        /// </summary>
        public bool IsPlaceholder;
        /// <summary>
        /// 是否为正则表达式
        /// </summary>
        public bool IsRegex;
        /// <summary>
        /// 创建表达式结构
        /// </summary>
        /// <param name="expressionString">命令字</param>
        /// <param name="isPlaceholder">是否为参数列</param>
        /// <param name="isRegex">是否为正则表达式串</param>
        public Expression(string expressionString, bool isPlaceholder = false, bool isRegex = false)
        {
            IsRegex = isRegex;
            if (isRegex)
            {
                if(expressionString.Length > 3)
                {
                    ExpressionString = expressionString[1..^1];
                }
                else
                {
                    ExpressionString = expressionString;
                }
            }
            else
            {
                ExpressionString = expressionString;
            }
            IsPlaceholder = isPlaceholder;
        }
        /// <summary>
        /// 重载运算,如果两个有任意一个为命令预留位则一致
        /// </summary>
        /// <param name="a">表达式A</param>
        /// <param name="b">表达式B</param>
        /// <returns></returns>
        public static bool operator ==(Expression a, Expression b)
        {
            if(a.ExpressionString.Equals(b.ExpressionString))
            {
                return true;
            }
            else if(a.IsPlaceholder || b.IsPlaceholder)
            {
                return true;
            }
            else if (a.IsRegex && b.IsRegex)
            {
                return a.ExpressionString == b.ExpressionString;
            }
            else if (a.IsRegex)
            {
                return Regex.IsMatch(b.ExpressionString, a.ExpressionString);
            }
            else if (b.IsRegex)
            {
                return Regex.IsMatch(a.ExpressionString, b.ExpressionString);
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 重载运算,如果两个有任意一个为命令预留位则一致
        /// </summary>
        /// <param name="a">表达式A</param>
        /// <param name="b">表达式B</param>
        /// <returns></returns>
        public static bool operator !=(Expression a, Expression b) => !(a == b);
        /// <summary>
        /// 判断字符串是否和表达式一致
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(string a,Expression b)
        {
            if (a.Equals(b.ExpressionString))
            {
                return true;
            }
            else if (b.IsPlaceholder)
            {
                return true;
            }
            else if (b.IsRegex)
            {
                return Regex.IsMatch(a, b.ExpressionString);
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 判断字符串是否和表达式一致
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(string a, Expression b) => !(a == b);
        /// <summary>
        /// 判断字符串是否和表达式一致
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Expression a, string b) => (b == a);
        /// <summary>
        /// 判断字符串是否和表达式一致
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Expression a, string b) => !(b == a);
        /// <summary>
        /// 重写的对比方法
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj) => (obj is Expression expression && this == expression) || (obj is string str && this == str);
        /// <summary>
        /// 重写的哈希编码规则
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => HashCode.Combine(ExpressionString, IsPlaceholder);
        /// <inheritdoc/>
        public int CompareTo(Expression other)
        {
            if (this == other)
            {
                return 0;
            }
            else
            {
                return other.ExpressionString.GetHashCode() - this.ExpressionString.GetHashCode();
            }
        }
        /// <inheritdoc/>
        public int Compare(Expression x, Expression y) => x.CompareTo(y);
    }
    /// <summary>
    /// 表达式比较法
    /// </summary>
    public class ExpressionCompare : IComparer<Expression[]>
    {
        /// <inheritdoc/>
        public int Compare(Expression[]? x, Expression[]? y) 
        {
            if (x.Compare(y))
            {
                return 0;
            }
            else
            {
                return (x?.GetHashCode() - y?.GetHashCode()) ?? 0;
            }
        }
    }

    /// <summary>
    /// 命令解释器
    /// </summary>
    public class Interpreter
    {
        SortedDictionary<Expression[], Action<string[], object?>> PatternDictionary { get; } = new(new ExpressionCompare());
        Action<object?> DefaultAction = (e) => { Console.WriteLine($"[MI { DateTime.Now:t}] :: No Such Command Even Exist"); };
        /// <summary>
        /// 是否标记日志
        /// </summary>
        public bool Log { private set; get; }
        /// <summary>
        /// 内部日志记录函数
        /// </summary>
        /// <param name="s"></param>
        private void LogInfo(string s)
        {
            if (Log)
            {
                Console.WriteLine($"[MI {DateTime.Now:t}] :: {s}");
            }
        }
        /// <summary>
        /// 生成一个解析器实例
        /// </summary>
        /// <param name="Log"></param>
        public Interpreter(bool Log = true)
        {
            this.Log = Log;
        }
        /// <summary>
        /// 尝试增加一条命令
        /// </summary>
        /// <param name="epls">命令列</param>
        /// <param name="action">需要执行的动作</param>
        /// <returns></returns>
        public bool InjectAction(Expression[] epls, Action<string[], object?> action) => PatternDictionary.TryAdd(epls, action);
        /// <summary>
        /// 注入命令空余操作
        /// </summary>
        /// <param name="DefaultAction">默认操作</param>
        public void InjectDefaultAction(Action<object?> DefaultAction) => this.DefaultAction = DefaultAction;
        /// <summary>
        /// 尝试获取命令要执行的操作
        /// </summary>
        /// <param name="context">文法</param>
        /// <returns></returns>
        public Action<string[], object?>? GetInterpreterAction(string[] context) => PatternDictionary.GetAction(context.StringDataToExpression());
        /// <summary>
        /// 开始执行对文法的解析(分割好的字符串组)
        /// </summary>
        /// <param name="context">操作句</param>
        /// <param name="action_object">传入参数</param>
        /// <returns>解析总计时间(ms)</returns>
        public double DoInterpret(string[] context, object? action_object = null)
        {
            var start = DateTime.Now;
            var k = GetInterpreterAction(context); // 获取命令执行的操作
            if (k != null)
            {
                k?.Invoke(context, action_object); // 如查找到则开始判断
            }
            else
            {
                DefaultAction.Invoke(action_object); // 未查找到
            }
            var end = DateTime.Now;
            var s = (end - start).TotalMilliseconds;
            LogInfo($"Interpreter Complete in {s} ms :: On Command {context}");
            return s;
        }
        /// <summary>
        /// 开始执行对文法的解析(字符串)
        /// <para>默认使用string.ToExpression()分割[Util扩展]</para>
        /// </summary>
        /// <param name="context">操作句</param>
        /// <param name="action_object">传入参数</param>
        /// <param name="default_Splitor">默认分隔符为空格,如果使用请注意不要使用和命令串一致的字符</param>
        /// <param name="default_command_placeholder">默认的命令串为$,如需要输入字符串$请输入两次</param>
        /// <param name="_filter_space">默认过滤空格,输入false不进行过滤</param>
        /// <returns>解析总计时间(ms)</returns>
        public double DoInterpret(string context, object? action_object = null, string default_Splitor = " ", string default_command_placeholder = "$", bool _filter_space = true)
        {
            var start = DateTime.Now;
            var cc = context.ToExpression(default_Splitor,default_command_placeholder,_filter_space);
            var k = PatternDictionary.GetAction(cc);
            if (k != null) //获取命令
            {
                k?.Invoke(cc.ExpressionDataToString(), action_object); // 如查找到则开始判断
            }
            else
            {
                DefaultAction?.Invoke(action_object); // 未查找到
            }
            var end = DateTime.Now;
            var s = (end - start).TotalMilliseconds;
            LogInfo($"Interpreter Complete in {s} ms :: On Command {context}");
            return s;
        }
        /// <summary>
        /// 初始化表
        /// </summary>
        public void DoInit()
        {
            var start = DateTime.Now;
            var cc = "".ToExpression();
            var d = PatternDictionary.GetAction(cc);
            if (d == null)
            {
                var end = DateTime.Now;
                var s = (end - start).TotalMilliseconds;
                LogInfo($"Interpreter Load in {s} ms :: On Init *Default null*");
            }
            else
            {
                var end = DateTime.Now;
                var s = (end - start).TotalMilliseconds;
                LogInfo($"Interpreter Load in {s} ms :: On Init *Substantial null*");
            }
        }
    }
    /// <summary>
    /// 泛型类解释器
    /// </summary>
    /// <typeparam name="T">泛型列</typeparam>
    public class Interpreter<T>
    {
        SortedDictionary<Expression[], Action<string[], T?>> PatternDictionary { get; } = new(new ExpressionCompare());
        Action<T?> DefaultAction = (e) => { Console.WriteLine($"[MI { DateTime.Now:t}] :: No Such Command Even Exist"); };
        private Action<string[], T?>? GetAction(SortedDictionary<Expression[], Action<string[], T?>> dic, Expression[] expr)
        {
            //return (from a in dic where a.Key.Compare(expr) == true select a).FirstOrDefault().Value; //much more expres
            foreach (var i in dic)
            {
                if (i.Key.Compare(expr))
                {
                    return i.Value;
                }
            }
            return null;
        }
        /// <summary>
        /// 是否标记日志
        /// </summary>
        public bool Log { private set; get; }
        /// <summary>
        /// 内部日志记录函数
        /// </summary>
        /// <param name="s"></param>
        private void LogInfo(string s)
        {
            if (Log)
            {
                Console.WriteLine($"[MI {DateTime.Now:t}] :: {s}");
            }
        }
        /// <summary>
        /// 生成一个解析器实例
        /// </summary>
        /// <param name="Log"></param>
        public Interpreter(bool Log = true)
        {
            this.Log = Log;
        }
        /// <summary>
        /// 尝试增加一条命令
        /// </summary>
        /// <param name="epls">命令列</param>
        /// <param name="action">需要执行的动作</param>
        /// <returns></returns>
        public bool InjectAction(Expression[] epls, Action<string[], T?> action) => PatternDictionary.TryAdd(epls, action);
        /// <summary>
        /// 注入命令空余操作
        /// </summary>
        /// <param name="DefaultAction">默认操作</param>
        public void InjectDefaultAction(Action<T?> DefaultAction) => this.DefaultAction = DefaultAction;
        /// <summary>
        /// 尝试获取命令要执行的操作
        /// </summary>
        /// <param name="context">文法</param>
        /// <returns></returns>
        public Action<string[], T?>? GetInterpreterAction(string[] context) => GetAction(PatternDictionary,context.StringDataToExpression());
        /// <summary>
        /// 开始执行对文法的解析(分割好的字符串组)
        /// </summary>
        /// <param name="context">操作句</param>
        /// <param name="action_object">传入参数</param>
        /// <returns>解析总计时间(ms)</returns>
        public double DoInterpret(string[] context, T? action_object)
        {
            var start = DateTime.Now;
            var k = GetInterpreterAction(context); // 获取命令执行的操作
            if (k != null)
            {
                k?.Invoke(context, action_object); // 如查找到则开始判断
            }
            else
            {
                DefaultAction.Invoke(action_object); // 未查找到
            }
            var end = DateTime.Now;
            var s = (end - start).TotalMilliseconds;
            LogInfo($"Interpreter Complete in {s} ms :: On Command {context}");
            return s;
        }
        /// <summary>
        /// 开始执行对文法的解析(字符串)
        /// <para>默认使用string.ToExpression()分割[Util扩展]</para>
        /// </summary>
        /// <param name="context">操作句</param>
        /// <param name="action_object">传入参数</param>
        /// <param name="default_Splitor">默认分隔符为空格,如果使用请注意不要使用和命令串一致的字符</param>
        /// <param name="default_command_placeholder">默认的命令串为$,如需要输入字符串$请输入两次</param>
        /// <param name="_filter_space">默认过滤空格,输入false不进行过滤</param>
        /// <returns>解析总计时间(ms)</returns>
        public double DoInterpret(string context, T? action_object, string default_Splitor = " ", string default_command_placeholder = "$", bool _filter_space = true)
        {
            var start = DateTime.Now;
            var cc = context.ToExpression(default_Splitor, default_command_placeholder, _filter_space);
            var k = GetAction(PatternDictionary,cc);
            if (k != null) //获取命令
            {
                k?.Invoke(cc.ExpressionDataToString(), action_object); // 如查找到则开始判断
            }
            else
            {
                DefaultAction?.Invoke(action_object); // 未查找到
            }
            var end = DateTime.Now;
            var s = (end - start).TotalMilliseconds;
            LogInfo($"Interpreter Complete in {s} ms :: On Command {context}");
            return s;
        }
        /// <summary>
        /// 初始化表
        /// </summary>
        public void DoInit()
        {
            var start = DateTime.Now;
            var cc = "".ToExpression();
            var d = GetAction(PatternDictionary,cc);
            if (d == null)
            {
                var end = DateTime.Now;
                var s = (end - start).TotalMilliseconds;
                LogInfo($"Interpreter Load in {s} ms :: On Init *Default null*");
            }
            else
            {
                var end = DateTime.Now;
                var s = (end - start).TotalMilliseconds;
                LogInfo($"Interpreter Load in {s} ms :: On Init *Substantial null*");
            }
        }
    }
}
