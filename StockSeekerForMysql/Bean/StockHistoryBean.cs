using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSeekerForMysql.Bean
{
    public  class StockHistoryBean
    {

        /// <summary>
        /// 股票代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Rq { get; set; }

        /// <summary>
        /// 股票名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 收盘价
        /// </summary>
        public decimal ClosePrice { get; set; }
        
        /// <summary>
        /// 最高
        /// </summary>
        public decimal HighPrice { get; set; }

        /// <summary>
        /// 换手率
        /// </summary>
        public decimal HuanShouLv { get; set; }


        /// <summary>
        /// 最低
        /// </summary>
        public decimal LowPrice { get; set; }

        /// <summary>
        /// 开盘价
        /// </summary>
        public decimal OpenPrice { get; set; }
        
        /// <summary>
        /// 涨幅
        /// </summary>
        public decimal ZhangFu { get; set; }

        /// <summary>
        /// 振幅
        /// </summary>
        public decimal ZhenFu { get; set; }

        /// <summary>
        /// 成交量
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 成绩金额
        /// </summary>
        public decimal Volume { get; set; }
    }
}
