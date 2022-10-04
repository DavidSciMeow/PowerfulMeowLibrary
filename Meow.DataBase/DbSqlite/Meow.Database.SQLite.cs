using System;
using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;

namespace Meow.DataBase.SQLite
{
    /// <summary>
    /// SQLite数据库帮助类
    /// </summary>
    public class SQLiteDBH : DbHelper, IDbHelper<SQLiteDBH>, IDisposable
    {
        /// <summary>
        /// DB文件
        /// </summary>
        public string DBfile { get; }
        private bool disposedValue;

        /// <summary>
        /// 生成一个SQLite数据库实例
        /// </summary>
        /// <param name="dBfile">文件存储位置</param>
        public SQLiteDBH(string dBfile)
        {
            DBfile = dBfile;
            conn = new SqliteConnection($"Data Source={dBfile}");
            conn.Open();
        }

        /// <inheritdoc/>
        public SQLiteDBH PrepareDb(string cmdText)
        {
            if (conn.State != ConnectionState.Open) { conn.Open(); }
            command = conn.CreateCommand();
            command.CommandText = cmdText;
            return this;
        }
        /// <inheritdoc/>
        public SQLiteDBH PrepareDb(string cmdText, params SqliteParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open) { conn.Open(); }
            command = conn.CreateCommand();
            command.CommandText = cmdText;
            if (cmdParms != null)
            {
                command.Parameters.Clear();
                foreach (var parm in cmdParms)
                {
                    command.Parameters.Add(parm);
                }
            }
            return this;
        }
        /// <inheritdoc/>
        public int ExecuteNonQuery() => command.ExecuteNonQuery();
        /// <inheritdoc/>
        public DataTable GetTable()
        {
            if (command.CommandText.Contains("SELECT"))
            {
                DataTable dt = new();
                dt.Load(command.ExecuteReader());
                return dt;
            }
            else
            {
                throw new Exception("No SELECT Present In Command, Please Use ExecuteNonQuery");
            }
        }

        /// <inheritdoc/>
        public bool SelectExist() => GetTable().Rows.Count > 0;
        /// <inheritdoc/>
        public DataRowCollection GetRows() => GetTable()?.Rows;
        /// <inheritdoc/>
        public T GetFirstRowItem<T>(string colname) => GetTable().Rows[0].Field<T>(colname);

        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <returns></returns>
        public bool DeleteDb()
        {
            File.Delete(DBfile);
            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    conn.Close();
                }

                disposedValue = true;
            }
        }
        /// <summary>
        /// 析构
        /// </summary>
        ~SQLiteDBH()
        {
            Dispose(disposing: false);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


    }
}

