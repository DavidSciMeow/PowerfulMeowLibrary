
using Meow.Interpreter.Command;

namespace MeowCoreTest
{
    internal class Program
    {
		public static void Main(string[] args)
        {
            var ipt = new Interpreter();
            ipt.InjectAction("$^我觉得$".ToExpression(), (s, a) => { System.Console.WriteLine($"command 我觉得 {s[0]}"); });
            ipt.InjectAction("$对$$".ToExpression(), (s, a) => { System.Console.WriteLine($"command 我认为 {s[0]}"); });
            ipt.DoInit();

            while (true)
            {
                System.Console.WriteLine("---");
                ipt.DoInterpret(System.Console.ReadLine(),null);
            }
        }
    }
}

