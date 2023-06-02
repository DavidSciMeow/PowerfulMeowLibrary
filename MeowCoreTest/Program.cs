using Meow.Util.Network.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MeowCoreTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if(args.Length == 1)
            {
                var d = Meow.Weather.CN.Interpreter.RectifyWord(args[0]);
                Console.WriteLine(d.Data.Count);
                foreach(var i in d.Data)
                {
                    Console.WriteLine($"{i.DataTime} -> {i.ImgUrl}");
                }
            }
            else if(args.Length > 0) 
            {
                var d = Meow.Weather.CN.Interpreter.RectifyWord(args);
                d.Wait();
                Console.WriteLine(d.Result);
            }
            else
            {
                Console.WriteLine("usage errs");
            }
        }
    }
}


