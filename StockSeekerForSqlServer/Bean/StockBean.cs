using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XjsStock.Bean
{
    public class StockBean:BaseBean
    {
        /// <summary>
        /// CreateDay
        /// </summary>
        public DateTime CreateDay { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
    }
}
