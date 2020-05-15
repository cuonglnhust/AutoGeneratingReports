using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGeneratingReports.Common
{
    public static class ConvertTimeToString
    {
        public static string ConvertToString(DateTime dateTime)
        {
            var TimeToString = dateTime.ToString("HH:mm:ss");
            return TimeToString;
        }
        public static DateTime ConvertToTime(string Time)
        {
            DateTime stringToTime = DateTime.ParseExact(Time, "HH:mm:ss",CultureInfo.InvariantCulture);
            return stringToTime;
        }
    }
}
