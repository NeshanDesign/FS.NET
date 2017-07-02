using System.ComponentModel;
using System.Globalization;

namespace FsNet.Common.DatetimeUtils
{
    public class Time
    {
        public Time() { }


        public Time(int hour, int minute)
            : this(hour, minute, 0, 0)
        {

        }
        public Time(int hour, int minute, int second)
            : this(hour, minute, second, 0)
        {

        }

        public Time(int hour, int minute, int second, int miliSeconds)
        {
            Hour = hour;
            Minute = minute;
            Second = second;
            Milisecond = miliSeconds;
        }

        [DefaultValue(-1)]
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
        public int Milisecond { get; set; }

        public string ToLongString()
        {
            return $"{Hour:D2}:{Minute:D2}:{Second:D2}.{Milisecond.ToString(CultureInfo.InvariantCulture).PadLeft(3, '0')}";
        }
        public override string ToString()
        {
            return $"{Hour:D2}:{Minute:D2}:{Second:D2}";
        }

        public string ToShortString()
        {
            return $"{Minute:D2}:{Second:D2}";
        }

        public static bool operator >(Time _this, Time that)
        {
            return _this.Hour > that.Hour || (_this.Hour == that.Hour && _this.Minute > that.Minute) ||
                   (_this.Hour == that.Hour && _this.Minute == that.Minute && _this.Second > that.Second) ||
                   (_this.Hour == that.Hour && _this.Minute == that.Minute && _this.Second == that.Second && _this.Milisecond > that.Milisecond);
        }

        public static bool operator <(Time _this, Time that)
        {
            return _this.Hour < that.Hour || (_this.Hour == that.Hour && _this.Minute < that.Minute) ||
                   (_this.Hour == that.Hour && _this.Minute == that.Minute && _this.Second < that.Second) ||
                   (_this.Hour == that.Hour && _this.Minute == that.Minute && _this.Second == that.Second && _this.Milisecond < that.Milisecond);
        }

        public static bool operator ==(Time _this, Time that)
        {
            if (null == (object)_this && (object)that == null) return true;
            if (null == (object)_this || (null == (object)that)) return false;

            return _this.Hour == that.Hour && _this.Minute == that.Minute && _this.Second == that.Second && _this.Milisecond == that.Milisecond;
        }

        public static bool operator !=(Time _this, Time that)
        {
            return !(_this == that);
        }

        public static bool operator >=(Time _this, Time that)
        {
            return (_this == that || _this > that);
        }

        public static bool operator <=(Time _this, Time that)
        {
            return (_this == that || _this < that);
        }

        protected bool Equals(Time other)
        {
            return Hour == other.Hour && Minute == other.Minute && Second == other.Second && Milisecond == other.Milisecond;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof(Time) && Equals((Time)obj);
        }

        public override int GetHashCode()
        {
            //FNV hash algothm
            unchecked
            {
                const int prime = 23497;//big enough prime number
                var hashCode = Hour;
                hashCode = (hashCode * prime) ^ Minute;
                hashCode = (hashCode * prime) ^ Second;
                hashCode = (hashCode * prime) ^ Milisecond;
                return hashCode;
            }
        }

        public bool IsValid()
        {
            return Hour >= 0 && Hour < 24 && Minute >= 0 && Minute < 60 && Second >= 0 && Second < 60 && Milisecond >= 0 && Milisecond < 1000;
        }
    }
}