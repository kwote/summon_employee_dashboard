using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonEmployeeDashboard
{
    static class Utils
    {
        private const string format = "yyyy-MM-dd'T'HH:mm:ss.fff'Z'";
        public static string GetStringTime(this DateTime datetime)
        {
            return datetime.ToUniversalTime().ToString(format);
        }
        public static DateTime GetDateTime(this string datetime)
        {
            return DateTime.Parse(datetime);
        }
        
        /// <summary>
         /// Compares a supplied date to the current date and generates a friendly English 
         /// comparison ("5 days ago", "5 days from now")
         /// </summary>
         /// <param name="date">The date to convert</param>
         /// <param name="approximate">When off, calculate timespan down to the second.
         /// When on, approximate to the largest round unit of time.</param>
         /// <returns></returns>
        public static string ToRelativeDateString(long milliseconds, bool approximate)
        {
            string prefix = (milliseconds > 0) ? "" : "через ";
            StringBuilder sb = new StringBuilder(prefix);

            string suffix = (milliseconds < 0) ? "" : " назад";

            long ticks = milliseconds * 10000;
            TimeSpan timeSpan = new TimeSpan(Math.Abs(ticks));
            int days = timeSpan.Days;
            if (days > 7)
            {
                return DateTime.Now.AddTicks(-ticks).ToShortDateString();
            }

            if (days > 0)
            {
                sb.AppendFormat("{0} {1}", days,
                  days == 1 ? "день" : days < 5 ? "дня" : "дней");
                if (approximate) return sb.ToString() + suffix;
            }
            int hours = timeSpan.Hours;
            if (hours > 0)
            {
                int remainder = hours % 10;
                sb.AppendFormat("{0}{1} {2}", (sb.Length > 0) ? ", " : string.Empty,
                  hours, remainder == 1 ? "час" : remainder > 4 || remainder == 0 || (hours > 10 && hours < 20) ? "часов" : "часа");
                if (approximate) return sb.ToString() + suffix;
            }
            int minutes = timeSpan.Minutes;
            if (minutes > 0)
            {
                int remainder = minutes % 10;
                sb.AppendFormat("{0}{1} {2}", (sb.Length > 0) ? ", " : string.Empty,
                  minutes, remainder == 1 ? "минуту" : remainder > 4 || remainder == 0 || (minutes > 10 && minutes < 20) ? "минут" : "минуты");
                if (approximate) return sb.ToString() + suffix;
            }
            int seconds = timeSpan.Seconds;
            if (seconds > 0)
            {
                int remainder = seconds % 10;
                sb.AppendFormat("{0}{1} {2}", (sb.Length > 0) ? ", " : string.Empty,
                  seconds, remainder == 1 ? "секунду" : remainder > 4 || remainder == 0 || (seconds > 10 && seconds < 20) ? "секунд" : "секунды");
                if (approximate) return sb.ToString() + suffix;
            }
            if (sb.Length == 0) return "сейчас";

            sb.Append(suffix);
            return sb.ToString();
        }
    }
}
