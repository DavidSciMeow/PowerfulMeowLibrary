
using Meow.Interpreter.Command;

namespace MeowCoreTest
{
    internal class Program
    {
		public static void Main(string[] args)
        {
            var ipt = new Interpreter();
            ipt.InjectAction("天气 $ $".ToExpression(), (s, a) => { System.Console.WriteLine($"command 您要查询天气,{s[1]} {s[2]}"); });
            ipt.InjectAction("天气 $".ToExpression(), (s, a) => { System.Console.WriteLine($"command 您要查询天气,{s[1]}"); });
            ipt.InjectAction("a b $".ToExpression(), (s, a) => { System.Console.WriteLine($"command a,b,{s[2]}"); });
            ipt.DoInit();
            while (true)
            {
                System.Console.WriteLine("---");
                ipt.DoInterpret(System.Console.ReadLine(),null);
            }
        }
    }
}

