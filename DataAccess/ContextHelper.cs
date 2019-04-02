using Chloe;
using Chloe.MySql;
using Chloe.PostgreSQL;
using Chloe.SQLite;
using Chloe.SqlServer;
using DataAccess.DbConnectionFactory;
using System;
using System.Configuration;
using System.Data;

namespace DataAccess
{
    public class ContextHelper
    {
        public static DbContext GetContext()
        {
            string dbtype = ConfigurationManager.AppSettings["dbtype"];
            if (dbtype.ToLower() == "sqlite")
            {
                string conString = ConfigurationManager.AppSettings["sqlite"];
                var factory = new SQLiteConnectionFactory(conString);
                return new SQLiteContext(factory);
            }
            if (dbtype.ToLower() == "mysql")
            {
                string conString = ConfigurationManager.AppSettings["mysql"];
                var factory = new MySqlConnectionFactory(conString);
                return new MySqlContext(factory);
            }
            if (dbtype.ToLower() == "sqlserver")
            {
                string conString = ConfigurationManager.AppSettings["sqlserver"];
                return new MsSqlContext(conString);
            }
            if (dbtype.ToLower() == "postgre")
            {
                string conString = ConfigurationManager.AppSettings["postgre"];
                var factory = new PostgreSQLConnectionFactory(conString);
                return new PostgreSQLContext(factory);
            }
            throw new Exception("暂不支持此类型数据库");
        }

        public static DataTable GetTable(string sql)
        {
            var context = GetContext();
            return context.Session.ExecuteDataTable(sql);
        }

        public static int ExcuteSql(string sql, DbParam[] parameters)
        {
            try
            {
                var context = GetContext();
                if (parameters == null || parameters.Length < 1)
                {
                    return context.Session.ExecuteNonQuery(sql);
                }
                else
                {
                    return context.Session.ExecuteNonQuery(sql, parameters);
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Duplicate entry"))
                {
                    return 1;
                }
                throw e;
            }
        }
    }
}