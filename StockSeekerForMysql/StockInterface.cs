using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using DataAccess;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StockSeeker;
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
                                    new StockDao().Add(stockBean);
                                    Console.WriteLine("增加收录股票"+_code + _name);
                                }
                                else //更新实时信息
                                {
                                    new StockDao().Update(stockBean);
                                    Console.WriteLine("更新股票名称"+_code + _name);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex.Message);
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
                    string sql= "update stock set lastupdatetime='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"'";
                    sql += $" ,name='{strs[1]}'";
                    sql += ", shiyinglv=" + SlConvert.TryToDecimal(strs[39]); //市盈率;
                    sql += " ,shijinglv=" + SlConvert.TryToDecimal(strs[46]); //市净率
                    sql += " ,OpenPrice=" + strs[5];
                    sql += " ,ClosePrice=" + strs[3];
                    sql += " ,MaxPrice=" + strs[41];
                    sql += " ,MinPrice=" + strs[42];
                    sql += " ,LimitUp=" + strs[47];
                    sql += " ,LimitDown=" + strs[48];
                    sql += " ,ZhenFu=" + strs[43];
                    sql += " ,ZhangFu=" + strs[32];
                    sql += " ,ZhangDieMoney=" + strs[31];
                    sql += " ,Volume=" + strs[36];
                    sql += " ,Amount=" + strs[37];
                    sql += " ,HuanShoulv=" + strs[38];
                    sql += " ,PreClose=" + strs[4];
                    sql += " ,LiuTongShiZhi=" + strs[44];
                    sql += " ,ZongShiZhi=" + strs[45];
                    sql += string.Format("  where id='{0}'", code);
                    ContextHelper.ExcuteSql(sql,null);
                    Console.WriteLine(sql);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
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
                        ContextHelper.ExcuteSql(sql,null);
                        Console.WriteLine(sql);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
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
            else if (DateTime.Parse(createDay).Year < 2019)
            {
                createDay = "20190101";
            }
            else
            {
                createDay= DateTime.Parse(createDay).ToString("yyyyMMdd");
            }
            DataTable priceTable = StockPriceService.GetStockPriceTable(code,2019);
            string endDay = DateTime.Now.Hour >= 15? DateTime.Now.ToString("yyyyMMdd"): DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            string url = "http://q.stock.sohu.com/hisHq?code=cn_"+code+"&start="+ createDay + "&end="+ endDay + "&stat=1&order=D";
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
                    bean.ZhenFu = (bean.HighPrice - bean.LowPrice) * 2.0m / (bean.HighPrice + bean.LowPrice);
                    new StockPriceDao().Add(bean);
                    Console.WriteLine(string.Format("{0}--{1}--{2}", bean.Rq.ToString("yy年MM月dd日"), bean.Code, bean.ClosePrice));
                }
                int xjs = 123;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void UpdateStockPriceBy163(string code, int year )
        {
            try
            {
                string fileName = @"E:\DataBase\HistoryData\" + code + ".csv";
                string fileName2= @"E:\DataBase\HistoryData2\" + code + ".csv";
                if (!File.Exists(fileName)) return;
                var lines = XFileCtr.getContentList(fileName);
                if (lines.Count<1)return;
                var beans = new List<StockPriceBean>();
                DataTable priceTable = StockPriceService.GetStockPriceTable(code, year);
                foreach (string ttt in lines)
                {
                    try
                    {

                        #region 单支股票当天

                        if (ttt.Contains("日期") || ttt.Contains("None"))
                        {
                            continue;
                        }
                        // 0日期,1股票代码,2名称,3收盘价,4最高价,5最低价,6开盘价,7前收盘,8涨跌额,9涨跌幅,10换手率,11成交量,12成交金额,13总市值,14流通市值
                        var jb = ttt.Split(',');
                        StockPriceBean bean = new StockPriceBean();
                        bean.Code = code;
                        bean.Rq = DateTime.Parse(jb[0].ToString());
                        if (bean.Rq < DateTime.Parse(year + "-01-01"))
                        {
                            continue;
                        }
                        var row = priceTable.Select(string.Format("rq='{0}'", bean.Rq.ToString("yyyy-MM-dd")))
                            .FirstOrDefault();
                        if (null != row)
                        {
                            continue;
                        }
                        bean.ClosePrice = SlConvert.TryToDecimal(jb[3].ToString());
                        bean.OpenPrice = SlConvert.TryToDecimal(jb[6].ToString());
                        bean.HighPrice = SlConvert.TryToDecimal(jb[4].ToString());
                        bean.LowPrice = SlConvert.TryToDecimal(jb[5].ToString());
                        bean.ZhangFu = SlConvert.TryToDecimal(jb[9].ToString().Replace("%", ""));
                        bean.Volume = SlConvert.TryToDecimal(jb[11].ToString().Replace("%", ""));
                        bean.Amount = SlConvert.TryToDecimal(jb[12].ToString().Replace("%", ""));
                        bean.HuanShou = SlConvert.TryToDecimal(jb[10].ToString().Replace("%", ""));
                        bean.ZhenFu = (bean.HighPrice - bean.LowPrice) * 2.0m / (bean.HighPrice + bean.LowPrice);
                        beans.Add(bean);
                        Console.WriteLine($"{bean.Rq.ToString("yyyyMMdd")}--{bean.Code}-{jb[2]}-{bean.ClosePrice}");

                        #endregion
                    }
                    catch (Exception e)
                    {
                    }
                }
                new StockPriceDao().Add(beans);
                new FileInfo(fileName).MoveTo(fileName2);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 更新股票分红
        /// </summary>
        public static void StockMoney()
        {
            var url = "http://www.sse.com.cn/market/stockdata/dividends/dividend/";
        }
    }
}
