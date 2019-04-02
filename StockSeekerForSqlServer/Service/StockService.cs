using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SoufunLab.Framework.Data;
using XjsStock.Dao;
using XjsStock.Entity;

namespace XjsStock.Service
{
    public  class StockService
    {
        public static  StockDao mDao=new StockDao();

        public static List<StockEntity> GetList(DataTable table)
        {
            var list=new List<StockEntity>();
            foreach (DataRow row in table.Rows)
            {
                StockEntity entity=new StockEntity();
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
            var table = DbHelper.Query(ConfigHelper.Db, "SELECT *  FROM [stock] order by LastUpdateTime");
            return table;
        }

    }
}
