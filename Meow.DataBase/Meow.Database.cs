using System;
using System.Data;
using System.Data.Common;

namespace Meow.DataBase
{
    /// <summary>
    /// 数据库协助类基类接口模式
    /// </summary>
    /// <typeparam name="T">数据库类型</typeparam>
    public interface IDbHelper<T>
    {
        /// <summary>
        /// 执行非选择性查询
        /// </summary>
        /// <returns></returns>
        public int ExecuteNonQuery();
        /// <summary>
        /// 获取表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable();
        /// <summary>
        /// 准备命令并返回数据库类型的实例
        /// </summary>
        /// <param name="command">命令模式</param>
        /// <returns></returns>
        public T PrepareDb(string command);
    }
    /// <summary>
    /// 数据库协助类基类
    /// </summary>
    public abstract class DbHelper
    {
        /// <summary>
        /// 数据库链接
        /// </summary>
        public DbConnection conn;
        /// <summary>
        /// 数据库命令
        /// </summary>
        public DbCommand command;
        /// <summary>
        /// 日志模式
        /// </summary>
        public bool Log;
        /// <summary>
        /// 基类定义记录日志
        /// </summary>
        /// <param name="s"></param>
        protected void ExtLog(string s)
        {
            if (Log)
            {
                Console.WriteLine(s);
            }
        }
    }
}
