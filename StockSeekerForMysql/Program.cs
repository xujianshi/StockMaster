using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using XjsStock;
using XjsStock.Service;

namespace StockSeeker
{
    class Program
    {
        static void Main(string[] args)
        {
            StockInterface.UpDateStockList();//更新股票名称
            StockInterface.UpdateShangShiRq();//更新上市日期
            StockInterface.UpdateByTecent(); //更新实时股票价格
            //updateStockUDPPS(stockTable);//更新财务资料
            updateDayDate();//更新日数据
        }

        /// <summary>
        /// 更新季报财务数据
        /// </summary>
        /// <param name="table"></param>
        static void updateStockUDPPS(DataTable table)
        {
            var exsist_table = ContextHelper.GetTable("select distinct code from stockfinanceoverview");
            var codeList=new List<string>();
            foreach (DataRow row in exsist_table.Rows)
            {
                codeList.Add(row["code"].ToString());
            }
            Parallel.ForEach(table.AsEnumerable(), new ParallelOptions() { MaxDegreeOfParallelism = 1 }, dataRow =>
            {
                var code = dataRow["id"].ToString();
                if (!codeList.Contains(code))
                {
                    StockInterface.updateStockUDPPS(dataRow["id"].ToString());
                    Console.WriteLine(dataRow["id"].ToString());
                }
            });
        }

        /// <summary>
        /// 更新日数据
        /// </summary>
        /// <param name="stockTable"></param>
        static void updateDayDate()
        {
            var stockTable = StockService.GetStockTable();
            Parallel.ForEach(stockTable.AsEnumerable(), new ParallelOptions() { MaxDegreeOfParallelism = 20 }, dataRow =>
            {
                Console.WriteLine("开始更新-->" + dataRow["id"] + dataRow["name"]);
                //StockInterface.UpdateStockPriceBy163(dataRow["id"].ToString(), 2000);
                StockInterface.UpdateStockPriceBySohu(dataRow["id"].ToString(), dataRow["createday"].ToString());
            });
        }
    }
}
