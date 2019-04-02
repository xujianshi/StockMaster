using Chloe;
using DataAccess;
using StockSeeker;
using System;
using System.Collections.Generic;
using System.Text;
using XjsStock.Bean;

namespace XjsStock.Dao
{
    public class StockPriceDao : BaseDao<StockPriceBean>
    {
        public override string Columns
        {
            get
            {
                return @"
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

        public long Add(StockPriceBean bean)
        {
            var columns = new List<string>();
            var values = new List<string>();
            var param = new List<DbParam>();

            columns.Add("ClosePrice");
            values.Add("@ClosePrice");
            param.Add(new DbParam("@ClosePrice", bean.ClosePrice));

            columns.Add("Code");
            values.Add("@Code");
            param.Add(new DbParam("@Code", bean.Code));

            columns.Add("HighPrice");
            values.Add("@HighPrice");
            param.Add(new DbParam("@HighPrice", bean.HighPrice));

            columns.Add("HuanShou");
            values.Add("@HuanShou");
            param.Add(new DbParam("@HuanShou", bean.HuanShou));

            columns.Add("Amount");
            values.Add("@Amount");
            param.Add(new DbParam("@Amount", bean.Amount));

            columns.Add("Volume");
            values.Add("@Volume");
            param.Add(new DbParam("@Volume", bean.Volume));

            columns.Add("LowPrice");
            values.Add("@LowPrice");
            param.Add(new DbParam("@LowPrice", bean.LowPrice));

            columns.Add("OpenPrice");
            values.Add("@OpenPrice");
            param.Add(new DbParam("@OpenPrice", bean.OpenPrice));

            if (bean.Rq.Year < 1900)
            {
                bean.Rq = DateTime.Parse("1900-01-01");
            }
            columns.Add("Rq");
            values.Add("@Rq");
            param.Add(new DbParam("@Rq", bean.Rq));

            columns.Add("ZhangFu");
            values.Add("@ZhangFu");
            param.Add(new DbParam("@ZhangFu", bean.ZhangFu));

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("insert into StockPrice ( " + string.Join(",", columns) + " ) values ( " + string.Join(",", values) + " ) ; ");

            try
            {
                return ContextHelper.ExcuteSql(sbSql.ToString(), param.ToArray());
            }
            catch { }
            return 0;
        }
    }
}