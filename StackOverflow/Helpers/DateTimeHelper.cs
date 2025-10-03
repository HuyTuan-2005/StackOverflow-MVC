using System;

namespace StackOverflow.Helpers
{
    public class DateTimeHelper
    {
        public static string ConvertDateToString(DateTime date)
        {
            TimeSpan ts = DateTime.Now - date;
    
            if (ts.TotalMilliseconds < 1000)
            {
                return "Vừa mới";
            }
            else if (ts.TotalSeconds < 60)
            {
                return (int)ts.TotalSeconds + " giây trước";
            }
            else if (ts.TotalMinutes < 60)
            {
                return (int)ts.TotalMinutes + " phút trước";
            }
            else if (ts.TotalHours < 24) 
            {
                return (int)ts.TotalHours + " giờ trước";
            }
            else if (ts.TotalDays < 7)
            {
                return (int)ts.TotalDays + " ngày trước";
            }
            else if (ts.TotalDays < 30)
            {
                return (int)(ts.TotalDays / 7) + " tuần trước";
            }
            else if (ts.TotalDays < 365)
            {
                // Tính tháng chính xác dựa trên DateTime
                int months = ((DateTime.Now.Year - date.Year) * 12) + DateTime.Now.Month - date.Month;
                if (DateTime.Now.Day < date.Day)
                    months--;
                return months + " tháng trước";
            }
            else
            {
                // Tính năm chính xác
                int years = DateTime.Now.Year - date.Year;
                if (DateTime.Now.Month < date.Month || 
                    (DateTime.Now.Month == date.Month && DateTime.Now.Day < date.Day))
                    years--;
                return years + " năm trước";
            }
        }
    }
}