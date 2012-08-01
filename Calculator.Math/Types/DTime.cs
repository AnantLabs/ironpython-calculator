using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginApi;

namespace Calculator.Mathematics.Types
{
    [Serializable]
    [ReflectionLoad]
    public class DTime: ICloneable
    {
        public DTime()
        {
            Month = 0;
            Year = 0;
            Day = 0;
            Hour = 0;
            Minute = 0;
            Second = 0;
            MiliSeconds = 0;
        }

        public DTime(short Year, sbyte Month, sbyte Day): this()
        {
            this.Year = Year;
            this.Month = Month;
            this.Day = Day;
        }

        public DTime(sbyte Hour, sbyte Minute, sbyte Second, short Milisec = 0): this()
        {
            this.Hour = Hour;
            this.Minute = Minute;
            this.Second = Second;
            this.MiliSeconds = Milisec;
        }

        public DTime(short Year, sbyte Month, sbyte Day, sbyte Hour, sbyte Minute, sbyte Second, short Milisec = 0): this(Year, Month, Day)
        {
            this.Hour = Hour;
            this.Minute = Minute;
            this.Second = Second;
            this.MiliSeconds = Milisec;
        }

        public DTime(long ticks) : this()
        {
            this.DNetDateTime = new DateTime(ticks);
        }

        public DTime(string datetime): this()
        {
            DateTime parsed;
            TimeSpan ts;
            if (datetime.ToLower() == "now") this.DNetDateTime = DateTime.Now;
            else if (DateTime.TryParse(datetime, out parsed)) this.DNetDateTime = parsed;
            else if (TimeSpan.TryParse(datetime, out ts)) this.DNetDateTime.AddTicks(ts.Ticks);
            else throw new Exception("Can't parse date");
        }

        public DateTime DNetDateTime
        {
            get { return new DateTime(Year, Month, Day, Hour, Minute, Second, MiliSeconds); }
            set
            {
                Year = (short)value.Year;
                Month = (sbyte)value.Month;
                Day = (sbyte)value.Day;
                Hour = (sbyte)value.Hour;
                Minute = (sbyte)value.Minute;
                Second = (sbyte)value.Second;
                MiliSeconds = (short)value.Millisecond;
            }
        }

        public sbyte Month { get; set; }
        public short Year { get; set; }
        public sbyte Day { get; set; }
        public sbyte Hour { get; set; }
        public sbyte Minute { get; set; }
        public sbyte Second { get; set; }
        public short MiliSeconds { get; set; }

        public double TotalDays
        {
            get
            {
                TimeSpan ts = new TimeSpan(DNetDateTime.Ticks);
                return ts.TotalDays;
            }
        }

        public double TotalHours
        {
            get
            {
                TimeSpan ts = new TimeSpan(DNetDateTime.Ticks);
                return ts.TotalHours;
            }
        }

        public double TotalMinutes
        {
            get
            {
                TimeSpan ts = new TimeSpan(DNetDateTime.Ticks);
                return ts.TotalMinutes;
            }
        }

        public double TotalSeconds
        {
            get
            {
                TimeSpan ts = new TimeSpan(DNetDateTime.Ticks);
                return ts.TotalSeconds;
            }
        }

        public double TotalMiliseconds
        {
            get
            {
                TimeSpan ts = new TimeSpan(DNetDateTime.Ticks);
                return ts.TotalMilliseconds;
            }
        }

        public byte WeekOfYear
        {
            get { return (byte)Math.Round((double)DNetDateTime.DayOfYear / 7, 0); }
        }

        public short DayOfYear
        {
            get { return (short)DNetDateTime.DayOfYear; }
        }

        public byte DayOfWeek
        {
            get { return (byte)DNetDateTime.DayOfWeek; }
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}-{2} {3}:{4}:{5}", Year, Month, Day, Hour, Minute, Second);
        }

        public string ToString(string format)
        {
            string ret = format.Replace("%Y", Year.ToString());
            ret = ret.Replace("%M", Month.ToString());
            ret = ret.Replace("%D", Day.ToString());
            ret = ret.Replace("%h", Hour.ToString());
            ret = ret.Replace("%m", Minute.ToString());
            ret = ret.Replace("%d", Day.ToString());
            return ret;
        }

        public object Clone()
        {
            return new DTime(this.DNetDateTime.Ticks);
        }
    }
}
