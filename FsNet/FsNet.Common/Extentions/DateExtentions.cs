using System;
using System.Globalization;
using FsNet.Common.DatetimeUtils;

namespace FsNet.Common.Extentions
{
    public static class DateExtentions
    {
        public static long TotalMiliSeconds(this DateTime date)
        {
            return date.TotalSeconds() * 1000 + date.Millisecond;
        }
        public static long TotalSeconds(this DateTime date)
        {
            return date.Second + date.Minute * 60 + date.Hour * 3600L + 24L * 3600 * (365 - date.DayOfYear) + (date.Year - 1) * 24 * 3600 * 365L;
        }

        public static long TotalMinutes(this DateTime date)
        {
            return date.TotalSeconds() / 60;
        }

        public static string GetPersianDatetime(this DateTime date, string dateSplitor = "-")
        {
            var pc = new PersianCalendar();
            return $@"{pc.GetYear(date):D4}{dateSplitor}{pc.GetMonth(date):D2}{dateSplitor}{pc.GetDayOfMonth(date):D2} ~ {date:HH:mm:ss}";
        }

        public static string GetPersianDate(this DateTime date, string splitor = "")
        {
            var pc = new PersianCalendar();
            return $@"{pc.GetYear(date):D4}{splitor}{pc.GetMonth(date):D2}{splitor}{pc.GetDayOfMonth(date):D2}";
        }

        public static string GetPerisanDatetime(this DateTime date, string dateTimeSplitor = "~", string dateSplitor = "-", string timeSplitor = ":")
        {
            return $"{date.GetPersianDate(dateSplitor)}{dateTimeSplitor}{date.ToString(string.Format("HH{0}mm{0}ss", timeSplitor))}";
        }

        public static string GetPerisanDatetimeRtl(this DateTime date, string dateTimeSplitor = "~", string dateSplitor = "-", string timeSplitor = ":")
        {
            return $"{date.ToString($"HH{timeSplitor}mm{timeSplitor}ss")}{dateTimeSplitor}{date.GetPersianDate(dateSplitor)}";
        }

        public static double GetDiffInMiliSecond(this DateTime thisDate, DateTime other)
        {
            return thisDate.Subtract(other).TotalMilliseconds;
        }

        public static  DateTime? GetDateTimeFromPersianDateStr(this string dateTime, string dateSplitor = "/", string timeSplitor = ":")
        {
            try
            {
                if (string.IsNullOrEmpty(dateTime)) return null;
                var dateparts = dateTime.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                var dateArr = dateparts[0].Split(new[] { dateSplitor }, StringSplitOptions.RemoveEmptyEntries);
                var timeArr = dateparts[1].Split(new[] { timeSplitor }, StringSplitOptions.RemoveEmptyEntries);
                var pc = new PersianCalendar();
                var result = pc.ToDateTime(int.Parse(dateArr[0]), int.Parse(dateArr[1]), int.Parse(dateArr[2]), int.Parse(timeArr[0]),
                    int.Parse(timeArr[1]), 0, 0);
                return result;
            }
            catch// (Exception e)
            {
                return null;
            }
        }


        public static string GetPersianFormatedDate(this DateTime date)
        {
            return $"{date.GetDayName()} {date.GetDayOfMonth()} {date.GetMounthName()} {GetYear(date)}";
        }

        public static string GetPersianFormatedDateTime(this DateTime date)
        {
            return $"{date.GetDayName()} {date.GetDayOfMonth()} {date.GetMounthName()} {GetYear(date)} {Properties.Resources.Hour} {date}:HH:mm:ss";
        }


        static string GetDayName(this DateTime date)
        {
            switch (date.DayOfWeek)
            {
                    case DayOfWeek.Friday:    return Properties.Resources.Friday;
                    case DayOfWeek.Saturday:  return Properties.Resources.Saturday;
                    case DayOfWeek.Sunday:    return Properties.Resources.Sunday;
                    case DayOfWeek.Monday:    return Properties.Resources.Monday;
                    case DayOfWeek.Tuesday:   return Properties.Resources.Tuesday;
                    case DayOfWeek.Wednesday: return Properties.Resources.Wendsday;
                    case DayOfWeek.Thursday:  return Properties.Resources.Thursday;
            }
            return null;
        }


        private static string GetMounthName(this DateTime date)
        {
            return ((PersianMounth)new PersianCalendar().GetMonth(date)).ToPersianString();
        }

        private static int GetDayOfMonth(this DateTime date)
        {
            return new PersianCalendar().GetDayOfMonth(date);
        }

        private static int GetYear(this DateTime date)
        {
            return new PersianCalendar().GetYear(date);
        }
    }
}