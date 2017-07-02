namespace FsNet.Common.DatetimeUtils
{
    public class TimeRange
    {
        public TimeRange() : this(new Time(23, 59, 59), new Time(0, 0, 0)) { }
        public TimeRange(Time startTime, Time endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public Time StartTime { get; set; }
        public Time EndTime { get; set; }

        public string StartTimeStr => $"{StartTime.Hour:D2}:{StartTime.Minute:D2}:{StartTime.Second:D2}";
        public string EndTimeStr => $"{EndTime.Hour:D2}:{EndTime.Minute:D2}:{EndTime.Second:D2}";

        public bool IsBetween(TimeRange timeRange)
        {
            return timeRange.IsInRange(StartTime) && timeRange.IsInRange(EndTime);
        }

        public bool IsInRange(Time time)
        {
            if (EndTime >= StartTime) return time >= StartTime && time <= EndTime;
            return (time >= StartTime && time >= EndTime);
        }

        public bool ColideWith(TimeRange range)
        {
            return range.IsInRange(StartTime) || range.IsInRange(EndTime) || IsInRange(range.StartTime) || IsInRange(range.EndTime);
        }
    }
}