namespace TestLibrary
{
    using System;

    using PropertyTools.DataAnnotations;

    public class TestFormatStringAttribute : TestBase
    {
        [FormatString("0.00")]
        public double Double { get; set; }

        [FormatString("000")]
        public int Integer { get; set; }

        [FormatString("yyyy-mm-dd")]
        public DateTime Date { get; set; }

        [FormatString("hh:MM")]
        public DateTime Time { get; set; }

        public TestFormatStringAttribute()
        {
            Double = Math.PI;
            Integer = 1;
            Date = Time = DateTime.Now;
        }

        public override string ToString()
        {
            return "FormatString attribute";
        }
    }
}