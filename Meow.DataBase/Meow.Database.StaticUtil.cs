using Meow.DataBase.SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.DataBase
{
    /*
    public static class DbUtil
    {
        /// <summary>
        /// 转换C#类型到Sqlite数据类型
        /// </summary>
        /// <param name="t">类型模式T</param>
        /// <returns></returns>
        /// <exception cref="Exception">未支持模式或未支持转换目标</exception>
        public static string ConvertCSTypeIntoSqliteType(this Type t)
        {
            if (
                //t == typeof(ulong) ||
                t == typeof(long) ||
                t == typeof(int) ||
                t == typeof(uint) ||
                t == typeof(short) ||
                t == typeof(ushort) ||
                t == typeof(byte) ||
                t == typeof(sbyte) ||
                t == typeof(bool))
            {
                return "INTEGER";
            }
            else if (
                t == typeof(string) ||
                t == typeof(decimal) ||
                t == typeof(DateTime)
                )
            {
                return "TEXT";
            }
            else if (t == typeof(byte[]))
            {
                return "BLOB";
            }
            else if (
                t == typeof(float) ||
                t == typeof(double)
                )
            {
                return "REAL";
            }
            else
            {
                throw new Exception($"数据结构{t}未被Sqlite支持或未找到转换目标");
            }
        }
        /// <summary>
        /// 比较表结构 (判错模式,If-Excluded)
        /// <para>0 表同步, 完全一致</para>
        /// <para>-1 列不一致</para>
        /// <para>-2 主键数量不一致</para>
        /// <para>-10 某列名称不一致</para>
        /// <para>-11 某列数据类型不一致</para>
        /// <para>-12 某列唯一约束不一致</para>
        /// <para>-13 默认值不一致</para>
        /// <para>-14 步进模式不一致</para>
        /// <para>-15 步进模式种子不一致</para>
        /// <para>-16 步进值不一致</para>
        /// <para>-20 行数不一致</para>
        /// <para>-21 行内容数量不一致</para>
        /// <para>-22 行中有更改</para>
        /// </summary>
        /// <param name="dt1">表A</param>
        /// <param name="dt2">表B</param>
        /// <param name="C_ColName">对比列名(默认对比)</param>
        /// <param name="C_AutoIncre">对比自增列模式(默认不对比)</param>
        /// <param name="C_RowDiff">对比行数据(默认不对比)</param>
        /// <returns></returns>
        public static int CompareTableDiff(
            DataTable dt1, DataTable dt2
            , bool C_ColName = true, bool C_AutoIncre = false, bool C_RowDiff = false
            )
        {
            if (dt1.Columns.Count != dt2.Columns.Count)
            {
                return -1; //列不一致
            } //ALE
            if (dt1.PrimaryKey.Length != dt2.PrimaryKey.Length)
            {
                return -2; //主键数量不一致
            } //APLE
            for (int i = 0; i < dt1.Columns.Count; i++)
            {
                var dc1 = dt1.Columns[i];
                var dc2 = dt2.Columns[i];
                if (C_ColName)
                {
                    if (dc1.ColumnName != dc2.ColumnName) //主键比较位置
                    {
                        return -10; //某列名称不一致
                    }
                }
                if (dc1.DataType != dc2.DataType)
                {
                    return -11; //某列数据类型不一致
                }
                if (dc1.Unique != dc2.Unique)
                {
                    return -12; //某列唯一约束不一致
                }
                if (dc1.DefaultValue != dc2.DefaultValue)
                {
                    return -13; //默认值不一致
                }
                if (C_AutoIncre)
                {
                    if ((dc1.AutoIncrement == dc2.AutoIncrement))
                    {
                        if (dc1.AutoIncrementSeed != dc2.AutoIncrementSeed)
                        {
                            return -15; //步进模式种子不一致
                        }
                        if (dc1.AutoIncrementStep != dc2.AutoIncrementStep)
                        {
                            return -16; //步进值不一致
                        }
                    }
                    else
                    {
                        return -14; //步进模式不一致
                    }
                }

            } //结构逻辑
            if (C_RowDiff) //对比整个列表的行逻辑
            {
                if (dt1.Rows.Count != dt2.Rows.Count)
                {
                    return -20; //行数不一致
                }

                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    var drow1 = dt1.Rows[i];
                    var drow2 = dt2.Rows[i];

                    if (drow1.ItemArray.Length != drow2.ItemArray.Length)
                    {
                        return -21; //行内容数量不一致
                    }
                    if (!drow1.ItemArray.SequenceEqual(drow2.ItemArray)) //object * N/A
                    {
                        return -22; //行中有更改
                    }

                }
            }
            return 0; //结构一致
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enclosebody"></param>
        /// <returns></returns>
        public static DataTable SelectEntireTable(this (DataTable dt, SQLiteDBH helper) enclosebody)
            => enclosebody.helper.PrepareDb($"SELECT * FROM {enclosebody.dt.TableName}").GetTable();
        public static string CreateTableSQL(this DataTable dt
            //, ConstraintOrderMode COM = ConstraintOrderMode.BINARY, OrderType OT = OrderType.DESC
            )
        {
            var ss = $"CREATE TABLE {dt.TableName} (";
            string constrains = null;
            foreach (DataColumn col in dt.Columns)
            {
                //设置行
                ss += $" '{col.ColumnName}' {col.DataType.ConvertCSTypeIntoSqliteType()} {((!col.AllowDBNull) ? "NOT NULL" : "")},";
                //SQLITE
                检查唯一性 且 非主键
                if (col.Unique && !((from a in dt.PrimaryKey where a == col select a).Any()) )
                {
                    ss += $" CONSTRAINT '{col.ColumnName}' UNIQUE ('{col.ColumnName}' COLLATE {COM} {OT}),";
                }
                
            }
            foreach (DataColumn col in dt.PrimaryKey)
            {
                constrains = $"PRIMARY KEY ('{col.ColumnName}'),";
            }
            ss = ss[0..^1] + $"{(constrains == null ? "" : $", {constrains[0..^1]}")});";
            return ss;
        }
        public static string AlterTableName(string name, string alterto)
            => $"ALTER TABLE {name} RENAME TO {alterto};";
        public static string AlterTableColumnType(string tablename, string columnname, string columntype, bool notnull = false)
            => $"ALTER TABLE {tablename} ALTER COLUMN {columnname} {columntype} {(notnull ? "NOT NULL" : "")};";


        public static void DataRowTableSQLiteMonitor(this SQLiteDBH sh, DataTable dt)
        {
            var tn = dt.TableName;
            dt.TableNewRow += (s, e) =>
            {
                Console.WriteLine($"NEW ROW : {e.Row.ItemArray}");
                /*
                var colx = "(";
                var valx = "(";
                List<Microsoft.Data.Sqlite.SqliteParameter> slp = new();
                foreach(DataColumn col in dt.Columns)
                {
                    colx += $"'{col.ColumnName}',";
                    valx += $"@{col.ColumnName}";
                    slp.Add(new Microsoft.Data.Sqlite.SqliteParameter(col.ColumnName, e.Row[col]));
                }
                var rr = sh.PrepareDb($"INSERT INTO {tn} {colx} VALUES {colx}", slp.ToArray()).ExecuteNonQuery();
                if(rr == 0)
                {

                }
            };//添加行
            dt.RowChanged += (s, e) =>
            {
                Console.WriteLine($"{e.Action} {e.Row.ItemArray}");
            };//更新行
            dt.RowDeleted += (s, e) =>
            {
                Console.WriteLine($"{e.Action} {e.Row.ItemArray}");
            };//删除行
            dt.TableCleared += (s, e) =>
            {
                Console.WriteLine($"{e.TableName} CLEARED");
            };//表清空
            dt.ColumnChanged += (s, e) =>
            {
                Console.WriteLine($"{e.Column.ColumnName} IS ALTERING TABLE");
            };//表结构更改
        }
    }
    */

    /// <summary>
    /// SQLite数据处理协助类
    /// </summary>
    public static class SQLiteDataHelper
    {
        /// <summary>
        /// 获得一个整数
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public static long GetInt(this DataRow dr, string FieldName) => dr.Field<long>(FieldName);
        /// <summary>
        /// 获得一个字符串
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public static string GetString(this DataRow dr, string FieldName) => dr.Field<string>(FieldName);
        public static double GetFloat(this DataRow dr, string FieldName) => dr.Field<double>(FieldName);
        public static byte[] GetBLOB(this DataRow dr, string FieldName) => dr.Field<byte[]>(FieldName);
        public static T Get<T>(this DataRow dr, string FieldName) => dr.Field<T>(FieldName);
    }
}
