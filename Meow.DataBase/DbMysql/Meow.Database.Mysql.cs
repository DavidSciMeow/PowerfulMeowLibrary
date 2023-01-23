using Meow.DataBase;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Meow.Database.Mysql
{
    /// <summary>
    /// MysqlHelper
    /// <para>使用的时候自动包裹using语句即可</para>
    /// <para>when use this classes, don't forget using as our suggest Form as below</para>
    /// <code>
    /// using var dbc = new MysqlDBH(`ConnectionString`);
    /// </code>
    /// </summary>
    public class MysqlDBH : DbHelper, IDbHelper<MysqlDBH>, IDisposable
    {
        readonly System.Timers.Timer WatchDog;
        private void GlobalOpen()
        {
            if (conn.State is ConnectionState.Closed or ConnectionState.Broken)
            {
                conn.Open();
            }
            command = ((MySqlConnection)conn).CreateCommand();
            ExtLog($"[MYSQLWD] Conn Opened");
            if (KeepAlive)
            {
                WatchDog.Start();
                ExtLog($"[MYSQLWD] WatchDog Enabled");
                WatchDog.Elapsed += (e, c) =>
                {
                    ExtLog($"[MYSQLWD] PINGED");
                    if (((MySqlConnection)conn).Ping())//if ping errs is connection dropped
                    {
                        conn.Close();
                        if(conn.State is ConnectionState.Closed or ConnectionState.Broken)
                        {
                            conn.Open();
                        }
                    }
                };
            }
        }


        /// <summary>
        /// 保持长连接(默认保持)
        /// </summary>
        public bool KeepAlive { get; }
        /// <summary>
        /// 最大超时时间
        /// </summary>
        public int MaxTimedOut { get; set; } = 600000;
        /// <summary>
        /// 初始化一个Mysql连接(字符串初始化)
        /// <para>to Initialize a link with a ConnectionString</para>
        /// </summary>
        /// <param name="d">链接字符串</param>
        /// <param name="initOpen">是否初始化</param>
        /// <param name="log">是否记录日志</param>
        /// <param name="keepAlive">是否保持长连接</param>
        public MysqlDBH(string d, bool log = false, bool keepAlive = false)
        {
            conn ??= new MySqlConnection(d);
            WatchDog = new(MaxTimedOut);
            Log = log;
            KeepAlive = keepAlive;
            GlobalOpen();
        }
        /// <summary>
        /// 初始化一个Mysql连接(字符串初始化)
        /// <para>to Initialize a link with a ConnectionString</para>
        /// </summary>
        /// <param name="d">链接字符串</param>
        /// <param name="initOpen">是否初始化</param>
        /// <param name="log">是否记录日志</param>
        /// <param name="keepAlive">是否保持长连接</param>
        public MysqlDBH(MySqlConnection d, bool log = false, bool keepAlive = false)
        {
            conn ??= d;
            WatchDog = new(MaxTimedOut);
            Log = log;
            KeepAlive = keepAlive;
            GlobalOpen();
        }
        /// <summary>
        /// 初始化一个Mysql连接(字符串初始化)
        /// <para>to Initialize a link with a ConnectionString</para>
        /// </summary>
        /// <param name="initOpen">是否初始化</param>
        /// <param name="log">是否记录日志</param>
        /// <param name="keepAlive">是否保持长连接</param>
        /// <param name="DataBase">数据库</param>
        /// <param name="DataSource">数据源</param>
        /// <param name="Port">端口</param>
        /// <param name="UserId">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="Charset">字符集</param>
        /// <param name="otherParameter">其他添加函数</param>
        public MysqlDBH(
            string DataBase, string DataSource, string Port, string UserId, string password, string Charset = "utf8", string otherParameter = "", 
            bool log = false, bool keepAlive = false)
        {
            conn = new MySqlConnection(
               $"Database={DataBase};DataSource={DataSource};Port={Port};UserId={UserId};Password={password};Charset={Charset};{otherParameter}"
               );
            WatchDog = new(MaxTimedOut);
            Log = log;
            KeepAlive = keepAlive;
            GlobalOpen();
        }

        /// <inheritdoc/>
        public MysqlDBH PrepareDb(string cmdText, CommandType cmdType = CommandType.Text, params MySqlParameter[] commandParameters)
        {
            command.Connection = conn;
            command.CommandText = cmdText;
            command.CommandType = cmdType;
            if (commandParameters != null)
            {
                command.Parameters.Clear();
                foreach (MySqlParameter parm in commandParameters)
                {
                    command.Parameters.Add(parm);
                }
            }
            return this;
        }
        /// <inheritdoc/>
        public MysqlDBH PrepareDb(string cmdText, params MySqlParameter[] commandParameters)
        {
            command.Connection = conn;
            command.CommandText = cmdText;
            command.CommandType = CommandType.Text;
            if (commandParameters != null)
            {
                command.Parameters.Clear();
                foreach (MySqlParameter parm in commandParameters)
                {
                    command.Parameters.Add(parm);
                }
            }
            return this;
        }
        /// <inheritdoc/>
        public MysqlDBH PrepareDb(string cmdText)
        {
            command.Connection = conn;
            command.CommandText = cmdText;
            command.CommandType = CommandType.Text;
            return this;
        }


        /// <inheritdoc/>
        public int ExecuteNonQuery()
        {
            try
            {
                return ((MySqlCommand)command).ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }
        /// <inheritdoc/>
        public DataTable GetTable()
        {
            try
            {
                MySqlDataAdapter adapter = new();
                adapter.SelectCommand = (MySqlCommand)command;
                DataSet ds = new();
                adapter.Fill(ds);
                return ds.Tables[0];
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public bool SelectExist() => GetTable().Rows.Count > 0;
        /// <inheritdoc/>
        public DataRowCollection GetRows() => GetTable()?.Rows;
        /// <inheritdoc/>
        public R GetFirstRowItem<R>(string colname) => GetTable().Rows[0].Field<R>(colname);

        /// <summary>
        /// 默认关闭的Dispose
        /// <para>for using statement auto close conn</para>
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            try
            {
                conn.Close();
                conn.Dispose();
                WatchDog.Stop();
                WatchDog.Dispose();
                ExtLog($"[MYSQLWD] CLOSE");
            }
            catch
            {
                conn = null;
                GC.Collect();
            }
        }
    }
}
