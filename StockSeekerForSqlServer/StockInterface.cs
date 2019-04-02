using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoufunLab.Framework;
using SoufunLab.Framework.Data;
using XjsStock.Bean;
using XjsStock.Dao;
using XjsStock.Service;

namespace XjsStock
{
    public class StockInterface
    {

        /// <summary>
        /// 调取同花顺接口获得所有股票列表
        /// </summary>
        /// <param name="vStockTable"></param>
        public static void UpDateStockList(DataTable vStockTable)
        {
            for (int market = 0; market < 3; market++)
            {

                for (int page = 1; page < 5; page++)
                {
                    string href = "http://quote.tool.hexun.com/hqzx/quote.aspx?type=2&market=" + market + "&sorttype=3&updown=up&page=" + page + "&count=1000&time=150000";
                    string html = new WebApi().GetHtml(href);
                    html = WebApi.GetRegexValue(html, "dataArr =", ";StockListPage");
                    try
                    {
                        JArray xxs = (JArray)JsonConvert.DeserializeObject(html);
                        for (int i = 0; i < xxs.Count; i++)
                        {
                            JArray jb = (JArray)xxs[i];
                            string _code = jb[0].ToString();
                            string _name = jb[1].ToString();
                            try
                            {
                                var stockBean = new StockBean
                                {
                                    ID = _code,
                                    Name = _name
                                };
                                var theone = vStockTable.Select(string.Format("id='{0}'", _code)).FirstOrDefault();
                                if (null == theone)
                                {
                                    new StockDao().Add(ConfigHelper.Db, stockBean);
                                    Console.WriteLine("增加收录股票"+_code + _name);
                                }
                                else //更新实时信息
                                {
                                    new StockDao().Update(ConfigHelper.Db, stockBean);
                                    Console.WriteLine("更新股票名称"+_code + _name);
                                }
                            }
                            catch (Exception ex)
                            {
                                int xjs = 123;
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                    }
                }                
            }
        }


        public static void UpdateByTecent(DataTable vStockTable)
        {
            foreach (DataRow row in vStockTable.Rows)
            {
                try
                {
                    string code = row["id"].ToString();
                    string lable = code.StartsWith("60") ? "sh" : "sz";
                    string url = "http://qt.gtimg.cn/q=" + lable + code;
                    string html = new WebApi().GetHtml(url);
                    string jsonStr = WebApi.GetRegexValue(html, "v_" + lable + code + "=\"", "\";");
                    string[] strs = jsonStr.Split('~');
                    string sql= "update stock set LastUpdateTime=getdate()";
                    sql += $" ,[name]='{strs[1]}'";
                    sql += ", ShiYingLV=" + SlConvert.TryToDecimal(strs[39]); //市盈率;
                    sql += " ,ShiJingLV=" + SlConvert.TryToDecimal(strs[46]); //市净率
                    sql += " ,[OpenPrice]=" + strs[5];
                    sql += " ,[ClosePrice]=" + strs[3];
                    sql += " ,[MaxPrice]=" + strs[41];
                    sql += " ,[MinPrice]=" + strs[42];
                    sql += " ,[LimitUp]=" + strs[47];
                    sql += " ,[LimitDown]=" + strs[48];
                    sql += " ,[ZhenFu]=" + strs[43];
                    sql += " ,[ZhangFu]=" + strs[32];
                    sql += " ,[ZhangDieMoney]=" + strs[31];
                    sql += " ,[Volume]=" + strs[36];
                    sql += " ,[Amount]=" + strs[37];
                    sql += " ,[HuanShoulv]=" + strs[38];
                    sql += " ,[PreClose]=" + strs[4];
                    sql += " ,[LiuTongShiZhi]=" + strs[44];
                    sql += " ,[ZongShiZhi]=" + strs[45];
                    sql += string.Format("  where id='{0}'", code);

                    SlDatabase.ExecuteNonQuery(ConfigHelper.Db, sql);
                    Console.WriteLine(sql);
                }
                catch (Exception ex)
                {
                    int xjs = 123;
                }
            }
            Console.WriteLine("更新完毕");
        }

        /// <summary>
        /// 获取上市日期
        /// </summary>
        /// <param name="vStockTable"></param>
        public static void UpdateShangShiRq(DataTable vStockTable)
        {
            foreach (DataRow row in vStockTable.Rows)
            {
                string sql = string.Empty;
                try
                {
                    string code = row["id"].ToString();
                    if (!string.IsNullOrEmpty(row["CreateDay"].ToString())
                        &&DateTime.Parse(row["CreateDay"].ToString()).Year> 1980)
                    {
                        continue;
                    }
                    string url = "http://stockpage.10jqka.com.cn/" +  code;
                    string html = new WebApi().GetHtml(url);
                    if (string.IsNullOrEmpty(html))
                    {
                        break;
                    }
                    HtmlDocument doc=new HtmlDocument();
                    doc.LoadHtml(html);
                    HtmlNodeCollection hrefList = doc.DocumentNode.SelectNodes("//dl[@class='company_details']");
                    if (hrefList != null && hrefList.Count > 0)
                    {
                        HtmlNode node = hrefList[0];
                        var text = WebApi.GetRegexValue(node.InnerText, "上市日期：", "每股净资产");
                        text = text.Replace("\t", "").Replace("\r\n","");
                        sql = $"update stock set CreateDay='{text}'  where id='{code}'";
                        SlDatabase.ExecuteNonQuery(ConfigHelper.Db, sql);
                        Console.WriteLine(sql);
                    }
                }
                catch (Exception ex)
                {
                    int xjs = 123;
                }
            }
        }


        /// <summary>
        /// http://q.stock.sohu.com/cn/300444/lshq.shtml
        /// 调用搜狐接口更新实时股价
        /// http://q.stock.sohu.com/hisHq?code=cn_300444&start=20160123&end=20161222&stat=1&order=D
        /// </summary>
        public static void UpdateStockPriceBySohu(string code,string createDay)
        {
            if (string.IsNullOrEmpty(createDay))
            {
                createDay = DateTime.Now.AddYears(-1).ToString("yyyyMMdd");
            }
            else if (DateTime.Parse(createDay).Year < 2018)
            {
                createDay = "20180101";
            }
            else
            {
                createDay= DateTime.Parse(createDay).ToString("yyyyMMdd");
            }

            DataTable priceTable = StockPriceService.GetStockPriceTable(code);
            var tt = DateTime.Now.Hour >= 15 ? 0 : -1;
            var yestodayrow = priceTable.Select(string.Format("rq='{0}'", DateTime.Now.AddDays(tt).ToString("yyyy-MM-dd"))).FirstOrDefault();
            if (null != yestodayrow)
            {
                return;
            }
            string url = "http://q.stock.sohu.com/hisHq?code=cn_"+code+"&start="+ createDay + "&end="+ DateTime.Now.ToString("yyyyMMdd") + "&stat=1&order=D";
            string html = new WebApi().GetHtml(url);
            try
            {
                var job = (JArray)JsonConvert.DeserializeObject(html);
                JArray xxs = (JArray)job[0]["hq"];
                foreach (JToken ttt in xxs)
                {
                    // 0日期	1开盘	2收盘	3涨跌额	4涨跌幅	5最低	6最高	7成交量(手)	8成交金额(万)	9换手率
                    JArray jb = (JArray)ttt;
                    StockPriceBean bean = new StockPriceBean();
                    bean.Code = code;
                    bean.Rq = DateTime.Parse(jb[0].ToString());
                    if (bean.Rq.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd")&& DateTime.Now.Hour < 15)
                    {
                        continue;
                    }
                    var row =
                        priceTable.Select(string.Format("rq='{0}'", bean.Rq.ToString("yyyy-MM-dd"))).FirstOrDefault();
                    if (null != row)
                    {
                        continue;
                    }
                    bean.ClosePrice = SlConvert.TryToDecimal(jb[2].ToString());
                    bean.OpenPrice = SlConvert.TryToDecimal(jb[1].ToString());
                    bean.HighPrice = SlConvert.TryToDecimal(jb[6].ToString());
                    bean.LowPrice = SlConvert.TryToDecimal(jb[5].ToString());
                    bean.ZhangFu = SlConvert.TryToDecimal(jb[4].ToString().Replace("%",""));
                    bean.Volume = SlConvert.TryToDecimal(jb[7].ToString().Replace("%", ""));
                    bean.Amount = SlConvert.TryToDecimal(jb[8].ToString().Replace("%", ""));
                    bean.HuanShou = SlConvert.TryToDecimal(jb[9].ToString().Replace("%", ""));
                    new StockPriceDao().Add(ConfigHelper.Db, bean);
                    Console.WriteLine(string.Format("{0}--{1}--{2}", bean.Rq.ToString("yy年MM月dd日"), bean.Code, bean.ClosePrice));
                }
                int xjs = 123;
            }
            catch(Exception ex)
            {
                int xjsd = 123;
            }
        }

    }
}
