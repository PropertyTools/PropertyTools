namespace TestLibrary
{
    using System;

    using PropertyTools.DataAnnotations;

    public class TestSpinnableAttribute : TestBase
    {
        [Spinnable(10, 90, 0, 360)]
        [Width(100)]
        public double Angle { get; set; }

        [Spinnable(10, 90, 0, 360)]
        [FormatString("{0:0}°")]
        [Width(100)]
        public double AngleWithFormatString { get; set; }

        [Spinnable]
        [FormatString("{0:yyyy-MM-dd}")]
        [Width(100)]
        public DateTime Date { get; set; }

        [Spinnable(SmallChange = 1.0 / 24 / 2, LargeChange = 1.0 / 24)]
        [FormatString("{0:HH:mm:ss}")]
        [Width(100)]
        public DateTime Time { get { return Date; } set { Date = value; } }

        [Spinnable(1, 10, 0, int.MaxValue)]
        [Width(100)]
        public int Days { get; set; }

        public TestSpinnableAttribute()
        {
            Date = DateTime.Now;
        }

        public override string ToString()
        {
            return "Spinnable attribute";
        }
    }
}