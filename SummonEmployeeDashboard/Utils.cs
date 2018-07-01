using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonEmployeeDashboard
{
    class Utils
    {
        private const string format = "yyyy-MM-dd'T'HH:mm:ss.fff'Z'";
        public static string GetStringTime(DateTime datetime)
        {
            return datetime.ToUniversalTime().ToString(format);
        }
        public static DateTime GetDateTime(string datetime)
        {
            return DateTime.Parse(datetime);
        }
    }
}
