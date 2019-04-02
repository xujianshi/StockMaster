using Chloe;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using XjsStock.Bean;

namespace XjsStock.Dao
{
    public class StockDao : BaseDao<StockBean>
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
            bean.CreateDay = DateTime.Parse(row["CreateDay"].ToString());
            bean.ID = (row["ID"].ToString());
            bean.Name = row["Name"].ToString();
        }

        public long Add(StockBean bean)
        {
            var columns = new List<string>();
            var values = new List<string>();
            var param = new List<DbParam>();

            if (bean.CreateDay.Year > 1900)
            {
                columns.Add("CreateDay");
                values.Add("@CreateDay");
                param.Add(new DbParam("@CreateDay", bean.CreateDay));
            }

            columns.Add("ID");
            values.Add("@ID");
            param.Add(new DbParam("@ID", bean.ID));

            if (bean.Name != null)
            {
                columns.Add("Name");
                values.Add("@Name");
                param.Add(new DbParam("@Name", bean.Name));
            }

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("insert into " + TableName + " ( " + string.Join(",", columns) + " ) values ( " + string.Join(",", values) + " ) ; ");
            return ContextHelper.ExcuteSql(sbSql.ToString(), param.ToArray());
        }

        public void Update(StockBean bean)
        {
            string sql = "update stock set name='{0}' where id='{1}'";
            sql = string.Format(sql, bean.Name, bean.ID);
            ContextHelper.ExcuteSql(sql, null);
        }
    }
}