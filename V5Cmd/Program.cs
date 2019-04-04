using DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace V5Cmd
{
    internal class Program
    {
       static readonly Dictionary<string, int> Dic = new Dictionary<string, int>();
        private static void Main(string[] args)
        {
            Dic.Add("&#xEEC5;", 0);
            Dic.Add("&#xE793;", 1);
            Dic.Add("&#xECE9;", 2);
            Dic.Add("&#xEA5D;", 3);
            Dic.Add("&#xF78F;", 4);
            Dic.Add("&#xE4E5;", 5);
            Dic.Add("&#xE73F;", 6);
            Dic.Add("&#xE712;", 7);
            Dic.Add("&#xE268;", 8);
            Dic.Add("&#xF2F8;", 9);
            //CreateStock();
            StockReport("201809", @"2018年3季报.txt");
            //StockReport("201806", @"2018中报.txt");
            Console.WriteLine("完事了");
        }

        /***
** 功能：  字符串格式化替换操作
** Author: Allen Zhang
** RTX：   14002
***/
        //String.prototype.format = function()
        //{
        //    var args = arguments;
        //    return this.replace(/\{ (\d +)\}/ g,
        //    function(m, i) {
        //        return args[i];
        //    });
        //}


        private static void StockReport(string date,string filename)
        {
            StringBuilder sb=new StringBuilder();
            StreamReader sr = File.OpenText(filename);
            string nextLine = sr.ReadLine();
            while (nextLine != null)
            {
                sb.AppendLine(nextLine);
                nextLine = sr.ReadLine();
            }
            sr.Close();
            var jsonString = sb.ToString();
            foreach (var item in Dic)
            {
                jsonString = jsonString.Replace(item.Key, item.Value.ToString());
            }
            JArray json=JArray.Parse(jsonString);
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