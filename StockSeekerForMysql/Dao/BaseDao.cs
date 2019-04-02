using System;
using System.Collections.Generic;
using System.Data;
using XjsStock.Bean;

namespace XjsStock.Dao
{
    public abstract class BaseDao<T> where T : BaseBean, new()
    {
        #region abstract Files&Methods
        public abstract string Columns { get; }

        public abstract string TableName { get; }

        /// <summary>
        /// 一行数据设置一个对象
        /// </summary>
        /// <param name="row"></param>
        /// <param name="bean"></param>
        public abstract void SetBean(DataRow row, T bean);

        #endregion  abstract Files&Methods

        /// <summary>
        /// 获取全部
        /// </summary>
        public string SelectAll
        {
            get { return " SELECT   " + Columns + " FROM " + TableName ; }
        }


        /// <summary>
        /// 一个表转一个list
        /// </summary>
        /// <param name="beans"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public void SetBeans(List<T> beans, DataTable table)
        {
            if (null == beans)
            {
                beans = new List<T>();
            }
            if (null != table)
            {
                foreach (DataRow row in table.Rows)
                {
                    T bean = new T();
                    SetBean(row, bean);
                    beans.Add(bean);
                }
            }
        }
        
  
        #region 数据转换

        protected string TryToString(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName))
            {
                return row[columnName].ToString();
            }
            return string.Empty;
        }

        protected int TryToInt32(DataRow row, string columnName, Int32 defaultValue = 0)
        {
            if (row.Table.Columns.Contains(columnName))
            {
                return Int32.Parse(row[columnName].ToString());
            }
            return 0;
        }

        protected long TryToInt64(DataRow row, string columnName, Int64 defaultValue = 0)
        {
            if (row.Table.Columns.Contains(columnName))
            {
                return Int64.Parse(row[columnName].ToString());
            }
            return 0;
        }

        protected Decimal TryToDecimal(DataRow row, string columnName, Decimal defaultValue = 0)
        {
            if (row.Table.Columns.Contains(columnName))
            {
                return Decimal.Parse(row[columnName].ToString());
            }
            return 0;
        }

        protected DateTime TryToDateTime(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName))
            {
                return DateTime.Parse(row[columnName].ToString());
            }
            return DateTime.Parse("1900-01-01");
        }

        #endregion 数据转换
    }
}
