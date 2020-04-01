using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGeneratingReports.Common
{
   public static class DataSource
    {
        public static string ConnectionString()
        {
            var con = "Data Source=DESKTOP-1N7VK9G;Initial Catalog=SafenetLocal;User ID=sa;Password=123456";
            return con;
        }
    }
}
