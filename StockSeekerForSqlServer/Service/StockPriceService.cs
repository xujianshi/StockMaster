using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XjsStock.Bean;
using XjsStock.Dao;

namespace XjsStock.Service
{
    public class StockPriceService
    {
        public  static  StockPriceDao mDao=new StockPriceDao();

        public static DataTable GetStockPriceTable(string code)
        {
            string sql = $"SELECT   * FROM stockprice where code ='{code}' order by rq desc ";
            return DbHelper.Query(ConfigHelper.Db, sql);
        }

        public static List<StockPriceBean> GetStockPriceBeasBeans(string code)
        {
            var list = new List<StockPriceBean>();
            var table = GetStockPriceTable(code);
            foreach (DataRow row in table.Rows)
            {
                StockPriceBean bean=new StockPriceBean();
                mDao.SetBean(row,bean);
                list.Add(bean);
            }
            return list;
        }


    }
}
