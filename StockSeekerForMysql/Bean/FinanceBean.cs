using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSeekerForMysql.Bean
{
   public  class FinanceBean
    {
        public  string date { get; set; }

        public string code { get; set; }

        /// <summary>
        /// 每股收益
        /// </summary>
        public double mgsy { get; set; }

        /// <summary>
        /// 每股净资产
        /// </summary>
        public double mgjzc { get; set; }

        /// <summary>
        /// 每股经营现金流净额
        /// </summary>
        public double mgjyxjlje { get; set; }

        /// <summary>
        /// 净资产收益率
        /// </summary>
        public double jzcsyl { get; set; }

        /// <summary>
        /// 每股未分配利润
        /// </summary>
        public double mgwfplr { get; set; }

        /// <summary>
        /// 每股公积金
        /// </summary>
        public double mggjj { get; set; }

    }
}
