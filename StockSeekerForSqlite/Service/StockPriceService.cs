using DataAccess;
using System.Collections.Generic;
using System.Data;
using XjsStock.Bean;
using XjsStock.Dao;

namespace XjsStock.Service
{
    public class StockPriceService
    {
        public static StockPriceDao mDao = new StockPriceDao();

        public static DataTable GetStockPriceTable(string code)
        {
            string sql = mDao.SelectAll + string.Format(" where code ='{0}' order by rq desc ", code);
            return ContextHelper.GetTable(sql);
        }

        public static List<StockPriceBean> GetStockPriceBeasBeans(string code)
        {
            var list = new List<StockPriceBean>();
            var table = GetStockPriceTable(code);
            foreach (DataRow row in table.Rows)
            {
                StockPriceBean bean = new StockPriceBean();
                mDao.SetBean(row, bean);
                list.Add(bean);
            }
            return list;
        }
    }
}