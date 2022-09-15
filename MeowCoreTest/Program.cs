using System;
using System.Data;
using Meow.DataBase;
using Meow.DataBase.SQLite;

namespace MeowCoreTest
{
    internal class Program
    {
		public static void Main(string[] args)
        {


            SQLiteDBH s = new("D:\\Users\\David\\Desktop\\testdb2.db");
            
            //var r = s.PrepareDb("CREATE TABLE 'abc' ( 'id' INTEGER NOT NULL, 'text' TEXT, PRIMARY KEY('id'));").ExecuteNonQuery();

            //var r = s.PrepareDb("INSERT INTO dbt (id,text,double) VALUES (@id,@text,@double)",
            //    new("id",10),new("text","2xd"),new("double",1.116)).ExecuteNonQuery();

            //var r = s.PrepareDb("SELECT * FROM abc").GetTable();

            //var row = inx.Rows[0];
            //Console.WriteLine(row.Field<long>("id"));

            DataTable r = new("tite");
            s.DataRowTableSQLiteMonitor(r);
            var row = new DataColumn("name", typeof(string));
            var row2 = new DataColumn("typex", typeof(string));
            row2.Unique = true;
            r.Columns.Add(row);
            r.Columns.Add(row2);
            r.PrimaryKey = new DataColumn[] { row };

            /*
            try
            {
                Console.WriteLine(s.PrepareDb(r.CreateTableSQL()).ExecuteNonQuery() == -2 ? "TABLE EXIST" : "CREATED");
            }
            catch (Microsoft.Data.Sqlite.SqliteException ex)
            {
                if(ex.SqliteErrorCode == 1 && ex.Message.Contains("table tite already exists"))
                {
                    Console.WriteLine("TABLE EXISTS");
                }
            }
            */
            
            var tbfo = (r, s).SelectEntireTable();
            var diff = DbUtil.CompareTableDiff(r,tbfo);
            Console.WriteLine(diff);

            var nr1 = r.NewRow();
            nr1["name"] = "a";

            r.Rows.Add(nr1);

            while (true)
            {
                var k = Console.ReadLine().Split(' ');
                if (k[0] == ("add"))
                {
                }
                else if (k[0] == ("alt"))
                {
                }
                else if (k[0] == ("del"))
                {
                }
                r.AcceptChanges();
            }
        }
        /*
         查：

        DataTable dt =数据库查询集合;
        DataRow[] dr = dt.Select("Id=" + txt_Id.Value);
        txt_Name.Value = dr[0]["Name"].ToString();
        txt_Add.Value = dr[0]["Add"].ToString();

        增：

        DataTable dataTable = 数据库查询集合；
        dataTable. Rows.Add(new object[] { Name, Sex,Add,Tel });
        数据列表绑定(dataTable);

        删：

        DataTable dTable =数据库查询集合；
        dTable.Rows.Remove(dTable.Select("Id=2”)[0]);
        数据列表绑定(dataTable);

        改：

        DataTable dTable =数据库查询集合；
        DataRow dRow = dTable.Select("Id=3")[0];
        dRow.BeginEdit();
        dRow["Name"] = txt_Name.Value.Trim();
        dRow["Add"] = txt_Add.Value.Trim();
        dRow.EndEdit();
        数据列表绑定(dTable);
         */


    }
}

