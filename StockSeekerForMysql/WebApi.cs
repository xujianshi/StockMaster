
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace XjsStock
{
    public class WebApi
    {
        /// <summary>
        /// 用来保存cookie的容器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private CookieContainer cookieContainer = new CookieContainer();


        public string GetHtml(string href,string characterSet="")
        {
            try
            {  
                //创建URL，并创建请求
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(href);
                request.Accept = "text/html";
                request.Headers.Add("Accept-Language", "zh-cn");
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                request.KeepAlive = true;
                //携带cookie
                request.CookieContainer = cookieContainer;
                //发送请求，并获取HTML
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                //处理ISO-8859-1字符集：直接设置为UTF-8
                if (string.IsNullOrEmpty(characterSet))
                {
                    characterSet = response.CharacterSet;
                    if (characterSet == "ISO-8859-1")
                    {
                        characterSet = "UTF-8";
                    }
                }
                if (string.IsNullOrEmpty(characterSet))
                {
                    characterSet = "gb2312";
                }
                //读取流
                StreamReader streamreader = new StreamReader(stream, Encoding.GetEncoding(characterSet));
                string html = streamreader.ReadToEnd();
                streamreader.Close();
                response.Close();
                return html;
            }
            catch (System.Exception ex)
            {
            }
            return "";
        }

        /// <summary>
        /// 获得字符串中开始和结束字符串中间得值
        /// </summary>
        /// <param name="vFullString">字符串</param>
        /// <param name="vStart">开始</param>
        /// <param name="vEnd">结束</param>
        /// <returns></returns>
        public static string GetRegexValue(string vFullString, string vStart, string vEnd)
        {
            Regex rg = new Regex("(?<=(" + vStart + "))[.\\s\\S]*?(?=(" + vEnd + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            Match mc = rg.Match(vFullString);
            if (mc!=null)
            {
                return mc.Value;
            }
            else
            {
                return "";
            }            
        }

    }
}
