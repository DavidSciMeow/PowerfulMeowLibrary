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
    /// <summary>
    /// Sqlite表结构不一致类型
    /// </summary>
    public enum SqliteTableStructureDiff
    {
        /// <summary>
        /// 一致
        /// </summary>
        Synced,
        /// <summary>
        /// 列不一致(数量)
        /// </summary>
        Col_Count,
        /// <summary>
        /// 主键数量不一致
        /// </summary>
        PK_Length,
        /// <summary>
        /// 某列名称不一致
        /// </summary>
        Col_Name,
        /// <summary>
        /// 某列数据不一致
        /// </summary>
        Col_Type,
        /// <summary>
        /// 某列唯一约束不一致
        /// </summary>
        Col_Unique,
        /// <summary>
        /// 某列默认值不一致
        /// </summary>
        Col_DefaultValue,
        /// <summary>
        /// 某列步进模式种子不一致
        /// </summary>
        Col_AutoIncrementSeed,
        /// <summary>
        /// 某列步进模式步长不一致
        /// </summary>
        Col_AutoIncrementStep,
        /// <summary>
        /// 某列自动步进模式不一致
        /// </summary>
        Col_AutoIncrementMode,
        /// <summary>
        /// 行数不一致
        /// </summary>
        Rows_Count,
        /// <summary>
        /// 行内数据不一致
        /// </summary>
        Rows_ItemArray,
        /// <summary>
        /// 行内数据有改动
        /// </summary>
        Rows_Item
    }
}
