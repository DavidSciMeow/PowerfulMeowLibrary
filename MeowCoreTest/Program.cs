using System;
using System.Linq;

namespace MeowCoreTest
{
    internal class Program
    {
		public static void Main(string[] args)
        {
            var k = from a in args where a != null && a.Length > 0 select a;
            k.
            new Program().Select().From("").Where("").OrderBy().GroupBy().Limit(1000)
        }
        //select entire table
        public Program Select(params object[] columnSelection)
        {
            return this;
        }
        public Program From(string tableName)
        {
            return this;
        }
        public Program Where(string Caluse)
        {
            return this;
        }
        public Program OrderBy(params object[] columnSelection)
        {
            return this;
        }
        public Program GroupBy(params object[] columnSelection)
        {
            return this;
        }
        public Program Limit(long num)
        {
            return this;
        }
        //select table with some col.
        public static void Select(string table, params object[] columnSelection)
        {

        }
        //select table with limitation
        public static void Select(string table, long limitation, params object[] columnSelection)
        {

        }
    }
}

