using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using DataAccess;
using XjsStock.Bean;

namespace XjsStock.Dao
{
    public  class StockDao:BaseDao<StockBean>
    {
        public override string Columns
        {
            get
            {
                return @"    CreateDay,`ID`,`Name`         ";
            }
        }

        public override string TableName
        {
            get { return "Stock"; }
        }

        public override void SetBean(System.Data.DataRow row, StockBean bean)
        {
            bean.CreateDay =DateTime.Parse(row["CreateDay"].ToString());
            bean.ID =(row["ID"].ToString());
            bean.Name = row["Name"].ToString();

        }

        public  long Add(StockBean bean)
        {
            string sql = $"insert into Stock ( `ID`,`Name` ) values ( '{bean.ID}','{bean.Name}' ) ; ";
            return ContextHelper.ExcuteSql(sql,null);
        }

        public void Update(StockBean bean)
        {
            string sql = "update stock set name='{0}' where id='{1}'";
            sql = string.Format(sql, bean.Name, bean.ID);
            ContextHelper.ExcuteSql(sql,null);
        }
         
    }
}
