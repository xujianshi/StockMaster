using System.Collections.Generic;
using System.Data;
using DataAccess;
using XjsStock.Bean;
using XjsStock.Dao;

namespace XjsStock.Service
{
    public  class StockService
    {
        public static  StockDao mDao=new StockDao();

        public static List<StockBean> GetList(DataTable table)
        {
            var list=new List<StockBean>();
            foreach (DataRow row in table.Rows)
            {
                StockBean entity =new StockBean();
                mDao.SetBean(row, entity);
                list.Add(entity);
            }
            return list;
        }

        public static List<StockBean> GetStockList()
        {
           var table = GetStockTable();
            return GetList(table);
        }

        public static DataTable GetStockTable()
        {
            var table = ContextHelper.GetTable(mDao.SelectAll+ " order by id");
            return table;
        }

    }
}
