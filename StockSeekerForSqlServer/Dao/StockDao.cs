using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SoufunLab.Framework;
using SoufunLab.Framework.Data;
using XjsStock.Bean;

namespace XjsStock.Dao
{
   public  class StockDao:BaseDao<StockBean>
    {
        public override string Columns
        {
            get
            {
                return @"
                            [CreateDay]
                           ,[ID] 
                           ,[Name] 
                    ";
            }
        }

        public override string TableName
        {
            get { return "Stock"; }
        }

        public override void SetBean(System.Data.DataRow row, StockBean bean)
        {
            bean.CreateDay = SlConvert.TryToDateTime(row["CreateDay"], Convert.ToDateTime("1900-01-01"));
            bean.ID = SlConvert.TryToString(row["ID"]);
            bean.Name = SlConvert.TryToString(row["Name"], string.Empty);

        }

        public  long Add(string databaseConnectionString, StockBean bean)
        {
            var columns = new List<string>();
            var values = new List<string>();
            var param = new List<SqlParameter>();

            if (bean.CreateDay.Year > 1900)
            {
                columns.Add("CreateDay");
                values.Add("@CreateDay");
                param.Add(new SqlParameter("@CreateDay", bean.CreateDay));
            }


            columns.Add("ID");
            values.Add("@ID");
            param.Add(new SqlParameter("@ID", bean.ID));

            if (bean.Name != null)
            {
                columns.Add("Name");
                values.Add("@Name");
                param.Add(new SqlParameter("@Name", bean.Name));
            }
            
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("insert into "+ TableName +" ( " + string.Join(",", columns) + " ) values ( " + string.Join(",", values) + " ) ; ");
            return SlDatabase.ExecuteNonQuery(databaseConnectionString, sbSql.ToString(), param.ToArray());
        }

        public void Update(string databaseConnectionString, StockBean bean)
        {
            string sql = "update stock set name='{0}' where id='{1}'";
            sql = string.Format(sql, bean.Name, bean.ID);
            SlDatabase.ExecuteNonQuery(databaseConnectionString, sql, null);
        }
         
    }
}
