using Meow.Database.Mysql;
using System;

namespace MeowCoreTest
{
    internal class Program
    {
        static readonly MysqlDBH ms = new("rinko", "112.6.216.175", "12306", "rinkobot", "rinkobot",log:true,keepAlive:true);
        public static void Main(string[] args)
        {
            while (true)
            {
                var cr = Console.ReadLine();
                if ("t".Equals(cr))
                {
                    var s = ms.PrepareDb("SELECT * FROM ServiceGroup").GetTable();
                    Console.WriteLine(s.Columns.Count);
                }
            }
        }
    }
}

