using Meow.Database.Mysql;

namespace MeowCoreTest
{
    internal class Program
    {
		public static void Main(string[] args)
        {
            using var d = ReturnService();
            var k = d.PrepareDb("SELECT qid," +
                "rinkorushenable,rinkorushtype,rinkorushscore," +
                "rinkorushdndtimestart,rinkorushdndtimeend" +
                " FROM RinkoUser").GetDataSet();
            System.Console.WriteLine(k);
        }
        public static MysqlDBH ReturnService()
        {
            return new MysqlDBH("rinko", "112.6.216.175", "12306", "rinkobot", "rinkobot");
        }
    }
}

