using System;
using System.Data;
using System.IO;
using System.Net;
using XjsStock;
using XjsStock.Service;

namespace StockSeeker
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var stockTable = StockService.GetStockTable();
            //更新股票名称
            //StockInterface.UpDateStockList(stockTable);
            //更新上市日期
            //StockInterface.UpdateShangShiRq(stockTable);
            //更新实时股票价格
            StockInterface.UpdateByTecent(stockTable);
            foreach (DataRow dataRow in stockTable.Rows)
            {
                Console.WriteLine("开始更新" + dataRow["name"]);
                StockInterface.UpdateStockPriceBySohu(dataRow["id"].ToString(), dataRow["createDay"].ToString());
            }
        }
    }
}