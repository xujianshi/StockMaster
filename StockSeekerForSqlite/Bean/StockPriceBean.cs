using System;

namespace XjsStock.Bean
{
    public class StockPriceBean : BaseBean
    {
        /// <summary>
        /// 收盘价
        /// </summary>
        public decimal ClosePrice { get; set; }

        /// <summary>
        /// 股票代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 最高
        /// </summary>
        public decimal HighPrice { get; set; }

        /// <summary>
        /// 换手率
        /// </summary>
        public decimal HuanShou { get; set; }

        /// <summary>
        /// 最低
        /// </summary>
        public decimal LowPrice { get; set; }

        /// <summary>
        /// 开盘价
        /// </summary>
        public decimal OpenPrice { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Rq { get; set; }

        /// <summary>
        /// 涨幅
        /// </summary>
        public decimal ZhangFu { get; set; }

        public decimal Amount { get; set; }

        public decimal Volume { get; set; }
    }
}