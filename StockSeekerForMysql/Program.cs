using System;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using XjsStock;
using XjsStock.Service;

namespace StockSeeker
{
    class Program
    {
        static void Main(string[] args)
        {
            var stockTable = StockService.GetStockTable();
            //更新股票名称
            //StockInterface.UpDateStockList(stockTable);
            //更新上市日期
            //StockInterface.UpdateShangShiRq(stockTable);
            //更新实时股票价格
            //StockInterface.UpdateByTecent(stockTable);

            updateStockUDPPS(stockTable);//更新财务资料

            //updateDayDate(stockTable);
        }

        /// <summary>
        /// 更新季报财务数据
        /// </summary>
        /// <param name="table"></param>
        static void updateStockUDPPS(DataTable table)
        {
            Parallel.ForEach(table.AsEnumerable(), new ParallelOptions() { MaxDegreeOfParallelism = 20 }, dataRow =>
            {
                StockInterface.updateStockUDPPS(dataRow["id"].ToString());
            });
        }

        /// <summary>
        /// 更新日数据
        /// </summary>
        /// <param name="stockTable"></param>
        static void updateDayDate(DataTable stockTable)
        {
            Parallel.ForEach(stockTable.AsEnumerable(), new ParallelOptions() { MaxDegreeOfParallelism = 20 }, dataRow =>
            {
                Console.WriteLine("开始更新-->" + dataRow["id"] + dataRow["name"]);
                //StockInterface.UpdateStockPriceBy163(dataRow["id"].ToString(), 2000);
                StockInterface.UpdateStockPriceBySohu(dataRow["id"].ToString(), dataRow["createday"].ToString());
            });
        }

        /// <summary>
        /// 合并文件
        /// </summary>
        static void MergeFiles()
        {
            DirectoryInfo dir = new DirectoryInfo(@"E:\DataBase\HistoryData");
            var files = dir.GetFiles("*.csv");
            foreach (FileInfo info in files)
            {
                var lines = XFileCtr.getContentList(info.FullName);
                StringBuilder sb = new StringBuilder();
                foreach (string line in lines)
                {
                    if (!line.StartsWith("日期"))
                    {
                        sb.AppendLine(line.Replace("'", ""));
                    }
                }
                XFileCtr.OutFile(@"E:\DataBase\HistoryData.csv", sb.ToString());
                Console.WriteLine(info.Name);
            }
        }
    }
}
