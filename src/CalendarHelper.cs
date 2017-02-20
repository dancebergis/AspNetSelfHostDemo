using System;
using System.Globalization;

namespace AspNetSelfHostDemo
{
    public class CalendarHelper
    {
        public static int GetWeekOfYear(DateTime date)
        {
            return DateTimeFormatInfo.InvariantInfo.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay,
                DayOfWeek.Monday);
        }
    }
}
