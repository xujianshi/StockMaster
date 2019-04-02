using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSeeker
{
    public  class SlConvert
    {
        public static decimal TryToDecimal(object value, decimal defalut=0.0m)
        {
            try
            {
                return decimal.Parse(value.ToString());
            }
            catch (Exception e)
            {
                return defalut;
            }
        }


        public static string TryToString(object value)
        {
            return value + "";
        }

        public static DateTime TryToDateTime(object value, DateTime defalut)
        {
            try
            {
                return DateTime.Parse(value.ToString());
            }
            catch (Exception e)
            {
                return defalut;
            }
        }
    }
}
