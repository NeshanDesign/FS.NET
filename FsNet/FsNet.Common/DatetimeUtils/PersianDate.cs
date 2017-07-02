using System;
using System.Globalization;

namespace FsNet.Common.DatetimeUtils
{
    public class PersianDate
    {
        public int Year { get; set; }
        public int Mounth { get; set; }
        public int DayOfMounth { get; set; }
        public int DayOfWeek { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }

        public override string ToString()
        {
            return $"{Year:D4}/{Mounth:D2}/{DayOfMounth:D2}~{new Time() {Hour = this.Hour, Minute = this.Minute, Second = this.Second}}";
        }

        public string ToRtlString()
        {
            return $"{new Time() { Hour = this.Hour, Minute = this.Minute, Second = this.Second }}~{Year:D4}/{Mounth:D2}/{DayOfMounth:D2}";
        }

        public string ToDateString()
        {
            return $"{Year:D4}/{Mounth:D2}/{DayOfMounth:D2}";
        }

        public DateTime ToDateTime()
        {
            var pc = new PersianCalendar();
            return pc.ToDateTime(Year, Mounth, DayOfMounth, Hour, Minute, Second, 0);
        }

    }
}
