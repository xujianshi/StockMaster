using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace StockDownLoad
{
    class Program
    {
        static void Main(string[] args)
        {
            ClearFolder();
            var stockTable = ContextHelper.GetTable("select * from stock order by id");
            DownLoad(stockTable);
        }

        private static void DownLoad(DataTable stockTable)
        {
            foreach (DataRow row in stockTable.Rows)
            {
                string code = row["id"].ToString();
                int flag = code.StartsWith("60") ? 0 : 1;
                DateTime createDay = DateTime.Parse(row["createday"].ToString());
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
                    DownLoad(url, fileName);
                }
            }
        }

        private static void ClearFolder()
        {
            try
            {
                DirectoryInfo dir=new DirectoryInfo("HistoryData");
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

        private static void DownLoad(string url, string fileName)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(url, fileName);
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
        static void DeleteName(string filename)
        {
            try
            {
                FileInfo info = new FileInfo(filename);
                var lines = XFileCtr.getContentList(info.FullName);
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
