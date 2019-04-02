using DataAccess;
using System.Collections.Generic;
using System.Data;
using XjsStock.Dao;
using XjsStock.Entity;

namespace XjsStock.Service
{
    public class StockService
    {
        public static StockDao mDao = new StockDao();

        public static List<StockEntity> GetList(DataTable table)
        {
            var list = new List<StockEntity>();
            foreach (DataRow row in table.Rows)
            {
                StockEntity entity = new StockEntity();
                mDao.SetBean(row, entity);
                list.Add(entity);
            }
            return list;
        }

        public static List<StockEntity> GetStockList()
        {
            var table = GetStockTable();
            return GetList(table);
        }

        public static DataTable GetStockTable()
        {
            var table = ContextHelper.GetTable(mDao.SelectAll + " order by id");
            return table;
        }
    }
}