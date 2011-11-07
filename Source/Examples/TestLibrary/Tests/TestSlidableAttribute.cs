namespace TestLibrary
{
    using PropertyTools.DataAnnotations;

    public class TestSlidableAttribute : TestBase
    {
        [Slidable(0, 360, 45, 1)]
        [FormatString("0.00")]
        public double Angle { get; set; }

        [Slidable(0, 360, 5, 1, true, 45)]
        [FormatString("{0:0}°")]
        public double AngleWithSnapping { get; set; }

        [Slidable(0, 365)]
        public int Days { get; set; }

        public override string ToString()
        {
            return "Slidable attribute";
        }
    }
}