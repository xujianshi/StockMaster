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
    public class StockPriceDao : BaseDao<StockPriceBean>
    {
        public override string Columns
        {
            get
            { 
                return  @"
                           [ClosePrice] 
                           ,[Code] 
                           ,[HighPrice] 
                           ,[HuanShou] 
                           ,[LowPrice] 
                           ,[OpenPrice] 
                           ,[Rq]                            
                           ,[ZhangFu] ";
               }
        }

        public override string TableName
        {
            get { return "StockPrice"; }
        }

        public override void SetBean(System.Data.DataRow row, StockPriceBean bean)
        {
            bean.ClosePrice = SlConvert.TryToDecimal(row["ClosePrice"], 0);
            bean.Code = SlConvert.TryToString(row["Code"]);
            bean.HighPrice = SlConvert.TryToDecimal(row["HighPrice"], 0);
            bean.HuanShou = SlConvert.TryToDecimal(row["HuanShou"], 0);
            bean.LowPrice = SlConvert.TryToDecimal(row["LowPrice"], 0);
            bean.OpenPrice = SlConvert.TryToDecimal(row["OpenPrice"], 0);
            bean.Rq = SlConvert.TryToDateTime(row["Rq"], Convert.ToDateTime("1900-01-01"));
            bean.ZhangFu = SlConvert.TryToDecimal(row["ZhangFu"], 0);
        }

        public  long Add(string databaseConnectionString, StockPriceBean bean)
        {
            var columns = new List<string>();
            var values = new List<string>();
            var param = new List<SqlParameter>();

            columns.Add("ClosePrice");
            values.Add("@ClosePrice");
            param.Add(new SqlParameter("@ClosePrice", bean.ClosePrice));

            columns.Add("code");
            values.Add("@Code");
            param.Add(new SqlParameter("@Code", bean.Code));

            columns.Add("HighPrice");
            values.Add("@HighPrice");
            param.Add(new SqlParameter("@HighPrice", bean.HighPrice));

            columns.Add("HuanShou");
            values.Add("@HuanShou");
            param.Add(new SqlParameter("@HuanShou", bean.HuanShou));


            columns.Add("Amount");
            values.Add("@Amount");
            param.Add(new SqlParameter("@Amount", bean.Amount));

            columns.Add("Volume");
            values.Add("@Volume");
            param.Add(new SqlParameter("@Volume", bean.Volume));



            columns.Add("LowPrice");
            values.Add("@LowPrice");
            param.Add(new SqlParameter("@LowPrice", bean.LowPrice));

            columns.Add("OpenPrice");
            values.Add("@OpenPrice");
            param.Add(new SqlParameter("@OpenPrice", bean.OpenPrice));

            if (bean.Rq.Year < 1900)
            {
                bean.Rq = DateTime.Parse("1900-01-01");
            }
            columns.Add("rq");
            values.Add("@Rq");
            param.Add(new SqlParameter("@Rq", bean.Rq));

            columns.Add("ZhangFu");
            values.Add("@ZhangFu");
            param.Add(new SqlParameter("@ZhangFu", bean.ZhangFu));

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("insert into stockprice ( " + string.Join(",", columns) + " ) values ( " + string.Join(",", values) + " ) ; ");

            try
            {
                return SlDatabase.ExecuteNonQuery(databaseConnectionString, sbSql.ToString(), param.ToArray());
            }
            catch(Exception ex)
            {
                int xjs = 123;
            }
            return 0;
        }
    }
}
