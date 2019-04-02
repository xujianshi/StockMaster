using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace XjsStock
{
    public class ConfigHelper
    {
        public static string Db => ConfigurationManager.AppSettings["sqlserver"];

    }
}
