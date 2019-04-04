using DataAccess;
using System;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace V5Cmd
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //CreateStock();
            StockReport201806();
            Console.WriteLine("完事了");
        }

        private static void StockReport201806()
        {
            StringBuilder sb=new StringBuilder();
            StreamReader sr = File.OpenText(@"2018中报.txt");
            var date = "201806";
            string nextLine = sr.ReadLine();
            while (nextLine != null)
            {
                sb.AppendLine(nextLine);
                nextLine = sr.ReadLine();
            }
            sr.Close();
            JArray json=JArray.Parse(sb.ToString());
            foreach (var job in json)
            {
                try
                {
                    string code = job["scode"].ToString();
                    string totaloperatereve = job["totaloperatereve"].ToString();
                    var parentnetprofit = job["parentnetprofit"].ToString();
                    var basiceps = job["basiceps"].ToString();
                    if (basiceps=="-")
                    {
                        basiceps = "0";
                    }
                    var xsmll = job["xsmll"].ToString();
                    var sql= $@"INSERT INTO `stock`.`stockreport`
                                    (`date`, `code`, `totaloperatereve`, `parentnetprofit`, `basiceps`, `xsmll`) 
                                    VALUES ('{date}', '{code}', {totaloperatereve}, {parentnetprofit}, {basiceps}, '{xsmll}');       ";
                  var rst=  ContextHelper.ExcuteSql(sql,null);
                  if (rst>0)
                  {
                      Console.WriteLine(code);
                  }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
       
        private static void CreateStock()
        {
            StreamReader sr = File.OpenText(@"2014-2018汇总.txt");
            string nextLine = sr.ReadLine();
            while (nextLine != null)
            {
                //0成交日期,1成交时间,2股东代码,3证券代码,4证券名称,5委托类别,6成交价格,7成交数量,8成交金额,9发生金额,10佣金,11印花税,12过户费,13其他费,14成交编号,
                try
                {
                    string[] words = nextLine.Split(',');
                    DateTime time = DateTime.Parse(getDate(words[0]) + " " + words[1]);
                    string code = words[3];
                    string name = words[4];
                    int tradetype = words[5] == "买入" ? -1 : 1;
                    double price = double.Parse(words[6]);
                    double nums = double.Parse(words[7]);
                    double money = double.Parse(words[8]);
                    double realMoney = double.Parse(words[9]);
                    double otherMoney = double.Parse(words[10]) + double.Parse(words[11]) + double.Parse(words[12]) + double.Parse(words[13]);
                    if (otherMoney < 1)
                    {
                        nextLine = sr.ReadLine();
                        continue;
                    }
                    long id = long.Parse(words[14]);
                    string sql =
                        @"INSERT INTO `stocktraderecord`(`Id`, `TradeTime`, `code`, `name`, `tradetype`, `price`, `num`, `chengjiaoMoney`, `totalmoney`, `OtherMoney`)
                        VALUES ('{0}', '{1}', '{2}', '{3}', {4}, {5}, {6}, {7}, {8}, {9});";
                    sql = string.Format(sql, id, time.ToString("yyyy-MM-dd HH:mm:ss"), code, name, tradetype, price, nums, money, realMoney, otherMoney);
                    ContextHelper.ExcuteSql(sql, null);
                }
                catch (Exception e)
                {
                    string msg = e.ToString();
                    if (!msg.Contains("Duplicate entry"))
                    {
                        Console.WriteLine(nextLine);
                    }
                }
                nextLine = sr.ReadLine();
            }
            sr.Close();
        }

        private static string getDate(string line)
        {
            string result = line.Substring(0, 4) + "-" + line.Substring(4, 2) + "-" + line.Substring(6, 2);
            return result;
        }
    }
}