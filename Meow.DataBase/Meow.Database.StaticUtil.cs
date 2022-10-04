using Meow.DataBase.SQLite;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.DataBase
{
    /// <summary>
    /// 数据库通用模式句
    /// </summary>
    public static class SqlGen
    {
        /// <summary>
        /// 表换名
        /// </summary>
        /// <param name="name">原名</param>
        /// <param name="alterto">更改到</param>
        /// <returns></returns>
        public static string AlterTableName(string name, string alterto) 
            => $"ALTER TABLE {name} RENAME TO {alterto};";
        /// <summary>
        /// 更换单列类型
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="columnname">列名</param>
        /// <param name="columntype">列类型</param>
        /// <param name="notnull">是否为空</param>
        /// <returns></returns>
        public static string AlterTableColumnType(string tablename, string columnname, string columntype, bool notnull = false) 
            => $"ALTER TABLE {tablename} ALTER COLUMN {columnname} {columntype} {(notnull ? "NOT NULL" : "")};";
        /// <summary>
        /// 删除一行(带有主键的)
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="PKcolname"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static string DeleteRowWhere(string tablename, string PKcolname, string identifier) 
            => $"DELETE FROM {tablename} WHERE {PKcolname} = {identifier}";
        /// <summary>
        /// 删除某一行(按照行数)
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="rowid"></param>
        /// <returns></returns>
        public static string DeleteRowWhere(string tablename, int rowid) 
            => $"DELETE FROM {tablename} WHERE rowid = {rowid}";

    }
}
