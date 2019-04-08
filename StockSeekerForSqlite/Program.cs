using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using DataAccess;
using XjsStock;
using XjsStock.Service;

namespace StockSeeker
{
    internal class Program
    {

        private static DataTable LastDayTable = null;
        private static void Main(string[] args)
        {
            StockInterface.UpDateStockList();//更新股票名称
            var stockTable = StockService.GetStockTable();
            StockInterface.UpdateByTecent(stockTable);//更新实时股票价格
            foreach (DataRow dataRow in stockTable.Rows)
            {
                Console.WriteLine("开始更新" + dataRow["name"]);
                StockInterface.UpdateStockPriceBySohu(dataRow["id"].ToString(), dataRow["createDay"].ToString());
            }
            LastDayTable = ContextHelper.GetTable($"select (select max(rq)as lastRq from stockprice where code=stock.id)as lastrq, * from stock ");
            DownLoad();//下载excel股票
        }

        private static void DownLoad()
        {
            var stockTable = StockService.GetStockTable();
            ClearFolder();
            foreach (DataRow row in stockTable.Rows)
            {
                 string code = row["id"].ToString();
                try
                {
                    int flag = code.StartsWith("60") ? 0 : 1;
                    DateTime createDay = string.IsNullOrEmpty(row["createday"].ToString())
                        ? DateTime.Parse("2019-01-01")
                        : DateTime.Parse(row["createday"].ToString());
                    DateTime minDate = DateTime.Parse(ConfigurationManager.AppSettings["mindate"]);
                    if (createDay < minDate)
                    {
                        createDay = minDate;
                    }
                    string begin = createDay.ToString("yyyyMMdd");
                    string end = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
                    string url = $"http://quotes.money.163.com/service/chddata.html?code={flag}{code}&start={begin}&end={end}&fields=TCLOSE;HIGH;LOW;TOPEN;LCLOSE;CHG;PCHG;TURNOVER;VOTURNOVER;VATURNOVER;TCAP;MCAP";
                    string fileName = "HistoryData\\" + code + ".csv";
                    if (!File.Exists(fileName))
                    {
                        DownLoad(code,url, fileName);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message+"-->"+code);
                }
            }
        }

        private static void ClearFolder()
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo("HistoryData");
                if (!dir.Exists)
                {
                    dir.Create();
                }
                var files = dir.GetFiles("*.csv");
                foreach (var fileInfo in files)
                {
                    fileInfo.Delete();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void DownLoad(string code,string url, string fileName)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(url, fileName);
                    DeleteName(code,fileName);
                    Console.WriteLine(fileName + "下载成功");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(fileName + "下载失败");
            }
        }

        /// <summary>
        /// 合并文件
        /// </summary>
        static void DeleteName(string code,string filename)
        {
            try
            {
                FileInfo info=new FileInfo(filename);
                var lines = XFileCtr.getContentList(info.FullName);
                var row = LastDayTable.AsEnumerable().FirstOrDefault(o => o["id"].ToString() == code);
                if (!string.IsNullOrEmpty(row["lastrq"].ToString()))
                {
                    var rq = DateTime.Parse(row["lastrq"].ToString()).ToString("yyyy-MM-dd");
                    var count = lines.Count(s => s.Contains(rq));
                    if (count == 0)
                    {
                        //日期
                        //股票代码
                        //名称
                        var name = row["name"].ToString();
                        //收盘价
                        var closePrice = row["closeprice"].ToString();
                        //最高价
                        var highPrice = row["maxprice"].ToString();
                        //最低价
                        var lowPrice = row["minprice"].ToString();
                        //开盘价
                        var openPrice = row["openprice"].ToString();
                        //前收盘
                        var prePrice = row["preclose"].ToString();
                        //涨跌额
                        var zhangdieMoney = row["zhangdiemoney"].ToString();
                        //涨跌幅
                        var zhangfu = row["zhangfu"].ToString();
                        //换手率
                        var huanshoulv = row["huanshoulv"].ToString();
                        //成交量
                        var chengjiaoliang = row["volume"].ToString();
                        //成交金额
                        var amount = row["amount"].ToString();
                        //总市值
                        var zongshizhi = row["zongshizhi"].ToString();
                        //流通市值
                        var liutongshizhi = row["liutongshizhi"].ToString();
                        var newLine =
                            $"{rq},{code},{name},{closePrice},{highPrice},{lowPrice},{openPrice},{prePrice},{zhangdieMoney},{zhangfu},{huanshoulv},{chengjiaoliang},{amount},{zongshizhi},{liutongshizhi}";
                        lines.Insert(1, newLine);
                    }
                }
                StringBuilder sb = new StringBuilder();
                foreach (string line in lines)
                {
                    var list = new List<string>(line.Replace("'", "").Split(','));
                    list.RemoveAt(2);
                    var newLine = string.Join(",", list);
                    sb.AppendLine(newLine);
                }
                info.Delete();
                XFileCtr.OutFile(filename, sb.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


    }
}