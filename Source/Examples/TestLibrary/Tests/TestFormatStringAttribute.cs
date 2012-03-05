namespace TestLibrary
{
    using System;
    using System.ComponentModel;

    using PropertyTools.DataAnnotations;

    public class TestFormatStringAttribute : TestBase
    {
        [Category("Double")]
        [FormatString("0.00")]
        public double Double { get; set; }

        [Category("Int")]
        [FormatString("000")]
        public int Integer { get; set; }

        [Category("TimeSpan")]
        [FormatString("hh:mm")]
        [Description("hh:mm")]
        public TimeSpan TimeSpan1 { get; set; }

        [FormatString("mm:ss")]
        [Description("mm:ss")]
        public TimeSpan TimeSpan2 { get; set; }

        [Category("DateTime")]
        [FormatString("yyyy-MM-dd hh:mm")]
        [Description("yyyy-MM-dd hh:mm")]
        public DateTime DateTime1 { get; set; }

        [FormatString("MM/dd/yyyy hh.mm.ss")]
        [Description("MM/dd/yyyy hh.mm.ss")]
        public DateTime DateTime2 { get; set; }

        [FormatString("yyyy-MM-dd")]
        public DateTime Date { get; set; }

        [FormatString("hh:MM")]
        public DateTime Time { get; set; }

        public TestFormatStringAttribute()
        {
            Double = Math.PI;
            Integer = 1;

            TimeSpan1 = new TimeSpan(0, 12, 39, 0);
            TimeSpan2 = new TimeSpan(0, 0, 12, 39);
            Date = Time = DateTime1 = DateTime2 = new DateTime(2012, 3, 5, 21, 34, 14);
        }

        public override string ToString()
        {
            return "FormatString attribute";
        }
    }
}