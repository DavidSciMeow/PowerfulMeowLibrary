using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meow.DataBase.SQLite
{
    /// <summary>
    /// 唯一键排序类型
    /// </summary>
    public enum ConstraintOrderMode
    {
        /// <summary>
        /// 基本(二进制形式的)
        /// </summary>
        BINARY,
        /// <summary>
        /// 不区分大小写
        /// </summary>
        NOCASE,
        /// <summary>
        /// 右侧空格清除
        /// </summary>
        RTRIM
    }
    /// <summary>
    /// 排序模式
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// 降序
        /// </summary>
        DESC,
        /// <summary>
        /// 升序
        /// </summary>
        ASC
    }
}
