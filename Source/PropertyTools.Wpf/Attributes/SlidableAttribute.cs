using System;
using System.Windows.Controls.Primitives;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// The SlidableAttribute is used for numeric properties.
    /// Properties marked with [Slidable] will have a slider next to its editor.
    /// Example usage:
    ///   [Slidable(0,100)]
    ///   [Slidable(0,100,1,10)]
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class SlidableAttribute : Attribute
    {
        public static readonly SlidableAttribute Default = new SlidableAttribute();

        public SlidableAttribute()
        {
            Minimum = 0;
            Maximum = 100;
            SmallChange = 1;
            LargeChange = 10;
            SnapToTicks = false;
            TickFrequency = 1;
            TickPlacement = TickPlacement.None;
        }

        public SlidableAttribute(double minimum, double maximum)
            : this(minimum, maximum, 1, 10)
        {

        }
        public SlidableAttribute(double minimum, double maximum, double smallChange, double largeChange)
        {
            Minimum = minimum;
            Maximum = maximum;
            SmallChange = smallChange;
            LargeChange = largeChange;
        }

        public SlidableAttribute(double minimum, double maximum, double smallChange, double largeChange, bool snapToTicks, double tickFrequency)
            : this(minimum, maximum, smallChange, largeChange)
        {
            SnapToTicks = snapToTicks;
            TickFrequency = tickFrequency;
        }

        public SlidableAttribute(double minimum, double maximum, double smallChange, double largeChange, bool snapToTicks, double tickFrequency, TickPlacement tickPlacement)
            : this(minimum, maximum, smallChange, largeChange, snapToTicks, tickFrequency)
        {
            TickPlacement = tickPlacement;
        }



        public double Minimum { get; set; }
        public double Maximum { get; set; }
        public double LargeChange { get; set; }
        public double SmallChange { get; set; }
        public bool SnapToTicks { get; set; }
        public double TickFrequency { get; set; }
        public TickPlacement TickPlacement { get; set; }

        public override bool Equals(object obj)
        {
            var o = obj as SlidableAttribute;

            return o == null ? false : Minimum.Equals(o.Minimum) && Maximum.Equals(o.Maximum);
        }

        public override int GetHashCode()
        {
            return Minimum.GetHashCode() ^ Maximum.GetHashCode();
        }

        public override bool IsDefaultAttribute()
        {
            return Equals(Default);
        }

    }
}
