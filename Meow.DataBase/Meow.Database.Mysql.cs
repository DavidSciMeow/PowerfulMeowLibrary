using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
    public class MysqlDBH : IDisposable
    {
        string cmdText;
        MySqlParameter[] commandParameters;
        CommandType cmdType;
        readonly System.Timers.Timer WatchDog;
        MySqlConnection conn = null;

        /// <summary>
        /// 保持长连接(默认保持)
        /// </summary>
        public bool KeepAlive { get; }
        /// <summary>
        /// 是否记录日志
        /// </summary>
        public bool Log { get; }
        /// <summary>
        /// 最大超时时间
        /// </summary>
        public int MaxTimedOut { get; set; } = 600000;

        /// <summary>
        /// 初始化一个Mysql连接(字符串初始化)
        /// <para>to Initialize a link with a ConnectionString</para>
        /// </summary>
        /// <param name="d">
        /// 连接字符串
        /// <para>ConnectionString</para>
        /// <param name="initOpen">
        /// 构造时链接
        /// <para>ConnectWhenConstruct</para>
        public MysqlDBH(string d, bool initOpen = false, bool log = false, bool keepAlive = false)
        {
            conn ??= new MySqlConnection(d);
            WatchDog = new(MaxTimedOut);
            if (initOpen)
            {
                GlobalOpen();
            }
            Log = log;
            KeepAlive = keepAlive;
        }
        /// <summary>
        /// 初始化一个Mysql连接(实例初始化)
        /// <para>init instance with a Connection Instance</para>
        /// </summary>
        /// <param name="d">
        /// 一个Mysql连接实例
        /// <para>An Instance of Connection</para>
        /// <param name="initOpen">
        /// 构造时链接
        /// <para>ConnectWhenConstruct</para>
        public MysqlDBH(MySqlConnection d, bool initOpen = false, bool log = false, bool keepAlive = false)
        {
            conn ??= d;
            WatchDog = new(MaxTimedOut);
            if (initOpen)
            {
                GlobalOpen();
            }
            Log = log;
            KeepAlive = keepAlive;
        }
        /// <summary>
        /// 根据参数初始化一个Mysql连接
        /// <para>by define of MySql's Connection</para>
        /// </summary>
        /// <param name="DataBase">数据库名称<para>Database's Name</para></param>
        /// <param name="DataSource">源地址<para>DataSource</para></param>
        /// <param name="Port">端口<para>Port of database</para></param>
        /// <param name="UserId">用户名<para>User Name</para></param>
        /// <param name="password">密码<para>Password</para></param>
        /// <param name="Charset">字符集(默认utf8)<para>Charachter Set</para></param>
        /// <param name="otherParameter">其他子串,如果有默认以;结尾<para>Other Parameter (mustends with ';')</para></param>
        /// <param name="initOpen">
        /// 构造时链接
        /// <para>ConnectWhenConstruct</para>
        public MysqlDBH(string DataBase, string DataSource, string Port, string UserId, string password, string Charset = "utf8", string otherParameter = "", bool initOpen = false, bool log = false, bool keepAlive = false)
        {
            conn = new MySqlConnection(
               $"Database={DataBase};DataSource={DataSource};Port={Port};UserId={UserId};Password={password};Charset={Charset};{otherParameter}"
               );
            WatchDog = new(MaxTimedOut);
            if (initOpen)
            {
                GlobalOpen();
            }
            Log = log;
            KeepAlive = keepAlive;
        }

        void ExtLog(string s)
        {
            if (Log)
            {
                Console.WriteLine(s);
            }
        }

        /// <summary>
        /// 长连接全局开启
        /// </summary>
        public void GlobalOpen()
        {
            conn.Open();
            if (KeepAlive)
            {
                ExtLog($"[MYSQLWD] KEEP ALIVE OPEN");
                WatchDog.Start();
                WatchDog.Elapsed += (e, c) =>
                {
                    ExtLog($"[MYSQLWD] PINGED");
                    conn.Ping();
                };
            }
        }

        /// <summary>
        /// 准备一个数据库查询操作
        /// <para>prepare a command for sql- which is your statement</para>
        /// </summary>
        /// <param name="cmdText">
        /// 执行的SQL命令
        /// <para>the SQL Command that you want to send</para>
        /// </param>
        /// <param name="cmdType">
        /// 命令模式*一般使用Text*输入`default`不更改本项
        /// <para>Command MODE, like wise we all using Text, which you can type Default.</para>
        /// </param>
        /// <param name="commandParameters">命令参数</param>
        /// <returns>返回一个可连写的DBH</returns>
        public MysqlDBH PrepareDb(string cmdText, CommandType cmdType = CommandType.Text, params MySqlParameter[] commandParameters)
        {
            this.cmdText = cmdText;
            this.commandParameters = commandParameters;
            this.cmdType = cmdType;
            return this;
        }
        /// <summary>
        /// 准备一个数据库查询操作
        /// <para>prepare a command for sql- which is your statement</para>
        /// </summary>
        /// <param name="cmdText">
        /// 执行的SQL命令
        /// <para>the SQL Command that you want to send</para>
        /// </param>
        /// <param name="commandParameters">命令参数</param>
        /// <returns>返回一个可连写的DBH</returns>
        public MysqlDBH PrepareDb(string cmdText, params MySqlParameter[] commandParameters)
        {
            this.cmdText = cmdText;
            this.commandParameters = commandParameters;
            this.cmdType = CommandType.Text;
            return this;
        }
        /// <summary>
        /// 准备一个数据库查询操作
        /// <para>prepare a command for sql- which is your statement</para>
        /// </summary>
        /// <param name="cmdText">
        /// 执行的SQL命令
        /// <para>the SQL Command that you want to send</para>
        /// </param>
        /// <returns>返回一个可连写的DBH</returns>
        public MysqlDBH PrepareDb(string cmdText)
        {
            this.cmdText = cmdText;
            this.cmdType = CommandType.Text;
            return this;
        }

        /// <summary>
        /// 执行一个无返回值的SQL操作(非查询类)
        /// <para>perform an action that without SQL SELECT.(mostly)</para>
        /// </summary>
        /// <returns>
        /// 操作的行数
        /// <para>Infected lines</para>
        /// </returns>
        public int ExecuteNonQuery()
        {
            try
            {
                using MySqlCommand cmd = new();
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                return val;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 获取查询操作的一个Reader
        /// <para>Get an SQL reader (which SELECT)</para>
        /// </summary>
        /// <returns>
        /// 一个Reader
        /// <para>a Reader</para>
        /// </returns>
        public MySqlDataReader ExecuteReader()
        {
            try
            {
                using MySqlCommand cmd = new();
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 获取一个DataSet对象(查询操作)
        /// <para>get A DataSet Instance (For SELECT)</para>
        /// </summary>
        /// <returns>
        /// 一个DataSet
        /// <para>a DataSet</para>
        /// </returns>
        public DataSet GetDataSet()
        {
            try
            {
                using MySqlCommand cmd = new();
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                MySqlDataAdapter adapter = new();
                adapter.SelectCommand = cmd;
                DataSet ds = new();
                adapter.Fill(ds);
                return ds;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 获取结果表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable()
        {
            try
            {
                using MySqlCommand cmd = new();
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                MySqlDataAdapter adapter = new();
                adapter.SelectCommand = cmd;
                DataSet ds = new();
                adapter.Fill(ds);
                return ds.Tables[0];
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 默认关闭的Dispose
        /// <para>for using statement auto close conn</para>
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            try
            {
                GC.ReRegisterForFinalize(this);
            }
            catch
            {
                conn = null;
                GC.Collect();
            }
        }
        /// <summary>
        /// 析构
        /// </summary>
        ~MysqlDBH()
        {
            conn.Close();
            conn.Dispose();
            WatchDog.Stop();
            WatchDog.Dispose();
            ExtLog($"[MYSQLWD] CLOSE");
        }
        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn,
            MySqlTransaction trans, CommandType cmdType, string cmdText,
            MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open) { conn.Open(); }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;
            cmd.Transaction = trans;
            if (cmdParms != null)
            {
                cmd.Parameters.Clear();
                foreach (MySqlParameter parm in cmdParms)
                {
                    cmd.Parameters.Add(parm);
                }
            }
        }
    }
}
