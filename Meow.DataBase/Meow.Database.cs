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
        int ExecuteNonQuery();
        /// <summary>
        /// 获取表
        /// </summary>
        /// <returns></returns>
        DataTable GetTable();

        /// <summary>
        /// 选取的表内含有行(布尔检查)
        /// </summary>
        /// <returns></returns>
        bool SelectExist();
        /// <summary>
        /// 获取行(如果有)
        /// </summary>
        /// <returns></returns>
        DataRowCollection GetRows();
        /// <summary>
        /// 获取第一行的某一列内容
        /// </summary>
        /// <typeparam name="R">列数据结构</typeparam>
        /// <param name="colname">列名</param>
        /// <returns></returns>
        R GetFirstRowItem<R>(string colname);
        /// <summary>
        /// 准备命令并返回数据库类型的实例
        /// </summary>
        /// <param name="command">命令模式</param>
        /// <returns></returns>
        T PrepareDb(string command);
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
