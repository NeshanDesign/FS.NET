using System;

namespace FsNet.Common.DatetimeUtils
{
    public class DateTimeRange
    {
        private DateTime _startDate;
        private DateTime _endDate;

        private static DateTime BaseDateStart
        {
            get
            {
                var cur = DateTime.Now;
                return new DateTime(cur.Year, cur.Month, cur.Day, 0, 0, 0, 0);
            }
        }
        private static DateTime BaseDateEnd
        {
            get
            {
                var cur = DateTime.Now;
                return new DateTime(cur.Year, cur.Month, cur.Day, 23, 59, 59, 999);
            }
        }

        public DateTime StartDate {
            get => _startDate;
            set => _startDate = value;
        }
        public DateTime EndDate {
            get => _endDate;
            set => _endDate = value;
        }

        public DateTimeRange() : this(BaseDateStart, BaseDateEnd) { }
        public DateTimeRange(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public bool IsBetween(DateTimeRange dateRange)
        {
            return dateRange.IsInRange(StartDate) && dateRange.IsInRange(EndDate);
        }

        public bool IsInRange(DateTime dateTime)
        {
            return dateTime >= StartDate && dateTime <= EndDate;
        }

        public bool ColideWith(DateTimeRange range)
        {
            return range.IsInRange(StartDate) || range.IsInRange(EndDate) || IsInRange(range.StartDate) || IsInRange(range.EndDate);
        }
    }
}