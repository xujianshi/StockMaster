using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SoufunLab.Framework.Data;

namespace XjsStock
{
    /// <summary>
    /// 增加数据库中转统一控制函数
    /// 更新类操作，必须记录日志。查询类操作可以增加监听控制
    /// jsxu 2016-3-4 12:49:19
    /// </summary>
    public class DbHelper
    {
        public static DataTable Query(string connectionString, string sentence)
        {
            DateTime starTime = DateTime.Now;
            DataTable dtResult = new DataTable("TuanTable");
            SlDatabase.Fill2016(connectionString, dtResult, sentence);
            return dtResult;
        }
    }
}
