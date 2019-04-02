using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DataAccess;
using XjsStock.Bean;
using XjsStock.Dao;

namespace XjsStock.Service
{
    public class StockPriceService
    {
        public static DataTable GetStockPriceTable(string code,int year)
        {
            string sql =$"select rq  from stockprice where rq>='{year}-01-01' and code='{code}' order by rq desc ";
            return ContextHelper.GetTable(sql);
        }
    }
}
